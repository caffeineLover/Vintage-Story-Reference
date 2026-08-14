using System;

public class Packet_EntityTypeSerializer
{
	private const int field = 8;

	public static Packet_EntityType DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_EntityType instance = new Packet_EntityType();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_EntityType DeserializeBuffer(byte[] buffer, int length, Packet_EntityType instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_EntityType Deserialize(CitoMemoryStream stream, Packet_EntityType instance)
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
				instance.Code = ProtocolParser.ReadString(stream);
				break;
			case 18:
				instance.Class = ProtocolParser.ReadString(stream);
				break;
			case 298:
				if (instance.Tags == null)
				{
					instance.Tags = Packet_TagSetFastSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_TagSetFastSerializer.DeserializeLengthDelimited(stream, instance.Tags);
				}
				break;
			case 26:
				instance.Renderer = ProtocolParser.ReadString(stream);
				break;
			case 32:
				instance.Habitat = ProtocolParser.ReadUInt32(stream);
				break;
			case 202:
				instance.Drops = ProtocolParser.ReadBytes(stream);
				break;
			case 90:
				if (instance.Shape == null)
				{
					instance.Shape = Packet_CompositeShapeSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_CompositeShapeSerializer.DeserializeLengthDelimited(stream, instance.Shape);
				}
				break;
			case 42:
				instance.BehaviorsAdd(Packet_BehaviorSerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 48:
				instance.CollisionBoxLength = ProtocolParser.ReadUInt32(stream);
				break;
			case 56:
				instance.CollisionBoxHeight = ProtocolParser.ReadUInt32(stream);
				break;
			case 208:
				instance.DeadCollisionBoxLength = ProtocolParser.ReadUInt32(stream);
				break;
			case 216:
				instance.DeadCollisionBoxHeight = ProtocolParser.ReadUInt32(stream);
				break;
			case 256:
				instance.SelectionBoxLength = ProtocolParser.ReadUInt32(stream);
				break;
			case 264:
				instance.SelectionBoxHeight = ProtocolParser.ReadUInt32(stream);
				break;
			case 272:
				instance.DeadSelectionBoxLength = ProtocolParser.ReadUInt32(stream);
				break;
			case 280:
				instance.DeadSelectionBoxHeight = ProtocolParser.ReadUInt32(stream);
				break;
			case 66:
				instance.Attributes = ProtocolParser.ReadString(stream);
				break;
			case 74:
				instance.SoundKeysAdd(ProtocolParser.ReadString(stream));
				break;
			case 82:
				instance.SoundAttributesAdd(Packet_SoundAttributesSerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 112:
				instance.IdleSoundChance = ProtocolParser.ReadUInt32(stream);
				break;
			case 98:
				instance.TextureCodesAdd(ProtocolParser.ReadString(stream));
				break;
			case 106:
				instance.CompositeTexturesAdd(Packet_CompositeTextureSerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 120:
				instance.Size = ProtocolParser.ReadUInt32(stream);
				break;
			case 128:
				instance.EyeHeight = ProtocolParser.ReadUInt32(stream);
				break;
			case 288:
				instance.SwimmingEyeHeight = ProtocolParser.ReadUInt32(stream);
				break;
			case 232:
				instance.Weight = ProtocolParser.ReadUInt32(stream);
				break;
			case 136:
				instance.CanClimb = ProtocolParser.ReadUInt32(stream);
				break;
			case 146:
				instance.AnimationMetaData = ProtocolParser.ReadBytes(stream);
				break;
			case 152:
				instance.KnockbackResistance = ProtocolParser.ReadUInt32(stream);
				break;
			case 160:
				instance.GlowLevel = ProtocolParser.ReadUInt32(stream);
				break;
			case 168:
				instance.CanClimbAnywhere = ProtocolParser.ReadUInt32(stream);
				break;
			case 176:
				instance.ClimbTouchDistance = ProtocolParser.ReadUInt32(stream);
				break;
			case 184:
				instance.RotateModelOnClimb = ProtocolParser.ReadUInt32(stream);
				break;
			case 192:
				instance.FallDamage = ProtocolParser.ReadUInt32(stream);
				break;
			case 312:
				instance.FallDamageMultiplier = ProtocolParser.ReadUInt32(stream);
				break;
			case 226:
				instance.VariantAdd(Packet_VariantPartSerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 240:
				instance.SizeGrowthFactor = ProtocolParser.ReadUInt32(stream);
				break;
			case 248:
				instance.PitchStep = ProtocolParser.ReadUInt32(stream);
				break;
			case 306:
				instance.Color = ProtocolParser.ReadString(stream);
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

	public static Packet_EntityType DeserializeLengthDelimited(CitoMemoryStream stream, Packet_EntityType instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_EntityType result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_EntityType instance)
	{
		if (instance.Code != null)
		{
			stream.WriteByte(10);
			ProtocolParser.WriteString(stream, instance.Code);
		}
		if (instance.Class != null)
		{
			stream.WriteByte(18);
			ProtocolParser.WriteString(stream, instance.Class);
		}
		if (instance.Tags != null)
		{
			stream.WriteKey(37, 2);
			Packet_TagSetFastSerializer.SerializeWithSize(stream, instance.Tags);
		}
		if (instance.Renderer != null)
		{
			stream.WriteByte(26);
			ProtocolParser.WriteString(stream, instance.Renderer);
		}
		if (instance.Habitat != 0)
		{
			stream.WriteByte(32);
			ProtocolParser.WriteUInt32(stream, instance.Habitat);
		}
		if (instance.Drops != null)
		{
			stream.WriteKey(25, 2);
			ProtocolParser.WriteBytes(stream, instance.Drops);
		}
		if (instance.Shape != null)
		{
			stream.WriteByte(90);
			Packet_CompositeShapeSerializer.SerializeWithSize(stream, instance.Shape);
		}
		if (instance.Behaviors != null)
		{
			Packet_Behavior[] elems = instance.Behaviors;
			int elemCount = instance.BehaviorsCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteByte(42);
				Packet_BehaviorSerializer.SerializeWithSize(stream, elems[k]);
			}
		}
		if (instance.CollisionBoxLength != 0)
		{
			stream.WriteByte(48);
			ProtocolParser.WriteUInt32(stream, instance.CollisionBoxLength);
		}
		if (instance.CollisionBoxHeight != 0)
		{
			stream.WriteByte(56);
			ProtocolParser.WriteUInt32(stream, instance.CollisionBoxHeight);
		}
		if (instance.DeadCollisionBoxLength != 0)
		{
			stream.WriteKey(26, 0);
			ProtocolParser.WriteUInt32(stream, instance.DeadCollisionBoxLength);
		}
		if (instance.DeadCollisionBoxHeight != 0)
		{
			stream.WriteKey(27, 0);
			ProtocolParser.WriteUInt32(stream, instance.DeadCollisionBoxHeight);
		}
		if (instance.SelectionBoxLength != 0)
		{
			stream.WriteKey(32, 0);
			ProtocolParser.WriteUInt32(stream, instance.SelectionBoxLength);
		}
		if (instance.SelectionBoxHeight != 0)
		{
			stream.WriteKey(33, 0);
			ProtocolParser.WriteUInt32(stream, instance.SelectionBoxHeight);
		}
		if (instance.DeadSelectionBoxLength != 0)
		{
			stream.WriteKey(34, 0);
			ProtocolParser.WriteUInt32(stream, instance.DeadSelectionBoxLength);
		}
		if (instance.DeadSelectionBoxHeight != 0)
		{
			stream.WriteKey(35, 0);
			ProtocolParser.WriteUInt32(stream, instance.DeadSelectionBoxHeight);
		}
		if (instance.Attributes != null)
		{
			stream.WriteByte(66);
			ProtocolParser.WriteString(stream, instance.Attributes);
		}
		if (instance.SoundKeys != null)
		{
			string[] elems2 = instance.SoundKeys;
			int elemCount2 = instance.SoundKeysCount;
			for (int i = 0; i < elems2.Length && i < elemCount2; i++)
			{
				stream.WriteByte(74);
				ProtocolParser.WriteString(stream, elems2[i]);
			}
		}
		if (instance.SoundAttributes != null)
		{
			Packet_SoundAttributes[] elems3 = instance.SoundAttributes;
			int elemCount3 = instance.SoundAttributesCount;
			for (int j = 0; j < elems3.Length && j < elemCount3; j++)
			{
				stream.WriteByte(82);
				Packet_SoundAttributesSerializer.SerializeWithSize(stream, elems3[j]);
			}
		}
		if (instance.IdleSoundChance != 0)
		{
			stream.WriteByte(112);
			ProtocolParser.WriteUInt32(stream, instance.IdleSoundChance);
		}
		if (instance.TextureCodes != null)
		{
			string[] elems4 = instance.TextureCodes;
			int elemCount4 = instance.TextureCodesCount;
			for (int l = 0; l < elems4.Length && l < elemCount4; l++)
			{
				stream.WriteByte(98);
				ProtocolParser.WriteString(stream, elems4[l]);
			}
		}
		if (instance.CompositeTextures != null)
		{
			Packet_CompositeTexture[] elems5 = instance.CompositeTextures;
			int elemCount5 = instance.CompositeTexturesCount;
			for (int m = 0; m < elems5.Length && m < elemCount5; m++)
			{
				stream.WriteByte(106);
				Packet_CompositeTextureSerializer.SerializeWithSize(stream, elems5[m]);
			}
		}
		if (instance.Size != 0)
		{
			stream.WriteByte(120);
			ProtocolParser.WriteUInt32(stream, instance.Size);
		}
		if (instance.EyeHeight != 0)
		{
			stream.WriteKey(16, 0);
			ProtocolParser.WriteUInt32(stream, instance.EyeHeight);
		}
		if (instance.SwimmingEyeHeight != 0)
		{
			stream.WriteKey(36, 0);
			ProtocolParser.WriteUInt32(stream, instance.SwimmingEyeHeight);
		}
		if (instance.Weight != 0)
		{
			stream.WriteKey(29, 0);
			ProtocolParser.WriteUInt32(stream, instance.Weight);
		}
		if (instance.CanClimb != 0)
		{
			stream.WriteKey(17, 0);
			ProtocolParser.WriteUInt32(stream, instance.CanClimb);
		}
		if (instance.AnimationMetaData != null)
		{
			stream.WriteKey(18, 2);
			ProtocolParser.WriteBytes(stream, instance.AnimationMetaData);
		}
		if (instance.KnockbackResistance != 0)
		{
			stream.WriteKey(19, 0);
			ProtocolParser.WriteUInt32(stream, instance.KnockbackResistance);
		}
		if (instance.GlowLevel != 0)
		{
			stream.WriteKey(20, 0);
			ProtocolParser.WriteUInt32(stream, instance.GlowLevel);
		}
		if (instance.CanClimbAnywhere != 0)
		{
			stream.WriteKey(21, 0);
			ProtocolParser.WriteUInt32(stream, instance.CanClimbAnywhere);
		}
		if (instance.ClimbTouchDistance != 0)
		{
			stream.WriteKey(22, 0);
			ProtocolParser.WriteUInt32(stream, instance.ClimbTouchDistance);
		}
		if (instance.RotateModelOnClimb != 0)
		{
			stream.WriteKey(23, 0);
			ProtocolParser.WriteUInt32(stream, instance.RotateModelOnClimb);
		}
		if (instance.FallDamage != 0)
		{
			stream.WriteKey(24, 0);
			ProtocolParser.WriteUInt32(stream, instance.FallDamage);
		}
		if (instance.FallDamageMultiplier != 0)
		{
			stream.WriteKey(39, 0);
			ProtocolParser.WriteUInt32(stream, instance.FallDamageMultiplier);
		}
		if (instance.Variant != null)
		{
			Packet_VariantPart[] elems6 = instance.Variant;
			int elemCount6 = instance.VariantCount;
			for (int n = 0; n < elems6.Length && n < elemCount6; n++)
			{
				stream.WriteKey(28, 2);
				Packet_VariantPartSerializer.SerializeWithSize(stream, elems6[n]);
			}
		}
		if (instance.SizeGrowthFactor != 0)
		{
			stream.WriteKey(30, 0);
			ProtocolParser.WriteUInt32(stream, instance.SizeGrowthFactor);
		}
		if (instance.PitchStep != 0)
		{
			stream.WriteKey(31, 0);
			ProtocolParser.WriteUInt32(stream, instance.PitchStep);
		}
		if (instance.Color != null)
		{
			stream.WriteKey(38, 2);
			ProtocolParser.WriteString(stream, instance.Color);
		}
	}

	public static int GetSize(Packet_EntityType instance)
	{
		int size = 0;
		if (instance.Code != null)
		{
			size += ProtocolParser.GetSize(instance.Code) + 1;
		}
		if (instance.Class != null)
		{
			size += ProtocolParser.GetSize(instance.Class) + 1;
		}
		if (instance.Tags != null)
		{
			int packetlength = Packet_TagSetFastSerializer.GetSize(instance.Tags);
			size += packetlength + ProtocolParser.GetSize(packetlength) + 2;
		}
		if (instance.Renderer != null)
		{
			size += ProtocolParser.GetSize(instance.Renderer) + 1;
		}
		if (instance.Habitat != 0)
		{
			size += ProtocolParser.GetSize(instance.Habitat) + 1;
		}
		if (instance.Drops != null)
		{
			size += ProtocolParser.GetSize(instance.Drops) + 2;
		}
		if (instance.Shape != null)
		{
			int packetlength2 = Packet_CompositeShapeSerializer.GetSize(instance.Shape);
			size += packetlength2 + ProtocolParser.GetSize(packetlength2) + 1;
		}
		if (instance.Behaviors != null)
		{
			for (int k = 0; k < instance.BehaviorsCount; k++)
			{
				int packetlength3 = Packet_BehaviorSerializer.GetSize(instance.Behaviors[k]);
				size += packetlength3 + ProtocolParser.GetSize(packetlength3) + 1;
			}
		}
		if (instance.CollisionBoxLength != 0)
		{
			size += ProtocolParser.GetSize(instance.CollisionBoxLength) + 1;
		}
		if (instance.CollisionBoxHeight != 0)
		{
			size += ProtocolParser.GetSize(instance.CollisionBoxHeight) + 1;
		}
		if (instance.DeadCollisionBoxLength != 0)
		{
			size += ProtocolParser.GetSize(instance.DeadCollisionBoxLength) + 2;
		}
		if (instance.DeadCollisionBoxHeight != 0)
		{
			size += ProtocolParser.GetSize(instance.DeadCollisionBoxHeight) + 2;
		}
		if (instance.SelectionBoxLength != 0)
		{
			size += ProtocolParser.GetSize(instance.SelectionBoxLength) + 2;
		}
		if (instance.SelectionBoxHeight != 0)
		{
			size += ProtocolParser.GetSize(instance.SelectionBoxHeight) + 2;
		}
		if (instance.DeadSelectionBoxLength != 0)
		{
			size += ProtocolParser.GetSize(instance.DeadSelectionBoxLength) + 2;
		}
		if (instance.DeadSelectionBoxHeight != 0)
		{
			size += ProtocolParser.GetSize(instance.DeadSelectionBoxHeight) + 2;
		}
		if (instance.Attributes != null)
		{
			size += ProtocolParser.GetSize(instance.Attributes) + 1;
		}
		if (instance.SoundKeys != null)
		{
			for (int i = 0; i < instance.SoundKeysCount; i++)
			{
				string i9 = instance.SoundKeys[i];
				size += ProtocolParser.GetSize(i9) + 1;
			}
		}
		if (instance.SoundAttributes != null)
		{
			for (int j = 0; j < instance.SoundAttributesCount; j++)
			{
				int packetlength4 = Packet_SoundAttributesSerializer.GetSize(instance.SoundAttributes[j]);
				size += packetlength4 + ProtocolParser.GetSize(packetlength4) + 1;
			}
		}
		if (instance.IdleSoundChance != 0)
		{
			size += ProtocolParser.GetSize(instance.IdleSoundChance) + 1;
		}
		if (instance.TextureCodes != null)
		{
			for (int l = 0; l < instance.TextureCodesCount; l++)
			{
				string i12 = instance.TextureCodes[l];
				size += ProtocolParser.GetSize(i12) + 1;
			}
		}
		if (instance.CompositeTextures != null)
		{
			for (int m = 0; m < instance.CompositeTexturesCount; m++)
			{
				int packetlength5 = Packet_CompositeTextureSerializer.GetSize(instance.CompositeTextures[m]);
				size += packetlength5 + ProtocolParser.GetSize(packetlength5) + 1;
			}
		}
		if (instance.Size != 0)
		{
			size += ProtocolParser.GetSize(instance.Size) + 1;
		}
		if (instance.EyeHeight != 0)
		{
			size += ProtocolParser.GetSize(instance.EyeHeight) + 2;
		}
		if (instance.SwimmingEyeHeight != 0)
		{
			size += ProtocolParser.GetSize(instance.SwimmingEyeHeight) + 2;
		}
		if (instance.Weight != 0)
		{
			size += ProtocolParser.GetSize(instance.Weight) + 2;
		}
		if (instance.CanClimb != 0)
		{
			size += ProtocolParser.GetSize(instance.CanClimb) + 2;
		}
		if (instance.AnimationMetaData != null)
		{
			size += ProtocolParser.GetSize(instance.AnimationMetaData) + 2;
		}
		if (instance.KnockbackResistance != 0)
		{
			size += ProtocolParser.GetSize(instance.KnockbackResistance) + 2;
		}
		if (instance.GlowLevel != 0)
		{
			size += ProtocolParser.GetSize(instance.GlowLevel) + 2;
		}
		if (instance.CanClimbAnywhere != 0)
		{
			size += ProtocolParser.GetSize(instance.CanClimbAnywhere) + 2;
		}
		if (instance.ClimbTouchDistance != 0)
		{
			size += ProtocolParser.GetSize(instance.ClimbTouchDistance) + 2;
		}
		if (instance.RotateModelOnClimb != 0)
		{
			size += ProtocolParser.GetSize(instance.RotateModelOnClimb) + 2;
		}
		if (instance.FallDamage != 0)
		{
			size += ProtocolParser.GetSize(instance.FallDamage) + 2;
		}
		if (instance.FallDamageMultiplier != 0)
		{
			size += ProtocolParser.GetSize(instance.FallDamageMultiplier) + 2;
		}
		if (instance.Variant != null)
		{
			for (int n = 0; n < instance.VariantCount; n++)
			{
				int packetlength6 = Packet_VariantPartSerializer.GetSize(instance.Variant[n]);
				size += packetlength6 + ProtocolParser.GetSize(packetlength6) + 2;
			}
		}
		if (instance.SizeGrowthFactor != 0)
		{
			size += ProtocolParser.GetSize(instance.SizeGrowthFactor) + 2;
		}
		if (instance.PitchStep != 0)
		{
			size += ProtocolParser.GetSize(instance.PitchStep) + 2;
		}
		if (instance.Color != null)
		{
			size += ProtocolParser.GetSize(instance.Color) + 2;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_EntityType instance)
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

	public static byte[] SerializeToBytes(Packet_EntityType instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_EntityType instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
