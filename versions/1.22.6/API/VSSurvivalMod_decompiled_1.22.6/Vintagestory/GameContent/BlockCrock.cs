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

public class BlockCrock : BlockCookedContainerBase, IBlockMealContainer, IContainedMeshSource
{
	private string shapeLocation = "game:shapes/block/clay/crock/";

	private string[] labelNames = new string[13]
	{
		"carrot", "cabbage", "onion", "parsnip", "turnip", "pumpkin", "soybean", "bellpepper", "cassava", "mushroom",
		"redmeat", "poultry", "porridge"
	};

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		string[] labelNames = Attributes?["labelNames"].AsArray<string>();
		if (labelNames != null)
		{
			this.labelNames = labelNames;
			for (int i = 0; i < labelNames.Length; i++)
			{
				if (labelNames[i] == null)
				{
					throw new ArgumentNullException($"label name at index {i} is null for BlockCrock {Code}");
				}
			}
		}
		if (Vintagestory.API.Common.Shape.TryGet(api, Shape.Base?.Clone().WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json")) != null)
		{
			shapeLocation = Shape.Base.Clone().WithFilename("").WithPathPrefixOnce("shapes/")
				.ToString();
		}
		else if (Vintagestory.API.Common.Shape.TryGet(api, AssetLocation.Create("shapes/block/clay/crock/base.json", Code.Domain)) != null)
		{
			shapeLocation = Code.Domain + ":shapes/block/clay/crock/";
		}
		ObjectCacheUtil.GetOrCreate(api, "crocksealitems", () => api.World.Collectibles.Where((CollectibleObject obj) => obj.Attributes?.IsTrue("canSealCrock") ?? false).SelectMany((CollectibleObject obj) => obj.GetHandBookStacks(api as ICoreClientAPI) ?? new List<ItemStack>()).ToArray());
	}

	public override float GetContainingTransitionModifierContained(IWorldAccessor world, ItemSlot inSlot, EnumTransitionType transType)
	{
		float mul = 1f;
		if (transType == EnumTransitionType.Perish)
		{
			ItemStack? itemstack = inSlot.Itemstack;
			mul = ((itemstack == null || itemstack.Attributes?.GetBool("sealed") != true) ? (mul * 0.85f) : ((inSlot.Itemstack.Attributes.GetString("recipeCode") == null) ? (mul * 0.25f) : (mul * 0.1f)));
		}
		return mul;
	}

	public override float GetContainingTransitionModifierPlaced(IWorldAccessor world, BlockPos pos, EnumTransitionType transType)
	{
		float mul = 1f;
		if (!(world.BlockAccessor.GetBlockEntity(pos) is BlockEntityCrock becrock))
		{
			return mul;
		}
		if (transType == EnumTransitionType.Perish)
		{
			mul = ((!becrock.Sealed) ? (mul * 0.85f) : ((becrock.RecipeCode == null) ? (mul * 0.25f) : (mul * 0.1f)));
		}
		return mul;
	}

	public AssetLocation LabelForContents(string? recipeCode, ItemStack[]? contents)
	{
		if (contents == null || contents.Length == 0 || contents[0] == null)
		{
			return AssetLocation.Create(shapeLocation + "label-empty.json");
		}
		if (MealMeshCache.ContentsRotten(contents))
		{
			return AssetLocation.Create(shapeLocation + "label-rot.json");
		}
		if (recipeCode != null && recipeCode.Length > 0)
		{
			return AssetLocation.Create(shapeLocation + "label-" + getLabelForMeal(recipeCode, contents) + ".json");
		}
		return AssetLocation.Create(shapeLocation + "label-" + (CodeToLabel(contents[0].Collectible.Code) ?? "empty") + ".json");
	}

	public string? CodeToLabel(AssetLocation? loc)
	{
		if (loc == null)
		{
			return null;
		}
		string type = null;
		string[] array = labelNames;
		foreach (string name in array)
		{
			if (loc.Path.Contains(name))
			{
				type = name;
				break;
			}
		}
		return type;
	}

	private string? getLabelForMeal(string recipeCode, ItemStack[] contents)
	{
		if (labelNames.Contains<string>(recipeCode))
		{
			return recipeCode;
		}
		if (recipeCode == "jam")
		{
			CookingRecipeIngredient fruitIngred = api.GetCookingRecipe("jam").Ingredients?.FirstOrDefault((CookingRecipeIngredient i) => i.Code == "fruit");
			ItemStack fruit = null;
			bool match = true;
			foreach (ItemStack stack in contents)
			{
				if (fruitIngred != null && fruitIngred.Matches(stack))
				{
					if (fruit == null)
					{
						fruit = stack;
					}
					else if (fruit.Collectible.Code != stack.Collectible.Code)
					{
						match = false;
					}
				}
			}
			if (match)
			{
				return CodeToLabel(fruit?.Collectible.Code);
			}
		}
		return CodeToLabel(getMostCommonMealIngredient(contents)) ?? "meal";
	}

	private AssetLocation? getMostCommonMealIngredient(ItemStack[] contents)
	{
		Dictionary<AssetLocation, int> sdf = new Dictionary<AssetLocation, int>();
		foreach (ItemStack stack in contents)
		{
			sdf.TryGetValue(stack.Collectible.Code, out var cnt);
			sdf[stack.Collectible.Code] = 1 + cnt;
		}
		AssetLocation key = sdf.Aggregate((KeyValuePair<AssetLocation, int> l, KeyValuePair<AssetLocation, int> r) => (l.Value <= r.Value) ? r : l).Key;
		if (sdf[key] < 3)
		{
			return null;
		}
		return key;
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
	{
		ItemStack stack = new ItemStack(world.GetBlock(CodeWithVariant("side", "east")));
		if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityCrock becrock)
		{
			ItemStack[] contents = becrock.GetContentStacks();
			for (int i = 0; i < contents.Length; i++)
			{
				if (contents[i] != null)
				{
					SetContents(stack, contents);
					if (becrock.RecipeCode != null)
					{
						stack.Attributes.SetString("recipeCode", becrock.RecipeCode);
						stack.Attributes.SetFloat("quantityServings", becrock.QuantityServings);
						stack.Attributes.SetBool("sealed", becrock.Sealed);
					}
				}
			}
		}
		return stack;
	}

	public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
	{
		bool isSealed = itemstack.Attributes.GetBool("sealed");
		ItemStack[] contents = GetNonEmptyContents(capi.World, itemstack);
		string recipeCode = itemstack.Attributes.GetString("recipeCode");
		AssetLocation loc = LabelForContents(recipeCode, contents);
		if (loc == null)
		{
			return;
		}
		Dictionary<string, MultiTextureMeshRef> meshrefs = ObjectCacheUtil.GetOrCreate(capi, "blockcrockGuiMeshRefs", () => new Dictionary<string, MultiTextureMeshRef>());
		string key = Code.ToShortString() + loc.ToShortString() + (isSealed ? "sealed" : "");
		if (!meshrefs.TryGetValue(key, out var meshref))
		{
			MeshData mesh = GenMesh(capi, loc, new Vec3f(0f, 270f, 0f));
			if (isSealed)
			{
				MeshData sealMesh = GenSealMesh(capi);
				if (sealMesh != null)
				{
					mesh.AddMeshData(sealMesh);
				}
			}
			meshref = (meshrefs[key] = capi.Render.UploadMultiTextureMesh(mesh));
		}
		renderinfo.ModelRef = meshref;
	}

	public virtual string GetMeshCacheKey(ItemSlot slot)
	{
		ItemStack itemstack = slot.Itemstack;
		bool isSealed = itemstack.Attributes.GetBool("sealed");
		ItemStack[] contents = GetNonEmptyContents(api.World, itemstack);
		string recipeCode = itemstack.Attributes.GetString("recipeCode");
		AssetLocation loc = LabelForContents(recipeCode, contents);
		return Code.ToShortString() + loc.ToShortString() + (isSealed ? "sealed" : "");
	}

	public MeshData GenMesh(ItemSlot slot, ITextureAtlasAPI targetAtlas, BlockPos? forBlockPos = null)
	{
		ItemStack itemstack = slot.Itemstack;
		ICoreClientAPI capi = (ICoreClientAPI)api;
		ItemStack[] contents = GetNonEmptyContents(api.World, itemstack);
		string recipeCode = itemstack.Attributes.GetString("recipeCode");
		MeshData mesh = GenMesh(capi, LabelForContents(recipeCode, contents));
		if (itemstack.Attributes.GetBool("sealed"))
		{
			MeshData sealMesh = GenSealMesh(capi);
			if (sealMesh != null)
			{
				mesh.AddMeshData(sealMesh);
			}
		}
		return mesh;
	}

	public MeshData GenMesh(ICoreClientAPI capi, AssetLocation labelLoc, Vec3f? rot = null)
	{
		ITesselatorAPI tesselator = capi.Tesselator;
		Shape baseshape = Vintagestory.API.Common.Shape.TryGet(capi, AssetLocation.Create(shapeLocation + "base.json"));
		Shape labelshape = Vintagestory.API.Common.Shape.TryGet(capi, labelLoc);
		tesselator.TesselateShape(this, baseshape, out var mesh, rot);
		if (labelshape != null)
		{
			tesselator.TesselateShape(this, labelshape, out var labelmesh, rot);
			mesh.AddMeshData(labelmesh);
		}
		return mesh;
	}

	public MeshData? GenSealMesh(ICoreClientAPI capi)
	{
		ITesselatorAPI tesselator = capi.Tesselator;
		Shape shape = Vintagestory.API.Common.Shape.TryGet(capi, AssetLocation.Create(shapeLocation + "seal.json"));
		if (shape == null)
		{
			return null;
		}
		tesselator.TesselateShape(this, shape, out var mesh);
		return mesh;
	}

	public override string HandbookPageCodeForStack(IWorldAccessor world, ItemStack stack)
	{
		ItemStack cleanStack = stack.Clone();
		cleanStack.Attributes.RemoveAttribute("sealed");
		return base.HandbookPageCodeForStack(world, cleanStack);
	}

	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling)
	{
		if (blockSel?.Position == null)
		{
			base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handHandling);
			return;
		}
		ItemStack crockStack = slot.Itemstack;
		if (crockStack == null)
		{
			return;
		}
		Block block = api.World.BlockAccessor.GetBlock(blockSel.Position);
		float quantityServings = (float)crockStack.Attributes.GetDecimal("quantityServings");
		if (block != null && block.Attributes?.IsTrue("mealContainer") == true)
		{
			if (byEntity.Controls.ShiftKey)
			{
				if (quantityServings > 0f)
				{
					ServeIntoBowl(block, blockSel.Position, slot, byEntity.World);
				}
				handHandling = EnumHandHandling.PreventDefault;
			}
			return;
		}
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
		if (block is BlockCookingContainer bcc && crockStack.Attributes.HasAttribute("recipeCode"))
		{
			handHandling = EnumHandHandling.PreventDefault;
			((byEntity as EntityPlayer)?.Player as IClientPlayer)?.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
			string recipeCode = crockStack.Attributes.GetString("recipeCode");
			float movedServings = bcc.PutMeal(blockSel.Position, GetNonEmptyContents(api.World, crockStack), recipeCode, quantityServings);
			quantityServings -= movedServings;
			if (quantityServings > 0f)
			{
				crockStack.Attributes.SetFloat("quantityServings", quantityServings);
				return;
			}
			crockStack.Attributes.RemoveAttribute("recipeCode");
			crockStack.Attributes.RemoveAttribute("quantityServings");
			crockStack.Attributes.RemoveAttribute("contents");
		}
		else if (block is BlockBarrel)
		{
			if (api.World.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityBarrel bebarrel)
			{
				ItemStack[] ownContentStacks = GetNonEmptyContents(api.World, crockStack);
				if (ownContentStacks == null || ownContentStacks.Length == 0)
				{
					ItemSlot sourceSlot = bebarrel.Inventory.FirstOrDefault(delegate(ItemSlot itemSlot)
					{
						ItemStack? itemstack = itemSlot.Itemstack;
						return itemstack != null && itemstack.Collectible.Attributes?.IsTrue("crockable") == true;
					});
					if (sourceSlot != null)
					{
						float servingCapacity = crockStack.Block.Attributes["servingCapacity"].AsFloat(1f);
						float itemsPerServing = (BlockLiquidContainerBase.GetContainableProps(sourceSlot.Itemstack)?.ItemsPerLitre ?? 1f) * 4f;
						ItemStack foodstack = sourceSlot.TakeOut((int)(servingCapacity * itemsPerServing));
						if (foodstack != null)
						{
							float servingSize = (float)foodstack.StackSize / itemsPerServing;
							foodstack.StackSize = (int)itemsPerServing;
							SetContents(null, crockStack, new ItemStack[1] { foodstack }, servingSize);
							bebarrel.MarkDirty(redrawOnClient: true);
							slot.MarkDirty();
						}
					}
				}
				else if (ownContentStacks.Length == 1 && crockStack.Attributes.GetString("recipeCode") == null)
				{
					WaterTightContainableProps liquidProps = BlockLiquidContainerBase.GetContainableProps(ownContentStacks[0]);
					ItemSlot sinkSlot = bebarrel.Inventory[(liquidProps != null) ? 1 : 0];
					float itemsPerServing2 = (liquidProps?.ItemsPerLitre ?? 1f) * 4f;
					ItemStack foodstack2 = ownContentStacks[0].Clone();
					foodstack2.StackSize = (int)(quantityServings * itemsPerServing2);
					int transfered = new DummySlot(foodstack2).TryPutInto(api.World, sinkSlot, foodstack2.StackSize);
					quantityServings = (quantityServings * itemsPerServing2 - (float)transfered) / itemsPerServing2;
					if (quantityServings <= 0f)
					{
						crockStack.Attributes.RemoveAttribute("recipeCode");
						crockStack.Attributes.RemoveAttribute("quantityServings");
						crockStack.Attributes.RemoveAttribute("contents");
					}
					else
					{
						crockStack.Attributes.SetFloat("quantityServings", quantityServings);
					}
					crockStack.Attributes.RemoveAttribute("sealed");
					bebarrel.MarkDirty(redrawOnClient: true);
					slot.MarkDirty();
				}
			}
			handHandling = EnumHandHandling.PreventDefault;
			((byEntity as EntityPlayer)?.Player as IClientPlayer)?.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
		}
		else
		{
			base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handHandling);
		}
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		ItemSlot hotbarSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
		if (!hotbarSlot.Empty)
		{
			JsonObject attributes = hotbarSlot.Itemstack.Collectible.Attributes;
			if (attributes != null && attributes.IsTrue("mealContainer") && (!(hotbarSlot.Itemstack.Collectible is BlockCrock) || hotbarSlot.StackSize == 1))
			{
				if (!(world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityCrock bec))
				{
					return false;
				}
				if (hotbarSlot.Itemstack.Attributes.GetDecimal("quantityServings") == 0.0)
				{
					bec.ServeInto(byPlayer, hotbarSlot);
					return true;
				}
				if (bec.QuantityServings == 0f)
				{
					ServeIntoBowl(this, blockSel.Position, hotbarSlot, world);
					bec.Sealed = false;
					bec.MarkDirty(redrawOnClient: true);
					return true;
				}
			}
		}
		return base.OnBlockInteractStart(world, byPlayer, blockSel);
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		ItemStack crockStack = inSlot.Itemstack;
		if (crockStack == null)
		{
			return;
		}
		CookingRecipe recipe = GetCookingRecipe(world, crockStack);
		ItemStack[] stacks = GetNonEmptyContents(world, crockStack);
		if (stacks == null || stacks.Length == 0)
		{
			dsc.AppendLine(Lang.Get("Empty"));
			if (crockStack.Attributes.GetBool("sealed"))
			{
				dsc.AppendLine("<font color=\"lightgreen\">" + Lang.Get("Sealed.") + "</font>");
			}
			return;
		}
		DummyInventory dummyInv = new DummyInventory(api);
		ItemSlot dummySlot = GetDummySlotForFirstPerishableStack(api.World, stacks, null, dummyInv);
		dummyInv.OnAcquireTransitionSpeed += delegate(EnumTransitionType transType, ItemStack stack, float mul)
		{
			float num = inSlot.Inventory?.GetTransitionSpeedMul(transType, crockStack) ?? 1f;
			if (transType != EnumTransitionType.Perish)
			{
				num = 0f;
			}
			return num * GetContainingTransitionModifierContained(world, inSlot, transType);
		};
		if (recipe != null)
		{
			double servings = crockStack.Attributes.GetDecimal("quantityServings");
			if (recipe != null)
			{
				if (servings == 1.0)
				{
					dsc.AppendLine(Lang.Get("{0} serving of {1}", Math.Round(servings, 1), recipe.GetOutputName(world, stacks)));
				}
				else
				{
					dsc.AppendLine(Lang.Get("{0} servings of {1}", Math.Round(servings, 1), recipe.GetOutputName(world, stacks)));
				}
			}
			BlockMeal[]? array = BlockMeal.AllMealBowls(api);
			string facts = ((array == null) ? null : array[0]?.GetContentNutritionFacts(world, inSlot, null));
			if (facts != null)
			{
				dsc.Append(facts);
			}
		}
		else if (crockStack.Attributes.HasAttribute("quantityServings"))
		{
			double servings2 = crockStack.Attributes.GetDecimal("quantityServings");
			if (Math.Round(servings2, 1) < 0.05)
			{
				dsc.AppendLine(Lang.Get("meal-servingsleft-percent", Math.Round(servings2 * 100.0, 0)));
			}
			else
			{
				dsc.AppendLine(Lang.Get("{0} servings left", Math.Round(servings2, 1)));
			}
		}
		else if (!MealMeshCache.ContentsRotten(stacks))
		{
			dsc.AppendLine(Lang.Get("Contents: {0}", Lang.Get("meal-ingredientlist-" + stacks.Length, stacks.Select((ItemStack stack) => Lang.Get("{0}x {1}", stack.StackSize, stack.GetName())))));
		}
		TransitionState perishState = dummySlot.Itemstack?.Collectible.UpdateAndGetTransitionStates(api.World, dummySlot)?.FirstOrDefault((TransitionState state) => state.Props.Type == EnumTransitionType.Perish);
		if (perishState != null)
		{
			dummySlot.Itemstack?.Collectible.AppendPerishableInfoText(dummySlot, dsc, world, perishState, nowSpoiling: false);
		}
		if (crockStack.Attributes.GetBool("sealed"))
		{
			dsc.AppendLine("<font color=\"lightgreen\">" + Lang.Get("Sealed.") + "</font>");
		}
	}

	public override string GetHeldItemName(ItemStack? itemStack)
	{
		if (IsEmpty(itemStack))
		{
			return base.GetHeldItemName(itemStack);
		}
		ItemStack[] contentStacks = GetContents(api.World, itemStack);
		string recipeCode = itemStack?.Collectible.GetCollectibleInterface<IBlockMealContainer>()?.GetRecipeCode(api.World, itemStack);
		string code = Lang.Get("mealrecipe-name-" + recipeCode + "-in-container");
		if (recipeCode == null)
		{
			code = ((!MealMeshCache.ContentsRotten(contentStacks)) ? contentStacks[0].GetName() : Lang.Get("Rotten Food"));
		}
		AssetLocation loc = CodeWithVariant("type", "meal");
		return Lang.GetMatching(loc.Domain + ":block-" + loc.Path, code);
	}

	public override string GetPlacedBlockInfo(IWorldAccessor world, BlockPos pos, IPlayer forPlayer)
	{
		if (!(world.BlockAccessor.GetBlockEntity(pos) is BlockEntityCrock becrock))
		{
			return "";
		}
		CookingRecipe recipe = api.GetCookingRecipe(becrock.RecipeCode);
		ItemStack[] stacks = (from itemSlot in becrock.inventory
			where !itemSlot.Empty
			select itemSlot.Itemstack).ToArray();
		if (stacks.Length == 0)
		{
			return Lang.Get("Empty");
		}
		StringBuilder dsc = new StringBuilder();
		if (recipe != null)
		{
			ItemSlot slot = GetDummySlotForFirstPerishableStack(api.World, stacks, forPlayer.Entity, becrock.inventory);
			if (recipe != null)
			{
				if (becrock.QuantityServings == 1f)
				{
					dsc.AppendLine(Lang.Get("{0} serving of {1}", Math.Round(becrock.QuantityServings, 1), recipe.GetOutputName(world, stacks)));
				}
				else
				{
					dsc.AppendLine(Lang.Get("{0} servings of {1}", Math.Round(becrock.QuantityServings, 1), recipe.GetOutputName(world, stacks)));
				}
			}
			BlockMeal[]? array = BlockMeal.AllMealBowls(api);
			string facts = ((array == null) ? null : array[0]?.GetContentNutritionFacts(world, new DummySlot(OnPickBlock(world, pos)), null));
			if (facts != null)
			{
				dsc.Append(facts);
			}
			slot.Itemstack?.Collectible.AppendPerishableInfoText(slot, dsc, world);
		}
		else
		{
			dsc.AppendLine("Contents:");
			ItemStack[] array2 = stacks;
			foreach (ItemStack stack in array2)
			{
				if (stack != null)
				{
					dsc.AppendLine(stack.StackSize + "x  " + stack.GetName());
				}
			}
			becrock.inventory[0].Itemstack?.Collectible.AppendPerishableInfoText(becrock.inventory[0], dsc, api.World);
		}
		if (becrock.Sealed)
		{
			dsc.AppendLine("<font color=\"lightgreen\">" + Lang.Get("Sealed.") + "</font>");
		}
		return dsc.ToString();
	}

	public static ItemSlot GetDummySlotForFirstPerishableStack(IWorldAccessor world, ItemStack[]? stacks, Entity? forEntity, InventoryBase slotInventory)
	{
		ItemStack stack = null;
		if (stacks != null)
		{
			for (int i = 0; i < stacks.Length; i++)
			{
				if (stacks[i] != null)
				{
					TransitionableProperties[] props = stacks[i].Collectible.GetTransitionableProperties(world, stacks[i], forEntity);
					if (props != null && props.Length != 0)
					{
						stack = stacks[i];
						break;
					}
				}
			}
		}
		DummySlot dummySlot = new DummySlot(stack, slotInventory);
		dummySlot.MarkedDirty += () => true;
		return dummySlot;
	}

	public override TransitionState[]? UpdateAndGetTransitionStates(IWorldAccessor world, ItemSlot inslot)
	{
		ItemStack crockStack = inslot.Itemstack;
		if (crockStack == null)
		{
			return null;
		}
		ItemStack[] stacks = GetNonEmptyContents(world, crockStack);
		ItemStack[] array = stacks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].StackSize *= (int)Math.Max(1f, crockStack.Attributes.TryGetFloat("quantityServings") ?? 1f);
		}
		SetContents(crockStack, stacks);
		TransitionState[] states = base.UpdateAndGetTransitionStates(world, inslot);
		stacks = GetNonEmptyContents(world, crockStack);
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
			SetContents(crockStack, stacks);
			crockStack.Attributes.RemoveAttribute("recipeCode");
			crockStack.Attributes.RemoveAttribute("quantityServings");
		}
		array = stacks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].StackSize /= (int)Math.Max(1f, crockStack.Attributes.TryGetFloat("quantityServings") ?? 1f);
		}
		SetContents(crockStack, stacks);
		return states;
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
		if (!MealMeshCache.ContentsRotten(stacks))
		{
			return;
		}
		for (int i = 0; i < stacks.Length; i++)
		{
			if (stacks[i] != null && stacks[i].StackSize > 0 && stacks[i].Collectible.Code.Path == "rot")
			{
				world.SpawnItemEntity(stacks[i], entityItem.Pos.XYZ);
			}
		}
		entityItem.Itemstack.Attributes.RemoveAttribute("sealed");
		entityItem.Itemstack.Attributes.RemoveAttribute("recipeCode");
		entityItem.Itemstack.Attributes.RemoveAttribute("quantityServings");
		entityItem.Itemstack.Attributes.RemoveAttribute("contents");
		entityItem.MarkShapeModified();
	}

	public override void OnUnloaded(ICoreAPI api)
	{
		if (!(api is ICoreClientAPI capi) || !capi.ObjectCache.TryGetValue("blockcrockGuiMeshRefs", out var obj))
		{
			return;
		}
		if (obj is Dictionary<string, MultiTextureMeshRef> meshrefs)
		{
			foreach (KeyValuePair<string, MultiTextureMeshRef> item in meshrefs)
			{
				item.Value.Dispose();
			}
		}
		capi.ObjectCache.Remove("blockcrockGuiMeshRefs");
	}

	public bool IsFullAndUnsealed(ItemStack stack)
	{
		if (!IsEmpty(stack))
		{
			return !stack.Attributes.GetBool("sealed");
		}
		return false;
	}

	public override int GetMergableQuantity(ItemStack sinkStack, ItemStack sourceStack, EnumMergePriority priority)
	{
		if (priority == EnumMergePriority.DirectMerge)
		{
			JsonObject itemAttributes = sourceStack.ItemAttributes;
			if (itemAttributes != null && itemAttributes["canSealCrock"]?.AsBool() == true && IsFullAndUnsealed(sinkStack))
			{
				return 1;
			}
		}
		return base.GetMergableQuantity(sinkStack, sourceStack, priority);
	}

	public override void TryMergeStacks(ItemStackMergeOperation op)
	{
		ItemSlot sourceSlot = op.SourceSlot;
		if (op.CurrentPriority == EnumMergePriority.DirectMerge)
		{
			JsonObject itemAttributes = sourceSlot.Itemstack.ItemAttributes;
			if (itemAttributes != null && itemAttributes["canSealCrock"]?.AsBool() == true)
			{
				ItemSlot sinkSlot = op.SinkSlot;
				if (IsFullAndUnsealed(sinkSlot.Itemstack))
				{
					sinkSlot.Itemstack.Attributes.SetBool("sealed", value: true);
					op.MovedQuantity = 1;
					sourceSlot.TakeOut(1);
					sinkSlot.MarkDirty();
				}
				return;
			}
		}
		base.TryMergeStacks(op);
	}

	public override bool OnContainedInteractStart(BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		ItemSlot handSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
		ItemStack? itemstack = handSlot.Itemstack;
		if (itemstack != null && itemstack.Collectible.Attributes?["canSealCrock"]?.AsBool() == true)
		{
			if (IsFullAndUnsealed(slot.Itemstack))
			{
				slot.Itemstack.Attributes.SetBool("sealed", value: true);
				(be as BlockEntityDisplay)?.MarkMeshesDirty();
				slot.MarkDirty();
				handSlot.TakeOut(1);
				handSlot.MarkDirty();
				be.MarkDirty();
			}
			else
			{
				(api as ICoreClientAPI)?.TriggerIngameError(this, "crockemptyorsealed", Lang.Get("ingameerror-crock-empty-or-sealed"));
			}
			return true;
		}
		return base.OnContainedInteractStart(be, slot, byPlayer, blockSel);
	}

	public override WorldInteraction[] GetContainedInteractionHelp(BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		WorldInteraction[] baseInteractions = base.GetContainedInteractionHelp(be, slot, byPlayer, blockSel);
		if (baseInteractions.Length == 0 || IsEmpty(slot.Itemstack))
		{
			return Array.Empty<WorldInteraction>();
		}
		ItemStack? itemstack = slot.Itemstack;
		if (itemstack != null && itemstack.Attributes.GetBool("sealed"))
		{
			return baseInteractions;
		}
		WorldInteraction[] array = baseInteractions;
		int num = 0;
		WorldInteraction[] array2 = new WorldInteraction[1 + array.Length];
		ReadOnlySpan<WorldInteraction> readOnlySpan = new ReadOnlySpan<WorldInteraction>(array);
		readOnlySpan.CopyTo(new Span<WorldInteraction>(array2).Slice(num, readOnlySpan.Length));
		num += readOnlySpan.Length;
		array2[num] = new WorldInteraction
		{
			ActionLangCode = "blockhelp-crock-seal",
			MouseButton = EnumMouseButton.Right,
			Itemstacks = ObjectCacheUtil.TryGet<ItemStack[]>(api, "crocksealitems")
		};
		return array2;
	}
}
