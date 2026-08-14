using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001DE RID: 478
	public sealed class FlowEntry : Token
	{
		// Token: 0x06000AC1 RID: 2753 RVA: 0x000257DB File Offset: 0x000239DB
		public FlowEntry() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0002570F File Offset: 0x0002390F
		public FlowEntry(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
