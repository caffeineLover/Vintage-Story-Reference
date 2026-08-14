using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001E2 RID: 482
	public sealed class FlowSequenceStart : Token
	{
		// Token: 0x06000AC9 RID: 2761 RVA: 0x00025823 File Offset: 0x00023A23
		public FlowSequenceStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x0002570F File Offset: 0x0002390F
		public FlowSequenceStart(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
