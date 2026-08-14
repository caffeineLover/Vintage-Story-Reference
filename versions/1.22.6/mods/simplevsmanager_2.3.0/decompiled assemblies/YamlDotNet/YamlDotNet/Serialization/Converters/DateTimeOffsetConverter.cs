using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
	// Token: 0x02000095 RID: 149
	[NullableContext(1)]
	[Nullable(0)]
	public class DateTimeOffsetConverter : IYamlTypeConverter
	{
		// Token: 0x06000342 RID: 834 RVA: 0x0000BE7A File Offset: 0x0000A07A
		public DateTimeOffsetConverter([Nullable(2)] IFormatProvider provider = null, ScalarStyle style = ScalarStyle.Any, DateTimeStyles dateStyle = 0, params string[] formats)
		{
			this.provider = (provider ?? CultureInfo.InvariantCulture);
			this.style = style;
			this.dateStyle = dateStyle;
			this.formats = Enumerable.ToArray<string>(Enumerable.DefaultIfEmpty<string>(formats, "O"));
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000BEB7 File Offset: 0x0000A0B7
		public bool Accepts(Type type)
		{
			return type == typeof(DateTimeOffset);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000BECC File Offset: 0x0000A0CC
		public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
		{
			string value = parser.Consume<Scalar>().Value;
			DateTimeOffset dateTimeOffset = DateTimeOffset.ParseExact(value, this.formats, this.provider, this.dateStyle);
			return dateTimeOffset;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000BF04 File Offset: 0x0000A104
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type, ObjectSerializer serializer)
		{
			string value2 = ((DateTimeOffset)value).ToString(Enumerable.First<string>(this.formats), this.provider);
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value2, this.style, true, false));
		}

		// Token: 0x040000EB RID: 235
		private readonly IFormatProvider provider;

		// Token: 0x040000EC RID: 236
		private readonly ScalarStyle style;

		// Token: 0x040000ED RID: 237
		private readonly DateTimeStyles dateStyle;

		// Token: 0x040000EE RID: 238
		private readonly string[] formats;
	}
}
