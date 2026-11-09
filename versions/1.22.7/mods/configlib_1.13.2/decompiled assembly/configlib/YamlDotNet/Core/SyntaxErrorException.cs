using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001CB RID: 459
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SyntaxErrorException : YamlException
	{
		// Token: 0x06000A4A RID: 2634 RVA: 0x0001C8F4 File Offset: 0x0001AAF4
		public SyntaxErrorException(string message) : base(message)
		{
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0001C8FD File Offset: 0x0001AAFD
		public SyntaxErrorException(in Mark start, in Mark end, string message) : base(start, end, message)
		{
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0001C908 File Offset: 0x0001AB08
		public SyntaxErrorException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
