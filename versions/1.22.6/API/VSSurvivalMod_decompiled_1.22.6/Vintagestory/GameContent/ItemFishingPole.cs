using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class ItemFishingPole : Item, IContainedInteractable, IHandBookPageCodeProvider
{
	protected ClothManager cm;

	protected string aimAnimation;

	protected Vec3f offsetToPoleTipFp;

	protected Vec3f offsetToPoleTipTp;

	protected CompositeShape? ropelessShapeLoc;

	private EnumCameraMode? previousCm;

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		cm = api.ModLoader.GetModSystem<ClothManager>();
		aimAnimation = Attributes["aimAnimation"].AsString("bowaimlong");
		offsetToPoleTipFp = Attributes["offsetToPoleTipFp"].AsObject(new Vec3f());
		offsetToPoleTipFp = new Vec3f(3.3f, -0.05f, 0.4f);
		offsetToPoleTipTp = new Vec3f(2.3f, 0f, 0f);
		ropelessShapeLoc = Attributes["ropelessShape"].AsObject<CompositeShape>();
		ObjectCacheUtil.GetOrCreate(api, "fishingbaititems", () => api.World.Collectibles.Where((CollectibleObject obj) => obj.Attributes?.IsTrue("isFishBait") ?? false).SelectMany((CollectibleObject obj) => obj.GetHandBookStacks(api as ICoreClientAPI) ?? new List<ItemStack>()).ToArray());
	}

	public override int GetMergableQuantity(ItemStack sinkStack, ItemStack sourceStack, EnumMergePriority priority)
	{
		if (sinkStack.Attributes.GetBool("fishing"))
		{
			return 0;
		}
		if (priority == EnumMergePriority.DirectMerge)
		{
			JsonObject itemAttributes = sourceStack.ItemAttributes;
			if (itemAttributes != null && itemAttributes.IsTrue("isFishBait"))
			{
				return 1;
			}
		}
		return base.GetMergableQuantity(sinkStack, sourceStack, priority);
	}

	public override void TryMergeStacks(ItemStackMergeOperation op)
	{
		ItemStack? itemstack = op.SinkSlot.Itemstack;
		if (itemstack != null && itemstack.Attributes.GetBool("fishing"))
		{
			return;
		}
		if (op.CurrentPriority == EnumMergePriority.DirectMerge)
		{
			ItemStack? itemstack2 = op.SourceSlot.Itemstack;
			if (itemstack2 != null && itemstack2.ItemAttributes?.IsTrue("isFishBait") == true)
			{
				ItemStack oldBaitStack = op.SinkSlot.Itemstack?.Attributes.GetItemstack("fishingBait");
				op.SinkSlot.Itemstack?.Attributes.SetItemstack("fishingBait", op.SourceSlot.TakeOut(1));
				op.MovedQuantity = 1;
				if (oldBaitStack != null)
				{
					if (op.SourceSlot.StackSize <= 1)
					{
						op.SourceSlot.Itemstack = oldBaitStack;
						op.SourceSlot.MarkDirty();
					}
					else if (!op.ActingPlayer.InventoryManager.TryGiveItemstack(oldBaitStack))
					{
						op.ActingPlayer.Entity.World.SpawnItemEntity(oldBaitStack, op.ActingPlayer.Entity.Pos.XYZ);
					}
				}
				return;
			}
		}
		base.TryMergeStacks(op);
	}

	public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
	{
		if (target == EnumItemRenderTarget.HandTp && itemstack.Attributes.GetBool("fishing") && ropelessShapeLoc != null)
		{
			MultiTextureMeshRef meshdata = ObjectCacheUtil.GetOrCreate(capi, "fishingPoleRopelessShape" + Code, delegate
			{
				Shape shape = capi.Assets.Get(ropelessShapeLoc.Base.WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json")).ToObject<Shape>();
				capi.Tesselator.TesselateShape(this, shape, out var modeldata);
				return capi.Render.UploadMultiTextureMesh(modeldata);
			});
			renderinfo.ModelRef = meshdata;
		}
		base.OnBeforeRender(capi, itemstack, target, ref renderinfo);
	}

	public override void OnCollected(ItemStack stack, Entity entity)
	{
		if (entity is EntityAgent agent)
		{
			StopFishing(stack, agent);
		}
	}

	public override void OnHeldDropped(IWorldAccessor world, IPlayer byPlayer, ItemSlot slot, int quantity, ref EnumHandling handling)
	{
		StopFishing(slot.Itemstack, byPlayer.Entity);
	}

	public override void OnHeldIdle(ItemSlot slot, EntityAgent byEntity)
	{
		updatePinOffset(slot, byEntity);
	}

	private void updatePinOffset(ItemSlot slot, EntityAgent byEntity)
	{
		if (!(api is ICoreClientAPI capi))
		{
			return;
		}
		EnumCameraMode camMode = capi.World.Player.CameraMode;
		if (camMode != previousCm && previousCm == EnumCameraMode.FirstPerson && camMode != EnumCameraMode.FirstPerson)
		{
			ClothSystem cs = getRope(slot);
			if (cs != null)
			{
				cs.FirstPoint.pinnedToOffset = offsetToPoleTipTp.Clone();
				cs.FirstPoint.NoAttachTransform = false;
			}
		}
		if (camMode == EnumCameraMode.FirstPerson)
		{
			ClothSystem cs2 = getRope(slot);
			if (cs2 != null)
			{
				Matrixf matrixf = new Matrixf();
				LoadHeldItemModelMatrix(matrixf, byEntity, slot, capi);
				Vec4f pos = matrixf.TransformVector(new Vec4f(offsetToPoleTipFp.X, offsetToPoleTipFp.Y, offsetToPoleTipFp.Z, 1f));
				IRenderAPI rapi = capi.Render;
				float[] pMatrixHandFov = (capi.World.Player.Entity.Properties.Client.Renderer as EntityShapeRenderer)?.pMatrixHandFov;
				float[] pMatrixNormalFov = (capi.World.Player.Entity.Properties.Client.Renderer as EntityShapeRenderer)?.pMatrixNormalFov;
				if (pMatrixHandFov != null)
				{
					Matrixf tmpMatrix = new Matrixf();
					Vec4f screenSpaceCoordNormalFov = tmpMatrix.Set(pMatrixHandFov).Mul(rapi.CameraMatrixOriginf).TransformVector(pos);
					pos = new Matrixf(rapi.CameraMatrixOriginf).Invert().Mul(tmpMatrix.Set(pMatrixNormalFov).Invert()).TransformVector(screenSpaceCoordNormalFov);
				}
				cs2.FirstPoint.pinnedToOffset = pos.XYZ;
				cs2.FirstPoint.NoAttachTransform = true;
			}
		}
		previousCm = camMode;
	}

	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection? entitySel, bool firstEvent, ref EnumHandHandling handling)
	{
		base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
		if (handling == EnumHandHandling.PreventDefault)
		{
			return;
		}
		if (StopFishing(slot.Itemstack, byEntity))
		{
			slot.MarkDirty();
			handling = EnumHandHandling.PreventDefault;
		}
		else if (!(byEntity.MountedOn?.Controls ?? byEntity.Controls).CtrlKey || (entitySel?.SelectionBoxIndex ?? (-1)) < 0 || entitySel?.Entity?.GetBehavior<EntityBehaviorAttachable>() == null)
		{
			byEntity.AnimManager.StartAnimation(aimAnimation);
			slot.Itemstack?.Attributes.SetLong("fishingEntityId", byEntity.EntityId);
			byEntity.AnimManager.RegisterFrameCallback(new AnimFrameCallback
			{
				Animation = aimAnimation,
				Frame = 40f,
				Callback = delegate
				{
					throwBobber(slot, byEntity);
				}
			});
			handling = EnumHandHandling.PreventDefault;
		}
	}

	private void throwBobber(ItemSlot slot, EntityAgent byEntity)
	{
		byEntity.AnimManager.StopAnimation(aimAnimation);
		ItemStack? itemstack = byEntity.ActiveHandItemSlot.Itemstack;
		bool isFishing = itemstack == null || itemstack.Attributes.GetLong("fishingEntityId", 0L) != 0;
		if (slot.Empty || !isFishing)
		{
			return;
		}
		byEntity.AnimManager.StartAnimation("fishingpole-idle");
		if (byEntity.World.Side == EnumAppSide.Client)
		{
			slot.Itemstack.Attributes.SetBool("fishing", value: true);
			slot.MarkDirty();
			return;
		}
		long prevEntityId = slot.Itemstack.Attributes.GetLong("bobberEntityId", 0L);
		if (prevEntityId != 0L && api.World.GetEntityById(prevEntityId) is EntityBobber)
		{
			slot.Itemstack.Attributes.SetBool("fishing", value: true);
			slot.MarkDirty();
			return;
		}
		byEntity.World.PlaySoundAt(new AssetLocation("sounds/effect/poleswing"), byEntity, null, randomizePitch: false, 8f);
		EntityProperties type = byEntity.World.GetEntityType(new AssetLocation("bobber-normal"));
		EntityBobber ebobber = (EntityBobber)byEntity.World.ClassRegistry.CreateEntity(type);
		ebobber.FiredBy = byEntity;
		ebobber.DropOnImpactChance = 0f;
		Vec3d pos = byEntity.Pos.XYZ.Add(0.0, byEntity.LocalEyePos.Y, 0.0);
		Vec3d velocity = (pos.AheadCopy(1.0, byEntity.Pos.Pitch, byEntity.Pos.Yaw) - pos) * byEntity.Stats.GetBlended("bowDrawingStrength") / 6f;
		velocity.Y += 0.125;
		ebobber.Pos.SetPosWithDimension(byEntity.Pos.BehindCopy(0.21).XYZ.Add(0.0, byEntity.LocalEyePos.Y, 0.0));
		ebobber.Pos.Motion.Set(velocity);
		ebobber.World = byEntity.World;
		ebobber.AttachedToEntityId = byEntity.EntityId;
		ebobber.BaitStack = slot.Itemstack.Attributes.GetItemstack("fishingBait");
		ebobber.BaitStack?.ResolveBlockOrItem(byEntity.World);
		slot.Itemstack.Attributes.RemoveAttribute("fishingBait");
		ebobber.PreInitialize();
		byEntity.World.SpawnPriorityEntity(ebobber);
		ClothSystem cs = getRope(slot);
		if (cs != null)
		{
			cm.UnregisterCloth(cs.ClothId);
		}
		createRope(slot, byEntity, ebobber, ebobber.Pos.XYZ);
		slot.Itemstack.Attributes.SetLong("bobberEntityId", ebobber.EntityId);
		slot.Itemstack.Attributes.SetBool("fishing", value: true);
	}

	public bool StopFishing(ItemStack? itemstack, EntityAgent byEntity)
	{
		byEntity.AnimManager.StopAnimation("fishingpole-idle");
		if (itemstack == null)
		{
			return false;
		}
		itemstack.Attributes.SetBool("fishing", value: false);
		long beid = itemstack.Attributes.GetLong("bobberEntityId", 0L);
		if (beid != 0L)
		{
			cm.UnregisterCloth(itemstack.Attributes.GetInt("clothId"));
			if (api.Side == EnumAppSide.Server)
			{
				EntityBobber be = api.World.GetEntityById(beid) as EntityBobber;
				be?.TryCatchFish(byEntity);
				be?.Die();
				if (be?.BaitStack != null)
				{
					itemstack.Attributes.SetItemstack("fishingBait", be.BaitStack);
				}
				else
				{
					itemstack.Attributes.RemoveAttribute("fishingBait");
				}
			}
			itemstack.Attributes.RemoveAttribute("bobberEntityId");
			itemstack.Attributes.RemoveAttribute("clothId");
			itemstack.Attributes.RemoveAttribute("fishingEntityId");
			return true;
		}
		return false;
	}

	public override void OnCreatedByCrafting(ItemSlot[] allInputSlots, ItemSlot outputSlot, IRecipeBase byRecipe)
	{
		ItemStack baitStack = allInputSlots.FirstOrDefault(delegate(ItemSlot slot)
		{
			ItemStack? itemstack = slot.Itemstack;
			return itemstack != null && itemstack.Collectible.Attributes?.IsTrue("isFishBait") == true;
		})?.Itemstack;
		if (baitStack != null)
		{
			baitStack = baitStack.Clone();
			baitStack.StackSize = 1;
			outputSlot.Itemstack?.Attributes.SetItemstack("fishingBait", baitStack);
		}
	}

	public override bool MatchesForCrafting(ItemStack inputStack, IRecipeBase gridRecipe, IRecipeIngredient ingredient)
	{
		if (gridRecipe.RecipeIngredients.Any((IRecipeIngredient ingred) => ingred != null && ingred.ResolvedItemStack?.Collectible?.Attributes?.IsTrue("isFishBait") == true) && inputStack.Attributes.GetItemstack("fishingBait") != null)
		{
			return false;
		}
		return base.MatchesForCrafting(inputStack, gridRecipe, ingredient);
	}

	public string HandbookPageCodeForStack(IWorldAccessor world, ItemStack stack)
	{
		ItemStack itemStack = stack.Clone();
		itemStack.Attributes.RemoveAttribute("fishingBait");
		return GuiHandbookItemStackPage.PageCodeForStack(itemStack);
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		ItemStack baitStack = inSlot.Itemstack?.Attributes.GetItemstack("fishingBait");
		baitStack?.ResolveBlockOrItem(world);
		if (baitStack != null)
		{
			dsc.AppendLine(Lang.Get("item-fishingpole-bait", baitStack.GetName()));
		}
		else if (inSlot.Itemstack.Attributes.GetBool("fishing"))
		{
			EntityBobber ebobber = api.World.GetEntityById(inSlot.Itemstack.Attributes.GetLong("bobberEntityId", 0L)) as EntityBobber;
			if (ebobber?.BaitStack != null)
			{
				dsc.AppendLine(Lang.Get("item-fishingpole-bait", ebobber.BaitStack.GetName()));
			}
		}
	}

	public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		updatePinOffset(slot, byEntity);
		return true;
	}

	public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
	}

	public override void OnModifiedInInventorySlot(IWorldAccessor world, ItemSlot slot, ItemStack? extractedStack = null)
	{
		long eid = slot.Itemstack?.Attributes.GetLong("fishingEntityId", 0L) ?? 0;
		EntityAgent eagent = world.GetEntityById(eid) as EntityAgent;
		if (extractedStack != null && eagent != null && eagent.RightHandItemSlot.Empty)
		{
			StopFishing(extractedStack, eagent);
		}
	}

	private ClothSystem? getRope(ItemSlot slot)
	{
		int clothId = slot.Itemstack?.Attributes.GetInt("clothId") ?? 0;
		return cm.GetClothSystem(clothId);
	}

	private ClothSystem createRope(ItemSlot slot, EntityAgent byEntity, EntityBobber ebobber, Vec3d targetPos)
	{
		ClothSystem sys = ClothSystem.CreateRope(api, cm, byEntity.Pos.XYZ, targetPos, null, 4f);
		sys.ChangeRopeLength(0.5);
		sys.CanPull = false;
		sys.CanRip = false;
		sys.StretchWarn = 1.5f;
		sys.StretchRip = 2f;
		sys.FirstPoint.PinTo(byEntity, offsetToPoleTipTp);
		sys.LastPoint.PinTo(ebobber, new Vec3f());
		sys.LastPoint.NoAttachTransform = true;
		sys.RopeRenderThickness = 0.35f;
		cm.RegisterCloth(sys);
		api.World.RegisterCallback(delegate
		{
			sys.CanRip = true;
		}, 2000);
		slot.Itemstack?.Attributes.SetLong("ropeHeldByEntityId", byEntity.EntityId);
		slot.Itemstack?.Attributes.SetInt("clothId", sys.ClothId);
		slot.MarkDirty();
		return sys;
	}

	public static bool LoadHeldItemModelMatrix(Matrixf mmat, EntityAgent entity, ItemSlot itemSlot, ICoreClientAPI api, bool right = true)
	{
		if (!(entity.Properties.Client.Renderer is EntityShapeRenderer entityShapeRenderer))
		{
			return false;
		}
		if (itemSlot.Itemstack == null)
		{
			return false;
		}
		AttachmentPointAndPose attachmentPointAndPose = entity.AnimManager?.Animator?.GetAttachmentPointPose(right ? "RightHand" : "LeftHand");
		if (attachmentPointAndPose == null)
		{
			return false;
		}
		AttachmentPoint attachPoint = attachmentPointAndPose.AttachPoint;
		ItemRenderInfo itemStackRenderInfo = api.Render.GetItemStackRenderInfo(itemSlot, right ? EnumItemRenderTarget.HandTp : EnumItemRenderTarget.HandTpOff, 0f);
		if (itemStackRenderInfo?.Transform == null)
		{
			return false;
		}
		mmat.Set(entityShapeRenderer.ModelMat).Mul(attachmentPointAndPose.AnimModelMatrix).Translate(itemStackRenderInfo.Transform.Origin.X, itemStackRenderInfo.Transform.Origin.Y, itemStackRenderInfo.Transform.Origin.Z)
			.Scale(itemStackRenderInfo.Transform.ScaleXYZ.X, itemStackRenderInfo.Transform.ScaleXYZ.Y, itemStackRenderInfo.Transform.ScaleXYZ.Z)
			.Translate(attachPoint.PosX / 16.0 + (double)itemStackRenderInfo.Transform.Translation.X, attachPoint.PosY / 16.0 + (double)itemStackRenderInfo.Transform.Translation.Y, attachPoint.PosZ / 16.0 + (double)itemStackRenderInfo.Transform.Translation.Z)
			.RotateX((float)(attachPoint.RotationX + (double)itemStackRenderInfo.Transform.Rotation.X) * ((float)Math.PI / 180f))
			.RotateY((float)(attachPoint.RotationY + (double)itemStackRenderInfo.Transform.Rotation.Y) * ((float)Math.PI / 180f))
			.RotateZ((float)(attachPoint.RotationZ + (double)itemStackRenderInfo.Transform.Rotation.Z) * ((float)Math.PI / 180f))
			.Translate(0f - itemStackRenderInfo.Transform.Origin.X, 0f - itemStackRenderInfo.Transform.Origin.Y, 0f - itemStackRenderInfo.Transform.Origin.Z);
		return true;
	}

	public bool OnContainedInteractStart(BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		ItemSlot handSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
		ItemStack? itemstack = handSlot.Itemstack;
		if (itemstack != null && itemstack.Collectible.Attributes?["isFishBait"]?.AsBool() == true)
		{
			if (slot.Itemstack?.Attributes.GetItemstack("fishingBait") == null)
			{
				slot.Itemstack?.Attributes.SetItemstack("fishingBait", handSlot.Itemstack);
				handSlot.TakeOut(1);
				handSlot.MarkDirty();
			}
			else
			{
				(api as ICoreClientAPI)?.TriggerIngameError(this, "fishingpolealreadybaited", Lang.Get("ingameerror-fishingpole-alreadybaited"));
			}
			return true;
		}
		return false;
	}

	public bool OnContainedInteractStep(float secondsUsed, BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		return false;
	}

	public void OnContainedInteractStop(float secondsUsed, BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
	}

	public WorldInteraction[] GetContainedInteractionHelp(BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (slot.Itemstack?.Attributes.GetItemstack("fishingBait") != null)
		{
			return Array.Empty<WorldInteraction>();
		}
		return new WorldInteraction[1]
		{
			new WorldInteraction
			{
				ActionLangCode = "blockhelp-fishingpole-bait",
				MouseButton = EnumMouseButton.Right,
				Itemstacks = ObjectCacheUtil.TryGet<ItemStack[]>(api, "fishingbaititems")
			}
		};
	}
}
