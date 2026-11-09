using System;

public class Packet_ServerChunksSerializer
{
	private const int field = 8;

	public static Packet_ServerChunks DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_ServerChunks instance = new Packet_ServerChunks();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_ServerChunks DeserializeBuffer(byte[] buffer, int length, Packet_ServerChunks instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_ServerChunks Deserialize(CitoMemoryStream stream, Packet_ServerChunks instance)
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
				instance.ChunksAdd(Packet_ServerChunkSerializer.DeserializeLengthDelimitedNew(stream));
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

	public static Packet_ServerChunks DeserializeLengthDelimited(CitoMemoryStream stream, Packet_ServerChunks instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_ServerChunks result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_ServerChunks instance)
	{
		if (instance.Chunks != null)
		{
			Packet_ServerChunk[] elems = instance.Chunks;
			int elemCount = instance.ChunksCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(10);
				Packet_ServerChunkSerializer.SerializeWithSize(stream, elems[k]);
			}
		}
	}

	public static int GetSize(Packet_ServerChunks instance)
	{
		int size = 0;
		if (instance.Chunks != null)
		{
			for (int k = 0; k < instance.ChunksCount; k++)
			{
				int packetlength = Packet_ServerChunkSerializer.GetSize(instance.Chunks[k]);
				size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_ServerChunks instance)
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

	public static byte[] SerializeToBytes(Packet_ServerChunks instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_ServerChunks instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
