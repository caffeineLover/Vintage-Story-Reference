using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TentBagReworked.Config;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace TentBagReworked.Util
{
	// Token: 0x02000005 RID: 5
	[NullableContext(1)]
	[Nullable(0)]
	public static class TentArea
	{
		// Token: 0x0600004A RID: 74 RVA: 0x000048C4 File Offset: 0x00002AC4
		public static void GetPackBounds(TentBagReworkedConfig config, IBlockAccessor blockAccessor, BlockPos lookedAt, out BlockPos start, out BlockPos end)
		{
			Block block = blockAccessor.GetBlock(lookedAt);
			bool flag;
			if (config.ReplacePlantsAndRocks)
			{
				int? num = (block != null) ? new int?(block.Replaceable) : default(int?);
				if (num != null)
				{
					int valueOrDefault = num.GetValueOrDefault();
					if (valueOrDefault >= 5500)
					{
						flag = (valueOrDefault <= 6500);
						goto IL_57;
					}
				}
				flag = false;
				IL_57:;
			}
			else
			{
				flag = false;
			}
			int y = flag ? 1 : 0;
			int floorShift = config.GrabFloor ? -1 : 0;
			start = lookedAt.AddCopy(-config.MaxRadius, 1 - y + floorShift, -config.MaxRadius);
			end = lookedAt.AddCopy(config.MaxRadius, Math.Max(config.MaxHeight, 3), config.MaxRadius);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004980 File Offset: 0x00002B80
		public static List<BlockPos> GetCuboidEdges(BlockPos a, BlockPos b)
		{
			int minX = Math.Min(a.X, b.X);
			int maxX = Math.Max(a.X, b.X);
			int minY = Math.Min(a.Y, b.Y);
			int maxY = Math.Max(a.Y, b.Y);
			int minZ = Math.Min(a.Z, b.Z);
			int maxZ = Math.Max(a.Z, b.Z);
			int dim = a.dimension;
			List<BlockPos> edges = new List<BlockPos>();
			for (int x = minX; x <= maxX; x++)
			{
				bool xEdge = x == minX || x == maxX;
				for (int y = minY; y <= maxY; y++)
				{
					bool yEdge = y == minY || y == maxY;
					for (int z = minZ; z <= maxZ; z++)
					{
						bool zEdge = z == minZ || z == maxZ;
						int onBoundary = (xEdge ? 1 : 0) + (yEdge ? 1 : 0) + (zEdge ? 1 : 0);
						bool flag = onBoundary >= 2;
						if (flag)
						{
							edges.Add(new BlockPos(x, y, z, dim));
						}
					}
				}
			}
			return edges;
		}
	}
}
