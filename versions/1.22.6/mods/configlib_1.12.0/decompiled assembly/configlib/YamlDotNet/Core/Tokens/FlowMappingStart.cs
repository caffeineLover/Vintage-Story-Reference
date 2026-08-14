using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001E0 RID: 480
	public sealed class FlowMappingStart : Token
	{
		// Token: 0x06000AC5 RID: 2757 RVA: 0x000257FF File Offset: 0x000239FF
		public FlowMappingStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x0002570F File Offset: 0x0002390F
		public FlowMappingStart(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
