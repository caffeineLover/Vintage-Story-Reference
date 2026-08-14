using System;

public class Packet_ServerQueryAnswerSerializer
{
	private const int field = 8;

	public static Packet_ServerQueryAnswer DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_ServerQueryAnswer instance = new Packet_ServerQueryAnswer();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_ServerQueryAnswer DeserializeBuffer(byte[] buffer, int length, Packet_ServerQueryAnswer instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_ServerQueryAnswer Deserialize(CitoMemoryStream stream, Packet_ServerQueryAnswer instance)
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
				instance.Name = ProtocolParser.ReadString(stream);
				break;
			case 18:
				instance.MOTD = ProtocolParser.ReadString(stream);
				break;
			case 24:
				instance.PlayerCount = ProtocolParser.ReadUInt32(stream);
				break;
			case 32:
				instance.MaxPlayers = ProtocolParser.ReadUInt32(stream);
				break;
			case 42:
				instance.GameMode = ProtocolParser.ReadString(stream);
				break;
			case 48:
				instance.Password = ProtocolParser.ReadBool(stream);
				break;
			case 58:
				instance.ServerVersion = ProtocolParser.ReadString(stream);
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

	public static Packet_ServerQueryAnswer DeserializeLengthDelimited(CitoMemoryStream stream, Packet_ServerQueryAnswer instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_ServerQueryAnswer result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_ServerQueryAnswer instance)
	{
		if (instance.Name != null)
		{
			stream.WriteByte(10);
			ProtocolParser.WriteString(stream, instance.Name);
		}
		if (instance.MOTD != null)
		{
			stream.WriteByte(18);
			ProtocolParser.WriteString(stream, instance.MOTD);
		}
		if (instance.PlayerCount != 0)
		{
			stream.WriteByte(24);
			ProtocolParser.WriteUInt32(stream, instance.PlayerCount);
		}
		if (instance.MaxPlayers != 0)
		{
			stream.WriteByte(32);
			ProtocolParser.WriteUInt32(stream, instance.MaxPlayers);
		}
		if (instance.GameMode != null)
		{
			stream.WriteByte(42);
			ProtocolParser.WriteString(stream, instance.GameMode);
		}
		if (instance.Password)
		{
			stream.WriteByte(48);
			ProtocolParser.WriteBool(stream, instance.Password);
		}
		if (instance.ServerVersion != null)
		{
			stream.WriteByte(58);
			ProtocolParser.WriteString(stream, instance.ServerVersion);
		}
	}

	public static int GetSize(Packet_ServerQueryAnswer instance)
	{
		int size = 0;
		if (instance.Name != null)
		{
			size += ProtocolParser.GetSize(instance.Name) + 1;
		}
		if (instance.MOTD != null)
		{
			size += ProtocolParser.GetSize(instance.MOTD) + 1;
		}
		if (instance.PlayerCount != 0)
		{
			size += ProtocolParser.GetSize(instance.PlayerCount) + 1;
		}
		if (instance.MaxPlayers != 0)
		{
			size += ProtocolParser.GetSize(instance.MaxPlayers) + 1;
		}
		if (instance.GameMode != null)
		{
			size += ProtocolParser.GetSize(instance.GameMode) + 1;
		}
		if (instance.Password)
		{
			size += 2;
		}
		if (instance.ServerVersion != null)
		{
			size += ProtocolParser.GetSize(instance.ServerVersion) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_ServerQueryAnswer instance)
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

	public static byte[] SerializeToBytes(Packet_ServerQueryAnswer instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_ServerQueryAnswer instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
