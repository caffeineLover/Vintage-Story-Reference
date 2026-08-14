using System;

public class Packet_SpawnParticlesSerializer
{
	private const int field = 8;

	public static Packet_SpawnParticles DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_SpawnParticles instance = new Packet_SpawnParticles();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_SpawnParticles DeserializeBuffer(byte[] buffer, int length, Packet_SpawnParticles instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_SpawnParticles Deserialize(CitoMemoryStream stream, Packet_SpawnParticles instance)
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
				instance.ParticlePropertyProviderClassName = ProtocolParser.ReadString(stream);
				break;
			case 18:
				instance.Data = ProtocolParser.ReadBytes(stream);
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

	public static Packet_SpawnParticles DeserializeLengthDelimited(CitoMemoryStream stream, Packet_SpawnParticles instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_SpawnParticles result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_SpawnParticles instance)
	{
		if (instance.ParticlePropertyProviderClassName != null)
		{
			stream.WriteByte(10);
			ProtocolParser.WriteString(stream, instance.ParticlePropertyProviderClassName);
		}
		if (instance.Data != null)
		{
			stream.WriteByte(18);
			ProtocolParser.WriteBytes(stream, instance.Data);
		}
	}

	public static int GetSize(Packet_SpawnParticles instance)
	{
		int size = 0;
		if (instance.ParticlePropertyProviderClassName != null)
		{
			size += ProtocolParser.GetSize(instance.ParticlePropertyProviderClassName) + 1;
		}
		if (instance.Data != null)
		{
			size += ProtocolParser.GetSize(instance.Data) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_SpawnParticles instance)
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

	public static byte[] SerializeToBytes(Packet_SpawnParticles instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_SpawnParticles instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
