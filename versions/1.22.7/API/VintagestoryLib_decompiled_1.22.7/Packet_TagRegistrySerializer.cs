using System;

public class Packet_TagRegistrySerializer
{
	private const int field = 8;

	public static Packet_TagRegistry DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_TagRegistry instance = new Packet_TagRegistry();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_TagRegistry DeserializeBuffer(byte[] buffer, int length, Packet_TagRegistry instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_TagRegistry Deserialize(CitoMemoryStream stream, Packet_TagRegistry instance)
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
				instance.RegistryName = ProtocolParser.ReadString(stream);
				break;
			case 18:
				instance.TagNamesAdd(ProtocolParser.ReadString(stream));
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

	public static Packet_TagRegistry DeserializeLengthDelimited(CitoMemoryStream stream, Packet_TagRegistry instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_TagRegistry result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_TagRegistry instance)
	{
		if (instance.RegistryName != null)
		{
			stream.WriteByte(10);
			ProtocolParser.WriteString(stream, instance.RegistryName);
		}
		if (instance.TagNames != null)
		{
			string[] elems = instance.TagNames;
			int elemCount = instance.TagNamesCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(18);
				ProtocolParser.WriteString(stream, elems[k]);
			}
		}
	}

	public static int GetSize(Packet_TagRegistry instance)
	{
		int size = 0;
		if (instance.RegistryName != null)
		{
			size += ProtocolParser.GetSize(instance.RegistryName) + 1;
		}
		if (instance.TagNames != null)
		{
			for (int k = 0; k < instance.TagNamesCount; k++)
			{
				string i2 = instance.TagNames[k];
				size += ProtocolParser.GetSize(i2) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_TagRegistry instance)
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

	public static byte[] SerializeToBytes(Packet_TagRegistry instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_TagRegistry instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
