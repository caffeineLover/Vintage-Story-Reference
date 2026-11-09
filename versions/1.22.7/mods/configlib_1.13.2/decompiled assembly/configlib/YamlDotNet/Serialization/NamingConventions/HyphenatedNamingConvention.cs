using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NamingConventions
{
	// Token: 0x0200015D RID: 349
	public sealed class HyphenatedNamingConvention : INamingConvention
	{
		// Token: 0x0600070E RID: 1806 RVA: 0x0000412E File Offset: 0x0000232E
		[Obsolete("Use the Instance static field instead of creating new instances")]
		public HyphenatedNamingConvention()
		{
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00018757 File Offset: 0x00016957
		[NullableContext(1)]
		public string Apply(string value)
		{
			return value.FromCamelCase("-");
		}

		// Token: 0x04000306 RID: 774
		[Nullable(1)]
		public static readonly INamingConvention Instance = new HyphenatedNamingConvention();
	}
}
