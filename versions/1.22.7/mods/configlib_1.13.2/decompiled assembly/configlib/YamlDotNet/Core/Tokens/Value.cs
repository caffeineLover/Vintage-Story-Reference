using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001E6 RID: 486
	public sealed class Value : Token
	{
		// Token: 0x06000AAB RID: 2731 RVA: 0x00025130 File Offset: 0x00023330
		public Value() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x00024DC7 File Offset: 0x00022FC7
		public Value(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
