using System;

public class Packet_BulkEntityDebugAttributesSerializer
{
	private const int field = 8;

	public static Packet_BulkEntityDebugAttributes DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_BulkEntityDebugAttributes instance = new Packet_BulkEntityDebugAttributes();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_BulkEntityDebugAttributes DeserializeBuffer(byte[] buffer, int length, Packet_BulkEntityDebugAttributes instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_BulkEntityDebugAttributes Deserialize(CitoMemoryStream stream, Packet_BulkEntityDebugAttributes instance)
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
				instance.FullUpdatesAdd(Packet_EntityAttributesSerializer.DeserializeLengthDelimitedNew(stream));
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

	public static Packet_BulkEntityDebugAttributes DeserializeLengthDelimited(CitoMemoryStream stream, Packet_BulkEntityDebugAttributes instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_BulkEntityDebugAttributes result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_BulkEntityDebugAttributes instance)
	{
		if (instance.FullUpdates != null)
		{
			Packet_EntityAttributes[] elems = instance.FullUpdates;
			int elemCount = instance.FullUpdatesCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(10);
				Packet_EntityAttributesSerializer.SerializeWithSize(stream, elems[k]);
			}
		}
	}

	public static int GetSize(Packet_BulkEntityDebugAttributes instance)
	{
		int size = 0;
		if (instance.FullUpdates != null)
		{
			for (int k = 0; k < instance.FullUpdatesCount; k++)
			{
				int packetlength = Packet_EntityAttributesSerializer.GetSize(instance.FullUpdates[k]);
				size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_BulkEntityDebugAttributes instance)
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

	public static byte[] SerializeToBytes(Packet_BulkEntityDebugAttributes instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_BulkEntityDebugAttributes instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
