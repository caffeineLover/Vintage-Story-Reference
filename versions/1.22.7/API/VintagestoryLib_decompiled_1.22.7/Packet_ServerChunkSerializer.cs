using System;

public class Packet_ServerChunkSerializer
{
	private const int field = 8;

	public static Packet_ServerChunk DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_ServerChunk instance = new Packet_ServerChunk();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_ServerChunk DeserializeBuffer(byte[] buffer, int length, Packet_ServerChunk instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_ServerChunk Deserialize(CitoMemoryStream stream, Packet_ServerChunk instance)
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
				instance.Blocks = ProtocolParser.ReadBytes(stream);
				break;
			case 18:
				instance.Light = ProtocolParser.ReadBytes(stream);
				break;
			case 26:
				instance.LightSat = ProtocolParser.ReadBytes(stream);
				break;
			case 122:
				instance.Liquids = ProtocolParser.ReadBytes(stream);
				break;
			case 72:
				instance.LightPositionsAdd(ProtocolParser.ReadUInt32(stream));
				break;
			case 32:
				instance.X = ProtocolParser.ReadUInt32(stream);
				break;
			case 40:
				instance.Y = ProtocolParser.ReadUInt32(stream);
				break;
			case 48:
				instance.Z = ProtocolParser.ReadUInt32(stream);
				break;
			case 58:
				instance.EntitiesAdd(Packet_EntitySerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 66:
				instance.BlockEntitiesAdd(Packet_BlockEntitySerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 82:
				instance.Moddata = ProtocolParser.ReadBytes(stream);
				break;
			case 88:
				instance.Empty = ProtocolParser.ReadUInt32(stream);
				break;
			case 96:
				instance.DecorsPosAdd(ProtocolParser.ReadUInt32(stream));
				break;
			case 104:
				instance.DecorsIdsAdd(ProtocolParser.ReadUInt32(stream));
				break;
			case 112:
				instance.Compver = ProtocolParser.ReadUInt32(stream);
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

	public static Packet_ServerChunk DeserializeLengthDelimited(CitoMemoryStream stream, Packet_ServerChunk instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_ServerChunk result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_ServerChunk instance)
	{
		if (instance.Blocks != null)
		{
			stream.WriteByte(10);
			ProtocolParser.WriteBytes(stream, instance.Blocks);
		}
		if (instance.Light != null)
		{
			stream.WriteByte(18);
			ProtocolParser.WriteBytes(stream, instance.Light);
		}
		if (instance.LightSat != null)
		{
			stream.WriteByte(26);
			ProtocolParser.WriteBytes(stream, instance.LightSat);
		}
		if (instance.Liquids != null)
		{
			stream.WriteByte(122);
			ProtocolParser.WriteBytes(stream, instance.Liquids);
		}
		if (instance.LightPositions != null)
		{
			int[] elems = instance.LightPositions;
			int elemCount = instance.LightPositionsCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(72);
				ProtocolParser.WriteUInt32(stream, elems[k]);
			}
		}
		if (instance.X != 0)
		{
			stream.WriteByte(32);
			ProtocolParser.WriteUInt32(stream, instance.X);
		}
		if (instance.Y != 0)
		{
			stream.WriteByte(40);
			ProtocolParser.WriteUInt32(stream, instance.Y);
		}
		if (instance.Z != 0)
		{
			stream.WriteByte(48);
			ProtocolParser.WriteUInt32(stream, instance.Z);
		}
		if (instance.Entities != null)
		{
			Packet_Entity[] elems2 = instance.Entities;
			int elemCount2 = instance.EntitiesCount;
			for (int i = 0; i < elems2.Length && i < elemCount2; i++)
			{
				stream.WriteByte(58);
				Packet_EntitySerializer.SerializeWithSize(stream, elems2[i]);
			}
		}
		if (instance.BlockEntities != null)
		{
			Packet_BlockEntity[] elems3 = instance.BlockEntities;
			int elemCount3 = instance.BlockEntitiesCount;
			for (int j = 0; j < elems3.Length && j < elemCount3; j++)
			{
				stream.WriteByte(66);
				Packet_BlockEntitySerializer.SerializeWithSize(stream, elems3[j]);
			}
		}
		if (instance.Moddata != null)
		{
			stream.WriteByte(82);
			ProtocolParser.WriteBytes(stream, instance.Moddata);
		}
		if (instance.Empty != 0)
		{
			stream.WriteByte(88);
			ProtocolParser.WriteUInt32(stream, instance.Empty);
		}
		if (instance.DecorsPos != null)
		{
			int[] elems4 = instance.DecorsPos;
			int elemCount4 = instance.DecorsPosCount;
			for (int l = 0; l < elems4.Length && l < elemCount4; l++)
			{
				stream.WriteByte(96);
				ProtocolParser.WriteUInt32(stream, elems4[l]);
			}
		}
		if (instance.DecorsIds != null)
		{
			int[] elems5 = instance.DecorsIds;
			int elemCount5 = instance.DecorsIdsCount;
			for (int m = 0; m < elems5.Length && m < elemCount5; m++)
			{
				stream.WriteByte(104);
				ProtocolParser.WriteUInt32(stream, elems5[m]);
			}
		}
		if (instance.Compver != 0)
		{
			stream.WriteByte(112);
			ProtocolParser.WriteUInt32(stream, instance.Compver);
		}
	}

	public static int GetSize(Packet_ServerChunk instance)
	{
		int size = 0;
		if (instance.Blocks != null)
		{
			size += ProtocolParser.GetSize(instance.Blocks) + 1;
		}
		if (instance.Light != null)
		{
			size += ProtocolParser.GetSize(instance.Light) + 1;
		}
		if (instance.LightSat != null)
		{
			size += ProtocolParser.GetSize(instance.LightSat) + 1;
		}
		if (instance.Liquids != null)
		{
			size += ProtocolParser.GetSize(instance.Liquids) + 1;
		}
		if (instance.LightPositions != null)
		{
			for (int k = 0; k < instance.LightPositionsCount; k++)
			{
				int i9 = instance.LightPositions[k];
				size += ProtocolParser.GetSize(i9) + 1;
			}
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
		if (instance.Entities != null)
		{
			for (int j = 0; j < instance.EntitiesCount; j++)
			{
				int packetlength = Packet_EntitySerializer.GetSize(instance.Entities[j]);
				size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
			}
		}
		if (instance.BlockEntities != null)
		{
			for (int l = 0; l < instance.BlockEntitiesCount; l++)
			{
				int packetlength2 = Packet_BlockEntitySerializer.GetSize(instance.BlockEntities[l]);
				size += packetlength2 + ProtocolParser.GetSize(packetlength2) + 1;
			}
		}
		if (instance.Moddata != null)
		{
			size += ProtocolParser.GetSize(instance.Moddata) + 1;
		}
		if (instance.Empty != 0)
		{
			size += ProtocolParser.GetSize(instance.Empty) + 1;
		}
		if (instance.DecorsPos != null)
		{
			for (int m = 0; m < instance.DecorsPosCount; m++)
			{
				int i12 = instance.DecorsPos[m];
				size += ProtocolParser.GetSize(i12) + 1;
			}
		}
		if (instance.DecorsIds != null)
		{
			for (int n = 0; n < instance.DecorsIdsCount; n++)
			{
				int i13 = instance.DecorsIds[n];
				size += ProtocolParser.GetSize(i13) + 1;
			}
		}
		if (instance.Compver != 0)
		{
			size += ProtocolParser.GetSize(instance.Compver) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_ServerChunk instance)
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

	public static byte[] SerializeToBytes(Packet_ServerChunk instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_ServerChunk instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
