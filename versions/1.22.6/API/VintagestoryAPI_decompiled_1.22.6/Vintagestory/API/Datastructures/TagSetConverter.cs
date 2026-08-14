using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Vintagestory.API.Datastructures;

public sealed class TagSetConverter<TTagSet>(ITagRegistry<TTagSet> registry) : JsonConverter<TTagSet> where TTagSet : struct
{
	/// <summary> Initialized from Main init. </summary>
	private readonly ITagRegistry<TTagSet> registry = registry;

	public override TTagSet ReadJson(JsonReader reader, Type objectType, TTagSet existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		return ReadJson(JToken.ReadFrom(reader));
	}

	public TTagSet ReadJson(JToken rootToken)
	{
		if (rootToken is JArray rootArray)
		{
			JTokenType? firstType = rootArray.First?.Type;
			if (firstType == JTokenType.String)
			{
				IEnumerable<string> tags = from e in rootArray.Where(delegate(JToken t)
					{
						if (t.Type != JTokenType.String)
						{
							return false;
						}
						string text = (string?)t;
						TagValidationError tagValidationError = TagRegistry.ValidateTag(text.AsSpan());
						if (tagValidationError == TagValidationError.None)
						{
							return true;
						}
						registry.logger.Debug($"[{registry.debugName}] [{t.Path}] " + TagRegistry.FormatIssueMessage(tagValidationError, text.AsSpan()));
						return false;
					})
					select (string?)e;
				registry.TryRegisterAndCreateTagSetAndLogIssues(out var set, tags);
				return set;
			}
			if (!firstType.HasValue)
			{
				return default(TTagSet);
			}
		}
		throw new InvalidOperationException("Error while parsing tag set, must be an array of strings. json:\n" + rootToken);
	}

	public override void WriteJson(JsonWriter writer, TTagSet value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
