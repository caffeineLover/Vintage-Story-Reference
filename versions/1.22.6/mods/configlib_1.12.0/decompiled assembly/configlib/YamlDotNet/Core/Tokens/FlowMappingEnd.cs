using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001DF RID: 479
	public sealed class FlowMappingEnd : Token
	{
		// Token: 0x06000AC3 RID: 2755 RVA: 0x000257ED File Offset: 0x000239ED
		public FlowMappingEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x0002570F File Offset: 0x0002390F
		public FlowMappingEnd(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
