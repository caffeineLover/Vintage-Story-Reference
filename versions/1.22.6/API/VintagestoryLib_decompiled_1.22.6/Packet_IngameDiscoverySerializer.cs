using System;

public class Packet_IngameDiscoverySerializer
{
	private const int field = 8;

	public static Packet_IngameDiscovery DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_IngameDiscovery instance = new Packet_IngameDiscovery();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_IngameDiscovery DeserializeBuffer(byte[] buffer, int length, Packet_IngameDiscovery instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_IngameDiscovery Deserialize(CitoMemoryStream stream, Packet_IngameDiscovery instance)
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
				instance.Code = ProtocolParser.ReadString(stream);
				break;
			case 18:
				instance.Message = ProtocolParser.ReadString(stream);
				break;
			case 26:
				instance.LangParamsAdd(ProtocolParser.ReadString(stream));
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

	public static Packet_IngameDiscovery DeserializeLengthDelimited(CitoMemoryStream stream, Packet_IngameDiscovery instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_IngameDiscovery result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_IngameDiscovery instance)
	{
		if (instance.Code != null)
		{
			stream.WriteByte(10);
			ProtocolParser.WriteString(stream, instance.Code);
		}
		if (instance.Message != null)
		{
			stream.WriteByte(18);
			ProtocolParser.WriteString(stream, instance.Message);
		}
		if (instance.LangParams != null)
		{
			string[] elems = instance.LangParams;
			int elemCount = instance.LangParamsCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(26);
				ProtocolParser.WriteString(stream, elems[k]);
			}
		}
	}

	public static int GetSize(Packet_IngameDiscovery instance)
	{
		int size = 0;
		if (instance.Code != null)
		{
			size += ProtocolParser.GetSize(instance.Code) + 1;
		}
		if (instance.Message != null)
		{
			size += ProtocolParser.GetSize(instance.Message) + 1;
		}
		if (instance.LangParams != null)
		{
			for (int k = 0; k < instance.LangParamsCount; k++)
			{
				string i3 = instance.LangParams[k];
				size += ProtocolParser.GetSize(i3) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_IngameDiscovery instance)
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

	public static byte[] SerializeToBytes(Packet_IngameDiscovery instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_IngameDiscovery instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
