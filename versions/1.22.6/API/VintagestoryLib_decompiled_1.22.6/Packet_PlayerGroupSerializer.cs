using System;

public class Packet_PlayerGroupSerializer
{
	private const int field = 8;

	public static Packet_PlayerGroup DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_PlayerGroup instance = new Packet_PlayerGroup();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_PlayerGroup DeserializeBuffer(byte[] buffer, int length, Packet_PlayerGroup instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_PlayerGroup Deserialize(CitoMemoryStream stream, Packet_PlayerGroup instance)
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
			case 8:
				instance.Uid = ProtocolParser.ReadUInt32(stream);
				break;
			case 18:
				instance.Owneruid = ProtocolParser.ReadString(stream);
				break;
			case 26:
				instance.Name = ProtocolParser.ReadString(stream);
				break;
			case 34:
				instance.ChathistoryAdd(Packet_ChatLineSerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 40:
				instance.Createdbyprivatemessage = ProtocolParser.ReadUInt32(stream);
				break;
			case 48:
				instance.Membership = ProtocolParser.ReadUInt32(stream);
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

	public static Packet_PlayerGroup DeserializeLengthDelimited(CitoMemoryStream stream, Packet_PlayerGroup instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_PlayerGroup result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_PlayerGroup instance)
	{
		if (instance.Uid != 0)
		{
			stream.WriteByte(8);
			ProtocolParser.WriteUInt32(stream, instance.Uid);
		}
		if (instance.Owneruid != null)
		{
			stream.WriteByte(18);
			ProtocolParser.WriteString(stream, instance.Owneruid);
		}
		if (instance.Name != null)
		{
			stream.WriteByte(26);
			ProtocolParser.WriteString(stream, instance.Name);
		}
		if (instance.Chathistory != null)
		{
			Packet_ChatLine[] elems = instance.Chathistory;
			int elemCount = instance.ChathistoryCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(34);
				Packet_ChatLineSerializer.SerializeWithSize(stream, elems[k]);
			}
		}
		if (instance.Createdbyprivatemessage != 0)
		{
			stream.WriteByte(40);
			ProtocolParser.WriteUInt32(stream, instance.Createdbyprivatemessage);
		}
		if (instance.Membership != 0)
		{
			stream.WriteByte(48);
			ProtocolParser.WriteUInt32(stream, instance.Membership);
		}
	}

	public static int GetSize(Packet_PlayerGroup instance)
	{
		int size = 0;
		if (instance.Uid != 0)
		{
			size += ProtocolParser.GetSize(instance.Uid) + 1;
		}
		if (instance.Owneruid != null)
		{
			size += ProtocolParser.GetSize(instance.Owneruid) + 1;
		}
		if (instance.Name != null)
		{
			size += ProtocolParser.GetSize(instance.Name) + 1;
		}
		if (instance.Chathistory != null)
		{
			for (int k = 0; k < instance.ChathistoryCount; k++)
			{
				int packetlength = Packet_ChatLineSerializer.GetSize(instance.Chathistory[k]);
				size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
			}
		}
		if (instance.Createdbyprivatemessage != 0)
		{
			size += ProtocolParser.GetSize(instance.Createdbyprivatemessage) + 1;
		}
		if (instance.Membership != 0)
		{
			size += ProtocolParser.GetSize(instance.Membership) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_PlayerGroup instance)
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

	public static byte[] SerializeToBytes(Packet_PlayerGroup instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_PlayerGroup instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
