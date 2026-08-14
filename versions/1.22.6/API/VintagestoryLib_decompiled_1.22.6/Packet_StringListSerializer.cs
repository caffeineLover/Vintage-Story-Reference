using System;

public class Packet_StringListSerializer
{
	private const int field = 8;

	public static Packet_StringList DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_StringList instance = new Packet_StringList();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_StringList DeserializeBuffer(byte[] buffer, int length, Packet_StringList instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_StringList Deserialize(CitoMemoryStream stream, Packet_StringList instance)
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
				instance.ItemsAdd(ProtocolParser.ReadString(stream));
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

	public static Packet_StringList DeserializeLengthDelimited(CitoMemoryStream stream, Packet_StringList instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_StringList result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_StringList instance)
	{
		if (instance.Items != null)
		{
			string[] elems = instance.Items;
			int elemCount = instance.ItemsCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(10);
				ProtocolParser.WriteString(stream, elems[k]);
			}
		}
	}

	public static int GetSize(Packet_StringList instance)
	{
		int size = 0;
		if (instance.Items != null)
		{
			for (int k = 0; k < instance.ItemsCount; k++)
			{
				string i1 = instance.Items[k];
				size += ProtocolParser.GetSize(i1) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_StringList instance)
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

	public static byte[] SerializeToBytes(Packet_StringList instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_StringList instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
