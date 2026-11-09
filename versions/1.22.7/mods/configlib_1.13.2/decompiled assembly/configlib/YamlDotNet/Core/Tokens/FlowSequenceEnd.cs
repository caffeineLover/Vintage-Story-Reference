using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001DD RID: 477
	public sealed class FlowSequenceEnd : Token
	{
		// Token: 0x06000A8B RID: 2699 RVA: 0x00024EC9 File Offset: 0x000230C9
		public FlowSequenceEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00024DC7 File Offset: 0x00022FC7
		public FlowSequenceEnd(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
