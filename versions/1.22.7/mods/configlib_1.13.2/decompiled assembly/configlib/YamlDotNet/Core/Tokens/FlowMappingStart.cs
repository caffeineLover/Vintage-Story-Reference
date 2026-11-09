using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001DC RID: 476
	public sealed class FlowMappingStart : Token
	{
		// Token: 0x06000A89 RID: 2697 RVA: 0x00024EB7 File Offset: 0x000230B7
		public FlowMappingStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00024DC7 File Offset: 0x00022FC7
		public FlowMappingStart(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
