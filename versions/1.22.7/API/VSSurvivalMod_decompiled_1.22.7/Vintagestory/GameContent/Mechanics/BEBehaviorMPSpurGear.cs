using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent.Mechanics;

public class BEBehaviorMPSpurGear : BEBehaviorMPBase
{
	private float angleOffset;

	public BlockFacing Facing => BlockFacing.FromFirstLetter(base.Block.Variant["orientation"]);

	public override float AngleRad => base.AngleRad + angleOffset;

	public BEBehaviorMPSpurGear(BlockEntity blockentity)
		: base(blockentity)
	{
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		angleOffset = (float)Math.PI / 16f * (float)(base.Pos.X % 32 + base.Pos.Y % 32 + base.Pos.Z % 32);
		AxisSign = new int[3];
		switch (Facing.Index)
		{
		case 0:
			AxisSign[2] = -1;
			break;
		case 1:
			AxisSign[0] = -1;
			break;
		case 2:
			AxisSign[2] = -1;
			break;
		case 3:
			AxisSign[0] = -1;
			break;
		case 4:
			AxisSign[1] = 1;
			break;
		case 5:
			AxisSign[1] = 1;
			break;
		}
	}

	public override MechPowerPath[] GetMechPowerExits(MechPowerPath entryDir)
	{
		BlockFacing left;
		BlockFacing right;
		BlockFacing above;
		BlockFacing below;
		if (Facing.IsHorizontal)
		{
			left = ((entryDir.OutFacing.Opposite == Facing) ? entryDir.OutFacing.GetCW() : Facing.GetCW());
			right = ((entryDir.OutFacing.Opposite == Facing) ? entryDir.OutFacing.GetCCW() : Facing.GetCCW());
			above = BlockFacing.UP;
			below = BlockFacing.DOWN;
		}
		else
		{
			left = BlockFacing.WEST;
			right = BlockFacing.EAST;
			above = BlockFacing.NORTH;
			below = BlockFacing.SOUTH;
		}
		BlockPos tmpPos = base.Pos.Copy();
		bool doLeft = Api.World.BlockAccessor.GetBlock(tmpPos.Add(left)) == base.Block;
		bool doRight = Api.World.BlockAccessor.GetBlock(tmpPos.Set(base.Pos).Add(right)) == base.Block;
		bool doAbove = Api.World.BlockAccessor.GetBlock(tmpPos.Set(base.Pos).Add(above)) == base.Block;
		bool doBelow = Api.World.BlockAccessor.GetBlock(tmpPos.Set(base.Pos).Add(below)) == base.Block;
		SmallBoolArray bools = new SmallBoolArray
		{
			[0] = doLeft,
			[1] = doRight,
			[2] = doAbove,
			[3] = doBelow
		};
		MechPowerPath axial = ((entryDir.OutFacing.Opposite == Facing) ? entryDir : entryDir.PropagatedClone(Facing, entryDir.invert, propagationDir));
		if (bools == 0)
		{
			return new MechPowerPath[1] { axial };
		}
		MechPowerPath side = null;
		if (bools == 1)
		{
			side = entryDir.PropagatedClone(left, !entryDir.invert, propagationDir.Opposite);
		}
		if (bools == 2)
		{
			side = entryDir.PropagatedClone(right, !entryDir.invert, propagationDir.Opposite);
		}
		if (bools == 4)
		{
			side = entryDir.PropagatedClone(above, !entryDir.invert, propagationDir.Opposite);
		}
		if (bools == 8)
		{
			side = entryDir.PropagatedClone(below, !entryDir.invert, propagationDir.Opposite);
		}
		if (side != null)
		{
			return new MechPowerPath[2] { axial, side };
		}
		int num = 1;
		List<MechPowerPath> list = new List<MechPowerPath>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<MechPowerPath> span = CollectionsMarshal.AsSpan(list);
		int index = 0;
		span[index] = axial;
		List<MechPowerPath> paths = list;
		if (doLeft)
		{
			paths.Add(entryDir.PropagatedClone(left, !entryDir.invert, propagationDir.Opposite));
		}
		if (doRight)
		{
			paths.Add(entryDir.PropagatedClone(right, !entryDir.invert, propagationDir.Opposite));
		}
		if (doAbove)
		{
			paths.Add(entryDir.PropagatedClone(above, !entryDir.invert, propagationDir.Opposite));
		}
		if (doBelow)
		{
			paths.Add(entryDir.PropagatedClone(below, !entryDir.invert, propagationDir.Opposite));
		}
		return paths.ToArray();
	}

	public override BlockFacing GetPropagatingTurnDir(BlockFacing toFacing)
	{
		return propagationDir.Opposite;
	}

	public override float GetResistance()
	{
		return 0.0005f;
	}
}
