using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockBehaviorCabinetDoors : StrongBlockBehavior
{
	public Cuboidf[]? DoorSelectionBoxOpened;

	public Cuboidf[]? DoorSelectionBoxClosed;

	public string?[]? DoorElements;

	public BlockBehaviorCabinetDoors(Block block)
		: base(block)
	{
	}

	public override void Initialize(JsonObject properties)
	{
		base.Initialize(properties);
		DoorSelectionBoxOpened = properties["selectionBoxesOpened"].AsObject<Cuboidf[]>();
		DoorSelectionBoxClosed = properties["selectionBoxesClosed"].AsObject<Cuboidf[]>();
		if (DoorSelectionBoxClosed == null)
		{
			throw new NullReferenceException("selectionBoxesClosed cannot be null! Block code " + block.Code);
		}
	}

	public override Cuboidf[] GetSelectionBoxes(IBlockAccessor blockAccessor, BlockPos pos, ref EnumHandling handled)
	{
		handled = EnumHandling.Handled;
		BlockEntity be = blockAccessor.GetBlockEntity(pos);
		if (be == null)
		{
			return base.GetSelectionBoxes(blockAccessor, pos, ref handled);
		}
		return be.GetBehavior<BEBehaviorCabinetDoors>().GetSelectionBoxes();
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer, ref EnumHandling handling)
	{
		if (selection.SelectionBoxId == null)
		{
			ItemStack[] doorStacks = ObjectCacheUtil.GetOrCreate(world.Api, "cabinetDoors", delegate
			{
				List<ItemStack> list = new List<ItemStack>();
				foreach (CollectibleObject current in world.Collectibles)
				{
					if (current.Code.PathStartsWith("cabinetdoor"))
					{
						list.Add(new ItemStack(current));
					}
				}
				return list.ToArray();
			});
			if (block.GetBEBehavior<BEBehaviorCabinetDoors>(selection.Position)?.DoorStack == null)
			{
				return base.GetPlacedBlockInteractionHelp(world, selection, forPlayer, ref handling).Append(new WorldInteraction
				{
					ActionLangCode = "addcabinetdoors",
					HotKeyCode = "ctrl",
					MouseButton = EnumMouseButton.Right,
					Itemstacks = doorStacks
				});
			}
			if (BlockBehaviorWrenchOrientable.wrenchItems == null)
			{
				BlockBehaviorWrenchOrientable.loadWrenchItems(world);
			}
			return base.GetPlacedBlockInteractionHelp(world, selection, forPlayer, ref handling).Append(new WorldInteraction
			{
				ActionLangCode = "removecabinetdoors",
				HotKeyCode = "ctrl",
				MouseButton = EnumMouseButton.Right,
				Itemstacks = BlockBehaviorWrenchOrientable.wrenchItems
			});
		}
		return base.GetPlacedBlockInteractionHelp(world, selection, forPlayer, ref handling);
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
	{
		ItemStack doorStack = block.GetBEBehavior<BEBehaviorCabinetDoors>(pos)?.DoorStack;
		if (doorStack != null)
		{
			handling = EnumHandling.PreventDefault;
			return new ItemStack[1] { doorStack };
		}
		return base.GetDrops(world, pos, byPlayer, ref dropChanceMultiplier, ref handling);
	}
}
