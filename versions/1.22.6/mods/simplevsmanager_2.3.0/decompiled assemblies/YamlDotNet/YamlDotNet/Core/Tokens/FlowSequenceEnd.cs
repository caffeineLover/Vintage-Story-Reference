using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000EE RID: 238
	public sealed class FlowSequenceEnd : Token
	{
		// Token: 0x06000606 RID: 1542 RVA: 0x00016F45 File Offset: 0x00015145
		public FlowSequenceEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00016F57 File Offset: 0x00015157
		public FlowSequenceEnd(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
