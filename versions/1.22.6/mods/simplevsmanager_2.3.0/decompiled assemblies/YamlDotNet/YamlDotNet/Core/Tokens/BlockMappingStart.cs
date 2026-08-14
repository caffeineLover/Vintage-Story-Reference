using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000E5 RID: 229
	public sealed class BlockMappingStart : Token
	{
		// Token: 0x060005F2 RID: 1522 RVA: 0x00016E19 File Offset: 0x00015019
		public BlockMappingStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00016E2B File Offset: 0x0001502B
		public BlockMappingStart(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
