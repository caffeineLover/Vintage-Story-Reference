using System.IO;
using Newtonsoft.Json;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common;

/// <summary>
/// Properties that control how a sound is played. There are two forms of this that might be used in JSON files:
/// Form 1 is simply stored as an <see cref="T:Vintagestory.API.Common.AssetLocation" /> that points to the sound file path. There are no extra proprties. This is what most JSON objects use.
/// Form 2 is more complex, and uses the properties below to fully configure the sound. Only certain JSON objects use this, such as entity sounds.
/// You can mix and match in a single sound array.
/// </summary>
/// <example>
/// Form 1: <code lang="json">
///             "sounds": {
///    "place": "block/plant",
///    "break": "block/plant",
///    "hit": "block/plant"
///             },
/// </code>
/// Form 2. Note that entity sounds can include * at the end for multiple sound files: <code lang="json">
///             "sounds": {
///    "hurt": "creature/chicken/chick*",
///    "death": "creature/chicken/chick*",
///    "idle": {
///        "path": "creature/chicken/chick*",
///        "range": 14,
///        "pitch": {
///            "avg": 1,
///            "var": 0.25
///        }
///    }
///             },
/// </code>
/// </example>
[DocumentAsJson]
public struct SoundAttributes
{
	/// <summary>
	/// <!--<jsonalias>path</jsonalias>-->
	/// The location of the sound file.
	/// </summary>
	[DocumentAsJson("Required", "", false)]
	[JsonProperty("path")]
	public AssetLocation? Location;

	/// <summary>
	/// Controls a random pitch for when the sound is played.
	/// </summary>
	[DocumentAsJson("Optional", "1", false)]
	public NatFloat Pitch;

	/// <summary>
	/// Controls a random volume for when the sound is played.
	/// </summary>
	[DocumentAsJson("Optional", "1", false)]
	public NatFloat Volume;

	/// <summary>
	/// The type of sound, used for volume control in the settings.
	/// </summary>
	[DocumentAsJson("Optional", "Sound", false)]
	[JsonProperty("soundType")]
	public EnumSoundType Type;

	/// <summary>
	/// The range at which the gain will be attenuated to 1% of the supplied volume. (i.e. the maximum range that the player will hear the sound)
	/// </summary>
	[DocumentAsJson("Optional", "32", false)]
	public float Range;

	public static NatFloat NatFloatOne;

	public static NatFloat RandomPitch;

	public SoundAttributes()
	{
		Location = null;
		Pitch = NatFloatOne;
		Volume = NatFloatOne;
		Type = EnumSoundType.Sound;
		Range = 32f;
	}

	public SoundAttributes(AssetLocation assetLocation, bool withRandomPitch)
	{
		Pitch = NatFloatOne;
		Volume = NatFloatOne;
		Type = EnumSoundType.Sound;
		Range = 32f;
		Location = assetLocation;
		if (withRandomPitch)
		{
			Pitch = RandomPitch;
		}
	}

	public bool Equals(SoundAttributes other)
	{
		if (other.Location == Location && other.Pitch == Pitch && other.Volume == Volume && other.Range == Range)
		{
			return other.Type == Type;
		}
		return false;
	}

	public override bool Equals(object? obj)
	{
		if (obj is SoundAttributes other)
		{
			return Equals(other);
		}
		return false;
	}

	public static bool operator ==(SoundAttributes? left, SoundAttributes? right)
	{
		if ((object)left == null)
		{
			return (object)right == null;
		}
		return left.Equals(right);
	}

	public static bool operator !=(SoundAttributes? left, SoundAttributes? right)
	{
		return !(left == right);
	}

	public override int GetHashCode()
	{
		return (Location?.GetHashCode() ?? 0) ^ Pitch.GetHashCode() ^ Volume.GetHashCode() ^ Range.GetHashCode() ^ Type.GetHashCode();
	}

	public SoundAttributes Clone()
	{
		SoundAttributes sound = new SoundAttributes();
		sound.Location = Location?.PermanentClone();
		sound.Pitch = Pitch.Clone();
		sound.Volume = Volume.Clone();
		sound.Type = Type;
		sound.Range = Range;
		return sound;
	}

	public void ToBytes(BinaryWriter writer)
	{
		writer.Write(Location != null);
		if (Location != null)
		{
			writer.Write(Location.ToShortString());
		}
		writer.Write((int)Type);
		writer.Write(Range);
		Pitch.ToBytes(writer);
		Volume.ToBytes(writer);
	}

	public static SoundAttributes FromBytes(BinaryReader reader, string version)
	{
		SoundAttributes result = new SoundAttributes();
		result.Location = (reader.ReadBoolean() ? AssetLocation.Create(reader.ReadString()) : null);
		result.Type = (EnumSoundType)reader.ReadInt32();
		result.Range = reader.ReadSingle();
		result.Pitch = NatFloat.createFromBytes(reader);
		result.Volume = NatFloat.createFromBytes(reader);
		return result;
	}

	public override string ToString()
	{
		return $"SoundAttributes {{ Location: {Location}, Pitch: {Pitch}, Volume: {Volume}, Range: {Range}, Type: {Type} }}";
	}

	static SoundAttributes()
	{
		NatFloatOne = NatFloat.One;
		RandomPitch = new NatFloat(1f, 0.25f, EnumDistribution.UNIFORM);
	}
}
