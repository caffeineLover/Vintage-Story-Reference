using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent.Mechanics;

public class BlockSpurGear : BlockMPBase
{
	protected BlockFacing Orientation;

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		Orientation = BlockFacing.FromFirstLetter(Variant["orientation"]);
	}

	public override bool HasMechPowerConnectorAt(IWorldAccessor world, BlockPos pos, BlockFacing face, BlockMPBase forBlock)
	{
		if (face != Orientation)
		{
			if (forBlock == this)
			{
				return face != Orientation.Opposite;
			}
			return false;
		}
		return true;
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
	{
		return new ItemStack(world.GetBlock(CodeWithVariant("orientation", "s")));
	}

	public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
	{
		if (!CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
		{
			return false;
		}
		BlockFacing targetFace = blockSel.Face.Opposite;
		BlockPos pos = blockSel.Position.AddCopy(targetFace);
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(pos);
		IMechanicalPowerBlock neighbour = blockEntity?.Block as IMechanicalPowerBlock;
		BEBehaviorMPAxle bempaxle = blockEntity?.GetBehavior<BEBehaviorMPAxle>();
		if (bempaxle == null || !(bempaxle.Block as BlockMPBase).HasMechPowerConnectorAt(world, pos, targetFace, this))
		{
			failureCode = "requiresaxle";
			return false;
		}
		if (bempaxle != null && !BEBehaviorMPAxle.IsAttachedToBlock(world.BlockAccessor, neighbour as Block, pos))
		{
			failureCode = "axlemusthavesupport";
			return false;
		}
		BlockSpurGear toPlaceBlock = world.GetBlock(CodeWithVariant("orientation", targetFace.Code[0].ToString() ?? "")) as BlockSpurGear;
		world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, blockSel.Position);
		BEBehaviorMPBase selfBeh = GetBEBehavior<BEBehaviorMPBase>(blockSel.Position);
		MechPowerPath[] mechPowerExits = selfBeh.GetMechPowerExits(new MechPowerPath
		{
			OutFacing = targetFace
		});
		List<BlockFacing> possiblyNetworklessCandidates = new List<BlockFacing>();
		MechPowerPath[] array = mechPowerExits;
		foreach (MechPowerPath exit in array)
		{
			BlockPos npos = blockSel.Position.AddCopy(exit.OutFacing);
			IMechanicalPowerBlock obj = world.BlockAccessor.GetBlock(npos) as IMechanicalPowerBlock;
			obj?.DidConnectAt(world, blockSel.Position, exit.OutFacing.Opposite);
			if (obj != null && !selfBeh.tryConnect(exit.OutFacing))
			{
				possiblyNetworklessCandidates.Add(exit.OutFacing);
			}
		}
		if (selfBeh.Network != null)
		{
			foreach (BlockFacing face in possiblyNetworklessCandidates)
			{
				selfBeh.tryConnect(face);
			}
		}
		return true;
	}

	public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
	{
		Block nblock = world.BlockAccessor.GetBlock(pos.AddCopy(Orientation));
		if (!(nblock is BlockMPBase) || nblock.SideIsSolid(world.BlockAccessor, pos, Orientation.Opposite.Index))
		{
			world.BlockAccessor.BreakBlock(pos, null);
		}
		else
		{
			base.OnNeighbourBlockChange(world, pos, neibpos);
		}
	}

	public override void DidConnectAt(IWorldAccessor world, BlockPos pos, BlockFacing face)
	{
	}
}
