using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.NamingConventions
{
	// Token: 0x02000163 RID: 355
	public sealed class NullNamingConvention : INamingConvention
	{
		// Token: 0x06000750 RID: 1872 RVA: 0x00003E0A File Offset: 0x0000200A
		[Obsolete("Use the Instance static field instead of creating new instances")]
		public NullNamingConvention()
		{
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00009452 File Offset: 0x00007652
		[NullableContext(1)]
		public string Apply(string value)
		{
			return value;
		}

		// Token: 0x04000336 RID: 822
		[Nullable(1)]
		public static readonly INamingConvention Instance = new NullNamingConvention();
	}
}
