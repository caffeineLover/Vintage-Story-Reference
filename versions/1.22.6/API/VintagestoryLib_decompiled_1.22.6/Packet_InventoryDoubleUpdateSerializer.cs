using System;

public class Packet_InventoryDoubleUpdateSerializer
{
	private const int field = 8;

	public static Packet_InventoryDoubleUpdate DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_InventoryDoubleUpdate instance = new Packet_InventoryDoubleUpdate();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_InventoryDoubleUpdate DeserializeBuffer(byte[] buffer, int length, Packet_InventoryDoubleUpdate instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_InventoryDoubleUpdate Deserialize(CitoMemoryStream stream, Packet_InventoryDoubleUpdate instance)
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
				instance.InventoryId1 = ProtocolParser.ReadString(stream);
				break;
			case 26:
				instance.InventoryId2 = ProtocolParser.ReadString(stream);
				break;
			case 32:
				instance.SlotId1 = ProtocolParser.ReadUInt32(stream);
				break;
			case 40:
				instance.SlotId2 = ProtocolParser.ReadUInt32(stream);
				break;
			case 50:
				if (instance.ItemStack1 == null)
				{
					instance.ItemStack1 = Packet_ItemStackSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ItemStackSerializer.DeserializeLengthDelimited(stream, instance.ItemStack1);
				}
				break;
			case 58:
				if (instance.ItemStack2 == null)
				{
					instance.ItemStack2 = Packet_ItemStackSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ItemStackSerializer.DeserializeLengthDelimited(stream, instance.ItemStack2);
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

	public static Packet_InventoryDoubleUpdate DeserializeLengthDelimited(CitoMemoryStream stream, Packet_InventoryDoubleUpdate instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_InventoryDoubleUpdate result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_InventoryDoubleUpdate instance)
	{
		if (instance.ClientId != 0)
		{
			stream.WriteByte(8);
			ProtocolParser.WriteUInt32(stream, instance.ClientId);
		}
		if (instance.InventoryId1 != null)
		{
			stream.WriteByte(18);
			ProtocolParser.WriteString(stream, instance.InventoryId1);
		}
		if (instance.InventoryId2 != null)
		{
			stream.WriteByte(26);
			ProtocolParser.WriteString(stream, instance.InventoryId2);
		}
		if (instance.SlotId1 != 0)
		{
			stream.WriteByte(32);
			ProtocolParser.WriteUInt32(stream, instance.SlotId1);
		}
		if (instance.SlotId2 != 0)
		{
			stream.WriteByte(40);
			ProtocolParser.WriteUInt32(stream, instance.SlotId2);
		}
		if (instance.ItemStack1 != null)
		{
			stream.WriteByte(50);
			Packet_ItemStackSerializer.SerializeWithSize(stream, instance.ItemStack1);
		}
		if (instance.ItemStack2 != null)
		{
			stream.WriteByte(58);
			Packet_ItemStackSerializer.SerializeWithSize(stream, instance.ItemStack2);
		}
	}

	public static int GetSize(Packet_InventoryDoubleUpdate instance)
	{
		int size = 0;
		if (instance.ClientId != 0)
		{
			size += ProtocolParser.GetSize(instance.ClientId) + 1;
		}
		if (instance.InventoryId1 != null)
		{
			size += ProtocolParser.GetSize(instance.InventoryId1) + 1;
		}
		if (instance.InventoryId2 != null)
		{
			size += ProtocolParser.GetSize(instance.InventoryId2) + 1;
		}
		if (instance.SlotId1 != 0)
		{
			size += ProtocolParser.GetSize(instance.SlotId1) + 1;
		}
		if (instance.SlotId2 != 0)
		{
			size += ProtocolParser.GetSize(instance.SlotId2) + 1;
		}
		if (instance.ItemStack1 != null)
		{
			int packetlength = Packet_ItemStackSerializer.GetSize(instance.ItemStack1);
			size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
		}
		if (instance.ItemStack2 != null)
		{
			int packetlength2 = Packet_ItemStackSerializer.GetSize(instance.ItemStack2);
			size += packetlength2 + ProtocolParser.GetSize(packetlength2) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_InventoryDoubleUpdate instance)
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

	public static byte[] SerializeToBytes(Packet_InventoryDoubleUpdate instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_InventoryDoubleUpdate instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
