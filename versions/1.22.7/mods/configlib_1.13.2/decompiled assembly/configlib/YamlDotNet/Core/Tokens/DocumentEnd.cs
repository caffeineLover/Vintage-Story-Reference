using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001D7 RID: 471
	public sealed class DocumentEnd : Token
	{
		// Token: 0x06000A7F RID: 2687 RVA: 0x00024E54 File Offset: 0x00023054
		public DocumentEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00024DC7 File Offset: 0x00022FC7
		public DocumentEnd(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
