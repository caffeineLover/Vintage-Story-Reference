using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
	// Token: 0x02000098 RID: 152
	[NullableContext(1)]
	[Nullable(0)]
	public class TimeOnlyConverter : IYamlTypeConverter
	{
		// Token: 0x0600034E RID: 846 RVA: 0x0000C046 File Offset: 0x0000A246
		public TimeOnlyConverter([Nullable(2)] IFormatProvider provider = null, bool doubleQuotes = false, params string[] formats)
		{
			this.provider = (provider ?? CultureInfo.InvariantCulture);
			this.doubleQuotes = doubleQuotes;
			this.formats = Enumerable.ToArray<string>(Enumerable.DefaultIfEmpty<string>(formats, "T"));
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000C07B File Offset: 0x0000A27B
		public bool Accepts(Type type)
		{
			return type == typeof(TimeOnly);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000C090 File Offset: 0x0000A290
		public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
		{
			string value = parser.Consume<Scalar>().Value;
			TimeOnly timeOnly = TimeOnly.ParseExact(value, this.formats, this.provider, 0);
			return timeOnly;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000C0C4 File Offset: 0x0000A2C4
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type, ObjectSerializer serializer)
		{
			string value2 = ((TimeOnly)value).ToString(Enumerable.First<string>(this.formats), this.provider);
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value2, this.doubleQuotes ? ScalarStyle.DoubleQuoted : ScalarStyle.Any, true, false));
		}

		// Token: 0x040000F0 RID: 240
		private readonly IFormatProvider provider;

		// Token: 0x040000F1 RID: 241
		private readonly bool doubleQuotes;

		// Token: 0x040000F2 RID: 242
		private readonly string[] formats;
	}
}
