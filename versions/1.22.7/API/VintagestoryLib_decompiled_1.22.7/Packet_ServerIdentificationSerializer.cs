using System;

public class Packet_ServerIdentificationSerializer
{
	private const int field = 8;

	public static Packet_ServerIdentification DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_ServerIdentification instance = new Packet_ServerIdentification();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_ServerIdentification DeserializeBuffer(byte[] buffer, int length, Packet_ServerIdentification instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_ServerIdentification Deserialize(CitoMemoryStream stream, Packet_ServerIdentification instance)
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
				instance.NetworkVersion = ProtocolParser.ReadString(stream);
				break;
			case 138:
				instance.GameVersion = ProtocolParser.ReadString(stream);
				break;
			case 26:
				instance.ServerName = ProtocolParser.ReadString(stream);
				break;
			case 56:
				instance.MapSizeX = ProtocolParser.ReadUInt32(stream);
				break;
			case 64:
				instance.MapSizeY = ProtocolParser.ReadUInt32(stream);
				break;
			case 72:
				instance.MapSizeZ = ProtocolParser.ReadUInt32(stream);
				break;
			case 168:
				instance.RegionMapSizeX = ProtocolParser.ReadUInt32(stream);
				break;
			case 176:
				instance.RegionMapSizeY = ProtocolParser.ReadUInt32(stream);
				break;
			case 184:
				instance.RegionMapSizeZ = ProtocolParser.ReadUInt32(stream);
				break;
			case 88:
				instance.DisableShadows = ProtocolParser.ReadUInt32(stream);
				break;
			case 96:
				instance.PlayerAreaSize = ProtocolParser.ReadUInt32(stream);
				break;
			case 104:
				instance.Seed = ProtocolParser.ReadUInt32(stream);
				break;
			case 130:
				instance.PlayStyle = ProtocolParser.ReadString(stream);
				break;
			case 144:
				instance.RequireRemapping = ProtocolParser.ReadUInt32(stream);
				break;
			case 154:
				instance.ModsAdd(Packet_ModIdSerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 162:
				instance.WorldConfiguration = ProtocolParser.ReadBytes(stream);
				break;
			case 194:
				instance.SavegameIdentifier = ProtocolParser.ReadString(stream);
				break;
			case 202:
				instance.PlayListCode = ProtocolParser.ReadString(stream);
				break;
			case 210:
				instance.ServerModIdBlackListAdd(ProtocolParser.ReadString(stream));
				break;
			case 218:
				instance.ServerModIdWhiteListAdd(ProtocolParser.ReadString(stream));
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

	public static Packet_ServerIdentification DeserializeLengthDelimited(CitoMemoryStream stream, Packet_ServerIdentification instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_ServerIdentification result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_ServerIdentification instance)
	{
		if (instance.NetworkVersion != null)
		{
			stream.WriteByte(10);
			ProtocolParser.WriteString(stream, instance.NetworkVersion);
		}
		if (instance.GameVersion != null)
		{
			stream.WriteKey(17, 2);
			ProtocolParser.WriteString(stream, instance.GameVersion);
		}
		if (instance.ServerName != null)
		{
			stream.WriteByte(26);
			ProtocolParser.WriteString(stream, instance.ServerName);
		}
		if (instance.MapSizeX != 0)
		{
			stream.WriteByte(56);
			ProtocolParser.WriteUInt32(stream, instance.MapSizeX);
		}
		if (instance.MapSizeY != 0)
		{
			stream.WriteByte(64);
			ProtocolParser.WriteUInt32(stream, instance.MapSizeY);
		}
		if (instance.MapSizeZ != 0)
		{
			stream.WriteByte(72);
			ProtocolParser.WriteUInt32(stream, instance.MapSizeZ);
		}
		if (instance.RegionMapSizeX != 0)
		{
			stream.WriteKey(21, 0);
			ProtocolParser.WriteUInt32(stream, instance.RegionMapSizeX);
		}
		if (instance.RegionMapSizeY != 0)
		{
			stream.WriteKey(22, 0);
			ProtocolParser.WriteUInt32(stream, instance.RegionMapSizeY);
		}
		if (instance.RegionMapSizeZ != 0)
		{
			stream.WriteKey(23, 0);
			ProtocolParser.WriteUInt32(stream, instance.RegionMapSizeZ);
		}
		if (instance.DisableShadows != 0)
		{
			stream.WriteByte(88);
			ProtocolParser.WriteUInt32(stream, instance.DisableShadows);
		}
		if (instance.PlayerAreaSize != 0)
		{
			stream.WriteByte(96);
			ProtocolParser.WriteUInt32(stream, instance.PlayerAreaSize);
		}
		if (instance.Seed != 0)
		{
			stream.WriteByte(104);
			ProtocolParser.WriteUInt32(stream, instance.Seed);
		}
		if (instance.PlayStyle != null)
		{
			stream.WriteKey(16, 2);
			ProtocolParser.WriteString(stream, instance.PlayStyle);
		}
		if (instance.RequireRemapping != 0)
		{
			stream.WriteKey(18, 0);
			ProtocolParser.WriteUInt32(stream, instance.RequireRemapping);
		}
		if (instance.Mods != null)
		{
			Packet_ModId[] elems = instance.Mods;
			int elemCount = instance.ModsCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteKey(19, 2);
				Packet_ModIdSerializer.SerializeWithSize(stream, elems[k]);
			}
		}
		if (instance.WorldConfiguration != null)
		{
			stream.WriteKey(20, 2);
			ProtocolParser.WriteBytes(stream, instance.WorldConfiguration);
		}
		if (instance.SavegameIdentifier != null)
		{
			stream.WriteKey(24, 2);
			ProtocolParser.WriteString(stream, instance.SavegameIdentifier);
		}
		if (instance.PlayListCode != null)
		{
			stream.WriteKey(25, 2);
			ProtocolParser.WriteString(stream, instance.PlayListCode);
		}
		if (instance.ServerModIdBlackList != null)
		{
			string[] elems2 = instance.ServerModIdBlackList;
			int elemCount2 = instance.ServerModIdBlackListCount;
			for (int i = 0; i < elems2.Length && i < elemCount2; i++)
			{
				stream.WriteKey(26, 2);
				ProtocolParser.WriteString(stream, elems2[i]);
			}
		}
		if (instance.ServerModIdWhiteList != null)
		{
			string[] elems3 = instance.ServerModIdWhiteList;
			int elemCount3 = instance.ServerModIdWhiteListCount;
			for (int j = 0; j < elems3.Length && j < elemCount3; j++)
			{
				stream.WriteKey(27, 2);
				ProtocolParser.WriteString(stream, elems3[j]);
			}
		}
	}

	public static int GetSize(Packet_ServerIdentification instance)
	{
		int size = 0;
		if (instance.NetworkVersion != null)
		{
			size += ProtocolParser.GetSize(instance.NetworkVersion) + 1;
		}
		if (instance.GameVersion != null)
		{
			size += ProtocolParser.GetSize(instance.GameVersion) + 2;
		}
		if (instance.ServerName != null)
		{
			size += ProtocolParser.GetSize(instance.ServerName) + 1;
		}
		if (instance.MapSizeX != 0)
		{
			size += ProtocolParser.GetSize(instance.MapSizeX) + 1;
		}
		if (instance.MapSizeY != 0)
		{
			size += ProtocolParser.GetSize(instance.MapSizeY) + 1;
		}
		if (instance.MapSizeZ != 0)
		{
			size += ProtocolParser.GetSize(instance.MapSizeZ) + 1;
		}
		if (instance.RegionMapSizeX != 0)
		{
			size += ProtocolParser.GetSize(instance.RegionMapSizeX) + 2;
		}
		if (instance.RegionMapSizeY != 0)
		{
			size += ProtocolParser.GetSize(instance.RegionMapSizeY) + 2;
		}
		if (instance.RegionMapSizeZ != 0)
		{
			size += ProtocolParser.GetSize(instance.RegionMapSizeZ) + 2;
		}
		if (instance.DisableShadows != 0)
		{
			size += ProtocolParser.GetSize(instance.DisableShadows) + 1;
		}
		if (instance.PlayerAreaSize != 0)
		{
			size += ProtocolParser.GetSize(instance.PlayerAreaSize) + 1;
		}
		if (instance.Seed != 0)
		{
			size += ProtocolParser.GetSize(instance.Seed) + 1;
		}
		if (instance.PlayStyle != null)
		{
			size += ProtocolParser.GetSize(instance.PlayStyle) + 2;
		}
		if (instance.RequireRemapping != 0)
		{
			size += ProtocolParser.GetSize(instance.RequireRemapping) + 2;
		}
		if (instance.Mods != null)
		{
			for (int k = 0; k < instance.ModsCount; k++)
			{
				int packetlength = Packet_ModIdSerializer.GetSize(instance.Mods[k]);
				size += packetlength + ProtocolParser.GetSize(packetlength) + 2;
			}
		}
		if (instance.WorldConfiguration != null)
		{
			size += ProtocolParser.GetSize(instance.WorldConfiguration) + 2;
		}
		if (instance.SavegameIdentifier != null)
		{
			size += ProtocolParser.GetSize(instance.SavegameIdentifier) + 2;
		}
		if (instance.PlayListCode != null)
		{
			size += ProtocolParser.GetSize(instance.PlayListCode) + 2;
		}
		if (instance.ServerModIdBlackList != null)
		{
			for (int i = 0; i < instance.ServerModIdBlackListCount; i++)
			{
				string i26 = instance.ServerModIdBlackList[i];
				size += ProtocolParser.GetSize(i26) + 2;
			}
		}
		if (instance.ServerModIdWhiteList != null)
		{
			for (int j = 0; j < instance.ServerModIdWhiteListCount; j++)
			{
				string i27 = instance.ServerModIdWhiteList[j];
				size += ProtocolParser.GetSize(i27) + 2;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_ServerIdentification instance)
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

	public static byte[] SerializeToBytes(Packet_ServerIdentification instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_ServerIdentification instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
