using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.NamingConventions
{
	// Token: 0x0200015F RID: 351
	public sealed class NullNamingConvention : INamingConvention
	{
		// Token: 0x06000714 RID: 1812 RVA: 0x0000412E File Offset: 0x0000232E
		[Obsolete("Use the Instance static field instead of creating new instances")]
		public NullNamingConvention()
		{
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0000976E File Offset: 0x0000796E
		[NullableContext(1)]
		public string Apply(string value)
		{
			return value;
		}

		// Token: 0x04000308 RID: 776
		[Nullable(1)]
		public static readonly INamingConvention Instance = new NullNamingConvention();
	}
}
