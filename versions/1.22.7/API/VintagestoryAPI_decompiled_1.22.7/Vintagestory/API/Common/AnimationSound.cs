using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common;

/// <summary>
/// A sound that should play alongside an animation.
/// </summary>
[DocumentAsJson]
public class AnimationSound
{
	/// <summary>
	/// The sound will play when this frame is reached. Should be set if <see cref="F:Vintagestory.API.Common.AnimationSound.Looping" /> is false.
	/// </summary>
	[DocumentAsJson("Recommended", "0", false)]
	public int Frame;

	[JsonIgnore]
	public SoundAttributes Attributes = new SoundAttributes
	{
		Type = EnumSoundType.Entity
	};

	/// <summary>
	/// The chance to play this sound.
	/// </summary>
	[DocumentAsJson("Optional", "1", false)]
	public float Chance = 1f;

	/// <summary>
	/// Should this sound loop whilst the animation is playing? 
	/// </summary>
	[DocumentAsJson("Optional", "False", false)]
	public bool Looping;

	/// <summary>
	/// The location of the sound file.
	/// </summary>
	[DocumentAsJson("Required", "", false)]
	[JsonProperty]
	private AssetLocation Location
	{
		set
		{
			Attributes.Location = value;
		}
	}

	/// <summary>
	/// Alternative form for <see cref="P:Vintagestory.API.Common.AnimationSound.Location" />.
	/// </summary>
	[DocumentAsJson("Optional", "", false)]
	[JsonProperty]
	private AssetLocation Path
	{
		set
		{
			Attributes.Location = value;
		}
	}

	/// <summary>
	/// Controls a random pitch for the sound.
	/// </summary>
	[DocumentAsJson("Optional", "1", false)]
	[JsonProperty]
	private NatFloat Pitch
	{
		set
		{
			Attributes.Pitch = value;
		}
	}

	/// <summary>
	/// Controls a random volume for the sound.
	/// </summary>
	[DocumentAsJson("Optional", "1", false)]
	[JsonProperty]
	private NatFloat Volume
	{
		set
		{
			Attributes.Volume = value;
		}
	}

	/// <summary>
	/// At this many meters away, the sound will be 1% of the volume.
	/// </summary>
	[DocumentAsJson("Optional", "32", false)]
	[JsonProperty]
	private float Range
	{
		set
		{
			Attributes.Range = value;
		}
	}

	/// <summary>
	/// If true, sets the pitch to be randomized on sound play between 0.75 and 1.25. If false, the pitch will be 1. If not set, then the pitch is provided by <see cref="P:Vintagestory.API.Common.AnimationSound.Pitch" />.
	/// </summary>
	[DocumentAsJson("Optional", "False", false)]
	[JsonProperty]
	private bool RandomizePitch
	{
		set
		{
			Attributes.Pitch = (value ? SoundAttributes.RandomPitch : SoundAttributes.NatFloatOne);
		}
	}

	public AnimationSound Clone()
	{
		return new AnimationSound
		{
			Frame = Frame,
			Attributes = Attributes.Clone(),
			Chance = Chance,
			Looping = Looping
		};
	}

	public void ToBytes(BinaryWriter writer)
	{
		Attributes.ToBytes(writer);
		writer.Write(Chance);
		writer.Write(Looping);
		writer.Write(Frame);
	}

	public static AnimationSound CreateFromBytes(BinaryReader reader, string version)
	{
		if (GameVersion.IsAtLeastVersion(version, "1.22.0-dev.5"))
		{
			return new AnimationSound
			{
				Attributes = SoundAttributes.FromBytes(reader, version),
				Chance = reader.ReadSingle(),
				Looping = reader.ReadBoolean(),
				Frame = reader.ReadInt32()
			};
		}
		if (GameVersion.IsAtLeastVersion(version, "1.22.0-dev.2"))
		{
			AnimationSound obj = new AnimationSound
			{
				Location = AssetLocation.Create(reader.ReadString()),
				Range = reader.ReadSingle()
			};
			bool randomizePitch = reader.ReadBoolean();
			obj.Pitch = (randomizePitch ? SoundAttributes.RandomPitch : SoundAttributes.NatFloatOne);
			obj.Looping = GameVersion.IsAtLeastVersion(version, "1.22.0-dev.3") && reader.ReadBoolean();
			return obj;
		}
		return new AnimationSound
		{
			Attributes = new SoundAttributes
			{
				Location = AssetLocation.Create(reader.ReadString()),
				Range = reader.ReadSingle()
			},
			Frame = reader.ReadInt32(),
			Pitch = (reader.ReadBoolean() ? SoundAttributes.RandomPitch : SoundAttributes.NatFloatOne)
		};
	}

	[OnDeserialized]
	protected void OnDeserialized(StreamingContext context)
	{
		Attributes.Location?.WithPathPrefixOnce("sounds/");
	}
}
