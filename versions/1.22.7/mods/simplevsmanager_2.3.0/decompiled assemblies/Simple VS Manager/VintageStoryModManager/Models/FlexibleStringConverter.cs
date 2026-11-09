using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000A2 RID: 162
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1
	})]
	public sealed class FlexibleStringConverter : JsonConverter<string>
	{
		// Token: 0x06000DE2 RID: 3554 RVA: 0x00051720 File Offset: 0x0004F920
		public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
			case 7:
				return reader.GetString() ?? string.Empty;
			case 8:
			{
				long num;
				return reader.TryGetInt64(ref num) ? num.ToString(CultureInfo.InvariantCulture) : reader.GetDouble().ToString(CultureInfo.InvariantCulture);
			}
			case 11:
				return string.Empty;
			}
			return string.Empty;
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x000517A1 File Offset: 0x0004F9A1
		public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value ?? string.Empty);
		}
	}
}
