using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
	// Token: 0x0200016F RID: 367
	[NullableContext(1)]
	[Nullable(0)]
	public class TimeOnlyConverter : IYamlTypeConverter
	{
		// Token: 0x0600078E RID: 1934 RVA: 0x00019D0E File Offset: 0x00017F0E
		public TimeOnlyConverter([Nullable(2)] IFormatProvider provider = null, bool doubleQuotes = false, params string[] formats)
		{
			this.provider = (provider ?? CultureInfo.InvariantCulture);
			this.doubleQuotes = doubleQuotes;
			this.formats = Enumerable.ToArray<string>(Enumerable.DefaultIfEmpty<string>(formats, "T"));
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00019D43 File Offset: 0x00017F43
		public bool Accepts(Type type)
		{
			return type == typeof(TimeOnly);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00019D55 File Offset: 0x00017F55
		public object ReadYaml(IParser parser, Type type)
		{
			return TimeOnly.ParseExact(parser.Consume<Scalar>().Value, this.formats, this.provider, 0);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00019D7C File Offset: 0x00017F7C
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type)
		{
			string value2 = ((TimeOnly)value).ToString(Enumerable.First<string>(this.formats), this.provider);
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value2, this.doubleQuotes ? ScalarStyle.DoubleQuoted : ScalarStyle.Any, true, false));
		}

		// Token: 0x0400034F RID: 847
		private readonly IFormatProvider provider;

		// Token: 0x04000350 RID: 848
		private readonly bool doubleQuotes;

		// Token: 0x04000351 RID: 849
		private readonly string[] formats;
	}
}
