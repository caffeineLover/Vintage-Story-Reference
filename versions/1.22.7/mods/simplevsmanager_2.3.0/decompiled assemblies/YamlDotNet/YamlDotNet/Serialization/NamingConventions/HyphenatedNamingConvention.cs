using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NamingConventions
{
	// Token: 0x02000089 RID: 137
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HyphenatedNamingConvention : INamingConvention
	{
		// Token: 0x06000306 RID: 774 RVA: 0x0000B2ED File Offset: 0x000094ED
		[Obsolete("Use the Instance static field instead of creating new instances")]
		public HyphenatedNamingConvention()
		{
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000B2F5 File Offset: 0x000094F5
		public string Apply(string value)
		{
			return value.FromCamelCase("-");
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000B304 File Offset: 0x00009504
		public string Reverse(string value)
		{
			return value.ToPascalCase();
		}

		// Token: 0x040000D1 RID: 209
		public static readonly INamingConvention Instance = new HyphenatedNamingConvention();
	}
}
