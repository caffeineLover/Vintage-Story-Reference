using System;

public class Packet_ToolModeSerializer
{
	private const int field = 8;

	public static Packet_ToolMode DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_ToolMode instance = new Packet_ToolMode();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_ToolMode DeserializeBuffer(byte[] buffer, int length, Packet_ToolMode instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_ToolMode Deserialize(CitoMemoryStream stream, Packet_ToolMode instance)
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
				instance.X = ProtocolParser.ReadUInt32(stream);
				break;
			case 24:
				instance.Y = ProtocolParser.ReadUInt32(stream);
				break;
			case 32:
				instance.Z = ProtocolParser.ReadUInt32(stream);
				break;
			case 40:
				instance.Face = ProtocolParser.ReadUInt32(stream);
				break;
			case 48:
				instance.SelectionBoxIndex = ProtocolParser.ReadUInt32(stream);
				break;
			case 56:
				instance.HitX = ProtocolParser.ReadUInt64(stream);
				break;
			case 64:
				instance.HitY = ProtocolParser.ReadUInt64(stream);
				break;
			case 72:
				instance.HitZ = ProtocolParser.ReadUInt64(stream);
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

	public static Packet_ToolMode DeserializeLengthDelimited(CitoMemoryStream stream, Packet_ToolMode instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_ToolMode result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_ToolMode instance)
	{
		if (instance.Mode != 0)
		{
			stream.WriteByte(8);
			ProtocolParser.WriteUInt32(stream, instance.Mode);
		}
		if (instance.X != 0)
		{
			stream.WriteByte(16);
			ProtocolParser.WriteUInt32(stream, instance.X);
		}
		if (instance.Y != 0)
		{
			stream.WriteByte(24);
			ProtocolParser.WriteUInt32(stream, instance.Y);
		}
		if (instance.Z != 0)
		{
			stream.WriteByte(32);
			ProtocolParser.WriteUInt32(stream, instance.Z);
		}
		if (instance.Face != 0)
		{
			stream.WriteByte(40);
			ProtocolParser.WriteUInt32(stream, instance.Face);
		}
		if (instance.SelectionBoxIndex != 0)
		{
			stream.WriteByte(48);
			ProtocolParser.WriteUInt32(stream, instance.SelectionBoxIndex);
		}
		if (instance.HitX != 0L)
		{
			stream.WriteByte(56);
			ProtocolParser.WriteUInt64(stream, instance.HitX);
		}
		if (instance.HitY != 0L)
		{
			stream.WriteByte(64);
			ProtocolParser.WriteUInt64(stream, instance.HitY);
		}
		if (instance.HitZ != 0L)
		{
			stream.WriteByte(72);
			ProtocolParser.WriteUInt64(stream, instance.HitZ);
		}
	}

	public static int GetSize(Packet_ToolMode instance)
	{
		int size = 0;
		if (instance.Mode != 0)
		{
			size += ProtocolParser.GetSize(instance.Mode) + 1;
		}
		if (instance.X != 0)
		{
			size += ProtocolParser.GetSize(instance.X) + 1;
		}
		if (instance.Y != 0)
		{
			size += ProtocolParser.GetSize(instance.Y) + 1;
		}
		if (instance.Z != 0)
		{
			size += ProtocolParser.GetSize(instance.Z) + 1;
		}
		if (instance.Face != 0)
		{
			size += ProtocolParser.GetSize(instance.Face) + 1;
		}
		if (instance.SelectionBoxIndex != 0)
		{
			size += ProtocolParser.GetSize(instance.SelectionBoxIndex) + 1;
		}
		if (instance.HitX != 0L)
		{
			size += ProtocolParser.GetSize(instance.HitX) + 1;
		}
		if (instance.HitY != 0L)
		{
			size += ProtocolParser.GetSize(instance.HitY) + 1;
		}
		if (instance.HitZ != 0L)
		{
			size += ProtocolParser.GetSize(instance.HitZ) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_ToolMode instance)
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

	public static byte[] SerializeToBytes(Packet_ToolMode instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_ToolMode instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
