using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NamingConventions
{
	// Token: 0x02000160 RID: 352
	public sealed class CamelCaseNamingConvention : INamingConvention
	{
		// Token: 0x06000747 RID: 1863 RVA: 0x00003E0A File Offset: 0x0000200A
		[Obsolete("Use the Instance static field instead of creating new instances")]
		public CamelCaseNamingConvention()
		{
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0001908B File Offset: 0x0001728B
		[NullableContext(1)]
		public string Apply(string value)
		{
			return value.ToCamelCase();
		}

		// Token: 0x04000333 RID: 819
		[Nullable(1)]
		public static readonly INamingConvention Instance = new CamelCaseNamingConvention();
	}
}
