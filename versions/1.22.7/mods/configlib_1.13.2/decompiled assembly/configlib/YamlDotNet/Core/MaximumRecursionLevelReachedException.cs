using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001BA RID: 442
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MaximumRecursionLevelReachedException : YamlException
	{
		// Token: 0x06000997 RID: 2455 RVA: 0x0001C8F4 File Offset: 0x0001AAF4
		public MaximumRecursionLevelReachedException(string message) : base(message)
		{
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0001C8FD File Offset: 0x0001AAFD
		public MaximumRecursionLevelReachedException(in Mark start, in Mark end, string message) : base(start, end, message)
		{
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0001C908 File Offset: 0x0001AB08
		public MaximumRecursionLevelReachedException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
