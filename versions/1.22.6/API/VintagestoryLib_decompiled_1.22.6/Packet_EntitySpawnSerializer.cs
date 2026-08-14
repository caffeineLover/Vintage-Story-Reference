using System;

public class Packet_EntitySpawnSerializer
{
	private const int field = 8;

	public static Packet_EntitySpawn DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_EntitySpawn instance = new Packet_EntitySpawn();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_EntitySpawn DeserializeBuffer(byte[] buffer, int length, Packet_EntitySpawn instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_EntitySpawn Deserialize(CitoMemoryStream stream, Packet_EntitySpawn instance)
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
				instance.EntityAdd(Packet_EntitySerializer.DeserializeLengthDelimitedNew(stream));
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

	public static Packet_EntitySpawn DeserializeLengthDelimited(CitoMemoryStream stream, Packet_EntitySpawn instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_EntitySpawn result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_EntitySpawn instance)
	{
		if (instance.Entity != null)
		{
			Packet_Entity[] elems = instance.Entity;
			int elemCount = instance.EntityCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(10);
				Packet_EntitySerializer.SerializeWithSize(stream, elems[k]);
			}
		}
	}

	public static int GetSize(Packet_EntitySpawn instance)
	{
		int size = 0;
		if (instance.Entity != null)
		{
			for (int k = 0; k < instance.EntityCount; k++)
			{
				int packetlength = Packet_EntitySerializer.GetSize(instance.Entity[k]);
				size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_EntitySpawn instance)
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

	public static byte[] SerializeToBytes(Packet_EntitySpawn instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_EntitySpawn instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
