using System;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent.Mechanics;

public class BlockWaterWheel : BlockWindmillRotor
{
	public override bool HasMechPowerConnectorAt(IWorldAccessor world, BlockPos pos, BlockFacing face, BlockMPBase forBlock)
	{
		if (face != powerOutFacing)
		{
			return face == powerOutFacing.Opposite;
		}
		return true;
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		return (from wi in base.GetPlacedBlockInteractionHelp(world, selection, forPlayer)
			where wi.ActionLangCode != "heldhelp-addsails"
			select wi).ToArray();
	}

	public override bool CanPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref string failureCode)
	{
		bool invalid;
		BlockFacing facing = GetFacingForPlacement(world, blockSel.Position, doPlace: false, out invalid);
		if (invalid)
		{
			return false;
		}
		if (facing == null)
		{
			BlockFacing[] facings = Block.SuggestedHVOrientation(byPlayer, blockSel);
			if (facings != null && facings.Length != 0)
			{
				facing = facings[0];
			}
		}
		if (facing != null)
		{
			BlockPos pos = blockSel.Position.Copy();
			int xMin = 0;
			int xMax = 0;
			int zMin = 0;
			int zMax = 0;
			if (facing.IsAxisNS)
			{
				xMin = -2;
				xMax = 2;
			}
			else
			{
				zMin = -2;
				zMax = 2;
			}
			for (int x = xMin; x <= xMax; x++)
			{
				for (int z = zMin; z <= zMax; z++)
				{
					for (int y = -2; y <= 2; y++)
					{
						if (x == 0 && y == 0 && z == 0)
						{
							continue;
						}
						bool beyondRadius = Math.Abs(x) > 1 || Math.Abs(y) > 1 || Math.Abs(z) > 1;
						pos.Set(blockSel.Position.X + x, blockSel.Position.Y + y, blockSel.Position.Z + z);
						if (world.BlockAccessor.GetBlock(pos, 1) is IMPPowered MPblock && (!beyondRadius || (MPblock is BlockWindmillRotor rotor && rotor.GetFacing()?.Axis == facing.Axis)))
						{
							failureCode = "rotationblocked";
							return false;
						}
						if (!beyondRadius)
						{
							pos.Add(facing.Opposite);
							if (world.BlockAccessor.GetBlock(pos, 1) is BlockWindmillRotor rotor2 && rotor2.GetFacing()?.Axis != facing.Axis)
							{
								failureCode = "rotationblocked";
								return false;
							}
							pos.Add(facing);
							pos.Add(facing);
							if (world.BlockAccessor.GetBlock(pos, 1) is BlockWindmillRotor rotor3 && rotor3.GetFacing()?.Axis != facing.Axis)
							{
								failureCode = "rotationblocked";
								return false;
							}
						}
					}
				}
			}
		}
		return base.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode);
	}
}
