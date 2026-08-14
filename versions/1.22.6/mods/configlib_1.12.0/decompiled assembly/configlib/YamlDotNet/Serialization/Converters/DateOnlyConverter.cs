using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
	// Token: 0x0200016A RID: 362
	[NullableContext(1)]
	[Nullable(0)]
	public class DateOnlyConverter : IYamlTypeConverter
	{
		// Token: 0x06000779 RID: 1913 RVA: 0x00019971 File Offset: 0x00017B71
		public DateOnlyConverter([Nullable(2)] IFormatProvider provider = null, bool doubleQuotes = false, params string[] formats)
		{
			this.provider = (provider ?? CultureInfo.InvariantCulture);
			this.doubleQuotes = doubleQuotes;
			this.formats = Enumerable.ToArray<string>(Enumerable.DefaultIfEmpty<string>(formats, "d"));
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x000199A6 File Offset: 0x00017BA6
		public bool Accepts(Type type)
		{
			return type == typeof(DateOnly);
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x000199B8 File Offset: 0x00017BB8
		public object ReadYaml(IParser parser, Type type)
		{
			return DateOnly.ParseExact(parser.Consume<Scalar>().Value, this.formats, this.provider, 0);
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x000199DC File Offset: 0x00017BDC
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type)
		{
			string value2 = ((DateOnly)value).ToString(Enumerable.First<string>(this.formats), this.provider);
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value2, this.doubleQuotes ? ScalarStyle.DoubleQuoted : ScalarStyle.Any, true, false));
		}

		// Token: 0x04000343 RID: 835
		private readonly IFormatProvider provider;

		// Token: 0x04000344 RID: 836
		private readonly bool doubleQuotes;

		// Token: 0x04000345 RID: 837
		private readonly string[] formats;
	}
}
