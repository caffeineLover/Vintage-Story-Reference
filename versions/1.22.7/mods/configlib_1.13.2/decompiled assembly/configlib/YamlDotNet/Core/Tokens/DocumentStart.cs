using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001D8 RID: 472
	public sealed class DocumentStart : Token
	{
		// Token: 0x06000A81 RID: 2689 RVA: 0x00024E66 File Offset: 0x00023066
		public DocumentStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00024DC7 File Offset: 0x00022FC7
		public DocumentStart(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
