using System;

public class Packet_CompositeShapeSerializer
{
	private const int field = 8;

	public static Packet_CompositeShape DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_CompositeShape instance = new Packet_CompositeShape();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_CompositeShape DeserializeBuffer(byte[] buffer, int length, Packet_CompositeShape instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_CompositeShape Deserialize(CitoMemoryStream stream, Packet_CompositeShape instance)
	{
		instance.InitializeValues();
		int keyInt;
		while (true)
		{
			keyInt = stream.ReadByte();
			if ((keyInt & 0x80) != 0)
			{
				keyInt = ProtocolParser.ReadKeyAsInt(keyInt, stream);
				if ((keyInt & 0x4000) != 0)
				{
					break;
				}
			}
			switch (keyInt)
			{
			case 0:
				return null;
			case 10:
				instance.Base = ProtocolParser.ReadString(stream);
				break;
			case 16:
				instance.Rotatex = ProtocolParser.ReadUInt32(stream);
				break;
			case 24:
				instance.Rotatey = ProtocolParser.ReadUInt32(stream);
				break;
			case 32:
				instance.Rotatez = ProtocolParser.ReadUInt32(stream);
				break;
			case 42:
				instance.AlternatesAdd(DeserializeLengthDelimitedNew(stream));
				break;
			case 90:
				instance.OverlaysAdd(DeserializeLengthDelimitedNew(stream));
				break;
			case 48:
				instance.VoxelizeShape = ProtocolParser.ReadUInt32(stream);
				break;
			case 58:
				instance.SelectiveElementsAdd(ProtocolParser.ReadString(stream));
				break;
			case 138:
				instance.IgnoreElementsAdd(ProtocolParser.ReadString(stream));
				break;
			case 64:
				instance.QuantityElements = ProtocolParser.ReadUInt32(stream);
				break;
			case 72:
				instance.QuantityElementsSet = ProtocolParser.ReadUInt32(stream);
				break;
			case 80:
				instance.Format = ProtocolParser.ReadUInt32(stream);
				break;
			case 96:
				instance.Offsetx = ProtocolParser.ReadUInt32(stream);
				break;
			case 104:
				instance.Offsety = ProtocolParser.ReadUInt32(stream);
				break;
			case 112:
				instance.Offsetz = ProtocolParser.ReadUInt32(stream);
				break;
			case 120:
				instance.InsertBakedTextures = ProtocolParser.ReadBool(stream);
				break;
			case 128:
				instance.ScaleAdjust = ProtocolParser.ReadUInt32(stream);
				break;
			default:
				ProtocolParser.SkipKey(stream, Key.Create(keyInt));
				break;
			}
		}
		if (keyInt >= 0)
		{
			return null;
		}
		return instance;
	}

	public static Packet_CompositeShape DeserializeLengthDelimited(CitoMemoryStream stream, Packet_CompositeShape instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_CompositeShape result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_CompositeShape instance)
	{
		if (instance.Base != null)
		{
			stream.WriteByte(10);
			ProtocolParser.WriteString(stream, instance.Base);
		}
		if (instance.Rotatex != 0)
		{
			stream.WriteByte(16);
			ProtocolParser.WriteUInt32(stream, instance.Rotatex);
		}
		if (instance.Rotatey != 0)
		{
			stream.WriteByte(24);
			ProtocolParser.WriteUInt32(stream, instance.Rotatey);
		}
		if (instance.Rotatez != 0)
		{
			stream.WriteByte(32);
			ProtocolParser.WriteUInt32(stream, instance.Rotatez);
		}
		if (instance.Alternates != null)
		{
			Packet_CompositeShape[] elems = instance.Alternates;
			int elemCount = instance.AlternatesCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(42);
				SerializeWithSize(stream, elems[k]);
			}
		}
		if (instance.Overlays != null)
		{
			Packet_CompositeShape[] elems2 = instance.Overlays;
			int elemCount2 = instance.OverlaysCount;
			for (int i = 0; i < elems2.Length && i < elemCount2; i++)
			{
				stream.WriteByte(90);
				SerializeWithSize(stream, elems2[i]);
			}
		}
		if (instance.VoxelizeShape != 0)
		{
			stream.WriteByte(48);
			ProtocolParser.WriteUInt32(stream, instance.VoxelizeShape);
		}
		if (instance.SelectiveElements != null)
		{
			string[] elems3 = instance.SelectiveElements;
			int elemCount3 = instance.SelectiveElementsCount;
			for (int j = 0; j < elems3.Length && j < elemCount3; j++)
			{
				stream.WriteByte(58);
				ProtocolParser.WriteString(stream, elems3[j]);
			}
		}
		if (instance.IgnoreElements != null)
		{
			string[] elems4 = instance.IgnoreElements;
			int elemCount4 = instance.IgnoreElementsCount;
			for (int l = 0; l < elems4.Length && l < elemCount4; l++)
			{
				stream.WriteKey(17, 2);
				ProtocolParser.WriteString(stream, elems4[l]);
			}
		}
		if (instance.QuantityElements != 0)
		{
			stream.WriteByte(64);
			ProtocolParser.WriteUInt32(stream, instance.QuantityElements);
		}
		if (instance.QuantityElementsSet != 0)
		{
			stream.WriteByte(72);
			ProtocolParser.WriteUInt32(stream, instance.QuantityElementsSet);
		}
		if (instance.Format != 0)
		{
			stream.WriteByte(80);
			ProtocolParser.WriteUInt32(stream, instance.Format);
		}
		if (instance.Offsetx != 0)
		{
			stream.WriteByte(96);
			ProtocolParser.WriteUInt32(stream, instance.Offsetx);
		}
		if (instance.Offsety != 0)
		{
			stream.WriteByte(104);
			ProtocolParser.WriteUInt32(stream, instance.Offsety);
		}
		if (instance.Offsetz != 0)
		{
			stream.WriteByte(112);
			ProtocolParser.WriteUInt32(stream, instance.Offsetz);
		}
		if (instance.InsertBakedTextures)
		{
			stream.WriteByte(120);
			ProtocolParser.WriteBool(stream, instance.InsertBakedTextures);
		}
		if (instance.ScaleAdjust != 0)
		{
			stream.WriteKey(16, 0);
			ProtocolParser.WriteUInt32(stream, instance.ScaleAdjust);
		}
	}

