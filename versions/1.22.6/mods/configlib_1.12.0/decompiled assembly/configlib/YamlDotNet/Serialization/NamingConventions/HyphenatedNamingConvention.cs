using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NamingConventions
{
	// Token: 0x02000161 RID: 353
	public sealed class HyphenatedNamingConvention : INamingConvention
	{
		// Token: 0x0600074A RID: 1866 RVA: 0x00003E0A File Offset: 0x0000200A
		[Obsolete("Use the Instance static field instead of creating new instances")]
		public HyphenatedNamingConvention()
		{
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0001909F File Offset: 0x0001729F
		[NullableContext(1)]
		public string Apply(string value)
		{
			return value.FromCamelCase("-");
		}

		// Token: 0x04000334 RID: 820
		[Nullable(1)]
		public static readonly INamingConvention Instance = new HyphenatedNamingConvention();
	}
}
