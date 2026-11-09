using System;
using ProtoBuf;

namespace Vintagestory.API.MathTools;

/// <summary>
/// Represents a vector of 2 ints. Go bug Tyron if you need more utility methods in this class.
/// </summary>
[DocumentAsJson]
[ProtoContract]
public struct Vec2iStruct : IEquatable<Vec2iStruct>
{
	/// <summary>
	/// The X element of the vector.
	/// </summary>
	[ProtoMember(1)]
	[DocumentAsJson("Optional", "0", false)]
	public int X;

	/// <summary>
	/// The Y element of the vector.
	/// </summary>
	[ProtoMember(2)]
	[DocumentAsJson("Optional", "0", false)]
	public int Y;

	/// <summary>
	/// Will always return a number in the range 0..1023, no matter what the value of X and Y
	/// </summary>
	public int ToInChunkIndex => (Y & 0x1F) * 32 + (X & 0x1F);

	public int this[int index]
	{
		get
		{
			if (index != 0)
			{
				return Y;
			}
			return X;
		}
		set
		{
			switch (index)
			{
			case 0:
				X = value;
				break;
			case 1:
				Y = value;
				break;
			}
		}
	}

	public Vec2iStruct()
	{
		X = 0;
		Y = 0;
	}

	public Vec2iStruct(int x, int y)
	{
		X = x;
		Y = y;
	}

	public Vec2iStruct(Vec3d pos)
	{
		X = (int)pos.X;
		Y = (int)pos.Z;
	}

	public bool Equals(Vec2iStruct other)
	{
		if (X == other.X)
		{
			return Y == other.Y;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is Vec2i)
		{
			Vec2i pos = (Vec2i)obj;
			if (X == pos.X)
			{
				return Y == pos.Y;
			}
			return false;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (17 * 23 + X.GetHashCode()) * 23 + Y.GetHashCode();
	}

	[Obsolete("Use the correctly-spelled ManhattanDistance instead")]
	public int ManhattenDistance(Vec2i point)
	{
		return ManhattanDistance(point);
	}

	public int ManhattanDistance(Vec2i point)
	{
		return Math.Abs(X - point.X) + Math.Abs(Y - point.Y);
	}

	[Obsolete("Use the correctly-spelled ManhattanDistance instead")]
	public int ManhattenDistance(int x, int y)
	{
		return ManhattanDistance(x, y);
	}

	public int ManhattanDistance(int x, int y)
	{
		return Math.Abs(X - x) + Math.Abs(Y - y);
	}

	public Vec2iStruct Set(int x, int y)
	{
		X = x;
		Y = y;
		return this;
	}

	public Vec2iStruct Set(Vec2i vec)
	{
		X = vec.X;
		Y = vec.Y;
		return this;
	}

	public override string ToString()
	{
		return X + " / " + Y;
	}

	public Vec2iStruct Add(int dx, int dy)
	{
		X += dx;
		Y += dy;
		return this;
	}

	/// <summary>
	/// 27 lowest bits for X Coordinate, then 27 bits for Z coordinate
	/// </summary>
	/// <returns></returns>
	public ulong ToChunkIndex()
	{
		return (ulong)(((long)Y << 27) | (uint)X);
	}

	public static Vec2iStruct operator -(Vec2iStruct left, Vec2iStruct right)
	{
		return new Vec2iStruct(left.X - right.X, left.Y - right.Y);
	}

	public static Vec2iStruct operator +(Vec2iStruct left, Vec2iStruct right)
	{
		return new Vec2iStruct(left.X + right.X, left.Y + right.Y);
	}

	public static Vec2iStruct operator -(Vec2iStruct left, int right)
	{
		return new Vec2iStruct(left.X - right, left.Y - right);
	}

	public static Vec2iStruct operator -(int left, Vec2iStruct right)
	{
		return new Vec2iStruct(left - right.X, left - right.Y);
	}

	public static Vec2iStruct operator +(Vec2iStruct left, int right)
	{
		return new Vec2iStruct(left.X + right, left.Y + right);
	}

	public static Vec2iStruct operator *(Vec2iStruct left, int right)
	{
		return new Vec2iStruct(left.X * right, left.Y * right);
	}

	public static Vec2iStruct operator *(int left, Vec2iStruct right)
	{
		return new Vec2iStruct(left * right.X, left * right.Y);
	}

	public static Vec2iStruct operator *(Vec2iStruct left, double right)
	{
		return new Vec2iStruct((int)((double)left.X * right), (int)((double)left.Y * right));
	}

	public static Vec2iStruct operator *(double left, Vec2iStruct right)
	{
		return new Vec2iStruct((int)(left * (double)right.X), (int)(left * (double)right.Y));
	}

	public static double operator *(Vec2iStruct left, Vec2iStruct right)
	{
		return left.X * right.X + left.Y * right.Y;
	}

	public static Vec2iStruct operator /(Vec2iStruct left, int right)
	{
		return new Vec2iStruct(left.X / right, left.Y / right);
	}

	public static Vec2iStruct operator /(Vec2iStruct left, float right)
	{
		return new Vec2iStruct((int)((float)left.X / right), (int)((float)left.Y / right));
	}

	public static bool operator ==(Vec2iStruct left, Vec2iStruct right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(Vec2iStruct left, Vec2iStruct right)
	{
		return !(left == right);
	}
}
