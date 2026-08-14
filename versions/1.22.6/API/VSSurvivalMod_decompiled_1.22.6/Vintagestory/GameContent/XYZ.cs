using System;

namespace Vintagestory.GameContent;

public struct XYZ(int x, int y, int z) : IEquatable<XYZ>
{
	public int X = x;

	public int Y = y;

	public int Z = z;

	public int this[int index]
	{
		get
		{
			return (2 - index) / 2 * X + index % 2 * Y + index / 2 * Z;
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
			default:
				Z = value;
				break;
			}
		}
	}

	public bool Equals(XYZ other)
	{
		if (other.X == X && other.Y == Y)
		{
			return other.Z == Z;
		}
		return false;
	}
}
