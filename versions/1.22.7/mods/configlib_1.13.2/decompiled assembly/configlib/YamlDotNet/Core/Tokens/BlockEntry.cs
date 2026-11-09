using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001D3 RID: 467
	public sealed class BlockEntry : Token
	{
		// Token: 0x06000A75 RID: 2677 RVA: 0x00024DD1 File Offset: 0x00022FD1
		public BlockEntry() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x00024DC7 File Offset: 0x00022FC7
		public BlockEntry(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
