using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
	// Token: 0x02000168 RID: 360
	[NullableContext(1)]
	[Nullable(0)]
	public class DateTimeOffsetConverter : IYamlTypeConverter
	{
		// Token: 0x06000746 RID: 1862 RVA: 0x0001922E File Offset: 0x0001742E
		public DateTimeOffsetConverter([Nullable(2)] IFormatProvider provider = null, ScalarStyle style = ScalarStyle.Any, DateTimeStyles dateStyle = 0, params string[] formats)
		{
			this.provider = (provider ?? CultureInfo.InvariantCulture);
			this.style = style;
			this.dateStyle = dateStyle;
			this.formats = Enumerable.ToArray<string>(Enumerable.DefaultIfEmpty<string>(formats, "O"));
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0001926B File Offset: 0x0001746B
		public bool Accepts(Type type)
		{
			return type == typeof(DateTimeOffset);
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0001927D File Offset: 0x0001747D
		public object ReadYaml(IParser parser, Type type)
		{
			return DateTimeOffset.ParseExact(parser.Consume<Scalar>().Value, this.formats, this.provider, this.dateStyle);
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x000192A8 File Offset: 0x000174A8
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type)
		{
			string value2 = ((DateTimeOffset)value).ToString(Enumerable.First<string>(this.formats), this.provider);
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value2, this.style, true, false));
		}

		// Token: 0x0400031C RID: 796
		private readonly IFormatProvider provider;

		// Token: 0x0400031D RID: 797
		private readonly ScalarStyle style;

		// Token: 0x0400031E RID: 798
		private readonly DateTimeStyles dateStyle;

		// Token: 0x0400031F RID: 799
		private readonly string[] formats;
	}
}
