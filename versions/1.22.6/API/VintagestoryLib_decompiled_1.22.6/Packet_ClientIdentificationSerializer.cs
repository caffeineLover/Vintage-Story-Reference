using System;

public class Packet_ClientIdentificationSerializer
{
	private const int field = 8;

	public static Packet_ClientIdentification DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_ClientIdentification instance = new Packet_ClientIdentification();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_ClientIdentification DeserializeBuffer(byte[] buffer, int length, Packet_ClientIdentification instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_ClientIdentification Deserialize(CitoMemoryStream stream, Packet_ClientIdentification instance)
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
				instance.MdProtocolVersion = ProtocolParser.ReadString(stream);
				break;
			case 18:
				instance.Playername = ProtocolParser.ReadString(stream);
				break;
			case 26:
				instance.MpToken = ProtocolParser.ReadString(stream);
				break;
			case 34:
				instance.ServerPassword = ProtocolParser.ReadString(stream);
				break;
			case 50:
				instance.PlayerUID = ProtocolParser.ReadString(stream);
				break;
			case 56:
				instance.ViewDistance = ProtocolParser.ReadUInt32(stream);
				break;
			case 64:
				instance.RenderMetaBlocks = ProtocolParser.ReadUInt32(stream);
				break;
			case 74:
				instance.NetworkVersion = ProtocolParser.ReadString(stream);
				break;
			case 82:
				instance.ShortGameVersion = ProtocolParser.ReadString(stream);
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

	public static Packet_ClientIdentification DeserializeLengthDelimited(CitoMemoryStream stream, Packet_ClientIdentification instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_ClientIdentification result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_ClientIdentification instance)
	{
		if (instance.MdProtocolVersion != null)
		{
			stream.WriteByte(10);
			ProtocolParser.WriteString(stream, instance.MdProtocolVersion);
		}
		if (instance.Playername != null)
		{
			stream.WriteByte(18);
			ProtocolParser.WriteString(stream, instance.Playername);
		}
		if (instance.MpToken != null)
		{
			stream.WriteByte(26);
			ProtocolParser.WriteString(stream, instance.MpToken);
		}
		if (instance.ServerPassword != null)
		{
			stream.WriteByte(34);
			ProtocolParser.WriteString(stream, instance.ServerPassword);
		}
		if (instance.PlayerUID != null)
		{
			stream.WriteByte(50);
			ProtocolParser.WriteString(stream, instance.PlayerUID);
		}
		if (instance.ViewDistance != 0)
		{
			stream.WriteByte(56);
			ProtocolParser.WriteUInt32(stream, instance.ViewDistance);
		}
		if (instance.RenderMetaBlocks != 0)
		{
			stream.WriteByte(64);
			ProtocolParser.WriteUInt32(stream, instance.RenderMetaBlocks);
		}
		if (instance.NetworkVersion != null)
		{
			stream.WriteByte(74);
			ProtocolParser.WriteString(stream, instance.NetworkVersion);
		}
		if (instance.ShortGameVersion != null)
		{
			stream.WriteByte(82);
			ProtocolParser.WriteString(stream, instance.ShortGameVersion);
		}
	}

	public static int GetSize(Packet_ClientIdentification instance)
	{
		int size = 0;
		if (instance.MdProtocolVersion != null)
		{
			size += ProtocolParser.GetSize(instance.MdProtocolVersion) + 1;
		}
		if (instance.Playername != null)
		{
			size += ProtocolParser.GetSize(instance.Playername) + 1;
		}
		if (instance.MpToken != null)
		{
			size += ProtocolParser.GetSize(instance.MpToken) + 1;
		}
		if (instance.ServerPassword != null)
		{
			size += ProtocolParser.GetSize(instance.ServerPassword) + 1;
		}
		if (instance.PlayerUID != null)
		{
			size += ProtocolParser.GetSize(instance.PlayerUID) + 1;
		}
		if (instance.ViewDistance != 0)
		{
			size += ProtocolParser.GetSize(instance.ViewDistance) + 1;
		}
		if (instance.RenderMetaBlocks != 0)
		{
			size += ProtocolParser.GetSize(instance.RenderMetaBlocks) + 1;
		}
		if (instance.NetworkVersion != null)
		{
			size += ProtocolParser.GetSize(instance.NetworkVersion) + 1;
		}
		if (instance.ShortGameVersion != null)
		{
			size += ProtocolParser.GetSize(instance.ShortGameVersion) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_ClientIdentification instance)
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

	public static byte[] SerializeToBytes(Packet_ClientIdentification instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_ClientIdentification instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
