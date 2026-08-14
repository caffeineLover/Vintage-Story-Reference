using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
	// Token: 0x02000092 RID: 146
	[NullableContext(1)]
	[Nullable(0)]
	public class DateOnlyConverter : IYamlTypeConverter
	{
		// Token: 0x06000334 RID: 820 RVA: 0x0000BBAC File Offset: 0x00009DAC
		public DateOnlyConverter([Nullable(2)] IFormatProvider provider = null, bool doubleQuotes = false, params string[] formats)
		{
			this.provider = (provider ?? CultureInfo.InvariantCulture);
			this.doubleQuotes = doubleQuotes;
			this.formats = Enumerable.ToArray<string>(Enumerable.DefaultIfEmpty<string>(formats, "d"));
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000BBE1 File Offset: 0x00009DE1
		public bool Accepts(Type type)
		{
			return type == typeof(DateOnly);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000BBF4 File Offset: 0x00009DF4
		public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
		{
			string value = parser.Consume<Scalar>().Value;
			DateOnly dateOnly = DateOnly.ParseExact(value, this.formats, this.provider, 0);
			return dateOnly;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000BC28 File Offset: 0x00009E28
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type, ObjectSerializer serializer)
		{
			string value2 = ((DateOnly)value).ToString(Enumerable.First<string>(this.formats), this.provider);
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value2, this.doubleQuotes ? ScalarStyle.DoubleQuoted : ScalarStyle.Any, true, false));
		}

		// Token: 0x040000E3 RID: 227
		private readonly IFormatProvider provider;

		// Token: 0x040000E4 RID: 228
		private readonly bool doubleQuotes;

		// Token: 0x040000E5 RID: 229
		private readonly string[] formats;
	}
}
