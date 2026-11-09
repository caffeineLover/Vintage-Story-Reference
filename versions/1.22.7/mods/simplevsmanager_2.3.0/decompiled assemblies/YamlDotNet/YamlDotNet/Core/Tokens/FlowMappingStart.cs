using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000ED RID: 237
	public sealed class FlowMappingStart : Token
	{
		// Token: 0x06000604 RID: 1540 RVA: 0x00016F29 File Offset: 0x00015129
		public FlowMappingStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00016F3B File Offset: 0x0001513B
		public FlowMappingStart(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
