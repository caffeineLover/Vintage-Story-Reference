using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Cairo;
using Newtonsoft.Json;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class CollectibleBehaviorHandbookTextAndExtraInfo : CollectibleBehavior
{
	protected const int TinyPadding = 2;

	protected const int TinyIndent = 2;

	protected const int MarginBottom = 3;

	protected const int SmallPadding = 7;

	protected const int MediumPadding = 14;

	protected static TagSet fluxTag = TagSet.Empty;

	public ExtraHandbookSection[] ExtraHandBookSections;

	private ICoreAPI Api;

	private InventorySmelting dummySmeltingInv;

	private static string[] juiceableIgnoredAttributes;

	public CollectibleBehaviorHandbookTextAndExtraInfo(CollectibleObject collObj)
		: base(collObj)
	{
	}

	public override void OnLoaded(ICoreAPI api)
	{
		Api = api;
		if (fluxTag.IsEmpty)
		{
			api.CollectibleTagRegistry.TryRegisterAndCreateTagSetAndLogIssues(out fluxTag, "flux");
		}
		dummySmeltingInv = new InventorySmelting("smelting-handbook", api);
		JsonObject obj = collObj.Attributes?["handbook"]?["extraSections"];
		if (obj != null && obj.Exists)
		{
			ExtraHandBookSections = obj?.AsObject<ExtraHandbookSection[]>();
		}
	}

	public virtual RichTextComponentBase[] GetHandbookInfo(ItemSlot inSlot, ICoreClientAPI capi, ItemStack[] allStacks, ActionConsumable<string> openDetailPageFor)
	{
		ItemStack stack = inSlot.Itemstack;
		List<RichTextComponentBase> components = new List<RichTextComponentBase>();
		addGeneralInfo(inSlot, capi, stack, components, out var marginTop, out var marginBottom);
		capi.World.FrameProfiler.Mark("handbook-CollectibleGetHandbookInfo-1");
		List<ItemStack> allCookingContainers = new List<ItemStack>();
		List<ItemStack> allFuels = new List<ItemStack>();
		List<ItemStack> allMolds = new List<ItemStack>();
		List<ItemStack> allAnvils = new List<ItemStack>();
		List<ItemStack> breakBlocks = new List<ItemStack>();
		foreach (ItemStack aStack in allStacks)
		{
			JsonObject itemAttributes = aStack.ItemAttributes;
			if (itemAttributes != null && itemAttributes.KeyExists("cookingContainerSlots"))
			{
				allCookingContainers.Add(aStack);
			}
			CombustibleProperties combustibleProps = aStack.Collectible?.GetCombustibleProperties(Api.World, aStack, null);
			if (combustibleProps != null)
			{
				_ = combustibleProps.BurnDuration;
			}
			else
			{
				if (combustibleProps == null)
				{
					goto IL_00ce;
				}
				_ = combustibleProps.BurnTemperature;
			}
			allFuels.Add(aStack);
			goto IL_00ce;
			IL_00ce:
			CollectibleObject collectible = aStack.Collectible;
			if ((collectible is BlockToolMold || collectible is BlockIngotMold) ? true : false)
			{
				allMolds.Add(aStack);
			}
			if (aStack.Collectible is BlockAnvil)
			{
				allAnvils.Add(aStack);
			}
			BlockDropItemStack[] droppedStacks = aStack.Block?.GetDropsForHandbook(aStack, capi.World.Player);
			if (droppedStacks != null)
			{
				for (int i = 0; i < droppedStacks.Length; i++)
				{
					if (droppedStacks[i].ResolvedItemstack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
					{
						breakBlocks.Add(aStack);
					}
				}
			}
		}
		addDropsInfo(capi, openDetailPageFor, stack, components, marginTop, breakBlocks);
		bool haveText = addObtainedThroughInfo(capi, allStacks, openDetailPageFor, stack, components, marginTop, breakBlocks);
		haveText = addFoundInInfo(capi, openDetailPageFor, stack, components, marginTop, haveText);
		haveText = addAlloyForInfo(capi, openDetailPageFor, stack, components, marginTop, allCookingContainers, allFuels, haveText);
		haveText = addAlloyedFromInfo(capi, allStacks, openDetailPageFor, stack, components, marginTop, allCookingContainers, allFuels, haveText);
		capi.World.FrameProfiler.Mark("handbook-CollectibleGetHandbookInfo-2");
		haveText = addProcessesIntoInfo(capi, openDetailPageFor, stack, components, marginTop, marginBottom, allCookingContainers, allFuels, haveText);
		haveText = addProcessorForInfo(capi, allStacks, openDetailPageFor, stack, components, marginTop, marginBottom, allCookingContainers, allFuels, allMolds, allAnvils, haveText);
		capi.World.FrameProfiler.Mark("handbook-CollectibleGetHandbookInfo-3a");
		haveText = addIngredientForInfo(capi, allStacks, openDetailPageFor, stack, components, marginTop, allCookingContainers, allFuels, allMolds, haveText);
		capi.World.FrameProfiler.Mark("handbook-CollectibleGetHandbookInfo-3b");
		haveText = addCreatedByInfo(capi, allStacks, openDetailPageFor, stack, components, marginTop, allCookingContainers, allFuels, allMolds, allAnvils, haveText);
		capi.World.FrameProfiler.Mark("handbook-CollectibleGetHandbookInfo-3-CreatedBy");
		addExtraSections(capi, stack, components, marginTop);
		addEatenByInfo(capi, stack, components, marginTop);
		capi.World.FrameProfiler.Mark("handbook-CollectibleGetHandbookInfo-3d");
		addStorableInfo(capi, allStacks, openDetailPageFor, stack, components, marginTop);
		addStoredInInfo(capi, allStacks, openDetailPageFor, stack, components, marginTop);
		collObj.GetCollectibleInterface<ICustomHandbookPageContent>()?.OnHandbookPageComposed(components, inSlot, capi, allStacks, openDetailPageFor);
		capi.World.FrameProfiler.Mark("handbook-CollectibleGetHandbookInfo-4");
		return components.ToArray();
	}

	protected void addGeneralInfo(ItemSlot inSlot, ICoreClientAPI capi, ItemStack stack, List<RichTextComponentBase> components, out float marginTop, out float marginBottom)
	{
		components.Add(new ItemstackTextComponent(capi, stack, 100.0, 10.0));
		components.AddRange(VtmlUtil.Richtextify(capi, stack.GetName() + "\n", CairoFont.WhiteSmallishText()));
		if (capi.Settings.Bool["extendedDebugInfo"])
		{
			CairoFont font = CairoFont.WhiteDetailText();
			font.Color[3] = 0.5;
			components.AddRange(VtmlUtil.Richtextify(capi, "Page code:" + GuiHandbookItemStackPage.PageCodeForStack(stack) + "\n", font));
		}
		components.AddRange(VtmlUtil.Richtextify(capi, stack.GetDescription(capi.World, inSlot), CairoFont.WhiteSmallText()));
		marginTop = 7f;
		marginBottom = 3f;
	}

	protected void addDropsInfo(ICoreClientAPI capi, ActionConsumable<string> openDetailPageFor, ItemStack stack, List<RichTextComponentBase> components, float marginTop, List<ItemStack> breakBlocks)
	{
		if (stack.Class != EnumItemClass.Block)
		{
			return;
		}
		Dictionary<int, List<ItemStack>> dropsStacksByTool = new Dictionary<int, List<ItemStack>>();
		BlockDropItemStack[] blockDropStacks = stack.Block.GetDropsForHandbook(stack, capi.World.Player);
		int i;
		if (blockDropStacks != null)
		{
			BlockDropItemStack[] array = blockDropStacks;
			for (i = 0; i < array.Length; i++)
			{
				BlockDropItemStack val = array[i];
				int tool = (int)((!val.Tool.HasValue) ? ((EnumTool)(-1)) : val.Tool.Value);
				if (dropsStacksByTool.TryGetValue(tool, out var dropsStacks))
				{
					dropsStacks.Add(val.ResolvedItemstack);
					continue;
				}
				int num = 1;
				List<ItemStack> list = new List<ItemStack>(num);
				CollectionsMarshal.SetCount(list, num);
				Span<ItemStack> span = CollectionsMarshal.AsSpan(list);
				int index = 0;
				span[index] = val.ResolvedItemstack;
				dropsStacksByTool.Add(tool, list);
			}
		}
		if (dropsStacksByTool.Count > 0)
		{
			int j = 0;
			while (j < breakBlocks.Count)
			{
				if (dropsStacksByTool.Any((KeyValuePair<int, List<ItemStack>> e) => e.Value.Any((ItemStack itemStack) => itemStack.Equals(capi.World, breakBlocks[j], GlobalConstants.IgnoredStackAttributes))))
				{
					breakBlocks.RemoveAt(j);
				}
				i = j++;
			}
			bool haveText = components.Count > 0;
			AddHeading(components, capi, "Drops when broken", ref haveText);
			components.Add(new ClearFloatTextComponent(capi, 2f));
			foreach (KeyValuePair<int, List<ItemStack>> item in dropsStacksByTool)
			{
				item.Deconstruct(out i, out var value);
				int toolInt = i;
				List<ItemStack> dropsStacks2 = value;
				EnumTool? tool2 = ((toolInt == -1) ? ((EnumTool?)null) : new EnumTool?((EnumTool)toolInt));
				ItemStack[] toolStacks = ((!tool2.HasValue) ? null : ObjectCacheUtil.GetToolStacks(capi, tool2.Value));
				while (dropsStacks2.Count > 0)
				{
					ItemStack dstack = dropsStacks2[0];
					dropsStacks2.RemoveAt(0);
					if (dstack == null)
					{
						continue;
					}
					SlideshowItemstackTextComponent comp = new SlideshowItemstackTextComponent(capi, dstack, dropsStacks2, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
					{
						openDetailPageFor(getPageCodeForStack(capi, cs));
					});
					if (toolStacks != null)
					{
						comp.ExtraTooltipText = "\n\n<font color=\"orange\">" + Lang.Get("break-requires-tool-" + tool2.ToString().ToLowerInvariant()) + "</font>";
					}
					components.Add(comp);
					if (toolStacks != null)
					{
						components.Add(new SlideshowItemstackTextComponent(capi, toolStacks, 24.0, EnumFloat.Left, delegate(ItemStack cs)
						{
							openDetailPageFor(getPageCodeForStack(capi, cs));
						})
						{
							renderOffset = 
							{
								X = 0f - (float)GuiElement.scaled(17.0),
								Z = 100f
							},
							ShowTooltip = false
						});
					}
				}
			}
			components.Add(new ClearFloatTextComponent(capi, 2f));
		}
		BlockDropItemStack[] harvestDropStacks = stack.Block.GetBehavior<BlockBehaviorFruitingBush>()?.harvestedStacks;
		EnumTool? harvestTool = null;
		if (harvestDropStacks == null)
		{
			BlockBehaviorHarvestable bhh = stack.Block.GetBehavior<BlockBehaviorHarvestable>();
			if (bhh != null)
			{
				harvestTool = bhh.Tool;
				harvestDropStacks = bhh.harvestedStacks;
			}
		}
		if (harvestDropStacks == null && stack.Block is BlockFruitTreeBranch branch && branch.TypeProps.TryGetValue(stack.Attributes.GetString("type", "unknown"), out var props))
		{
			harvestDropStacks = props.FruitStacks;
		}
		List<ItemStack> harvestedStacks = new List<ItemStack>();
		if (harvestDropStacks != null)
		{
			harvestedStacks = (from blockDropItemStack in harvestDropStacks
				select blockDropItemStack.ResolvedItemstack into itemStack
				where itemStack != null
				select itemStack).ToList();
		}
		if (harvestedStacks.Count <= 0)
		{
			return;
		}
		bool haveText2 = components.Count > 0;
		AddHeading(components, capi, "handbook-dropswhen-harvested", ref haveText2);
		components.Add(new ClearFloatTextComponent(capi, 2f));
		ItemStack[] toolStacks2 = ((!harvestTool.HasValue) ? null : ObjectCacheUtil.GetToolStacks(capi, harvestTool.Value));
		while (harvestedStacks.Count > 0)
		{
			ItemStack hstack = harvestedStacks[0];
			harvestedStacks.RemoveAt(0);
			if (hstack == null)
			{
				continue;
			}
			SlideshowItemstackTextComponent comp2 = new SlideshowItemstackTextComponent(capi, hstack, harvestedStacks, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
			{
				openDetailPageFor(getPageCodeForStack(capi, cs));
			});
			if (toolStacks2 != null)
			{
				comp2.ExtraTooltipText = "\n\n<font color=\"orange\">" + Lang.Get("harvest-requires-tool-" + harvestTool.ToString().ToLowerInvariant()) + "</font>";
			}
			components.Add(comp2);
			if (toolStacks2 != null)
			{
				components.Add(new SlideshowItemstackTextComponent(capi, toolStacks2, 24.0, EnumFloat.Left, delegate(ItemStack cs)
				{
					openDetailPageFor(getPageCodeForStack(capi, cs));
				})
				{
					renderOffset = 
					{
						X = 0f - (float)GuiElement.scaled(17.0),
						Z = 100f
					},
					ShowTooltip = false
				});
			}
		}
		components.Add(new ClearFloatTextComponent(capi, 2f));
	}

	protected bool addObtainedThroughInfo(ICoreClientAPI capi, ItemStack[] allStacks, ActionConsumable<string> openDetailPageFor, ItemStack stack, List<RichTextComponentBase> components, float marginTop, List<ItemStack> breakBlocks)
	{
		List<ItemStack> harvestBlocks = new List<ItemStack>();
		List<string> killCreatures = new List<string>();
		List<string> harvestCreatures = new List<string>();
		HashSet<string> harvestCreatureCodes = new HashSet<string>();
		foreach (EntityProperties val in capi.World.EntityTypes)
		{
			if (val.Drops == null)
			{
				continue;
			}
			for (int i = 0; i < val.Drops.Length; i++)
			{
				if (val.Drops[i].ResolvedItemstack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
				{
					killCreatures.Add(Lang.Get(val.Code.Domain + ":item-creature-" + val.Code.Path));
				}
			}
			BlockDropItemStack[] harvestableDrops = val.Attributes?["harvestableDrops"]?.AsArray<BlockDropItemStack>();
			if (harvestableDrops == null)
			{
				continue;
			}
			BlockDropItemStack[] array = harvestableDrops;
			foreach (BlockDropItemStack hstack in array)
			{
				hstack.Resolve(Api.World, "handbook info", new AssetLocation());
				if (hstack.ResolvedItemstack != null && hstack.ResolvedItemstack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
				{
					string code = val.Code.Domain + ":item-creature-" + val.Code.Path;
					JsonObject attributes = val.Attributes;
					if (attributes != null && attributes["handbook"]["groupcode"]?.Exists == true)
					{
						code = val.Attributes?["handbook"]["groupcode"].AsString();
					}
					if (!harvestCreatureCodes.Contains(code))
					{
						harvestCreatures.Add(Lang.Get(code));
						harvestCreatureCodes.Add(code);
					}
					break;
				}
			}
		}
		foreach (ItemStack aStack in allStacks)
		{
			Block block = aStack.Block;
			if (block == null)
			{
				continue;
			}
			BlockDropItemStack[] harvestedStacks = block.GetBehavior<BlockBehaviorHarvestable>()?.harvestedStacks ?? block.GetBehavior<BlockBehaviorFruitingBush>()?.harvestedStacks;
			if (harvestedStacks == null)
			{
				if (block is BlockFruitTreeBranch branch && branch.TypeProps.TryGetValue(aStack.Attributes.GetString("type", "unknown"), out var props))
				{
					harvestedStacks = props.FruitStacks;
				}
				if (harvestedStacks == null)
				{
					continue;
				}
			}
			if (harvestedStacks.Any((BlockDropItemStack hStack) => hStack != null && hStack.ResolvedItemstack?.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes) == true))
			{
				harvestBlocks.Add(aStack);
			}
		}
		bool haveText = components.Count > 0;
		if (killCreatures.Count > 0)
		{
			AddHeading(components, capi, "Obtained by killing", ref haveText);
			components.Add(new ClearFloatTextComponent(capi, 2f));
			RichTextComponent comp = new RichTextComponent(capi, string.Join(", ", killCreatures) + "\n", CairoFont.WhiteSmallText());
			comp.PaddingLeft = 2.0;
			components.Add(comp);
		}
		if (harvestCreatures.Count > 0)
		{
			AddHeading(components, capi, "handbook-obtainedby-killing-harvesting", ref haveText);
			components.Add(new ClearFloatTextComponent(capi, 2f));
			RichTextComponent comp2 = new RichTextComponent(capi, string.Join(", ", harvestCreatures) + "\n", CairoFont.WhiteSmallText());
			comp2.PaddingLeft = 2.0;
			components.Add(comp2);
		}
		if (breakBlocks.Count > 0)
		{
			AddHeading(components, capi, "Obtained by breaking", ref haveText);
			components.Add(new ClearFloatTextComponent(capi, 2f));
			while (breakBlocks.Count > 0)
			{
				ItemStack dstack = breakBlocks[0];
				breakBlocks.RemoveAt(0);
				if (dstack != null)
				{
					SlideshowItemstackTextComponent comp3 = new SlideshowItemstackTextComponent(capi, dstack, breakBlocks, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
					{
						openDetailPageFor(getPageCodeForStack(capi, cs));
					});
					components.Add(comp3);
				}
			}
			components.Add(new ClearFloatTextComponent(capi, 2f));
		}
		if (harvestBlocks.Count > 0)
		{
			AddHeading(components, capi, "handbook-obtainedby-block-harvesting", ref haveText);
			components.Add(new ClearFloatTextComponent(capi, 2f));
			while (harvestBlocks.Count > 0)
			{
				ItemStack hstack2 = harvestBlocks[0];
				harvestBlocks.RemoveAt(0);
				if (hstack2 != null)
				{
					SlideshowItemstackTextComponent comp4 = new SlideshowItemstackTextComponent(capi, hstack2, harvestBlocks, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
					{
						openDetailPageFor(getPageCodeForStack(capi, cs));
					});
					components.Add(comp4);
				}
			}
			components.Add(new ClearFloatTextComponent(capi, 2f));
		}
		return haveText;
	}

	protected bool addFoundInInfo(ICoreClientAPI capi, ActionConsumable<string> openDetailPageFor, ItemStack stack, List<RichTextComponentBase> components, float marginTop, bool haveText)
	{
		string customFoundIn = stack.Collectible.Attributes?["handbook"]?["foundIn"]?.AsString();
		if (customFoundIn != null)
		{
			AddHeading(components, capi, "Found in", ref haveText);
			RichTextComponent comp = new RichTextComponent(capi, Lang.Get(customFoundIn), CairoFont.WhiteSmallText());
			comp.PaddingLeft = 2.0;
			components.Add(comp);
		}
		JsonObject attributes = collObj.Attributes;
		if (attributes != null && attributes["hostRockFor"].Exists)
		{
			AddHeading(components, capi, "Host rock for", ref haveText);
			int[] blockids = collObj.Attributes?["hostRockFor"].AsArray<int>();
			Vintagestory.API.Datastructures.OrderedDictionary<string, List<ItemStack>> blocks = new Vintagestory.API.Datastructures.OrderedDictionary<string, List<ItemStack>>();
			for (int i = 0; i < blockids.Length; i++)
			{
				Block block = capi.World.Blocks[blockids[i]];
				string key = block.Code.ToString();
				JsonObject attributes2 = block.Attributes;
				if (attributes2 != null && attributes2["handbook"]["groupBy"].Exists)
				{
					key = block.Attributes["handbook"]["groupBy"].AsArray<string>()[0];
				}
				if (!blocks.ContainsKey(key))
				{
					blocks[key] = new List<ItemStack>();
				}
				blocks[key].Add(new ItemStack(block));
			}
			int firstPadding = 2;
			foreach (KeyValuePair<string, List<ItemStack>> val in blocks)
			{
				SlideshowItemstackTextComponent comp2 = new SlideshowItemstackTextComponent(capi, val.Value.ToArray(), 40.0, EnumFloat.Inline, delegate(ItemStack cs)
				{
					openDetailPageFor(getPageCodeForStack(capi, cs));
				});
				comp2.PaddingLeft = firstPadding;
				firstPadding = 0;
				components.Add(comp2);
			}
		}
		JsonObject attributes3 = collObj.Attributes;
		if (attributes3 != null && attributes3["hostRock"].Exists)
		{
			AddHeading(components, capi, "Occurs in host rock", ref haveText);
			ushort[] blockids2 = collObj.Attributes?["hostRock"].AsArray<ushort>();
			Vintagestory.API.Datastructures.OrderedDictionary<string, List<ItemStack>> blocks2 = new Vintagestory.API.Datastructures.OrderedDictionary<string, List<ItemStack>>();
			for (int i2 = 0; i2 < blockids2.Length; i2++)
			{
				Block block2 = capi.World.Blocks[blockids2[i2]];
				string key2 = block2.Code.FirstCodePart();
				JsonObject attributes4 = block2.Attributes;
				if (attributes4 != null && attributes4["handbook"]["groupBy"].Exists)
				{
					key2 = block2.Attributes["handbook"]["groupBy"].AsArray<string>()[0];
				}
				if (block2.Attributes?["handbook"]?["exclude"]?.AsBool() != true)
				{
					if (!blocks2.ContainsKey(key2))
					{
						blocks2[key2] = new List<ItemStack>();
					}
					blocks2[key2].Add(new ItemStack(block2));
				}
			}
			int firstPadding2 = 2;
			foreach (KeyValuePair<string, List<ItemStack>> val2 in blocks2)
			{
				SlideshowItemstackTextComponent comp3 = new SlideshowItemstackTextComponent(capi, val2.Value.ToArray(), 40.0, EnumFloat.Inline, delegate(ItemStack cs)
				{
					openDetailPageFor(getPageCodeForStack(capi, cs));
				});
				comp3.PaddingLeft = firstPadding2;
				firstPadding2 = 0;
				components.Add(comp3);
			}
		}
		return haveText;
	}

	protected bool addAlloyForInfo(ICoreClientAPI capi, ActionConsumable<string> openDetailPageFor, ItemStack stack, List<RichTextComponentBase> components, float marginTop, List<ItemStack> containers, List<ItemStack> fuels, bool haveText)
	{
		CombustibleProperties combustibleProps = stack.Collectible.GetCombustibleProperties(Api.World, stack, null);
		Dictionary<AssetLocation, ItemStack> alloyables = new Dictionary<AssetLocation, ItemStack>();
		foreach (AlloyRecipe val in capi.GetMetalAlloys())
		{
			MetalAlloyIngredient[] ingredients = val.Ingredients;
			for (int i = 0; i < ingredients.Length; i++)
			{
				if (ingredients[i].ResolvedItemStack.Equals(capi.World, combustibleProps?.SmeltedStack?.ResolvedItemStack, GlobalConstants.IgnoredStackAttributes) && getCanContainerSmelt(capi, containers, fuels, stack))
				{
					CollectibleObject outputColl = val.Output.ResolvedItemStack.Collectible;
					AssetLocation bitCode = new AssetLocation(outputColl.Code.Domain, "metalbit-" + outputColl.Variant["metal"]);
					alloyables[bitCode] = new ItemStack(capi.World.GetItem(bitCode));
				}
			}
		}
		if (alloyables.Count > 0)
		{
			AddHeading(components, capi, "Alloy for", ref haveText);
			int firstPadding = 2;
			foreach (KeyValuePair<AssetLocation, ItemStack> val2 in alloyables)
			{
				ItemstackTextComponent comp = new ItemstackTextComponent(capi, val2.Value, 40.0, 0.0, EnumFloat.Inline, delegate(ItemStack cs)
				{
					openDetailPageFor(getPageCodeForStack(capi, cs));
				});
				comp.PaddingLeft = firstPadding;
				firstPadding = 0;
				components.Add(comp);
			}
			haveText = true;
		}
		return haveText;
	}

	protected bool addAlloyedFromInfo(ICoreClientAPI capi, ItemStack[] allStacks, ActionConsumable<string> openDetailPageFor, ItemStack stack, List<RichTextComponentBase> components, float marginTop, List<ItemStack> containers, List<ItemStack> fuels, bool haveText)
	{
		ItemStack ingotStack = stack.Collectible.GetCombustibleProperties(Api.World, stack, null)?.SmeltedStack?.ResolvedItemStack;
		if (stack.Collectible.FirstCodePart() != "metalbit" || ingotStack == null)
		{
			return haveText;
		}
		Dictionary<AssetLocation, MetalAlloyIngredient[]> alloyableFrom = new Dictionary<AssetLocation, MetalAlloyIngredient[]>();
		foreach (AlloyRecipe val in capi.GetMetalAlloys())
		{
			if (val.Output.ResolvedItemStack.Equals(capi.World, ingotStack, GlobalConstants.IgnoredStackAttributes))
			{
				alloyableFrom[val.Output.ResolvedItemStack.Collectible.Code] = val.Ingredients;
			}
		}
		if (alloyableFrom.Count > 0)
		{
			AddHeading(components, capi, "Alloyed from", ref haveText);
			int firstPadding = 2;
			foreach (KeyValuePair<AssetLocation, MetalAlloyIngredient[]> item in alloyableFrom)
			{
				MetalAlloyIngredient[] value = item.Value;
				foreach (MetalAlloyIngredient ingred in value)
				{
					string ratio = " " + Lang.Get("alloy-ratio-from-to", (int)(ingred.MinRatio * 100f), (int)(ingred.MaxRatio * 100f));
					components.Add(new RichTextComponent(capi, ratio, CairoFont.WhiteSmallText()));
					ItemStack[] ores = allStacks.Where((ItemStack aStack) => ingred.ResolvedItemStack.Equals(capi.World, aStack.Collectible.GetCombustibleProperties(Api.World, aStack, null)?.SmeltedStack?.ResolvedItemStack, GlobalConstants.IgnoredStackAttributes) && getCanContainerSmelt(capi, containers, fuels, aStack)).ToArray();
					if (ores == null || ores.Length != 0)
					{
						ItemstackComponentBase comp = new SlideshowItemstackTextComponent(capi, ores, 30.0, EnumFloat.Inline, delegate(ItemStack cs)
						{
							openDetailPageFor(getPageCodeForStack(capi, cs));
						});
						comp.offY = GuiElement.scaled(7.0);
						comp.PaddingLeft = firstPadding;
						firstPadding = 0;
						components.Add(comp);
					}
				}
				components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
			}
			components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
		}
		return haveText;
	}

	protected bool addProcessesIntoInfo(ICoreClientAPI capi, ActionConsumable<string> openDetailPageFor, ItemStack stack, List<RichTextComponentBase> components, float marginTop, float marginBottom, List<ItemStack> containers, List<ItemStack> fuels, bool haveText)
	{
		CombustibleProperties combustibleProps = collObj.GetCombustibleProperties(Api.World, stack, null);
		BakingProperties bp = collObj.Attributes?["bakingProperties"]?.AsObject<BakingProperties>();
		if (bp != null && bp.ResultCode != null)
		{
			Item item = capi.World.GetItem(new AssetLocation(bp.ResultCode));
			if (item != null)
			{
				AddHeading(components, capi, "smeltdesc-bake-title", ref haveText);
				ItemstackTextComponent cmp = new ItemstackTextComponent(capi, new ItemStack(item), 40.0, 10.0, EnumFloat.Inline, delegate(ItemStack cs)
				{
					openDetailPageFor(getPageCodeForStack(capi, cs));
				});
				cmp.ShowStacksize = true;
				cmp.PaddingLeft = 2.0;
				components.Add(cmp);
				components.Add(new ClearFloatTextComponent(capi, marginBottom));
			}
		}
		else if (combustibleProps?.SmeltedStack?.ResolvedItemStack != null && !combustibleProps.SmeltedStack.ResolvedItemStack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes) && (getCanSmelt(fuels, stack) || getCanBloomerySmelt(stack)))
		{
			string smelttype = combustibleProps.SmeltingType.ToString().ToLowerInvariant();
			AddHeading(components, capi, "game:smeltdesc-" + smelttype + "-title", ref haveText);
			ItemstackTextComponent cmp2 = new ItemstackTextComponent(capi, combustibleProps.SmeltedStack.ResolvedItemStack, 40.0, 10.0, EnumFloat.Inline, delegate(ItemStack cs)
			{
				openDetailPageFor(getPageCodeForStack(capi, cs));
			});
			cmp2.ShowStacksize = true;
			cmp2.PaddingLeft = 2.0;
			components.Add(cmp2);
			components.Add(new ClearFloatTextComponent(capi, marginBottom));
		}
		JsonObject attributes = collObj.Attributes;
		if (attributes != null && attributes["beehivekiln"].Exists)
		{
			Dictionary<string, JsonItemStack> beehivekilnProps = collObj.Attributes["beehivekiln"].AsObject<Dictionary<string, JsonItemStack>>();
			AddHeading(components, capi, "smeltdesc-beehivekiln-title", ref haveText);
			if (beehivekilnProps.DistinctBy((KeyValuePair<string, JsonItemStack> e) => e.Value.Code).Count() == 1)
			{
				JsonItemStack firesIntoStack = beehivekilnProps.FirstOrDefault().Value;
				if (firesIntoStack != null && firesIntoStack.Resolve(capi.World, "beehivekiln-burn"))
				{
					components.Add(new ItemstackTextComponent(capi, firesIntoStack.ResolvedItemStack, 40.0, 0.0, EnumFloat.Inline, delegate(ItemStack cs)
					{
						openDetailPageFor(getPageCodeForStack(capi, cs));
					}));
					components.Add(new ClearFloatTextComponent(capi, marginBottom));
				}
			}
			else
			{
				foreach (var (doorOpen, firesIntoStack2) in beehivekilnProps)
				{
					if (firesIntoStack2 != null && firesIntoStack2.Resolve(capi.World, "beehivekiln-burn"))
					{
						components.Add(new ItemstackTextComponent(capi, firesIntoStack2.ResolvedItemStack, 40.0, 2.0, EnumFloat.Inline, delegate(ItemStack cs)
						{
							openDetailPageFor(getPageCodeForStack(capi, cs));
						}));
						components.Add(new RichTextComponent(capi, Lang.Get("smeltdesc-beehivekiln-opendoors", doorOpen), CairoFont.WhiteDetailText())
						{
							VerticalAlign = EnumVerticalAlign.Middle,
							PaddingRight = -5.0
						});
						components.Add(new ItemstackTextComponent(capi, new ItemStack(capi.World.GetBlock("cokeovendoor-closed-north")), 40.0, 0.0, EnumFloat.Inline, delegate(ItemStack cs)
						{
							openDetailPageFor(getPageCodeForStack(capi, cs));
						}));
						components.Add(new ClearFloatTextComponent(capi, marginBottom));
					}
				}
			}
		}
		JsonObject attributes2 = collObj.Attributes;
		if (attributes2 != null && attributes2["carburizableProps"]?["carburizedOutput"]?.Exists == true)
		{
			JsonItemStack carburizedJStack = stack.ItemAttributes["carburizableProps"]["carburizedOutput"].AsObject<JsonItemStack>(null, stack.Collectible.Code.Domain);
			if (carburizedJStack != null && carburizedJStack.Resolve(Api.World, "carburizable handbook") && !carburizedJStack.ResolvedItemStack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
			{
				AddHeading(components, capi, "carburizesdesc-title", ref haveText);
				ItemstackTextComponent cmp3 = new ItemstackTextComponent(capi, carburizedJStack.ResolvedItemStack, 40.0, 10.0, EnumFloat.Inline, delegate(ItemStack cs)
				{
					openDetailPageFor(getPageCodeForStack(capi, cs));
				});
				cmp3.ShowStacksize = true;
				cmp3.PaddingLeft = 2.0;
				components.Add(cmp3);
				components.Add(new ClearFloatTextComponent(capi, marginBottom));
			}
		}
		CrushingProperties crushingProps = stack.Collectible.GetCrushingProperties(Api.World, stack);
		if (crushingProps?.CrushedStack?.ResolvedItemStack != null && !crushingProps.CrushedStack.ResolvedItemStack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
		{
			AddHeading(components, capi, "pulverizesdesc-title", ref haveText);
			ItemstackTextComponent cmp4 = new ItemstackTextComponent(capi, crushingProps.CrushedStack.ResolvedItemStack, 40.0, 10.0, EnumFloat.Inline, delegate(ItemStack cs)
			{
				openDetailPageFor(getPageCodeForStack(capi, cs));
			});
			cmp4.ShowStacksize = true;
			cmp4.PaddingLeft = 2.0;
			components.Add(cmp4);
			components.Add(new ClearFloatTextComponent(capi, marginBottom));
		}
		GrindingProperties grindingProps = collObj.GetGrindingProperties(Api.World, stack);
		if (grindingProps?.GroundStack?.ResolvedItemStack != null && !grindingProps.GroundStack.ResolvedItemStack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
		{
			AddHeading(components, capi, "Grinds into", ref haveText);
			ItemstackTextComponent cmp5 = new ItemstackTextComponent(capi, grindingProps.GroundStack.ResolvedItemStack, 40.0, 10.0, EnumFloat.Inline, delegate(ItemStack cs)
			{
				openDetailPageFor(getPageCodeForStack(capi, cs));
			});
			cmp5.ShowStacksize = true;
			cmp5.PaddingLeft = 2.0;
			components.Add(cmp5);
			components.Add(new ClearFloatTextComponent(capi, marginBottom));
		}
		JuiceableProperties jprops = getjuiceableProps(stack);
		double litresLeft = stack.Attributes?.GetDouble("juiceableLitresLeft") ?? 0.0;
		if (jprops != null && (jprops.LitresPerItem.HasValue || litresLeft > 0.0))
		{
			AddHeading(components, capi, "Juices into", ref haveText);
			ItemStack jstack = jprops.LiquidStack.ResolvedItemStack.Clone();
			if (jprops.LitresPerItem.HasValue)
			{
				jstack.StackSize = (int)(100f * jprops.LitresPerItem).Value;
			}
			if (litresLeft > 0.0)
			{
				jstack.StackSize = (int)(100.0 * litresLeft);
			}
			ItemstackTextComponent cmp6 = new ItemstackTextComponent(capi, jstack, 40.0, 0.0, EnumFloat.Inline, delegate(ItemStack cs)
			{
				openDetailPageFor(getPageCodeForStack(capi, cs));
			});
			cmp6.ShowStacksize = jprops.LitresPerItem.HasValue;
			cmp6.PaddingLeft = 2.0;
			components.Add(cmp6);
			if (jprops.ReturnStack?.ResolvedItemStack == null)
			{
				if (!stack.Equals(capi.World, jprops.PressedStack.ResolvedItemStack, GlobalConstants.IgnoredStackAttributes))
				{
					ItemStack pstack = jprops.PressedStack.ResolvedItemStack.Clone();
					cmp6 = new ItemstackTextComponent(capi, pstack, 40.0, 0.0, EnumFloat.Inline, delegate(ItemStack cs)
					{
						openDetailPageFor(getPageCodeForStack(capi, cs));
					});
					cmp6.PaddingLeft = 0.0;
					components.Add(cmp6);
				}
			}
			else
			{
				ItemStack rstack = jprops.ReturnStack.ResolvedItemStack.Clone();
				if (jprops.LitresPerItem.HasValue)
				{
					rstack.StackSize /= (int)(1f / jprops.LitresPerItem).Value;
				}
				cmp6 = new ItemstackTextComponent(capi, rstack, 40.0, 0.0, EnumFloat.Inline, delegate(ItemStack cs)
				{
					openDetailPageFor(getPageCodeForStack(capi, cs));
				});
				cmp6.ShowStacksize = jprops.LitresPerItem.HasValue;
				cmp6.PaddingLeft = 0.0;
				components.Add(cmp6);
			}
			components.Add(new ClearFloatTextComponent(capi, marginBottom));
		}
		CollectibleBehaviorSqueezable cbs = stack.Collectible.GetCollectibleBehavior<CollectibleBehaviorSqueezable>(withInheritance: true);
		if (cbs != null)
		{
			AddHeading(components, capi, "squeezesdesc-title", ref haveText);
			int indent = 2;
			if (cbs.SqueezedLiquid != null)
			{
				ItemStack lstack = new ItemStack(cbs.SqueezedLiquid);
				lstack.StackSize = (int)(100f * cbs.SqueezedLitres);
				ItemstackTextComponent cmp7 = new ItemstackTextComponent(capi, lstack, 40.0, 0.0, EnumFloat.Inline, delegate(ItemStack cs)
				{
					openDetailPageFor(getPageCodeForStack(capi, cs));
				});
				cmp7.ShowStacksize = true;
				cmp7.PaddingLeft = indent;
				components.Add(cmp7);
				indent = 0;
			}
			JsonItemStack[] returnStacks = cbs.ReturnStacks;
			foreach (JsonItemStack returnStack in returnStacks)
			{
				if (returnStack?.ResolvedItemStack != null && !stack.Equals(capi.World, returnStack.ResolvedItemStack, GlobalConstants.IgnoredStackAttributes))
				{
					ItemstackTextComponent cmp8 = new ItemstackTextComponent(capi, returnStack.ResolvedItemStack, 40.0, 0.0, EnumFloat.Inline, delegate(ItemStack cs)
					{
						openDetailPageFor(getPageCodeForStack(capi, cs));
					});
					cmp8.ShowStacksize = true;
					cmp8.PaddingLeft = indent;
					components.Add(cmp8);
					indent = 0;
				}
			}
			components.Add(new ClearFloatTextComponent(capi, marginBottom));
		}
		CollectibleBehaviorGroundStoredProcessable gsp = stack.Collectible.GetCollectibleBehavior<CollectibleBehaviorGroundStoredProcessable>(withInheritance: true);
		if (gsp != null)
		{
			List<ItemStack> processedStacks = (gsp.ProcessedStacks?.Select((BlockDropItemStack blockDropItemStack) => blockDropItemStack.ResolvedItemstack) ?? Array.Empty<ItemStack>()).ToList();
			ItemStack rStack = gsp.RemainingItem?.ResolvedItemStack;
			if (rStack != null)
			{
				processedStacks.Insert(0, rStack);
			}
			string extraTooltipText = null;
			ItemStack[] toolStacks = null;
			EnumTool? tool = gsp.Tool;
			if (tool.HasValue)
			{
				EnumTool tool2 = tool.GetValueOrDefault();
				extraTooltipText = "\n\n<font color=\"orange\">" + Lang.Get("processing-requires-tool-" + tool2.ToString().ToLowerInvariant()) + "</font>";
				toolStacks = ObjectCacheUtil.GetToolStacks(capi, tool2);
			}
			if (processedStacks.Count > 0)
			{
				AddHeading(components, capi, gsp.HandbookProcessIntoTitle, ref haveText);
				int indent2 = 2;
				while (processedStacks.Count > 0)
				{
					ItemStack pstack2 = processedStacks[0];
					processedStacks.RemoveAt(0);
					if (pstack2 == null)
					{
						continue;
					}
					components.Add(new SlideshowItemstackTextComponent(capi, pstack2, processedStacks, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
					{
						openDetailPageFor(getPageCodeForStack(capi, cs));
					})
					{
						ShowStackSize = true,
						PaddingLeft = indent2,
						ExtraTooltipText = extraTooltipText
					});
					indent2 = 0;
					if (toolStacks != null)
					{
						components.Add(new SlideshowItemstackTextComponent(capi, toolStacks, 24.0, EnumFloat.Left, delegate(ItemStack cs)
						{
							openDetailPageFor(getPageCodeForStack(capi, cs));
						})
						{
							renderOffset = 
							{
								X = 0f - (float)GuiElement.scaled(17.0),
								Z = 100f
							},
							ShowTooltip = false
						});
					}
				}
				components.Add(new ClearFloatTextComponent(capi, marginBottom));
			}
		}
		if (stack.Collectible is ItemOre)
		{
			int units = stack.ItemAttributes["metalUnits"].AsInt(5);
			string type = stack.Collectible.Variant["ore"].Replace("quartz_", "").Replace("galena_", "");
			Item item2 = capi.World.GetItem(new AssetLocation("nugget-" + type));
			if (item2 != null)
			{
				AddHeading(components, capi, "oresmasheddesc-title", ref haveText);
				components.Add(new ItemstackTextComponent(capi, new ItemStack(item2, Math.Max(1, units / 5)), 40.0, 10.0, EnumFloat.Inline, delegate(ItemStack cs)
				{
					openDetailPageFor(getPageCodeForStack(capi, cs));
				})
				{
					ShowStacksize = true,
					PaddingLeft = 2.0
				});
				components.Add(new ClearFloatTextComponent(capi, marginBottom));
			}
		}
		DistillationProps dprops = getDistillationProps(stack);
		if (dprops != null)
		{
			AddHeading(components, capi, "One liter distills into", ref haveText);
			ItemStack dstack = dprops.DistilledStack?.ResolvedItemStack.Clone();
			if (dprops.Ratio != 0f)
			{
				dstack.StackSize = (int)((float)(100 * stack.StackSize) * dprops.Ratio);
			}
			ItemstackTextComponent cmp9 = new ItemstackTextComponent(capi, dstack, 40.0, 10.0, EnumFloat.Inline, delegate(ItemStack cs)
			{
				openDetailPageFor(getPageCodeForStack(capi, cs));
			});
			cmp9.ShowStacksize = dprops.Ratio != 0f;
			cmp9.PaddingLeft = 2.0;
			components.Add(cmp9);
			components.Add(new ClearFloatTextComponent(capi, marginBottom));
		}
		TransitionableProperties[] props = collObj.GetTransitionableProperties(capi.World, stack, null);
		if (props != null)
		{
			haveText = true;
			ClearFloatTextComponent verticalSpace = new ClearFloatTextComponent(capi, 14f);
			TransitionableProperties[] array = props;
			foreach (TransitionableProperties prop in array)
			{
				if (prop.Type == EnumTransitionType.None)
				{
					continue;
				}
				float total = prop.FreshHours.avg + prop.TransitionHours.avg;
				string displayTime = "-hours";
				if (Math.Round(total, 0, MidpointRounding.AwayFromZero) >= (double)capi.World.Calendar.HoursPerDay)
				{
					total /= capi.World.Calendar.HoursPerDay;
					total = Math.Max(1f, total);
					displayTime = "-days";
					if (Math.Round(total, 0, MidpointRounding.AwayFromZero) >= (double)capi.World.Calendar.DaysPerYear)
					{
						total /= (float)capi.World.Calendar.DaysPerYear;
						total = Math.Max(1f, total);
						displayTime = "-years";
					}
					else
					{
						total = (float)Math.Round(total, 0, MidpointRounding.AwayFromZero);
					}
				}
				else if (total > 1f)
				{
					total = (float)Math.Round(total, 0, MidpointRounding.AwayFromZero);
				}
				components.Add(verticalSpace);
				components.Add(new RichTextComponent(capi, Lang.Get("handbook-processesinto-transition-" + prop.Type.ToString().ToLowerInvariant() + displayTime, total) + "\n", CairoFont.WhiteSmallText().WithWeight(FontWeight.Bold)));
				components.Add(new ItemstackTextComponent(capi, prop.TransitionedStack.ResolvedItemStack, 40.0, 10.0, EnumFloat.Inline, delegate(ItemStack cs)
				{
					openDetailPageFor(getPageCodeForStack(capi, cs));
				})
				{
					PaddingLeft = 2.0
				});
			}
			if (props.Length != 0)
			{
				components.Add(new ClearFloatTextComponent(capi, marginBottom));
			}
		}
		return haveText;
	}

	public bool addProcessorForInfo(ICoreClientAPI capi, ItemStack[] allStacks, ActionConsumable<string> openDetailPageFor, ItemStack stack, List<RichTextComponentBase> components, float marginTop, float marginBottom, List<ItemStack> containers, List<ItemStack> fuels, List<ItemStack> molds, List<ItemStack> anvils, bool haveText)
	{
		List<ItemStack> outputs = new List<ItemStack>();
		Block block = stack.Block;
		if (!(block is BlockToolMold))
		{
			if (!(block is BlockIngotMold))
			{
				if (!(block is BlockFruitPress))
				{
					if (!(block is BlockAnvil anvil))
					{
						if (block is BlockQuern)
						{
							ItemStack[] array = allStacks;
							foreach (ItemStack val in array)
							{
								GrindingProperties grindingProps = val.Collectible.GetGrindingProperties(Api.World, val);
								if (grindingProps != null)
								{
									ItemStack gstack = grindingProps.GroundStack.ResolvedItemStack;
									if (gstack != null && !outputs.Any((ItemStack s) => s.Equals(capi.World, gstack, GlobalConstants.IgnoredStackAttributes)))
									{
										outputs.Add(gstack);
									}
								}
							}
							if (outputs.Count > 0)
							{
								AddHeading(components, capi, "handbook-processorfor-grinding-title", ref haveText);
								int firstPadding = 2;
								while (outputs.Count > 0)
								{
									ItemStack dstack = outputs[0];
									outputs.RemoveAt(0);
									if (dstack != null)
									{
										SlideshowItemstackTextComponent comp = new SlideshowItemstackTextComponent(capi, dstack, outputs, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
										{
											openDetailPageFor(getPageCodeForStack(capi, cs));
										});
										comp.PaddingLeft = firstPadding;
										firstPadding = 0;
										components.Add(comp);
									}
								}
								components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
							}
						}
					}
					else
					{
						foreach (SmithingRecipe val2 in capi.GetSmithingRecipes())
						{
							if (!outputs.Any((ItemStack s) => s.Equals(capi.World, val2.Output.ResolvedItemStack, GlobalConstants.IgnoredStackAttributes)) && !(val2.Ingredient.ResolvedItemStack.Collectible.GetCollectibleInterface<IAnvilWorkable>()?.GetRequiredAnvilTier(val2.Ingredient.ResolvedItemStack) > anvil.MetalTier))
							{
								outputs.Add(val2.Output.ResolvedItemStack);
							}
						}
						if (outputs.Count > 0)
						{
							AddHeading(components, capi, "handbook-processorfor-smithing-title", ref haveText);
							int firstPadding2 = 2;
							while (outputs.Count > 0)
							{
								ItemStack dstack2 = outputs[0];
								outputs.RemoveAt(0);
								if (dstack2 != null)
								{
									SlideshowItemstackTextComponent comp2 = new SlideshowItemstackTextComponent(capi, dstack2, outputs, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
									{
										openDetailPageFor(getPageCodeForStack(capi, cs));
									});
									comp2.PaddingLeft = firstPadding2;
									firstPadding2 = 0;
									components.Add(comp2);
								}
							}
							components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
						}
					}
				}
				else
				{
					ItemStack[] array = allStacks;
					foreach (ItemStack val3 in array)
					{
						JuiceableProperties jprops = getjuiceableProps(val3);
						if (jprops == null)
						{
							continue;
						}
						double litresLeft = val3.Attributes?.GetDouble("juiceableLitresLeft") ?? 0.0;
						ItemStack jstack = jprops.LiquidStack.ResolvedItemStack.Clone();
						if (jprops.LitresPerItem.HasValue)
						{
							jstack.StackSize = (int)(100f * jprops.LitresPerItem).Value;
						}
						if (litresLeft > 0.0)
						{
							jstack.StackSize = (int)(100.0 * litresLeft);
						}
						if (!val3.Equals(capi.World, jstack, GlobalConstants.IgnoredStackAttributes) && !outputs.Any((ItemStack s) => s.Equals(capi.World, jstack, GlobalConstants.IgnoredStackAttributes)))
						{
							outputs.Add(jstack);
						}
						if (jprops.ReturnStack?.ResolvedItemStack == null)
						{
							if (!val3.Equals(capi.World, jprops.PressedStack.ResolvedItemStack, GlobalConstants.IgnoredStackAttributes))
							{
								ItemStack pstack = jprops.PressedStack.ResolvedItemStack.Clone();
								if (!val3.Equals(capi.World, jstack, GlobalConstants.IgnoredStackAttributes) && !outputs.Any((ItemStack s) => s.Equals(capi.World, pstack, GlobalConstants.IgnoredStackAttributes)))
								{
									outputs.Add(pstack);
								}
							}
							continue;
						}
						ItemStack rstack = jprops.ReturnStack.ResolvedItemStack.Clone();
						if (jprops.LitresPerItem.HasValue)
						{
							rstack.StackSize /= (int)(1f / jprops.LitresPerItem).Value;
						}
						if (!val3.Equals(capi.World, jstack, GlobalConstants.IgnoredStackAttributes) && !outputs.Any((ItemStack s) => s.Equals(capi.World, rstack, GlobalConstants.IgnoredStackAttributes)))
						{
							outputs.Add(rstack);
						}
						ItemStack pstack2 = jprops.PressedStack.ResolvedItemStack.Clone();
						pstack2.Attributes.SetDouble("juiceableLitresLeft", 1.0);
						if (!val3.Equals(capi.World, pstack2, GlobalConstants.IgnoredStackAttributes) && !outputs.Any((ItemStack s) => s.Equals(capi.World, pstack2, GlobalConstants.IgnoredStackAttributes)))
						{
							outputs.Add(pstack2);
						}
					}
					if (outputs.Count > 0)
					{
						AddHeading(components, capi, "handbook-processorfor-juicing-title", ref haveText);
						int firstPadding3 = 2;
						while (outputs.Count > 0)
						{
							ItemStack dstack3 = outputs[0];
							outputs.RemoveAt(0);
							if (dstack3 != null)
							{
								SlideshowItemstackTextComponent comp3 = new SlideshowItemstackTextComponent(capi, dstack3, outputs, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
								{
									openDetailPageFor(getPageCodeForStack(capi, cs));
								});
								comp3.PaddingLeft = firstPadding3;
								firstPadding3 = 0;
								components.Add(comp3);
							}
						}
						components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
					}
				}
			}
			else
			{
				ItemStack[] array = allStacks;
				foreach (ItemStack val4 in array)
				{
					if (val4.Collectible is ItemIngot)
					{
						Item bitObj = capi.World.GetItem("metalbit-" + val4.Collectible.LastCodePart());
						if (bitObj != null && getCanContainerSmelt(capi, containers, fuels, new ItemStack(bitObj)))
						{
							outputs.Add(val4);
						}
					}
				}
				if (outputs.Count > 0)
				{
					AddHeading(components, capi, "handbook-processorfor-metalmolding-title", ref haveText);
					int firstPadding4 = 2;
					while (outputs.Count > 0)
					{
						ItemStack dstack4 = outputs[0];
						outputs.RemoveAt(0);
						if (dstack4 != null)
						{
							SlideshowItemstackTextComponent comp4 = new SlideshowItemstackTextComponent(capi, dstack4, outputs, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
							{
								openDetailPageFor(getPageCodeForStack(capi, cs));
							});
							comp4.PaddingLeft = firstPadding4;
							firstPadding4 = 0;
							components.Add(comp4);
						}
					}
					components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
				}
			}
		}
		else
		{
			foreach (string metal in capi.ModLoader.GetModSystem<SurvivalCoreSystem>().metalsByCode.Keys)
			{
				Item bitObj2 = capi.World.GetItem("metalbit-" + metal);
				if (bitObj2 != null)
				{
					ItemStack bitStack = new ItemStack(bitObj2);
					if (getCanContainerSmelt(capi, containers, fuels, bitStack))
					{
						outputs.AddRange(getMoldedOutput(stack, new ItemStack[1] { bitStack }));
					}
				}
			}
			if (outputs.Count > 0)
			{
				AddHeading(components, capi, "handbook-processorfor-metalmolding-title", ref haveText);
				int firstPadding5 = 2;
				while (outputs.Count > 0)
				{
					ItemStack dstack5 = outputs[0];
					outputs.RemoveAt(0);
					if (dstack5 != null)
					{
						SlideshowItemstackTextComponent comp5 = new SlideshowItemstackTextComponent(capi, dstack5, outputs, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
						{
							openDetailPageFor(getPageCodeForStack(capi, cs));
						});
						comp5.PaddingLeft = firstPadding5;
						firstPadding5 = 0;
						components.Add(comp5);
					}
				}
				components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
			}
		}
		return haveText;
	}

	protected bool addIngredientForInfo(ICoreClientAPI capi, ItemStack[] allStacks, ActionConsumable<string> openDetailPageFor, ItemStack stack, List<RichTextComponentBase> components, float marginTop, List<ItemStack> containers, List<ItemStack> fuels, List<ItemStack> molds, bool haveText)
	{
		List<ItemStack> foundRecipeOutputs = new List<ItemStack>();
		foreach (KeyValuePair<IRecipeIngredientBase, List<IRecipeBase>> ingredientBase in capi.World.FastSearchRecipesByIngredient)
		{
			if (!ingredientBase.Key.SatisfiesAsIngredient(stack))
			{
				continue;
			}
			foreach (IRecipeBase item in ingredientBase.Value)
			{
				if (!(item is GridRecipe recval))
				{
					continue;
				}
				foreach (IRecipeIngredient ingred in recval.RecipeIngredients)
				{
					if (!ingred.SatisfiesAsIngredient(stack, checkStackSize: false))
					{
						continue;
					}
					DummySlot outSlot = new DummySlot();
					DummySlot[] inSlots = new DummySlot[recval.Width * recval.Height];
					for (int x = 0; x < recval.Width; x++)
					{
						for (int y = 0; y < recval.Height; y++)
						{
							CraftingRecipeIngredient inIngred = recval.GetElementInGrid(y, x, recval.ResolvedIngredients, recval.Width);
							ItemStack ingredStack = ((inIngred == ingred) ? stack.Clone() : inIngred?.ResolvedItemStack?.Clone());
							inSlots[y * recval.Width + x] = new DummySlot(ingredStack);
						}
					}
					ItemSlot[] inputSlots = inSlots;
					recval.GenerateOutputStack(inputSlots, outSlot);
					addToListUniquely(capi, foundRecipeOutputs, outSlot.Itemstack);
					ItemStack extraReturnedStack = ingred.ReturnedStack?.ResolvedItemStack;
					if (extraReturnedStack != null && !recval.ResolvedIngredients.Any((CraftingRecipeIngredient craftingRecipeIngredient) => craftingRecipeIngredient?.SatisfiesAsIngredient(extraReturnedStack, checkStackSize: false) ?? false))
					{
						addToListUniquely(capi, foundRecipeOutputs, extraReturnedStack);
					}
				}
			}
		}
		capi.World.FrameProfiler.Mark("handbook-recipematch-gridrecipes");
		foreach (SmithingRecipe val in capi.GetSmithingRecipes())
		{
			if (val.Ingredient.SatisfiesAsIngredient(stack, checkStackSize: false) && getIsAnvilWorkable(stack, fuels))
			{
				addToListUniquely(capi, foundRecipeOutputs, val.Output.ResolvedItemStack);
			}
		}
		foreach (ClayFormingRecipe val2 in capi.GetClayformingRecipes())
		{
			if (val2.Ingredient.SatisfiesAsIngredient(stack, checkStackSize: false))
			{
				addToListUniquely(capi, foundRecipeOutputs, val2.Output.ResolvedItemStack);
			}
		}
		foreach (KnappingRecipe val3 in capi.GetKnappingRecipes())
		{
			if (val3.Ingredient.SatisfiesAsIngredient(stack, checkStackSize: false))
			{
				addToListUniquely(capi, foundRecipeOutputs, val3.Output.ResolvedItemStack);
			}
		}
		capi.World.FrameProfiler.Mark("handbook-recipematch-smithing/clayforming/knapping");
		foreach (BarrelRecipe recipe in capi.GetBarrelRecipes())
		{
			BarrelRecipeIngredient[] ingredients = recipe.Ingredients;
			for (int num = 0; num < ingredients.Length; num++)
			{
				if (ingredients[num].SatisfiesAsIngredient(stack, checkStackSize: false))
				{
					addToListUniquely(capi, foundRecipeOutputs, recipe.RecipeOutput.ResolvedItemStack);
				}
			}
		}
		capi.World.FrameProfiler.Mark("handbook-recipematch-barrelrecipes");
		if (stack.Collectible is BlockAnvilPart)
		{
			foundRecipeOutputs.Add(new ItemStack(Api.World.GetBlock("anvil-" + stack.Collectible.Variant["metal"])));
		}
		JsonObject itemAttributes = stack.ItemAttributes;
		if ((itemAttributes != null && itemAttributes.IsTrue("isFlux")) || (stack != null && stack.Collectible?.GetTags(stack).Overlaps(in fluxTag) == true))
		{
			foreach (Block val4 in capi.World.Blocks)
			{
				if (val4 is BlockAnvilPart)
				{
					ItemStack anvil = new ItemStack(Api.World.GetBlock("anvil-" + val4.Variant["metal"]));
					addToListUniquely(capi, foundRecipeOutputs, anvil);
				}
			}
		}
		capi.World.FrameProfiler.Mark("handbook-recipematch-anvils");
		CombustibleProperties combustibleProps = stack.Collectible.GetCombustibleProperties(Api.World, stack, null);
		if (combustibleProps?.SmeltedStack?.ResolvedItemStack.Collectible is ItemIngot ingot)
		{
			foreach (ItemStack mold in molds)
			{
				if (!getCanContainerSmelt(capi, containers, fuels, stack))
				{
					break;
				}
				ItemStack smeltedStack = combustibleProps?.SmeltedStack?.ResolvedItemStack;
				if (smeltedStack != null && mold.Collectible is BlockIngotMold)
				{
					addToListUniquely(capi, foundRecipeOutputs, smeltedStack);
					continue;
				}
				string metaltype = ingot.LastCodePart();
				JsonItemStack dropStack = mold.ItemAttributes?["drop"]?.AsObject<JsonItemStack>(null, mold.Collectible.Code.Domain);
				if (dropStack != null)
				{
					dropStack.Code.Path = dropStack.Code.Path.Replace("{metal}", metaltype);
					dropStack.Resolve(capi.World, "handbookmolds", printWarningOnError: false);
					if (dropStack.ResolvedItemStack != null)
					{
						addToListUniquely(capi, foundRecipeOutputs, dropStack.ResolvedItemStack);
					}
					continue;
				}
				JsonItemStack[] dropStacks = mold.ItemAttributes?["drops"].AsObject<JsonItemStack[]>(null, mold.Collectible.Code.Domain);
				if (dropStacks == null)
				{
					continue;
				}
				JsonItemStack[] array = dropStacks;
				foreach (JsonItemStack dstack in array)
				{
					dstack.Code.Path = dstack.Code.Path.Replace("{metal}", metaltype);
					dstack.Resolve(capi.World, "handbookmolds", printWarningOnError: false);
					if (dstack.ResolvedItemStack != null)
					{
						addToListUniquely(capi, foundRecipeOutputs, dstack.ResolvedItemStack);
					}
				}
			}
		}
		capi.World.FrameProfiler.Mark("handbook-recipematch-smeltables");
		JuiceableProperties jprops = getjuiceableProps(stack);
		if (jprops != null)
		{
			ItemStack pstack = jprops.PressedStack.ResolvedItemStack.Clone();
			pstack.Attributes.SetDouble("juiceableLitresLeft", 1.0);
			if (jprops.ReturnStack != null && !stack.Equals(capi.World, pstack, GlobalConstants.IgnoredStackAttributes))
			{
				addToListUniquely(capi, foundRecipeOutputs, pstack);
			}
		}
		capi.World.FrameProfiler.Mark("handbook-recipematch-juicables");
		List<CookingRecipe> cookingrecipes = new List<CookingRecipe>();
		foreach (CookingRecipe recipe2 in capi.GetCookingRecipes())
		{
			bool canAdd = false;
			CookingRecipeIngredient[] ingredients2 = recipe2.Ingredients;
			for (int num = 0; num < ingredients2.Length; num++)
			{
				if (ingredients2[num].GetMatchingStack(stack) != null)
				{
					canAdd = true;
					break;
				}
			}
			if (canAdd)
			{
				ItemStack rStack = recipe2.CooksInto?.ResolvedItemStack;
				if (rStack != null)
				{
					addToListUniquely(capi, foundRecipeOutputs, rStack);
				}
				else
				{
					cookingrecipes.Add(recipe2);
				}
			}
		}
		capi.World.FrameProfiler.Mark("handbook-recipematch-cookingrecipes");
		List<CookingRecipe> pierecipes = new List<CookingRecipe>();
		CookingRecipe[] array2 = ObjectCacheUtil.TryGet<CookingRecipe[]>(capi, "handbookpierecipes");
		foreach (CookingRecipe recipe3 in array2)
		{
			if (recipe3.CooksInto?.ResolvedItemStack != null || pierecipes.Contains(recipe3))
			{
				continue;
			}
			bool canAdd2 = false;
			CookingRecipeIngredient[] ingredients2 = recipe3.Ingredients;
			for (int num2 = 0; num2 < ingredients2.Length; num2++)
			{
				if (ingredients2[num2].GetMatchingStack(stack) != null)
				{
					canAdd2 = true;
					break;
				}
			}
			if (canAdd2)
			{
				pierecipes.Add(recipe3);
			}
		}
		capi.World.FrameProfiler.Mark("handbook-recipematch-pierecipes");
		if (foundRecipeOutputs.Count <= 0 && cookingrecipes.Count <= 0 && pierecipes.Count <= 0)
		{
			return haveText;
		}
		AddHeading(components, capi, "Ingredient for", ref haveText);
		components.Add(new ClearFloatTextComponent(capi, 2f));
		while (foundRecipeOutputs.Count > 0)
		{
			ItemStack dstack2 = foundRecipeOutputs[0];
			foundRecipeOutputs.RemoveAt(0);
			if (dstack2 != null)
			{
				SlideshowItemstackTextComponent comp = new SlideshowItemstackTextComponent(capi, dstack2, foundRecipeOutputs, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
				{
					openDetailPageFor(getPageCodeForStack(capi, cs));
				});
				components.Add(comp);
			}
		}
		while (cookingrecipes.Count > 0)
		{
			CookingRecipe recipe4 = cookingrecipes[0];
			cookingrecipes.RemoveAt(0);
			if (recipe4 != null)
			{
				MealstackTextComponent comp2 = new MealstackTextComponent(capi, new ItemStack(BlockMeal.RandomMealBowl(capi)), recipe4, 40.0, EnumFloat.Inline, delegate
				{
					openDetailPageFor("handbook-mealrecipe-" + recipe4.Code);
				}, 4, isPie: false, stack);
				components.Add(comp2);
			}
		}
		while (pierecipes.Count > 0)
		{
			CookingRecipe recipe5 = pierecipes[0];
			pierecipes.RemoveAt(0);
			if (recipe5 != null)
			{
				ItemStack mealBlock = new ItemStack(capi.World.BlockAccessor.GetBlock("pie-perfect"));
				mealBlock.Attributes.SetInt("pieSize", 4);
				mealBlock.Attributes.SetString("topCrustType", BlockPie.TopCrustTypes[capi.World.Rand.Next(BlockPie.TopCrustTypes.Length)].Code);
				mealBlock.Attributes.SetInt("bakeLevel", 2);
				MealstackTextComponent comp3 = new MealstackTextComponent(capi, mealBlock, recipe5, 40.0, EnumFloat.Inline, delegate
				{
					openDetailPageFor("handbook-mealrecipe-" + recipe5.Code + "-pie");
				}, 6, isPie: true, stack);
				components.Add(comp3);
			}
		}
		capi.World.FrameProfiler.Mark("handbook-recipematch-addcomponents");
		components.Add(new ClearFloatTextComponent(capi, 3f));
		return haveText;
	}

	protected bool addCreatedByInfo(ICoreClientAPI capi, ItemStack[] allStacks, ActionConsumable<string> openDetailPageFor, ItemStack stack, List<RichTextComponentBase> components, float marginTop, List<ItemStack> containers, List<ItemStack> fuels, List<ItemStack> molds, List<ItemStack> anvils, bool haveText)
	{
		bool anvilweldable = false;
		List<KnappingRecipe> knaprecipes = new List<KnappingRecipe>();
		foreach (KnappingRecipe val in capi.GetKnappingRecipes())
		{
			if (val.Output.ResolvedItemStack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
			{
				knaprecipes.Add(val);
			}
		}
		List<ClayFormingRecipe> clayrecipes = new List<ClayFormingRecipe>();
		foreach (ClayFormingRecipe val2 in capi.GetClayformingRecipes())
		{
			if (val2.Output.ResolvedItemStack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
			{
				clayrecipes.Add(val2);
			}
		}
		clayrecipes = clayrecipes.OrderBy((ClayFormingRecipe clayFormingRecipe) => clayFormingRecipe.Output.ResolvedItemStack?.StackSize).ToList();
		if (stack.Collectible is BlockAnvil && capi.World.GetBlock("anvilpart-base-" + stack.Collectible.Variant["metal"]) != null)
		{
			anvilweldable = true;
		}
		List<GridRecipe> grecipes = new List<GridRecipe>();
		foreach (GridRecipe recipe in capi.World.GridRecipes)
		{
			if (!recipe.ShowInCreatedBy)
			{
				continue;
			}
			ItemStack? resolvedItemStack = recipe.RecipeOutput.ResolvedItemStack;
			if (resolvedItemStack != null && resolvedItemStack.Satisfies(stack))
			{
				grecipes.Add(recipe);
				continue;
			}
			CraftingRecipeIngredient[] resolvedIngredients = recipe.ResolvedIngredients;
			foreach (CraftingRecipeIngredient ingred in resolvedIngredients)
			{
				ItemStack rstack = ingred?.ReturnedStack?.ResolvedItemStack;
				if (rstack != null && rstack.Satisfies(stack))
				{
					ItemStack? resolvedItemStack2 = ingred.ResolvedItemStack;
					if (resolvedItemStack2 != null && !resolvedItemStack2.Satisfies(stack))
					{
						grecipes.Add(recipe);
						break;
					}
				}
			}
		}
		List<CookingRecipe> cookrecipes = new List<CookingRecipe>();
		foreach (CookingRecipe val3 in capi.GetCookingRecipes())
		{
			JsonItemStack? cooksInto = val3.CooksInto;
			if (cooksInto != null && cooksInto.ResolvedItemStack?.Satisfies(stack) == true)
			{
				cookrecipes.Add(val3);
			}
		}
		List<SmithingRecipe> smithingrecipes = new List<SmithingRecipe>();
		foreach (SmithingRecipe val4 in capi.GetSmithingRecipes())
		{
			ItemStack? resolvedItemStack3 = val4.Output.ResolvedItemStack;
			if (resolvedItemStack3 != null && resolvedItemStack3.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes) && getIsAnvilWorkable(val4.Ingredient.ResolvedItemStack, fuels))
			{
				smithingrecipes.Add(val4);
			}
		}
		smithingrecipes = smithingrecipes.OrderBy((SmithingRecipe smithingRecipe) => smithingRecipe.Output.ResolvedItemStack?.StackSize).ToList();
		List<ItemStack> moldStacks = new List<ItemStack>();
		string maybeMetalType = stack.Collectible.LastCodePart();
		foreach (ItemStack mold in molds)
		{
			if (mold.Collectible is BlockIngotMold && stack.Collectible is ItemIngot)
			{
				moldStacks.Add(mold);
				continue;
			}
			JsonItemStack dropStack = mold.ItemAttributes?["drop"]?.AsObject<JsonItemStack>(null, mold.Collectible.Code.Domain);
			if (dropStack != null)
			{
				dropStack.Code.Path = dropStack.Code.Path.Replace("{metal}", maybeMetalType);
				dropStack.Resolve(capi.World, "handbookmolds", printWarningOnError: false);
				ItemStack? resolvedItemStack4 = dropStack.ResolvedItemStack;
				if (resolvedItemStack4 != null && resolvedItemStack4.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
				{
					moldStacks.Add(mold);
				}
				continue;
			}
			JsonItemStack[] dropStacks = mold.ItemAttributes?["drops"].AsObject<JsonItemStack[]>(null, mold.Collectible.Code.Domain);
			if (dropStacks == null)
			{
				continue;
			}
			JsonItemStack[] array = dropStacks;
			foreach (JsonItemStack dstack in array)
			{
				dstack.Code.Path = dstack.Code.Path.Replace("{metal}", maybeMetalType);
				dstack.Resolve(capi.World, "handbookmolds", printWarningOnError: false);
				ItemStack? resolvedItemStack5 = dstack.ResolvedItemStack;
				if (resolvedItemStack5 != null && resolvedItemStack5.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
				{
					moldStacks.Add(mold);
				}
			}
		}
		List<ItemStack> knappables = new List<ItemStack>();
		List<ItemStack> clayformables = new List<ItemStack>();
		List<ItemStack> metalworkables = new List<ItemStack>();
		List<ItemStack> metalmoldables = new List<ItemStack>();
		List<ItemStack> bakables = new List<ItemStack>();
		List<ItemStack> bloomeryables = new List<ItemStack>();
		List<ItemStack> kilnables = new List<ItemStack>();
		List<ItemStack> carburizables = new List<ItemStack>();
		List<ItemStack> grindables = new List<ItemStack>();
		List<ItemStack> crushables = new List<ItemStack>();
		List<ItemStack> juiceables = new List<ItemStack>();
		List<ItemStack> squeezables = new List<ItemStack>();
		Dictionary<string, List<ItemStack>> groundstoredprocessables = new Dictionary<string, List<ItemStack>>();
		List<ItemStack> distillables = new List<ItemStack>();
		List<ItemStack> smashables = new List<ItemStack>();
		List<ItemStack> fluxes = new List<ItemStack>();
		Dictionary<EnumTransitionType, List<ItemStack>> transitionables = new Dictionary<EnumTransitionType, List<ItemStack>>();
		List<ItemStack> validanvils = anvils;
		List<ItemStack> allQuerns = new List<ItemStack>();
		List<ItemStack> allFruitpresses = new List<ItemStack>();
		int num = 0;
		while (num < allStacks.Length)
		{
			ItemStack val5 = allStacks[num];
			if (val5.Collectible is BlockFruitPress)
			{
				addToListUniquely(capi, allFruitpresses, val5);
			}
			if (val5.Collectible is BlockQuern)
			{
				addToListUniquely(capi, allQuerns, val5);
			}
			CombustibleProperties combustibleProps = val5.Collectible.GetCombustibleProperties(Api.World, val5, null);
			ItemStack smeltedStack = combustibleProps?.SmeltedStack?.ResolvedItemStack;
			if (smeltedStack != null && smeltedStack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes) && !val5.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
			{
				if (getCanBloomerySmelt(val5) && !bloomeryables.Any((ItemStack s) => s.Equals(capi.World, val5, GlobalConstants.IgnoredStackAttributes)))
				{
					ItemStack bloomeryStack = val5.Clone();
					bloomeryStack.StackSize = combustibleProps.SmeltedRatio;
					bloomeryables.Add(bloomeryStack);
				}
				if (combustibleProps.SmeltingType == EnumSmeltType.Fire && !kilnables.Any((ItemStack s) => s.Equals(capi.World, val5, GlobalConstants.IgnoredStackAttributes)))
				{
					kilnables.Add(val5);
				}
				else if (getCanSmelt(fuels, val5) && !bakables.Any((ItemStack s) => s.Equals(capi.World, val5, GlobalConstants.IgnoredStackAttributes)))
				{
					ItemStack bakeStack = val5.Clone();
					bakeStack.StackSize = combustibleProps.SmeltedRatio;
					bakables.Add(bakeStack);
				}
			}
			if (smeltedStack?.Collectible is ItemIngot && moldStacks.Count > 0 && smeltedStack?.Collectible.Variant["metal"] == maybeMetalType && getCanContainerSmelt(capi, containers, fuels, val5))
			{
				metalmoldables.Add(val5);
			}
			if (smithingrecipes.Count > 0 && !val5.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
			{
				IAnvilWorkable workable = val5.Collectible.GetCollectibleInterface<IAnvilWorkable>();
				IAnvilWorkable anvilWorkable = workable;
				if (anvilWorkable != null && anvilWorkable.GetMatchingRecipes(val5).Intersect(smithingrecipes).Any())
				{
					metalworkables.Add(val5);
					validanvils = validanvils.Where((ItemStack anvil) => workable.GetRequiredAnvilTier(val5) <= (anvil.Block as BlockAnvil).MetalTier).ToList();
				}
			}
			if (knaprecipes.Count > 0 && !val5.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes) && knaprecipes.Where((KnappingRecipe knappingRecipe) => knappingRecipe.Ingredient.SatisfiesAsIngredient(val5, checkStackSize: false)).FirstOrDefault() != null)
			{
				knappables.Add(val5);
			}
			if (clayrecipes.Count > 0 && !val5.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
			{
				ClayFormingRecipe recipe2 = clayrecipes.Where((ClayFormingRecipe clayFormingRecipe) => clayFormingRecipe.Ingredient.SatisfiesAsIngredient(val5, checkStackSize: false)).FirstOrDefault();
				if (recipe2 != null)
				{
					ItemStack clayStack = val5.Clone();
					clayStack.StackSize = (int)Math.Ceiling(GameMath.Max(1f, (float)(recipe2.Voxels.Cast<bool>().Count((bool voxel) => voxel) - 64) / 25f));
					clayformables.Add(clayStack);
				}
			}
			Dictionary<string, JsonItemStack> beehiveKilnProps = val5.ItemAttributes?["beehivekiln"].AsObject<Dictionary<string, JsonItemStack>>();
			if (beehiveKilnProps != null)
			{
				foreach (JsonItemStack beehiveStack in beehiveKilnProps.Values)
				{
					if (beehiveStack != null && beehiveStack.Resolve(capi.World, "beehivekiln-burn") && beehiveStack.ResolvedItemStack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
					{
						addToListUniquely(capi, kilnables, val5);
						break;
					}
				}
			}
			JsonItemStack carburizedJStack = val5.ItemAttributes?["carburizableProps"]?["carburizedOutput"]?.AsObject<JsonItemStack>(null, val5.Collectible.Code.Domain);
			if (carburizedJStack != null && carburizedJStack.Resolve(Api.World, "carburizable handbook"))
			{
				ItemStack carburizedStack = carburizedJStack.ResolvedItemStack;
				if (carburizedStack != null && carburizedStack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
				{
					addToListUniquely(capi, carburizables, val5);
				}
			}
			ItemStack groundStack = val5.Collectible.GetGrindingProperties(Api.World, val5)?.GroundStack.ResolvedItemStack;
			if (groundStack != null && groundStack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
			{
				addToListUniquely(capi, grindables, val5);
			}
			ItemStack crushedStack = val5.Collectible.GetCrushingProperties(Api.World, val5)?.CrushedStack.ResolvedItemStack;
			if (crushedStack != null && crushedStack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
			{
				addToListUniquely(capi, crushables, val5);
			}
			if (val5.Collectible is ItemOre)
			{
				string type = val5.Collectible.Variant["ore"].Replace("quartz_", "").Replace("galena_", "");
				Item item = capi.World.GetItem(new AssetLocation("nugget-" + type));
				if (item != null && new ItemStack(item).Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
				{
					addToListUniquely(capi, smashables, val5);
				}
			}
			JsonObject itemAttributes = val5.ItemAttributes;
			ItemStack pressedStack;
			ItemStack returnStack;
			ItemStack juiceableStack;
			if (itemAttributes != null && itemAttributes["juiceableProperties"].Exists)
			{
				JuiceableProperties fjprops = getjuiceableProps(val5);
				if (fjprops != null)
				{
					ItemStack juicedStack = fjprops.LiquidStack?.ResolvedItemStack;
					pressedStack = fjprops.PressedStack?.ResolvedItemStack;
					returnStack = fjprops.ReturnStack?.ResolvedItemStack;
					float ratio = 5f / (fjprops.LitresPerItem ?? 5f);
					juiceableStack = val5.Clone();
					ITreeAttribute attributes = val5.Attributes;
					if (attributes != null && attributes.GetDouble("juiceableLitresLeft") > 0.0)
					{
						juiceableStack.Attributes.SetDouble("juiceableLitresLeft", 5.0);
					}
					else
					{
						juiceableStack.StackSize = (int)Math.Ceiling(ratio);
					}
					if (juicedStack != null && juicedStack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes) && !juiceables.Any((ItemStack s) => s.Equals(capi.World, val5, GlobalConstants.IgnoredStackAttributes)))
					{
						if (!fjprops.LitresPerItem.HasValue)
						{
							ITreeAttribute attributes2 = val5.Attributes;
							if (attributes2 == null || !(attributes2.GetDouble("juiceableLitresLeft") > 0.0))
							{
								goto IL_0f89;
							}
						}
						juiceables.Add(juiceableStack);
					}
					goto IL_0f89;
				}
			}
			goto IL_104b;
			IL_0f89:
			if (pressedStack != null && pressedStack.Equals(capi.World, stack, juiceableIgnoredAttributes) && !juiceables.Any((ItemStack s) => s.Equals(capi.World, val5, GlobalConstants.IgnoredStackAttributes)) && !val5.Equals(capi.World, pressedStack, juiceableIgnoredAttributes))
			{
				juiceables.Add(juiceableStack);
			}
			if (returnStack != null && returnStack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes) && !juiceables.Any((ItemStack s) => s.Equals(capi.World, val5, GlobalConstants.IgnoredStackAttributes)))
			{
				juiceables.Add(juiceableStack);
			}
			goto IL_104b;
			IL_142b:
			TransitionableProperties[] oprops = val5.Collectible.GetTransitionableProperties(capi.World, val5, null);
			if (oprops != null)
			{
				TransitionableProperties[] array2 = oprops;
				foreach (TransitionableProperties prop in array2)
				{
					if (prop.Type == EnumTransitionType.None)
					{
						continue;
					}
					ItemStack transitionedStack = prop.TransitionedStack?.ResolvedItemStack;
					ItemStack itemStack = transitionedStack;
					if (itemStack == null || !itemStack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
					{
						continue;
					}
					if (transitionables.TryGetValue(prop.Type, out var transitionable))
					{
						if (!transitionable.Any((ItemStack s) => s.Equals(capi.World, transitionedStack, GlobalConstants.IgnoredStackAttributes)))
						{
							transitionable.Add(val5);
						}
						continue;
					}
					EnumTransitionType type2 = prop.Type;
					int num3 = 1;
					List<ItemStack> list = new List<ItemStack>(num3);
					CollectionsMarshal.SetCount(list, num3);
					Span<ItemStack> span = CollectionsMarshal.AsSpan(list);
					int index = 0;
					span[index] = val5;
					transitionables[type2] = list;
				}
			}
			num++;
			continue;
			IL_104b:
			CollectibleBehaviorSqueezable cbs = val5.Collectible.GetCollectibleBehavior<CollectibleBehaviorSqueezable>(withInheritance: true);
			if (cbs != null && cbs.ReturnStacks != null)
			{
				JsonItemStack[] array = cbs.ReturnStacks;
				foreach (JsonItemStack returnStack2 in array)
				{
					if (returnStack2?.ResolvedItemStack != null && stack.Equals(capi.World, returnStack2.ResolvedItemStack, GlobalConstants.IgnoredStackAttributes) && !squeezables.Any((ItemStack s) => s.Equals(capi.World, val5, GlobalConstants.IgnoredStackAttributes)))
					{
						squeezables.Add(val5);
					}
				}
				if (cbs.SqueezedLiquid != null)
				{
					ItemStack liquidStack = new ItemStack(cbs.SqueezedLiquid);
					if (stack.Equals(capi.World, liquidStack, GlobalConstants.IgnoredStackAttributes) && !squeezables.Any((ItemStack s) => s.Equals(capi.World, val5, GlobalConstants.IgnoredStackAttributes)))
					{
						squeezables.Add(val5);
					}
				}
			}
			CollectibleBehaviorGroundStoredProcessable gsp = val5.Collectible.GetCollectibleBehavior<CollectibleBehaviorGroundStoredProcessable>(withInheritance: true);
			if (gsp != null)
			{
				if (gsp.ProcessedStacks != null)
				{
					BlockDropItemStack[] processedStacks = gsp.ProcessedStacks;
					for (int num3 = 0; num3 < processedStacks.Length; num3++)
					{
						ItemStack pStack = processedStacks[num3]?.ResolvedItemstack;
						if (pStack != null && stack.Equals(capi.World, pStack, GlobalConstants.IgnoredStackAttributes))
						{
							if (groundstoredprocessables.TryGetValue(gsp.HandbookCreatedByTitle, out var gspList))
							{
								addToListUniquely(capi, gspList, val5);
								continue;
							}
							string? handbookCreatedByTitle = gsp.HandbookCreatedByTitle;
							int index = 1;
							List<ItemStack> list2 = new List<ItemStack>(index);
							CollectionsMarshal.SetCount(list2, index);
							Span<ItemStack> span2 = CollectionsMarshal.AsSpan(list2);
							int num2 = 0;
							span2[num2] = val5;
							groundstoredprocessables[handbookCreatedByTitle] = list2;
						}
					}
				}
				ItemStack rStack = gsp.RemainingItem?.ResolvedItemStack;
				if (rStack != null && stack.Equals(capi.World, rStack, GlobalConstants.IgnoredStackAttributes))
				{
					if (groundstoredprocessables.TryGetValue(gsp.HandbookCreatedByTitle, out var gspList2))
					{
						addToListUniquely(capi, gspList2, val5);
					}
					else
					{
						string? handbookCreatedByTitle2 = gsp.HandbookCreatedByTitle;
						int num3 = 1;
						List<ItemStack> list3 = new List<ItemStack>(num3);
						CollectionsMarshal.SetCount(list3, num3);
						Span<ItemStack> span3 = CollectionsMarshal.AsSpan(list3);
						int num2 = 0;
						span3[num2] = val5;
						groundstoredprocessables[handbookCreatedByTitle2] = list3;
					}
				}
			}
			JsonObject itemAttributes2 = val5.ItemAttributes;
			if (itemAttributes2 != null && itemAttributes2["distillationProps"].Exists)
			{
				ItemStack distilledStack = getDistillationProps(val5)?.DistilledStack?.ResolvedItemStack;
				if (distilledStack != null && distilledStack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
				{
					addToListUniquely(capi, distillables, val5);
				}
			}
			JsonObject itemAttributes3 = val5.ItemAttributes;
			if (itemAttributes3 == null || !itemAttributes3.IsTrue("isFlux"))
			{
				CollectibleObject collectible = val5.Collectible;
				if (collectible == null || !collectible.GetTags(stack).Overlaps(in fluxTag))
				{
					goto IL_142b;
				}
			}
			addToListUniquely(capi, fluxes, val5);
			goto IL_142b;
		}
		List<RichTextComponentBase> barrelRecipestext = BuildBarrelRecipesText(capi, stack, openDetailPageFor);
		string customCreatedBy = stack.Collectible.Attributes?["handbook"]?["createdBy"]?.AsString();
		string bakingInitialIngredient = collObj.Attributes?["bakingProperties"]?.AsObject<BakingProperties>()?.InitialCode;
		if (grecipes.Count > 0 || cookrecipes.Count > 0 || (metalmoldables.Count > 0 && moldStacks.Count > 0) || (metalworkables.Count > 0 && validanvils.Count > 0) || knappables.Count > 0 || clayformables.Count > 0 || anvilweldable || customCreatedBy != null || bakables.Count > 0 || bloomeryables.Count > 0 || kilnables.Count > 0 || carburizables.Count > 0 || (allQuerns.Count > 0 && grindables.Count > 0) || transitionables.Count > 0 || crushables.Count > 0 || barrelRecipestext.Count > 0 || bakingInitialIngredient != null || (juiceables.Count > 0 && allFruitpresses.Count > 0) || squeezables.Count > 0 || groundstoredprocessables.Count > 0 || distillables.Count > 0 || smashables.Count > 0)
		{
			AddHeading(components, capi, "Created by", ref haveText);
			ClearFloatTextComponent verticalSpaceSmall = new ClearFloatTextComponent(capi, 7f);
			ClearFloatTextComponent verticalSpace = new ClearFloatTextComponent(capi, 3f);
			if (customCreatedBy != null)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				RichTextComponent comp = new RichTextComponent(capi, "• ", CairoFont.WhiteSmallText());
				comp.PaddingLeft = 2.0;
				components.Add(comp);
				components.AddRange(VtmlUtil.Richtextify(capi, Lang.Get(customCreatedBy) + "\n", CairoFont.WhiteSmallText()));
			}
			if (metalmoldables.Count > 0 && moldStacks.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "handbook-createdby-metalmolding", "craftinginfo-smelting");
				int firstPadding = 2;
				while (moldStacks.Count > 0)
				{
					ItemStack dstack2 = moldStacks[0];
					moldStacks.RemoveAt(0);
					if (dstack2 != null)
					{
						ItemStack[] sizedMoldables = metalmoldables.Select(delegate(ItemStack moldable)
						{
							ItemStack itemStack2 = moldable.Clone();
							itemStack2.StackSize = (dstack2.ItemAttributes?["requiredUnits"].AsInt(100) ?? 100) / 5;
							return itemStack2;
						}).ToArray();
						ItemStack[] output = getMoldedOutput(dstack2, sizedMoldables);
						SlideshowItemstackTextComponent comp2 = new SlideshowItemstackTextComponent(capi, sizedMoldables, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
						{
							openDetailPageFor(getPageCodeForStack(capi, cs));
						});
						comp2.ShowStackSize = true;
						comp2.PaddingLeft = firstPadding;
						firstPadding = 0;
						components.Add(comp2);
						RichTextComponent cmp = new RichTextComponent(capi, " + ", CairoFont.WhiteMediumText());
						cmp.VerticalAlign = EnumVerticalAlign.Middle;
						components.Add(cmp);
						SlideshowItemstackTextComponent mcomp = new SlideshowItemstackTextComponent(capi, dstack2, moldStacks, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
						{
							openDetailPageFor(getPageCodeForStack(capi, cs));
						});
						mcomp.PaddingLeft = firstPadding;
						components.Add(mcomp);
						cmp = new RichTextComponent(capi, " = ", CairoFont.WhiteMediumText());
						cmp.VerticalAlign = EnumVerticalAlign.Middle;
						components.Add(cmp);
						SlideshowItemstackTextComponent ocomp = new SlideshowItemstackTextComponent(capi, output, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
						{
							openDetailPageFor(getPageCodeForStack(capi, cs));
						});
						ocomp.ShowStackSize = true;
						ocomp.PaddingLeft = firstPadding;
						components.Add(ocomp);
						components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
					}
				}
			}
			if (metalworkables.Count > 0 && validanvils.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "Smithing", "craftinginfo-smithing");
				while (smithingrecipes.Count > 0)
				{
					SmithingRecipe recipe3 = smithingrecipes[0];
					smithingrecipes.RemoveAt(0);
					if (recipe3 == null)
					{
						continue;
					}
					List<ItemStack> workableanvils = validanvils;
					ItemStack[] sizedWorkables = metalworkables.Select(delegate(ItemStack itemStack2)
					{
						IAnvilWorkable collectibleInterface = itemStack2.Collectible.GetCollectibleInterface<IAnvilWorkable>();
						int requiredAnvilTier = collectibleInterface.GetRequiredAnvilTier(itemStack2);
						ItemStack itemStack3 = itemStack2.Clone();
						itemStack3.StackSize = (int)Math.Ceiling((double)recipe3.Voxels.Cast<bool>().Count((bool voxel) => voxel) / (double)collectibleInterface.VoxelCountForHandbook(itemStack2));
						workableanvils = workableanvils.Where((ItemStack anvil) => (anvil.Block as BlockAnvil).MetalTier >= requiredAnvilTier).ToList();
						return itemStack3;
					}).ToArray();
					SlideshowItemstackTextComponent comp3 = new SlideshowItemstackTextComponent(capi, sizedWorkables, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
					{
						openDetailPageFor(getPageCodeForStack(capi, cs));
					});
					comp3.ShowStackSize = true;
					comp3.PaddingLeft = 2.0;
					components.Add(comp3);
					RichTextComponent cmp2 = new RichTextComponent(capi, " + ", CairoFont.WhiteMediumText());
					cmp2.PaddingLeft = 10.0;
					cmp2.VerticalAlign = EnumVerticalAlign.Middle;
					components.Add(cmp2);
					SlideshowItemstackTextComponent acomp = new SlideshowItemstackTextComponent(capi, workableanvils.ToArray(), 40.0, EnumFloat.Inline, delegate(ItemStack cs)
					{
						openDetailPageFor(getPageCodeForStack(capi, cs));
					});
					acomp.PaddingLeft = 0.0;
					components.Add(acomp);
					cmp2 = new RichTextComponent(capi, " = ", CairoFont.WhiteMediumText());
					cmp2.PaddingLeft = 10.0;
					cmp2.VerticalAlign = EnumVerticalAlign.Middle;
					components.Add(cmp2);
					ItemstackTextComponent ocomp2 = new ItemstackTextComponent(capi, recipe3.Output.ResolvedItemStack, 40.0, 0.0, EnumFloat.Inline);
					ocomp2.ShowStacksize = true;
					ocomp2.PaddingLeft = 0.0;
					components.Add(ocomp2);
					components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
				}
			}
			if (knappables.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "Knapping", "craftinginfo-knapping");
				AddSlideShowComponent(components, capi, knappables, openDetailPageFor, showStackSize: true);
				components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
			}
			if (clayformables.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "Clay forming", "craftinginfo-clayforming");
				AddSlideShowComponent(components, capi, clayformables, openDetailPageFor, showStackSize: true);
				components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
			}
			if (anvilweldable)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "handbook-createdby-anvilwelding", "gamemechanicinfo-steelmaking");
				ItemstackTextComponent baseComp = new ItemstackTextComponent(capi, new ItemStack(capi.World.GetBlock(new AssetLocation("anvilpart-base-" + stack.Collectible.Variant["metal"]))), 40.0, 0.0, EnumFloat.Inline, delegate(ItemStack cs)
				{
					openDetailPageFor(getPageCodeForStack(capi, cs));
				});
				baseComp.PaddingLeft = 2.0;
				components.Add(baseComp);
				RichTextComponent cmp3 = new RichTextComponent(capi, " + ", CairoFont.WhiteMediumText());
				cmp3.PaddingLeft = 10.0;
				cmp3.VerticalAlign = EnumVerticalAlign.Middle;
				components.Add(cmp3);
				SlideshowItemstackTextComponent fluxComp = new SlideshowItemstackTextComponent(capi, fluxes.ToArray(), 40.0, EnumFloat.Inline, delegate(ItemStack cs)
				{
					openDetailPageFor(getPageCodeForStack(capi, cs));
				});
				fluxComp.PaddingLeft = 10.0;
				components.Add(fluxComp);
				cmp3 = new RichTextComponent(capi, " + ", CairoFont.WhiteMediumText());
				cmp3.PaddingLeft = 10.0;
				cmp3.VerticalAlign = EnumVerticalAlign.Middle;
				components.Add(cmp3);
				ItemstackTextComponent topComp = new ItemstackTextComponent(capi, new ItemStack(capi.World.GetBlock(new AssetLocation("anvilpart-top-" + stack.Collectible.Variant["metal"]))), 40.0, 0.0, EnumFloat.Inline, delegate(ItemStack cs)
				{
					openDetailPageFor(getPageCodeForStack(capi, cs));
				});
				topComp.PaddingLeft = 0.0;
				components.Add(topComp);
				cmp3 = new RichTextComponent(capi, " = ", CairoFont.WhiteMediumText());
				cmp3.PaddingLeft = 10.0;
				cmp3.VerticalAlign = EnumVerticalAlign.Middle;
				components.Add(cmp3);
				ItemstackTextComponent ocomp3 = new ItemstackTextComponent(capi, stack, 40.0);
				ocomp3.PaddingLeft = 0.0;
				components.Add(ocomp3);
				components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
			}
			if (carburizables.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "handbook-createdby-carburizing", "gamemechanicinfo-steelmaking");
				AddSlideShowComponent(components, capi, carburizables, openDetailPageFor, showStackSize: true);
				components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
			}
			if (allQuerns.Count > 0 && grindables.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "Grinding", null);
				grindables.Sort(delegate(ItemStack left, ItemStack right)
				{
					GrindingProperties grindingProperties = left.Collectible.GetGrindingProperties(Api.World, left);
					GrindingProperties grindingProperties2 = right.Collectible.GetGrindingProperties(Api.World, right);
					ItemStack resolvedItemStack6 = grindingProperties.GroundStack.ResolvedItemStack;
					ItemStack resolvedItemStack7 = grindingProperties2.GroundStack.ResolvedItemStack;
					return (!resolvedItemStack6.Equals(capi.World, resolvedItemStack7, GlobalConstants.IgnoredStackAttributes)) ? (-1) : resolvedItemStack6.StackSize.CompareTo(resolvedItemStack7.StackSize);
				});
				while (grindables.Count > 0)
				{
					IEnumerable<ItemStack> currentGrindables = grindables.TakeWhile(delegate(ItemStack grindable, int num4)
					{
						if (num4 <= 0)
						{
							return true;
						}
						if (num4 >= grindables.Count)
						{
							return true;
						}
						ItemStack itemStack2 = grindables[num4 - 1];
						GrindingProperties grindingProperties = itemStack2.Collectible.GetGrindingProperties(Api.World, itemStack2);
						GrindingProperties grindingProperties2 = grindable.Collectible.GetGrindingProperties(Api.World, grindable);
						ItemStack resolvedItemStack6 = grindingProperties.GroundStack.ResolvedItemStack;
						ItemStack resolvedItemStack7 = grindingProperties2.GroundStack.ResolvedItemStack;
						return resolvedItemStack6.Equals(capi.World, resolvedItemStack7, GlobalConstants.IgnoredStackAttributes) && resolvedItemStack6.StackSize == resolvedItemStack7.StackSize;
					});
					grindables = grindables.Skip(currentGrindables.Count()).ToList();
					SlideshowItemstackTextComponent comp4 = new SlideshowItemstackTextComponent(capi, currentGrindables.ToArray(), 40.0, EnumFloat.Inline, delegate(ItemStack cs)
					{
						openDetailPageFor(getPageCodeForStack(capi, cs));
					});
					comp4.ShowStackSize = true;
					comp4.PaddingLeft = 2.0;
					components.Add(comp4);
					RichTextComponent cmp4 = new RichTextComponent(capi, " + ", CairoFont.WhiteMediumText());
					cmp4.PaddingLeft = 10.0;
					cmp4.VerticalAlign = EnumVerticalAlign.Middle;
					components.Add(cmp4);
					SlideshowItemstackTextComponent acomp2 = new SlideshowItemstackTextComponent(capi, allQuerns.ToArray(), 40.0, EnumFloat.Inline, delegate(ItemStack cs)
					{
						openDetailPageFor(getPageCodeForStack(capi, cs));
					});
					acomp2.PaddingLeft = 0.0;
					components.Add(acomp2);
					cmp4 = new RichTextComponent(capi, " = ", CairoFont.WhiteMediumText());
					cmp4.PaddingLeft = 10.0;
					cmp4.VerticalAlign = EnumVerticalAlign.Middle;
					components.Add(cmp4);
					GrindingProperties grindingProps = currentGrindables.First().Collectible.GetGrindingProperties(Api.World, currentGrindables.First());
					ItemstackTextComponent ocomp4 = new ItemstackTextComponent(capi, grindingProps?.GroundStack.ResolvedItemStack, 40.0, 0.0, EnumFloat.Inline, delegate(ItemStack cs)
					{
						openDetailPageFor(getPageCodeForStack(capi, cs));
					});
					ocomp4.ShowStacksize = true;
					ocomp4.PaddingLeft = 2.0;
					components.Add(ocomp4);
					components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
				}
			}
			if (crushables.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "Crushing", null);
				AddSlideShowComponent(components, capi, crushables, openDetailPageFor, showStackSize: true);
				components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
			}
			if (smashables.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "handbook-createdby-smashing", null);
				AddSlideShowComponent(components, capi, smashables, openDetailPageFor, showStackSize: true);
				components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
			}
			foreach (KeyValuePair<EnumTransitionType, List<ItemStack>> transitionable2 in transitionables)
			{
				if (transitionable2.Value.Count > 0)
				{
					components.Add(verticalSpace);
					verticalSpace = verticalSpaceSmall;
					AddSubHeading(components, capi, openDetailPageFor, "handbook-createdby-transition-" + transitionable2.Key.ToString().ToLowerInvariant(), null);
					AddSlideShowComponent(components, capi, transitionable2.Value, openDetailPageFor, showStackSize: false);
				}
			}
			if (bakables.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "Cooking/Smelting/Baking", null);
				AddSlideShowComponent(components, capi, bakables, openDetailPageFor, showStackSize: true);
				components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
			}
			if (bloomeryables.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "handbook-createdby-bloomerysmelting", null);
				AddSlideShowComponent(components, capi, bloomeryables, openDetailPageFor, showStackSize: true);
				components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
			}
			if (kilnables.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "handbook-createdby-kilnfiring", null);
				AddSlideShowComponent(components, capi, kilnables, openDetailPageFor, showStackSize: false);
				components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
			}
			if (allFruitpresses.Count > 0 && juiceables.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "Juicing", "block-fruitpress-ns");
				foreach (ItemStack juiceable in juiceables)
				{
					ItemstackTextComponent comp5 = new ItemstackTextComponent(capi, juiceable, 40.0, 0.0, EnumFloat.Inline, delegate(ItemStack cs)
					{
						openDetailPageFor(getPageCodeForStack(capi, cs));
					});
					comp5.ShowStacksize = true;
					comp5.PaddingLeft = 2.0;
					components.Add(comp5);
					RichTextComponent cmp5 = new RichTextComponent(capi, " + ", CairoFont.WhiteMediumText());
					cmp5.PaddingLeft = 10.0;
					cmp5.VerticalAlign = EnumVerticalAlign.Middle;
					components.Add(cmp5);
					SlideshowItemstackTextComponent acomp3 = new SlideshowItemstackTextComponent(capi, allFruitpresses.ToArray(), 40.0, EnumFloat.Inline, delegate(ItemStack cs)
					{
						openDetailPageFor(getPageCodeForStack(capi, cs));
					});
					acomp3.PaddingLeft = 0.0;
					components.Add(acomp3);
					cmp5 = new RichTextComponent(capi, " = ", CairoFont.WhiteMediumText());
					cmp5.PaddingLeft = 10.0;
					cmp5.VerticalAlign = EnumVerticalAlign.Middle;
					components.Add(cmp5);
					ITreeAttribute attributes3 = stack.Attributes;
					if (attributes3 != null && attributes3.GetDouble("juiceableLitresLeft") > 0.0)
					{
						ItemStack pstack = getjuiceableProps(juiceable).PressedStack.ResolvedItemStack.Clone();
						pstack.Attributes.SetDouble("juiceableLitresLeft", 5.0);
						ItemstackTextComponent ocomp5 = new ItemstackTextComponent(capi, pstack, 40.0, 0.0, EnumFloat.Inline, delegate(ItemStack cs)
						{
							openDetailPageFor(getPageCodeForStack(capi, cs));
						});
						ocomp5.PaddingLeft = 0.0;
						components.Add(ocomp5);
					}
					else
					{
						JuiceableProperties jprops = getjuiceableProps(juiceable);
						ItemStack jstack = jprops.LiquidStack.ResolvedItemStack.Clone();
						jstack.StackSize = (int)(BlockLiquidContainerBase.GetContainableProps(jstack).ItemsPerLitre * 5f);
						ItemstackTextComponent ocomp6 = new ItemstackTextComponent(capi, jstack, 40.0, 0.0, EnumFloat.Inline, delegate(ItemStack cs)
						{
							openDetailPageFor(getPageCodeForStack(capi, cs));
						});
						ocomp6.ShowStacksize = true;
						ocomp6.PaddingLeft = 2.0;
						components.Add(ocomp6);
						if (jprops.ReturnStack?.ResolvedItemStack == null)
						{
							ItemStack pstack2 = jprops.PressedStack.ResolvedItemStack.Clone();
							pstack2.StackSize = (int)(5f * jprops.PressedDryRatio);
							ocomp6 = new ItemstackTextComponent(capi, pstack2, 40.0, 0.0, EnumFloat.Inline, delegate(ItemStack cs)
							{
								openDetailPageFor(getPageCodeForStack(capi, cs));
							});
							ocomp6.ShowStacksize = true;
							ocomp6.PaddingLeft = 0.0;
							components.Add(ocomp6);
						}
						else
						{
							ItemStack rstack2 = jprops.ReturnStack.ResolvedItemStack.Clone();
							rstack2.StackSize *= 5;
							ocomp6 = new ItemstackTextComponent(capi, rstack2, 40.0, 0.0, EnumFloat.Inline, delegate(ItemStack cs)
							{
								openDetailPageFor(getPageCodeForStack(capi, cs));
							});
							ocomp6.ShowStacksize = true;
							ocomp6.PaddingLeft = 0.0;
							components.Add(ocomp6);
						}
					}
					components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
				}
			}
			if (squeezables.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "handbook-createdby-squeezing", null);
				AddSlideShowComponent(components, capi, squeezables, openDetailPageFor, showStackSize: false);
				components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
			}
			foreach (var (processTitle, processables) in groundstoredprocessables)
			{
				if (processables.Count > 0)
				{
					components.Add(verticalSpace);
					verticalSpace = verticalSpaceSmall;
					AddSubHeading(components, capi, openDetailPageFor, processTitle, null);
					AddSlideShowComponent(components, capi, processables, openDetailPageFor, showStackSize: false);
				}
			}
			if (distillables.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "Distillation", "craftinginfo-alcohol");
				AddSlideShowComponent(components, capi, distillables, openDetailPageFor, showStackSize: false);
				components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
			}
			if (bakingInitialIngredient != null)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "Baking (in oven)", null);
				CollectibleObject cobj = capi.World.GetItem(new AssetLocation(bakingInitialIngredient));
				if (cobj == null)
				{
					cobj = capi.World.GetBlock(new AssetLocation(bakingInitialIngredient));
				}
				ItemstackTextComponent comp6 = new ItemstackTextComponent(capi, new ItemStack(cobj), 40.0, 10.0, EnumFloat.Inline, delegate(ItemStack cs)
				{
					openDetailPageFor(getPageCodeForStack(capi, cs));
				});
				comp6.PaddingLeft = 2.0;
				components.Add(comp6);
			}
			if (cookrecipes.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "handbook-createdby-potcooking", null);
				foreach (CookingRecipe rec in cookrecipes)
				{
					int firstIndent = 2;
					for (int i = 0; i < rec.Ingredients.Length; i++)
					{
						CookingRecipeIngredient ingred2 = rec.Ingredients[i];
						if (i > 0)
						{
							RichTextComponent cmp6 = new RichTextComponent(capi, " + ", CairoFont.WhiteMediumText());
							cmp6.PaddingLeft = 10.0;
							cmp6.VerticalAlign = EnumVerticalAlign.Middle;
							components.Add(cmp6);
						}
						ItemStack[] stacks = ingred2.ValidStacks.SelectMany(delegate(CookingRecipeStack vs)
						{
							ItemStack itemStack2 = vs.ResolvedItemStack?.Clone();
							object obj;
							if (itemStack2 == null)
							{
								obj = null;
							}
							else
							{
								IEnumerable<ItemStack> enumerable = new global::_003C_003Ez__ReadOnlySingleElementList<ItemStack>(itemStack2);
								obj = enumerable;
							}
							IEnumerable<ItemStack> enumerable2 = (IEnumerable<ItemStack>)obj;
							if (vs.Type == EnumItemClass.Block)
							{
								if (enumerable2 == null)
								{
									enumerable2 = from block in capi.World.SearchBlocks(vs.Code)
										select new ItemStack(block);
								}
							}
							else if (enumerable2 == null)
							{
								enumerable2 = from item2 in capi.World.SearchItems(vs.Code)
									select new ItemStack(item2);
							}
							foreach (ItemStack item2 in enumerable2.Where((ItemStack s) => s.Collectible.Attributes?["waterTightContainerProps"].Exists ?? false))
							{
								WaterTightContainableProps containableProps = BlockLiquidContainerBase.GetContainableProps(item2);
								itemStack2.StackSize *= (int)((containableProps?.ItemsPerLitre ?? 1f) * ingred2.PortionSizeLitres);
							}
							return enumerable2;
						}).ToArray();
						for (int j = 0; j < ingred2.MinQuantity; j++)
						{
							if (j > 0)
							{
								RichTextComponent cmp7 = new RichTextComponent(capi, " + ", CairoFont.WhiteMediumText());
								cmp7.PaddingLeft = 10.0;
								cmp7.VerticalAlign = EnumVerticalAlign.Middle;
								components.Add(cmp7);
							}
							SlideshowItemstackTextComponent comp7 = new SlideshowItemstackTextComponent(capi, stacks, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
							{
								openDetailPageFor(getPageCodeForStack(capi, cs));
							});
							comp7.ShowStackSize = true;
							comp7.PaddingLeft = firstIndent;
							components.Add(comp7);
							firstIndent = 0;
						}
					}
					RichTextComponent eqcomp = new RichTextComponent(capi, " = ", CairoFont.WhiteMediumText());
					eqcomp.PaddingLeft = 10.0;
					eqcomp.VerticalAlign = EnumVerticalAlign.Middle;
					components.Add(eqcomp);
					ItemstackTextComponent ocmp = new ItemstackTextComponent(capi, rec.CooksInto.ResolvedItemStack, 40.0, 0.0, EnumFloat.Inline);
					ocmp.ShowStacksize = true;
					components.Add(ocmp);
					components.Add(new ClearFloatTextComponent(capi, -6f));
				}
			}
			if (grecipes.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "Crafting", null);
				Vintagestory.API.Datastructures.OrderedDictionary<int, List<GridRecipe>> grouped = new Vintagestory.API.Datastructures.OrderedDictionary<int, List<GridRecipe>>();
				ItemStack[] outputStacks = new ItemStack[grecipes.Count];
				int i2 = 0;
				foreach (GridRecipe recipe4 in grecipes)
				{
					if (!grouped.TryGetValue(recipe4.RecipeGroup, out var list5))
					{
						list5 = (grouped[recipe4.RecipeGroup] = new List<GridRecipe>());
					}
					ItemStack[] returnedStacks = (from craftingRecipeIngredient in recipe4.ResolvedIngredients
						select craftingRecipeIngredient?.ReturnedStack?.ResolvedItemStack into itemStack2
						where itemStack2 != null
						select itemStack2).ToArray();
					if (returnedStacks != null && returnedStacks.Length != 0 && returnedStacks.Any((ItemStack itemStack2) => itemStack2.Satisfies(stack)))
					{
						list5.Add(recipe4);
						outputStacks[i2++] = returnedStacks.FirstOrDefault((ItemStack itemStack2) => itemStack2.Satisfies(stack));
						continue;
					}
					if (recipe4.CopyAttributesFrom != null && recipe4.MergeAttributesFrom.Length != 0)
					{
						GridRecipe rec2 = recipe4.Clone();
						ItemStack outputStack = rec2.Output.ResolvedItemStack;
						ITreeAttribute cattr = stack.Attributes.Clone();
						if (rec2.GetIngredientById(recipe4.CopyAttributesFrom) is CraftingRecipeIngredient ingred3)
						{
							outputStack.Attributes = cattr.Clone();
							string[] mergeAttributesFrom = recipe4.MergeAttributesFrom;
							foreach (string ingredientName in mergeAttributesFrom)
							{
								if (rec2.GetIngredientById(ingredientName) is CraftingRecipeIngredient ingred4)
								{
									if (ingred4.ResolvedAttributes != null)
									{
										cattr.MergeTree(ingred4.ResolvedAttributes);
									}
									ingred4.ResolvedItemStack.Attributes = cattr;
								}
							}
							if (ingred3.ResolvedAttributes != null)
							{
								cattr.MergeTree(ingred3.ResolvedAttributes);
							}
							ingred3.ResolvedItemStack.Attributes = cattr;
							outputStacks[i2++] = outputStack;
							list5.Add(rec2);
							continue;
						}
					}
					if (recipe4.CopyAttributesFrom != null)
					{
						GridRecipe rec3 = recipe4.Clone();
						ItemStack outputStack2 = rec3.Output.ResolvedItemStack;
						ITreeAttribute cattr2 = stack.Attributes.Clone();
						if (rec3.GetIngredientById(recipe4.CopyAttributesFrom) is CraftingRecipeIngredient ingred5)
						{
							outputStack2.Attributes = cattr2.Clone();
							_ = ingred5.ResolvedItemStack;
							if (ingred5.ResolvedAttributes != null)
							{
								cattr2.MergeTree(ingred5.ResolvedAttributes);
							}
							ingred5.ResolvedItemStack.Attributes = cattr2;
							outputStacks[i2++] = outputStack2;
							list5.Add(rec3);
							continue;
						}
					}
					if (recipe4.MergeAttributesFrom.Length != 0)
					{
						GridRecipe rec4 = recipe4.Clone();
						ItemStack outputStack3 = rec4.Output.ResolvedItemStack;
						ITreeAttribute cattr3 = stack.Attributes.Clone();
						outputStack3.Attributes = cattr3.Clone();
						string[] mergeAttributesFrom = recipe4.MergeAttributesFrom;
						foreach (string ingredientName2 in mergeAttributesFrom)
						{
							if (rec4.GetIngredientById(ingredientName2) is CraftingRecipeIngredient ingred6)
							{
								if (ingred6.ResolvedAttributes != null)
								{
									cattr3.MergeTree(ingred6.ResolvedAttributes);
								}
								ingred6.ResolvedItemStack.Attributes = cattr3;
							}
						}
						outputStacks[i2++] = outputStack3;
						list5.Add(rec4);
					}
					else
					{
						list5.Add(recipe4);
						outputStacks[i2++] = recipe4.RecipeOutput.ResolvedItemStack;
					}
				}
				int j2 = 0;
				foreach (KeyValuePair<int, List<GridRecipe>> val6 in grouped)
				{
					if (j2++ % 2 == 0)
					{
						components.Add(verticalSpaceSmall);
					}
					SlideshowGridRecipeTextComponent comp8 = new SlideshowGridRecipeTextComponent(capi, val6.Value.ToArray(), 40.0, EnumFloat.Inline, delegate(ItemStack cs)
					{
						openDetailPageFor(getPageCodeForStack(capi, cs));
					}, allStacks);
					comp8.VerticalAlign = EnumVerticalAlign.Top;
					comp8.PaddingRight = 8.0;
					comp8.PaddingLeft = 4 + (1 - j2 % 2) * 20;
					components.Add(comp8);
					RichTextComponent ecomp = new RichTextComponent(capi, "=", CairoFont.WhiteMediumText());
					ecomp.VerticalAlign = EnumVerticalAlign.Middle;
					ecomp.PaddingRight = 5.0;
					SlideshowItemstackTextComponent ocomp7 = new SlideshowItemstackTextComponent(capi, outputStacks, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
					{
						openDetailPageFor(getPageCodeForStack(capi, cs));
					});
					ocomp7.overrideCurrentItemStack = comp8.GenerateCurrentVisibleOutputStack;
					ocomp7.VerticalAlign = EnumVerticalAlign.Middle;
					ocomp7.ShowStackSize = true;
					SlideshowItemstackTextComponent rcomp = new SlideshowItemstackTextComponent(capi, new ItemStack[1], 40.0, EnumFloat.Inline, delegate(ItemStack cs)
					{
						openDetailPageFor(getPageCodeForStack(capi, cs));
					});
					rcomp.overrideCurrentItemStack = () => (from craftingRecipeIngredient in comp8.CurrentVisibleRecipe.Recipe.ResolvedIngredients
						select craftingRecipeIngredient?.ReturnedStack?.ResolvedItemStack into itemStack2
						where itemStack2 != null
						select itemStack2).FirstOrDefault();
					rcomp.PaddingLeft = -40.0;
					rcomp.ShowStackSize = true;
					components.Add(ecomp);
					components.Add(ocomp7);
					components.Add(rcomp);
				}
				components.Add(new ClearFloatTextComponent(capi, 3f));
			}
			if (barrelRecipestext.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "Mixing (in Barrel)", null);
				components.AddRange(barrelRecipestext);
			}
		}
		return haveText;
	}

	public virtual void AddSlideShowComponent(List<RichTextComponentBase> components, ICoreClientAPI capi, List<ItemStack> stacks, ActionConsumable<string> openDetailPageFor, bool showStackSize)
	{
		int firstPadding = 2;
		while (stacks.Count > 0)
		{
			ItemStack dstack = stacks[0];
			stacks.RemoveAt(0);
			if (dstack != null)
			{
				SlideshowItemstackTextComponent comp = new SlideshowItemstackTextComponent(capi, dstack, stacks, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
				{
					openDetailPageFor(getPageCodeForStack(capi, cs));
				});
				comp.ShowStackSize = showStackSize;
				comp.PaddingLeft = firstPadding;
				firstPadding = 0;
				components.Add(comp);
			}
		}
	}

	public static void AddHeading(List<RichTextComponentBase> components, ICoreClientAPI capi, string heading, ref bool haveText)
	{
		if (haveText)
		{
			components.Add(new ClearFloatTextComponent(capi, 14f));
		}
		haveText = true;
		RichTextComponent headc = new RichTextComponent(capi, Lang.Get(heading) + "\n", CairoFont.WhiteSmallText().WithWeight(FontWeight.Bold));
		components.Add(headc);
	}

	public static void AddSubHeading(List<RichTextComponentBase> components, ICoreClientAPI capi, ActionConsumable<string> openDetailPageFor, string subheading, string detailpage)
	{
		if (detailpage == null)
		{
			RichTextComponent bullet = new RichTextComponent(capi, "• " + Lang.Get(subheading) + "\n", CairoFont.WhiteSmallText());
			bullet.PaddingLeft = 2.0;
			components.Add(bullet);
			return;
		}
		RichTextComponent bullet2 = new RichTextComponent(capi, "• ", CairoFont.WhiteSmallText());
		bullet2.PaddingLeft = 2.0;
		components.Add(bullet2);
		components.Add(new LinkTextComponent(capi, Lang.Get(subheading) + "\n", CairoFont.WhiteSmallText(), delegate
		{
			openDetailPageFor(detailpage);
		}));
	}

	protected void addExtraSections(ICoreClientAPI capi, ItemStack stack, List<RichTextComponentBase> components, float marginTop)
	{
		if (ExtraHandBookSections != null)
		{
			bool haveText = true;
			for (int i = 0; i < ExtraHandBookSections.Length; i++)
			{
				ExtraHandbookSection extraSection = ExtraHandBookSections[i];
				AddHeading(components, capi, extraSection.Title, ref haveText);
				components.Add(new ClearFloatTextComponent(capi, 2f));
				RichTextComponent spacer = new RichTextComponent(capi, "", CairoFont.WhiteSmallText());
				spacer.PaddingLeft = 2.0;
				components.Add(spacer);
				if (extraSection.TextParts != null)
				{
					components.AddRange(VtmlUtil.Richtextify(capi, string.Join(", ", extraSection.TextParts) + "\n", CairoFont.WhiteSmallText()));
				}
				else
				{
					components.AddRange(VtmlUtil.Richtextify(capi, Lang.Get(extraSection.Text) + "\n", CairoFont.WhiteSmallText()));
				}
			}
		}
		string text = collObj.Code.Domain + ":" + stack.Class.Name();
		string code = collObj.Code.ToShortString();
		string langExtraSectionTitle = Lang.GetMatchingIfExists(text + "-handbooktitle-" + code);
		string langExtraSectionText = Lang.GetMatchingIfExists(text + "-handbooktext-" + code);
		if (langExtraSectionTitle != null || langExtraSectionText != null)
		{
			components.Add(new ClearFloatTextComponent(capi, 14f));
			if (langExtraSectionTitle != null)
			{
				components.Add(new RichTextComponent(capi, langExtraSectionTitle + "\n", CairoFont.WhiteSmallText().WithWeight(FontWeight.Bold)));
				components.Add(new ClearFloatTextComponent(capi, 2f));
			}
			if (langExtraSectionText != null)
			{
				RichTextComponent spacer2 = new RichTextComponent(capi, "", CairoFont.WhiteSmallText());
				spacer2.PaddingLeft = 2.0;
				components.Add(spacer2);
				components.AddRange(VtmlUtil.Richtextify(capi, langExtraSectionText + "\n", CairoFont.WhiteSmallText()));
			}
		}
	}

	protected List<RichTextComponentBase> BuildBarrelRecipesText(ICoreClientAPI capi, ItemStack stack, ActionConsumable<string> openDetailPageFor)
	{
		List<RichTextComponentBase> barrelRecipesTexts = new List<RichTextComponentBase>();
		List<BarrelRecipe> barrelRecipes = capi.GetBarrelRecipes();
		if (barrelRecipes.Count == 0)
		{
			return barrelRecipesTexts;
		}
		Dictionary<string, List<BarrelRecipe>> brecipesbyCode = new Dictionary<string, List<BarrelRecipe>>();
		foreach (BarrelRecipe recipe in barrelRecipes)
		{
			ItemStack mixdStack = recipe.RecipeOutput.ResolvedItemStack;
			if (mixdStack != null && mixdStack.Equals(capi.World, stack, GlobalConstants.IgnoredStackAttributes))
			{
				string key = recipe.Code + "-ing" + recipe.Ingredients.Length + "-quant" + mixdStack.StackSize + "-hash" + mixdStack.GetHashCode(GlobalConstants.IgnoredStackAttributes);
				if (!brecipesbyCode.TryGetValue(key, out var tmp))
				{
					tmp = (brecipesbyCode[key] = new List<BarrelRecipe>());
				}
				tmp.Add(recipe);
			}
		}
		foreach (List<BarrelRecipe> recipes in brecipesbyCode.Values)
		{
			int ingredientsLen = recipes[0].Ingredients.Length;
			ItemStack[][] ingstacks = new ItemStack[ingredientsLen][];
			ItemStack[] outstacks = new ItemStack[recipes.Count];
			double sealHours = 0.0;
			for (int i = 0; i < recipes.Count; i++)
			{
				for (int j = 0; j < ingredientsLen; j++)
				{
					if (i == 0)
					{
						ingstacks[j] = new ItemStack[recipes.Count];
					}
					ingstacks[j][i] = recipes[i].Ingredients[j].ResolvedItemStack;
				}
				sealHours = recipes[i].SealHours;
				outstacks[i] = recipes[i].RecipeOutput.ResolvedItemStack;
			}
			int firstIndent = 2;
			for (int k = 0; k < ingredientsLen; k++)
			{
				if (k > 0)
				{
					RichTextComponent cmp = new RichTextComponent(capi, " + ", CairoFont.WhiteMediumText());
					cmp.VerticalAlign = EnumVerticalAlign.Middle;
					barrelRecipesTexts.Add(cmp);
				}
				SlideshowItemstackTextComponent scmp = new SlideshowItemstackTextComponent(capi, ingstacks[k], 40.0, EnumFloat.Inline, delegate(ItemStack cs)
				{
					openDetailPageFor(getPageCodeForStack(capi, cs));
				});
				scmp.ShowStackSize = true;
				scmp.PaddingLeft = firstIndent;
				firstIndent = 0;
				barrelRecipesTexts.Add(scmp);
			}
			RichTextComponent eqcomp = new RichTextComponent(capi, " = ", CairoFont.WhiteMediumText());
			eqcomp.VerticalAlign = EnumVerticalAlign.Middle;
			barrelRecipesTexts.Add(eqcomp);
			SlideshowItemstackTextComponent ocmp = new SlideshowItemstackTextComponent(capi, outstacks, 40.0, EnumFloat.Inline);
			ocmp.ShowStackSize = true;
			barrelRecipesTexts.Add(ocmp);
			string sealHoursText = ((sealHours > (double)capi.World.Calendar.HoursPerDay) ? (" " + Lang.Get("{0} days", Math.Round(sealHours / (double)capi.World.Calendar.HoursPerDay, 1))) : Lang.Get("{0} hours", Math.Round(sealHours)));
			RichTextComponent hoursCmp = new RichTextComponent(capi, sealHoursText, CairoFont.WhiteSmallText());
			hoursCmp.VerticalAlign = EnumVerticalAlign.Middle;
			barrelRecipesTexts.Add(hoursCmp);
			barrelRecipesTexts.Add(new ClearFloatTextComponent(capi, 10f));
		}
		return barrelRecipesTexts;
	}

	protected void addStorableInfo(ICoreClientAPI capi, ItemStack[] allStacks, ActionConsumable<string> openDetailPageFor, ItemStack stack, List<RichTextComponentBase> components, float marginTop)
	{
		List<ItemStack> foodStorables = new List<ItemStack>();
		List<ItemStack> liquidStorables = new List<ItemStack>();
		List<ItemStack> displayStorables = new List<ItemStack>();
		List<ItemStack> animalHusbandryStorables = new List<ItemStack>();
		bool groundStorable = stack.Collectible.HasBehavior<CollectibleBehaviorGroundStorable>();
		DummySlot dummySlot = new DummySlot(stack);
		string cat = stack.Collectible.GetCollectibleInterface<IWearableStatsSupplier>()?.GetDressType(dummySlot).ToString().ToLowerInvariant();
		int num = 0;
		while (num < allStacks.Length)
		{
			ItemStack val = allStacks[num];
			string[] placementSurfaces = Array.Empty<string>();
			PlacementSurface[] blockPlacementSurfaces = val.Block?.GetBehavior<BlockBehaviorDisplay>()?.PlacementSurfaces;
			if (blockPlacementSurfaces != null && blockPlacementSurfaces.Length != 0)
			{
				placementSurfaces = blockPlacementSurfaces.Select((PlacementSurface ps) => ps.DisplayCategory).ToArray();
			}
			if ((placementSurfaces.Length == 0 || !placementSurfaces.Any((string key) => BlockBehaviorDisplay.GetDisplayableAttributes(new DummySlot(stack), key) != null)) && (!(val.Collectible is BlockShelf) || !BlockEntityShelf.GetShelvableLayout(stack).HasValue))
			{
				if (!(val.Collectible is BlockToolRack))
				{
					goto IL_019f;
				}
				if (!stack.Collectible.GetTool(new DummySlot(stack)).HasValue)
				{
					JsonObject itemAttributes = stack.ItemAttributes;
					if (itemAttributes == null || !itemAttributes["rackable"].AsBool())
					{
						goto IL_019f;
					}
				}
			}
			goto IL_0207;
			IL_0211:
			if ((val.Block is BlockTroughBase blockTrough && ItemSlotTrough.getContentConfig(capi.World, blockTrough.contentConfigs, dummySlot) != null) || (val.Block is BlockAnimalTrap blockTrap && !val.Attributes.GetAsBool("destroyed") && blockTrap.IsAppetizingBait(capi, stack) && blockTrap.CanFitBait(capi, stack)))
			{
				addToListUniquely(capi, animalHusbandryStorables, val);
			}
			JsonObject attr = stack.ItemAttributes;
			if (attr != null)
			{
				if ((val.Collectible is BlockMoldRack && attr["moldrackable"].AsBool()) || (val.Collectible is BlockBookshelf && attr["bookshelveable"].AsBool()) || (val.Collectible is BlockScrollRack && attr["scrollrackable"].AsBool()) || (val.Collectible is BlockDisplayCase blockCase && attr["displaycaseable"].AsBool() && blockCase.height >= attr["displaycase"]["minHeight"].AsFloat(0.25f)) || (val.Collectible is BlockAntlerMount && attr["antlerMountable"].AsBool()) || (val.Collectible is BlockOmokTable && attr["omokpiece"].AsBool()))
				{
					addToListUniquely(capi, displayStorables, val);
				}
				ILiquidInterface contBlock = val.Collectible.GetCollectibleInterface<ILiquidInterface>();
				if (contBlock != null && contBlock.GetCurrentLitres(val) <= 0f && attr["waterTightContainerProps"].Exists)
				{
					addToListUniquely(capi, liquidStorables, val);
				}
				if (val.Collectible is BlockCrock && attr["crockable"].AsBool())
				{
					addToListUniquely(capi, foodStorables, val);
				}
			}
			num++;
			continue;
			IL_0207:
			addToListUniquely(capi, displayStorables, val);
			goto IL_0211;
			IL_019f:
			if (!(val.Collectible is BlockPlantContainer) || getPlantContainerProps(val, stack) == null)
			{
				JsonObject itemAttributes2 = val.ItemAttributes;
				if (itemAttributes2 == null || itemAttributes2["allowedDresstypes"].AsArray<string>()?.Contains<string>(cat) != true)
				{
					goto IL_0211;
				}
			}
			goto IL_0207;
		}
		if (foodStorables.Count > 0 || displayStorables.Count > 0 || liquidStorables.Count > 0 || animalHusbandryStorables.Count > 0 || groundStorable)
		{
			ClearFloatTextComponent verticalSpaceSmall = new ClearFloatTextComponent(capi, 7f);
			ClearFloatTextComponent verticalSpace = new ClearFloatTextComponent(capi, 3f);
			bool haveText = components.Count > 0;
			components.Add(verticalSpace);
			verticalSpace = verticalSpaceSmall;
			AddHeading(components, capi, "Storable in/on", ref haveText);
			if (groundStorable)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				RichTextComponent comp = new RichTextComponent(capi, "", CairoFont.WhiteSmallText());
				comp.PaddingLeft = 2.0;
				components.Add(comp);
				components.AddRange(VtmlUtil.Richtextify(capi, Lang.Get("handbook-storable-ground") + "\n", CairoFont.WhiteSmallText()));
			}
			if (displayStorables.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "handbook-storable-displaycontainers", null);
				AddSlideShowComponent(components, capi, displayStorables, openDetailPageFor, showStackSize: false);
				components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
			}
			if (liquidStorables.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "handbook-storable-liquidcontainers", null);
				AddSlideShowComponent(components, capi, liquidStorables, openDetailPageFor, showStackSize: false);
				components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
			}
			if (foodStorables.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "handbook-storable-foodcontainers", null);
				AddSlideShowComponent(components, capi, foodStorables, openDetailPageFor, showStackSize: false);
				components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
			}
			if (animalHusbandryStorables.Count > 0)
			{
				components.Add(verticalSpace);
				verticalSpace = verticalSpaceSmall;
				AddSubHeading(components, capi, openDetailPageFor, "handbook-storable-animalhusbandry", null);
				AddSlideShowComponent(components, capi, animalHusbandryStorables, openDetailPageFor, showStackSize: false);
				components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
			}
		}
	}

	protected void addStoredInInfo(ICoreClientAPI capi, ItemStack[] allStacks, ActionConsumable<string> openDetailPageFor, ItemStack stack, List<RichTextComponentBase> components, float marginTop)
	{
		List<ItemStack> storables = new List<ItemStack>();
		BlockAnimalTrap blockTrap = stack.Block as BlockAnimalTrap;
		bool stackDestroyedInTrap = stack.Attributes.GetAsBool("destroyed");
		ILiquidInterface stackLiquidInterface = stack.Collectible.GetCollectibleInterface<ILiquidInterface>();
		DummySlot dummySlot = new DummySlot();
		string[] allowedDresstypes = stack.ItemAttributes?["allowedDresstypes"].AsArray<string>();
		string[] placementSurfaces = (stack.Block?.GetBehavior<BlockBehaviorDisplay>()?.PlacementSurfaces.Select((PlacementSurface ps) => ps.DisplayCategory) ?? Array.Empty<string>()).ToArray();
		foreach (ItemStack val in allStacks)
		{
			if ((placementSurfaces.Length == 0 || !placementSurfaces.Any((string key) => BlockBehaviorDisplay.GetDisplayableAttributes(new DummySlot(val), key) != null)) && (!(stack.Collectible is BlockShelf) || !BlockEntityShelf.GetShelvableLayout(val).HasValue) && (!(stack.Collectible is BlockPlantContainer) || getPlantContainerProps(stack, val) == null))
			{
				if (!(stack.Collectible is BlockToolRack))
				{
					goto IL_01af;
				}
				if (!val.Collectible.GetTool(new DummySlot(val)).HasValue)
				{
					JsonObject itemAttributes = val.ItemAttributes;
					if (itemAttributes == null || !itemAttributes["rackable"].AsBool())
					{
						goto IL_01af;
					}
				}
			}
			goto IL_01df;
			IL_01df:
			addToListUniquely(capi, storables, val);
			continue;
			IL_01af:
			if (blockTrap != null && !stackDestroyedInTrap && blockTrap.IsAppetizingBait(capi, val) && blockTrap.CanFitBait(capi, val))
			{
				goto IL_01df;
			}
			dummySlot.Itemstack = val;
			ContentConfig contentConfig = ItemSlotTrough.getContentConfig(capi.World, (stack.Block as BlockTroughBase)?.contentConfigs, dummySlot);
			if (contentConfig != null)
			{
				ItemStack troughStack = val.GetEmptyClone();
				troughStack.StackSize = contentConfig.QuantityPerFillLevel;
				addToListUniquely(capi, storables, troughStack);
				continue;
			}
			if (allowedDresstypes != null)
			{
				string cat = val.Collectible.GetCollectibleInterface<IWearableStatsSupplier>()?.GetDressType(dummySlot).ToString().ToLowerInvariant();
				if (allowedDresstypes.Contains(cat))
				{
					addToListUniquely(capi, storables, val);
					continue;
				}
			}
			JsonObject attr = val.ItemAttributes;
			if (attr == null)
			{
				continue;
			}
			if ((!(stack.Collectible is BlockMoldRack) || !attr["moldrackable"].AsBool()) && (!(stack.Collectible is BlockBookshelf) || !attr["bookshelveable"].AsBool()) && (!(stack.Collectible is BlockScrollRack) || !attr["scrollrackable"].AsBool()) && (!(stack.Collectible is BlockDisplayCase blockCase) || !attr["displaycaseable"].AsBool() || !(blockCase.height >= attr["displaycase"]["minHeight"].AsFloat(0.25f))) && (!(stack.Collectible is BlockAntlerMount) || !attr["antlerMountable"].AsBool()))
			{
				if (stackLiquidInterface != null)
				{
					WaterTightContainableProps liquidProps = BlockLiquidContainerBase.GetContainableProps(val);
					if (liquidProps != null && liquidProps.WhenFilled == null)
					{
						goto IL_041e;
					}
				}
				if ((!(stack.Collectible is BlockCrock) || !attr["crockable"].AsBool()) && (!(stack.Collectible is BlockOmokTable) || !attr["omokpiece"].AsBool()))
				{
					continue;
				}
			}
			goto IL_041e;
			IL_041e:
			addToListUniquely(capi, storables, val);
		}
		if (storables.Count <= 0)
		{
			return;
		}
		bool haveText = components.Count > 0;
		components.Add(new ClearFloatTextComponent(capi, 7f));
		AddHeading(components, capi, "handbook-storedin", ref haveText);
		int firstPadding = 2;
		while (storables.Count > 0)
		{
			ItemStack dstack = storables[0];
			storables.RemoveAt(0);
			if (dstack == null)
			{
				continue;
			}
			if (dstack.Collectible is BlockPie)
			{
				List<CookingRecipe> pierecipes = ObjectCacheUtil.TryGet<CookingRecipe[]>(capi, "handbookpierecipes").ToList();
				while (pierecipes.Count > 0)
				{
					CookingRecipe recipe = pierecipes[0];
					pierecipes.RemoveAt(0);
					if (recipe != null)
					{
						ItemStack mealBlock = dstack.Clone();
						mealBlock.Attributes.SetInt("pieSize", 4);
						mealBlock.Attributes.SetString("topCrustType", BlockPie.TopCrustTypes[capi.World.Rand.Next(BlockPie.TopCrustTypes.Length)].Code);
						mealBlock.Attributes.SetInt("bakeLevel", 2);
						MealstackTextComponent mealComp = new MealstackTextComponent(capi, mealBlock, recipe, 40.0, EnumFloat.Inline, delegate
						{
							openDetailPageFor("handbook-mealrecipe-" + recipe.Code + "-pie");
						}, 6, isPie: true);
						components.Add(mealComp);
					}
				}
			}
			else
			{
				SlideshowItemstackTextComponent comp = new SlideshowItemstackTextComponent(capi, dstack, storables, 40.0, EnumFloat.Inline, delegate(ItemStack cs)
				{
					openDetailPageFor(getPageCodeForStack(capi, cs));
				});
				if (BlockLiquidContainerBase.GetContainableProps(dstack) == null)
				{
					comp.ShowStackSize = true;
				}
				comp.PaddingLeft = firstPadding;
				firstPadding = 0;
				components.Add(comp);
			}
		}
		components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
	}

	protected void addEatenByInfo(ICoreClientAPI capi, ItemStack stack, List<RichTextComponentBase> components, float marginTop)
	{
		HashSet<string> creatureNames = new HashSet<string>();
		foreach (EntityProperties entityType in Api.World.EntityTypes)
		{
			JsonObject attr = entityType.Attributes;
			if (attr != null && attr["creatureDiet"].AsObject<CreatureDiet>()?.Matches(stack) == true)
			{
				string code = attr?["creatureDietGroup"].AsString() ?? attr?["handbook"]["groupcode"].AsString() ?? (entityType.Code.Domain + ":item-creature-" + entityType.Code.Path);
				creatureNames.Add(Lang.Get(code));
			}
		}
		bool haveText = components.Count > 0;
		if (creatureNames.Count > 0)
		{
			AddHeading(components, capi, "handbook-eatenby", ref haveText);
			components.Add(new ClearFloatTextComponent(capi, 2f));
			RichTextComponent comp = new RichTextComponent(capi, string.Join(", ", creatureNames) + "\n", CairoFont.WhiteSmallText());
			comp.PaddingLeft = 2.0;
			components.Add(comp);
		}
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		JsonObject attributes = collObj.Attributes;
		if (attributes != null && attributes["pigment"]?["color"].Exists == true)
		{
			dsc.AppendLine(Lang.Get("Pigment: {0}", Lang.Get(collObj.Attributes["pigment"]["name"].AsString())));
		}
		JsonObject obj = collObj.Attributes?["fertilizerProps"];
		if (obj != null && obj.Exists)
		{
			FertilizerProps fprops = obj.AsObject<FertilizerProps>();
			if (fprops != null)
			{
				dsc.AppendLine(Lang.Get("Fertilizer: {0}% N, {1}% P, {2}% K", fprops.N, fprops.P, fprops.K));
			}
		}
		JuiceableProperties jprops = getjuiceableProps(inSlot.Itemstack);
		if (jprops != null)
		{
			float litres = (jprops.LitresPerItem.HasValue ? (jprops.LitresPerItem.Value * (float)inSlot.Itemstack.StackSize) : ((float)inSlot.Itemstack.Attributes.GetDecimal("juiceableLitresLeft")));
			if ((double)litres > 0.01)
			{
				dsc.AppendLine(Lang.Get("collectibleinfo-juicingproperties", litres, jprops.LiquidStack.ResolvedItemStack.GetName()));
			}
		}
	}

	public JuiceableProperties getjuiceableProps(ItemStack stack)
	{
		JuiceableProperties? obj = stack?.ItemAttributes?["juiceableProperties"]?.AsObject<JuiceableProperties>(null, stack.Collectible.Code.Domain);
		obj?.LiquidStack?.Resolve(Api.World, "juiceable properties liquidstack");
		obj?.PressedStack?.Resolve(Api.World, "juiceable properties pressedstack");
		if (obj != null)
		{
			JsonItemStack returnStack = obj.ReturnStack;
			if (returnStack != null)
			{
				returnStack.Resolve(Api.World, "juiceable properties returnstack");
				return obj;
			}
			return obj;
		}
		return obj;
	}

	public DistillationProps getDistillationProps(ItemStack stack)
	{
		DistillationProps? obj = stack?.ItemAttributes?["distillationProps"]?.AsObject<DistillationProps>(null, stack.Collectible.Code.Domain);
		if (obj != null)
		{
			JsonItemStack distilledStack = obj.DistilledStack;
			if (distilledStack != null)
			{
				distilledStack.Resolve(Api.World, "distillation props distilled stack");
				return obj;
			}
			return obj;
		}
		return obj;
	}

	private ItemStack[] getMoldedOutput(ItemStack mold, ItemStack[] metalmoldables)
	{
		ItemStack ingotStack = metalmoldables[0].Collectible.GetCombustibleProperties(Api.World, metalmoldables[0], null)?.SmeltedStack?.ResolvedItemStack;
		if (mold.Block is BlockIngotMold)
		{
			return new ItemStack[1] { ingotStack };
		}
		if (!(mold.Block is BlockToolMold))
		{
			return Array.Empty<ItemStack>();
		}
		try
		{
			if (mold.Block.Attributes["drop"].Exists)
			{
				JsonItemStack jstack = mold.Block.Attributes["drop"].AsObject<JsonItemStack>(null, mold.Block.Code.Domain);
				ItemStack stack = moldedStackFromCode(mold.Block, jstack, ingotStack);
				if (stack == null)
				{
					return Array.Empty<ItemStack>();
				}
				return new ItemStack[1] { stack };
			}
			return (from jstack2 in mold.Block.Attributes["drops"].AsObject<JsonItemStack[]>(null, mold.Block.Code.Domain)
				select moldedStackFromCode(mold.Block, jstack2, ingotStack) into itemStack
				where itemStack != null
				select itemStack).ToArray();
		}
		catch (JsonReaderException)
		{
			Api.World.Logger.Error("Failed getting molded stacks from tool mold of block {0}, probably unable to parse drop or drops attribute", mold.Block.Code);
			throw;
		}
	}

	private ItemStack moldedStackFromCode(Block mold, JsonItemStack jstack, ItemStack fromMetal)
	{
		if (mold == null || jstack == null || fromMetal == null)
		{
			return null;
		}
		string metaltype = fromMetal.Collectible.LastCodePart();
		jstack.Code.Path = jstack.Code.Path.Replace("{metal}", metaltype);
		jstack.Resolve(Api.World, "tool mold drop for " + mold.Code, printWarningOnError: false);
		return jstack.ResolvedItemStack;
	}

	private bool getIsAnvilWorkable(ItemStack stack, List<ItemStack> fuels)
	{
		CombustibleProperties combustibleProps = stack?.Collectible?.GetCombustibleProperties(Api.World, stack, null);
		if (stack?.Collectible?.GetCollectibleInterface<IAnvilWorkable>() != null)
		{
			if (combustibleProps == null)
			{
				JsonObject attributes = stack.Collectible.Attributes;
				if (attributes == null || !attributes["workableTemperature"].Exists)
				{
					goto IL_0069;
				}
			}
			float maxFuelTemp = (from fuel in fuels
				orderby fuel.Collectible.GetCombustibleProperties(Api.World, fuel, null).BurnTemperature
				select fuel.Collectible.GetCombustibleProperties(Api.World, fuel, null).BurnTemperature).LastOrDefault();
			float meltingPoint = ((combustibleProps != null) ? (combustibleProps.MeltingPoint / 2) : 0);
			return (stack.Collectible.Attributes?["workableTemperature"]?.AsFloat(meltingPoint) ?? meltingPoint) <= maxFuelTemp;
		}
		goto IL_0069;
		IL_0069:
		return false;
	}

	private bool getCanBloomerySmelt(ItemStack stack)
	{
		CombustibleProperties combustibleProps = stack.Collectible.GetCombustibleProperties(Api.World, stack, null);
		if (combustibleProps?.SmeltedStack?.ResolvedItemStack == null)
		{
			return false;
		}
		int stackMeltingPoint = combustibleProps?.MeltingPoint ?? 0;
		if (stackMeltingPoint < 1500)
		{
			return stackMeltingPoint >= 1000;
		}
		return false;
	}

	private bool getCanContainerSmelt(ICoreClientAPI capi, List<ItemStack> containers, List<ItemStack> fuels, ItemStack stack)
	{
		int maxFuelTemp = (from fuel in fuels
			orderby fuel.Collectible.GetCombustibleProperties(capi.World, fuel, null).BurnTemperature
			select fuel.Collectible.GetCombustibleProperties(capi.World, fuel, null).BurnTemperature).LastOrDefault();
		CombustibleProperties combustibleProps = stack?.Collectible?.GetCombustibleProperties(Api.World, stack, null);
		if (combustibleProps == null || combustibleProps.MeltingPoint > maxFuelTemp)
		{
			return false;
		}
		ItemStack[] source = containers.Where((ItemStack container) => container.ItemAttributes["maxContentDimensions"].AsObject<Size3f>()?.CanContain(stack.Collectible.Dimensions) ?? true).ToArray();
		ItemStack smeltedStack = stack.Clone();
		smeltedStack.StackSize = combustibleProps.SmeltedRatio;
		dummySmeltingInv.Slots[1].Itemstack = smeltedStack;
		return source.Any((ItemStack container) => container.Collectible.CanSmelt(capi.World, dummySmeltingInv, container, null));
	}

	private bool getCanSmelt(List<ItemStack> fuels, ItemStack stack)
	{
		int maxFuelTemp = (from fuel in fuels
			orderby fuel.Collectible.GetCombustibleProperties(Api.World, fuel, null).BurnTemperature
			select fuel.Collectible.GetCombustibleProperties(Api.World, fuel, null).BurnTemperature).LastOrDefault();
		CombustibleProperties combustibleProps = stack.Collectible.GetCombustibleProperties(Api.World, stack, null);
		if (combustibleProps == null || combustibleProps.MeltingPoint > maxFuelTemp)
		{
			return false;
		}
		if (!combustibleProps.RequiresContainer)
		{
			return true;
		}
		return false;
	}

	private PlantContainerProps getPlantContainerProps(ItemStack containerStack, ItemStack plantStack)
	{
		string containerSize = containerStack?.ItemAttributes?["plantContainerSize"].AsString();
		if (containerSize != null)
		{
			CollectibleObject plant = plantStack?.Collectible;
			if (plant != null)
			{
				return plant.Attributes?["plantContainable"]?[containerSize + "Container"]?.AsObject<PlantContainerProps>(null, plant.Code.Domain);
			}
		}
		return null;
	}

	private string getPageCodeForStack(ICoreClientAPI capi, ItemStack stack)
	{
		return stack.Collectible.GetCollectibleInterface<IHandBookPageCodeProvider>()?.HandbookPageCodeForStack(capi.World, stack) ?? GuiHandbookItemStackPage.PageCodeForStack(stack);
	}

	private void addToListUniquely(ICoreClientAPI capi, List<ItemStack> list, ItemStack entry)
	{
		if (!list.Any((ItemStack s) => s.Equals(capi.World, entry, GlobalConstants.IgnoredStackAttributes)))
		{
			list.Add(entry);
		}
	}

	static CollectibleBehaviorHandbookTextAndExtraInfo()
	{
		string[] ignoredStackAttributes = GlobalConstants.IgnoredStackAttributes;
		int num = 0;
		string[] array = new string[1 + ignoredStackAttributes.Length];
		ReadOnlySpan<string> readOnlySpan = new ReadOnlySpan<string>(ignoredStackAttributes);
		readOnlySpan.CopyTo(new Span<string>(array).Slice(num, readOnlySpan.Length));
		num += readOnlySpan.Length;
		array[num] = "juiceableLitresLeft";
		juiceableIgnoredAttributes = array;
	}
}
