using System;

public class Packet_GrindingPropertiesSerializer
{
	private const int field = 8;

	public static Packet_GrindingProperties DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_GrindingProperties instance = new Packet_GrindingProperties();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_GrindingProperties DeserializeBuffer(byte[] buffer, int length, Packet_GrindingProperties instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_GrindingProperties Deserialize(CitoMemoryStream stream, Packet_GrindingProperties instance)
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
				instance.GroundStack = ProtocolParser.ReadBytes(stream);
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

	public static Packet_GrindingProperties DeserializeLengthDelimited(CitoMemoryStream stream, Packet_GrindingProperties instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_GrindingProperties result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_GrindingProperties instance)
	{
		if (instance.GroundStack != null)
		{
			stream.WriteByte(10);
			ProtocolParser.WriteBytes(stream, instance.GroundStack);
		}
	}

	public static int GetSize(Packet_GrindingProperties instance)
	{
		int size = 0;
		if (instance.GroundStack != null)
		{
			size += ProtocolParser.GetSize(instance.GroundStack) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_GrindingProperties instance)
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

	public static byte[] SerializeToBytes(Packet_GrindingProperties instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_GrindingProperties instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
