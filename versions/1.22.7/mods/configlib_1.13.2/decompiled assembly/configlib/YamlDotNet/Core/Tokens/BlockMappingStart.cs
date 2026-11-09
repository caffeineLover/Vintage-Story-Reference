using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001D4 RID: 468
	public sealed class BlockMappingStart : Token
	{
		// Token: 0x06000A77 RID: 2679 RVA: 0x00024DE3 File Offset: 0x00022FE3
		public BlockMappingStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00024DC7 File Offset: 0x00022FC7
		public BlockMappingStart(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
