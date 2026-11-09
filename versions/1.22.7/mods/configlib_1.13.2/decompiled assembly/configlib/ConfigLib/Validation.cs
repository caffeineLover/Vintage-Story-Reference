using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Datastructures;

namespace ConfigLib
{
	// Token: 0x02000028 RID: 40
	[NullableContext(2)]
	[Nullable(0)]
	public class Validation
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00008724 File Offset: 0x00006924
		// (set) Token: 0x06000155 RID: 341 RVA: 0x0000872C File Offset: 0x0000692C
		[Nullable(new byte[]
		{
			2,
			1,
			1
		})]
		public Dictionary<string, JsonObject> Mapping { [return: Nullable(new byte[]
		{
			2,
			1,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1,
			1
		})] private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00008735 File Offset: 0x00006935
		// (set) Token: 0x06000157 RID: 343 RVA: 0x0000873D File Offset: 0x0000693D
		public JsonObject Minimum { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00008746 File Offset: 0x00006946
		// (set) Token: 0x06000159 RID: 345 RVA: 0x0000874E File Offset: 0x0000694E
		public JsonObject Maximum { get; private set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00008757 File Offset: 0x00006957
		// (set) Token: 0x0600015B RID: 347 RVA: 0x0000875F File Offset: 0x0000695F
		public JsonObject Step { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00008768 File Offset: 0x00006968
		// (set) Token: 0x0600015D RID: 349 RVA: 0x00008770 File Offset: 0x00006970
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public List<JsonObject> Values { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] private set; }

		// Token: 0x0600015E RID: 350 RVA: 0x0000412E File Offset: 0x0000232E
		public Validation()
		{
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00008779 File Offset: 0x00006979
		[NullableContext(1)]
		public Validation(Dictionary<string, JsonObject> mapping)
		{
			this.Mapping = mapping;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00008788 File Offset: 0x00006988
		[NullableContext(1)]
		public Validation(List<JsonObject> values)
		{
			this.Values = values;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00008797 File Offset: 0x00006997
		public Validation(JsonObject min, JsonObject max = null, JsonObject step = null)
		{
			this.Minimum = min;
			this.Maximum = max;
			this.Step = step;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000087B4 File Offset: 0x000069B4
		[NullableContext(1)]
		[return: TupleElementNames(new string[]
		{
			"mappingKey",
			"value",
			"validation"
		})]
		[return: Nullable(new byte[]
		{
			0,
			2,
			2,
			2
		})]
		internal static ValueTuple<string, JsonObject, Validation> FromJson(JsonObject json, [Nullable(2)] string value)
		{
			Validation validation = new Validation();
			if (value != null && json.KeyExists("mapping"))
			{
				Validation.SetMapping(json["mapping"], validation);
				Dictionary<string, JsonObject> mapping = validation.Mapping;
				return new ValueTuple<string, JsonObject, Validation>(value, (mapping != null) ? mapping[value] : null, validation);
			}
			if (json.KeyExists("range"))
			{
				Validation.SetRange(json["range"], validation);
				return new ValueTuple<string, JsonObject, Validation>(null, null, validation);
			}
			if (json.KeyExists("values"))
			{
				Validation.SetValues(json["values"].AsArray(), validation);
				return new ValueTuple<string, JsonObject, Validation>(null, null, validation);
			}
			return new ValueTuple<string, JsonObject, Validation>(null, null, null);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00008860 File Offset: 0x00006A60
		[NullableContext(1)]
		private static void SetMapping(JsonObject mapping, Validation validation)
		{
			JObject jobject = mapping.Token as JObject;
			if (jobject == null)
			{
				throw new InvalidConfigException("Mapping has wrong format");
			}
			validation.Mapping = new Dictionary<string, JsonObject>();
			foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
			{
				string text;
				JToken jtoken;
				keyValuePair.Deconstruct(ref text, ref jtoken);
				string key = text;
				JToken mappingValue = jtoken;
				if (mappingValue == null)
				{
					throw new InvalidConfigException("Mapping value for entry '" + key + "' is not valid value");
				}
				validation.Mapping.Add(key, new JsonObject(mappingValue));
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00008900 File Offset: 0x00006B00
		[NullableContext(1)]
		private static void SetRange(JsonObject range, Validation validation)
		{
			JsonObject min = range.KeyExists("min") ? range["min"] : null;
			JsonObject max = range.KeyExists("max") ? range["max"] : null;
			JsonObject step = range.KeyExists("step") ? range["step"] : null;
			validation.Minimum = min;
			validation.Maximum = max;
			validation.Step = step;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00008976 File Offset: 0x00006B76
		[NullableContext(1)]
		private static void SetValues(JsonObject[] values, Validation validation)
		{
			validation.Values = Enumerable.ToList<JsonObject>(values);
		}
	}
}
