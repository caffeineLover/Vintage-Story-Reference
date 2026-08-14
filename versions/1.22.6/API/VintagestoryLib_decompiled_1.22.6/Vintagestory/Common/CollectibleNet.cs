using System;
using System.Collections.Generic;
using System.IO;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.Common;

public abstract class CollectibleNet
{
	public static ModelTransform DefGuiTransform = ModelTransform.BlockDefaultGui();

	public static ModelTransform DefFpHandTransform = ModelTransform.BlockDefaultFp();

	public static ModelTransform DefTpHandTransform = ModelTransform.BlockDefaultTp();

	public static ModelTransform DefTpOffHandTransform = ModelTransform.BlockDefaultTp();

	public static ModelTransform DefGroundTransform = ModelTransform.BlockDefaultGround();

	public static int SerializeFloat(float p)
	{
		return (int)(p * 64f);
	}

	public static float DeserializeFloat(int p)
	{
		return (float)p / 64f;
	}

	public static long SerializeDouble(double p)
	{
		return (long)(p * 1024.0);
	}

	public static double DeserializeDouble(long p)
	{
		return (double)p / 1024.0;
	}

	public static long SerializeDoublePrecise(double p)
	{
		return (long)(p * 16384.0);
	}

	public static double DeserializeDoublePrecise(long p)
	{
		return (double)p / 16384.0;
	}

	public static int SerializeFloatPrecise(float v)
	{
		return (int)(v * 1024f);
	}

	public static float DeserializeFloatPrecise(int v)
	{
		return (float)v / 1024f;
	}

	public static int SerializePlayerPos(double v)
	{
		return (int)(v * 10240.0 * 1000.0);
	}

	public static double DeserializePlayerPos(int v)
	{
		return (double)v / 10240000.0;
	}

	public static int SerializeFloatVeryPrecise(float v)
	{
		return (int)(v * 10000f);
	}

	public static float DeserializeFloatVeryPrecise(int v)
	{
		return (float)v / 10000f;
	}

	public static Packet_HeldSoundSet ToPacket(HeldSounds sounds)
	{
		return new Packet_HeldSoundSet
		{
			Idle = ToPacket(sounds.Idle),
			Attack = ToPacket(sounds.Attack),
			Equip = ToPacket(sounds.Equip),
			Unequip = ToPacket(sounds.Unequip),
			InvPickup = ToPacket(sounds.InvPickup),
			InvPlace = ToPacket(sounds.InvPlace)
		};
	}

	public static Packet_VariantPart[] ToPacket(Vintagestory.API.Datastructures.OrderedDictionary<string, string> variant)
	{
		Packet_VariantPart[] p = new Packet_VariantPart[variant.Count];
		int i = 0;
		foreach (KeyValuePair<string, string> val in variant)
		{
			p[i++] = new Packet_VariantPart
			{
				Code = val.Key,
				Value = val.Value
			};
		}
		return p;
	}

	public static Packet_NutritionProperties ToPacket(FoodNutritionProperties props)
	{
		if (props.EatenStack == null)
		{
			return ToPacket(props, null);
		}
		using FastMemoryStream ms = new FastMemoryStream();
		return ToPacket(props, ms);
	}

	public static Packet_NutritionProperties ToPacket(FoodNutritionProperties props, FastMemoryStream ms)
	{
		Packet_NutritionProperties p = new Packet_NutritionProperties
		{
			FoodCategory = (int)props.FoodCategory,
			Satiety = SerializeFloat(props.Satiety),
			Health = SerializeFloat(props.Health),
			Intoxication = SerializeFloat(props.Intoxication),
			Psychedelic = SerializeFloat(props.Psychedelic),
			SaturationLossDelay = SerializeFloat(props.SaturationLossDelay)
		};
		if (props.EatenStack != null)
		{
			ms.Reset();
			BinaryWriter writer = new BinaryWriter(ms);
			props.EatenStack.ToBytes(writer);
			p.SetEatenStack(ms.ToArray());
		}
		return p;
	}

	public static Packet_TransitionableProperties[] ToPacket(TransitionableProperties[] mprops)
	{
		using FastMemoryStream ms = new FastMemoryStream();
		return ToPacket(mprops, ms);
	}

