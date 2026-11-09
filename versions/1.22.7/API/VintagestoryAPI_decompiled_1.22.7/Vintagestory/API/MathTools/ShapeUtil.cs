using System;
using System.Collections.Generic;
using System.Linq;

namespace Vintagestory.API.MathTools;

public static class ShapeUtil
{
	public static int MaxShells = 38;

	public static int GetLength(int radius)
	{
		return (2 * radius + 1) * (2 * radius + 1) * 6;
	}

	public static FastVec3f GetRay(int r, int j)
	{
		int length = GetLength(r) / 6;
		Vec3i facingNormali = BlockFacing.ALLFACES[j / length].Normali;
		j %= length;
		int a = j / (2 * r + 1) - r;
		int b = j % (2 * r + 1) - r;
		FastVec3f pos = new FastVec3f(facingNormali.X * r, facingNormali.Y * r, facingNormali.Z * r);
		if (pos.X == 0f)
		{
			pos.X = a;
			if (pos.Y == 0f)
			{
				pos.Y = b;
			}
		}
		else if (pos.Y == 0f)
		{
			pos.Y = a;
		}
		if (pos.Z == 0f)
		{
			pos.Z = b;
		}
		return pos.Normalize();
	}

	public static Vec3f[] GetCachedCubicShellNormalizedVectors(int radius)
	{
		Vec3f[] result = new Vec3f[GetLength(radius)];
		for (int j = 0; j < result.Length; j++)
		{
			result[j] = new Vec3f(GetRay(radius, j));
		}
		return result;
	}

	public static FastVec3f[] GetCubicShellNormalizedVectors(int radius)
	{
		FastVec3f[] result = new FastVec3f[GetLength(radius)];
		for (int j = 0; j < result.Length; j++)
		{
			result[j] = GetRay(radius, j);
		}
		return result;
	}

	public static Vec3i[] GenCubicShellVectors(int r)
	{
		int[] ab = new int[2];
		Vec3i[] vectors = new Vec3i[GetLength(r)];
		int j = 0;
		BlockFacing[] aLLFACES = BlockFacing.ALLFACES;
		foreach (BlockFacing facing in aLLFACES)
		{
			ab[0] = -r;
			while (ab[0] <= r)
			{
				ab[1] = -r;
				while (ab[1] <= r)
				{
					Vec3i pos = new Vec3i(facing.Normali.X * r, facing.Normali.Y * r, facing.Normali.Z * r);
					int l = 0;
					if (pos.X == 0)
					{
						pos.X = ab[l++];
					}
					if (pos.Y == 0)
					{
						pos.Y = ab[l++];
					}
					if (l < 2 && pos.Z == 0)
					{
						pos.Z = ab[l++];
					}
					vectors[j++] = pos;
					ab[1]++;
				}
				ab[0]++;
			}
		}
		return vectors;
	}

	/// <summary>
	/// Returns an array of vectors for each point in a square, sorted by manhattan distance to center, exluding the center point
	/// </summary>
	/// <param name="halflength"></param>
	/// <returns></returns>
	public static Vec2i[] GetSquarePointsSortedByMDist(int halflength)
	{
		if (halflength == 0)
		{
			return Array.Empty<Vec2i>();
		}
		Vec2i[] result = new Vec2i[(2 * halflength + 1) * (2 * halflength + 1) - 1];
		int i = 0;
		for (int x = -halflength; x <= halflength; x++)
		{
			for (int y = -halflength; y <= halflength; y++)
			{
				if (x != 0 || y != 0)
				{
					result[i++] = new Vec2i(x, y);
				}
			}
		}
		return result.OrderBy((Vec2i vec) => vec.ManhattanDistance(0, 0)).ToArray();
	}

