using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.ServerMods.NoObf;

[DocumentAsJson]
[JsonObject(MemberSerialization.OptIn)]
public class EntityType : RegistryObjectType
{
	[JsonProperty]
	[DocumentAsJson("Optional", "Land", false)]
	public EnumHabitat Habitat = EnumHabitat.Land;

	[JsonProperty]
	[DocumentAsJson("Optional", "0.5, 0.5", false)]
	public Vec2f CollisionBoxSize = new Vec2f(0.5f, 0.5f);

	[JsonProperty]
	[DocumentAsJson("Optional", "0.5, 0.25", false)]
	public Vec2f DeadCollisionBoxSize = new Vec2f(0.5f, 0.25f);

	[JsonProperty]
	[DocumentAsJson("Optional", "CollisionBoxSize", false)]
	public Vec2f SelectionBoxSize;

	[JsonProperty]
	[DocumentAsJson("Optional", "DeadCollisionBoxSize", false)]
	public Vec2f DeadSelectionBoxSize;

	[JsonProperty]
	[DocumentAsJson("Optional", "0.1", false)]
	public double EyeHeight = 0.1;

	[JsonProperty]
	[DocumentAsJson("Optional", "EyeHeight", false)]
	public double? SwimmingEyeHeight;

	[JsonProperty]
	[DocumentAsJson("Optional", "25", false)]
	public float Weight = 25f;

	[JsonProperty]
	[DocumentAsJson("Optional", "False", false)]
	public bool CanClimb;

	[JsonProperty]
	[DocumentAsJson("Optional", "False", false)]
	public bool CanClimbAnywhere;

	[JsonProperty]
	[DocumentAsJson("Optional", "1", false)]
	public float FallDamageMultiplier = 1f;

	[JsonProperty]
	[DocumentAsJson("Optional", "0.5", false)]
	public float ClimbTouchDistance = 0.5f;

	[JsonProperty]
	[DocumentAsJson("Optional", "False", false)]
	public bool RotateModelOnClimb;

	[JsonProperty]
	[DocumentAsJson("Optional", "0", false)]
	public float KnockbackResistance;

	[JsonProperty]
	[JsonConverter(typeof(JsonAttributesConverter))]
	[DocumentAsJson("Optional", "None", false)]
	public JsonObject Attributes;

	[JsonProperty(ItemConverterType = typeof(JsonAttributesConverter))]
	[DocumentAsJson("Optional", "None", false)]
	public Dictionary<string, JsonObject> BehaviorConfigs;

	[JsonProperty]
	[DocumentAsJson("Required", "", false)]
	public ClientEntityConfig Client;

	[JsonProperty]
	[DocumentAsJson("Required", "", false)]
	public ServerEntityConfig Server;

	[JsonProperty]
	[DocumentAsJson("Recommended", "None. Sound ranges default to 24, and pitches default to non-random except for the idle sound.", false)]
	public Dictionary<string, SoundAttributes> Sounds;

	[JsonProperty]
	[DocumentAsJson("Optional", "0.3", false)]
	public float IdleSoundChance = 0.05f;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public BlockDropItemStack[] Drops;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public TagSetFast Tags;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public Vec2f HitBoxSize
	{
		get
		{
			return null;
		}
		set
		{
			CollisionBoxSize = value;
			SelectionBoxSize = value;
		}
	}

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public Vec2f DeadHitBoxSize
	{
		get
		{
			return null;
		}
		set
		{
			DeadCollisionBoxSize = value;
			DeadSelectionBoxSize = value;
		}
	}

