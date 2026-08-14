using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BEBehaviorCabinetDoors : BEBehaviorAnimatable, IInteractable
{
	protected bool opened;

	protected Cuboidf[]? selectionBoxesOpened;

	protected Cuboidf[]? selectionBoxesClosed;

	protected ICoreClientAPI? capi;

	protected ItemStack? doorStack;

	private MeshData? doorMesh;

	public ItemStack? DoorStack => doorStack;

	public BEBehaviorCabinetDoors(BlockEntity blockentity)
		: base(blockentity)
	{
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		capi = api as ICoreClientAPI;
		updateAnimationState();
	}

	private void updateAnimationState()
	{
		if (!opened)
		{
			animUtil?.StopAnimation("opened");
			return;
		}
		animUtil?.StartAnimation(new AnimationMetaData
		{
			Animation = "opened",
			Code = "opened",
			AnimationSpeed = 0.7f,
			EaseOutSpeed = 4f
		});
	}

	public bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		if (doorStack == null)
		{
			if (byPlayer.Entity.Controls.CtrlKey)
			{
				ItemSlot slot = byPlayer.InventoryManager?.ActiveHotbarSlot;
				if (slot?.Itemstack?.Collectible.GetCollectibleBehavior<CollectibleBehaviorTypedTexture>(withInheritance: true) != null)
				{
					if (byPlayer.WorldData.CurrentGameMode == EnumGameMode.Creative)
					{
						doorStack = slot.Itemstack.Clone();
						doorStack.StackSize = 1;
					}
					else
					{
						doorStack = slot.TakeOut(1);
					}
					playDoorPlaceRemoveSound(doorStack, byPlayer);
					slot.MarkDirty();
					updateAnimationState();
					handling = EnumHandling.PreventSubsequent;
					Blockentity.MarkDirty(redrawOnClient: true);
					return true;
				}
			}
			return false;
		}
		if (byPlayer.Entity.Controls.CtrlKey)
		{
			ItemStack? itemstack = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack;
			if (itemstack != null && itemstack.Collectible.Tool == EnumTool.Wrench)
			{
				if (byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative && !byPlayer.InventoryManager.TryGiveItemstack(doorStack))
				{
					world.SpawnItemEntity(doorStack, blockSel.Position);
				}
				playDoorPlaceRemoveSound(DoorStack, byPlayer);
				doorStack = null;
				doorMesh = null;
				opened = false;
				animUtil.animator = null;
				animUtil.renderer?.Dispose();
				animUtil.renderer = null;
				handling = EnumHandling.PreventSubsequent;
				Blockentity.MarkDirty(redrawOnClient: true);
				return true;
			}
		}
		if (blockSel.SelectionBoxId == "door")
		{
			opened = !opened;
			updateAnimationState();
			handling = EnumHandling.PreventDefault;
			return true;
		}
		return false;
	}

	protected void playDoorPlaceRemoveSound(ItemStack doorStack, IPlayer byPlayer)
	{
		SoundAttributes soundAttributes = new SoundAttributes();
		soundAttributes.Location = AssetLocation.Create(doorStack.Collectible.Attributes["placeRemoveSound"].AsString("block/planks"), doorStack.Collectible.Code.Domain).WithPathPrefixOnce("sounds/");
		soundAttributes.Range = 8f;
		soundAttributes.Type = EnumSoundType.Sound;
		SoundAttributes soundAttr = soundAttributes;
		Api.World.PlaySoundAt(soundAttr, base.Pos, 0.5, byPlayer);
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		base.FromTreeAttributes(tree, worldAccessForResolve);
		opened = tree.GetBool("opened");
		bool doorStackWasNull = doorStack == null;
		doorStack = tree.GetItemstack("doorStack");
		doorStack?.ResolveBlockOrItem(worldAccessForResolve);
		if (doorStack != null && doorStackWasNull && worldAccessForResolve.Side == EnumAppSide.Client)
		{
			Blockentity.MarkDirty(redrawOnClient: true);
		}
		updateAnimationState();
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetBool("opened", opened);
		tree.SetItemstack("doorStack", doorStack);
	}

	public override void OnLoadCollectibleMappings(IWorldAccessor worldForNewMappings, Dictionary<int, AssetLocation> oldBlockIdMapping, Dictionary<int, AssetLocation> oldItemIdMapping, int schematicSeed, bool resolveImports)
	{
		ItemStack? itemStack = doorStack;
		if (itemStack != null && !itemStack.FixMapping(oldBlockIdMapping, oldItemIdMapping, worldForNewMappings))
		{
			doorStack = null;
		}
		else
		{
			doorStack?.Collectible.OnLoadCollectibleMappings(worldForNewMappings, new DummySlot(doorStack), oldBlockIdMapping, oldItemIdMapping, resolveImports);
		}
	}

	public override void OnStoreCollectibleMappings(Dictionary<int, AssetLocation> blockIdMapping, Dictionary<int, AssetLocation> itemIdMapping)
	{
		doorStack?.Collectible.OnStoreCollectibleMappings(Api.World, new DummySlot(doorStack), blockIdMapping, itemIdMapping);
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		if (doorStack == null)
		{
			return false;
		}
		ensureMeshExists(tessThreadTesselator);
		if (animUtil.activeAnimationsByAnimCode.Count > 0 || (animUtil.animator != null && animUtil.animator.ActiveAnimationCount > 0))
		{
			return false;
		}
		float angleRad = Blockentity.GetBehavior<IRotatablePlaceable>()?.MeshAngleRad ?? 0f;
		float[] mat = new Matrixf().Translate(0.5f, 0f, 0.5f).RotateYDeg(base.Block.Shape.rotateY).RotateY(angleRad)
			.Translate(-0.5f, 0f, -0.5f)
			.Values;
		mesher.AddMeshData(doorMesh, mat);
		return false;
	}

	private void ensureMeshExists(ITesselatorAPI tesselator)
	{
		CollectibleBehaviorTypedTexture titem = doorStack?.Collectible.GetCollectibleBehavior<CollectibleBehaviorTypedTexture>(withInheritance: true);
		if (titem != null && capi != null && doorStack != null && doorMesh == null)
		{
			AssetLocation shapeLoc = titem.CompositeShape.Base.Clone();
			shapeLoc.Path = shapeLoc.Path.Replace("middle", base.Block.Variant["section"]);
			Shape shape = capi.Assets.Get<Shape>(shapeLoc.WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json"));
			string dictKey = "cabinetdoors-" + base.Block.Code;
			Vec3f rot = new Vec3f(0f, (180f / (float)Math.PI * Blockentity.GetBehavior<IRotatablePlaceable>()?.MeshAngleRad).GetValueOrDefault(), 0f);
			string animkey = base.Block.Shape.ToString();
			Dictionary<string, string> materials = titem.GetMaterials(doorStack);
			materials["section"] = base.Block.Variant["section"];
			TypedTextureSource texSource = new TypedTextureSource(capi, capi.BlockTextureAtlas, doorStack.Collectible.Code.Domain, titem.TextureMapping, materials);
			doorMesh = animUtil.CreateMesh(animkey, shape, out var animatableShape, texSource, new TesselationMetaData());
			animUtil.InitializeAnimator(dictKey, doorMesh, animatableShape, rot);
			updateAnimationState();
		}
	}

	public Cuboidf[]? GetSelectionBoxes()
	{
		if (doorStack == null)
		{
			return Array.Empty<Cuboidf>();
		}
		if (selectionBoxesClosed == null)
		{
			selectionBoxesOpened = rotatedCopy(base.Block.GetBehavior<BlockBehaviorCabinetDoors>().DoorSelectionBoxOpened);
			selectionBoxesClosed = rotatedCopy(base.Block.GetBehavior<BlockBehaviorCabinetDoors>().DoorSelectionBoxClosed);
		}
		if (!opened)
		{
			return selectionBoxesClosed;
		}
		return selectionBoxesOpened;
	}

	private Cuboidf[]? rotatedCopy(Cuboidf[]? boxes)
	{
		float angleRad = Blockentity.GetBehavior<IRotatablePlaceable>()?.MeshAngleRad ?? 0f;
		CuboidfWithId[] rotCubes = new CuboidfWithId[boxes.Length];
		for (int i = 0; i < boxes.Length; i++)
		{
			rotCubes[i] = new CuboidfWithId(boxes[i].RotatedCopyRad(0f, angleRad, 0f, new Vec3d(0.5, 0.0, 0.5)))
			{
				Id = "door"
			};
		}
		return rotCubes;
	}
}
