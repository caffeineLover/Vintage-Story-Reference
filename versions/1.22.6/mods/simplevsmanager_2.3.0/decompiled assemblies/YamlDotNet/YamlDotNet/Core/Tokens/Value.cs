using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000F7 RID: 247
	public sealed class Value : Token
	{
		// Token: 0x06000626 RID: 1574 RVA: 0x000171E0 File Offset: 0x000153E0
		public Value() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x000171F2 File Offset: 0x000153F2
		public Value(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
