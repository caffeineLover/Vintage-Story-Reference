using System;

public class Packet_NutritionPropertiesSerializer
{
	private const int field = 8;

	public static Packet_NutritionProperties DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_NutritionProperties instance = new Packet_NutritionProperties();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_NutritionProperties DeserializeBuffer(byte[] buffer, int length, Packet_NutritionProperties instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_NutritionProperties Deserialize(CitoMemoryStream stream, Packet_NutritionProperties instance)
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
				instance.FoodCategory = ProtocolParser.ReadUInt32(stream);
				break;
			case 16:
				instance.Satiety = ProtocolParser.ReadUInt32(stream);
				break;
			case 24:
				instance.Health = ProtocolParser.ReadUInt32(stream);
				break;
			case 34:
				instance.EatenStack = ProtocolParser.ReadBytes(stream);
				break;
			case 40:
				instance.Intoxication = ProtocolParser.ReadUInt32(stream);
				break;
			case 48:
				instance.Psychedelic = ProtocolParser.ReadUInt32(stream);
				break;
			case 56:
				instance.SaturationLossDelay = ProtocolParser.ReadUInt32(stream);
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

	public static Packet_NutritionProperties DeserializeLengthDelimited(CitoMemoryStream stream, Packet_NutritionProperties instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_NutritionProperties result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_NutritionProperties instance)
	{
		if (instance.FoodCategory != 0)
		{
			stream.WriteByte(8);
			ProtocolParser.WriteUInt32(stream, instance.FoodCategory);
		}
		if (instance.Satiety != 0)
		{
			stream.WriteByte(16);
			ProtocolParser.WriteUInt32(stream, instance.Satiety);
		}
		if (instance.Health != 0)
		{
			stream.WriteByte(24);
			ProtocolParser.WriteUInt32(stream, instance.Health);
		}
		if (instance.EatenStack != null)
		{
			stream.WriteByte(34);
			ProtocolParser.WriteBytes(stream, instance.EatenStack);
		}
		if (instance.Intoxication != 0)
		{
			stream.WriteByte(40);
			ProtocolParser.WriteUInt32(stream, instance.Intoxication);
		}
		if (instance.Psychedelic != 0)
		{
			stream.WriteByte(48);
			ProtocolParser.WriteUInt32(stream, instance.Psychedelic);
		}
		if (instance.SaturationLossDelay != 0)
		{
			stream.WriteByte(56);
			ProtocolParser.WriteUInt32(stream, instance.SaturationLossDelay);
		}
	}

	public static int GetSize(Packet_NutritionProperties instance)
	{
		int size = 0;
		if (instance.FoodCategory != 0)
		{
			size += ProtocolParser.GetSize(instance.FoodCategory) + 1;
		}
		if (instance.Satiety != 0)
		{
			size += ProtocolParser.GetSize(instance.Satiety) + 1;
		}
		if (instance.Health != 0)
		{
			size += ProtocolParser.GetSize(instance.Health) + 1;
		}
		if (instance.EatenStack != null)
		{
			size += ProtocolParser.GetSize(instance.EatenStack) + 1;
		}
		if (instance.Intoxication != 0)
		{
			size += ProtocolParser.GetSize(instance.Intoxication) + 1;
		}
		if (instance.Psychedelic != 0)
		{
			size += ProtocolParser.GetSize(instance.Psychedelic) + 1;
		}
		if (instance.SaturationLossDelay != 0)
		{
			size += ProtocolParser.GetSize(instance.SaturationLossDelay) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_NutritionProperties instance)
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

	public static byte[] SerializeToBytes(Packet_NutritionProperties instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_NutritionProperties instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
