using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
	// Token: 0x0200016B RID: 363
	[NullableContext(1)]
	[Nullable(0)]
	public class DateTimeConverter : IYamlTypeConverter
	{
		// Token: 0x0600077D RID: 1917 RVA: 0x00019A30 File Offset: 0x00017C30
		public DateTimeConverter(DateTimeKind kind = 1, [Nullable(2)] IFormatProvider provider = null, bool doubleQuotes = false, params string[] formats)
		{
			this.kind = ((kind == null) ? 1 : kind);
			this.provider = (provider ?? CultureInfo.InvariantCulture);
			this.doubleQuotes = doubleQuotes;
			this.formats = Enumerable.ToArray<string>(Enumerable.DefaultIfEmpty<string>(formats, "G"));
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00019A7E File Offset: 0x00017C7E
		public bool Accepts(Type type)
		{
			return type == typeof(DateTime);
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00019A90 File Offset: 0x00017C90
		public object ReadYaml(IParser parser, Type type)
		{
			string value = parser.Consume<Scalar>().Value;
			DateTimeStyles dateTimeStyles = (this.kind == 2) ? 32 : 64;
			return DateTimeConverter.EnsureDateTimeKind(DateTime.ParseExact(value, this.formats, this.provider, dateTimeStyles), this.kind);
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00019ADC File Offset: 0x00017CDC
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type)
		{
			DateTime dateTime = (DateTime)value;
			string value2 = ((this.kind == 2) ? dateTime.ToLocalTime() : dateTime.ToUniversalTime()).ToString(Enumerable.First<string>(this.formats), this.provider);
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value2, this.doubleQuotes ? ScalarStyle.DoubleQuoted : ScalarStyle.Any, true, false));
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x00019B47 File Offset: 0x00017D47
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

		// Token: 0x04000346 RID: 838
		private readonly DateTimeKind kind;

		// Token: 0x04000347 RID: 839
		private readonly IFormatProvider provider;

		// Token: 0x04000348 RID: 840
		private readonly bool doubleQuotes;

		// Token: 0x04000349 RID: 841
		private readonly string[] formats;
	}
}