	public static Packet_TransitionableProperties[] ToPacket(TransitionableProperties[] mprops, FastMemoryStream ms)
	{
		Packet_TransitionableProperties[] packets = new Packet_TransitionableProperties[mprops.Length];
		for (int i = 0; i < mprops.Length; i++)
		{
			TransitionableProperties props = mprops[i];
			Packet_TransitionableProperties p = new Packet_TransitionableProperties
			{
				FreshHours = ToPacket(props.FreshHours),
				TransitionHours = ToPacket(props.TransitionHours),
				TransitionRatio = SerializeFloat(props.TransitionRatio),
				Type = (int)props.Type
			};
			if (props.TransitionedStack != null)
			{
				ms.Reset();
				BinaryWriter writer = new BinaryWriter(ms);
				props.TransitionedStack.ToBytes(writer);
				p.SetTransitionedStack(ms.ToArray());
			}
			packets[i] = p;
		}
		return packets;
	}

	public static Packet_NatFloat ToPacket(NatFloat val)
	{
		return new Packet_NatFloat
		{
			Avg = SerializeFloatPrecise(val.avg),
			Var = SerializeFloatPrecise(val.var),
			Dist = (int)val.dist
		};
	}

	public static NatFloat FromPacket(Packet_NatFloat val)
	{
		return new NatFloat(DeserializeFloatPrecise(val.Avg), DeserializeFloatPrecise(val.Var), (EnumDistribution)val.Dist);
	}

	public static Packet_SoundAttributes ToPacket(SoundAttributes val)
	{
		if (val.Location == null)
		{
			return null;
		}
		return new Packet_SoundAttributes
		{
			Location = val.Location.ToNonNullString(),
			Pitch = ToPacket(val.Pitch),
			Volume = ToPacket(val.Volume),
			Type = (int)val.Type,
			Range = SerializeFloat(val.Range)
		};
	}

	public static SoundAttributes FromPacket(Packet_SoundAttributes packet)
	{
		if (packet == null)
		{
			return new SoundAttributes();
		}
		SoundAttributes result = new SoundAttributes();
		result.Location = AssetLocation.CreateOrNull(packet.Location);
		result.Pitch = FromPacket(packet.Pitch);
		result.Volume = FromPacket(packet.Volume);
		result.Type = (EnumSoundType)packet.Type;
		result.Range = DeserializeFloat(packet.Range);
		return result;
	}

	public static Packet_GrindingProperties ToPacket(GrindingProperties props)
	{
		if (props.GroundStack == null)
		{
			return ToPacket(props, null);
		}
		using FastMemoryStream ms = new FastMemoryStream();
		return ToPacket(props, ms);
	}

	public static Packet_GrindingProperties ToPacket(GrindingProperties props, FastMemoryStream ms)
	{
		Packet_GrindingProperties p = new Packet_GrindingProperties();
		if (props.GroundStack != null)
		{
			ms.Reset();
			BinaryWriter writer = new BinaryWriter(ms);
			props.GroundStack.ToBytes(writer);
			p.SetGroundStack(ms.ToArray());
		}
		return p;
	}

	public static Packet_CrushingProperties ToPacket(CrushingProperties props)
	{
		if (props.CrushedStack == null)
		{
			return ToPacket(props, null);
		}
		using FastMemoryStream ms = new FastMemoryStream();
		return ToPacket(props, ms);
	}

	public static Packet_CrushingProperties ToPacket(CrushingProperties props, FastMemoryStream ms)
	{
		Packet_CrushingProperties p = new Packet_CrushingProperties();
		if (props.CrushedStack != null)
		{
			ms.Reset();
			BinaryWriter writer = new BinaryWriter(ms);
			props.CrushedStack.ToBytes(writer);
			p.SetCrushedStack(ms.ToArray());
			p.HardnessTier = props.HardnessTier;
			p.Quantity = ToPacket(props.Quantity);
		}
		return p;
	}

