using System;

public class Packet_EntityDespawnSerializer
{
	private const int field = 8;

	public static Packet_EntityDespawn DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_EntityDespawn instance = new Packet_EntityDespawn();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_EntityDespawn DeserializeBuffer(byte[] buffer, int length, Packet_EntityDespawn instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_EntityDespawn Deserialize(CitoMemoryStream stream, Packet_EntityDespawn instance)
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
				instance.EntityIdAdd(ProtocolParser.ReadUInt64(stream));
				break;
			case 16:
				instance.DespawnReasonAdd(ProtocolParser.ReadUInt32(stream));
				break;
			case 24:
				instance.DeathDamageSourceAdd(ProtocolParser.ReadUInt32(stream));
				break;
			case 32:
				instance.ByEntityIdAdd(ProtocolParser.ReadUInt64(stream));
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

	public static Packet_EntityDespawn DeserializeLengthDelimited(CitoMemoryStream stream, Packet_EntityDespawn instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_EntityDespawn result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_EntityDespawn instance)
	{
		if (instance.EntityId != null)
		{
			long[] elems = instance.EntityId;
			int elemCount = instance.EntityIdCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(8);
				ProtocolParser.WriteUInt64(stream, elems[k]);
			}
		}
		if (instance.DespawnReason != null)
		{
			int[] elems2 = instance.DespawnReason;
			int elemCount2 = instance.DespawnReasonCount;
			for (int i = 0; i < elems2.Length && i < elemCount2; i++)
			{
				stream.WriteByte(16);
				ProtocolParser.WriteUInt32(stream, elems2[i]);
			}
		}
		if (instance.DeathDamageSource != null)
		{
			int[] elems3 = instance.DeathDamageSource;
			int elemCount3 = instance.DeathDamageSourceCount;
			for (int j = 0; j < elems3.Length && j < elemCount3; j++)
			{
				stream.WriteByte(24);
				ProtocolParser.WriteUInt32(stream, elems3[j]);
			}
		}
		if (instance.ByEntityId != null)
		{
			long[] elems4 = instance.ByEntityId;
			int elemCount4 = instance.ByEntityIdCount;
			for (int l = 0; l < elems4.Length && l < elemCount4; l++)
			{
				stream.WriteByte(32);
				ProtocolParser.WriteUInt64(stream, elems4[l]);
			}
		}
	}

	public static int GetSize(Packet_EntityDespawn instance)
	{
		int size = 0;
		if (instance.EntityId != null)
		{
			for (int k = 0; k < instance.EntityIdCount; k++)
			{
				long i1 = instance.EntityId[k];
				size += ProtocolParser.GetSize(i1) + 1;
			}
		}
		if (instance.DespawnReason != null)
		{
			for (int j = 0; j < instance.DespawnReasonCount; j++)
			{
				int i2 = instance.DespawnReason[j];
				size += ProtocolParser.GetSize(i2) + 1;
			}
		}
		if (instance.DeathDamageSource != null)
		{
			for (int l = 0; l < instance.DeathDamageSourceCount; l++)
			{
				int i3 = instance.DeathDamageSource[l];
				size += ProtocolParser.GetSize(i3) + 1;
			}
		}
		if (instance.ByEntityId != null)
		{
			for (int m = 0; m < instance.ByEntityIdCount; m++)
			{
				long i4 = instance.ByEntityId[m];
				size += ProtocolParser.GetSize(i4) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_EntityDespawn instance)
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

	public static byte[] SerializeToBytes(Packet_EntityDespawn instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_EntityDespawn instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