	public EntityProperties CreateProperties(ICoreAPI api)
	{
		BlockDropItemStack[] DropsCopy;
		if (Drops == null)
		{
			DropsCopy = null;
		}
		else
		{
			DropsCopy = new BlockDropItemStack[Drops.Length];
			for (int i = 0; i < DropsCopy.Length; i++)
			{
				DropsCopy[i] = Drops[i].Clone();
			}
		}
		EntityProperties properties = new EntityProperties
		{
			Code = Code,
			Variant = new Vintagestory.API.Datastructures.OrderedDictionary<string, string>(Variant),
			Class = Class,
			Habitat = Habitat,
			CollisionBoxSize = CollisionBoxSize,
			DeadCollisionBoxSize = DeadCollisionBoxSize,
			SelectionBoxSize = SelectionBoxSize,
			DeadSelectionBoxSize = DeadSelectionBoxSize,
			Weight = Weight,
			CanClimb = CanClimb,
			CanClimbAnywhere = CanClimbAnywhere,
			FallDamage = (FallDamageMultiplier > 0f),
			FallDamageMultiplier = FallDamageMultiplier,
			ClimbTouchDistance = ClimbTouchDistance,
			RotateModelOnClimb = RotateModelOnClimb,
			KnockbackResistance = KnockbackResistance,
			Attributes = Attributes,
			Sounds = ((Sounds == null) ? new Dictionary<string, SoundAttributes>() : new Dictionary<string, SoundAttributes>(Sounds)),
			IdleSoundChance = IdleSoundChance,
			Drops = DropsCopy,
			EyeHeight = EyeHeight,
			SwimmingEyeHeight = (SwimmingEyeHeight ?? EyeHeight),
			Tags = Tags
		};
		JsonObject serverBhHealth = Server?.Behaviors?.FirstOrDefault((JsonObject jsobjs) => jsobjs["code"].AsString() == "health");
		if (serverBhHealth != null && Client.Behaviors.FirstOrDefault((JsonObject jsobjc) => jsobjc["code"].AsString() == "health") == null)
		{
			Client.Behaviors = Client.Behaviors.Append(serverBhHealth);
		}
		if (Client != null)
		{
			properties.Client = new EntityClientProperties(Client.Behaviors, BehaviorConfigs)
			{
				RendererName = Client.Renderer,
				Textures = new FastSmallDictionary<string, CompositeTexture>(Client.Textures),
				GlowLevel = Client.GlowLevel,
				PitchStep = Client.PitchStep,
				Shape = Client.Shape,
				Size = Client.Size,
				SizeGrowthFactor = Client.SizeGrowthFactor,
				Animations = Client.Animations,
				AnimationsByMetaCode = Client.AnimationsByMetaCode
			};
		}
		if (Server != null)
		{
			properties.Server = new EntityServerProperties(Server.Behaviors, BehaviorConfigs)
			{
				Attributes = (Server.Attributes?.ToAttribute() as TreeAttribute),
				SpawnConditions = Server.SpawnConditions
			};
		}
		return properties;
	}

	internal override RegistryObjectType CreateAndPopulate(ICoreServerAPI api, AssetLocation fullcode, JObject jobject, JsonSerializer deserializer, Vintagestory.API.Datastructures.OrderedDictionary<string, string> variant)
	{
		deserializer.Converters.Add(new SoundAttributeConverter(randomPitch: false, 24f));
		EntityType type = CreateResolvedType<EntityType>(api, fullcode, jobject, deserializer, variant);
		try
		{
			JToken idleSoundRange = jobject["idleSoundRange"];
			if (idleSoundRange != null)
			{
				SoundAttributes idleSound = type.Sounds["idle"];
				idleSound.Range = idleSoundRange.ToObject<float>();
				type.Sounds["idle"] = idleSound;
			}
			JToken jIdleSound = (jobject["sounds"] as JObject)?["idle"];
			if (jIdleSound != null && (jIdleSound as JObject)?["pitch"] == null)
			{
				SoundAttributes idleSound2 = type.Sounds["idle"];
				idleSound2.Pitch = SoundAttributes.RandomPitch;
				type.Sounds["idle"] = idleSound2;
			}
		}
		catch (Exception e)
		{
			api.Server.Logger.Error("Exception thrown while trying to resolve entity type {0}, variant {1}. Will ignore most of the attributes. Exception thrown:", Code, fullcode);
			api.Server.Logger.Error(e);
		}
		return type;
	}
}
