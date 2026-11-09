using System;

public class Packet_InventoryUpdateSerializer
{
	private const int field = 8;

	public static Packet_InventoryUpdate DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_InventoryUpdate instance = new Packet_InventoryUpdate();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_InventoryUpdate DeserializeBuffer(byte[] buffer, int length, Packet_InventoryUpdate instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_InventoryUpdate Deserialize(CitoMemoryStream stream, Packet_InventoryUpdate instance)
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
				instance.ClientId = ProtocolParser.ReadUInt32(stream);
				break;
			case 18:
				instance.InventoryId = ProtocolParser.ReadString(stream);
				break;
			case 24:
				instance.SlotId = ProtocolParser.ReadUInt32(stream);
				break;
			case 34:
				if (instance.ItemStack == null)
				{
					instance.ItemStack = Packet_ItemStackSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ItemStackSerializer.DeserializeLengthDelimited(stream, instance.ItemStack);
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

	public static Packet_InventoryUpdate DeserializeLengthDelimited(CitoMemoryStream stream, Packet_InventoryUpdate instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_InventoryUpdate result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_InventoryUpdate instance)
	{
		if (instance.ClientId != 0)
		{
			stream.WriteByte(8);
			ProtocolParser.WriteUInt32(stream, instance.ClientId);
		}
		if (instance.InventoryId != null)
		{
			stream.WriteByte(18);
			ProtocolParser.WriteString(stream, instance.InventoryId);
		}
		if (instance.SlotId != 0)
		{
			stream.WriteByte(24);
			ProtocolParser.WriteUInt32(stream, instance.SlotId);
		}
		if (instance.ItemStack != null)
		{
			stream.WriteByte(34);
			Packet_ItemStackSerializer.SerializeWithSize(stream, instance.ItemStack);
		}
	}

	public static int GetSize(Packet_InventoryUpdate instance)
	{
		int size = 0;
		if (instance.ClientId != 0)
		{
			size += ProtocolParser.GetSize(instance.ClientId) + 1;
		}
		if (instance.InventoryId != null)
		{
			size += ProtocolParser.GetSize(instance.InventoryId) + 1;
		}
		if (instance.SlotId != 0)
		{
			size += ProtocolParser.GetSize(instance.SlotId) + 1;
		}
		if (instance.ItemStack != null)
		{
			int packetlength = Packet_ItemStackSerializer.GetSize(instance.ItemStack);
			size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_InventoryUpdate instance)
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

	public static byte[] SerializeToBytes(Packet_InventoryUpdate instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_InventoryUpdate instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
