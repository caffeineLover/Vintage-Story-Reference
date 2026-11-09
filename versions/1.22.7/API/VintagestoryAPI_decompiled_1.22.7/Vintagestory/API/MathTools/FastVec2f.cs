using System;

namespace Vintagestory.API.MathTools;

/// <summary>
/// A fast version of <see cref="T:Vintagestory.API.MathTools.Vec2f" />
/// </summary>
[DocumentAsJson]
public struct FastVec2f : IEquatable<FastVec2f>
{
	private const float tinyEpsilon = 1E-20f;

	public static readonly FastVec2f Zero;

	/// <summary>
	/// The X-Component of the vector
	/// </summary>
	[DocumentAsJson("Optional", "0", false)]
	public float X;

	/// <summary>
	/// The Y-Component of the vector
	/// </summary>
	[DocumentAsJson("Optional", "0", false)]
	public float Y;

	public float A => X;

	public float B => Y;

	/// <summary>Identical to .Y - optionally, you can use .Z instead for code readability if the vector represents X,Z values</summary>
	public float Z => Y;

	public FastVec2f()
	{
		X = 0f;
		Y = 0f;
	}

	public FastVec2f(float x, float y)
	{
		X = x;
		Y = y;
	}

	public FastVec2f(Size2i size)
	{
		X = size.Width;
		Y = size.Height;
	}

	public override string ToString()
	{
		return "X=" + X + ", Y=" + Y;
	}

	public float Length()
	{
		return GameMath.Sqrt(X * X + Y * Y);
	}

	public float LengthSq()
	{
		return X * X + Y * Y;
	}

	public float DistanceTo(float x, float y)
	{
		float num = X - x;
		float dy = Y - y;
		return GameMath.Sqrt(num * num + dy * dy);
	}

	public float DistanceTo(FastVec2f p)
	{
		float num = X - p.X;
		float dy = Y - p.Y;
		return GameMath.Sqrt(num * num + dy * dy);
	}

	public static float Distance(float x1, float y1, float x2, float y2)
	{
		float num = x2 - x1;
		float dy = y2 - y1;
		return GameMath.Sqrt(num * num + dy * dy);
	}

	public static float Dot(FastVec2f a, FastVec2f b)
	{
		return a.X * b.X + a.Y * b.Y;
	}

	public FastVec2f Normalize()
	{
		float ls = LengthSq();
		if (ls < 1E-20f)
		{
			return new FastVec2f(1f, 0f);
		}
		return this / GameMath.Sqrt(ls);
	}

	public FastVec2f LeftNormal()
	{
		return new FastVec2f(0f - Y, X);
	}

	public FastVec2f Rotate(float angleRad)
	{
		float c = GameMath.Cos(angleRad);
		float s = GameMath.Sin(angleRad);
		return new FastVec2f(c * X - s * Y, s * X + c * Y);
	}

	public override readonly bool Equals(object obj)
	{
		if (obj is FastVec2f f)
		{
			return Equals(f);
		}
		return false;
	}

	public readonly bool Equals(FastVec2f f)
	{
		if (X == f.X)
		{
			return Y == f.Y;
		}
		return false;
	}

	public override readonly int GetHashCode()
	{
		return HashCode.Combine(X, Y);
	}

	public static FastVec2f operator -(FastVec2f left, FastVec2f right)
	{
		return new FastVec2f(left.X - right.X, left.Y - right.Y);
	}

	public static FastVec2f operator +(FastVec2f left, FastVec2f right)
	{
		return new FastVec2f(left.X + right.X, left.Y + right.Y);
	}

	public static FastVec2f operator -(FastVec2f left, float right)
	{
		return new FastVec2f(left.X - right, left.Y - right);
	}

	public static FastVec2f operator -(float left, FastVec2f right)
	{
		return new FastVec2f(left - right.X, left - right.Y);
	}

	public static FastVec2f operator +(FastVec2f left, float right)
	{
		return new FastVec2f(left.X + right, left.Y + right);
	}

	public static FastVec2f operator *(FastVec2f left, float right)
	{
		return new FastVec2f(left.X * right, left.Y * right);
	}

	public static FastVec2f operator *(float left, FastVec2f right)
	{
		return new FastVec2f(left * right.X, left * right.Y);
	}

	public static FastVec2f operator *(FastVec2f left, double right)
	{
		return new FastVec2f(left.X * (float)right, left.Y * (float)right);
	}

	public static FastVec2f operator *(double left, FastVec2f right)
	{
		return new FastVec2f((float)left * right.X, (float)left * right.Y);
	}

	public static double operator *(FastVec2f left, FastVec2f right)
	{
		return left.X * right.X + left.Y * right.Y;
	}

	public static FastVec2f operator /(FastVec2f left, float right)
	{
		return new FastVec2f(left.X / right, left.Y / right);
	}

	public static FastVec2f operator +(FastVec2f left, Vec2i right)
	{
		return new FastVec2f(left.X + (float)right.X, left.Y + (float)right.Y);
	}

	public static bool operator ==(FastVec2f left, FastVec2f right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(FastVec2f left, FastVec2f right)
	{
		return !(left == right);
	}

	static FastVec2f()
	{
		Zero = new FastVec2f(0f, 0f);
	}
}
