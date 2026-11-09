using System.Collections.Generic;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace Vintagestory.Server;

internal class EntityRangeComparer : IComparer<Entity>
{
	private Vec3d origin;

	public EntityRangeComparer(Vec3d origin)
	{
		this.origin = origin;
	}

	int IComparer<Entity>.Compare(Entity a, Entity b)
	{
		if (a == null || b == null)
		{
			return 0;
		}
		if (!(a.Pos.HorDistanceTo(origin) < b.Pos.HorDistanceTo(origin)))
		{
			return 1;
		}
		return -1;
	}
}
