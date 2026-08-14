using System;

public class Packet_SoundAttributesSerializer
{
	private const int field = 8;

	public static Packet_SoundAttributes DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_SoundAttributes instance = new Packet_SoundAttributes();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_SoundAttributes DeserializeBuffer(byte[] buffer, int length, Packet_SoundAttributes instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_SoundAttributes Deserialize(CitoMemoryStream stream, Packet_SoundAttributes instance)
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
				instance.Location = ProtocolParser.ReadString(stream);
				break;
			case 18:
				if (instance.Pitch == null)
				{
					instance.Pitch = Packet_NatFloatSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_NatFloatSerializer.DeserializeLengthDelimited(stream, instance.Pitch);
				}
				break;
			case 26:
				if (instance.Volume == null)
				{
					instance.Volume = Packet_NatFloatSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_NatFloatSerializer.DeserializeLengthDelimited(stream, instance.Volume);
				}
				break;
			case 32:
				instance.Type = ProtocolParser.ReadUInt32(stream);
				break;
			case 40:
				instance.Range = ProtocolParser.ReadUInt32(stream);
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

	public static Packet_SoundAttributes DeserializeLengthDelimited(CitoMemoryStream stream, Packet_SoundAttributes instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_SoundAttributes result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_SoundAttributes instance)
	{
		if (instance.Location != null)
		{
			stream.WriteByte(10);
			ProtocolParser.WriteString(stream, instance.Location);
		}
		if (instance.Pitch != null)
		{
			stream.WriteByte(18);
			Packet_NatFloatSerializer.SerializeWithSize(stream, instance.Pitch);
		}
		if (instance.Volume != null)
		{
			stream.WriteByte(26);
			Packet_NatFloatSerializer.SerializeWithSize(stream, instance.Volume);
		}
		if (instance.Type != 0)
		{
			stream.WriteByte(32);
			ProtocolParser.WriteUInt32(stream, instance.Type);
		}
		if (instance.Range != 0)
		{
			stream.WriteByte(40);
			ProtocolParser.WriteUInt32(stream, instance.Range);
		}
	}

	public static int GetSize(Packet_SoundAttributes instance)
	{
		int size = 0;
		if (instance.Location != null)
		{
			size += ProtocolParser.GetSize(instance.Location) + 1;
		}
		if (instance.Pitch != null)
		{
			int packetlength = Packet_NatFloatSerializer.GetSize(instance.Pitch);
			size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
		}
		if (instance.Volume != null)
		{
			int packetlength2 = Packet_NatFloatSerializer.GetSize(instance.Volume);
			size += packetlength2 + ProtocolParser.GetSize(packetlength2) + 1;
		}
		if (instance.Type != 0)
		{
			size += ProtocolParser.GetSize(instance.Type) + 1;
		}
		if (instance.Range != 0)
		{
			size += ProtocolParser.GetSize(instance.Range) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_SoundAttributes instance)
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

	public static byte[] SerializeToBytes(Packet_SoundAttributes instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_SoundAttributes instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
