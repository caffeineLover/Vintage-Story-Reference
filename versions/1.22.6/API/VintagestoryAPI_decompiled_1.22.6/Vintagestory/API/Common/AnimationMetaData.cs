using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.API.Common;

/// <summary>
/// Animation Meta Data is a json type that controls how an animation should be played.
/// </summary>
/// <example>
/// <code language="json">
///             "animations": [
///             	{
///             		"code": "hurt",
///             		"animation": "hurt",
///             		"animationSpeed": 2.2,
///             		"weight": 10,
///             		"blendMode": "AddAverage"
///             	},
///             	{
///             		"code": "die",
///             		"animation": "death",
///             		"animationSpeed": 1.25,
///             		"weight": 10,
///             		"blendMode": "Average",
///             		"triggeredBy": { "onControls": [ "dead" ] }
///             	},
///             	{
///             		"code": "idle",
///             		"animation": "idle",
///             		"blendMode": "AddAverage",
///             		"easeOutSpeed": 4,
///             		"triggeredBy": { "defaultAnim": true }
///             	},
///             	{
///             		"code": "walk",
///             		"animation": "walk",
///             		"weight": 5
///             	}
///             ]
/// </code>
/// </example>
[DocumentAsJson]
public class AnimationMetaData
{
	/// <summary>
	/// Unique identifier to be able to reference this AnimationMetaData instance
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Required", "", false)]
	public string Code;

	/// <summary>
	/// Custom attributes that can be used for the animation.
	/// Valid vanilla attributes are:<br />
	/// - damageAtFrame (float)<br />
	/// - soundAtFrame (float)<br />
	/// - authorative (bool)<br />
	/// </summary>
	[JsonProperty]
	[JsonConverter(typeof(JsonAttributesConverter))]
	[DocumentAsJson("Optional", "None", false)]
	public JsonObject Attributes;

	/// <summary>
	/// The animations code identifier that we want to play
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Required", "", false)]
	public string Animation;

