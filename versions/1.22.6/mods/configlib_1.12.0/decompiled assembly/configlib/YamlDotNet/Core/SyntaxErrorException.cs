using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001CF RID: 463
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SyntaxErrorException : YamlException
	{
		// Token: 0x06000A86 RID: 2694 RVA: 0x0001D23C File Offset: 0x0001B43C
		public SyntaxErrorException(string message) : base(message)
		{
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0001D245 File Offset: 0x0001B445
		public SyntaxErrorException(in Mark start, in Mark end, string message) : base(start, end, message)
		{
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x0001D250 File Offset: 0x0001B450
		public SyntaxErrorException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
