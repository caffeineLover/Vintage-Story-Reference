using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NamingConventions
{
	// Token: 0x02000160 RID: 352
	public sealed class PascalCaseNamingConvention : INamingConvention
	{
		// Token: 0x06000717 RID: 1815 RVA: 0x0000412E File Offset: 0x0000232E
		[Obsolete("Use the Instance static field instead of creating new instances")]
		public PascalCaseNamingConvention()
		{
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00018795 File Offset: 0x00016995
		[NullableContext(1)]
		public string Apply(string value)
		{
			return value.ToPascalCase();
		}

		// Token: 0x04000309 RID: 777
		[Nullable(1)]
		public static readonly INamingConvention Instance = new PascalCaseNamingConvention();
	}
}
