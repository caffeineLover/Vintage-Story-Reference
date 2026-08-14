using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000EC RID: 236
	public sealed class FlowMappingEnd : Token
	{
		// Token: 0x06000602 RID: 1538 RVA: 0x00016F0D File Offset: 0x0001510D
		public FlowMappingEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00016F1F File Offset: 0x0001511F
		public FlowMappingEnd(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
