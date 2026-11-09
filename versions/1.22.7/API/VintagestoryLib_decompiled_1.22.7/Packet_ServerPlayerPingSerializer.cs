using System;

public class Packet_ServerPlayerPingSerializer
{
	private const int field = 8;

	public static Packet_ServerPlayerPing DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_ServerPlayerPing instance = new Packet_ServerPlayerPing();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_ServerPlayerPing DeserializeBuffer(byte[] buffer, int length, Packet_ServerPlayerPing instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_ServerPlayerPing Deserialize(CitoMemoryStream stream, Packet_ServerPlayerPing instance)
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
				instance.ClientIdsAdd(ProtocolParser.ReadUInt32(stream));
				break;
			case 16:
				instance.PingsAdd(ProtocolParser.ReadUInt32(stream));
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

	public static Packet_ServerPlayerPing DeserializeLengthDelimited(CitoMemoryStream stream, Packet_ServerPlayerPing instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_ServerPlayerPing result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_ServerPlayerPing instance)
	{
		if (instance.ClientIds != null)
		{
			int[] elems = instance.ClientIds;
			int elemCount = instance.ClientIdsCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(8);
				ProtocolParser.WriteUInt32(stream, elems[k]);
			}
		}
		if (instance.Pings != null)
		{
			int[] elems2 = instance.Pings;
			int elemCount2 = instance.PingsCount;
			for (int i = 0; i < elems2.Length && i < elemCount2; i++)
			{
				stream.WriteByte(16);
				ProtocolParser.WriteUInt32(stream, elems2[i]);
			}
		}
	}

	public static int GetSize(Packet_ServerPlayerPing instance)
	{
		int size = 0;
		if (instance.ClientIds != null)
		{
			for (int k = 0; k < instance.ClientIdsCount; k++)
			{
				int i1 = instance.ClientIds[k];
				size += ProtocolParser.GetSize(i1) + 1;
			}
		}
		if (instance.Pings != null)
		{
			for (int j = 0; j < instance.PingsCount; j++)
			{
				int i2 = instance.Pings[j];
				size += ProtocolParser.GetSize(i2) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_ServerPlayerPing instance)
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

	public static byte[] SerializeToBytes(Packet_ServerPlayerPing instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_ServerPlayerPing instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
