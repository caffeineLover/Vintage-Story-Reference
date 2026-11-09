using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent.Mechanics;

public class ModSystemWindTurbulence : ModSystem
{
	private Dictionary<long, RegionTurbulenceSources> windmillsByRegion = new Dictionary<long, RegionTurbulenceSources>();

	private int regionSize;

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return forSide == EnumAppSide.Server;
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		base.StartServerSide(api);
		regionSize = api.World.BlockAccessor.RegionSize;
	}

	public void SetTurbulence(BlockPos pos, float radius)
	{
		getorCreateSources(pos).RadiiByPosition[pos.Copy()] = radius;
	}

	public void RemoveTurbulence(BlockPos pos)
	{
		getSources(pos)?.RadiiByPosition.Remove(pos);
	}

	public float GetNearestTurbulenceDistance(BlockPos pos)
	{
		float mindistsq = float.MaxValue;
		for (int dx = -1; dx <= 1; dx++)
		{
			for (int dz = -1; dz <= 1; dz++)
			{
				float distsq = getSourceMinDist(pos, getSources(pos.AddCopy(dx * regionSize, 0, dz * regionSize)));
				if (distsq < mindistsq)
				{
					mindistsq = distsq;
				}
			}
		}
		return mindistsq;
	}

	private static float getSourceMinDist(BlockPos pos, RegionTurbulenceSources src)
	{
		if (src == null)
		{
			return float.MaxValue;
		}
		float minDist = float.MaxValue;
		foreach (KeyValuePair<BlockPos, float> val in src.RadiiByPosition)
		{
			if (!(val.Key == pos))
			{
				float dist = Math.Max(0f, pos.DistanceTo(val.Key) - val.Value);
				if (dist < minDist)
				{
					minDist = dist;
				}
			}
		}
		return minDist;
	}

	private RegionTurbulenceSources getorCreateSources(BlockPos pos)
	{
		long key = ((long)pos.Z / (long)regionSize << 31) + pos.X / regionSize;
		if (!windmillsByRegion.TryGetValue(key, out var sources))
		{
			return windmillsByRegion[key] = new RegionTurbulenceSources();
		}
		return sources;
	}

	private RegionTurbulenceSources getSources(BlockPos pos)
	{
		long key = ((long)pos.Z / (long)regionSize << 31) + pos.X / regionSize;
		if (!windmillsByRegion.TryGetValue(key, out var sources))
		{
			return null;
		}
		return sources;
	}
}
