using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NamingConventions
{
	// Token: 0x0200008C RID: 140
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PascalCaseNamingConvention : INamingConvention
	{
		// Token: 0x06000312 RID: 786 RVA: 0x0000B3A4 File Offset: 0x000095A4
		[Obsolete("Use the Instance static field instead of creating new instances")]
		public PascalCaseNamingConvention()
		{
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000B3AC File Offset: 0x000095AC
		public string Apply(string value)
		{
			return value.ToPascalCase();
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000B3B4 File Offset: 0x000095B4
		public string Reverse(string value)
		{
			return value.ToPascalCase();
		}

		// Token: 0x040000D4 RID: 212
		public static readonly INamingConvention Instance = new PascalCaseNamingConvention();
	}
}
