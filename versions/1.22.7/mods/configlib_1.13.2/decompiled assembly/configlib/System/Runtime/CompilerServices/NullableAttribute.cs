using System;
using Microsoft.CodeAnalysis;

namespace System.Runtime.CompilerServices
{
	// Token: 0x0200006D RID: 109
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(27524, AllowMultiple = false, Inherited = false)]
	internal sealed class NullableAttribute : Attribute
	{
		// Token: 0x060002B9 RID: 697 RVA: 0x0000EDF3 File Offset: 0x0000CFF3
		public NullableAttribute(byte A_1)
		{
			this.NullableFlags = new byte[]
			{
				A_1
			};
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000EE0B File Offset: 0x0000D00B
		public NullableAttribute(byte[] A_1)
		{
			this.NullableFlags = A_1;
		}

		// Token: 0x04000158 RID: 344
		public readonly byte[] NullableFlags;
	}
}
