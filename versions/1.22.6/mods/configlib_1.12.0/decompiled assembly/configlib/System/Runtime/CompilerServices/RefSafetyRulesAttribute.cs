using System;
using Microsoft.CodeAnalysis;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000073 RID: 115
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(2, AllowMultiple = false, Inherited = false)]
	internal sealed class RefSafetyRulesAttribute : Attribute
	{
		// Token: 0x060002F8 RID: 760 RVA: 0x0000F771 File Offset: 0x0000D971
		public RefSafetyRulesAttribute(int A_1)
		{
			this.Version = A_1;
		}

		// Token: 0x04000188 RID: 392
		public readonly int Version;
	}
}
