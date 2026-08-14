using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001B4 RID: 436
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ForwardAnchorNotSupportedException : YamlException
	{
		// Token: 0x060009A0 RID: 2464 RVA: 0x0001D23C File Offset: 0x0001B43C
		public ForwardAnchorNotSupportedException(string message) : base(message)
		{
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0001D245 File Offset: 0x0001B445
		public ForwardAnchorNotSupportedException(in Mark start, in Mark end, string message) : base(start, end, message)
		{
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0001D250 File Offset: 0x0001B450
		public ForwardAnchorNotSupportedException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