	public static HeldSounds FromPacket(Packet_HeldSoundSet p)
	{
		return new HeldSounds
		{
			Idle = FromPacket(p.Idle),
			Equip = FromPacket(p.Equip),
			Unequip = FromPacket(p.Unequip),
			Attack = FromPacket(p.Attack),
			InvPickup = FromPacket(p.InvPickup),
			InvPlace = FromPacket(p.InvPlace)
		};
	}

	public static GrindingProperties FromPacket(Packet_GrindingProperties pn, IWorldAccessor world)
	{
		GrindingProperties props = new GrindingProperties();
		if (pn.GroundStack != null)
		{
			using MemoryStream ms = new MemoryStream(pn.GroundStack);
			BinaryReader reader = new BinaryReader(ms);
			props.GroundStack = new JsonItemStack();
			props.GroundStack.FromBytes(reader, world.ClassRegistry);
		}
		return props;
	}

	public static CrushingProperties FromPacket(Packet_CrushingProperties pn, IWorldAccessor world)
	{
		CrushingProperties props = new CrushingProperties();
		if (pn.CrushedStack != null)
		{
			using MemoryStream ms = new MemoryStream(pn.CrushedStack);
			BinaryReader reader = new BinaryReader(ms);
			props.CrushedStack = new JsonItemStack();
			props.CrushedStack.FromBytes(reader, world.ClassRegistry);
			props.HardnessTier = pn.HardnessTier;
			if (pn.Quantity != null)
			{
				props.Quantity = FromPacket(pn.Quantity);
			}
		}
		return props;
	}

	public static FoodNutritionProperties FromPacket(Packet_NutritionProperties pn, IWorldAccessor world)
	{
		FoodNutritionProperties props = new FoodNutritionProperties
		{
			FoodCategory = (EnumFoodCategory)pn.FoodCategory,
			Satiety = DeserializeFloat(pn.Satiety),
			Health = DeserializeFloat(pn.Health),
			Intoxication = DeserializeFloat(pn.Intoxication),
			Psychedelic = DeserializeFloat(pn.Psychedelic),
			SaturationLossDelay = DeserializeFloat(pn.SaturationLossDelay)
		};
		if (pn.EatenStack != null)
		{
			using MemoryStream ms = new MemoryStream(pn.EatenStack);
			BinaryReader reader = new BinaryReader(ms);
			props.EatenStack = new JsonItemStack();
			props.EatenStack.FromBytes(reader, world.ClassRegistry);
		}
		return props;
	}

	public static TransitionableProperties[] FromPacket(Packet_TransitionableProperties[] pns, IWorldAccessor world)
	{
		List<TransitionableProperties> mprops = new List<TransitionableProperties>();
		foreach (Packet_TransitionableProperties pn in pns)
		{
			if (pn == null)
			{
				continue;
			}
			TransitionableProperties props = new TransitionableProperties
			{
				FreshHours = FromPacket(pn.FreshHours),
				TransitionHours = FromPacket(pn.TransitionHours),
				TransitionRatio = DeserializeFloat(pn.TransitionRatio),
				Type = (EnumTransitionType)pn.Type
			};
			if (pn.TransitionedStack != null)
			{
				using MemoryStream ms = new MemoryStream(pn.TransitionedStack);
				BinaryReader reader = new BinaryReader(ms);
				props.TransitionedStack = new JsonItemStack();
				props.TransitionedStack.FromBytes(reader, world.ClassRegistry);
			}
			mprops.Add(props);
		}
		return mprops.ToArray();
	}

	public static Packet_CombustibleProperties ToPacket(CombustibleProperties props)
	{
		if (props.SmeltedStack == null)
		{
			return ToPacket(props, null);
		}
		using FastMemoryStream ms = new FastMemoryStream();
		return ToPacket(props, ms);
	}

