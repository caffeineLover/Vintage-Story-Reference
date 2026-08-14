using System;

public class Packet_BlockEntitiesSerializer
{
	private const int field = 8;

	public static Packet_BlockEntities DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_BlockEntities instance = new Packet_BlockEntities();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_BlockEntities DeserializeBuffer(byte[] buffer, int length, Packet_BlockEntities instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_BlockEntities Deserialize(CitoMemoryStream stream, Packet_BlockEntities instance)
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
				instance.BlockEntititesAdd(Packet_BlockEntitySerializer.DeserializeLengthDelimitedNew(stream));
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

	public static Packet_BlockEntities DeserializeLengthDelimited(CitoMemoryStream stream, Packet_BlockEntities instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_BlockEntities result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_BlockEntities instance)
	{
		if (instance.BlockEntitites != null)
		{
			Packet_BlockEntity[] elems = instance.BlockEntitites;
			int elemCount = instance.BlockEntititesCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(10);
				Packet_BlockEntitySerializer.SerializeWithSize(stream, elems[k]);
			}
		}
	}

	public static int GetSize(Packet_BlockEntities instance)
	{
		int size = 0;
		if (instance.BlockEntitites != null)
		{
			for (int k = 0; k < instance.BlockEntititesCount; k++)
			{
				int packetlength = Packet_BlockEntitySerializer.GetSize(instance.BlockEntitites[k]);
				size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_BlockEntities instance)
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

	public static byte[] SerializeToBytes(Packet_BlockEntities instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_BlockEntities instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
