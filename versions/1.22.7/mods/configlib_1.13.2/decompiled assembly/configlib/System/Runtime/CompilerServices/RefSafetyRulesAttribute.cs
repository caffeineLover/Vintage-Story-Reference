using System;
using Microsoft.CodeAnalysis;

namespace System.Runtime.CompilerServices
{
	// Token: 0x0200006F RID: 111
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(2, AllowMultiple = false, Inherited = false)]
	internal sealed class RefSafetyRulesAttribute : Attribute
	{
		// Token: 0x060002BC RID: 700 RVA: 0x0000EE29 File Offset: 0x0000D029
		public RefSafetyRulesAttribute(int A_1)
		{
			this.Version = A_1;
		}

		// Token: 0x0400015A RID: 346
		public readonly int Version;
	}
}
