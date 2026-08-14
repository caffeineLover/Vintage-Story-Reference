using System;

public class Packet_PartialAttributeSerializer
{
	private const int field = 8;

	public static Packet_PartialAttribute DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_PartialAttribute instance = new Packet_PartialAttribute();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_PartialAttribute DeserializeBuffer(byte[] buffer, int length, Packet_PartialAttribute instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_PartialAttribute Deserialize(CitoMemoryStream stream, Packet_PartialAttribute instance)
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
				instance.Path = ProtocolParser.ReadString(stream);
				break;
			case 18:
				instance.Data = ProtocolParser.ReadBytes(stream);
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

	public static Packet_PartialAttribute DeserializeLengthDelimited(CitoMemoryStream stream, Packet_PartialAttribute instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_PartialAttribute result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_PartialAttribute instance)
	{
		if (instance.Path != null)
		{
			stream.WriteByte(10);
			ProtocolParser.WriteString(stream, instance.Path);
		}
		if (instance.Data != null)
		{
			stream.WriteByte(18);
			ProtocolParser.WriteBytes(stream, instance.Data);
		}
	}

	public static int GetSize(Packet_PartialAttribute instance)
	{
		int size = 0;
		if (instance.Path != null)
		{
			size += ProtocolParser.GetSize(instance.Path) + 1;
		}
		if (instance.Data != null)
		{
			size += ProtocolParser.GetSize(instance.Data) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_PartialAttribute instance)
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

	public static byte[] SerializeToBytes(Packet_PartialAttribute instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_PartialAttribute instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
