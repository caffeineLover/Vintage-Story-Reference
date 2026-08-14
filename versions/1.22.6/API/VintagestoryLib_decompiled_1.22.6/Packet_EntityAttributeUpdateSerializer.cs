using System;

public class Packet_EntityAttributeUpdateSerializer
{
	private const int field = 8;

	public static Packet_EntityAttributeUpdate DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_EntityAttributeUpdate instance = new Packet_EntityAttributeUpdate();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_EntityAttributeUpdate DeserializeBuffer(byte[] buffer, int length, Packet_EntityAttributeUpdate instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_EntityAttributeUpdate Deserialize(CitoMemoryStream stream, Packet_EntityAttributeUpdate instance)
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
				instance.EntityId = ProtocolParser.ReadUInt64(stream);
				break;
			case 18:
				instance.AttributesAdd(Packet_PartialAttributeSerializer.DeserializeLengthDelimitedNew(stream));
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

	public static Packet_EntityAttributeUpdate DeserializeLengthDelimited(CitoMemoryStream stream, Packet_EntityAttributeUpdate instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_EntityAttributeUpdate result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_EntityAttributeUpdate instance)
	{
		if (instance.EntityId != 0L)
		{
			stream.WriteByte(8);
			ProtocolParser.WriteUInt64(stream, instance.EntityId);
		}
		if (instance.Attributes != null)
		{
			Packet_PartialAttribute[] elems = instance.Attributes;
			int elemCount = instance.AttributesCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(18);
				Packet_PartialAttributeSerializer.SerializeWithSize(stream, elems[k]);
			}
		}
	}

	public static int GetSize(Packet_EntityAttributeUpdate instance)
	{
		int size = 0;
		if (instance.EntityId != 0L)
		{
			size += ProtocolParser.GetSize(instance.EntityId) + 1;
		}
		if (instance.Attributes != null)
		{
			for (int k = 0; k < instance.AttributesCount; k++)
			{
				int packetlength = Packet_PartialAttributeSerializer.GetSize(instance.Attributes[k]);
				size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_EntityAttributeUpdate instance)
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

	public static byte[] SerializeToBytes(Packet_EntityAttributeUpdate instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_EntityAttributeUpdate instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
