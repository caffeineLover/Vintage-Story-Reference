using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001D9 RID: 473
	public sealed class BlockSequenceStart : Token
	{
		// Token: 0x06000AB5 RID: 2741 RVA: 0x0002573D File Offset: 0x0002393D
		public BlockSequenceStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x0002570F File Offset: 0x0002390F
		public BlockSequenceStart(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
