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

public class BlockCookedContainer : BlockCookedContainerBase, IInFirepitRendererSupplier, IContainedMeshSource, IGroundStoredParticleEmitter, IAttachableToEntity
{
	public static SimpleParticleProperties smokeHeld;

	public static SimpleParticleProperties foodSparks;

	private Vec3d gsSmokePos = new Vec3d(0.5, 0.3125, 0.5);

	private WorldInteraction[]? interactions;

	private IAttachableToEntity? attrAtta;

	private MealMeshCache? meshCache;

	public float yoff = 2.5f;

	public int RequiresBehindSlots { get; set; }

	public bool IsAttachable(Entity toEntity, ItemStack itemStack)
	{
		return attrAtta != null;
	}

	string? IAttachableToEntity.GetCategoryCode(ItemStack stack)
	{
		return attrAtta?.GetCategoryCode(stack);
	}

	CompositeShape? IAttachableToEntity.GetAttachedShape(ItemStack stack, string slotCode)
	{
		return attrAtta?.GetAttachedShape(stack, slotCode);
	}

	string[]? IAttachableToEntity.GetDisableElements(ItemStack stack)
	{
		return attrAtta?.GetDisableElements(stack);
	}

	string[]? IAttachableToEntity.GetKeepElements(ItemStack stack)
	{
		return attrAtta?.GetKeepElements(stack);
	}

	string? IAttachableToEntity.GetTexturePrefixCode(ItemStack stack)
	{
		return attrAtta?.GetTexturePrefixCode(stack);
	}

