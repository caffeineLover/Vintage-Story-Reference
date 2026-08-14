using System;

public class Packet_UdpPacketSerializer
{
	private const int field = 8;

	public static Packet_UdpPacket DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_UdpPacket instance = new Packet_UdpPacket();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_UdpPacket DeserializeBuffer(byte[] buffer, int length, Packet_UdpPacket instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_UdpPacket Deserialize(CitoMemoryStream stream, Packet_UdpPacket instance)
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
				instance.Id = ProtocolParser.ReadUInt32(stream);
				break;
			case 18:
				if (instance.EntityPosition == null)
				{
					instance.EntityPosition = Packet_EntityPositionSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_EntityPositionSerializer.DeserializeLengthDelimited(stream, instance.EntityPosition);
				}
				break;
			case 26:
				if (instance.BulkPositions == null)
				{
					instance.BulkPositions = Packet_BulkEntityPositionSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_BulkEntityPositionSerializer.DeserializeLengthDelimited(stream, instance.BulkPositions);
				}
				break;
			case 34:
				if (instance.ChannelPacket == null)
				{
					instance.ChannelPacket = Packet_CustomPacketSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_CustomPacketSerializer.DeserializeLengthDelimited(stream, instance.ChannelPacket);
				}
				break;
			case 42:
				if (instance.ConnectionPacket == null)
				{
					instance.ConnectionPacket = Packet_ConnectionPacketSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ConnectionPacketSerializer.DeserializeLengthDelimited(stream, instance.ConnectionPacket);
				}
				break;
			case 48:
				instance.Length = ProtocolParser.ReadUInt32(stream);
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

	public static Packet_UdpPacket DeserializeLengthDelimited(CitoMemoryStream stream, Packet_UdpPacket instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_UdpPacket result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_UdpPacket instance)
	{
		stream.WriteByte(8);
		ProtocolParser.WriteUInt32(stream, instance.Id);
		if (instance.EntityPosition != null)
		{
			stream.WriteByte(18);
			Packet_EntityPosition i2 = instance.EntityPosition;
			Packet_EntityPositionSerializer.GetSize(i2);
			Packet_EntityPositionSerializer.SerializeWithSize(stream, i2);
		}
		if (instance.BulkPositions != null)
		{
			stream.WriteByte(26);
			Packet_BulkEntityPosition i3 = instance.BulkPositions;
			Packet_BulkEntityPositionSerializer.GetSize(i3);
			Packet_BulkEntityPositionSerializer.SerializeWithSize(stream, i3);
		}
		if (instance.ChannelPacket != null)
		{
			stream.WriteByte(34);
			Packet_CustomPacket i4 = instance.ChannelPacket;
			Packet_CustomPacketSerializer.GetSize(i4);
			Packet_CustomPacketSerializer.SerializeWithSize(stream, i4);
		}
		if (instance.ConnectionPacket != null)
		{
			stream.WriteByte(42);
			Packet_ConnectionPacket i5 = instance.ConnectionPacket;
			Packet_ConnectionPacketSerializer.GetSize(i5);
			Packet_ConnectionPacketSerializer.SerializeWithSize(stream, i5);
		}
		if (instance.Length != 0)
		{
			stream.WriteByte(48);
			ProtocolParser.WriteUInt32(stream, instance.Length);
		}
	}

	public static int GetSize(Packet_UdpPacket instance)
	{
		int size = 0;
		size += ProtocolParser.GetSize(instance.Id) + 1;
		if (instance.EntityPosition != null)
		{
			int packetlength = Packet_EntityPositionSerializer.GetSize(instance.EntityPosition);
			size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
		}
		if (instance.BulkPositions != null)
		{
			int packetlength2 = Packet_BulkEntityPositionSerializer.GetSize(instance.BulkPositions);
			size += packetlength2 + ProtocolParser.GetSize(packetlength2) + 1;
		}
		if (instance.ChannelPacket != null)
		{
			int packetlength3 = Packet_CustomPacketSerializer.GetSize(instance.ChannelPacket);
			size += packetlength3 + ProtocolParser.GetSize(packetlength3) + 1;
		}
		if (instance.ConnectionPacket != null)
		{
			int packetlength4 = Packet_ConnectionPacketSerializer.GetSize(instance.ConnectionPacket);
			size += packetlength4 + ProtocolParser.GetSize(packetlength4) + 1;
		}
		if (instance.Length != 0)
		{
			size += ProtocolParser.GetSize(instance.Length) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_UdpPacket instance)
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

	public static byte[] SerializeToBytes(Packet_UdpPacket instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_UdpPacket instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
