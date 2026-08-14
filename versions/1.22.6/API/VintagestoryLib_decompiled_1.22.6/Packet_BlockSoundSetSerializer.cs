using System;

public class Packet_BlockSoundSetSerializer
{
	private const int field = 8;

	public static Packet_BlockSoundSet DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_BlockSoundSet instance = new Packet_BlockSoundSet();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_BlockSoundSet DeserializeBuffer(byte[] buffer, int length, Packet_BlockSoundSet instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_BlockSoundSet Deserialize(CitoMemoryStream stream, Packet_BlockSoundSet instance)
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
				if (instance.Walk == null)
				{
					instance.Walk = Packet_SoundAttributesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_SoundAttributesSerializer.DeserializeLengthDelimited(stream, instance.Walk);
				}
				break;
			case 18:
				if (instance.Break == null)
				{
					instance.Break = Packet_SoundAttributesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_SoundAttributesSerializer.DeserializeLengthDelimited(stream, instance.Break);
				}
				break;
			case 26:
				if (instance.Place == null)
				{
					instance.Place = Packet_SoundAttributesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_SoundAttributesSerializer.DeserializeLengthDelimited(stream, instance.Place);
				}
				break;
			case 34:
				if (instance.Hit == null)
				{
					instance.Hit = Packet_SoundAttributesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_SoundAttributesSerializer.DeserializeLengthDelimited(stream, instance.Hit);
				}
				break;
			case 42:
				if (instance.Inside == null)
				{
					instance.Inside = Packet_SoundAttributesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_SoundAttributesSerializer.DeserializeLengthDelimited(stream, instance.Inside);
				}
				break;
			case 50:
				instance.Ambient = ProtocolParser.ReadString(stream);
				break;
			case 72:
				instance.AmbientBlockCount = ProtocolParser.ReadUInt32(stream);
				break;
			case 80:
				instance.AmbientSoundType = ProtocolParser.ReadUInt32(stream);
				break;
			case 88:
				instance.AmbientMaxDistanceMerge = ProtocolParser.ReadUInt32(stream);
				break;
			case 56:
				instance.ByToolToolAdd(ProtocolParser.ReadUInt32(stream));
				break;
			case 66:
				instance.ByToolSoundAdd(DeserializeLengthDelimitedNew(stream));
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

	public static Packet_BlockSoundSet DeserializeLengthDelimited(CitoMemoryStream stream, Packet_BlockSoundSet instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_BlockSoundSet result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_BlockSoundSet instance)
	{
		if (instance.Walk != null)
		{
			stream.WriteByte(10);
			Packet_SoundAttributesSerializer.SerializeWithSize(stream, instance.Walk);
		}
		if (instance.Break != null)
		{
			stream.WriteByte(18);
			Packet_SoundAttributesSerializer.SerializeWithSize(stream, instance.Break);
		}
		if (instance.Place != null)
		{
			stream.WriteByte(26);
			Packet_SoundAttributesSerializer.SerializeWithSize(stream, instance.Place);
		}
		if (instance.Hit != null)
		{
			stream.WriteByte(34);
			Packet_SoundAttributesSerializer.SerializeWithSize(stream, instance.Hit);
		}
		if (instance.Inside != null)
		{
			stream.WriteByte(42);
			Packet_SoundAttributesSerializer.SerializeWithSize(stream, instance.Inside);
		}
		if (instance.Ambient != null)
		{
			stream.WriteByte(50);
			ProtocolParser.WriteString(stream, instance.Ambient);
		}
		if (instance.AmbientBlockCount != 0)
		{
			stream.WriteByte(72);
			ProtocolParser.WriteUInt32(stream, instance.AmbientBlockCount);
		}
		if (instance.AmbientSoundType != 0)
		{
			stream.WriteByte(80);
			ProtocolParser.WriteUInt32(stream, instance.AmbientSoundType);
		}
		if (instance.AmbientMaxDistanceMerge != 0)
		{
			stream.WriteByte(88);
			ProtocolParser.WriteUInt32(stream, instance.AmbientMaxDistanceMerge);
		}
		if (instance.ByToolTool != null)
		{
			int[] elems = instance.ByToolTool;
			int elemCount = instance.ByToolToolCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(56);
				ProtocolParser.WriteUInt32(stream, elems[k]);
			}
		}
		if (instance.ByToolSound != null)
		{
			Packet_BlockSoundSet[] elems2 = instance.ByToolSound;
			int elemCount2 = instance.ByToolSoundCount;
			for (int i = 0; i < elems2.Length && i < elemCount2; i++)
			{
				stream.WriteByte(66);
				SerializeWithSize(stream, elems2[i]);
			}
		}
	}

	public static int GetSize(Packet_BlockSoundSet instance)
	{
		int size = 0;
		if (instance.Walk != null)
		{
			int packetlength = Packet_SoundAttributesSerializer.GetSize(instance.Walk);
			size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
		}
		if (instance.Break != null)
		{
			int packetlength2 = Packet_SoundAttributesSerializer.GetSize(instance.Break);
			size += packetlength2 + ProtocolParser.GetSize(packetlength2) + 1;
		}
		if (instance.Place != null)
		{
			int packetlength3 = Packet_SoundAttributesSerializer.GetSize(instance.Place);
			size += packetlength3 + ProtocolParser.GetSize(packetlength3) + 1;
		}
		if (instance.Hit != null)
		{
			int packetlength4 = Packet_SoundAttributesSerializer.GetSize(instance.Hit);
			size += packetlength4 + ProtocolParser.GetSize(packetlength4) + 1;
		}
		if (instance.Inside != null)
		{
			int packetlength5 = Packet_SoundAttributesSerializer.GetSize(instance.Inside);
			size += packetlength5 + ProtocolParser.GetSize(packetlength5) + 1;
		}
		if (instance.Ambient != null)
		{
			size += ProtocolParser.GetSize(instance.Ambient) + 1;
		}
		if (instance.AmbientBlockCount != 0)
		{
			size += ProtocolParser.GetSize(instance.AmbientBlockCount) + 1;
		}
		if (instance.AmbientSoundType != 0)
		{
			size += ProtocolParser.GetSize(instance.AmbientSoundType) + 1;
		}
		if (instance.AmbientMaxDistanceMerge != 0)
		{
			size += ProtocolParser.GetSize(instance.AmbientMaxDistanceMerge) + 1;
		}
		if (instance.ByToolTool != null)
		{
			for (int k = 0; k < instance.ByToolToolCount; k++)
			{
				int i7 = instance.ByToolTool[k];
				size += ProtocolParser.GetSize(i7) + 1;
			}
		}
		if (instance.ByToolSound != null)
		{
			for (int j = 0; j < instance.ByToolSoundCount; j++)
			{
				int packetlength6 = GetSize(instance.ByToolSound[j]);
				size += packetlength6 + ProtocolParser.GetSize(packetlength6) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_BlockSoundSet instance)
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

	public static byte[] SerializeToBytes(Packet_BlockSoundSet instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_BlockSoundSet instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
