using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001D6 RID: 470
	public sealed class BlockEnd : Token
	{
		// Token: 0x06000AAF RID: 2735 RVA: 0x000256FD File Offset: 0x000238FD
		public BlockEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0002570F File Offset: 0x0002390F
		public BlockEnd(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
