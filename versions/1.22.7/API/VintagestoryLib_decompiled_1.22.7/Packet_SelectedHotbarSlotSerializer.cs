using System;

public class Packet_SelectedHotbarSlotSerializer
{
	private const int field = 8;

	public static Packet_SelectedHotbarSlot DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_SelectedHotbarSlot instance = new Packet_SelectedHotbarSlot();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_SelectedHotbarSlot DeserializeBuffer(byte[] buffer, int length, Packet_SelectedHotbarSlot instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_SelectedHotbarSlot Deserialize(CitoMemoryStream stream, Packet_SelectedHotbarSlot instance)
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
				instance.SlotNumber = ProtocolParser.ReadUInt32(stream);
				break;
			case 16:
				instance.ClientId = ProtocolParser.ReadUInt32(stream);
				break;
			case 26:
				if (instance.Itemstack == null)
				{
					instance.Itemstack = Packet_ItemStackSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ItemStackSerializer.DeserializeLengthDelimited(stream, instance.Itemstack);
				}
				break;
			case 34:
				if (instance.OffhandStack == null)
				{
					instance.OffhandStack = Packet_ItemStackSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ItemStackSerializer.DeserializeLengthDelimited(stream, instance.OffhandStack);
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

	public static Packet_SelectedHotbarSlot DeserializeLengthDelimited(CitoMemoryStream stream, Packet_SelectedHotbarSlot instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_SelectedHotbarSlot result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_SelectedHotbarSlot instance)
	{
		if (instance.SlotNumber != 0)
		{
			stream.WriteByte(8);
			ProtocolParser.WriteUInt32(stream, instance.SlotNumber);
		}
		if (instance.ClientId != 0)
		{
			stream.WriteByte(16);
			ProtocolParser.WriteUInt32(stream, instance.ClientId);
		}
		if (instance.Itemstack != null)
		{
			stream.WriteByte(26);
			Packet_ItemStackSerializer.SerializeWithSize(stream, instance.Itemstack);
		}
		if (instance.OffhandStack != null)
		{
			stream.WriteByte(34);
			Packet_ItemStackSerializer.SerializeWithSize(stream, instance.OffhandStack);
		}
	}

	public static int GetSize(Packet_SelectedHotbarSlot instance)
	{
		int size = 0;
		if (instance.SlotNumber != 0)
		{
			size += ProtocolParser.GetSize(instance.SlotNumber) + 1;
		}
		if (instance.ClientId != 0)
		{
			size += ProtocolParser.GetSize(instance.ClientId) + 1;
		}
		if (instance.Itemstack != null)
		{
			int packetlength = Packet_ItemStackSerializer.GetSize(instance.Itemstack);
			size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
		}
		if (instance.OffhandStack != null)
		{
			int packetlength2 = Packet_ItemStackSerializer.GetSize(instance.OffhandStack);
			size += packetlength2 + ProtocolParser.GetSize(packetlength2) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_SelectedHotbarSlot instance)
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

	public static byte[] SerializeToBytes(Packet_SelectedHotbarSlot instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_SelectedHotbarSlot instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