	public static int GetSize(Packet_CompositeShape instance)
	{
		int size = 0;
		if (instance.Base != null)
		{
			size += ProtocolParser.GetSize(instance.Base) + 1;
		}
		if (instance.Rotatex != 0)
		{
			size += ProtocolParser.GetSize(instance.Rotatex) + 1;
		}
		if (instance.Rotatey != 0)
		{
			size += ProtocolParser.GetSize(instance.Rotatey) + 1;
		}
		if (instance.Rotatez != 0)
		{
			size += ProtocolParser.GetSize(instance.Rotatez) + 1;
		}
		if (instance.Alternates != null)
		{
			for (int k = 0; k < instance.AlternatesCount; k++)
			{
				int packetlength = GetSize(instance.Alternates[k]);
				size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
			}
		}
		if (instance.Overlays != null)
		{
			for (int i = 0; i < instance.OverlaysCount; i++)
			{
				int packetlength2 = GetSize(instance.Overlays[i]);
				size += packetlength2 + ProtocolParser.GetSize(packetlength2) + 1;
			}
		}
		if (instance.VoxelizeShape != 0)
		{
			size += ProtocolParser.GetSize(instance.VoxelizeShape) + 1;
		}
		if (instance.SelectiveElements != null)
		{
			for (int j = 0; j < instance.SelectiveElementsCount; j++)
			{
				string i7 = instance.SelectiveElements[j];
				size += ProtocolParser.GetSize(i7) + 1;
			}
		}
		if (instance.IgnoreElements != null)
		{
			for (int l = 0; l < instance.IgnoreElementsCount; l++)
			{
				string i17 = instance.IgnoreElements[l];
				size += ProtocolParser.GetSize(i17) + 2;
			}
		}
		if (instance.QuantityElements != 0)
		{
			size += ProtocolParser.GetSize(instance.QuantityElements) + 1;
		}
		if (instance.QuantityElementsSet != 0)
		{
			size += ProtocolParser.GetSize(instance.QuantityElementsSet) + 1;
		}
		if (instance.Format != 0)
		{
			size += ProtocolParser.GetSize(instance.Format) + 1;
		}
		if (instance.Offsetx != 0)
		{
			size += ProtocolParser.GetSize(instance.Offsetx) + 1;
		}
		if (instance.Offsety != 0)
		{
			size += ProtocolParser.GetSize(instance.Offsety) + 1;
		}
		if (instance.Offsetz != 0)
		{
			size += ProtocolParser.GetSize(instance.Offsetz) + 1;
		}
		if (instance.InsertBakedTextures)
		{
			size += 2;
		}
		if (instance.ScaleAdjust != 0)
		{
			size += ProtocolParser.GetSize(instance.ScaleAdjust) + 2;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_CompositeShape instance)
	{
		ProtocolParser.WriteUInt32_(stream, instance.size);
		int positionSaved = stream.Position();
		Serialize(stream, instance);
		int len = stream.Position() - positionSaved;
		if (len != instance.size)
		{
			throw new Exception("Sizing mismatch: " + instance.size + " != " + len);
		}
	}

	public static byte[] SerializeToBytes(Packet_CompositeShape instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_CompositeShape instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
