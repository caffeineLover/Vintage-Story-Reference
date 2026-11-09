using System;
using Newtonsoft.Json;

namespace Vintagestory.API.Datastructures;

public sealed class CollectibleTagSetConverter : JsonConverter<TagSet>
{
	public static TagSetConverter<TagSet> ProxyInstance;

	/// <summary> Called from Main init as soon as the registries are available. </summary>
	public static void StaticInit(ITagRegistry<TagSet> registry)
	{
		ProxyInstance = new TagSetConverter<TagSet>(registry);
	}

	public override TagSet ReadJson(JsonReader reader, Type objectType, TagSet existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		return ProxyInstance.ReadJson(reader, objectType, existingValue, hasExistingValue, serializer);
	}

	public override void WriteJson(JsonWriter writer, TagSet value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
