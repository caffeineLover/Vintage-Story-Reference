namespace Vintagestory.Common;

public class BoxedPacket : BoxedArray
{
	public int Length;

	public int LengthSent;

	internal int Serialize(IPacket p)
	{
		CitoMemoryStream ms = new CitoMemoryStream(this);
		p.SerializeTo(ms);
		return Length = ms.Position();
	}

	public override void Dispose()
	{
		buffer = null;
		Length = 0;
		LengthSent = 0;
	}

	internal byte[] Clone(int destOffset)
	{
		return ArrayConvert.ByteToByte(buffer, Length, destOffset);
	}
}
