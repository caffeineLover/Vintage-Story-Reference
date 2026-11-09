using System;

public class Packet_NetworkChannelsSerializer
{
	private const int field = 8;

	public static Packet_NetworkChannels DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_NetworkChannels instance = new Packet_NetworkChannels();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_NetworkChannels DeserializeBuffer(byte[] buffer, int length, Packet_NetworkChannels instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_NetworkChannels Deserialize(CitoMemoryStream stream, Packet_NetworkChannels instance)
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
				instance.ChannelIdsAdd(ProtocolParser.ReadUInt32(stream));
				break;
			case 18:
				instance.ChannelNamesAdd(ProtocolParser.ReadString(stream));
				break;
			case 24:
				instance.ChannelUdpIdsAdd(ProtocolParser.ReadUInt32(stream));
				break;
			case 34:
				instance.ChannelUdpNamesAdd(ProtocolParser.ReadString(stream));
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

	public static Packet_NetworkChannels DeserializeLengthDelimited(CitoMemoryStream stream, Packet_NetworkChannels instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_NetworkChannels result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_NetworkChannels instance)
	{
		if (instance.ChannelIds != null)
		{
			int[] elems = instance.ChannelIds;
			int elemCount = instance.ChannelIdsCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(8);
				ProtocolParser.WriteUInt32(stream, elems[k]);
			}
		}
		if (instance.ChannelNames != null)
		{
			string[] elems2 = instance.ChannelNames;
			int elemCount2 = instance.ChannelNamesCount;
			for (int i = 0; i < elems2.Length && i < elemCount2; i++)
			{
				stream.WriteByte(18);
				ProtocolParser.WriteString(stream, elems2[i]);
			}
		}
		if (instance.ChannelUdpIds != null)
		{
			int[] elems3 = instance.ChannelUdpIds;
			int elemCount3 = instance.ChannelUdpIdsCount;
			for (int j = 0; j < elems3.Length && j < elemCount3; j++)
			{
				stream.WriteByte(24);
				ProtocolParser.WriteUInt32(stream, elems3[j]);
			}
		}
		if (instance.ChannelUdpNames != null)
		{
			string[] elems4 = instance.ChannelUdpNames;
			int elemCount4 = instance.ChannelUdpNamesCount;
			for (int l = 0; l < elems4.Length && l < elemCount4; l++)
			{
				stream.WriteByte(34);
				ProtocolParser.WriteString(stream, elems4[l]);
			}
		}
	}

	public static int GetSize(Packet_NetworkChannels instance)
	{
		int size = 0;
		if (instance.ChannelIds != null)
		{
			for (int k = 0; k < instance.ChannelIdsCount; k++)
			{
				int i1 = instance.ChannelIds[k];
				size += ProtocolParser.GetSize(i1) + 1;
			}
		}
		if (instance.ChannelNames != null)
		{
			for (int j = 0; j < instance.ChannelNamesCount; j++)
			{
				string i2 = instance.ChannelNames[j];
				size += ProtocolParser.GetSize(i2) + 1;
			}
		}
		if (instance.ChannelUdpIds != null)
		{
			for (int l = 0; l < instance.ChannelUdpIdsCount; l++)
			{
				int i3 = instance.ChannelUdpIds[l];
				size += ProtocolParser.GetSize(i3) + 1;
			}
		}
		if (instance.ChannelUdpNames != null)
		{
			for (int m = 0; m < instance.ChannelUdpNamesCount; m++)
			{
				string i4 = instance.ChannelUdpNames[m];
				size += ProtocolParser.GetSize(i4) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_NetworkChannels instance)
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

	public static byte[] SerializeToBytes(Packet_NetworkChannels instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_NetworkChannels instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
