using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001CC RID: 460
	[NullableContext(1)]
	[Nullable(0)]
	public class SemanticErrorException : YamlException
	{
		// Token: 0x06000A70 RID: 2672 RVA: 0x0001D23C File Offset: 0x0001B43C
		public SemanticErrorException(string message) : base(message)
		{
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x0001D245 File Offset: 0x0001B445
		public SemanticErrorException(in Mark start, in Mark end, string message) : base(start, end, message)
		{
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x0001D250 File Offset: 0x0001B450
		public SemanticErrorException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