	/// <summary>
	/// The weight of this animation. When using multiple animations at a time, this controls the significance of each animation.
	/// The method for determining final animation values depends on this and <see cref="F:Vintagestory.API.Common.AnimationMetaData.BlendMode" />.
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "1", false)]
	public float Weight = 1f;

	/// <summary>
	/// A way of specifying <see cref="F:Vintagestory.API.Common.AnimationMetaData.Weight" /> for each element.
	/// Also see <see cref="F:Vintagestory.API.Common.AnimationMetaData.ElementBlendMode" /> to control blend modes per element..
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public Dictionary<string, float> ElementWeight = new Dictionary<string, float>();

	/// <summary>
	/// The speed this animation should play at.
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "1", false)]
	public float AnimationSpeed = 1f;

	/// <summary>
	/// Should this animation speed be multiplied by the movement speed of the entity?
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "False", false)]
	public bool MulWithWalkSpeed;

	/// <summary>
	/// This property can be used in cases where a animation with high weight is played alongside another animation with low element weight.
	/// In these cases, the easeIn become unaturally fast. Setting a value of 0.8f or similar here addresses this issue.<br />
	/// - 0f = uncapped weight<br />
	/// - 0.5f = weight cannot exceed 2<br />
	/// - 1f = weight cannot exceed 1
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "0", false)]
	public float WeightCapFactor;

	/// <summary>
	/// A multiplier applied to the weight value to "ease in" the animation. Choose a high value for looping animations or it will be glitchy
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "10", false)]
	public float EaseInSpeed = 10f;

	/// <summary>
	/// A multiplier applied to the weight value to "ease out" the animation. Choose a high value for looping animations or it will be glitchy
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "10", false)]
	public float EaseOutSpeed = 10f;

	/// <summary>
	/// Controls when this animation should be played.
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public AnimationTrigger TriggeredBy;

	/// <summary>
	/// The animation blend mode. Controls how this animation will react with other concurrent animations.
	/// Also see <see cref="F:Vintagestory.API.Common.AnimationMetaData.ElementBlendMode" /> to control blend mode per element.
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "Add", false)]
	public EnumAnimationBlendMode BlendMode;

	/// <summary>
	/// A way of specifying <see cref="F:Vintagestory.API.Common.AnimationMetaData.BlendMode" /> per element.
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public Dictionary<string, EnumAnimationBlendMode> ElementBlendMode = new Dictionary<string, EnumAnimationBlendMode>(StringComparer.OrdinalIgnoreCase);

	/// <summary>
	/// Should this animation stop default animations from playing?
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "False", false)]
	public bool SupressDefaultAnimation;

	/// <summary>
	/// A value that determines whether to change the first-person eye position for the camera.
	/// Higher values will keep eye position static.
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "99", false)]
	public float HoldEyePosAfterEasein = 99f;

	/// <summary>
	/// If true, the server does not sync this animation.
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "False", false)]
	public bool ClientSide;

	[JsonProperty]
	public bool WithFpVariant;

	[JsonProperty]
	public AnimationSound[] AnimationSounds;

	public AnimationMetaData FpVariant;

	public float StartFrameOnce;

	private int withActivitiesMerged;

	public uint CodeCrc32;

	public bool WasStartedFromTrigger;

	[JsonProperty]
	public bool AdjustCollisionBox { get; set; }

	public float GetCurrentAnimationSpeed(float walkspeed)
	{
		return AnimationSpeed * (MulWithWalkSpeed ? walkspeed : 1f) * GlobalConstants.OverallSpeedMultiplier;
	}

	public AnimationMetaData Init()
	{
		withActivitiesMerged = 0;
		EnumEntityActivity[] OnControls = TriggeredBy?.OnControls;
		if (OnControls != null)
		{
			for (int i = 0; i < OnControls.Length; i++)
			{
				withActivitiesMerged |= (int)OnControls[i];
			}
		}
		CodeCrc32 = GetCrc32(Code);
		if (WithFpVariant)
		{
			FpVariant = Clone();
			FpVariant.WithFpVariant = false;
			FpVariant.Animation += "-fp";
			FpVariant.Code += "-fp";
			FpVariant.Init();
		}
		int i2 = 0;
		while (AnimationSounds != null && i2 < AnimationSounds.Length)
		{
			if (AnimationSounds[i2]?.Attributes.Location == null)
			{
				ShapeElement.Logger?.Warning("Animation with code '" + Code + "' has null sound at index " + i2);
			}
			i2++;
		}
		return this;
	}

	[OnDeserialized]
	internal void OnDeserializedMethod(StreamingContext context)
	{
		Animation = Animation?.ToLowerInvariant() ?? "";
		if (Code == null)
		{
			Code = Animation;
		}
		CodeCrc32 = GetCrc32(Code);
	}

	public static uint GetCrc32(string animcode)
	{
		int mask = int.MaxValue;
		return (uint)(GameMath.Crc32(animcode.ToLowerInvariant()) & mask);
	}

	public bool Matches(int currentActivities)
	{
		AnimationTrigger triggeredBy = TriggeredBy;
		bool result;
		if (triggeredBy != null && triggeredBy.MatchExact)
		{
			result = currentActivities == withActivitiesMerged;
		}
		else
		{
			result = (currentActivities & withActivitiesMerged) > 0;
			if (result && (withActivitiesMerged & 0x90) != 0 && (currentActivities & 0x4000) != 0)
			{
				result = false;
			}
		}
		return result;
	}

	/// <summary>
	/// Not a deep clone of all fields. Scalar fields, for example AnimationSpeed, are fresh copies and can be changed dynamically or per-entity, after cloning the AnimationMetaData.
	/// <br /> For performance reasons, the non-scalar fields such as the Attributes, ElementWeight, ElementBlendMode and TriggeredBy are not deep-cloned as these fields are usually unchanging for all entities using the animation.
	/// <br /> Note: If any implementing entity needs to change the non-scalar fields dynamically, or on a per-entity basis, that entity's code can clone and replace the object in its own individual copy of the AnimationMetaData.  e.g. EntityDrifter replaces .TriggeredBy in some of its animations
	/// </summary>
	public AnimationMetaData Clone()
	{
		return new AnimationMetaData
		{
			Code = Code,
			Animation = Animation,
			AnimationSounds = AnimationSounds,
			Weight = Weight,
			Attributes = Attributes,
			ClientSide = ClientSide,
			ElementWeight = ElementWeight,
			AnimationSpeed = AnimationSpeed,
			MulWithWalkSpeed = MulWithWalkSpeed,
			EaseInSpeed = EaseInSpeed,
			EaseOutSpeed = EaseOutSpeed,
			TriggeredBy = TriggeredBy,
			AdjustCollisionBox = AdjustCollisionBox,
			BlendMode = BlendMode,
			ElementBlendMode = ElementBlendMode,
			withActivitiesMerged = withActivitiesMerged,
			CodeCrc32 = CodeCrc32,
			WasStartedFromTrigger = WasStartedFromTrigger,
			HoldEyePosAfterEasein = HoldEyePosAfterEasein,
			StartFrameOnce = StartFrameOnce,
			SupressDefaultAnimation = SupressDefaultAnimation,
			WeightCapFactor = WeightCapFactor
		};
	}

	public override bool Equals(object obj)
	{
		if (obj is AnimationMetaData other && other.Animation == Animation && other.AnimationSpeed == AnimationSpeed)
		{
			return other.BlendMode == BlendMode;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Animation.GetHashCode() ^ AnimationSpeed.GetHashCode() ^ BlendMode.GetHashCode();
	}

	public void ToBytes(BinaryWriter writer)
	{
		writer.Write(Code);
		writer.Write(Animation);
		writer.Write(Weight);
		writer.Write(ElementWeight.Count);
		foreach (KeyValuePair<string, float> val in ElementWeight)
		{
			writer.Write(val.Key);
			writer.Write(val.Value);
		}
		writer.Write(AnimationSpeed);
		writer.Write(EaseInSpeed);
		writer.Write(EaseOutSpeed);
		writer.Write(TriggeredBy != null);
		if (TriggeredBy != null)
		{
			writer.Write(TriggeredBy.MatchExact);
			EnumEntityActivity[] OnControls = TriggeredBy.OnControls;
			if (OnControls != null)
			{
				writer.Write(OnControls.Length);
				for (int i = 0; i < OnControls.Length; i++)
				{
					writer.Write((int)OnControls[i]);
				}
			}
			else
			{
				writer.Write(0);
			}
			writer.Write(TriggeredBy.DefaultAnim);
		}
		writer.Write((int)BlendMode);
		writer.Write(ElementBlendMode.Count);
		foreach (KeyValuePair<string, EnumAnimationBlendMode> val2 in ElementBlendMode)
		{
			writer.Write(val2.Key);
			writer.Write((int)val2.Value);
		}
		writer.Write(MulWithWalkSpeed);
		writer.Write(StartFrameOnce);
		writer.Write(HoldEyePosAfterEasein);
		writer.Write(ClientSide);
		writer.Write(Attributes?.ToString() ?? "");
		writer.Write(WeightCapFactor);
		AnimationSound[] animationSounds = AnimationSounds;
		writer.Write((animationSounds != null) ? animationSounds.Length : 0);
		if (AnimationSounds != null)
		{
			AnimationSound[] animationSounds2 = AnimationSounds;
			for (int j = 0; j < animationSounds2.Length; j++)
			{
				animationSounds2[j].ToBytes(writer);
			}
		}
		writer.Write(AdjustCollisionBox);
	}

	public static AnimationMetaData FromBytes(BinaryReader reader, string version)
	{
		AnimationMetaData animdata = new AnimationMetaData();
		animdata.Code = reader.ReadString().DeDuplicate();
		animdata.Animation = reader.ReadString();
		animdata.Weight = reader.ReadSingle();
		int weightCount = reader.ReadInt32();
		for (int i = 0; i < weightCount; i++)
		{
			animdata.ElementWeight[reader.ReadString().DeDuplicate()] = reader.ReadSingle();
		}
		animdata.AnimationSpeed = reader.ReadSingle();
		animdata.EaseInSpeed = reader.ReadSingle();
		animdata.EaseOutSpeed = reader.ReadSingle();
		if (reader.ReadBoolean())
		{
			animdata.TriggeredBy = new AnimationTrigger();
			animdata.TriggeredBy.MatchExact = reader.ReadBoolean();
			weightCount = reader.ReadInt32();
			animdata.TriggeredBy.OnControls = new EnumEntityActivity[weightCount];
			for (int j = 0; j < weightCount; j++)
			{
				animdata.TriggeredBy.OnControls[j] = (EnumEntityActivity)reader.ReadInt32();
			}
			animdata.TriggeredBy.DefaultAnim = reader.ReadBoolean();
		}
		animdata.BlendMode = (EnumAnimationBlendMode)reader.ReadInt32();
		weightCount = reader.ReadInt32();
		for (int k = 0; k < weightCount; k++)
		{
			animdata.ElementBlendMode[reader.ReadString().DeDuplicate()] = (EnumAnimationBlendMode)reader.ReadInt32();
		}
		animdata.MulWithWalkSpeed = reader.ReadBoolean();
		if (version != "" && GameVersion.IsAtLeastVersion(version, "1.12.5-dev.1"))
		{
			animdata.StartFrameOnce = reader.ReadSingle();
		}
		if (version != "" && GameVersion.IsAtLeastVersion(version, "1.13.0-dev.3"))
		{
			animdata.HoldEyePosAfterEasein = reader.ReadSingle();
		}
		if (version != "" && GameVersion.IsAtLeastVersion(version, "1.17.0-dev.18"))
		{
			animdata.ClientSide = reader.ReadBoolean();
		}
		if (version != "" && GameVersion.IsAtLeastVersion(version, "1.19.0-dev.20"))
		{
			string attributes = reader.ReadString();
			if (attributes != "")
			{
				animdata.Attributes = new JsonObject(JToken.Parse(attributes));
			}
			else
			{
				animdata.Attributes = new JsonObject(JToken.Parse("{}"));
			}
		}
		if (version != "" && GameVersion.IsAtLeastVersion(version, "1.19.0-rc.6"))
		{
			animdata.WeightCapFactor = reader.ReadSingle();
		}
		if (version != "" && GameVersion.IsAtLeastVersion(version, "1.22.0-dev.2"))
		{
			int animsoundslen = reader.ReadInt32();
			if (animsoundslen > 0)
			{
				animdata.AnimationSounds = new AnimationSound[animsoundslen];
				for (int l = 0; l < animsoundslen; l++)
				{
					animdata.AnimationSounds[l] = AnimationSound.CreateFromBytes(reader, version);
				}
			}
		}
		else if (version != "" && GameVersion.IsAtLeastVersion(version, "1.20.0-dev.13") && reader.ReadBoolean())
		{
			animdata.AnimationSounds = new AnimationSound[1];
			animdata.AnimationSounds[0] = AnimationSound.CreateFromBytes(reader, version);
		}
		if (version != "" && GameVersion.IsAtLeastVersion(version, "1.21.0-dev.1"))
		{
			animdata.AdjustCollisionBox = reader.ReadBoolean();
		}
		animdata.Init();
		return animdata;
	}

	internal void DeDuplicate()
	{
		Code = Code.DeDuplicate();
		Dictionary<string, float> newElementWeight = new Dictionary<string, float>(ElementWeight.Count);
		foreach (KeyValuePair<string, float> entry in ElementWeight)
		{
			newElementWeight[entry.Key.DeDuplicate()] = entry.Value;
		}
		ElementWeight = newElementWeight;
		Dictionary<string, EnumAnimationBlendMode> newElementBlendMode = new Dictionary<string, EnumAnimationBlendMode>(ElementBlendMode.Count);
		foreach (KeyValuePair<string, EnumAnimationBlendMode> entry2 in ElementBlendMode)
		{
			newElementBlendMode[entry2.Key.DeDuplicate()] = entry2.Value;
		}
		ElementBlendMode = newElementBlendMode;
	}
}
