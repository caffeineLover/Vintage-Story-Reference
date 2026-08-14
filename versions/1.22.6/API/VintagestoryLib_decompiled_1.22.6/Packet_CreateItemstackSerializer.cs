using System;

public class Packet_CreateItemstackSerializer
{
	private const int field = 8;

	public static Packet_CreateItemstack DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_CreateItemstack instance = new Packet_CreateItemstack();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_CreateItemstack DeserializeBuffer(byte[] buffer, int length, Packet_CreateItemstack instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_CreateItemstack Deserialize(CitoMemoryStream stream, Packet_CreateItemstack instance)
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
				instance.TargetInventoryId = ProtocolParser.ReadString(stream);
				break;
			case 16:
				instance.TargetSlot = ProtocolParser.ReadUInt32(stream);
				break;
			case 24:
				instance.TargetLastChanged = ProtocolParser.ReadUInt64(stream);
				break;
			case 34:
				if (instance.Itemstack == null)
				{
					instance.Itemstack = Packet_ItemStackSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ItemStackSerializer.DeserializeLengthDelimited(stream, instance.Itemstack);
				}
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

	public static Packet_CreateItemstack DeserializeLengthDelimited(CitoMemoryStream stream, Packet_CreateItemstack instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_CreateItemstack result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_CreateItemstack instance)
	{
		if (instance.TargetInventoryId != null)
		{
			stream.WriteByte(10);
			ProtocolParser.WriteString(stream, instance.TargetInventoryId);
		}
		if (instance.TargetSlot != 0)
		{
			stream.WriteByte(16);
			ProtocolParser.WriteUInt32(stream, instance.TargetSlot);
		}
		if (instance.TargetLastChanged != 0L)
		{
			stream.WriteByte(24);
			ProtocolParser.WriteUInt64(stream, instance.TargetLastChanged);
		}
		if (instance.Itemstack != null)
		{
			stream.WriteByte(34);
			Packet_ItemStackSerializer.SerializeWithSize(stream, instance.Itemstack);
		}
	}

	public static int GetSize(Packet_CreateItemstack instance)
	{
		int size = 0;
		if (instance.TargetInventoryId != null)
		{
			size += ProtocolParser.GetSize(instance.TargetInventoryId) + 1;
		}
		if (instance.TargetSlot != 0)
		{
			size += ProtocolParser.GetSize(instance.TargetSlot) + 1;
		}
		if (instance.TargetLastChanged != 0L)
		{
			size += ProtocolParser.GetSize(instance.TargetLastChanged) + 1;
		}
		if (instance.Itemstack != null)
		{
			int packetlength = Packet_ItemStackSerializer.GetSize(instance.Itemstack);
			size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_CreateItemstack instance)
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

	public static byte[] SerializeToBytes(Packet_CreateItemstack instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_CreateItemstack instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
