using System;

public class Packet_BulkEntityAttributesSerializer
{
	private const int field = 8;

	public static Packet_BulkEntityAttributes DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_BulkEntityAttributes instance = new Packet_BulkEntityAttributes();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_BulkEntityAttributes DeserializeBuffer(byte[] buffer, int length, Packet_BulkEntityAttributes instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_BulkEntityAttributes Deserialize(CitoMemoryStream stream, Packet_BulkEntityAttributes instance)
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
			case 18:
				instance.PartialUpdatesAdd(Packet_EntityAttributeUpdateSerializer.DeserializeLengthDelimitedNew(stream));
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

	public static Packet_BulkEntityAttributes DeserializeLengthDelimited(CitoMemoryStream stream, Packet_BulkEntityAttributes instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_BulkEntityAttributes result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_BulkEntityAttributes instance)
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
		if (instance.PartialUpdates != null)
		{
			Packet_EntityAttributeUpdate[] elems2 = instance.PartialUpdates;
			int elemCount2 = instance.PartialUpdatesCount;
			for (int i = 0; i < elems2.Length && i < elemCount2; i++)
			{
				stream.WriteByte(18);
				Packet_EntityAttributeUpdateSerializer.SerializeWithSize(stream, elems2[i]);
			}
		}
	}

	public static int GetSize(Packet_BulkEntityAttributes instance)
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
		if (instance.PartialUpdates != null)
		{
			for (int i = 0; i < instance.PartialUpdatesCount; i++)
			{
				int packetlength2 = Packet_EntityAttributeUpdateSerializer.GetSize(instance.PartialUpdates[i]);
				size += packetlength2 + ProtocolParser.GetSize(packetlength2) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_BulkEntityAttributes instance)
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

	public static byte[] SerializeToBytes(Packet_BulkEntityAttributes instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_BulkEntityAttributes instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
