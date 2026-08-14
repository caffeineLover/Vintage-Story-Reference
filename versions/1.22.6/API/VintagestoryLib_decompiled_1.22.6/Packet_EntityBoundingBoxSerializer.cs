using System;

public class Packet_EntityBoundingBoxSerializer
{
	private const int field = 8;

	public static Packet_EntityBoundingBox DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_EntityBoundingBox instance = new Packet_EntityBoundingBox();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_EntityBoundingBox DeserializeBuffer(byte[] buffer, int length, Packet_EntityBoundingBox instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_EntityBoundingBox Deserialize(CitoMemoryStream stream, Packet_EntityBoundingBox instance)
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
			case 8:
				instance.SizeX = ProtocolParser.ReadUInt32(stream);
				break;
			case 16:
				instance.SizeY = ProtocolParser.ReadUInt32(stream);
				break;
			case 24:
				instance.SizeZ = ProtocolParser.ReadUInt32(stream);
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

	public static Packet_EntityBoundingBox DeserializeLengthDelimited(CitoMemoryStream stream, Packet_EntityBoundingBox instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_EntityBoundingBox result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_EntityBoundingBox instance)
	{
		if (instance.SizeX != 0)
		{
			stream.WriteByte(8);
			ProtocolParser.WriteUInt32(stream, instance.SizeX);
		}
		if (instance.SizeY != 0)
		{
			stream.WriteByte(16);
			ProtocolParser.WriteUInt32(stream, instance.SizeY);
		}
		if (instance.SizeZ != 0)
		{
			stream.WriteByte(24);
			ProtocolParser.WriteUInt32(stream, instance.SizeZ);
		}
	}

	public static int GetSize(Packet_EntityBoundingBox instance)
	{
		int size = 0;
		if (instance.SizeX != 0)
		{
			size += ProtocolParser.GetSize(instance.SizeX) + 1;
		}
		if (instance.SizeY != 0)
		{
			size += ProtocolParser.GetSize(instance.SizeY) + 1;
		}
		if (instance.SizeZ != 0)
		{
			size += ProtocolParser.GetSize(instance.SizeZ) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_EntityBoundingBox instance)
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

	public static byte[] SerializeToBytes(Packet_EntityBoundingBox instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_EntityBoundingBox instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
