using Vintagestory.API.MathTools;

namespace Vintagestory.Server;

public struct MapRegionAndPos(Vec3i pos, ServerMapRegion reg)
{
	public Vec3i pos = pos;

	public ServerMapRegion region = reg;
}
