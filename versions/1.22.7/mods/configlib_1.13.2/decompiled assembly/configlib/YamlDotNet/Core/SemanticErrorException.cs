using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001C8 RID: 456
	[NullableContext(1)]
	[Nullable(0)]
	public class SemanticErrorException : YamlException
	{
		// Token: 0x06000A34 RID: 2612 RVA: 0x0001C8F4 File Offset: 0x0001AAF4
		public SemanticErrorException(string message) : base(message)
		{
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0001C8FD File Offset: 0x0001AAFD
		public SemanticErrorException(in Mark start, in Mark end, string message) : base(start, end, message)
		{
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x0001C908 File Offset: 0x0001AB08
		public SemanticErrorException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