	public static Packet_CombustibleProperties ToPacket(CombustibleProperties props, FastMemoryStream ms)
	{
		Packet_CombustibleProperties p = new Packet_CombustibleProperties
		{
			BurnDuration = SerializeFloat(props.BurnDuration),
			BurnTemperature = props.BurnTemperature,
			HeatResistance = props.HeatResistance,
			MeltingDuration = SerializeFloat(props.MeltingDuration),
			MeltingPoint = props.MeltingPoint,
			SmeltedRatio = props.SmeltedRatio,
			RequiresContainer = (props.RequiresContainer ? 1 : 0),
			MeltingType = (int)props.SmeltingType,
			MaxTemperature = props.MaxTemperature
		};
		if (props.SmeltedStack != null)
		{
			ms.Reset();
			BinaryWriter writer = new BinaryWriter(ms);
			props.SmeltedStack.ToBytes(writer);
			p.SetSmeltedStack(ms.ToArray());
		}
		return p;
	}

	public static CombustibleProperties FromPacket(Packet_CombustibleProperties pc, IWorldAccessor world)
	{
		CombustibleProperties props = new CombustibleProperties
		{
			BurnDuration = DeserializeFloat(pc.BurnDuration),
			HeatResistance = pc.HeatResistance,
			BurnTemperature = pc.BurnTemperature,
			MeltingPoint = pc.MeltingPoint,
			MeltingDuration = DeserializeFloat(pc.MeltingDuration),
			SmeltedRatio = pc.SmeltedRatio,
			RequiresContainer = (pc.RequiresContainer > 0),
			SmeltingType = (EnumSmeltType)pc.MeltingType,
			MaxTemperature = pc.MaxTemperature
		};
		if (pc.SmeltedStack != null)
		{
			using MemoryStream ms = new MemoryStream(pc.SmeltedStack);
			BinaryReader reader = new BinaryReader(ms);
			props.SmeltedStack = new JsonItemStack();
			props.SmeltedStack.FromBytes(reader, world.ClassRegistry);
		}
		return props;
	}

	public static Packet_ModelTransform ToTransformPacket(ModelTransform transform, ModelTransform defaultTf)
	{
		FastVec3f rotation = (transform.Rotation.Equals(ModelTransformNoDefaults.defaultTf) ? defaultTf.Rotation : transform.Rotation);
		FastVec3f origin = transform.Origin;
		FastVec3f translation = (transform.Translation.Equals(ModelTransformNoDefaults.defaultTf) ? defaultTf.Translation : transform.Translation);
		FastVec3f scaleXYZ = transform.ScaleXYZ;
		return new Packet_ModelTransform
		{
			RotateX = SerializeFloatVeryPrecise(rotation.X),
			RotateY = SerializeFloatVeryPrecise(rotation.Y),
			RotateZ = SerializeFloatVeryPrecise(rotation.Z),
			OriginX = SerializeFloatVeryPrecise(origin.X),
			OriginY = SerializeFloatVeryPrecise(origin.Y),
			OriginZ = SerializeFloatVeryPrecise(origin.Z),
			TranslateX = SerializeFloatVeryPrecise(translation.X),
			TranslateY = SerializeFloatVeryPrecise(translation.Y),
			TranslateZ = SerializeFloatVeryPrecise(translation.Z),
			ScaleX = SerializeFloatVeryPrecise(scaleXYZ.X),
			ScaleY = SerializeFloatVeryPrecise(scaleXYZ.Y),
			ScaleZ = SerializeFloatVeryPrecise(scaleXYZ.Z),
			Rotate = (transform.Rotate ? 1 : 0)
		};
	}

	public static ModelTransform FromTransformPacket(Packet_ModelTransform p)
	{
		return new ModelTransform
		{
			Rotation = new FastVec3f(DeserializeFloatVeryPrecise(p.RotateX), DeserializeFloatVeryPrecise(p.RotateY), DeserializeFloatVeryPrecise(p.RotateZ)),
			Origin = new FastVec3f(DeserializeFloatVeryPrecise(p.OriginX), DeserializeFloatVeryPrecise(p.OriginY), DeserializeFloatVeryPrecise(p.OriginZ)),
			Translation = new FastVec3f(DeserializeFloatVeryPrecise(p.TranslateX), DeserializeFloatVeryPrecise(p.TranslateY), DeserializeFloatVeryPrecise(p.TranslateZ)),
			ScaleXYZ = new FastVec3f(DeserializeFloatVeryPrecise(p.ScaleX), DeserializeFloatVeryPrecise(p.ScaleY), DeserializeFloatVeryPrecise(p.ScaleZ)),
			Rotate = (p.Rotate > 0)
		};
	}

