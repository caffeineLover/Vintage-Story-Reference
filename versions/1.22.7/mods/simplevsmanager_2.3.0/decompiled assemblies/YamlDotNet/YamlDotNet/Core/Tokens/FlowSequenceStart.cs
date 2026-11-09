using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000EF RID: 239
	public sealed class FlowSequenceStart : Token
	{
		// Token: 0x06000608 RID: 1544 RVA: 0x00016F61 File Offset: 0x00015161
		public FlowSequenceStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00016F73 File Offset: 0x00015173
		public FlowSequenceStart(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
