using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NamingConventions
{
	// Token: 0x02000088 RID: 136
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CamelCaseNamingConvention : INamingConvention
	{
		// Token: 0x06000302 RID: 770 RVA: 0x0000B2BB File Offset: 0x000094BB
		[Obsolete("Use the Instance static field instead of creating new instances")]
		public CamelCaseNamingConvention()
		{
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000B2C3 File Offset: 0x000094C3
		public string Apply(string value)
		{
			return value.ToCamelCase();
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000B2CC File Offset: 0x000094CC
		public string Reverse(string value)
		{
			return value.ToPascalCase();
		}

		// Token: 0x040000D0 RID: 208
		public static readonly INamingConvention Instance = new CamelCaseNamingConvention();
	}
}
