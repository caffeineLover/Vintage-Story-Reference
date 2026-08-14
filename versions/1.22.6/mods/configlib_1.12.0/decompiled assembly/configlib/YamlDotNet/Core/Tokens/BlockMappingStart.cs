using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001D8 RID: 472
	public sealed class BlockMappingStart : Token
	{
		// Token: 0x06000AB3 RID: 2739 RVA: 0x0002572B File Offset: 0x0002392B
		public BlockMappingStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0002570F File Offset: 0x0002390F
		public BlockMappingStart(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
