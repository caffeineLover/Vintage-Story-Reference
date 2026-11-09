using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace ConfigLib
{
	// Token: 0x02000033 RID: 51
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class JsonObjectPath
	{
		// Token: 0x06000190 RID: 400 RVA: 0x00008F2C File Offset: 0x0000712C
		public JsonObjectPath(string path)
		{
			this._path = Enumerable.Select<string, JsonObjectPath.PathElementDelegate>(Enumerable.Where<string>(path.Split("/", 0), (string element) => element != ""), new Func<string, JsonObjectPath.PathElementDelegate>(this.Convert));
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00008F88 File Offset: 0x00007188
		public IEnumerable<JsonObject> Get(JsonObject tree)
		{
			IEnumerable<JsonObject> result = new <>z__ReadOnlySingleElementList<JsonObject>(tree);
			foreach (JsonObjectPath.PathElementDelegate pathElementDelegate in this._path)
			{
				result = pathElementDelegate(result);
				if (result == null)
				{
					return Array.Empty<JsonObject>();
				}
			}
			return result;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00008FEC File Offset: 0x000071EC
		public int Set(JsonObject tree, JsonObject value)
		{
			IEnumerable<JsonObject> result = this.Get(tree);
			foreach (JsonObject jsonObject in result)
			{
				JToken token = jsonObject.Token;
				if (token != null)
				{
					token.Replace(value.Token);
				}
			}
			return Enumerable.Count<JsonObject>(result);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00009050 File Offset: 0x00007250
		private JsonObjectPath.PathElementDelegate Convert(string element)
		{
			int index;
			if (int.TryParse(element, ref index))
			{
				return (IEnumerable<JsonObject> tree) => JsonObjectPath.PathElementByIndex(tree, index);
			}
			if (element == "-")
			{
				return (IEnumerable<JsonObject> tree) => JsonObjectPath.PathElementByAllIndexes(tree);
			}
			JsonObjectPath.PathElementDelegate rangeResult = JsonObjectPath.TryParseRange(element);
			if (rangeResult != null)
			{
				return rangeResult;
			}
			JsonObjectPath.PathElementDelegate wildcardResult = JsonObjectPath.TryParseWildcard(element);
			if (wildcardResult != null)
			{
				return wildcardResult;
			}
			JsonObjectPath.PathElementDelegate conditionResult = JsonObjectPath.TryParseCondition(element);
			if (conditionResult != null)
			{
				return conditionResult;
			}
			return (IEnumerable<JsonObject> tree) => JsonObjectPath.PathElementByKey(tree, element);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000090FC File Offset: 0x000072FC
		private static IEnumerable<JsonObject> PathElementByAllIndexes(IEnumerable<JsonObject> attributes)
		{
			List<JsonObject> result = new List<JsonObject>();
			foreach (JsonObject[] attributesArray in Enumerable.Select<JsonObject, JsonObject[]>(Enumerable.Where<JsonObject>(attributes, (JsonObject element) => element.IsArray()), (JsonObject element) => element.AsArray()))
			{
				int size = attributesArray.Length;
				for (int i = 0; i < size; i++)
				{
					result.Add(attributesArray[i]);
				}
			}
			return result;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000091AC File Offset: 0x000073AC
		private static IEnumerable<JsonObject> PathElementByIndexes(IEnumerable<JsonObject> attributes, int start, int end)
		{
			List<JsonObject> result = new List<JsonObject>();
			foreach (JsonObject[] attributesArray in Enumerable.Select<JsonObject, JsonObject[]>(Enumerable.Where<JsonObject>(attributes, (JsonObject element) => element.IsArray()), (JsonObject element) => element.AsArray()))
			{
				int size = attributesArray.Length;
				for (int i = Math.Max(0, start); i < Math.Min(end, size); i++)
				{
					result.Add(attributesArray[i]);
				}
			}
			return result;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00009268 File Offset: 0x00007468
		private static IEnumerable<JsonObject> PathElementByIndex(IEnumerable<JsonObject> attributes, int index)
		{
			List<JsonObject> result = new List<JsonObject>();
			foreach (JsonObject attribute in Enumerable.Where<JsonObject>(attributes, (JsonObject element) => element.IsArray()))
			{
				if (index >= 0 && attribute.AsArray().Length > index)
				{
					JsonObject[] jsonArray = attribute.AsArray();
					result.Add(jsonArray[index]);
				}
			}
			return result;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000092F4 File Offset: 0x000074F4
		private static IEnumerable<JsonObject> PathElementByKey(IEnumerable<JsonObject> attributes, string key)
		{
			List<JsonObject> result = new List<JsonObject>();
			foreach (JsonObject attribute in attributes)
			{
				if (attribute != null && attribute.KeyExists(key))
				{
					result.Add(attribute[key]);
				}
			}
			return result;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00009358 File Offset: 0x00007558
		private static IEnumerable<JsonObject> PathElementByWildcard(IEnumerable<JsonObject> attributes, string wildcard)
		{
			List<JsonObject> result = new List<JsonObject>();
			foreach (JObject jobject in Enumerable.OfType<JObject>(Enumerable.Select<JsonObject, JToken>(attributes, (JsonObject attribute) => attribute.Token)))
			{
				foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
				{
					string text;
					JToken jtoken;
					keyValuePair.Deconstruct(ref text, ref jtoken);
					string key = text;
					JToken value = jtoken;
					if (WildcardUtil.Match(wildcard, key) && value != null)
					{
						result.Add(new JsonObject(value));
					}
				}
			}
			return result;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00009424 File Offset: 0x00007624
		private static IEnumerable<JsonObject> PathElementByCondition(IEnumerable<JsonObject> attributes, string code, string condition)
		{
			IEnumerable<JArray> arrays = Enumerable.OfType<JArray>(Enumerable.Select<JsonObject, JToken>(attributes, (JsonObject element) => element.Token));
			IEnumerable<JObject> objects = Enumerable.OfType<JObject>(Enumerable.Select<JsonObject, JToken>(attributes, (JsonObject element) => element.Token));
			IEnumerable<JToken> tokens = Array.Empty<JToken>();
			if (Enumerable.Any<JArray>(arrays))
			{
				tokens = Enumerable.Concat<JToken>(tokens, Enumerable.Aggregate<IEnumerable<JToken>>(Enumerable.Select<JArray, IEnumerable<JToken>>(arrays, (JArray a) => a), (IEnumerable<JToken> a, IEnumerable<JToken> b) => Enumerable.Concat<JToken>(a, b)));
			}
			if (Enumerable.Any<JObject>(objects))
			{
				tokens = Enumerable.Concat<JToken>(tokens, Enumerable.Aggregate<IEnumerable<JToken>>(Enumerable.Select<JObject, IEnumerable<JToken>>(objects, (JObject a) => a), (IEnumerable<JToken> a, IEnumerable<JToken> b) => Enumerable.Concat<JToken>(a, b)));
			}
			IEnumerable<JsonObject> enumerable = Enumerable.Where<JsonObject>(Enumerable.Select<JObject, JsonObject>(Enumerable.OfType<JObject>(tokens), (JObject a) => new JsonObject(a)), (JsonObject a) => a.KeyExists(code) && a[code].AsString(null) == condition);
			IEnumerable<JsonObject> fromProperties = Enumerable.Where<JsonObject>(Enumerable.Select<JProperty, JsonObject>(Enumerable.OfType<JProperty>(tokens), (JProperty a) => new JsonObject(a.Value)), (JsonObject a) => a.KeyExists(code) && a[code].AsString(null) == condition);
			return Enumerable.Concat<JsonObject>(enumerable, fromProperties);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000095D0 File Offset: 0x000077D0
		[return: Nullable(2)]
		private static JsonObjectPath.PathElementDelegate TryParseRange(string element)
		{
			if (element.Contains('-'))
			{
				string[] indexes = element.Split('-', 0);
				if (indexes.Length != 2)
				{
					return null;
				}
				int start;
				bool parsedStart = int.TryParse(indexes[0], ref start);
				int end;
				bool parsedEnd = int.TryParse(indexes[1], ref end);
				if (!parsedStart || !parsedEnd)
				{
					return null;
				}
				return (IEnumerable<JsonObject> tree) => JsonObjectPath.PathElementByIndexes(tree, start, end);
			}
			else
			{
				if (!element.Contains(".."))
				{
					return null;
				}
				string[] indexes2 = element.Split("..", 0);
				if (indexes2.Length != 2)
				{
					return null;
				}
				int start;
				bool parsedStart2 = int.TryParse(indexes2[0], ref start);
				int end;
				bool parsedEnd2 = int.TryParse(indexes2[1], ref end);
				if (!parsedStart2 || !parsedEnd2)
				{
					return null;
				}
				return (IEnumerable<JsonObject> tree) => JsonObjectPath.PathElementByIndexes(tree, start, end + 1);
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000969C File Offset: 0x0000789C
		[return: Nullable(2)]
		private static JsonObjectPath.PathElementDelegate TryParseWildcard(string element)
		{
			if (!element.StartsWith("@@"))
			{
				return null;
			}
			string wildcard = element.Substring(2, element.Length - 2);
			return (IEnumerable<JsonObject> tree) => JsonObjectPath.PathElementByWildcard(tree, wildcard);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000096E0 File Offset: 0x000078E0
		[return: Nullable(2)]
		private static JsonObjectPath.PathElementDelegate TryParseCondition(string element)
		{
			if (!element.Contains("="))
			{
				return null;
			}
			string[] parts = element.Split("=", 0);
			if (parts.Length != 2)
			{
				return null;
			}
			string code = parts[0];
			string condition = parts[1];
			return (IEnumerable<JsonObject> tree) => JsonObjectPath.PathElementByCondition(tree, code, condition);
		}

		// Token: 0x040000AE RID: 174
		private readonly IEnumerable<JsonObjectPath.PathElementDelegate> _path;

		// Token: 0x02000034 RID: 52
		// (Invoke) Token: 0x0600019E RID: 414
		[NullableContext(0)]
		private delegate IEnumerable<JsonObject> PathElementDelegate(IEnumerable<JsonObject> attribute);
	}
}
