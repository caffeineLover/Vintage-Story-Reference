using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
	// Token: 0x02000167 RID: 359
	[NullableContext(1)]
	[Nullable(0)]
	public class DateTimeConverter : IYamlTypeConverter
	{
		// Token: 0x06000741 RID: 1857 RVA: 0x000190E8 File Offset: 0x000172E8
		public DateTimeConverter(DateTimeKind kind = 1, [Nullable(2)] IFormatProvider provider = null, bool doubleQuotes = false, params string[] formats)
		{
			this.kind = ((kind == null) ? 1 : kind);
			this.provider = (provider ?? CultureInfo.InvariantCulture);
			this.doubleQuotes = doubleQuotes;
			this.formats = Enumerable.ToArray<string>(Enumerable.DefaultIfEmpty<string>(formats, "G"));
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00019136 File Offset: 0x00017336
		public bool Accepts(Type type)
		{
			return type == typeof(DateTime);
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00019148 File Offset: 0x00017348
		public object ReadYaml(IParser parser, Type type)
		{
			string value = parser.Consume<Scalar>().Value;
			DateTimeStyles dateTimeStyles = (this.kind == 2) ? 32 : 64;
			return DateTimeConverter.EnsureDateTimeKind(DateTime.ParseExact(value, this.formats, this.provider, dateTimeStyles), this.kind);
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00019194 File Offset: 0x00017394
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type)
		{
			DateTime dateTime = (DateTime)value;
			string value2 = ((this.kind == 2) ? dateTime.ToLocalTime() : dateTime.ToUniversalTime()).ToString(Enumerable.First<string>(this.formats), this.provider);
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value2, this.doubleQuotes ? ScalarStyle.DoubleQuoted : ScalarStyle.Any, true, false));
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x000191FF File Offset: 0x000173FF
		private static DateTime EnsureDateTimeKind(DateTime dt, DateTimeKind kind)
		{
			if (dt.Kind == 2 && kind == 1)
			{
				return dt.ToUniversalTime();
			}
			if (dt.Kind == 1 && kind == 2)
			{
				return dt.ToLocalTime();
			}
			return dt;
		}

		// Token: 0x04000318 RID: 792
		private readonly DateTimeKind kind;

		// Token: 0x04000319 RID: 793
		private readonly IFormatProvider provider;

		// Token: 0x0400031A RID: 794
		private readonly bool doubleQuotes;

		// Token: 0x0400031B RID: 795
		private readonly string[] formats;
	}
}
