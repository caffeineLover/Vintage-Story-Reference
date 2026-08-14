using System;

public class Packet_ServerAssetsSerializer
{
	private const int field = 8;

	public static Packet_ServerAssets DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_ServerAssets instance = new Packet_ServerAssets();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_ServerAssets DeserializeBuffer(byte[] buffer, int length, Packet_ServerAssets instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_ServerAssets Deserialize(CitoMemoryStream stream, Packet_ServerAssets instance)
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
				instance.BlocksAdd(Packet_BlockTypeSerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 18:
				instance.ItemsAdd(Packet_ItemTypeSerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 26:
				instance.EntitiesAdd(Packet_EntityTypeSerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 34:
				instance.RecipesAdd(Packet_RecipesSerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 42:
				instance.TagRegistriesAdd(Packet_TagRegistrySerializer.DeserializeLengthDelimitedNew(stream));
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

	public static Packet_ServerAssets DeserializeLengthDelimited(CitoMemoryStream stream, Packet_ServerAssets instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_ServerAssets result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_ServerAssets instance)
	{
		if (instance.Blocks != null)
		{
			Packet_BlockType[] elems = instance.Blocks;
			int elemCount = instance.BlocksCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(10);
				Packet_BlockTypeSerializer.SerializeWithSize(stream, elems[k]);
			}
		}
		if (instance.Items != null)
		{
			Packet_ItemType[] elems2 = instance.Items;
			int elemCount2 = instance.ItemsCount;
			for (int i = 0; i < elems2.Length && i < elemCount2; i++)
			{
				stream.WriteByte(18);
				Packet_ItemTypeSerializer.SerializeWithSize(stream, elems2[i]);
			}
		}
		if (instance.Entities != null)
		{
			Packet_EntityType[] elems3 = instance.Entities;
			int elemCount3 = instance.EntitiesCount;
			for (int j = 0; j < elems3.Length && j < elemCount3; j++)
			{
				stream.WriteByte(26);
				Packet_EntityTypeSerializer.SerializeWithSize(stream, elems3[j]);
			}
		}
		if (instance.Recipes != null)
		{
			Packet_Recipes[] elems4 = instance.Recipes;
			int elemCount4 = instance.RecipesCount;
			for (int l = 0; l < elems4.Length && l < elemCount4; l++)
			{
				stream.WriteByte(34);
				Packet_RecipesSerializer.SerializeWithSize(stream, elems4[l]);
			}
		}
		if (instance.TagRegistries != null)
		{
			Packet_TagRegistry[] elems5 = instance.TagRegistries;
			int elemCount5 = instance.TagRegistriesCount;
			for (int m = 0; m < elems5.Length && m < elemCount5; m++)
			{
				stream.WriteByte(42);
				Packet_TagRegistrySerializer.SerializeWithSize(stream, elems5[m]);
			}
		}
	}

	public static int GetSize(Packet_ServerAssets instance)
	{
		int size = 0;
		if (instance.Blocks != null)
		{
			for (int k = 0; k < instance.BlocksCount; k++)
			{
				int packetlength = Packet_BlockTypeSerializer.GetSize(instance.Blocks[k]);
				size += packetlength + ProtocolParser.GetSize(packetlength) + 1;
			}
		}
		if (instance.Items != null)
		{
			for (int i = 0; i < instance.ItemsCount; i++)
			{
				int packetlength2 = Packet_ItemTypeSerializer.GetSize(instance.Items[i]);
				size += packetlength2 + ProtocolParser.GetSize(packetlength2) + 1;
			}
		}
		if (instance.Entities != null)
		{
			for (int j = 0; j < instance.EntitiesCount; j++)
			{
				int packetlength3 = Packet_EntityTypeSerializer.GetSize(instance.Entities[j]);
				size += packetlength3 + ProtocolParser.GetSize(packetlength3) + 1;
			}
		}
		if (instance.Recipes != null)
		{
			for (int l = 0; l < instance.RecipesCount; l++)
			{
				int packetlength4 = Packet_RecipesSerializer.GetSize(instance.Recipes[l]);
				size += packetlength4 + ProtocolParser.GetSize(packetlength4) + 1;
			}
		}
		if (instance.TagRegistries != null)
		{
			for (int m = 0; m < instance.TagRegistriesCount; m++)
			{
				int packetlength5 = Packet_TagRegistrySerializer.GetSize(instance.TagRegistries[m]);
				size += packetlength5 + ProtocolParser.GetSize(packetlength5) + 1;
			}
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_ServerAssets instance)
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

	public static byte[] SerializeToBytes(Packet_ServerAssets instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_ServerAssets instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
