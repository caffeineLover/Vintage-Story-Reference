using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001DE RID: 478
	public sealed class FlowSequenceStart : Token
	{
		// Token: 0x06000A8D RID: 2701 RVA: 0x00024EDB File Offset: 0x000230DB
		public FlowSequenceStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x00024DC7 File Offset: 0x00022FC7
		public FlowSequenceStart(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