	void IAttachableToEntity.CollectTextures(ItemStack itemstack, Shape intoShape, string texturePrefixCode, Dictionary<string, CompositeTexture> intoDict)
	{
		attrAtta?.CollectTextures(itemstack, intoShape, texturePrefixCode, intoDict);
	}

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		if (CollisionBoxes[0] != null)
		{
			gsSmokePos.Y = CollisionBoxes[0].MaxY;
		}
		attrAtta = IAttachableToEntity.FromAttributes(this);
		ICoreClientAPI capi = api as ICoreClientAPI;
		if (capi == null)
		{
			return;
		}
		meshCache = api.ModLoader.GetModSystem<MealMeshCache>();
		interactions = ObjectCacheUtil.GetOrCreate(api, "cookedContainerBlockInteractions", delegate
		{
			List<ItemStack> list = new List<ItemStack>();
			foreach (CollectibleObject current in api.World.Collectibles)
			{
				JsonObject attributes = current.Attributes;
				if (attributes != null && attributes.IsTrue("mealContainer"))
				{
					List<ItemStack> handBookStacks = current.GetHandBookStacks(capi);
					if (handBookStacks != null)
					{
						list.AddRange(handBookStacks);
					}
				}
			}
			return new WorldInteraction[2]
			{
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-cookedcontainer-takefood",
					HotKeyCode = null,
					MouseButton = EnumMouseButton.Right,
					Itemstacks = list.ToArray()
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-cookedcontainer-pickup",
					HotKeyCode = null,
					MouseButton = EnumMouseButton.Right
				}
			};
		});
	}

	static BlockCookedContainer()
	{
		smokeHeld = new SimpleParticleProperties(1f, 1f, ColorUtil.ToRgba(50, 220, 220, 220), new Vec3d(), new Vec3d(), new Vec3f(-0.05f, 0.1f, -0.05f), new Vec3f(0.05f, 0.15f, 0.05f), 1.5f, 0f, 0.25f, 0.35f, EnumParticleModel.Quad);
		smokeHeld.SelfPropelled = true;
		smokeHeld.AddPos.Set(0.1, 0.1, 0.1);
		foodSparks = new SimpleParticleProperties(1f, 1f, ColorUtil.ToRgba(255, 83, 233, 255), new Vec3d(), new Vec3d(), new Vec3f(-3f, 1f, -3f), new Vec3f(3f, 8f, 3f), 0.5f, 1f, 0.25f, 0.25f);
		foodSparks.VertexFlags = 0;
	}

	public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
	{
		if (meshCache == null)
		{
			meshCache = capi.ModLoader.GetModSystem<MealMeshCache>();
		}
		CookingRecipe recipe = GetCookingRecipe(capi.World, itemstack);
		ItemStack[] contents = GetNonEmptyContents(capi.World, itemstack);
		MultiTextureMeshRef meshref = meshCache.GetOrCreateMealInContainerMeshRef(this, recipe, contents, new Vec3f(0f, yoff / 16f, 0f));
		if (meshref != null)
		{
			renderinfo.ModelRef = meshref;
		}
	}

	public virtual string GetMeshCacheKey(ItemSlot slot)
	{
		return meshCache.GetMealHashCode(slot.Itemstack, new Vec3f(0f, yoff / 16f, 0f)).ToString();
	}

	public MeshData GenMesh(ItemSlot slot, ITextureAtlasAPI targetAtlas, BlockPos? forBlockPos)
	{
		return meshCache.GenMealInContainerMesh(this, GetCookingRecipe(api.World, slot.Itemstack), GetNonEmptyContents(api.World, slot.Itemstack), new Vec3f(0f, yoff / 16f, 0f));
	}

	public override TransitionState[]? UpdateAndGetTransitionStates(IWorldAccessor world, ItemSlot inslot)
	{
		ItemStack cookedContStack = inslot.Itemstack;
		if (cookedContStack == null)
		{
			return null;
		}
		ItemStack[] stacks = GetNonEmptyContents(world, cookedContStack);
		ItemStack[] array = stacks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].StackSize *= (int)Math.Max(1f, cookedContStack.Attributes.TryGetFloat("quantityServings") ?? 1f);
		}
		SetContents(cookedContStack, stacks);
		TransitionState[] states = base.UpdateAndGetTransitionStates(world, inslot);
		stacks = GetNonEmptyContents(world, cookedContStack);
		if (stacks.Length == 0 || MealMeshCache.ContentsRotten(stacks))
		{
			for (int j = 0; j < stacks.Length; j++)
			{
				TransitionableProperties spoilProps = stacks[j].Collectible.GetTransitionableProperties(world, stacks[j], null)?.FirstOrDefault((TransitionableProperties props) => props.Type == EnumTransitionType.Perish);
				if (spoilProps != null)
				{
					stacks[j] = stacks[j].Collectible.OnTransitionNow(GetContentInDummySlot(inslot, stacks[j]), spoilProps);
				}
			}
			SetContents(cookedContStack, stacks);
			cookedContStack.Attributes.RemoveAttribute("recipeCode");
			cookedContStack.Attributes.RemoveAttribute("quantityServings");
		}
		array = stacks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].StackSize /= (int)Math.Max(1f, cookedContStack.Attributes.TryGetFloat("quantityServings") ?? 1f);
		}
		SetContents(cookedContStack, stacks);
		if (stacks.Length == 0)
		{
			string emptiedBlockCode = Attributes?["emptiedBlockCode"]?.AsString();
			if (emptiedBlockCode != null)
			{
				Block block = world.GetBlock(new AssetLocation(emptiedBlockCode));
				if (block != null)
				{
					inslot.Itemstack = new ItemStack(block);
					inslot.MarkDirty();
				}
			}
		}
		return states;
	}

	public override string GetHeldItemName(ItemStack? itemStack)
	{
		ItemStack[] contentStacks = GetContents(api.World, itemStack);
		string recipeCode = itemStack?.Collectible.GetCollectibleInterface<IBlockMealContainer>()?.GetRecipeCode(api.World, itemStack);
		string code = Lang.Get("mealrecipe-name-" + recipeCode + "-in-container");
		if (recipeCode == null)
		{
			if (MealMeshCache.ContentsRotten(contentStacks))
			{
				code = Lang.Get("Rotten Food");
			}
			else if (contentStacks.Length != 0)
			{
				code = contentStacks[0].GetName();
			}
		}
		return Lang.GetMatching(Code?.Domain + ":" + ItemClass.Name() + "-" + Code?.Path, code);
	}

	public override void OnHeldIdle(ItemSlot slot, EntityAgent byEntity)
	{
		if (byEntity.World.Side == EnumAppSide.Client && GetTemperature(byEntity.World, slot.Itemstack) > 50f && byEntity.World.Rand.NextDouble() < 0.07)
		{
			float sideWays = 0.35f;
			if ((byEntity as EntityPlayer)?.Player is IClientPlayer { CameraMode: not EnumCameraMode.FirstPerson })
			{
				sideWays = 0f;
			}
			Vec3d pos = byEntity.Pos.XYZ.Add(0.0, byEntity.LocalEyePos.Y - 0.5, 0.0).Ahead(0.33000001311302185, byEntity.Pos.Pitch, byEntity.Pos.Yaw).Ahead(sideWays, 0f, byEntity.Pos.Yaw + (float)Math.PI / 2f);
			smokeHeld.MinPos = pos.AddCopy(-0.05, 0.1, -0.05);
			byEntity.World.SpawnParticles(smokeHeld);
		}
	}

	public override void OnGroundIdle(EntityItem entityItem)
	{
		base.OnGroundIdle(entityItem);
		IWorldAccessor world = entityItem.World;
		if (world.Side != EnumAppSide.Server || !entityItem.Swimming || !(world.Rand.NextDouble() < 0.01))
		{
			return;
		}
		ItemStack[] stacks = GetContents(world, entityItem.Itemstack);
		if (MealMeshCache.ContentsRotten(stacks))
		{
			for (int i = 0; i < stacks.Length; i++)
			{
				if (stacks[i] != null && stacks[i].StackSize > 0 && stacks[i].Collectible.Code.Path == "rot")
				{
					world.SpawnItemEntity(stacks[i], entityItem.Pos.XYZ);
				}
			}
		}
		else
		{
			ItemStack rndStack = stacks[world.Rand.Next(stacks.Length)];
			world.SpawnCubeParticles(entityItem.Pos.XYZ, rndStack, 0.3f, 25);
		}
		Block emptyPotBlock = world.GetBlock(new AssetLocation(Attributes["emptiedBlockCode"].AsString()));
		entityItem.Itemstack = new ItemStack(emptyPotBlock);
		entityItem.WatchedAttributes.MarkPathDirty("itemstack");
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
	{
		ItemStack stack = base.OnPickBlock(world, pos);
		if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityCookedContainer bec)
		{
			ItemStack[] contentStacks = bec.GetNonEmptyContentStacks();
			SetContents(bec.RecipeCode, bec.QuantityServings, stack, contentStacks);
			float temp = ((contentStacks.Length != 0) ? contentStacks[0].Collectible.GetTemperature(world, contentStacks[0]) : 0f);
			SetTemperature(world, stack, temp, delayCooldown: false);
		}
		return stack;
	}

	public override void OnUnloaded(ICoreAPI api)
	{
		if (!(api is ICoreClientAPI capi) || !capi.ObjectCache.TryGetValue("cookedMeshRefs", out var obj))
		{
			return;
		}
		if (obj is Dictionary<int, MultiTextureMeshRef> meshrefs)
		{
			foreach (KeyValuePair<int, MultiTextureMeshRef> item in meshrefs)
			{
				item.Value.Dispose();
			}
		}
		capi.ObjectCache.Remove("cookedMeshRefs");
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
	{
		return new ItemStack[1] { OnPickBlock(world, pos) };
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		ItemStack cookedContStack = inSlot.Itemstack;
		if (cookedContStack == null)
		{
			return;
		}
		base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		float temp = GetTemperature(world, cookedContStack);
		if (temp > 20f)
		{
			dsc.AppendLine(Lang.Get("Temperature: {0}°C", (int)temp));
		}
		CookingRecipe recipe = GetMealRecipe(world, cookedContStack);
		float servings = cookedContStack.Attributes.GetFloat("quantityServings");
		ItemStack[] stacks = GetNonEmptyContents(world, cookedContStack);
		if (recipe != null)
		{
			string outputName = recipe.GetOutputName(world, stacks);
			string message = ((recipe.CooksInto == null) ? "{0} servings of {1}" : "nonfood-portions");
			dsc.AppendLine(Lang.Get(message, Math.Round(servings, 1), outputName));
		}
		BlockMeal[]? array = BlockMeal.AllMealBowls(api);
		string nutriFacts = ((array == null) ? null : array[0]?.GetContentNutritionFacts(api.World, inSlot, stacks, null));
		if (nutriFacts != null && recipe?.CooksInto == null)
		{
			dsc.AppendLine(nutriFacts);
		}
		if (cookedContStack.Attributes.GetBool("timeFrozen"))
		{
			return;
		}
		DummyInventory dummyInv = new DummyInventory(api);
		ItemSlot dummySlot = BlockCrock.GetDummySlotForFirstPerishableStack(api.World, stacks, null, dummyInv);
		dummyInv.OnAcquireTransitionSpeed += delegate(EnumTransitionType transType, ItemStack stack, float mul)
		{
			float num = inSlot.Inventory?.GetTransitionSpeedMul(transType, cookedContStack) ?? 1f;
			if (transType != EnumTransitionType.Perish)
			{
				num = 0f;
			}
			return num * GetContainingTransitionModifierContained(world, inSlot, transType);
		};
		TransitionState perishState = dummySlot.Itemstack?.Collectible.UpdateAndGetTransitionStates(api.World, dummySlot)?.FirstOrDefault((TransitionState state) => state.Props.Type == EnumTransitionType.Perish);
		if (perishState != null)
		{
			dummySlot.Itemstack?.Collectible.AppendPerishableInfoText(dummySlot, dsc, world, perishState, nowSpoiling: false);
		}
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		ItemSlot hotbarSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
		if (!hotbarSlot.Empty)
		{
			JsonObject attributes = hotbarSlot.Itemstack.Collectible.Attributes;
			if (attributes != null && attributes.IsTrue("mealContainer"))
			{
				return (world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityCookedContainer)?.ServeInto(byPlayer, hotbarSlot) ?? false;
			}
		}
		ItemStack stack = OnPickBlock(world, blockSel.Position);
		if (byPlayer.InventoryManager.TryGiveItemstack(stack, slotNotifyEffect: true))
		{
			world.BlockAccessor.SetBlock(0, blockSel.Position);
			world.PlaySoundAt(Sounds.Place, byPlayer, byPlayer);
			return true;
		}
		return base.OnBlockInteractStart(world, byPlayer, blockSel);
	}

	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling)
	{
		if (blockSel != null)
		{
			BlockPos onBlockPos = blockSel.Position;
			Block block = byEntity.World.BlockAccessor.GetBlock(onBlockPos);
			if (block is BlockClayOven)
			{
				return;
			}
			Block selectedBlock = byEntity.World.BlockAccessor.GetBlock(blockSel.Position);
			if (selectedBlock != null && selectedBlock.Attributes?.IsTrue("mealContainer") == true)
			{
				if (byEntity.Controls.ShiftKey)
				{
					ServeIntoBowl(selectedBlock, blockSel.Position, slot, byEntity.World);
					handHandling = EnumHandHandling.PreventDefault;
				}
				return;
			}
			float quantityServings = (float)(slot.Itemstack?.Attributes.GetDecimal("quantityServings") ?? 0.0);
			if (block is BlockGroundStorage)
			{
				if (!byEntity.Controls.ShiftKey || !(api.World.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityGroundStorage begs))
				{
					return;
				}
				ItemSlot gsslot = begs.GetSlotAt(blockSel);
				if (gsslot == null || gsslot.Empty)
				{
					return;
				}
				JsonObject itemAttributes = gsslot.Itemstack.ItemAttributes;
				if (itemAttributes != null && itemAttributes.IsTrue("mealContainer"))
				{
					if (quantityServings > 0f)
					{
						ServeIntoStack(gsslot, slot, byEntity.World);
						gsslot.MarkDirty();
						begs.MarkMeshesDirty();
						begs.MarkDirty(redrawOnClient: true);
					}
					handHandling = EnumHandHandling.PreventDefault;
					return;
				}
			}
		}
		base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handHandling);
	}

	public override int GetRandomColor(ICoreClientAPI capi, BlockPos pos, BlockFacing facing, int rndIndex = -1)
	{
		if (capi.World.BlockAccessor.GetBlockEntity(pos) is BlockEntityCookedContainer bem)
		{
			ItemStack[] stacks = bem.GetNonEmptyContentStacks();
			if (stacks != null && stacks.Length != 0)
			{
				ItemStack rndStack = stacks[capi.World.Rand.Next(stacks.Length)];
				if (capi.World.Rand.NextDouble() < 0.4)
				{
					return capi.BlockTextureAtlas.GetRandomColor(Textures["ceramic"].Baked.TextureSubId);
				}
				if (rndStack.Class == EnumItemClass.Block)
				{
					return rndStack.Block.GetRandomColor(capi, pos, facing, rndIndex);
				}
				return capi.ItemTextureAtlas.GetRandomColor(rndStack.Item.FirstTexture.Baked.TextureSubId, rndIndex);
			}
		}
		return base.GetRandomColor(capi, pos, facing, rndIndex);
	}

	public override int GetRandomColor(ICoreClientAPI capi, ItemStack stack)
	{
		ItemStack[] stacks = GetNonEmptyContents(capi.World, stack);
		if (stacks.Length == 0)
		{
			return base.GetRandomColor(capi, stack);
		}
		ItemStack rndStack = stacks[capi.World.Rand.Next(stacks.Length)];
		if (capi.World.Rand.NextDouble() < 0.4)
		{
			return capi.BlockTextureAtlas.GetRandomColor(Textures["ceramic"].Baked.TextureSubId);
		}
		return rndStack.Collectible.GetRandomColor(capi, stack);
	}

	public IInFirepitRenderer GetRendererWhenInFirepit(ItemStack stack, BlockEntityFirepit firepit, bool forOutputSlot)
	{
		return new PotInFirepitRenderer(api as ICoreClientAPI, stack, firepit.Pos, forOutputSlot);
	}

	public EnumFirepitModel GetDesiredFirepitModel(ItemStack stack, BlockEntityFirepit firepit, bool forOutputSlot)
	{
		return EnumFirepitModel.Wide;
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		return interactions.Append<WorldInteraction>(base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
	}

	public override bool OnSmeltAttempt(InventoryBase inventorySmelting)
	{
		JsonObject attributes = Attributes;
		if (attributes != null && attributes["isDirtyPot"]?.AsBool() == true && inventorySmelting.Count > 1)
		{
			InventorySmelting inv = (InventorySmelting)inventorySmelting;
			int quantityServings = (int)((float)(inv[1].Itemstack?.Attributes.GetDecimal("quantityServings") ?? 0.0) + 0.001f);
			if (quantityServings > 0)
			{
				ItemStack[] myStacks = GetNonEmptyContents(api.World, inv[1].Itemstack);
				if (myStacks.Length != 0 && inv.CookingSlots.Length != 0)
				{
					myStacks[0].StackSize = quantityServings;
					inv.CookingSlots[0].Itemstack = myStacks[0];
				}
			}
			AssetLocation loc = AssetLocation.CreateOrNull(Attributes?["emptiedBlockCode"]?.AsString());
			Block block = api.World.GetBlock(loc);
			if (block != null)
			{
				inv[1].Itemstack = new ItemStack(block);
			}
			return true;
		}
		return false;
	}

	public virtual bool ShouldSpawnGSParticles(IWorldAccessor world, ItemStack stack)
	{
		return world.Rand.NextDouble() < (double)((GetTemperature(world, stack) - 50f) / 160f / 8f);
	}

	public virtual void DoSpawnGSParticles(IAsyncParticleManager manager, BlockPos pos, Vec3f offset)
	{
		smokeHeld.MinPos = pos.ToVec3d().AddCopy(gsSmokePos).AddCopy(offset);
		manager.Spawn(smokeHeld);
	}
}
