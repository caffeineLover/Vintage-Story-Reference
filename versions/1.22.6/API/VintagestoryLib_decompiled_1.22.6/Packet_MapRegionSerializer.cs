using System;

public class Packet_MapRegionSerializer
{
	private const int field = 8;

	public static Packet_MapRegion DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_MapRegion instance = new Packet_MapRegion();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_MapRegion DeserializeBuffer(byte[] buffer, int length, Packet_MapRegion instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_MapRegion Deserialize(CitoMemoryStream stream, Packet_MapRegion instance)
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
				instance.RegionX = ProtocolParser.ReadUInt32(stream);
				break;
			case 16:
				instance.RegionZ = ProtocolParser.ReadUInt32(stream);
				break;
			case 26:
				if (instance.LandformMap == null)
				{
					instance.LandformMap = Packet_IntMapSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_IntMapSerializer.DeserializeLengthDelimited(stream, instance.LandformMap);
				}
				break;
			case 34:
				if (instance.ForestMap == null)
				{
					instance.ForestMap = Packet_IntMapSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_IntMapSerializer.DeserializeLengthDelimited(stream, instance.ForestMap);
				}
				break;
			case 42:
				if (instance.ClimateMap == null)
				{
					instance.ClimateMap = Packet_IntMapSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_IntMapSerializer.DeserializeLengthDelimited(stream, instance.ClimateMap);
				}
				break;
			case 50:
				if (instance.GeologicProvinceMap == null)
				{
					instance.GeologicProvinceMap = Packet_IntMapSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_IntMapSerializer.DeserializeLengthDelimited(stream, instance.GeologicProvinceMap);
				}
				break;
			case 58:
				instance.GeneratedStructuresAdd(Packet_GeneratedStructureSerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 66:
				instance.Moddata = ProtocolParser.ReadBytes(stream);
				break;
			case 74:
				if (instance.OceanMap == null)
				{
					instance.OceanMap = Packet_IntMapSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_IntMapSerializer.DeserializeLengthDelimited(stream, instance.OceanMap);
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

	public static Packet_MapRegion DeserializeLengthDelimited(CitoMemoryStream stream, Packet_MapRegion instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_MapRegion result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_MapRegion instance)
	{
		if (instance.RegionX != 0)
		{
			stream.WriteByte(8);
			ProtocolParser.WriteUInt32(stream, instance.RegionX);
		}
		if (instance.RegionZ != 0)
		{
			stream.WriteByte(16);
			ProtocolParser.WriteUInt32(stream, instance.RegionZ);
		}
		if (instance.LandformMap != null)
		{
			stream.WriteByte(26);
			Packet_IntMapSerializer.SerializeWithSize(stream, instance.LandformMap);
		}
		if (instance.ForestMap != null)
		{
			stream.WriteByte(34);
			Packet_IntMapSerializer.SerializeWithSize(stream, instance.ForestMap);
		}
		if (instance.ClimateMap != null)
		{
			stream.WriteByte(42);
			Packet_IntMapSerializer.SerializeWithSize(stream, instance.ClimateMap);
		}
		if (instance.GeologicProvinceMap != null)
		{
			stream.WriteByte(50);
			Packet_IntMapSerializer.SerializeWithSize(stream, instance.GeologicProvinceMap);
		}
		if (instance.GeneratedStructures != null)
		{
			Packet_GeneratedStructure[] elems = instance.GeneratedStructures;
			int elemCount = instance.GeneratedStructuresCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(58);
				Packet_GeneratedStructureSerializer.SerializeWithSize(stream, elems[k]);
			}
		}
		if (instance.Moddata != null)
		{
			stream.WriteByte(66);
			ProtocolParser.WriteBytes(stream, instance.Moddata);
		}
		if (instance.OceanMap != null)
		{
			stream.WriteByte(74);
			Packet_IntMapSerializer.SerializeWithSize(stream, instance.OceanMap);
		}
	}

	public static int GetSize(Packet_MapRegion instance)
	{
		int size = 0;
		if (instance.RegionX != 0)
		{
			size += ProtocolParser.GetSize(instance.RegionX) + 1;
		}
		if (instance.RegionZ != 0)
		{
			size += ProtocolParser.GetSize(instance.RegionZ) + 1;
		}
		if (instance.LandformMap != null)
		{
			int packetlength = Packet_IntMapSerializer.GetSize(instance.LandformMap);
			size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
		}
		if (instance.ForestMap != null)
		{
			int packetlength2 = Packet_IntMapSerializer.GetSize(instance.ForestMap);
			size += packetlength2 + ProtocolParser.GetSize(packetlength2) + 1;
		}
		if (instance.ClimateMap != null)
		{
			int packetlength3 = Packet_IntMapSerializer.GetSize(instance.ClimateMap);
			size += packetlength3 + ProtocolParser.GetSize(packetlength3) + 1;
		}
		if (instance.GeologicProvinceMap != null)
		{
			int packetlength4 = Packet_IntMapSerializer.GetSize(instance.GeologicProvinceMap);
			size += packetlength4 + ProtocolParser.GetSize(packetlength4) + 1;
		}
		if (instance.GeneratedStructures != null)
		{
			for (int k = 0; k < instance.GeneratedStructuresCount; k++)
			{
				int packetlength5 = Packet_GeneratedStructureSerializer.GetSize(instance.GeneratedStructures[k]);
				size += packetlength5 + ProtocolParser.GetSize(packetlength5) + 1;
			}
		}
		if (instance.Moddata != null)
		{
			size += ProtocolParser.GetSize(instance.Moddata) + 1;
		}
		if (instance.OceanMap != null)
		{
			int packetlength6 = Packet_IntMapSerializer.GetSize(instance.OceanMap);
			size += packetlength6 + ProtocolParser.GetSize(packetlength6) + 1;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_MapRegion instance)
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

	public static byte[] SerializeToBytes(Packet_MapRegion instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_MapRegion instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
