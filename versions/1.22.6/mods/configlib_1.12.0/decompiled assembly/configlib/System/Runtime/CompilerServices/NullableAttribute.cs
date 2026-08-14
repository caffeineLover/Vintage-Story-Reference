using System;
using Microsoft.CodeAnalysis;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000071 RID: 113
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(27524, AllowMultiple = false, Inherited = false)]
	internal sealed class NullableAttribute : Attribute
	{
		// Token: 0x060002F5 RID: 757 RVA: 0x0000F73B File Offset: 0x0000D93B
		public NullableAttribute(byte A_1)
		{
			this.NullableFlags = new byte[]
			{
				A_1
			};
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000F753 File Offset: 0x0000D953
		public NullableAttribute(byte[] A_1)
		{
			this.NullableFlags = A_1;
		}

		// Token: 0x04000186 RID: 390
		public readonly byte[] NullableFlags;
	}
}
