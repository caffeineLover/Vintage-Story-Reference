using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Datastructures;

namespace ConfigLib
{
	// Token: 0x02000026 RID: 38
	[NullableContext(2)]
	[Nullable(0)]
	public class Validation
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00008408 File Offset: 0x00006608
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00008410 File Offset: 0x00006610
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

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00008419 File Offset: 0x00006619
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00008421 File Offset: 0x00006621
		public JsonObject Minimum { get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000133 RID: 307 RVA: 0x0000842A File Offset: 0x0000662A
		// (set) Token: 0x06000134 RID: 308 RVA: 0x00008432 File Offset: 0x00006632
		public JsonObject Maximum { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000135 RID: 309 RVA: 0x0000843B File Offset: 0x0000663B
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00008443 File Offset: 0x00006643
		public JsonObject Step { get; private set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000137 RID: 311 RVA: 0x0000844C File Offset: 0x0000664C
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00008454 File Offset: 0x00006654
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

		// Token: 0x06000139 RID: 313 RVA: 0x00003E0A File Offset: 0x0000200A
		public Validation()
		{
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000845D File Offset: 0x0000665D
		[NullableContext(1)]
		public Validation(Dictionary<string, JsonObject> mapping)
		{
			this.Mapping = mapping;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000846C File Offset: 0x0000666C
		[NullableContext(1)]
		public Validation(List<JsonObject> values)
		{
			this.Values = values;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000847B File Offset: 0x0000667B
		public Validation(JsonObject min, JsonObject max = null, JsonObject step = null)
		{
			this.Minimum = min;
			this.Maximum = max;
			this.Step = step;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00008498 File Offset: 0x00006698
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

		// Token: 0x0600013E RID: 318 RVA: 0x00008544 File Offset: 0x00006744
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

		// Token: 0x0600013F RID: 319 RVA: 0x000085E4 File Offset: 0x000067E4
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

		// Token: 0x06000140 RID: 320 RVA: 0x0000865A File Offset: 0x0000685A
		[NullableContext(1)]
		private static void SetValues(JsonObject[] values, Validation validation)
		{
			validation.Values = Enumerable.ToList<JsonObject>(values);
		}
	}
}
