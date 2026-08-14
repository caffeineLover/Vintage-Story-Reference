using System;

public class Packet_HighlightBlocksSerializer
{
	private const int field = 8;

	public static Packet_HighlightBlocks DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_HighlightBlocks instance = new Packet_HighlightBlocks();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_HighlightBlocks DeserializeBuffer(byte[] buffer, int length, Packet_HighlightBlocks instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_HighlightBlocks Deserialize(CitoMemoryStream stream, Packet_HighlightBlocks instance)
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
				instance.Mode = ProtocolParser.ReadUInt32(stream);
				break;
			case 16:
				instance.Shape = ProtocolParser.ReadUInt32(stream);
				break;
			case 26:
				instance.Blocks = ProtocolParser.ReadBytes(stream);
				break;
			case 32:
				instance.ColorsAdd(ProtocolParser.ReadUInt32(stream));
				break;
			case 40:
				instance.Slotid = ProtocolParser.ReadUInt32(stream);
				break;
			case 48:
				instance.Scale = ProtocolParser.ReadUInt32(stream);
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

	public static Packet_HighlightBlocks DeserializeLengthDelimited(CitoMemoryStream stream, Packet_HighlightBlocks instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_HighlightBlocks result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_HighlightBlocks instance)
	{
		if (instance.Mode != 0)
		{
			stream.WriteByte(8);
			ProtocolParser.WriteUInt32(stream, instance.Mode);
		}
		if (instance.Shape != 0)
		{
			stream.WriteByte(16);
			ProtocolParser.WriteUInt32(stream, instance.Shape);
		}
		if (instance.Blocks != null)
		{
			stream.WriteByte(26);
			ProtocolParser.WriteBytes(stream, instance.Blocks);
		}
		if (instance.Colors != null)
		{
			int[] elems = instance.Colors;
			int elemCount = instance.ColorsCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(32);
				ProtocolParser.WriteUInt32(stream, elems[k]);
			}
		}
		if (instance.Slotid != 0)
		{
			stream.WriteByte(40);
			ProtocolParser.WriteUInt32(stream, instance.Slotid);
		}
		if (instance.Scale != 0)
		{
			stream.WriteByte(48);
			ProtocolParser.WriteUInt32(stream, instance.Scale);
		}
	}

	public static int GetSize(Packet_HighlightBlocks instance)
	{
		int size = 0;
		if (instance.Mode != 0)
		{
			size += ProtocolParser.GetSize(instance.Mode) + 1;
		}
		if (instance.Shape != 0)
		{
			size += ProtocolParser.GetSize(instance.Shape) + 1;
		}
		if (instance.Blocks != null)
		{
			size += ProtocolParser.GetSize(instance.Blocks) + 1;
		}
		if (instance.Colors != null)
		{
			for (int k = 0; k < instance.ColorsCount; k++)
			{
				int i4 = instance.Colors[k];
				size += ProtocolParser.GetSize(i4) + 1;
			}
		}
		if (instance.Slotid != 0)
		{
			size += ProtocolParser.GetSize(instance.Slotid) + 1;
		}
		if (instance.Scale != 0)
		{
			size += ProtocolParser.GetSize(instance.Scale) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_HighlightBlocks instance)
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

	public static byte[] SerializeToBytes(Packet_HighlightBlocks instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_HighlightBlocks instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
