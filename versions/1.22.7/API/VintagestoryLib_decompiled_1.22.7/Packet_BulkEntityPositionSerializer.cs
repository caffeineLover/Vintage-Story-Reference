using System;

public class Packet_BulkEntityPositionSerializer
{
	private const int field = 8;

	public static Packet_BulkEntityPosition DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_BulkEntityPosition instance = new Packet_BulkEntityPosition();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_BulkEntityPosition DeserializeBuffer(byte[] buffer, int length, Packet_BulkEntityPosition instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_BulkEntityPosition Deserialize(CitoMemoryStream stream, Packet_BulkEntityPosition instance)
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
				instance.EntityPositionsAdd(Packet_EntityPositionSerializer.DeserializeLengthDelimitedNew(stream));
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

	public static Packet_BulkEntityPosition DeserializeLengthDelimited(CitoMemoryStream stream, Packet_BulkEntityPosition instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_BulkEntityPosition result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_BulkEntityPosition instance)
	{
		if (instance.EntityPositions != null)
		{
			Packet_EntityPosition[] elems = instance.EntityPositions;
			int elemCount = instance.EntityPositionsCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(10);
				Packet_EntityPositionSerializer.SerializeWithSize(stream, elems[k]);
			}
		}
	}

	public static int GetSize(Packet_BulkEntityPosition instance)
	{
		int size = 0;
		if (instance.EntityPositions != null)
		{
			for (int k = 0; k < instance.EntityPositionsCount; k++)
			{
				int packetlength = Packet_EntityPositionSerializer.GetSize(instance.EntityPositions[k]);
				size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_BulkEntityPosition instance)
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

	public static byte[] SerializeToBytes(Packet_BulkEntityPosition instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_BulkEntityPosition instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
