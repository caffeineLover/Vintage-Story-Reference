using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
	// Token: 0x0200016C RID: 364
	[NullableContext(1)]
	[Nullable(0)]
	public class DateTimeOffsetConverter : IYamlTypeConverter
	{
		// Token: 0x06000782 RID: 1922 RVA: 0x00019B76 File Offset: 0x00017D76
		public DateTimeOffsetConverter([Nullable(2)] IFormatProvider provider = null, ScalarStyle style = ScalarStyle.Any, DateTimeStyles dateStyle = 0, params string[] formats)
		{
			this.provider = (provider ?? CultureInfo.InvariantCulture);
			this.style = style;
			this.dateStyle = dateStyle;
			this.formats = Enumerable.ToArray<string>(Enumerable.DefaultIfEmpty<string>(formats, "O"));
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x00019BB3 File Offset: 0x00017DB3
		public bool Accepts(Type type)
		{
			return type == typeof(DateTimeOffset);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x00019BC5 File Offset: 0x00017DC5
		public object ReadYaml(IParser parser, Type type)
		{
			return DateTimeOffset.ParseExact(parser.Consume<Scalar>().Value, this.formats, this.provider, this.dateStyle);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x00019BF0 File Offset: 0x00017DF0
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type)
		{
			string value2 = ((DateTimeOffset)value).ToString(Enumerable.First<string>(this.formats), this.provider);
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value2, this.style, true, false));
		}

		// Token: 0x0400034A RID: 842
		private readonly IFormatProvider provider;

		// Token: 0x0400034B RID: 843
		private readonly ScalarStyle style;

		// Token: 0x0400034C RID: 844
		private readonly DateTimeStyles dateStyle;

		// Token: 0x0400034D RID: 845
		private readonly string[] formats;
	}
}
