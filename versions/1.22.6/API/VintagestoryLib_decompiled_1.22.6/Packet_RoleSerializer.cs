using System;

public class Packet_RoleSerializer
{
	private const int field = 8;

	public static Packet_Role DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_Role instance = new Packet_Role();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_Role DeserializeBuffer(byte[] buffer, int length, Packet_Role instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_Role Deserialize(CitoMemoryStream stream, Packet_Role instance)
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
			case 16:
				instance.PrivilegeLevel = ProtocolParser.ReadUInt32(stream);
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

	public static Packet_Role DeserializeLengthDelimited(CitoMemoryStream stream, Packet_Role instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_Role result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_Role instance)
	{
		if (instance.Code != null)
		{
			stream.WriteByte(10);
			ProtocolParser.WriteString(stream, instance.Code);
		}
		if (instance.PrivilegeLevel != 0)
		{
			stream.WriteByte(16);
			ProtocolParser.WriteUInt32(stream, instance.PrivilegeLevel);
		}
	}

	public static int GetSize(Packet_Role instance)
	{
		int size = 0;
		if (instance.Code != null)
		{
			size += ProtocolParser.GetSize(instance.Code) + 1;
		}
		if (instance.PrivilegeLevel != 0)
		{
			size += ProtocolParser.GetSize(instance.PrivilegeLevel) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_Role instance)
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

	public static byte[] SerializeToBytes(Packet_Role instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_Role instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
