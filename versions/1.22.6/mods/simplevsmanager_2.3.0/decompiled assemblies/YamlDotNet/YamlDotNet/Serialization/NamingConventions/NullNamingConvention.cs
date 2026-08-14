using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.NamingConventions
{
	// Token: 0x0200008B RID: 139
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NullNamingConvention : INamingConvention
	{
		// Token: 0x0600030E RID: 782 RVA: 0x0000B38A File Offset: 0x0000958A
		[Obsolete("Use the Instance static field instead of creating new instances")]
		public NullNamingConvention()
		{
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000B392 File Offset: 0x00009592
		public string Apply(string value)
		{
			return value;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000B395 File Offset: 0x00009595
		public string Reverse(string value)
		{
			return value;
		}

		// Token: 0x040000D3 RID: 211
		public static readonly INamingConvention Instance = new NullNamingConvention();
	}
}
