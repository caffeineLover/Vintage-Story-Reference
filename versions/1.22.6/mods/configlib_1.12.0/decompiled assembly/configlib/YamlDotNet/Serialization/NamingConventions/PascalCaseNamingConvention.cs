using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NamingConventions
{
	// Token: 0x02000164 RID: 356
	public sealed class PascalCaseNamingConvention : INamingConvention
	{
		// Token: 0x06000753 RID: 1875 RVA: 0x00003E0A File Offset: 0x0000200A
		[Obsolete("Use the Instance static field instead of creating new instances")]
		public PascalCaseNamingConvention()
		{
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x000190DD File Offset: 0x000172DD
		[NullableContext(1)]
		public string Apply(string value)
		{
			return value.ToPascalCase();
		}

		// Token: 0x04000337 RID: 823
		[Nullable(1)]
		public static readonly INamingConvention Instance = new PascalCaseNamingConvention();
	}
}
