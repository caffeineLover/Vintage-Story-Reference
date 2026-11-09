using System;

public class Packet_ItemTypeSerializer
{
	private const int field = 8;

	public static Packet_ItemType DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_ItemType instance = new Packet_ItemType();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_ItemType DeserializeBuffer(byte[] buffer, int length, Packet_ItemType instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_ItemType Deserialize(CitoMemoryStream stream, Packet_ItemType instance)
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
				instance.ItemId = ProtocolParser.ReadUInt32(stream);
				break;
			case 16:
				instance.MaxStackSize = ProtocolParser.ReadUInt32(stream);
				break;
			case 26:
				instance.Code = ProtocolParser.ReadString(stream);
				break;
			case 384:
				instance.TagsAdd(ProtocolParser.ReadUInt32(stream));
				break;
			case 314:
				instance.BehaviorsAdd(Packet_BehaviorSerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 34:
				instance.CompositeTexturesAdd(Packet_CompositeTextureSerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 40:
				instance.Durability = ProtocolParser.ReadUInt32(stream);
				break;
			case 48:
				instance.MiningmaterialAdd(ProtocolParser.ReadUInt32(stream));
				break;
			case 248:
				instance.MiningmaterialspeedAdd(ProtocolParser.ReadUInt32(stream));
				break;
			case 56:
				instance.DamagedbyAdd(ProtocolParser.ReadUInt32(stream));
				break;
			case 66:
				instance.CreativeInventoryStacks = ProtocolParser.ReadBytes(stream);
				break;
			case 74:
				instance.CreativeInventoryTabsAdd(ProtocolParser.ReadString(stream));
				break;
			case 82:
				if (instance.GuiTransform == null)
				{
					instance.GuiTransform = Packet_ModelTransformSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ModelTransformSerializer.DeserializeLengthDelimited(stream, instance.GuiTransform);
				}
				break;
			case 90:
				if (instance.FpHandTransform == null)
				{
					instance.FpHandTransform = Packet_ModelTransformSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ModelTransformSerializer.DeserializeLengthDelimited(stream, instance.FpHandTransform);
				}
				break;
			case 98:
				if (instance.TpHandTransform == null)
				{
					instance.TpHandTransform = Packet_ModelTransformSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ModelTransformSerializer.DeserializeLengthDelimited(stream, instance.TpHandTransform);
				}
				break;
			case 346:
				if (instance.TpOffHandTransform == null)
				{
					instance.TpOffHandTransform = Packet_ModelTransformSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ModelTransformSerializer.DeserializeLengthDelimited(stream, instance.TpOffHandTransform);
				}
				break;
			case 178:
				if (instance.GroundTransform == null)
				{
					instance.GroundTransform = Packet_ModelTransformSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ModelTransformSerializer.DeserializeLengthDelimited(stream, instance.GroundTransform);
				}
				break;
			case 106:
				instance.Attributes = ProtocolParser.ReadString(stream);
				break;
			case 114:
				if (instance.CombustibleProps == null)
				{
					instance.CombustibleProps = Packet_CombustiblePropertiesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_CombustiblePropertiesSerializer.DeserializeLengthDelimited(stream, instance.CombustibleProps);
				}
				break;
			case 122:
				if (instance.NutritionProps == null)
				{
					instance.NutritionProps = Packet_NutritionPropertiesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_NutritionPropertiesSerializer.DeserializeLengthDelimited(stream, instance.NutritionProps);
				}
				break;
			case 258:
				if (instance.GrindingProps == null)
				{
					instance.GrindingProps = Packet_GrindingPropertiesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_GrindingPropertiesSerializer.DeserializeLengthDelimited(stream, instance.GrindingProps);
				}
				break;
			case 306:
				if (instance.CrushingProps == null)
				{
					instance.CrushingProps = Packet_CrushingPropertiesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_CrushingPropertiesSerializer.DeserializeLengthDelimited(stream, instance.CrushingProps);
				}
				break;
			case 290:
				instance.TransitionablePropsAdd(Packet_TransitionablePropertiesSerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 130:
				if (instance.Shape == null)
				{
					instance.Shape = Packet_CompositeShapeSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_CompositeShapeSerializer.DeserializeLengthDelimited(stream, instance.Shape);
				}
				break;
			case 138:
				instance.TextureCodesAdd(ProtocolParser.ReadString(stream));
				break;
			case 146:
				instance.ItemClass = ProtocolParser.ReadString(stream);
				break;
			case 152:
				instance.Tool = ProtocolParser.ReadUInt32(stream);
				break;
			case 160:
				instance.MaterialDensity = ProtocolParser.ReadUInt32(stream);
				break;
			case 168:
				instance.AttackPower = ProtocolParser.ReadUInt32(stream);
				break;
			case 200:
				instance.AttackRange = ProtocolParser.ReadUInt32(stream);
				break;
			case 184:
				instance.LiquidSelectable = ProtocolParser.ReadUInt32(stream);
				break;
			case 192:
				instance.MiningTier = ProtocolParser.ReadUInt32(stream);
				break;
			case 208:
				instance.StorageFlags = ProtocolParser.ReadUInt32(stream);
				break;
			case 216:
				instance.RenderAlphaTest = ProtocolParser.ReadUInt32(stream);
				break;
			case 226:
				instance.HeldTpHitAnimation = ProtocolParser.ReadString(stream);
				break;
			case 234:
				instance.HeldRightTpIdleAnimation = ProtocolParser.ReadString(stream);
				break;
			case 274:
				instance.HeldLeftTpIdleAnimation = ProtocolParser.ReadString(stream);
				break;
			case 242:
				instance.HeldTpUseAnimation = ProtocolParser.ReadString(stream);
				break;
			case 264:
				instance.MatterState = ProtocolParser.ReadUInt32(stream);
				break;
			case 282:
				instance.VariantAdd(Packet_VariantPartSerializer.DeserializeLengthDelimitedNew(stream));
				break;
			case 298:
				if (instance.HeldSounds == null)
				{
					instance.HeldSounds = Packet_HeldSoundSetSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_HeldSoundSetSerializer.DeserializeLengthDelimited(stream, instance.HeldSounds);
				}
				break;
			case 320:
				instance.Width = ProtocolParser.ReadUInt32(stream);
				break;
			case 328:
				instance.Height = ProtocolParser.ReadUInt32(stream);
				break;
			case 336:
				instance.Length = ProtocolParser.ReadUInt32(stream);
				break;
			case 352:
				instance.LightHsvAdd(ProtocolParser.ReadUInt32(stream));
				break;
			case 360:
				instance.IsMissing = ProtocolParser.ReadUInt32(stream);
				break;
			case 370:
				instance.HeldLeftReadyAnimation = ProtocolParser.ReadString(stream);
				break;
			case 378:
				instance.HeldRightReadyAnimation = ProtocolParser.ReadString(stream);
				break;
			case 394:
				instance.ParticlesTextureCode = ProtocolParser.ReadString(stream);
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

	public static Packet_ItemType DeserializeLengthDelimited(CitoMemoryStream stream, Packet_ItemType instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_ItemType result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_ItemType instance)
	{
		if (instance.ItemId != 0)
		{
			stream.WriteByte(8);
			ProtocolParser.WriteUInt32(stream, instance.ItemId);
		}
		if (instance.MaxStackSize != 0)
		{
			stream.WriteByte(16);
			ProtocolParser.WriteUInt32(stream, instance.MaxStackSize);
		}
		if (instance.Code != null)
		{
			stream.WriteByte(26);
			ProtocolParser.WriteString(stream, instance.Code);
		}
		if (instance.Tags != null)
		{
			int[] elems = instance.Tags;
			int elemCount = instance.TagsCount;
			for (int k = 0; k < elems.Length && k < elemCount; k++)
			{
				stream.WriteKey(48, 0);
				ProtocolParser.WriteUInt32(stream, elems[k]);
			}
		}
		if (instance.Behaviors != null)
		{
			Packet_Behavior[] elems2 = instance.Behaviors;
			int elemCount2 = instance.BehaviorsCount;
			for (int i = 0; i < elems2.Length && i < elemCount2; i++)
			{
				stream.WriteKey(39, 2);
				Packet_BehaviorSerializer.SerializeWithSize(stream, elems2[i]);
			}
		}
		if (instance.CompositeTextures != null)
		{
			Packet_CompositeTexture[] elems3 = instance.CompositeTextures;
			int elemCount3 = instance.CompositeTexturesCount;
			for (int j = 0; j < elems3.Length && j < elemCount3; j++)
			{
				stream.WriteByte(34);
				Packet_CompositeTextureSerializer.SerializeWithSize(stream, elems3[j]);
			}
		}
		if (instance.Durability != 0)
		{
			stream.WriteByte(40);
			ProtocolParser.WriteUInt32(stream, instance.Durability);
		}
		if (instance.Miningmaterial != null)
		{
			int[] elems4 = instance.Miningmaterial;
			int elemCount4 = instance.MiningmaterialCount;
			for (int l = 0; l < elems4.Length && l < elemCount4; l++)
			{
				stream.WriteByte(48);
				ProtocolParser.WriteUInt32(stream, elems4[l]);
			}
		}
		if (instance.Miningmaterialspeed != null)
		{
			int[] elems5 = instance.Miningmaterialspeed;
			int elemCount5 = instance.MiningmaterialspeedCount;
			for (int m = 0; m < elems5.Length && m < elemCount5; m++)
			{
				stream.WriteKey(31, 0);
				ProtocolParser.WriteUInt32(stream, elems5[m]);
			}
		}
		if (instance.Damagedby != null)
		{
			int[] elems6 = instance.Damagedby;
			int elemCount6 = instance.DamagedbyCount;
			for (int n = 0; n < elems6.Length && n < elemCount6; n++)
			{
				stream.WriteByte(56);
				ProtocolParser.WriteUInt32(stream, elems6[n]);
			}
		}
		if (instance.CreativeInventoryStacks != null)
		{
			stream.WriteByte(66);
			ProtocolParser.WriteBytes(stream, instance.CreativeInventoryStacks);
		}
		if (instance.CreativeInventoryTabs != null)
		{
			string[] elems7 = instance.CreativeInventoryTabs;
			int elemCount7 = instance.CreativeInventoryTabsCount;
			for (int num = 0; num < elems7.Length && num < elemCount7; num++)
			{
				stream.WriteByte(74);
				ProtocolParser.WriteString(stream, elems7[num]);
			}
		}
		if (instance.GuiTransform != null)
		{
			stream.WriteByte(82);
			Packet_ModelTransformSerializer.SerializeWithSize(stream, instance.GuiTransform);
		}
		if (instance.FpHandTransform != null)
		{
			stream.WriteByte(90);
			Packet_ModelTransformSerializer.SerializeWithSize(stream, instance.FpHandTransform);
		}
		if (instance.TpHandTransform != null)
		{
			stream.WriteByte(98);
			Packet_ModelTransformSerializer.SerializeWithSize(stream, instance.TpHandTransform);
		}
		if (instance.TpOffHandTransform != null)
		{
			stream.WriteKey(43, 2);
			Packet_ModelTransformSerializer.SerializeWithSize(stream, instance.TpOffHandTransform);
		}
		if (instance.GroundTransform != null)
		{
			stream.WriteKey(22, 2);
			Packet_ModelTransformSerializer.SerializeWithSize(stream, instance.GroundTransform);
		}
		if (instance.Attributes != null)
		{
			stream.WriteByte(106);
			ProtocolParser.WriteString(stream, instance.Attributes);
		}
		if (instance.CombustibleProps != null)
		{
			stream.WriteByte(114);
			Packet_CombustiblePropertiesSerializer.SerializeWithSize(stream, instance.CombustibleProps);
		}
		if (instance.NutritionProps != null)
		{
			stream.WriteByte(122);
			Packet_NutritionPropertiesSerializer.SerializeWithSize(stream, instance.NutritionProps);
		}
		if (instance.GrindingProps != null)
		{
			stream.WriteKey(32, 2);
			Packet_GrindingPropertiesSerializer.SerializeWithSize(stream, instance.GrindingProps);
		}
		if (instance.CrushingProps != null)
		{
			stream.WriteKey(38, 2);
			Packet_CrushingPropertiesSerializer.SerializeWithSize(stream, instance.CrushingProps);
		}
		if (instance.TransitionableProps != null)
		{
			Packet_TransitionableProperties[] elems8 = instance.TransitionableProps;
			int elemCount8 = instance.TransitionablePropsCount;
			for (int num2 = 0; num2 < elems8.Length && num2 < elemCount8; num2++)
			{
				stream.WriteKey(36, 2);
				Packet_TransitionablePropertiesSerializer.SerializeWithSize(stream, elems8[num2]);
			}
		}
		if (instance.Shape != null)
		{
			stream.WriteKey(16, 2);
			Packet_CompositeShapeSerializer.SerializeWithSize(stream, instance.Shape);
		}
		if (instance.TextureCodes != null)
		{
			string[] elems9 = instance.TextureCodes;
			int elemCount9 = instance.TextureCodesCount;
			for (int num3 = 0; num3 < elems9.Length && num3 < elemCount9; num3++)
			{
				stream.WriteKey(17, 2);
				ProtocolParser.WriteString(stream, elems9[num3]);
			}
		}
		if (instance.ItemClass != null)
		{
			stream.WriteKey(18, 2);
			ProtocolParser.WriteString(stream, instance.ItemClass);
		}
		if (instance.Tool != 0)
		{
			stream.WriteKey(19, 0);
			ProtocolParser.WriteUInt32(stream, instance.Tool);
		}
		if (instance.MaterialDensity != 0)
		{
			stream.WriteKey(20, 0);
			ProtocolParser.WriteUInt32(stream, instance.MaterialDensity);
		}
		if (instance.AttackPower != 0)
		{
			stream.WriteKey(21, 0);
			ProtocolParser.WriteUInt32(stream, instance.AttackPower);
		}
		if (instance.AttackRange != 0)
		{
			stream.WriteKey(25, 0);
			ProtocolParser.WriteUInt32(stream, instance.AttackRange);
		}
		if (instance.LiquidSelectable != 0)
		{
			stream.WriteKey(23, 0);
			ProtocolParser.WriteUInt32(stream, instance.LiquidSelectable);
		}
		if (instance.MiningTier != 0)
		{
			stream.WriteKey(24, 0);
			ProtocolParser.WriteUInt32(stream, instance.MiningTier);
		}
		if (instance.StorageFlags != 0)
		{
			stream.WriteKey(26, 0);
			ProtocolParser.WriteUInt32(stream, instance.StorageFlags);
		}
		if (instance.RenderAlphaTest != 0)
		{
			stream.WriteKey(27, 0);
			ProtocolParser.WriteUInt32(stream, instance.RenderAlphaTest);
		}
		if (instance.HeldTpHitAnimation != null)
		{
			stream.WriteKey(28, 2);
			ProtocolParser.WriteString(stream, instance.HeldTpHitAnimation);
		}
		if (instance.HeldRightTpIdleAnimation != null)
		{
			stream.WriteKey(29, 2);
			ProtocolParser.WriteString(stream, instance.HeldRightTpIdleAnimation);
		}
		if (instance.HeldLeftTpIdleAnimation != null)
		{
			stream.WriteKey(34, 2);
			ProtocolParser.WriteString(stream, instance.HeldLeftTpIdleAnimation);
		}
		if (instance.HeldTpUseAnimation != null)
		{
			stream.WriteKey(30, 2);
			ProtocolParser.WriteString(stream, instance.HeldTpUseAnimation);
		}
		if (instance.MatterState != 0)
		{
			stream.WriteKey(33, 0);
			ProtocolParser.WriteUInt32(stream, instance.MatterState);
		}
		if (instance.Variant != null)
		{
			Packet_VariantPart[] elems10 = instance.Variant;
			int elemCount10 = instance.VariantCount;
			for (int num4 = 0; num4 < elems10.Length && num4 < elemCount10; num4++)
			{
				stream.WriteKey(35, 2);
				Packet_VariantPartSerializer.SerializeWithSize(stream, elems10[num4]);
			}
		}
		if (instance.HeldSounds != null)
		{
			stream.WriteKey(37, 2);
			Packet_HeldSoundSetSerializer.SerializeWithSize(stream, instance.HeldSounds);
		}
		if (instance.Width != 0)
		{
			stream.WriteKey(40, 0);
			ProtocolParser.WriteUInt32(stream, instance.Width);
		}
		if (instance.Height != 0)
		{
			stream.WriteKey(41, 0);
			ProtocolParser.WriteUInt32(stream, instance.Height);
		}
		if (instance.Length != 0)
		{
			stream.WriteKey(42, 0);
			ProtocolParser.WriteUInt32(stream, instance.Length);
		}
		if (instance.LightHsv != null)
		{
			int[] elems11 = instance.LightHsv;
			int elemCount11 = instance.LightHsvCount;
			for (int num5 = 0; num5 < elems11.Length && num5 < elemCount11; num5++)
			{
				stream.WriteKey(44, 0);
				ProtocolParser.WriteUInt32(stream, elems11[num5]);
			}
		}
		if (instance.IsMissing != 0)
		{
			stream.WriteKey(45, 0);
			ProtocolParser.WriteUInt32(stream, instance.IsMissing);
		}
		if (instance.HeldLeftReadyAnimation != null)
		{
			stream.WriteKey(46, 2);
			ProtocolParser.WriteString(stream, instance.HeldLeftReadyAnimation);
		}
		if (instance.HeldRightReadyAnimation != null)
		{
			stream.WriteKey(47, 2);
			ProtocolParser.WriteString(stream, instance.HeldRightReadyAnimation);
		}
		if (instance.ParticlesTextureCode != null)
		{
			stream.WriteKey(49, 2);
			ProtocolParser.WriteString(stream, instance.ParticlesTextureCode);
		}
	}

	public static int GetSize(Packet_ItemType instance)
	{
		int size = 0;
		if (instance.ItemId != 0)
		{
			size += ProtocolParser.GetSize(instance.ItemId) + 1;
		}
		if (instance.MaxStackSize != 0)
		{
			size += ProtocolParser.GetSize(instance.MaxStackSize) + 1;
		}
		if (instance.Code != null)
		{
			size += ProtocolParser.GetSize(instance.Code) + 1;
		}
		if (instance.Tags != null)
		{
			for (int k = 0; k < instance.TagsCount; k++)
			{
				int i48 = instance.Tags[k];
				size += ProtocolParser.GetSize(i48) + 2;
			}
		}
		if (instance.Behaviors != null)
		{
			for (int j = 0; j < instance.BehaviorsCount; j++)
			{
				int packetlength = Packet_BehaviorSerializer.GetSize(instance.Behaviors[j]);
				size += packetlength + ProtocolParser.GetSize(packetlength) + 2;
			}
		}
		if (instance.CompositeTextures != null)
		{
			for (int l = 0; l < instance.CompositeTexturesCount; l++)
			{
				int packetlength2 = Packet_CompositeTextureSerializer.GetSize(instance.CompositeTextures[l]);
				size += packetlength2 + ProtocolParser.GetSize(packetlength2) + 1;
			}
		}
		if (instance.Durability != 0)
		{
			size += ProtocolParser.GetSize(instance.Durability) + 1;
		}
		if (instance.Miningmaterial != null)
		{
			for (int m = 0; m < instance.MiningmaterialCount; m++)
			{
				int i49 = instance.Miningmaterial[m];
				size += ProtocolParser.GetSize(i49) + 1;
			}
		}
		if (instance.Miningmaterialspeed != null)
		{
			for (int n = 0; n < instance.MiningmaterialspeedCount; n++)
			{
				int i50 = instance.Miningmaterialspeed[n];
				size += ProtocolParser.GetSize(i50) + 2;
			}
		}
		if (instance.Damagedby != null)
		{
			for (int num = 0; num < instance.DamagedbyCount; num++)
			{
				int i51 = instance.Damagedby[num];
				size += ProtocolParser.GetSize(i51) + 1;
			}
		}
		if (instance.CreativeInventoryStacks != null)
		{
			size += ProtocolParser.GetSize(instance.CreativeInventoryStacks) + 1;
		}
		if (instance.CreativeInventoryTabs != null)
		{
			for (int num2 = 0; num2 < instance.CreativeInventoryTabsCount; num2++)
			{
				string i52 = instance.CreativeInventoryTabs[num2];
				size += ProtocolParser.GetSize(i52) + 1;
			}
		}
		if (instance.GuiTransform != null)
		{
			int packetlength3 = Packet_ModelTransformSerializer.GetSize(instance.GuiTransform);
			size += packetlength3 + ProtocolParser.GetSize(packetlength3) + 1;
		}
		if (instance.FpHandTransform != null)
		{
			int packetlength4 = Packet_ModelTransformSerializer.GetSize(instance.FpHandTransform);
			size += packetlength4 + ProtocolParser.GetSize(packetlength4) + 1;
		}
		if (instance.TpHandTransform != null)
		{
			int packetlength5 = Packet_ModelTransformSerializer.GetSize(instance.TpHandTransform);
			size += packetlength5 + ProtocolParser.GetSize(packetlength5) + 1;
		}
		if (instance.TpOffHandTransform != null)
		{
			int packetlength6 = Packet_ModelTransformSerializer.GetSize(instance.TpOffHandTransform);
			size += packetlength6 + ProtocolParser.GetSize(packetlength6) + 2;
		}
		if (instance.GroundTransform != null)
		{
			int packetlength7 = Packet_ModelTransformSerializer.GetSize(instance.GroundTransform);
			size += packetlength7 + ProtocolParser.GetSize(packetlength7) + 2;
		}
		if (instance.Attributes != null)
		{
			size += ProtocolParser.GetSize(instance.Attributes) + 1;
		}
		if (instance.CombustibleProps != null)
		{
			int packetlength8 = Packet_CombustiblePropertiesSerializer.GetSize(instance.CombustibleProps);
			size += packetlength8 + ProtocolParser.GetSize(packetlength8) + 1;
		}
		if (instance.NutritionProps != null)
		{
			int packetlength9 = Packet_NutritionPropertiesSerializer.GetSize(instance.NutritionProps);
			size += packetlength9 + ProtocolParser.GetSize(packetlength9) + 1;
		}
		if (instance.GrindingProps != null)
		{
			int packetlength10 = Packet_GrindingPropertiesSerializer.GetSize(instance.GrindingProps);
			size += packetlength10 + ProtocolParser.GetSize(packetlength10) + 2;
		}
		if (instance.CrushingProps != null)
		{
			int packetlength11 = Packet_CrushingPropertiesSerializer.GetSize(instance.CrushingProps);
			size += packetlength11 + ProtocolParser.GetSize(packetlength11) + 2;
		}
		if (instance.TransitionableProps != null)
		{
			for (int num3 = 0; num3 < instance.TransitionablePropsCount; num3++)
			{
				int packetlength12 = Packet_TransitionablePropertiesSerializer.GetSize(instance.TransitionableProps[num3]);
				size += packetlength12 + ProtocolParser.GetSize(packetlength12) + 2;
			}
		}
		if (instance.Shape != null)
		{
			int packetlength13 = Packet_CompositeShapeSerializer.GetSize(instance.Shape);
			size += packetlength13 + ProtocolParser.GetSize(packetlength13) + 2;
		}
		if (instance.TextureCodes != null)
		{
			for (int num4 = 0; num4 < instance.TextureCodesCount; num4++)
			{
				string i53 = instance.TextureCodes[num4];
				size += ProtocolParser.GetSize(i53) + 2;
			}
		}
		if (instance.ItemClass != null)
		{
			size += ProtocolParser.GetSize(instance.ItemClass) + 2;
		}
		if (instance.Tool != 0)
		{
			size += ProtocolParser.GetSize(instance.Tool) + 2;
		}
		if (instance.MaterialDensity != 0)
		{
			size += ProtocolParser.GetSize(instance.MaterialDensity) + 2;
		}
		if (instance.AttackPower != 0)
		{
			size += ProtocolParser.GetSize(instance.AttackPower) + 2;
		}
		if (instance.AttackRange != 0)
		{
			size += ProtocolParser.GetSize(instance.AttackRange) + 2;
		}
		if (instance.LiquidSelectable != 0)
		{
			size += ProtocolParser.GetSize(instance.LiquidSelectable) + 2;
		}
		if (instance.MiningTier != 0)
		{
			size += ProtocolParser.GetSize(instance.MiningTier) + 2;
		}
		if (instance.StorageFlags != 0)
		{
			size += ProtocolParser.GetSize(instance.StorageFlags) + 2;
		}
		if (instance.RenderAlphaTest != 0)
		{
			size += ProtocolParser.GetSize(instance.RenderAlphaTest) + 2;
		}
		if (instance.HeldTpHitAnimation != null)
		{
			size += ProtocolParser.GetSize(instance.HeldTpHitAnimation) + 2;
		}
		if (instance.HeldRightTpIdleAnimation != null)
		{
			size += ProtocolParser.GetSize(instance.HeldRightTpIdleAnimation) + 2;
		}
		if (instance.HeldLeftTpIdleAnimation != null)
		{
			size += ProtocolParser.GetSize(instance.HeldLeftTpIdleAnimation) + 2;
		}
		if (instance.HeldTpUseAnimation != null)
		{
			size += ProtocolParser.GetSize(instance.HeldTpUseAnimation) + 2;
		}
		if (instance.MatterState != 0)
		{
			size += ProtocolParser.GetSize(instance.MatterState) + 2;
		}
		if (instance.Variant != null)
		{
			for (int num5 = 0; num5 < instance.VariantCount; num5++)
			{
				int packetlength14 = Packet_VariantPartSerializer.GetSize(instance.Variant[num5]);
				size += packetlength14 + ProtocolParser.GetSize(packetlength14) + 2;
			}
		}
		if (instance.HeldSounds != null)
		{
			int packetlength15 = Packet_HeldSoundSetSerializer.GetSize(instance.HeldSounds);
			size += packetlength15 + ProtocolParser.GetSize(packetlength15) + 2;
		}
		if (instance.Width != 0)
		{
			size += ProtocolParser.GetSize(instance.Width) + 2;
		}
		if (instance.Height != 0)
		{
			size += ProtocolParser.GetSize(instance.Height) + 2;
		}
		if (instance.Length != 0)
		{
			size += ProtocolParser.GetSize(instance.Length) + 2;
		}
		if (instance.LightHsv != null)
		{
			for (int num6 = 0; num6 < instance.LightHsvCount; num6++)
			{
				int i54 = instance.LightHsv[num6];
				size += ProtocolParser.GetSize(i54) + 2;
			}
		}
		if (instance.IsMissing != 0)
		{
			size += ProtocolParser.GetSize(instance.IsMissing) + 2;
		}
		if (instance.HeldLeftReadyAnimation != null)
		{
			size += ProtocolParser.GetSize(instance.HeldLeftReadyAnimation) + 2;
		}
		if (instance.HeldRightReadyAnimation != null)
		{
			size += ProtocolParser.GetSize(instance.HeldRightReadyAnimation) + 2;
		}
		if (instance.ParticlesTextureCode != null)
		{
			size += ProtocolParser.GetSize(instance.ParticlesTextureCode) + 2;
		}
		instance.size = size;
		return size;
	}

	public static void SerializeWithSize(CitoStream stream, Packet_ItemType instance)
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

	public static byte[] SerializeToBytes(Packet_ItemType instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_ItemType instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
