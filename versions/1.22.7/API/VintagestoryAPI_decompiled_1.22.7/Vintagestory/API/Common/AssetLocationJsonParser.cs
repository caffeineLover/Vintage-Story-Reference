using System;
using Newtonsoft.Json;

namespace Vintagestory.API.Common;

public class AssetLocationJsonParser : JsonConverter<AssetLocation>
{
	protected string domain;

	public override bool CanWrite => false;

	public AssetLocationJsonParser(string domain)
	{
		this.domain = domain;
	}

	public override AssetLocation? ReadJson(JsonReader reader, Type objectType, AssetLocation? existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		if (reader.Value is string)
		{
			return AssetLocation.Create(reader.Value as string, domain);
		}
		return null;
	}

	public override void WriteJson(JsonWriter writer, AssetLocation? value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
