using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000EB RID: 235
	public sealed class FlowEntry : Token
	{
		// Token: 0x06000600 RID: 1536 RVA: 0x00016EF1 File Offset: 0x000150F1
		public FlowEntry() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00016F03 File Offset: 0x00015103
		public FlowEntry(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
