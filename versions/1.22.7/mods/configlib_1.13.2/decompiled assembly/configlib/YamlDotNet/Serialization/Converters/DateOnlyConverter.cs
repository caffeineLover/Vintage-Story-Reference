using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
	// Token: 0x02000166 RID: 358
	[NullableContext(1)]
	[Nullable(0)]
	public class DateOnlyConverter : IYamlTypeConverter
	{
		// Token: 0x0600073D RID: 1853 RVA: 0x00019029 File Offset: 0x00017229
		public DateOnlyConverter([Nullable(2)] IFormatProvider provider = null, bool doubleQuotes = false, params string[] formats)
		{
			this.provider = (provider ?? CultureInfo.InvariantCulture);
			this.doubleQuotes = doubleQuotes;
			this.formats = Enumerable.ToArray<string>(Enumerable.DefaultIfEmpty<string>(formats, "d"));
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0001905E File Offset: 0x0001725E
		public bool Accepts(Type type)
		{
			return type == typeof(DateOnly);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00019070 File Offset: 0x00017270
		public object ReadYaml(IParser parser, Type type)
		{
			return DateOnly.ParseExact(parser.Consume<Scalar>().Value, this.formats, this.provider, 0);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00019094 File Offset: 0x00017294
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type)
		{
			string value2 = ((DateOnly)value).ToString(Enumerable.First<string>(this.formats), this.provider);
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value2, this.doubleQuotes ? ScalarStyle.DoubleQuoted : ScalarStyle.Any, true, false));
		}

		// Token: 0x04000315 RID: 789
		private readonly IFormatProvider provider;

		// Token: 0x04000316 RID: 790
		private readonly bool doubleQuotes;

		// Token: 0x04000317 RID: 791
		private readonly string[] formats;
	}
}
