using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000E3 RID: 227
	public sealed class BlockEnd : Token
	{
		// Token: 0x060005EE RID: 1518 RVA: 0x00016DE1 File Offset: 0x00014FE1
		public BlockEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00016DF3 File Offset: 0x00014FF3
		public BlockEnd(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
