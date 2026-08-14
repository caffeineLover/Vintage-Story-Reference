using System;

public class Packet_HeldSoundSetSerializer
{
	private const int field = 8;

	public static Packet_HeldSoundSet DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_HeldSoundSet instance = new Packet_HeldSoundSet();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_HeldSoundSet DeserializeBuffer(byte[] buffer, int length, Packet_HeldSoundSet instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_HeldSoundSet Deserialize(CitoMemoryStream stream, Packet_HeldSoundSet instance)
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
				if (instance.Idle == null)
				{
					instance.Idle = Packet_SoundAttributesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_SoundAttributesSerializer.DeserializeLengthDelimited(stream, instance.Idle);
				}
				break;
			case 18:
				if (instance.Equip == null)
				{
					instance.Equip = Packet_SoundAttributesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_SoundAttributesSerializer.DeserializeLengthDelimited(stream, instance.Equip);
				}
				break;
			case 26:
				if (instance.Unequip == null)
				{
					instance.Unequip = Packet_SoundAttributesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_SoundAttributesSerializer.DeserializeLengthDelimited(stream, instance.Unequip);
				}
				break;
			case 34:
				if (instance.Attack == null)
				{
					instance.Attack = Packet_SoundAttributesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_SoundAttributesSerializer.DeserializeLengthDelimited(stream, instance.Attack);
				}
				break;
			case 42:
				if (instance.InvPickup == null)
				{
					instance.InvPickup = Packet_SoundAttributesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_SoundAttributesSerializer.DeserializeLengthDelimited(stream, instance.InvPickup);
				}
				break;
			case 50:
				if (instance.InvPlace == null)
				{
					instance.InvPlace = Packet_SoundAttributesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_SoundAttributesSerializer.DeserializeLengthDelimited(stream, instance.InvPlace);
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

	public static Packet_HeldSoundSet DeserializeLengthDelimited(CitoMemoryStream stream, Packet_HeldSoundSet instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_HeldSoundSet result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_HeldSoundSet instance)
	{
		if (instance.Idle != null)
		{
			stream.WriteByte(10);
			Packet_SoundAttributesSerializer.SerializeWithSize(stream, instance.Idle);
		}
		if (instance.Equip != null)
		{
			stream.WriteByte(18);
			Packet_SoundAttributesSerializer.SerializeWithSize(stream, instance.Equip);
		}
		if (instance.Unequip != null)
		{
			stream.WriteByte(26);
			Packet_SoundAttributesSerializer.SerializeWithSize(stream, instance.Unequip);
		}
		if (instance.Attack != null)
		{
			stream.WriteByte(34);
			Packet_SoundAttributesSerializer.SerializeWithSize(stream, instance.Attack);
		}
		if (instance.InvPickup != null)
		{
			stream.WriteByte(42);
			Packet_SoundAttributesSerializer.SerializeWithSize(stream, instance.InvPickup);
		}
		if (instance.InvPlace != null)
		{
			stream.WriteByte(50);
			Packet_SoundAttributesSerializer.SerializeWithSize(stream, instance.InvPlace);
		}
	}

	public static int GetSize(Packet_HeldSoundSet instance)
	{
		int size = 0;
		if (instance.Idle != null)
		{
			int packetlength = Packet_SoundAttributesSerializer.GetSize(instance.Idle);
			size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
		}
		if (instance.Equip != null)
		{
			int packetlength2 = Packet_SoundAttributesSerializer.GetSize(instance.Equip);
			size += packetlength2 + ProtocolParser.GetSize(packetlength2) + 1;
		}
		if (instance.Unequip != null)
		{
			int packetlength3 = Packet_SoundAttributesSerializer.GetSize(instance.Unequip);
			size += packetlength3 + ProtocolParser.GetSize(packetlength3) + 1;
		}
		if (instance.Attack != null)
		{
			int packetlength4 = Packet_SoundAttributesSerializer.GetSize(instance.Attack);
			size += packetlength4 + ProtocolParser.GetSize(packetlength4) + 1;
		}
		if (instance.InvPickup != null)
		{
			int packetlength5 = Packet_SoundAttributesSerializer.GetSize(instance.InvPickup);
			size += packetlength5 + ProtocolParser.GetSize(packetlength5) + 1;
		}
		if (instance.InvPlace != null)
		{
			int packetlength6 = Packet_SoundAttributesSerializer.GetSize(instance.InvPlace);
			size += packetlength6 + ProtocolParser.GetSize(packetlength6) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_HeldSoundSet instance)
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

	public static byte[] SerializeToBytes(Packet_HeldSoundSet instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_HeldSoundSet instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
