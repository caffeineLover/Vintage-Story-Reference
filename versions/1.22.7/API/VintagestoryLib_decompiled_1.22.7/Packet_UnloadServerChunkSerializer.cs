using System;

public class Packet_UnloadServerChunkSerializer
{
	private const int field = 8;

	public static Packet_UnloadServerChunk DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_UnloadServerChunk instance = new Packet_UnloadServerChunk();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_UnloadServerChunk DeserializeBuffer(byte[] buffer, int length, Packet_UnloadServerChunk instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_UnloadServerChunk Deserialize(CitoMemoryStream stream, Packet_UnloadServerChunk instance)
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
				instance.XAdd(ProtocolParser.ReadUInt32(stream));
				break;
			case 16:
				instance.YAdd(ProtocolParser.ReadUInt32(stream));
				break;
			case 24:
				instance.ZAdd(ProtocolParser.ReadUInt32(stream));
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

	public static Packet_UnloadServerChunk DeserializeLengthDelimited(CitoMemoryStream stream, Packet_UnloadServerChunk instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_UnloadServerChunk result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_UnloadServerChunk instance)
	{
		if (instance.X != null)
		{
			int[] elems = instance.X;
			int elemCount = instance.XCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(8);
				ProtocolParser.WriteUInt32(stream, elems[k]);
			}
		}
		if (instance.Y != null)
		{
			int[] elems2 = instance.Y;
			int elemCount2 = instance.YCount;
			for (int i = 0; i < elems2.Length && i < elemCount2; i++)
			{
				stream.WriteByte(16);
				ProtocolParser.WriteUInt32(stream, elems2[i]);
			}
		}
		if (instance.Z != null)
		{
			int[] elems3 = instance.Z;
			int elemCount3 = instance.ZCount;
			for (int j = 0; j < elems3.Length && j < elemCount3; j++)
			{
				stream.WriteByte(24);
				ProtocolParser.WriteUInt32(stream, elems3[j]);
			}
		}
	}

	public static int GetSize(Packet_UnloadServerChunk instance)
	{
		int size = 0;
		if (instance.X != null)
		{
			for (int k = 0; k < instance.XCount; k++)
			{
				int i1 = instance.X[k];
				size += ProtocolParser.GetSize(i1) + 1;
			}
		}
		if (instance.Y != null)
		{
			for (int j = 0; j < instance.YCount; j++)
			{
				int i2 = instance.Y[j];
				size += ProtocolParser.GetSize(i2) + 1;
			}
		}
		if (instance.Z != null)
		{
			for (int l = 0; l < instance.ZCount; l++)
			{
				int i3 = instance.Z[l];
				size += ProtocolParser.GetSize(i3) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_UnloadServerChunk instance)
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

	public static byte[] SerializeToBytes(Packet_UnloadServerChunk instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_UnloadServerChunk instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
