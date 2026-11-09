using System;

public class Packet_CrushingPropertiesSerializer
{
	private const int field = 8;

	public static Packet_CrushingProperties DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_CrushingProperties instance = new Packet_CrushingProperties();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_CrushingProperties DeserializeBuffer(byte[] buffer, int length, Packet_CrushingProperties instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_CrushingProperties Deserialize(CitoMemoryStream stream, Packet_CrushingProperties instance)
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
				instance.CrushedStack = ProtocolParser.ReadBytes(stream);
				break;
			case 16:
				instance.HardnessTier = ProtocolParser.ReadUInt32(stream);
				break;
			case 26:
				if (instance.Quantity == null)
				{
					instance.Quantity = Packet_NatFloatSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_NatFloatSerializer.DeserializeLengthDelimited(stream, instance.Quantity);
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

	public static Packet_CrushingProperties DeserializeLengthDelimited(CitoMemoryStream stream, Packet_CrushingProperties instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_CrushingProperties result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_CrushingProperties instance)
	{
		if (instance.CrushedStack != null)
		{
			stream.WriteByte(10);
			ProtocolParser.WriteBytes(stream, instance.CrushedStack);
		}
		if (instance.HardnessTier != 0)
		{
			stream.WriteByte(16);
			ProtocolParser.WriteUInt32(stream, instance.HardnessTier);
		}
		if (instance.Quantity != null)
		{
			stream.WriteByte(26);
			Packet_NatFloatSerializer.SerializeWithSize(stream, instance.Quantity);
		}
	}

	public static int GetSize(Packet_CrushingProperties instance)
	{
		int size = 0;
		if (instance.CrushedStack != null)
		{
			size += ProtocolParser.GetSize(instance.CrushedStack) + 1;
		}
		if (instance.HardnessTier != 0)
		{
			size += ProtocolParser.GetSize(instance.HardnessTier) + 1;
		}
		if (instance.Quantity != null)
		{
			int packetlength = Packet_NatFloatSerializer.GetSize(instance.Quantity);
			size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_CrushingProperties instance)
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

	public static byte[] SerializeToBytes(Packet_CrushingProperties instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_CrushingProperties instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
