using System;

namespace Vintagestory.GameContent;

public struct Voxel(byte x, byte y, byte z) : IEquatable<Voxel>
{
	public byte x = x;

	public byte y = y;

	public byte z = z;

	public bool Equals(Voxel other)
	{
		if (x == other.x && y == other.y)
		{
			return z == other.z;
		}
		return false;
	}
}
