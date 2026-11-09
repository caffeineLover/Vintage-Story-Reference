using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001B0 RID: 432
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ForwardAnchorNotSupportedException : YamlException
	{
		// Token: 0x06000964 RID: 2404 RVA: 0x0001C8F4 File Offset: 0x0001AAF4
		public ForwardAnchorNotSupportedException(string message) : base(message)
		{
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0001C8FD File Offset: 0x0001AAFD
		public ForwardAnchorNotSupportedException(in Mark start, in Mark end, string message) : base(start, end, message)
		{
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0001C908 File Offset: 0x0001AB08
		public ForwardAnchorNotSupportedException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
