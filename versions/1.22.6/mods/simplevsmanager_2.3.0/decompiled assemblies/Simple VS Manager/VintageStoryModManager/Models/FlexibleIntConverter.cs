using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000A1 RID: 161
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FlexibleIntConverter : JsonConverter<int>
	{
		// Token: 0x06000DDF RID: 3551 RVA: 0x000516A4 File Offset: 0x0004F8A4
		public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
			case 7:
			{
				int num;
				return int.TryParse(reader.GetString(), 511, CultureInfo.InvariantCulture, ref num) ? num : 0;
			}
			case 8:
			{
				int num2;
				return reader.TryGetInt32(ref num2) ? num2 : 0;
			}
			case 11:
				return 0;
			}
			return 0;
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0005170E File Offset: 0x0004F90E
		public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
		{
			writer.WriteNumberValue(value);
		}
	}
}
