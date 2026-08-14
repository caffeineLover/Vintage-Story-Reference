using System;

public class Packet_ItemStackSerializer
{
	private const int field = 8;

	public static Packet_ItemStack DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_ItemStack instance = new Packet_ItemStack();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_ItemStack DeserializeBuffer(byte[] buffer, int length, Packet_ItemStack instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_ItemStack Deserialize(CitoMemoryStream stream, Packet_ItemStack instance)
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
				instance.ItemClass = ProtocolParser.ReadUInt32(stream);
				break;
			case 16:
				instance.ItemId = ProtocolParser.ReadUInt32(stream);
				break;
			case 24:
				instance.StackSize = ProtocolParser.ReadUInt32(stream);
				break;
			case 34:
				instance.Attributes = ProtocolParser.ReadBytes(stream);
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

	public static Packet_ItemStack DeserializeLengthDelimited(CitoMemoryStream stream, Packet_ItemStack instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_ItemStack result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_ItemStack instance)
	{
		if (instance.ItemClass != 0)
		{
			stream.WriteByte(8);
			ProtocolParser.WriteUInt32(stream, instance.ItemClass);
		}
		if (instance.ItemId != 0)
		{
			stream.WriteByte(16);
			ProtocolParser.WriteUInt32(stream, instance.ItemId);
		}
		if (instance.StackSize != 0)
		{
			stream.WriteByte(24);
			ProtocolParser.WriteUInt32(stream, instance.StackSize);
		}
		if (instance.Attributes != null)
		{
			stream.WriteByte(34);
			ProtocolParser.WriteBytes(stream, instance.Attributes);
		}
	}

	public static int GetSize(Packet_ItemStack instance)
	{
		int size = 0;
		if (instance.ItemClass != 0)
		{
			size += ProtocolParser.GetSize(instance.ItemClass) + 1;
		}
		if (instance.ItemId != 0)
		{
			size += ProtocolParser.GetSize(instance.ItemId) + 1;
		}
		if (instance.StackSize != 0)
		{
			size += ProtocolParser.GetSize(instance.StackSize) + 1;
		}
		if (instance.Attributes != null)
		{
			size += ProtocolParser.GetSize(instance.Attributes) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_ItemStack instance)
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

	public static byte[] SerializeToBytes(Packet_ItemStack instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_ItemStack instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
