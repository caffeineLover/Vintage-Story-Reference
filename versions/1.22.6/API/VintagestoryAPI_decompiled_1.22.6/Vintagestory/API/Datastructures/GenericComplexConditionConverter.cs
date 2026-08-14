using System;
using Newtonsoft.Json;

namespace Vintagestory.API.Datastructures;

public sealed class GenericComplexConditionConverter : JsonConverter
{
	public static ComplexConditionConverter<TagSet> CollectibleConverter;

	public static ComplexConditionConverter<TagSetFast> EntityConverter;

	/// <summary> Called from Main init as soon as the registries are available. </summary>
	public static void StaticInit(ITagRegistry<TagSet> collectibleRegistry, ITagRegistry<TagSetFast> entityRegistry)
	{
		CollectibleConverter = new ComplexConditionConverter<TagSet>(collectibleRegistry);
		EntityConverter = new ComplexConditionConverter<TagSetFast>(entityRegistry);
	}

	public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
	{
		if (objectType == typeof(ComplexTagCondition<TagSet>))
		{
			return CollectibleConverter.ReadJson(reader, objectType, existingValue, serializer);
		}
		return EntityConverter.ReadJson(reader, objectType, existingValue, serializer);
	}

	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}

	public override bool CanConvert(Type objectType)
	{
		return objectType.GetGenericTypeDefinition() == typeof(ComplexTagCondition<>);
	}
}
