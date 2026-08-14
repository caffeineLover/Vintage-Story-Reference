using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NamingConventions
{
	// Token: 0x02000165 RID: 357
	public sealed class UnderscoredNamingConvention : INamingConvention
	{
		// Token: 0x06000756 RID: 1878 RVA: 0x00003E0A File Offset: 0x0000200A
		[Obsolete("Use the Instance static field instead of creating new instances")]
		public UnderscoredNamingConvention()
		{
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x000190F1 File Offset: 0x000172F1
		[NullableContext(1)]
		public string Apply(string value)
		{
			return value.FromCamelCase("_");
		}

		// Token: 0x04000338 RID: 824
		[Nullable(1)]
		public static readonly INamingConvention Instance = new UnderscoredNamingConvention();
	}
}
