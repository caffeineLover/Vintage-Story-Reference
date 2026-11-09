using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NamingConventions
{
	// Token: 0x02000161 RID: 353
	public sealed class UnderscoredNamingConvention : INamingConvention
	{
		// Token: 0x0600071A RID: 1818 RVA: 0x0000412E File Offset: 0x0000232E
		[Obsolete("Use the Instance static field instead of creating new instances")]
		public UnderscoredNamingConvention()
		{
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x000187A9 File Offset: 0x000169A9
		[NullableContext(1)]
		public string Apply(string value)
		{
			return value.FromCamelCase("_");
		}

		// Token: 0x0400030A RID: 778
		[Nullable(1)]
		public static readonly INamingConvention Instance = new UnderscoredNamingConvention();
	}
}
