using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020000D1 RID: 209
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MaximumRecursionLevelReachedException : YamlException
	{
		// Token: 0x0600053A RID: 1338 RVA: 0x00011EC7 File Offset: 0x000100C7
		public MaximumRecursionLevelReachedException(string message) : base(message)
		{
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00011ED0 File Offset: 0x000100D0
		public MaximumRecursionLevelReachedException(in Mark start, in Mark end, string message) : base(start, end, message)
		{
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00011EDB File Offset: 0x000100DB
		public MaximumRecursionLevelReachedException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
