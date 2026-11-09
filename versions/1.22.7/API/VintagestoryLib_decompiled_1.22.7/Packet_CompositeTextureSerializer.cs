using System;

public class Packet_CompositeTextureSerializer
{
	private const int field = 8;

	public static Packet_CompositeTexture DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_CompositeTexture instance = new Packet_CompositeTexture();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_CompositeTexture DeserializeBuffer(byte[] buffer, int length, Packet_CompositeTexture instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_CompositeTexture Deserialize(CitoMemoryStream stream, Packet_CompositeTexture instance)
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
			case 18:
				instance.OverlaysAdd(Packet_BlendedOverlayTextureSerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 26:
				instance.AlternatesAdd(DeserializeLengthDelimitedNew(stream));
				break;
			case 32:
				instance.Rotation = ProtocolParser.ReadUInt32(stream);
				break;
			case 40:
				instance.Alpha = ProtocolParser.ReadUInt32(stream);
				break;
			case 50:
				instance.TilesAdd(DeserializeLengthDelimitedNew(stream));
				break;
			case 56:
				instance.TilesWidth = ProtocolParser.ReadUInt32(stream);
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

	public static Packet_CompositeTexture DeserializeLengthDelimited(CitoMemoryStream stream, Packet_CompositeTexture instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_CompositeTexture result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_CompositeTexture instance)
	{
		if (instance.Base != null)
		{
			stream.WriteByte(10);
			ProtocolParser.WriteString(stream, instance.Base);
		}
		if (instance.Overlays != null)
		{
			Packet_BlendedOverlayTexture[] elems = instance.Overlays;
			int elemCount = instance.OverlaysCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(18);
				Packet_BlendedOverlayTextureSerializer.SerializeWithSize(stream, elems[k]);
			}
		}
		if (instance.Alternates != null)
		{
			Packet_CompositeTexture[] elems2 = instance.Alternates;
			int elemCount2 = instance.AlternatesCount;
			for (int i = 0; i < elems2.Length && i < elemCount2; i++)
			{
				stream.WriteByte(26);
				SerializeWithSize(stream, elems2[i]);
			}
		}
		if (instance.Rotation != 0)
		{
			stream.WriteByte(32);
			ProtocolParser.WriteUInt32(stream, instance.Rotation);
		}
		if (instance.Alpha != 0)
		{
			stream.WriteByte(40);
			ProtocolParser.WriteUInt32(stream, instance.Alpha);
		}
		if (instance.Tiles != null)
		{
			Packet_CompositeTexture[] elems3 = instance.Tiles;
			int elemCount3 = instance.TilesCount;
			for (int j = 0; j < elems3.Length && j < elemCount3; j++)
			{
				stream.WriteByte(50);
				SerializeWithSize(stream, elems3[j]);
			}
		}
		if (instance.TilesWidth != 0)
		{
			stream.WriteByte(56);
			ProtocolParser.WriteUInt32(stream, instance.TilesWidth);
		}
	}

	public static int GetSize(Packet_CompositeTexture instance)
	{
		int size = 0;
		if (instance.Base != null)
		{
			size += ProtocolParser.GetSize(instance.Base) + 1;
		}
		if (instance.Overlays != null)
		{
			for (int k = 0; k < instance.OverlaysCount; k++)
			{
				int packetlength = Packet_BlendedOverlayTextureSerializer.GetSize(instance.Overlays[k]);
				size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
			}
		}
		if (instance.Alternates != null)
		{
			for (int i = 0; i < instance.AlternatesCount; i++)
			{
				int packetlength2 = GetSize(instance.Alternates[i]);
				size += packetlength2 + ProtocolParser.GetSize(packetlength2) + 1;
			}
		}
		if (instance.Rotation != 0)
		{
			size += ProtocolParser.GetSize(instance.Rotation) + 1;
		}
		if (instance.Alpha != 0)
		{
			size += ProtocolParser.GetSize(instance.Alpha) + 1;
		}
		if (instance.Tiles != null)
		{
			for (int j = 0; j < instance.TilesCount; j++)
			{
				int packetlength3 = GetSize(instance.Tiles[j]);
				size += packetlength3 + ProtocolParser.GetSize(packetlength3) + 1;
			}
		}
		if (instance.TilesWidth != 0)
		{
			size += ProtocolParser.GetSize(instance.TilesWidth) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_CompositeTexture instance)
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

	public static byte[] SerializeToBytes(Packet_CompositeTexture instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_CompositeTexture instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