	public static CompositeShape FromPacket(Packet_CompositeShape packet)
	{
		CompositeShape shape = new CompositeShape
		{
			Base = ((packet.Base != null) ? new AssetLocation(packet.Base) : null),
			InsertBakedTextures = packet.InsertBakedTextures,
			rotateX = DeserializeFloat(packet.Rotatex),
			rotateY = DeserializeFloat(packet.Rotatey),
			rotateZ = DeserializeFloat(packet.Rotatez),
			offsetX = DeserializeFloatVeryPrecise(packet.Offsetx),
			offsetY = DeserializeFloatVeryPrecise(packet.Offsety),
			offsetZ = DeserializeFloatVeryPrecise(packet.Offsetz),
			Scale = DeserializeFloat(packet.ScaleAdjust) + 1f,
			Format = (EnumShapeFormat)packet.Format,
			VoxelizeTexture = (packet.VoxelizeShape > 0),
			QuantityElements = packet.QuantityElements
		};
		if (packet.QuantityElementsSet == 0)
		{
			shape.QuantityElements = null;
		}
		if (packet.AlternatesCount > 0)
		{
			shape.Alternates = new CompositeShape[packet.AlternatesCount];
			for (int i = 0; i < packet.AlternatesCount; i++)
			{
				shape.Alternates[i] = FromPacket(packet.Alternates[i]);
			}
		}
		if (packet.OverlaysCount > 0)
		{
			shape.Overlays = new CompositeShape[packet.OverlaysCount];
			for (int j = 0; j < packet.OverlaysCount; j++)
			{
				shape.Overlays[j] = FromPacket(packet.Overlays[j]);
			}
		}
		if (packet.SelectiveElementsCount > 0)
		{
			shape.SelectiveElements = new string[packet.SelectiveElementsCount];
			for (int k = 0; k < packet.SelectiveElementsCount; k++)
			{
				shape.SelectiveElements[k] = packet.SelectiveElements[k];
			}
		}
		if (packet.IgnoreElementsCount > 0)
		{
			shape.IgnoreElements = new string[packet.IgnoreElementsCount];
			for (int l = 0; l < packet.IgnoreElementsCount; l++)
			{
				shape.IgnoreElements[l] = packet.IgnoreElements[l];
			}
		}
		return shape;
	}

	public static Packet_CompositeShape ToPacket(CompositeShape shape)
	{
		Packet_CompositeShape packet = new Packet_CompositeShape
		{
			InsertBakedTextures = shape.InsertBakedTextures,
			Rotatex = SerializeFloat(shape.rotateX),
			Rotatey = SerializeFloat(shape.rotateY),
			Rotatez = SerializeFloat(shape.rotateZ),
			Offsetx = SerializeFloatVeryPrecise(shape.offsetX),
			Offsety = SerializeFloatVeryPrecise(shape.offsetY),
			Offsetz = SerializeFloatVeryPrecise(shape.offsetZ),
			ScaleAdjust = SerializeFloat(shape.Scale - 1f),
			Format = (int)shape.Format,
			Base = shape.Base?.ToShortString(),
			VoxelizeShape = (shape.VoxelizeTexture ? 1 : 0),
			QuantityElements = (shape.QuantityElements.HasValue ? shape.QuantityElements.Value : 0),
			QuantityElementsSet = (shape.QuantityElements.HasValue ? 1 : 0)
		};
		if (shape.SelectiveElements != null)
		{
			packet.SetSelectiveElements(shape.SelectiveElements);
		}
		if (shape.IgnoreElements != null)
		{
			packet.SetIgnoreElements(shape.IgnoreElements);
		}
		if (shape.Alternates != null)
		{
			Packet_CompositeShape[] packets = new Packet_CompositeShape[shape.Alternates.Length];
			for (int i = 0; i < shape.Alternates.Length; i++)
			{
				packets[i] = ToPacket(shape.Alternates[i]);
			}
			packet.SetAlternates(packets);
		}
		if (shape.Alternates != null)
		{
			Packet_CompositeShape[] packets2 = new Packet_CompositeShape[shape.Alternates.Length];
			for (int j = 0; j < shape.Alternates.Length; j++)
			{
				packets2[j] = ToPacket(shape.Alternates[j]);
			}
			packet.SetAlternates(packets2);
		}
		if (shape.Overlays != null)
		{
			Packet_CompositeShape[] packets3 = new Packet_CompositeShape[shape.Overlays.Length];
			for (int k = 0; k < shape.Overlays.Length; k++)
			{
				packets3[k] = ToPacket(shape.Overlays[k]);
			}
			packet.SetOverlays(packets3);
		}
		return packet;
	}

