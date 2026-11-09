using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001D2 RID: 466
	public sealed class BlockEnd : Token
	{
		// Token: 0x06000A73 RID: 2675 RVA: 0x00024DB5 File Offset: 0x00022FB5
		public BlockEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00024DC7 File Offset: 0x00022FC7
		public BlockEnd(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
