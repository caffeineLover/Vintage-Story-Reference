using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockPie : BlockMeal, IBakeableCallback, IShelvable
{
	private MealMeshCache? ms;

	private WorldInteraction[]? interactions;

	public static PieTopCrustType[] TopCrustTypes;

	private ModelTransform oneSliceTranformGui = new ModelTransform
	{
		Origin = new FastVec3f(0.375f, 0.1f, 0.375f),
		Scale = 2.82f,
		Rotation = new FastVec3f(-27f, 132f, -5f)
	}.EnsureDefaultValues();

	private ModelTransform oneSliceTranformTp = new ModelTransform
	{
		Translation = new FastVec3f(-0.82f, -0.34f, -0.57f),
		Origin = new FastVec3f(0.5f, 0.13f, 0.5f),
		Scale = 0.7f,
		Rotation = new FastVec3f(-49f, 29f, -112f)
	}.EnsureDefaultValues();

	public string State => Variant["state"];

	protected override bool PlacedBlockEating => false;

	public EnumShelvableLayout? GetShelvableType(ItemStack stack)
	{
		return stack.Attributes.GetAsInt("pieSize") switch
		{
			1 => EnumShelvableLayout.Quadrants, 
			2 => EnumShelvableLayout.Halves, 
			_ => EnumShelvableLayout.SingleCenter, 
		};
	}

	public ModelTransform? GetOnShelfTransform(ItemStack stack)
	{
		return GetShelvableType(stack) switch
		{
			EnumShelvableLayout.Quadrants => stack.Collectible.Attributes?["onShelfQuarterTransform"].AsObject<ModelTransform>(), 
			EnumShelvableLayout.Halves => stack.Collectible.Attributes?["onShelfHalfTransform"].AsObject<ModelTransform>(), 
			_ => stack.Collectible.Attributes?["onShelfFullTransform"].AsObject<ModelTransform>(), 
		};
	}

	[MemberNotNull(new string[] { "ms", "interactions" })]
	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		if (TopCrustTypes == null)
		{
			TopCrustTypes = api.Assets.Get("config/pietopcrusttypes.json").ToObject<PieTopCrustType[]>();
		}
		InteractionHelpYOffset = 0.375f;
		interactions = ObjectCacheUtil.GetOrCreate(api, "pieInteractions-", delegate
		{
			ItemStack[] toolStacks = ObjectCacheUtil.GetToolStacks(api, EnumTool.Knife);
			List<ItemStack> list = new List<ItemStack>();
			List<ItemStack> list2 = new List<ItemStack>();
			if (list.Count == 0 && list2.Count == 0)
			{
				foreach (CollectibleObject current in api.World.Collectibles)
				{
					if (current is ItemDough)
					{
						list2.Add(new ItemStack(current, 2));
					}
					JsonObject attributes = current.Attributes;
					if (attributes != null && attributes["inPieProperties"].AsObject<InPieProperties>(null, current.Code.Domain)?.PartType == EnumPiePartType.Filling)
					{
						list.Add(new ItemStack(current, 2));
					}
				}
			}
			return new WorldInteraction[4]
			{
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-pie-cut",
					MouseButton = EnumMouseButton.Right,
					Itemstacks = toolStacks,
					GetMatchingStacks = delegate(WorldInteraction wi, BlockSelection bs, EntitySelection _)
					{
						BlockEntityPie blockEntity = GetBlockEntity<BlockEntityPie>(bs.Position);
						if (blockEntity != null)
						{
							string state = blockEntity.State;
							if (state != null && !(state == "raw") && blockEntity.SlicesLeft > 1)
							{
								return wi.Itemstacks;
							}
						}
						return (ItemStack[])null;
					}
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-pie-addfilling",
					MouseButton = EnumMouseButton.Right,
					Itemstacks = list.ToArray(),
					GetMatchingStacks = delegate(WorldInteraction wi, BlockSelection bs, EntitySelection _)
					{
						BlockEntityPie blockEntity = GetBlockEntity<BlockEntityPie>(bs.Position);
						return (blockEntity == null || !(blockEntity.State == "raw") || blockEntity.HasAllFilling) ? null : wi.Itemstacks;
					}
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-pie-addcrust",
					MouseButton = EnumMouseButton.Right,
					Itemstacks = list2.ToArray(),
					GetMatchingStacks = delegate(WorldInteraction wi, BlockSelection bs, EntitySelection _)
					{
						BlockEntityPie blockEntity = GetBlockEntity<BlockEntityPie>(bs.Position);
						return (blockEntity == null || !(blockEntity.State == "raw") || !blockEntity.HasAllFilling || blockEntity.HasCrust) ? null : wi.Itemstacks;
					}
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-pie-changecruststyle",
					MouseButton = EnumMouseButton.Right,
					Itemstacks = toolStacks,
					GetMatchingStacks = delegate(WorldInteraction wi, BlockSelection bs, EntitySelection _)
					{
						BlockEntityPie blockEntity = GetBlockEntity<BlockEntityPie>(bs.Position);
						return (blockEntity == null || !(blockEntity.State == "raw") || !blockEntity.HasCrust) ? null : wi.Itemstacks;
					}
				}
			};
		});
		ms = api.ModLoader.GetModSystem<MealMeshCache>();
		displayContentsInfo = false;
	}

	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling)
	{
		if (canEat(slot))
		{
			base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handHandling);
		}
	}

	public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		if (!canEat(slot))
		{
			return false;
		}
		return base.OnHeldInteractStep(secondsUsed, slot, byEntity, blockSel, entitySel);
	}

	public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		if (canEat(slot))
		{
			base.OnHeldInteractStop(secondsUsed, slot, byEntity, blockSel, entitySel);
		}
	}

	protected bool canEat(ItemSlot slot)
	{
		ItemStack? itemstack = slot.Itemstack;
		if (itemstack != null && itemstack.Attributes?.GetAsInt("pieSize") == 1)
		{
			return State != "raw";
		}
		return false;
	}

	public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
	{
		base.OnBeforeRender(capi, itemstack, target, ref renderinfo);
		if (itemstack.Attributes.GetAsInt("pieSize") == 1)
		{
			if (target == EnumItemRenderTarget.Gui)
			{
				renderinfo.Transform = oneSliceTranformGui;
			}
			if (target == EnumItemRenderTarget.HandTp)
			{
				renderinfo.Transform = oneSliceTranformTp;
			}
		}
		renderinfo.ModelRef = ms.GetOrCreatePieMeshRef(itemstack);
	}

	public override MeshData? GenMesh(ItemSlot slot, ITextureAtlasAPI targetAtlas, BlockPos? atBlockPos = null)
	{
		return ms.GetPieMesh(slot.Itemstack);
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
	{
		ItemStack pieStack = (world.BlockAccessor.GetBlockEntity(pos) as BlockEntityPie)?.Inventory[0].Itemstack;
		if (pieStack != null)
		{
			return pieStack.Clone();
		}
		return base.OnPickBlock(world, pos);
	}

	public void OnBaked(ItemStack oldStack, ItemStack newStack)
	{
		newStack.Attributes["contents"] = oldStack.Attributes["contents"];
		newStack.Attributes.SetInt("pieSize", oldStack.Attributes.GetAsInt("pieSize"));
		newStack.Attributes.SetString("topCrustType", GetTopCrustType(oldStack));
		newStack.Attributes.SetInt("bakeLevel", oldStack.Attributes.GetAsInt("bakeLevel") + 1);
		ItemStack[] stacks = GetContents(api.World, newStack);
		SetContents(newStack, stacks);
	}

	public void TryPlacePie(EntityAgent byEntity, BlockSelection blockSel)
	{
		IPlayer byPlayer = (byEntity as EntityPlayer)?.Player;
		InPieProperties? obj = (byPlayer?.InventoryManager.ActiveHotbarSlot)?.Itemstack?.ItemAttributes["inPieProperties"]?.AsObject<InPieProperties>();
		if (obj != null && obj.PartType == EnumPiePartType.Crust)
		{
			BlockPos abovePos = blockSel.Position.UpCopy();
			if (api.World.BlockAccessor.GetBlock(abovePos).Replaceable >= 6000)
			{
				api.World.BlockAccessor.SetBlock(Id, abovePos);
				(api.World.BlockAccessor.GetBlockEntity(abovePos) as BlockEntityPie)?.OnPlaced(byPlayer);
			}
		}
	}

	public override string GetPlacedBlockName(IWorldAccessor world, BlockPos pos)
	{
		ItemStack pieStack = (world.BlockAccessor.GetBlockEntity(pos) as BlockEntityPie)?.Inventory[0]?.Itemstack;
		if (pieStack != null)
		{
			return GetHeldItemName(pieStack);
		}
		return base.GetPlacedBlockName(world, pos);
	}

	public override string GetHeldItemName(ItemStack? itemStack)
	{
		ItemStack[] cStacks = GetContents(api.World, itemStack);
		if (cStacks.Length <= 1)
		{
			return Lang.Get("pie-empty");
		}
		ItemStack cstack = cStacks[1];
		EnumFoodCategory[] foodCats = cStacks.Select(FillingFoodCategory).ToArray();
		EnumFoodCategory foodCat = foodCats[1];
		if (cstack == null)
		{
			return Lang.Get("pie-empty");
		}
		bool equal = true;
		bool foodCatEquals = true;
		IEnumerable<string> mixCodes = cstack.ItemAttributes["inPieProperties"].AsObject<InPieProperties>()?.MixingCodes ?? Array.Empty<string>();
		int i = 2;
		while ((equal || foodCatEquals || mixCodes.Any()) && i < cStacks.Length - 1)
		{
			if (cStacks[i] != null)
			{
				equal &= cstack.Equals(api.World, cStacks[i], GlobalConstants.IgnoredStackAttributes);
				foodCatEquals &= cStacks[i] == null || foodCats[i] == foodCat;
				mixCodes = cstack.ItemAttributes["inPieProperties"].AsObject<InPieProperties>()?.MixingCodes.Intersect(mixCodes) ?? Array.Empty<string>();
				cstack = cStacks[i];
				foodCat = foodCats[i];
			}
			i++;
		}
		string state = Variant["state"];
		if (MealMeshCache.ContentsRotten(cStacks))
		{
			return Lang.Get("pie-single-rotten");
		}
		if (equal)
		{
			return Lang.Get("pie-single-" + cstack.Collectible.Code.ToShortString() + "-" + state);
		}
		if (!foodCatEquals && mixCodes.Any())
		{
			return Lang.Get("pie-mixed-" + mixCodes.First() + "-" + state);
		}
		return Lang.Get("pie-mixed-" + FillingFoodCategory(cStacks[1]).ToString().ToLowerInvariant() + "-" + state);
	}

	public static EnumFoodCategory FillingFoodCategory(ItemStack? stack)
	{
		if (stack == null)
		{
			return EnumFoodCategory.Vegetable;
		}
		EnumFoodCategory? category = stack.ItemAttributes["inPieProperties"].AsObject<InPieProperties>()?.FoodCategory;
		if (!category.HasValue)
		{
			WaterTightContainableProps liquidProps = BlockLiquidContainerBase.GetContainableProps(stack);
			if (liquidProps != null)
			{
				FoodNutritionProperties nutritionPropsPerLitreWhenInMeal = liquidProps.NutritionPropsPerLitreWhenInMeal;
				category = ((nutritionPropsPerLitreWhenInMeal != null) ? new EnumFoodCategory?(nutritionPropsPerLitreWhenInMeal.FoodCategory) : liquidProps.NutritionPropsPerLitre?.FoodCategory);
			}
		}
		EnumFoodCategory? enumFoodCategory = category;
		if (!enumFoodCategory.HasValue)
		{
			category = stack.ItemAttributes?["nutritionPropsWhenInMeal"]?.AsObject<FoodNutritionProperties>()?.FoodCategory ?? stack.Collectible.GetNutritionProperties(null, stack, null)?.FoodCategory;
		}
		return category ?? EnumFoodCategory.Vegetable;
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		ItemStack pieStack = inSlot.Itemstack;
		if (pieStack != null)
		{
			int pieSize = pieStack.Attributes.GetAsInt("pieSize");
			float servingsLeft = GetQuantityServings(world, pieStack);
			if (!pieStack.Attributes.HasAttribute("quantityServings"))
			{
				servingsLeft = 1f;
			}
			if (pieSize == 1)
			{
				dsc.AppendLine(Lang.Get("pie-slice-single", servingsLeft));
			}
			else
			{
				dsc.AppendLine(Lang.Get("pie-slices", pieSize));
			}
			TransitionableProperties[] propsm = pieStack.Collectible.GetTransitionableProperties(api.World, pieStack, null);
			if (propsm != null && propsm.Length != 0)
			{
				pieStack.Collectible.AppendPerishableInfoText(inSlot, dsc, api.World);
			}
			ItemStack[] stacks = GetContents(api.World, pieStack);
			EntityPlayer forEntity = (world as IClientWorldAccessor)?.Player?.Entity;
			float[] nmul = GetNutritionHealthMul(null, inSlot, forEntity);
			dsc.AppendLine(GetContentNutritionFacts(api.World, inSlot, stacks, null, mulWithStacksize: true, servingsLeft * nmul[0], servingsLeft * nmul[1]));
		}
	}

	public override string GetPlacedBlockInfo(IWorldAccessor world, BlockPos pos, IPlayer forPlayer)
	{
		InventoryBase bepInv = (world.BlockAccessor.GetBlockEntity(pos) as BlockEntityPie)?.Inventory;
		if (bepInv != null && bepInv.Count >= 1)
		{
			ItemStack pieStack = bepInv[0].Itemstack;
			if (pieStack != null)
			{
				ItemStack[] stacks = GetContents(api.World, pieStack);
				StringBuilder sb = new StringBuilder();
				TransitionableProperties[] propsm = pieStack.Collectible.GetTransitionableProperties(api.World, pieStack, null);
				if (propsm != null && propsm.Length != 0)
				{
					pieStack.Collectible.AppendPerishableInfoText(bepInv[0], sb, api.World);
				}
				float servingsLeft = GetQuantityServings(world, pieStack);
				ITreeAttribute attributes = pieStack.Attributes;
				if (attributes != null && !attributes.HasAttribute("quantityServings"))
				{
					servingsLeft = (float)pieStack.Attributes.GetAsInt("pieSize") / 4f;
				}
				float[] nmul = GetNutritionHealthMul(pos, null, forPlayer.Entity);
				return sb.ToString() + GetContentNutritionFacts(api.World, bepInv[0], stacks, null, mulWithStacksize: true, nmul[0] * servingsLeft, nmul[1] * servingsLeft);
			}
		}
		return "";
	}

	public override TransitionState? UpdateAndGetTransitionState(IWorldAccessor world, ItemSlot inslot, EnumTransitionType type)
	{
		ItemStack[] cstacks = GetContents(world, inslot.Itemstack);
		UnspoilContents(world, cstacks);
		SetContents(inslot.Itemstack, cstacks);
		return base.UpdateAndGetTransitionState(world, inslot, type);
	}

	public override TransitionState[]? UpdateAndGetTransitionStates(IWorldAccessor world, ItemSlot inslot)
	{
		ItemStack[] cstacks = GetContents(world, inslot.Itemstack);
		UnspoilContents(world, cstacks);
		SetContents(inslot.Itemstack, cstacks);
		return base.UpdateAndGetTransitionStatesNative(world, inslot);
	}

	public override string GetContentNutritionFacts(IWorldAccessor world, ItemSlot inSlotorFirstSlot, ItemStack[] contentStacks, EntityAgent? forEntity, bool mulWithStacksize = false, float nutritionMul = 1f, float healthMul = 1f)
	{
		UnspoilContents(world, contentStacks);
		return base.GetContentNutritionFacts(world, inSlotorFirstSlot, contentStacks, forEntity, mulWithStacksize, nutritionMul, healthMul);
	}

	protected void UnspoilContents(IWorldAccessor world, ItemStack[] cstacks)
	{
		foreach (ItemStack cstack in cstacks)
		{
			if (cstack == null)
			{
				continue;
			}
			if (!(cstack.Attributes["transitionstate"] is ITreeAttribute))
			{
				cstack.Attributes["transitionstate"] = new TreeAttribute();
			}
			ITreeAttribute attr = (ITreeAttribute)cstack.Attributes["transitionstate"];
			if (attr.HasAttribute("createdTotalHours"))
			{
				attr.SetDouble("createdTotalHours", world.Calendar.TotalHours);
				attr.SetDouble("lastUpdatedTotalHours", world.Calendar.TotalHours);
				float[] transitionedHours = (attr["transitionedHours"] as FloatArrayAttribute)?.value;
				int j = 0;
				while (transitionedHours != null && j < transitionedHours.Length)
				{
					transitionedHours[j] = 0f;
					j++;
				}
			}
		}
	}

	public override float[] GetNutritionHealthMul(BlockPos? pos, ItemSlot? slot, EntityAgent? forEntity)
	{
		float satLossMul = 1f;
		if (slot == null && pos != null)
		{
			slot = (api.World.BlockAccessor.GetBlockEntity(pos) as BlockEntityPie)?.Inventory[0];
		}
		if (slot != null)
		{
			satLossMul = GlobalConstants.FoodSpoilageSatLossMul((slot.Itemstack?.Collectible.UpdateAndGetTransitionState(api.World, slot, EnumTransitionType.Perish))?.TransitionLevel ?? 0f, slot.Itemstack, forEntity);
		}
		return new float[2]
		{
			Attributes["nutritionMul"].AsFloat(1f) * satLossMul,
			satLossMul
		};
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		BlockEntityPie blockEntity = GetBlockEntity<BlockEntityPie>(blockSel.Position);
		if (blockEntity == null || !blockEntity.OnInteract(byPlayer))
		{
			return base.OnBlockInteractStart(world, byPlayer, blockSel);
		}
		return true;
	}

	public override void OnBlockInteractStop(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
	}

	public static ItemStack? TakeSlice(ref ItemStack? stack)
	{
		ItemStack outStack = stack?.Clone();
		if (outStack == null)
		{
			return null;
		}
		int size = stack.Attributes.GetAsInt("pieSize");
		float servings = stack.Attributes.GetFloat("quantityServings");
		if (size <= 1 && !outStack.Attributes.HasAttribute("quantityServings"))
		{
			outStack.Attributes.SetFloat("quantityServings", 0.25f);
			stack = null;
		}
		else
		{
			stack.Attributes.SetInt("pieSize", size - 1);
			if (stack.Attributes.HasAttribute("quantityServings"))
			{
				stack.Attributes.SetFloat("quantityServings", servings - 0.25f);
			}
			outStack.Attributes.SetInt("pieSize", 1);
			outStack.Attributes.SetFloat("quantityServings", 0.25f);
		}
		outStack.Attributes.SetBool("bakeable", value: false);
		return outStack;
	}

	public override bool MatchesForCrafting(ItemStack inputStack, IRecipeBase gridRecipe, IRecipeIngredient ingredient)
	{
		if (gridRecipe.Name != (AssetLocation)"pieslice")
		{
			return base.MatchesForCrafting(inputStack, gridRecipe, ingredient);
		}
		if (inputStack?.Collectible is BlockPie)
		{
			return inputStack.Attributes.GetAsInt("pieSize") > 1;
		}
		return false;
	}

	public override void OnCreatedByCrafting(ItemSlot[] allInputslots, ItemSlot outputSlot, IRecipeBase byRecipe)
	{
		if (byRecipe.Name != (AssetLocation)"pieslice")
		{
			base.OnCreatedByCrafting(allInputslots, outputSlot, byRecipe);
		}
		else
		{
			if (outputSlot.Itemstack == null)
			{
				return;
			}
			foreach (ItemSlot slot in allInputslots)
			{
				if (slot.Itemstack?.Collectible is BlockPie)
				{
					ItemStack pieStack = slot.Itemstack.Clone();
					outputSlot.Itemstack = TakeSlice(ref pieStack);
				}
			}
		}
	}

	public override void OnConsumedByCrafting(ItemSlot[] allInputSlots, ItemSlot stackInSlot, IRecipeBase recipe, IRecipeIngredient fromIngredient, IPlayer byPlayer, int quantity)
	{
		if (recipe.Name != (AssetLocation)"pieslice")
		{
			base.OnConsumedByCrafting(allInputSlots, stackInSlot, recipe, fromIngredient, byPlayer, quantity);
			return;
		}
		ItemStack pieStack = stackInSlot.Itemstack.Clone();
		TakeSlice(ref pieStack);
		if (pieStack != null && pieStack.Attributes.GetAsInt("pieSize") == 1)
		{
			pieStack.Attributes.SetFloat("quantityServings", 0.25f);
			pieStack.Attributes.SetBool("bakeable", value: false);
		}
		stackInSlot.Itemstack = pieStack;
	}

	public override int GetRandomContentColor(ICoreClientAPI capi, ItemStack[] stacks)
	{
		if (stacks.Length == 0)
		{
			return 8421504;
		}
		ItemStack[] cstacks = GetContents(capi.World, stacks[0]);
		if (cstacks.Length == 0)
		{
			return 8421504;
		}
		ItemStack rndStack = cstacks[capi.World.Rand.Next(stacks.Length)];
		return rndStack.Collectible.GetRandomColor(capi, rndStack);
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		WorldInteraction[] baseinteractions = base.GetPlacedBlockInteractionHelp(world, selection, forPlayer);
		baseinteractions = baseinteractions.RemoveAt(1);
		return interactions.Append<WorldInteraction>(baseinteractions);
	}

	public static List<CookingRecipe> GetHandbookRecipes(ICoreAPI api, ItemStack[] allStacks)
	{
		List<ItemStack> doughs = new List<ItemStack>();
		Dictionary<EnumFoodCategory, List<ItemStack>> categoryFillings = new Dictionary<EnumFoodCategory, List<ItemStack>>();
		Dictionary<string, List<ItemStack>> mixedFillings = new Dictionary<string, List<ItemStack>>();
		List<ItemStack> crusts = new List<ItemStack>();
		List<ItemStack> noMixFillings = new List<ItemStack>();
		foreach (ItemStack stack in allStacks)
		{
			InPieProperties pieProps = stack.ItemAttributes?["inPieProperties"]?.AsObject<InPieProperties>();
			if (pieProps != null && pieProps.PartType == EnumPiePartType.Crust)
			{
				doughs.Add(stack);
			}
			if (pieProps != null && pieProps.PartType == EnumPiePartType.Filling && pieProps.AllowMixing)
			{
				string[] mixingCodes = pieProps.MixingCodes;
				foreach (string code in mixingCodes)
				{
					if (mixedFillings.TryGetValue(code, out var value))
					{
						value.Add(stack);
						continue;
					}
					int num = 1;
					List<ItemStack> list = new List<ItemStack>(num);
					CollectionsMarshal.SetCount(list, num);
					Span<ItemStack> span = CollectionsMarshal.AsSpan(list);
					int index = 0;
					span[index] = stack;
					mixedFillings.Add(code, list);
				}
				EnumFoodCategory cat = FillingFoodCategory(stack);
				if (cat != EnumFoodCategory.NoNutrition && cat != EnumFoodCategory.Unknown)
				{
					if (categoryFillings.TryGetValue(cat, out var value2))
					{
						value2.Add(stack);
					}
					else
					{
						int j = 1;
						List<ItemStack> list2 = new List<ItemStack>(j);
						CollectionsMarshal.SetCount(list2, j);
						Span<ItemStack> span2 = CollectionsMarshal.AsSpan(list2);
						int index = 0;
						span2[index] = stack;
						categoryFillings.Add(cat, list2);
					}
				}
			}
			if (pieProps != null && pieProps.PartType == EnumPiePartType.Crust && pieProps.AllowMixing)
			{
				crusts.Add(stack);
			}
			if (pieProps != null && !pieProps.AllowMixing)
			{
				string[] mixingCodes = pieProps.MixingCodes;
				if (mixingCodes != null && mixingCodes.Length == 0)
				{
					noMixFillings.Add(stack);
				}
			}
		}
		List<CookingRecipe> list3 = new List<CookingRecipe>();
		list3.AddRange(categoryFillings.Select((KeyValuePair<EnumFoodCategory, List<ItemStack>> entry) => CreateRecipe(api.World, "mixed-" + entry.Key.ToString().ToLowerInvariant(), doughs, entry.Value.ToList(), crusts)));
		list3.AddRange(mixedFillings.Select((KeyValuePair<string, List<ItemStack>> entry) => CreateRecipe(api.World, "mixed-" + entry.Key.ToLowerInvariant(), doughs, entry.Value.ToList(), crusts)));
		list3.AddRange(noMixFillings.Select(delegate(ItemStack itemStack)
		{
			IWorldAccessor world = api.World;
			string code2 = "single-" + itemStack.Collectible.Code.ToShortString();
			List<ItemStack> doughs2 = doughs;
			int num2 = 1;
			List<ItemStack> list4 = new List<ItemStack>(num2);
			CollectionsMarshal.SetCount(list4, num2);
			Span<ItemStack> span3 = CollectionsMarshal.AsSpan(list4);
			int index2 = 0;
			span3[index2] = itemStack;
			return CreateRecipe(world, code2, doughs2, list4, crusts);
		}));
		return list3;
	}

	private static CookingRecipe CreateRecipe(IWorldAccessor world, string code, List<ItemStack> doughs, List<ItemStack> fillings, List<ItemStack> crusts, bool mixedRecipe = false)
	{
		CookingRecipe cookingRecipe = new CookingRecipe();
		cookingRecipe.Code = code;
		cookingRecipe.Ingredients = new CookingRecipeIngredient[3]
		{
			new CookingRecipeIngredient
			{
				Code = "dough",
				TypeName = "bottomcrust",
				MinQuantity = 1,
				MaxQuantity = 1,
				ValidStacks = doughs.Select((ItemStack dough) => new CookingRecipeStack
				{
					Code = dough.Collectible.Code,
					Type = dough.Collectible.ItemClass,
					Quantity = 2,
					ResolvedItemstack = dough.Clone()
				}).ToArray()
			},
			new CookingRecipeIngredient
			{
				Code = "filling",
				TypeName = "piefilling",
				MinQuantity = 4,
				MaxQuantity = 4,
				ValidStacks = fillings.Select((ItemStack filling) => new CookingRecipeStack
				{
					Code = filling.Collectible.Code,
					Type = filling.Collectible.ItemClass,
					Quantity = 2,
					ResolvedItemstack = filling.Clone()
				}).ToArray()
			},
			new CookingRecipeIngredient
			{
				Code = "crust",
				TypeName = "topcrust",
				MinQuantity = 0,
				MaxQuantity = 1,
				ValidStacks = crusts.Select((ItemStack crust) => new CookingRecipeStack
				{
					Code = crust.Collectible.Code,
					Type = crust.Collectible.ItemClass,
					Quantity = 2,
					ResolvedItemstack = crust.Clone()
				}).ToArray()
			}
		};
		cookingRecipe.PerishableProps = new TransitionableProperties();
		return cookingRecipe;
	}

	public static ItemStack?[] GenerateRandomPie(ICoreAPI api, ref Dictionary<CookingRecipeIngredient, HashSet<ItemStack?>>? cachedValidStacksByIngredient, CookingRecipe recipe, ItemStack? ingredientStack = null)
	{
		if (recipe.Ingredients == null)
		{
			return new ItemStack[6];
		}
		Dictionary<CookingRecipeIngredient, HashSet<ItemStack>> validStacksByIngredient = cachedValidStacksByIngredient;
		if (validStacksByIngredient == null)
		{
			validStacksByIngredient = new Dictionary<CookingRecipeIngredient, HashSet<ItemStack>>();
			CookingRecipeIngredient[] ingredients = recipe.Ingredients;
			foreach (CookingRecipeIngredient ingredient in ingredients)
			{
				HashSet<ItemStack> ingredientStacks = new HashSet<ItemStack>();
				ingredient.Resolve(api.World, "handbook meal recipes");
				foreach (ItemStack astack in ingredient.ValidStacks.Select((CookingRecipeStack cookingRecipeStack) => cookingRecipeStack.ResolvedItemstack))
				{
					CookingRecipeStack vstack = ingredient.GetMatchingStack(astack);
					if (vstack != null)
					{
						ItemStack stack = astack.Clone();
						stack.StackSize = vstack.StackSize;
						WaterTightContainableProps props = BlockLiquidContainerBase.GetContainableProps(stack);
						if (props != null)
						{
							stack.StackSize *= (int)(props.ItemsPerLitre * ingredient.PortionSizeLitres);
						}
						ingredientStacks.Add(stack);
					}
				}
				if (ingredient.MinQuantity <= 0)
				{
					ingredientStacks.Add(null);
				}
				validStacksByIngredient.Add(ingredient.Clone(), ingredientStacks);
			}
			cachedValidStacksByIngredient = validStacksByIngredient;
		}
		if (validStacksByIngredient == null)
		{
			return new ItemStack[6];
		}
		List<ItemStack> randomMeal = new List<ItemStack>();
		while (!recipe.Matches(randomMeal.ToArray()))
		{
			Dictionary<CookingRecipeIngredient, List<ItemStack>> valIngStacks = new Dictionary<CookingRecipeIngredient, List<ItemStack>>();
			foreach (KeyValuePair<CookingRecipeIngredient, HashSet<ItemStack>> entry in validStacksByIngredient)
			{
				valIngStacks.Add(entry.Key.Clone(), entry.Value.ToList());
			}
			valIngStacks = valIngStacks.OrderBy((KeyValuePair<CookingRecipeIngredient, List<ItemStack>> x) => api.World.Rand.Next()).ToDictionary((KeyValuePair<CookingRecipeIngredient, List<ItemStack>> item) => item.Key, (KeyValuePair<CookingRecipeIngredient, List<ItemStack>> item) => item.Value);
			CookingRecipeIngredient requestedIngredient = null;
			if (ingredientStack != null)
			{
				List<CookingRecipeIngredient> validIngredients = recipe.Ingredients.Where((CookingRecipeIngredient cookingRecipeIngredient) => cookingRecipeIngredient.Matches(ingredientStack)).ToList();
				requestedIngredient = validIngredients[api.World.Rand.Next(validIngredients.Count)].Clone();
			}
			randomMeal = new List<ItemStack>();
			CookingRecipeIngredient ingredient2 = valIngStacks.FirstOrDefault((KeyValuePair<CookingRecipeIngredient, List<ItemStack>> keyValuePair) => keyValuePair.Key.Code == "dough").Key;
			List<ItemStack> validStacks = valIngStacks.FirstOrDefault((KeyValuePair<CookingRecipeIngredient, List<ItemStack>> keyValuePair) => keyValuePair.Key.Code == "dough").Value;
			if (ingredient2.Code == requestedIngredient?.Code)
			{
				ItemStack stack2 = validStacks.First((ItemStack itemStack) => itemStack?.Collectible.Code == ingredientStack?.Collectible.Code);
				if (stack2 != null)
				{
					randomMeal.Add(stack2.Clone());
					ingredient2.MinQuantity--;
					ingredient2.MaxQuantity--;
				}
				requestedIngredient = null;
			}
			while (ingredient2.MinQuantity > 0)
			{
				randomMeal.Add(validStacks[api.World.Rand.Next(validStacks.Count)]?.Clone());
				ingredient2.MinQuantity--;
				ingredient2.MaxQuantity--;
			}
			ingredient2 = valIngStacks.FirstOrDefault((KeyValuePair<CookingRecipeIngredient, List<ItemStack>> keyValuePair) => keyValuePair.Key.Code == "filling").Key;
			validStacks = valIngStacks.FirstOrDefault((KeyValuePair<CookingRecipeIngredient, List<ItemStack>> keyValuePair) => keyValuePair.Key.Code == "filling").Value;
			if (ingredient2.Code == requestedIngredient?.Code)
			{
				ItemStack stack3 = validStacks.First((ItemStack itemStack) => itemStack?.Collectible.Code == ingredientStack?.Collectible.Code);
				if (stack3 != null)
				{
					randomMeal.Add(stack3.Clone());
					ingredient2.MinQuantity--;
					ingredient2.MaxQuantity--;
				}
				requestedIngredient = null;
			}
			while (ingredient2.MinQuantity > 0)
			{
				randomMeal.Add(validStacks[api.World.Rand.Next(validStacks.Count)]?.Clone());
				ingredient2.MinQuantity--;
				ingredient2.MaxQuantity--;
			}
			ingredient2 = valIngStacks.FirstOrDefault((KeyValuePair<CookingRecipeIngredient, List<ItemStack>> keyValuePair) => keyValuePair.Key.Code == "crust").Key;
			validStacks = valIngStacks.FirstOrDefault((KeyValuePair<CookingRecipeIngredient, List<ItemStack>> keyValuePair) => keyValuePair.Key.Code == "crust").Value;
			if (requestedIngredient != null)
			{
				if (ingredient2.Code == requestedIngredient?.Code)
				{
					ItemStack stack4 = validStacks.First((ItemStack itemStack) => itemStack?.Collectible.Code == ingredientStack?.Collectible.Code);
					if (stack4 != null)
					{
						randomMeal.Add(stack4.Clone());
						ingredient2.MaxQuantity--;
						requestedIngredient = null;
					}
				}
			}
			else if (ingredient2.MaxQuantity > 0)
			{
				ItemStack stack5 = validStacks[api.World.Rand.Next(validStacks.Count)];
				if (stack5 != null)
				{
					randomMeal.Add(stack5.Clone());
					ingredient2.MaxQuantity--;
				}
			}
			while (randomMeal.Count < 6)
			{
				randomMeal.Add(null);
			}
		}
		return randomMeal.ToArray();
	}

	[return: NotNullIfNotNull("pieStack")]
	public static ItemStack? CycleTopCrustType(ItemStack? pieStack)
	{
		if (pieStack == null)
		{
			return null;
		}
		string topCrustType = GetTopCrustType(pieStack);
		pieStack.Attributes.SetString("topCrustType", TopCrustTypes[(TopCrustTypes.IndexOf((PieTopCrustType type) => type.Code.EqualsFast(topCrustType)) + 1) % TopCrustTypes.Length].Code);
		return pieStack;
	}

	[return: NotNullIfNotNull("pieStack")]
	public static string? GetTopCrustType(ItemStack? pieStack)
	{
		if (pieStack == null)
		{
			return null;
		}
		string topCrustType = pieStack.Attributes.GetAsString("topCrustType", "full");
		if (!TopCrustTypes.Any((PieTopCrustType type) => type.Code.EqualsFast(topCrustType)))
		{
			switch (topCrustType.ToInt())
			{
			default:
				topCrustType = "full";
				break;
			case 1:
				topCrustType = "square";
				break;
			case 2:
				topCrustType = "diagonal";
				break;
			}
			pieStack.Attributes.SetString("topCrustType", topCrustType);
		}
		return topCrustType;
	}

	public override string HandbookPageCodeForStack(IWorldAccessor world, ItemStack stack)
	{
		string type = null;
		ItemStack[] contents = GetContents(world, stack);
		if (contents != null && contents.Length > 1)
		{
			ItemStack obj = contents[1];
			type = ((obj == null || obj.ItemAttributes?["inPieProperties"]?.AsObject<InPieProperties>()?.AllowMixing != false) ? ("mixed-" + FillingFoodCategory(contents[1]).ToString().ToLowerInvariant()) : ("single-" + contents[1].Collectible.Code.ToShortString()));
			return "handbook-mealrecipe-" + type + "-pie";
		}
		return "craftinginfo-pie";
	}
}
