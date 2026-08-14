using System;

namespace Vintagestory.Common.Database;

public struct Xyz(int x, int y, int z) : IEquatable<Xyz>
{
	public int X = x;

	public int Y = y;

	public int Z = z;

	public override readonly int GetHashCode()
	{
		return X ^ Y ^ Z;
	}

	public override readonly bool Equals(object obj)
	{
		if (obj is Xyz other)
		{
			return Equals(other);
		}
		return false;
	}

	public readonly bool Equals(Xyz other)
	{
		if (X == other.X && Y == other.Y)
		{
			return Z == other.Z;
		}
		return false;
	}
}