	public static CompositeTexture FromPacket(Packet_CompositeTexture packet)
	{
		CompositeTexture ct = new CompositeTexture
		{
			Base = new AssetLocation(packet.Base),
			Rotation = packet.Rotation,
			Alpha = packet.Alpha
		};
		if (packet.OverlaysCount > 0)
		{
			ct.Overlays = new AssetLocation[packet.OverlaysCount];
			for (int i = 0; i < packet.OverlaysCount; i++)
			{
				ct.BlendedOverlays[i] = new BlendedOverlayTexture
				{
					Base = new AssetLocation(packet.Overlays[i].Base),
					BlendMode = (EnumColorBlendMode)packet.Overlays[i].Mode
				};
			}
		}
		if (packet.AlternatesCount > 0)
		{
			ct.Alternates = new CompositeTexture[packet.AlternatesCount];
			for (int j = 0; j < packet.AlternatesCount; j++)
			{
				ct.Alternates[j] = FromPacket(packet.Alternates[j]);
			}
		}
		if (packet.TilesCount > 0)
		{
			ct.Tiles = new CompositeTexture[packet.TilesCount];
			for (int k = 0; k < packet.TilesCount; k++)
			{
				ct.Tiles[k] = FromPacket(packet.Tiles[k]);
			}
			ct.TilesWidth = packet.TilesWidth;
		}
		return ct;
	}

	public static Vintagestory.API.Datastructures.OrderedDictionary<string, string> FromPacket(Packet_VariantPart[] variant, int count)
	{
		Vintagestory.API.Datastructures.OrderedDictionary<string, string> variantdict = new Vintagestory.API.Datastructures.OrderedDictionary<string, string>();
		for (int i = 0; i < count; i++)
		{
			variantdict[variant[i].Code.DeDuplicate()] = variant[i].Value.DeDuplicate();
		}
		return variantdict;
	}

	public static Packet_CompositeTexture ToPacket(CompositeTexture ct)
	{
		Packet_CompositeTexture packet = new Packet_CompositeTexture();
		packet.Rotation = ct.Rotation;
		packet.Alpha = ct.Alpha;
		if (ct.Base == null)
		{
			throw new Exception("Cannot encode entity texture, Base property is null!");
		}
		packet.Base = ct.Base.ToShortString();
		if (ct.BlendedOverlays != null)
		{
			Packet_BlendedOverlayTexture[] overlay = new Packet_BlendedOverlayTexture[ct.BlendedOverlays.Length];
			for (int i = 0; i < overlay.Length; i++)
			{
				overlay[i] = new Packet_BlendedOverlayTexture
				{
					Base = ct.BlendedOverlays[i].Base.ToString(),
					Mode = (int)ct.BlendedOverlays[i].BlendMode
				};
			}
			packet.SetOverlays(overlay);
		}
		if (ct.Alternates != null)
		{
			packet.SetAlternates(ToPackets(ct.Alternates));
		}
		if (ct.Tiles != null)
		{
			packet.SetTiles(ToPackets(ct.Tiles));
		}
		packet.TilesWidth = ct.TilesWidth;
		return packet;
	}

	public static Packet_CompositeTexture[] ToPackets(CompositeTexture[] textures)
	{
		Packet_CompositeTexture[] packets = new Packet_CompositeTexture[textures.Length];
		for (int i = 0; i < textures.Length; i++)
		{
			packets[i] = ToPacket(textures[i]);
		}
		return packets;
	}
}
