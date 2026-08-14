using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000E4 RID: 228
	public sealed class BlockEntry : Token
	{
		// Token: 0x060005F0 RID: 1520 RVA: 0x00016DFD File Offset: 0x00014FFD
		public BlockEntry() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00016E0F File Offset: 0x0001500F
		public BlockEntry(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
