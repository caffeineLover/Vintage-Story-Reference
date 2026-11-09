using System;

public class Packet_ServerLevelInitializeSerializer
{
	private const int field = 8;

	public static Packet_ServerLevelInitialize DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_ServerLevelInitialize instance = new Packet_ServerLevelInitialize();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_ServerLevelInitialize DeserializeBuffer(byte[] buffer, int length, Packet_ServerLevelInitialize instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_ServerLevelInitialize Deserialize(CitoMemoryStream stream, Packet_ServerLevelInitialize instance)
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
				instance.ServerChunkSize = ProtocolParser.ReadUInt32(stream);
				break;
			case 16:
				instance.ServerMapChunkSize = ProtocolParser.ReadUInt32(stream);
				break;
			case 24:
				instance.ServerMapRegionSize = ProtocolParser.ReadUInt32(stream);
				break;
			case 32:
				instance.MaxViewDistance = ProtocolParser.ReadUInt32(stream);
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

	public static Packet_ServerLevelInitialize DeserializeLengthDelimited(CitoMemoryStream stream, Packet_ServerLevelInitialize instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_ServerLevelInitialize result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_ServerLevelInitialize instance)
	{
		if (instance.ServerChunkSize != 0)
		{
			stream.WriteByte(8);
			ProtocolParser.WriteUInt32(stream, instance.ServerChunkSize);
		}
		if (instance.ServerMapChunkSize != 0)
		{
			stream.WriteByte(16);
			ProtocolParser.WriteUInt32(stream, instance.ServerMapChunkSize);
		}
		if (instance.ServerMapRegionSize != 0)
		{
			stream.WriteByte(24);
			ProtocolParser.WriteUInt32(stream, instance.ServerMapRegionSize);
		}
		if (instance.MaxViewDistance != 0)
		{
			stream.WriteByte(32);
			ProtocolParser.WriteUInt32(stream, instance.MaxViewDistance);
		}
	}

	public static int GetSize(Packet_ServerLevelInitialize instance)
	{
		int size = 0;
		if (instance.ServerChunkSize != 0)
		{
			size += ProtocolParser.GetSize(instance.ServerChunkSize) + 1;
		}
		if (instance.ServerMapChunkSize != 0)
		{
			size += ProtocolParser.GetSize(instance.ServerMapChunkSize) + 1;
		}
		if (instance.ServerMapRegionSize != 0)
		{
			size += ProtocolParser.GetSize(instance.ServerMapRegionSize) + 1;
		}
		if (instance.MaxViewDistance != 0)
		{
			size += ProtocolParser.GetSize(instance.MaxViewDistance) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_ServerLevelInitialize instance)
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

	public static byte[] SerializeToBytes(Packet_ServerLevelInitialize instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_ServerLevelInitialize instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
