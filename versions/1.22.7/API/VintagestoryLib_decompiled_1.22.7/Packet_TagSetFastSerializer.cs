using System;

public class Packet_TagSetFastSerializer
{
	private const int field = 8;

	public static Packet_TagSetFast DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_TagSetFast instance = new Packet_TagSetFast();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_TagSetFast DeserializeBuffer(byte[] buffer, int length, Packet_TagSetFast instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_TagSetFast Deserialize(CitoMemoryStream stream, Packet_TagSetFast instance)
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
				instance.Part1 = ProtocolParser.ReadUInt64(stream);
				break;
			case 16:
				instance.Part2 = ProtocolParser.ReadUInt64(stream);
				break;
			case 24:
				instance.Part3 = ProtocolParser.ReadUInt64(stream);
				break;
			case 32:
				instance.Part4 = ProtocolParser.ReadUInt64(stream);
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

	public static Packet_TagSetFast DeserializeLengthDelimited(CitoMemoryStream stream, Packet_TagSetFast instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_TagSetFast result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_TagSetFast instance)
	{
		if (instance.Part1 != 0L)
		{
			stream.WriteByte(8);
			ProtocolParser.WriteUInt64(stream, instance.Part1);
		}
		if (instance.Part2 != 0L)
		{
			stream.WriteByte(16);
			ProtocolParser.WriteUInt64(stream, instance.Part2);
		}
		if (instance.Part3 != 0L)
		{
			stream.WriteByte(24);
			ProtocolParser.WriteUInt64(stream, instance.Part3);
		}
		if (instance.Part4 != 0L)
		{
			stream.WriteByte(32);
			ProtocolParser.WriteUInt64(stream, instance.Part4);
		}
	}

	public static int GetSize(Packet_TagSetFast instance)
	{
		int size = 0;
		if (instance.Part1 != 0L)
		{
			size += ProtocolParser.GetSize(instance.Part1) + 1;
		}
		if (instance.Part2 != 0L)
		{
			size += ProtocolParser.GetSize(instance.Part2) + 1;
		}
		if (instance.Part3 != 0L)
		{
			size += ProtocolParser.GetSize(instance.Part3) + 1;
		}
		if (instance.Part4 != 0L)
		{
			size += ProtocolParser.GetSize(instance.Part4) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_TagSetFast instance)
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

	public static byte[] SerializeToBytes(Packet_TagSetFast instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_TagSetFast instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