	/// <summary>
	/// Returns a square outline of given radius (only for odd lengths)
	/// </summary>
	/// <param name="halflength"></param>
	/// <returns></returns>
	public static Vec2i[] GetHollowSquarePoints(int halflength)
	{
		if (halflength == 0)
		{
			return Array.Empty<Vec2i>();
		}
		int radius = halflength * 2 + 1;
		Vec2i[] result = new Vec2i[radius * 4 - 4];
		int j = 0;
		for (int i = 0; i < radius * 4 - 1; i++)
		{
			int x = i % radius - halflength;
			int y = i % radius - halflength;
			int quadrant = i / radius;
			switch (quadrant)
			{
			case 0:
				y = -halflength;
				break;
			case 1:
				x = halflength;
				break;
			case 2:
				y = halflength;
				x = -x;
				break;
			case 3:
				x = -halflength;
				y = -y;
				break;
			}
			result[j++] = new Vec2i(x, y);
			if ((i + 1) / radius > quadrant)
			{
				i++;
			}
		}
		return result;
	}

	public static Vec2i[] GetOctagonPoints(int x, int y, int r)
	{
		if (r == 0)
		{
			return new Vec2i[1]
			{
				new Vec2i(x, y)
			};
		}
		List<Vec2i> points = new List<Vec2i>();
		int th = 9;
		int S = 2 * r;
		int a = Math.Min(S, th);
		int b = (int)Math.Ceiling((double)Math.Max(0, S - th) / 2.0);
		int a2 = a / 2;
		for (int i = 0; i < a; i++)
		{
			points.Add(new Vec2i(x + i - a2, y - r));
			points.Add(new Vec2i(x - i + a2, y + r));
			points.Add(new Vec2i(x - r, y - i + a2));
			points.Add(new Vec2i(x + r, y + i - a2));
		}
		for (int j = 0; j < b; j++)
		{
			points.Add(new Vec2i(x + a2 + j, y - r + j));
			points.Add(new Vec2i(x - r + j, y + a2 + j));
			points.Add(new Vec2i(x - r + j, y - a2 - j));
			points.Add(new Vec2i(x + a2 + j, y + r - j));
		}
		return Enumerable.ToArray(points);
	}

	public static void LoadOctagonIndices(ICollection<long> list, int x, int y, int r, int mapSizeX)
	{
		if (r == 0)
		{
			list.Add(MapUtil.Index2dL(x, y, mapSizeX));
			return;
		}
		int S = 2 * r;
		int a = Math.Min(S, 9);
		int b = (int)((double)Math.Max(0, S - 9) / Math.Sqrt(2.0));
		int a2 = a / 2;
		for (int i = 0; i < a; i++)
		{
			list.Add(MapUtil.Index2dL(x + i - a2, y - r, mapSizeX));
			list.Add(MapUtil.Index2dL(x - i + a2, y + r, mapSizeX));
			list.Add(MapUtil.Index2dL(x - r, y - i + a2, mapSizeX));
			list.Add(MapUtil.Index2dL(x + r, y + i - a2, mapSizeX));
		}
		for (int j = 0; j < b; j++)
		{
			list.Add(MapUtil.Index2dL(x + a2 + j, y - r + j, mapSizeX));
			list.Add(MapUtil.Index2dL(x - r + j, y + a2 + j, mapSizeX));
			list.Add(MapUtil.Index2dL(x - r + j, y - a2 - j, mapSizeX));
			list.Add(MapUtil.Index2dL(x + a2 + j, y + r - j, mapSizeX));
		}
	}

	public static Vec2i[] GetPointsOfCircle(int xm, int ym, int r)
	{
		List<Vec2i> points = new List<Vec2i>();
		int x = -r;
		int y = 0;
		int err = 2 - 2 * r;
		do
		{
			points.Add(new Vec2i(xm - x, ym + y));
			points.Add(new Vec2i(xm - y, ym - x));
			points.Add(new Vec2i(xm + x, ym - y));
			points.Add(new Vec2i(xm + y, ym + x));
			r = err;
			if (r <= y)
			{
				err += ++y * 2 + 1;
			}
			if (r > x || err > y)
			{
				err += ++x * 2 + 1;
			}
		}
		while (x < 0);
		return Enumerable.ToArray(points);
	}
}
