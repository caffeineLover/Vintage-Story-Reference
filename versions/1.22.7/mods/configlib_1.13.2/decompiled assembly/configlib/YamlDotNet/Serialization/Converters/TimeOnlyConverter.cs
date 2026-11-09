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
	public class TimeOnlyConverter : IYamlTypeConverter
	{
		// Token: 0x06000752 RID: 1874 RVA: 0x000193C6 File Offset: 0x000175C6
		public TimeOnlyConverter([Nullable(2)] IFormatProvider provider = null, bool doubleQuotes = false, params string[] formats)
		{
			this.provider = (provider ?? CultureInfo.InvariantCulture);
			this.doubleQuotes = doubleQuotes;
			this.formats = Enumerable.ToArray<string>(Enumerable.DefaultIfEmpty<string>(formats, "T"));
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x000193FB File Offset: 0x000175FB
		public bool Accepts(Type type)
		{
			return type == typeof(TimeOnly);
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0001940D File Offset: 0x0001760D
		public object ReadYaml(IParser parser, Type type)
		{
			return TimeOnly.ParseExact(parser.Consume<Scalar>().Value, this.formats, this.provider, 0);
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x00019434 File Offset: 0x00017634
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type)
		{
			string value2 = ((TimeOnly)value).ToString(Enumerable.First<string>(this.formats), this.provider);
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value2, this.doubleQuotes ? ScalarStyle.DoubleQuoted : ScalarStyle.Any, true, false));
		}

		// Token: 0x04000321 RID: 801
		private readonly IFormatProvider provider;

		// Token: 0x04000322 RID: 802
		private readonly bool doubleQuotes;

		// Token: 0x04000323 RID: 803
		private readonly string[] formats;
	}
}
