using System;

public class Packet_PlayerGroupsSerializer
{
	private const int field = 8;

	public static Packet_PlayerGroups DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_PlayerGroups instance = new Packet_PlayerGroups();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_PlayerGroups DeserializeBuffer(byte[] buffer, int length, Packet_PlayerGroups instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_PlayerGroups Deserialize(CitoMemoryStream stream, Packet_PlayerGroups instance)
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
				instance.GroupsAdd(Packet_PlayerGroupSerializer.DeserializeLengthDelimitedNew(stream));
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

	public static Packet_PlayerGroups DeserializeLengthDelimited(CitoMemoryStream stream, Packet_PlayerGroups instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_PlayerGroups result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_PlayerGroups instance)
	{
		if (instance.Groups != null)
		{
			Packet_PlayerGroup[] elems = instance.Groups;
			int elemCount = instance.GroupsCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(10);
				Packet_PlayerGroupSerializer.SerializeWithSize(stream, elems[k]);
			}
		}
	}

	public static int GetSize(Packet_PlayerGroups instance)
	{
		int size = 0;
		if (instance.Groups != null)
		{
			for (int k = 0; k < instance.GroupsCount; k++)
			{
				int packetlength = Packet_PlayerGroupSerializer.GetSize(instance.Groups[k]);
				size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_PlayerGroups instance)
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

	public static byte[] SerializeToBytes(Packet_PlayerGroups instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_PlayerGroups instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
