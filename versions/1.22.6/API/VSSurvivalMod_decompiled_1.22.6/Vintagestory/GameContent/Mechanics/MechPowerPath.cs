using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent.Mechanics;

public class MechPowerPath
{
	public BlockFacing OutFacing;

	public bool invert;

	public float gearingRatio;

	private readonly BlockPos fromPos;

	public BlockFacing turnDir;

	public MechPowerPath()
	{
	}

	public MechPowerPath(BlockFacing facing, float gearingRatioHere, BlockPos fromPos = null, bool inverted = false)
	{
		OutFacing = facing;
		invert = inverted;
		gearingRatio = gearingRatioHere;
		this.fromPos = fromPos;
	}

	public BlockFacing NetworkDir()
	{
		BlockFacing opposite = turnDir;
		if (opposite == null)
		{
			if (!invert)
			{
				return OutFacing;
			}
			opposite = OutFacing.Opposite;
		}
		return opposite;
	}

	public bool IsInvertedTowards(BlockPos testPos)
	{
		if (!(fromPos == null))
		{
			return fromPos.AddCopy(NetworkDir()) != testPos;
		}
		return invert;
	}

	public MechPowerPath PropagatedClone(BlockFacing outfacing, bool inverted = false, BlockFacing turnDir = null, BlockPos fromPos = null)
	{
		return new MechPowerPath(outfacing, gearingRatio, fromPos, inverted)
		{
			turnDir = turnDir
		};
	}
}
