using System;

public class Packet_InventoryContentsSerializer
{
	private const int field = 8;

	public static Packet_InventoryContents DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_InventoryContents instance = new Packet_InventoryContents();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_InventoryContents DeserializeBuffer(byte[] buffer, int length, Packet_InventoryContents instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_InventoryContents Deserialize(CitoMemoryStream stream, Packet_InventoryContents instance)
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
				instance.InventoryClass = ProtocolParser.ReadString(stream);
				break;
			case 26:
				instance.InventoryId = ProtocolParser.ReadString(stream);
				break;
			case 34:
				instance.ItemstacksAdd(Packet_ItemStackSerializer.DeserializeLengthDelimitedNew(stream));
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

	public static Packet_InventoryContents DeserializeLengthDelimited(CitoMemoryStream stream, Packet_InventoryContents instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_InventoryContents result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_InventoryContents instance)
	{
		if (instance.ClientId != 0)
		{
			stream.WriteByte(8);
			ProtocolParser.WriteUInt32(stream, instance.ClientId);
		}
		if (instance.InventoryClass != null)
		{
			stream.WriteByte(18);
			ProtocolParser.WriteString(stream, instance.InventoryClass);
		}
		if (instance.InventoryId != null)
		{
			stream.WriteByte(26);
			ProtocolParser.WriteString(stream, instance.InventoryId);
		}
		if (instance.Itemstacks != null)
		{
			Packet_ItemStack[] elems = instance.Itemstacks;
			int elemCount = instance.ItemstacksCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(34);
				Packet_ItemStackSerializer.SerializeWithSize(stream, elems[k]);
			}
		}
	}

	public static int GetSize(Packet_InventoryContents instance)
	{
		int size = 0;
		if (instance.ClientId != 0)
		{
			size += ProtocolParser.GetSize(instance.ClientId) + 1;
		}
		if (instance.InventoryClass != null)
		{
			size += ProtocolParser.GetSize(instance.InventoryClass) + 1;
		}
		if (instance.InventoryId != null)
		{
			size += ProtocolParser.GetSize(instance.InventoryId) + 1;
		}
		if (instance.Itemstacks != null)
		{
			for (int k = 0; k < instance.ItemstacksCount; k++)
			{
				int packetlength = Packet_ItemStackSerializer.GetSize(instance.Itemstacks[k]);
				size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_InventoryContents instance)
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

	public static byte[] SerializeToBytes(Packet_InventoryContents instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_InventoryContents instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
