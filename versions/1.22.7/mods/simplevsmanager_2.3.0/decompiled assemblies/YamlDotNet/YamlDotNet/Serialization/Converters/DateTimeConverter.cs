using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
	// Token: 0x02000094 RID: 148
	[NullableContext(1)]
	[Nullable(0)]
	public class DateTimeConverter : IYamlTypeConverter
	{
		// Token: 0x0600033D RID: 829 RVA: 0x0000BD20 File Offset: 0x00009F20
		public DateTimeConverter(DateTimeKind kind = 1, [Nullable(2)] IFormatProvider provider = null, bool doubleQuotes = false, params string[] formats)
		{
			this.kind = ((kind == null) ? 1 : kind);
			this.provider = (provider ?? CultureInfo.InvariantCulture);
			this.doubleQuotes = doubleQuotes;
			this.formats = Enumerable.ToArray<string>(Enumerable.DefaultIfEmpty<string>(formats, "G"));
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000BD6E File Offset: 0x00009F6E
		public bool Accepts(Type type)
		{
			return type == typeof(DateTime);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000BD80 File Offset: 0x00009F80
		public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
		{
			string value = parser.Consume<Scalar>().Value;
			DateTimeStyles dateTimeStyles = (this.kind == 2) ? 32 : 64;
			DateTime dateTime = DateTime.ParseExact(value, this.formats, this.provider, dateTimeStyles);
			dateTime = DateTimeConverter.EnsureDateTimeKind(dateTime, this.kind);
			return dateTime;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000BDD0 File Offset: 0x00009FD0
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type, ObjectSerializer serializer)
		{
			DateTime dateTime = (DateTime)value;
			string value2 = ((this.kind == 2) ? dateTime.ToLocalTime() : dateTime.ToUniversalTime()).ToString(Enumerable.First<string>(this.formats), this.provider);
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value2, this.doubleQuotes ? ScalarStyle.DoubleQuoted : ScalarStyle.Any, true, false));
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000BE3C File Offset: 0x0000A03C
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

		// Token: 0x040000E7 RID: 231
		private readonly DateTimeKind kind;

		// Token: 0x040000E8 RID: 232
		private readonly IFormatProvider provider;

		// Token: 0x040000E9 RID: 233
		private readonly bool doubleQuotes;

		// Token: 0x040000EA RID: 234
		private readonly string[] formats;
	}
}
