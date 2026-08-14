using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001DB RID: 475
	public sealed class DocumentEnd : Token
	{
		// Token: 0x06000ABB RID: 2747 RVA: 0x0002579C File Offset: 0x0002399C
		public DocumentEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0002570F File Offset: 0x0002390F
		public DocumentEnd(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
