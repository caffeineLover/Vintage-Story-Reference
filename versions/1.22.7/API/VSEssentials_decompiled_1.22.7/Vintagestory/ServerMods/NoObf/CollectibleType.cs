using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.ServerMods.NoObf;

[DocumentAsJson]
[JsonObject(MemberSerialization.OptIn)]
public abstract class CollectibleType : RegistryObjectType
{
	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public CollectibleBehaviorType[] Behaviors = Array.Empty<CollectibleBehaviorType>();

	[JsonProperty]
	[DocumentAsJson("Optional", "[0, 0, 0]", false)]
	public byte[] LightHsv = new byte[3];

	[JsonProperty]
	[DocumentAsJson("Optional", "0.05", false)]
	public float RenderAlphaTest = 0.05f;

	[JsonProperty]
	[DocumentAsJson("Optional", "1", false)]
	public int StorageFlags = 1;

	[JsonProperty]
	[DocumentAsJson("Optional", "1", false)]
	public int MaxStackSize = 1;

	[JsonProperty]
	[DocumentAsJson("Optional", "0.5", false)]
	public float AttackPower = 0.5f;

	[JsonProperty]
	[DocumentAsJson("Optional", "0", false)]
	public int Durability;

	[JsonProperty]
	[DocumentAsJson("Optional", "0.5, 0.5, 0.5", false)]
	public Size3f Size;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public EnumItemDamageSource[] DamagedBy;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public EnumTool? Tool;

	[JsonProperty]
	[DocumentAsJson("Optional", "1.5", false)]
	public float AttackRange = GlobalConstants.DefaultAttackRange;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public Dictionary<EnumBlockMaterial, float> MiningSpeed;

	[JsonProperty]
	[DocumentAsJson("Optional", "0", false)]
	public int ToolTier;

	[JsonProperty]
	[DocumentAsJson("Optional", "Solid", false)]
	public EnumMatterState MatterState = EnumMatterState.Solid;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public HeldSounds HeldSounds;

	[JsonProperty]
	[DocumentAsJson("Optional", "9999", false)]
	public int MaterialDensity = 9999;

	[JsonProperty]
	[JsonConverter(typeof(JsonAttributesConverter))]
	[DocumentAsJson("Optional", "None", false)]
	public JsonObject Attributes;

	[JsonProperty]
	[DocumentAsJson("Recommended", "None", false)]
	public CompositeShape Shape;

	[JsonProperty]
	[DocumentAsJson("Recommended", "None", false)]
	public ModelTransform GuiTransform;

	[JsonProperty]
	[Obsolete("Use TpHandTransform instead")]
	[DocumentAsJson("Obsolete", "", false)]
	public ModelTransform FpHandTransform;

	[JsonProperty]
	[DocumentAsJson("Recommended", "None", false)]
	public ModelTransform TpHandTransform;

	[JsonProperty]
	[DocumentAsJson("Recommended", "None", false)]
	public ModelTransform TpOffHandTransform;

	[JsonProperty]
	[DocumentAsJson("Recommended", "None", false)]
	public ModelTransform GroundTransform;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public CompositeTexture Texture;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public Dictionary<string, CompositeTexture> Textures;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public CombustibleProperties CombustibleProps;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public FoodNutritionProperties NutritionProps;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public TransitionableProperties[] TransitionableProps;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public GrindingProperties GrindingProps;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public CrushingProperties CrushingProps;

	[JsonProperty]
	[DocumentAsJson("Optional", "False", false)]
	public bool LiquidSelectable;

	[JsonProperty]
	[DocumentAsJson("Recommended", "None", false)]
	public Dictionary<string, string[]> CreativeInventory = new Dictionary<string, string[]>();

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public CreativeTabAndStackList[] CreativeInventoryStacks;

	[JsonProperty]
	[DocumentAsJson("Optional", "breakhand", false)]
	public string HeldTpHitAnimation = "breakhand";

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public string HeldRightTpIdleAnimation;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public string HeldLeftTpIdleAnimation;

	[JsonProperty]
	[DocumentAsJson("Optional", "helditemready", false)]
	public string HeldLeftReadyAnimation = "helditemready";

	[JsonProperty]
	[DocumentAsJson("Optional", "helditemready", false)]
	public string HeldRightReadyAnimation = "helditemready";

	[JsonProperty("heldTpIdleAnimation")]
	[DocumentAsJson("Obsolete", "", false)]
	private string HeldOldTpIdleAnimation;

	[JsonProperty]
	[DocumentAsJson("Optional", "interactstatic", false)]
	public string HeldTpUseAnimation = "interactstatic";

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public AdvancedParticleProperties[] ParticleProperties;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public string ParticlesTextureCode;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public TagSet Tags;

	[JsonProperty]
	[Obsolete("Use Size instead from game version 1.20.4 onwards, with the same values")]
	[DocumentAsJson("Optional", "0.5, 0.5, 0.5", false)]
	public Size3f Dimensions
	{
		get
		{
			return Size;
		}
		set
		{
			Size = value;
		}
	}

	[JsonProperty]
	[Obsolete("Use tool tier")]
	[DocumentAsJson("Obsolete", "", false)]
	public int MiningTier
	{
		get
		{
			return ToolTier;
		}
		set
		{
			ToolTier = value;
		}
	}

	[OnDeserialized]
	internal void OnDeserializedMethod(StreamingContext context)
	{
		OnDeserialized();
	}

	internal virtual void OnDeserialized()
	{
		if (Texture != null)
		{
			if (Textures == null)
			{
				Textures = new Dictionary<string, CompositeTexture>(1);
			}
			Textures["all"] = Texture;
		}
		if (HeldOldTpIdleAnimation != null && HeldRightTpIdleAnimation == null)
		{
			HeldRightTpIdleAnimation = HeldOldTpIdleAnimation;
		}
	}
}
