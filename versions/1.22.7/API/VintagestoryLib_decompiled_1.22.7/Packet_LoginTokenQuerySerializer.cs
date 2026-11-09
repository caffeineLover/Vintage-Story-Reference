using System;

public class Packet_LoginTokenQuerySerializer
{
	private const int field = 8;

	public static Packet_LoginTokenQuery DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_LoginTokenQuery instance = new Packet_LoginTokenQuery();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_LoginTokenQuery DeserializeBuffer(byte[] buffer, int length, Packet_LoginTokenQuery instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_LoginTokenQuery Deserialize(CitoMemoryStream stream, Packet_LoginTokenQuery instance)
	{
		instance.InitializeValues();
		while (true)
		{
			int keyInt = stream.ReadByte();
			if ((keyInt & 0x80) != 0)
			{
				keyInt = ProtocolParser.ReadKeyAsInt(keyInt, stream);
				if ((keyInt & 0x4000) != 0)
				{
					if (keyInt < 0)
					{
						break;
					}
					return null;
				}
			}
			if (keyInt == 0)
			{
				return null;
			}
			ProtocolParser.SkipKey(stream, Key.Create(keyInt));
		}
		return instance;
	}

	public static Packet_LoginTokenQuery DeserializeLengthDelimited(CitoMemoryStream stream, Packet_LoginTokenQuery instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_LoginTokenQuery result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_LoginTokenQuery instance)
	{
	}

	public static int GetSize(Packet_LoginTokenQuery instance)
	{
		return instance.size = 0;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_LoginTokenQuery instance)
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

	public static byte[] SerializeToBytes(Packet_LoginTokenQuery instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_LoginTokenQuery instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
