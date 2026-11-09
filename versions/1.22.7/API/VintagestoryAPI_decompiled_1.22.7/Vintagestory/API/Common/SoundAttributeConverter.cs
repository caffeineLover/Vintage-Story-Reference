using System;
using System.Linq;
using Newtonsoft.Json;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common;

public class SoundAttributeConverter : JsonConverter<SoundAttributes?>
{
	public float DefaultRange = 32f;

	public bool AddSoundsPrefix = true;

	public bool RandomPitch;

	public override bool CanWrite => false;

	public SoundAttributeConverter(bool randomPitch)
	{
		RandomPitch = randomPitch;
	}

	public SoundAttributeConverter(bool randomPitch, float defaultRange)
	{
		RandomPitch = randomPitch;
		DefaultRange = defaultRange;
	}

	public override void WriteJson(JsonWriter writer, SoundAttributes? value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}

	public override SoundAttributes? ReadJson(JsonReader reader, Type objectType, SoundAttributes? existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		if (reader.ValueType == typeof(string))
		{
			AssetLocation location = ((from x in serializer.Converters
				select x as AssetLocationJsonParser into x
				where x != null
				select x).FirstOrDefault() ?? new AssetLocationJsonParser("game")).ReadJson(reader, typeof(AssetLocation), null, hasExistingValue: false, serializer);
			ArgumentNullException.ThrowIfNull(location, "location");
			if (AddSoundsPrefix)
			{
				location.WithPathPrefixOnce("sounds/");
			}
			SoundAttributes value = new SoundAttributes(location, RandomPitch);
			value.Range = DefaultRange;
			return value;
		}
		SoundAttributes sound = new SoundAttributes();
		sound.Range = DefaultRange;
		sound.Pitch = null;
		sound.Volume = null;
		try
		{
			object boxed = sound;
			serializer.Populate(reader, boxed);
			sound = (SoundAttributes)boxed;
			if (AddSoundsPrefix)
			{
				sound.Location?.WithPathPrefixOnce("sounds/");
			}
		}
		catch (Exception innerException)
		{
			SoundAttributes value = sound;
			throw new Exception("Error while parsing " + value.ToString(), innerException);
		}
		ref NatFloat pitch = ref sound.Pitch;
		if ((object)pitch == null)
		{
			pitch = (RandomPitch ? SoundAttributes.RandomPitch : SoundAttributes.NatFloatOne);
		}
		pitch = ref sound.Volume;
		if ((object)pitch == null)
		{
			pitch = SoundAttributes.NatFloatOne;
		}
		if (!(sound.Location == null))
		{
			return sound;
		}
		return null;
	}
}
