using System;
using Newtonsoft.Json;

namespace Vintagestory.API.Datastructures;

public sealed class EntityTagSetConverter : JsonConverter<TagSetFast>
{
	public static TagSetConverter<TagSetFast> ProxyInstance;

	/// <summary> Called from Main init as soon as the registries are available. </summary>
	public static void StaticInit(ITagRegistry<TagSetFast> registry)
	{
		ProxyInstance = new TagSetConverter<TagSetFast>(registry);
	}

	public override TagSetFast ReadJson(JsonReader reader, Type objectType, TagSetFast existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		return ProxyInstance.ReadJson(reader, objectType, existingValue, hasExistingValue, serializer);
	}

	public override void WriteJson(JsonWriter writer, TagSetFast value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
