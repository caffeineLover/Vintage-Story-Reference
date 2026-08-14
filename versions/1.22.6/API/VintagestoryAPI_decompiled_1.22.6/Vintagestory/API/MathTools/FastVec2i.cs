using System;
using ProtoBuf;

namespace Vintagestory.API.MathTools;

/// <summary>
/// Represents a vector of 2 ints. Go bug Tyron if you need more utility methods in this class.
/// </summary>
[DocumentAsJson]
[ProtoContract]
public struct FastVec2i : IEquatable<FastVec2i>
{
	[ProtoMember(1)]
	public ulong val;

	/// <summary>
	/// The X-Component of the vector
	/// </summary>
	[DocumentAsJson("Optional", "0", false)]
	public int X
	{
		get
		{
			return (int)val;
		}
		set
		{
			val = (val & 0xFFFFFFFF00000000uL) + (uint)value;
		}
	}

	/// <summary>
	/// The Y-Component of the vector
	/// </summary>
	[DocumentAsJson("Optional", "0", false)]
	public int Y
	{
		get
		{
			return (int)(val >> 32);
		}
		set
		{
			val = (ulong)((uint)val + ((long)value << 32));
		}
	}

	/// <summary>Identical to .Y - optionally, you can use .Z instead for code readability if the vector represents X,Z values</summary>
	public int Z
	{
		get
		{
			return (int)(val >> 32);
		}
		set
		{
			val = (ulong)((uint)val + ((long)value << 32));
		}
	}

	public static FastVec2i Zero => new FastVec2i(0, 0);

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

	public FastVec2i()
	{
		val = 0uL;
	}

	public FastVec2i(int x, int y)
	{
		val = (ulong)((uint)x + ((long)y << 32));
	}

	public FastVec2i(Vec3d pos)
	{
		this = new FastVec2i((int)pos.X, (int)pos.Z);
	}

	public bool Equals(FastVec2i other)
	{
		return val == other.val;
	}

	public override bool Equals(object? obj)
	{
		if (obj is FastVec2i other)
		{
			return val == other.val;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (int)(val >> 32) * 23 * 17 + (int)val;
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

	public FastVec2i Set(int x, int y)
	{
		val = (ulong)((uint)x + ((long)y << 32));
		return this;
	}

	public FastVec2i Set(Vec2i vec)
	{
		val = (ulong)((uint)vec.X + ((long)vec.Y << 32));
		return this;
	}

	public FastVec2i Copy()
	{
		return this;
	}

	public override string ToString()
	{
		return X + " / " + Y;
	}

	public FastVec2i Add(int dx, int dy)
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

	public static FastVec2i operator -(FastVec2i left, FastVec2i right)
	{
		return new FastVec2i(left.X - right.X, left.Y - right.Y);
	}

	public static FastVec2i operator +(FastVec2i left, FastVec2i right)
	{
		return new FastVec2i(left.X + right.X, left.Y + right.Y);
	}

	public static FastVec2i operator -(FastVec2i left, int right)
	{
		return new FastVec2i(left.X - right, left.Y - right);
	}

	public static FastVec2i operator -(int left, FastVec2i right)
	{
		return new FastVec2i(left - right.X, left - right.Y);
	}

	public static FastVec2i operator +(FastVec2i left, int right)
	{
		return new FastVec2i(left.X + right, left.Y + right);
	}

	public static FastVec2i operator *(FastVec2i left, int right)
	{
		return new FastVec2i(left.X * right, left.Y * right);
	}

	public static FastVec2i operator *(int left, FastVec2i right)
	{
		return new FastVec2i(left * right.X, left * right.Y);
	}

	public static FastVec2i operator *(FastVec2i left, double right)
	{
		return new FastVec2i((int)((double)left.X * right), (int)((double)left.Y * right));
	}

	public static FastVec2i operator *(double left, FastVec2i right)
	{
		return new FastVec2i((int)(left * (double)right.X), (int)(left * (double)right.Y));
	}

	public static double operator *(FastVec2i left, FastVec2i right)
	{
		return left.X * right.X + left.Y * right.Y;
	}

	public static FastVec2i operator /(FastVec2i left, int right)
	{
		return new FastVec2i(left.X / right, left.Y / right);
	}

	public static FastVec2i operator /(FastVec2i left, float right)
	{
		return new FastVec2i((int)((float)left.X / right), (int)((float)left.Y / right));
	}

	public static bool operator ==(FastVec2i left, FastVec2i right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(FastVec2i left, FastVec2i right)
	{
		return !left.Equals(right);
	}
}
