using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001BE RID: 446
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MaximumRecursionLevelReachedException : YamlException
	{
		// Token: 0x060009D3 RID: 2515 RVA: 0x0001D23C File Offset: 0x0001B43C
		public MaximumRecursionLevelReachedException(string message) : base(message)
		{
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0001D245 File Offset: 0x0001B445
		public MaximumRecursionLevelReachedException(in Mark start, in Mark end, string message) : base(start, end, message)
		{
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0001D250 File Offset: 0x0001B450
		public MaximumRecursionLevelReachedException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
