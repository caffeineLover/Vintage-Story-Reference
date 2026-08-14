namespace Vintagestory.API.MathTools;

public class FluidBlockPos : BlockPos
{
	public FluidBlockPos()
	{
	}

	public FluidBlockPos(int x, int y, int z, int dim)
		: base(x, y, z, dim)
	{
	}

	public override BlockPos Copy()
	{
		return new FluidBlockPos(X, Y, Z, dimension);
	}
}
