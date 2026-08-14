using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020000DC RID: 220
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SyntaxErrorException : YamlException
	{
		// Token: 0x060005C6 RID: 1478 RVA: 0x0001697C File Offset: 0x00014B7C
		public SyntaxErrorException(string message) : base(message)
		{
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00016985 File Offset: 0x00014B85
		public SyntaxErrorException(in Mark start, in Mark end, string message) : base(start, end, message)
		{
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00016990 File Offset: 0x00014B90
		public SyntaxErrorException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
