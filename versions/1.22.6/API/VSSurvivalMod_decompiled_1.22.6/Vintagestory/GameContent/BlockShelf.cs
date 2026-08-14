using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockShelf : Block
{
	private WorldInteraction[]? interactions;

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		PlacedPriorityInteract = true;
		interactions = ObjectCacheUtil.GetOrCreate(api, "shelfInteractions", delegate
		{
			List<ItemStack> usableItemStacklist = new List<ItemStack>();
			List<ItemStack> list = new List<ItemStack>();
			foreach (CollectibleObject current in api.World.Collectibles)
			{
				bool flag = current != null && current.Attributes?["mealContainer"]?.AsBool() == true;
				if (!flag)
				{
					bool flag2 = ((current is IContainedInteractable || current is IBlockMealContainer) ? true : false);
					flag = flag2;
				}
				if (flag || (current != null && current.Attributes?["canSealCrock"]?.AsBool() == true))
				{
					usableItemStacklist.Add(new ItemStack(current));
				}
				if (BlockEntityShelf.GetShelvableLayout(new ItemStack(current)).HasValue)
				{
					if (current is BlockPie blockPie)
					{
						ItemStack itemStack = new ItemStack(current);
						itemStack.Attributes.SetInt("pieSize", 4);
						itemStack.Attributes.SetString("topCrustType", "square");
						ITreeAttribute attributes = itemStack.Attributes;
						attributes.SetInt("bakeLevel", blockPie.Variant["state"] switch
						{
							"raw" => 0, 
							"partbaked" => 1, 
							"perfect" => 2, 
							"charred" => 3, 
							_ => 0, 
						});
						ItemStack itemStack2 = new ItemStack(api.World.GetItem("dough-spelt"), 2);
						ItemStack itemStack3 = new ItemStack(api.World.GetItem("fruit-redapple"), 2);
						blockPie.SetContents(itemStack, new ItemStack[6] { itemStack2, itemStack3, itemStack3, itemStack3, itemStack3, itemStack2 });
						itemStack.Attributes.SetFloat("quantityServings", 1f);
						list.Add(itemStack);
					}
					else
					{
						list.Add(new ItemStack(current));
					}
				}
			}
			ItemStack[] itemstacks = list.ToArray();
			return new WorldInteraction[4]
			{
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-shelf-use",
					MouseButton = EnumMouseButton.Right,
					Itemstacks = itemstacks,
					GetMatchingStacks = delegate(WorldInteraction wi, BlockSelection bs, EntitySelection es)
					{
						BlockEntityShelf beshelf = api.World.BlockAccessor.GetBlockEntity(bs.Position) as BlockEntityShelf;
						return usableItemStacklist.Where((ItemStack stack) => beshelf?.CanUse(stack, bs) ?? false)?.ToArray();
					}
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-shelf-place",
					MouseButton = EnumMouseButton.Right,
					Itemstacks = itemstacks,
					GetMatchingStacks = delegate(WorldInteraction wi, BlockSelection bs, EntitySelection es)
					{
						BlockEntityShelf beshelf = api.World.BlockAccessor.GetBlockEntity(bs.Position) as BlockEntityShelf;
						return usableItemStacklist.All(delegate(ItemStack stack)
						{
							BlockEntityShelf blockEntityShelf = beshelf;
							return blockEntityShelf != null && !blockEntityShelf.CanUse(stack, bs);
						}) ? usableItemStacklist.Where((ItemStack stack) => beshelf?.CanPlace(stack, bs, out var _) ?? false).ToArray() : null;
					}
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-shelf-place",
					HotKeyCode = "shift",
					MouseButton = EnumMouseButton.Right,
					Itemstacks = itemstacks,
					GetMatchingStacks = delegate(WorldInteraction wi, BlockSelection bs, EntitySelection es)
					{
						BlockEntityShelf beshelf = api.World.BlockAccessor.GetBlockEntity(bs.Position) as BlockEntityShelf;
						return usableItemStacklist.Any((ItemStack stack) => beshelf?.CanUse(stack, bs) ?? false) ? usableItemStacklist.Where((ItemStack stack) => beshelf?.CanPlace(stack, bs, out var _) ?? false).ToArray() : null;
					}
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-shelf-take",
					MouseButton = EnumMouseButton.Right,
					RequireFreeHand = true,
					ShouldApply = delegate(WorldInteraction wi, BlockSelection bs, EntitySelection es)
					{
						BlockEntityShelf obj = api.World.BlockAccessor.GetBlockEntity(bs.Position) as BlockEntityShelf;
						bool canTake = false;
						obj?.CanPlace(null, bs, out canTake);
						return canTake;
					}
				}
			};
		});
	}

	public override bool DoPartialSelection(IWorldAccessor world, BlockPos pos)
	{
		return true;
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityShelf beshelf)
		{
			return beshelf.OnInteract(byPlayer, blockSel);
		}
		return base.OnBlockInteractStart(world, byPlayer, blockSel);
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		WorldInteraction[] worldInteractions = base.GetPlacedBlockInteractionHelp(world, selection, forPlayer);
		if (world.Claims.TestAccess(forPlayer, selection.Position, EnumBlockAccessFlags.Use) == EnumWorldAccessResponse.Granted)
		{
			WorldInteraction[] array = worldInteractions;
			WorldInteraction[] array2 = interactions;
			int num = 0;
			WorldInteraction[] array3 = new WorldInteraction[array.Length + array2.Length];
			ReadOnlySpan<WorldInteraction> readOnlySpan = new ReadOnlySpan<WorldInteraction>(array);
			readOnlySpan.CopyTo(new Span<WorldInteraction>(array3).Slice(num, readOnlySpan.Length));
			num += readOnlySpan.Length;
			ReadOnlySpan<WorldInteraction> readOnlySpan2 = new ReadOnlySpan<WorldInteraction>(array2);
			readOnlySpan2.CopyTo(new Span<WorldInteraction>(array3).Slice(num, readOnlySpan2.Length));
			num += readOnlySpan2.Length;
			return array3;
		}
		return worldInteractions;
	}
}
