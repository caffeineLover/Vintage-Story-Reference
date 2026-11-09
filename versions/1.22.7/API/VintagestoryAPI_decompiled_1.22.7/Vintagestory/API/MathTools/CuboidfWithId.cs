namespace Vintagestory.API.MathTools;

public class CuboidfWithId : Cuboidf
{
	public string Id;

	public CuboidfWithId()
	{
	}

	public CuboidfWithId(double x1, double y1, double z1, double x2, double y2, double z2)
	{
		Set((float)x1, (float)y1, (float)z1, (float)x2, (float)y2, (float)z2);
	}

	public CuboidfWithId(float x1, float y1, float z1, float x2, float y2, float z2)
	{
		Set(x1, y1, z1, x2, y2, z2);
	}

	public CuboidfWithId(Cuboidf cuboid)
	{
		Set(cuboid);
	}
}
