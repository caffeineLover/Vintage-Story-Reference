using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001DA RID: 474
	public sealed class FlowEntry : Token
	{
		// Token: 0x06000A85 RID: 2693 RVA: 0x00024E93 File Offset: 0x00023093
		public FlowEntry() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00024DC7 File Offset: 0x00022FC7
		public FlowEntry(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
