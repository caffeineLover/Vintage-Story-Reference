using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NamingConventions
{
	// Token: 0x0200015C RID: 348
	public sealed class CamelCaseNamingConvention : INamingConvention
	{
		// Token: 0x0600070B RID: 1803 RVA: 0x0000412E File Offset: 0x0000232E
		[Obsolete("Use the Instance static field instead of creating new instances")]
		public CamelCaseNamingConvention()
		{
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00018743 File Offset: 0x00016943
		[NullableContext(1)]
		public string Apply(string value)
		{
			return value.ToCamelCase();
		}

		// Token: 0x04000305 RID: 773
		[Nullable(1)]
		public static readonly INamingConvention Instance = new CamelCaseNamingConvention();
	}
}
