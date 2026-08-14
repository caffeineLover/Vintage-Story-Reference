using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent.Mechanics;

public class BlockWindmillRotor : BlockMPBase, IMPPowered
{
	protected BlockFacing powerOutFacing;

	public override void OnLoaded(ICoreAPI api)
	{
		powerOutFacing = BlockFacing.FromCode(Variant["side"]).Opposite;
		base.OnLoaded(api);
	}

	public override void DidConnectAt(IWorldAccessor world, BlockPos pos, BlockFacing face)
	{
	}

	public override bool HasMechPowerConnectorAt(IWorldAccessor world, BlockPos pos, BlockFacing face, BlockMPBase forBlock)
	{
		return face == powerOutFacing;
	}

	public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
	{
		if (!CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
		{
			return false;
		}
		bool invalid;
		BlockFacing face = GetFacingForPlacement(world, blockSel.Position, doPlace: true, out invalid);
		if (invalid)
		{
			return false;
		}
		bool ok = face != null;
		if (!ok)
		{
			ok = base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
		}
		if (ok)
		{
			WasPlaced(world, blockSel.Position, face);
		}
		return ok;
	}

	protected virtual BlockFacing GetFacingForPlacement(IWorldAccessor world, BlockPos position, bool doPlace, out bool invalid)
	{
		invalid = false;
		BlockFacing[] hORIZONTALS = BlockFacing.HORIZONTALS;
		foreach (BlockFacing face in hORIZONTALS)
		{
			BlockPos pos = position.AddCopy(face);
			if (world.BlockAccessor.GetBlock(pos) is IMechanicalPowerBlock block && block.HasMechPowerConnectorAt(world, pos, face.Opposite, this))
			{
				if (block is IMPPowered)
				{
					invalid = true;
					return null;
				}
				if (doPlace)
				{
					Block toPlaceBlock = world.GetBlock(CodeWithVariant("side", face.Opposite.Code));
					world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, position);
					block.DidConnectAt(world, pos, face.Opposite);
				}
				return face;
			}
		}
		return null;
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		return (world.BlockAccessor.GetBlockEntity(blockSel.Position)?.GetBehavior<BEBehaviorWindmillRotor>())?.OnInteract(byPlayer) ?? base.OnBlockInteractStart(world, byPlayer, blockSel);
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		BEBehaviorWindmillRotor be = world.BlockAccessor.GetBlockEntity(selection.Position)?.GetBehavior<BEBehaviorWindmillRotor>();
		if (be != null && be.SailLength >= 3)
		{
			return Array.Empty<WorldInteraction>();
		}
		WorldInteraction[] placedBlockInteractionHelp = base.GetPlacedBlockInteractionHelp(world, selection, forPlayer);
		int num = 0;
		WorldInteraction[] array = new WorldInteraction[1 + placedBlockInteractionHelp.Length];
		ReadOnlySpan<WorldInteraction> readOnlySpan = new ReadOnlySpan<WorldInteraction>(placedBlockInteractionHelp);
		readOnlySpan.CopyTo(new Span<WorldInteraction>(array).Slice(num, readOnlySpan.Length));
		num += readOnlySpan.Length;
		array[num] = new WorldInteraction
		{
			ActionLangCode = "heldhelp-addsails",
			MouseButton = EnumMouseButton.Right,
			Itemstacks = new ItemStack[1]
			{
				new ItemStack(world.GetItem(new AssetLocation("sail")), 4)
			}
		};
		return array;
	}

	public BlockFacing GetFacing()
	{
		return BlockFacing.FromCode(Variant["side"]);
	}
}
