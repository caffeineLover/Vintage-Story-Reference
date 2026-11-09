using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Vintagestory.API.Datastructures;

public sealed class ComplexConditionConverter<TTagSet>(ITagRegistry<TTagSet> registry) : JsonConverter<ComplexTagCondition<TTagSet>> where TTagSet : IEquatable<TTagSet>
{
	private enum OuterParseMode
	{
		Unknown,
		Simple,
		Complex
	}

	private enum ConditionParsingMode
	{
		Disjunctive,
		Conjunctive
	}

	private readonly ITagRegistry<TTagSet> registry = registry;

	public override ComplexTagCondition<TTagSet> ReadJson(JsonReader reader, Type objectType, ComplexTagCondition<TTagSet> existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		JToken rootToken = JToken.ReadFrom(reader);
		return ReadJson(rootToken);
	}

	public ComplexTagCondition<TTagSet> ReadJson(JToken rootToken)
	{
		if (rootToken is JArray rootArray)
		{
			JTokenType? firstType = rootArray.First?.Type;
			if (firstType == JTokenType.String)
			{
				return new ComplexTagCondition<TTagSet>
				{
					conditions = new ComplexTagCondition<TTagSet>.Condition[1]
					{
						new ComplexTagCondition<TTagSet>.Condition
						{
							RequiredTags = parseSet(rootArray)
						}
					},
					isDisjunctive = true
				};
			}
			if (!firstType.HasValue)
			{
				return default(ComplexTagCondition<TTagSet>);
			}
			ComplexTagCondition<TTagSet>.Condition[] conditions = parseArrayOfConditions(rootArray, ConditionParsingMode.Conjunctive);
			return new ComplexTagCondition<TTagSet>
			{
				conditions = conditions,
				isDisjunctive = true
			};
		}
		if (rootToken is JObject rootObject)
		{
			OuterParseMode outerMode = OuterParseMode.Unknown;
			ComplexTagCondition<TTagSet>.Condition simpleCondition = default(ComplexTagCondition<TTagSet>.Condition);
			ComplexTagCondition<TTagSet> resultComplexCondition = default(ComplexTagCondition<TTagSet>);
			foreach (var (name, valueToken) in rootObject)
			{
				switch (name)
				{
				case "allOf":
				{
					if (!(valueToken is JArray valueArray2))
					{
						throw new InvalidOperationException("Error while parsing tag condition, allOf must be an array. json:\n" + valueToken);
					}
					JToken? first2 = valueArray2.First;
					if (first2 != null && first2.Type == JTokenType.String)
					{
						if (outerMode == OuterParseMode.Complex)
						{
							throw new InvalidOperationException("Error while parsing tag condition, cannot have both anyOf and allOf here. json:\n" + valueToken);
						}
						outerMode = OuterParseMode.Simple;
						simpleCondition.RequiredTags = parseSet(valueArray2);
						resultComplexCondition.isDisjunctive = true;
					}
					else
					{
						if (outerMode == OuterParseMode.Simple)
						{
							throw new InvalidOperationException("Error while parsing tag condition, allOf must be an array of inner conditions here. json:\n" + valueToken);
						}
						outerMode = OuterParseMode.Complex;
						resultComplexCondition.conditions = parseArrayOfConditions(valueArray2, ConditionParsingMode.Disjunctive);
						resultComplexCondition.isDisjunctive = false;
					}
					break;
				}
				case "anyOf":
				{
					if (!(valueToken is JArray valueArray))
					{
						throw new InvalidOperationException("Error while parsing tag condition, anyOf must be an array. json:\n" + valueToken);
					}
					JToken? first = valueArray.First;
					if (first != null && first.Type == JTokenType.String)
					{
						if (outerMode == OuterParseMode.Complex)
						{
							throw new InvalidOperationException("Error while parsing tag condition, anyOf must be an array of inner conditions here. json:\n" + valueToken);
						}
						outerMode = OuterParseMode.Simple;
						simpleCondition.RequiredTags = parseSet(valueArray);
						resultComplexCondition.isDisjunctive = false;
					}
					else
					{
						if (outerMode == OuterParseMode.Simple)
						{
							throw new InvalidOperationException("Error while parsing tag condition, allOf must be an array of inner conditions here. json:\n" + valueToken);
						}
						outerMode = OuterParseMode.Complex;
						resultComplexCondition.conditions = parseArrayOfConditions(valueArray, ConditionParsingMode.Conjunctive);
						resultComplexCondition.isDisjunctive = true;
					}
					break;
				}
				case "noneOf":
					if (outerMode == OuterParseMode.Complex)
					{
						throw new InvalidOperationException("Error while parsing tag condition, noneOff cannot be used here with complex matching. Add it to the inner conditions instead. json:\n" + valueToken);
					}
					outerMode = OuterParseMode.Simple;
					if (valueToken is JArray valueArray3)
					{
						JToken? first3 = valueArray3.First;
						if (first3 != null && first3.Type == JTokenType.String)
						{
							simpleCondition.ForbiddenTags = parseSet(valueArray3);
							break;
						}
					}
					throw new InvalidOperationException("Error while parsing tag condition, noneOf must be an array of string here. json:\n" + valueToken);
				default:
					throw new InvalidOperationException("Error while parsing tag condition, unknown condition property '" + name + "'. json:\n" + valueToken);
				}
			}
			if (outerMode == OuterParseMode.Simple)
			{
				resultComplexCondition.conditions = new ComplexTagCondition<TTagSet>.Condition[1] { simpleCondition };
			}
			return resultComplexCondition;
		}
		throw new InvalidOperationException("Error while parsing tag condition, must be an array or object. json:\n" + rootToken.ToString());
	}

