using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000E8 RID: 232
	public sealed class DocumentEnd : Token
	{
		// Token: 0x060005FA RID: 1530 RVA: 0x00016E9E File Offset: 0x0001509E
		public DocumentEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00016EB0 File Offset: 0x000150B0
		public DocumentEnd(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
