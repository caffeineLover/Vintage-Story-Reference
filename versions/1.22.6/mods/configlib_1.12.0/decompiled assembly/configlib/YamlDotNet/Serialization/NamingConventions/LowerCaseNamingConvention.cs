using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NamingConventions
{
	// Token: 0x02000162 RID: 354
	public sealed class LowerCaseNamingConvention : INamingConvention
	{
		// Token: 0x0600074D RID: 1869 RVA: 0x00003E0A File Offset: 0x0000200A
		private LowerCaseNamingConvention()
		{
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x000190B8 File Offset: 0x000172B8
		[NullableContext(1)]
		public string Apply(string value)
		{
			return value.ToCamelCase().ToLower();
		}

		// Token: 0x04000335 RID: 821
		[Nullable(1)]
		public static readonly INamingConvention Instance = new LowerCaseNamingConvention();
	}
}