	private ComplexTagCondition<TTagSet>.Condition[] parseArrayOfConditions(JArray array, ConditionParsingMode mode)
	{
		ComplexTagCondition<TTagSet>.Condition[] conditions = new ComplexTagCondition<TTagSet>.Condition[array.Count];
		int i = 0;
		foreach (JToken element in array)
		{
			conditions[i++] = parseIndividualCondition(element, mode);
		}
		return conditions;
	}

	private ComplexTagCondition<TTagSet>.Condition parseIndividualCondition(JToken token, ConditionParsingMode mode)
	{
		if (token is JArray conditionArray)
		{
			JTokenType? firstType = conditionArray.FirstOrDefault()?.Type;
			if (firstType == JTokenType.String)
			{
				return new ComplexTagCondition<TTagSet>.Condition
				{
					RequiredTags = parseSet(conditionArray)
				};
			}
			if (!firstType.HasValue)
			{
				return default(ComplexTagCondition<TTagSet>.Condition);
			}
			throw new InvalidOperationException("Error while parsing tag condition, individual conditions must be arrays of strings or objects, got json:\n" + token.ToString());
		}
		if (token is JObject conditionObject)
		{
			ComplexTagCondition<TTagSet>.Condition condition = default(ComplexTagCondition<TTagSet>.Condition);
			{
				foreach (var (name, valueToken) in conditionObject)
				{
					switch (name)
					{
					case "allOf":
						if (mode != ConditionParsingMode.Conjunctive)
						{
							throw new InvalidOperationException("Error while parsing tag condition, cannot mix disjunctive and conjunctive forms. json:\n" + token.ToString());
						}
						if (valueToken is JArray valueArray)
						{
							JToken? first = valueArray.First;
							if (first != null && first.Type == JTokenType.String)
							{
								condition.RequiredTags = parseSet(valueArray);
								break;
							}
						}
						throw new InvalidOperationException("Error while parsing tag condition, allOf must be an array of string here. json:\n" + valueToken);
					case "anyOf":
						if (mode != ConditionParsingMode.Disjunctive)
						{
							throw new InvalidOperationException("Error while parsing tag condition, cannot mix disjunctive and conjunctive forms. json:\n" + token.ToString());
						}
						if (valueToken is JArray valueArray2)
						{
							JToken? first2 = valueArray2.First;
							if (first2 != null && first2.Type == JTokenType.String)
							{
								condition.RequiredTags = parseSet(valueArray2);
								break;
							}
						}
						throw new InvalidOperationException("Error while parsing tag condition, anyOf must be an array of string here. json:\n" + valueToken);
					case "noneOf":
						if (!(valueToken is JArray valueArray3))
						{
							throw new InvalidOperationException("Error while parsing tag condition, noneOf must be an array of string. json:\n" + valueToken);
						}
						condition.ForbiddenTags = parseSet(valueArray3);
						break;
					default:
						throw new InvalidOperationException("Error while parsing tag condition, unknown condition property '" + name + "'. json:\n" + valueToken);
					}
				}
				return condition;
			}
		}
		throw new InvalidOperationException("Error while parsing tag condition, must be an array or object. json:\n" + token.ToString());
	}

	private TTagSet parseSet(JArray array)
	{
		IEnumerable<string> tags = from e in array.Where(delegate(JToken t)
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

	public override void WriteJson(JsonWriter writer, ComplexTagCondition<TTagSet> value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
