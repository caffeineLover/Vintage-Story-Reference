using System;

public class Packet_ServerDisconnectPlayerSerializer
{
	private const int field = 8;

	public static Packet_ServerDisconnectPlayer DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_ServerDisconnectPlayer instance = new Packet_ServerDisconnectPlayer();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_ServerDisconnectPlayer DeserializeBuffer(byte[] buffer, int length, Packet_ServerDisconnectPlayer instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_ServerDisconnectPlayer Deserialize(CitoMemoryStream stream, Packet_ServerDisconnectPlayer instance)
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
				instance.DisconnectReason = ProtocolParser.ReadString(stream);
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

	public static Packet_ServerDisconnectPlayer DeserializeLengthDelimited(CitoMemoryStream stream, Packet_ServerDisconnectPlayer instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_ServerDisconnectPlayer result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_ServerDisconnectPlayer instance)
	{
		if (instance.DisconnectReason != null)
		{
			stream.WriteByte(10);
			ProtocolParser.WriteString(stream, instance.DisconnectReason);
		}
	}

	public static int GetSize(Packet_ServerDisconnectPlayer instance)
	{
		int size = 0;
		if (instance.DisconnectReason != null)
		{
			size += ProtocolParser.GetSize(instance.DisconnectReason) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_ServerDisconnectPlayer instance)
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

	public static byte[] SerializeToBytes(Packet_ServerDisconnectPlayer instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_ServerDisconnectPlayer instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
