using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001D5 RID: 469
	public sealed class BlockSequenceStart : Token
	{
		// Token: 0x06000A79 RID: 2681 RVA: 0x00024DF5 File Offset: 0x00022FF5
		public BlockSequenceStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x00024DC7 File Offset: 0x00022FC7
		public BlockSequenceStart(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
