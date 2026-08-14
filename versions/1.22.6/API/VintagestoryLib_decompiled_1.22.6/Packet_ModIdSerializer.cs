using System;

public class Packet_ModIdSerializer
{
	private const int field = 8;

	public static Packet_ModId DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_ModId instance = new Packet_ModId();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_ModId DeserializeBuffer(byte[] buffer, int length, Packet_ModId instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_ModId Deserialize(CitoMemoryStream stream, Packet_ModId instance)
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
				instance.Modid = ProtocolParser.ReadString(stream);
				break;
			case 18:
				instance.Name = ProtocolParser.ReadString(stream);
				break;
			case 26:
				instance.Version = ProtocolParser.ReadString(stream);
				break;
			case 34:
				instance.Networkversion = ProtocolParser.ReadString(stream);
				break;
			case 40:
				instance.RequiredOnClient = ProtocolParser.ReadBool(stream);
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

	public static Packet_ModId DeserializeLengthDelimited(CitoMemoryStream stream, Packet_ModId instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_ModId result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_ModId instance)
	{
		if (instance.Modid != null)
		{
			stream.WriteByte(10);
			ProtocolParser.WriteString(stream, instance.Modid);
		}
		if (instance.Name != null)
		{
			stream.WriteByte(18);
			ProtocolParser.WriteString(stream, instance.Name);
		}
		if (instance.Version != null)
		{
			stream.WriteByte(26);
			ProtocolParser.WriteString(stream, instance.Version);
		}
		if (instance.Networkversion != null)
		{
			stream.WriteByte(34);
			ProtocolParser.WriteString(stream, instance.Networkversion);
		}
		if (instance.RequiredOnClient)
		{
			stream.WriteByte(40);
			ProtocolParser.WriteBool(stream, instance.RequiredOnClient);
		}
	}

	public static int GetSize(Packet_ModId instance)
	{
		int size = 0;
		if (instance.Modid != null)
		{
			size += ProtocolParser.GetSize(instance.Modid) + 1;
		}
		if (instance.Name != null)
		{
			size += ProtocolParser.GetSize(instance.Name) + 1;
		}
		if (instance.Version != null)
		{
			size += ProtocolParser.GetSize(instance.Version) + 1;
		}
		if (instance.Networkversion != null)
		{
			size += ProtocolParser.GetSize(instance.Networkversion) + 1;
		}
		if (instance.RequiredOnClient)
		{
			size += 2;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_ModId instance)
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

	public static byte[] SerializeToBytes(Packet_ModId instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_ModId instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
