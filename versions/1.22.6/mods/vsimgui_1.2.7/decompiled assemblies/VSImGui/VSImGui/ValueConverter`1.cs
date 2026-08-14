using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace VSImGui
{
	// Token: 0x0200000F RID: 15
	[NullableContext(1)]
	[Nullable(0)]
	public class ValueConverter<[Nullable(0)] TValue> : JsonConverter where TValue : IFloatArrayValue, new()
	{
		// Token: 0x0600018D RID: 397 RVA: 0x0000ACD4 File Offset: 0x00008ED4
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(TValue);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000ACE8 File Offset: 0x00008EE8
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			TValue result = Activator.CreateInstance<TValue>();
			List<float> values = new List<float>();
			for (;;)
			{
				double? num = reader.ReadAsDouble();
				if (num == null)
				{
					break;
				}
				double value = num.GetValueOrDefault();
				values.Add((float)value);
			}
			ref TValue ptr = ref result;
			if (default(TValue) == null)
			{
				TValue tvalue = result;
				ptr = ref tvalue;
			}
			ptr.Array = values.ToArray();
			return result;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000AD58 File Offset: 0x00008F58
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			TValue input = (TValue)((object)value);
			writer.WriteStartArray();
			foreach (float item in input.Array)
			{
				writer.WriteValue(item);
			}
			writer.WriteEndArray();
		}
	}
}
