using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001E1 RID: 481
	public sealed class FlowSequenceEnd : Token
	{
		// Token: 0x06000AC7 RID: 2759 RVA: 0x00025811 File Offset: 0x00023A11
		public FlowSequenceEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x0002570F File Offset: 0x0002390F
		public FlowSequenceEnd(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
