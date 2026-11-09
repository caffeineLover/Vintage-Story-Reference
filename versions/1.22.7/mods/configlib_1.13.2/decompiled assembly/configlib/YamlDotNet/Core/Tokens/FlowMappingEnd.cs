using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001DB RID: 475
	public sealed class FlowMappingEnd : Token
	{
		// Token: 0x06000A87 RID: 2695 RVA: 0x00024EA5 File Offset: 0x000230A5
		public FlowMappingEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00024DC7 File Offset: 0x00022FC7
		public FlowMappingEnd(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
