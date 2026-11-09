namespace Vintagestory.API.MathTools;

/// <summary>
/// Not really a sphere, actually now an AABB centred on x,y,z, but we keep the name for API consistency
/// </summary>
public struct Sphere(float x1, float y1, float z1, float dx, float dy, float dz)
{
	public float x = x1;

	public float y = y1;

	public float z = z1;

	public float radius = 0.8660254f * dx;

	public float radiusY = 0.8660254f * dy;

	public float radiusZ = 0.8660254f * dz;

	public const float sqrt3half = 0.8660254f;

	public static Sphere BoundingSphereForCube(float x, float y, float z, float size)
	{
		return new Sphere
		{
			x = x + size / 2f,
			y = y + size / 2f,
			z = z + size / 2f,
			radius = 0.8660254f * size,
			radiusY = 0.8660254f * size,
			radiusZ = 0.8660254f * size
		};
	}
}
