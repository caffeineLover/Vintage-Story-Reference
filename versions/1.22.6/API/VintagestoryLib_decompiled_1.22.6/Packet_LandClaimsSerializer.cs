using System;

public class Packet_LandClaimsSerializer
{
	private const int field = 8;

	public static Packet_LandClaims DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_LandClaims instance = new Packet_LandClaims();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_LandClaims DeserializeBuffer(byte[] buffer, int length, Packet_LandClaims instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_LandClaims Deserialize(CitoMemoryStream stream, Packet_LandClaims instance)
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
				instance.AllclaimsAdd(Packet_LandClaimSerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 18:
				instance.AddclaimsAdd(Packet_LandClaimSerializer.DeserializeLengthDelimitedNew(stream));
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

	public static Packet_LandClaims DeserializeLengthDelimited(CitoMemoryStream stream, Packet_LandClaims instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_LandClaims result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_LandClaims instance)
	{
		if (instance.Allclaims != null)
		{
			Packet_LandClaim[] elems = instance.Allclaims;
			int elemCount = instance.AllclaimsCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(10);
				Packet_LandClaimSerializer.SerializeWithSize(stream, elems[k]);
			}
		}
		if (instance.Addclaims != null)
		{
			Packet_LandClaim[] elems2 = instance.Addclaims;
			int elemCount2 = instance.AddclaimsCount;
			for (int i = 0; i < elems2.Length && i < elemCount2; i++)
			{
				stream.WriteByte(18);
				Packet_LandClaimSerializer.SerializeWithSize(stream, elems2[i]);
			}
		}
	}

	public static int GetSize(Packet_LandClaims instance)
	{
		int size = 0;
		if (instance.Allclaims != null)
		{
			for (int k = 0; k < instance.AllclaimsCount; k++)
			{
				int packetlength = Packet_LandClaimSerializer.GetSize(instance.Allclaims[k]);
				size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
			}
		}
		if (instance.Addclaims != null)
		{
			for (int i = 0; i < instance.AddclaimsCount; i++)
			{
				int packetlength2 = Packet_LandClaimSerializer.GetSize(instance.Addclaims[i]);
				size += packetlength2 + ProtocolParser.GetSize(packetlength2) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_LandClaims instance)
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

	public static byte[] SerializeToBytes(Packet_LandClaims instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_LandClaims instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
