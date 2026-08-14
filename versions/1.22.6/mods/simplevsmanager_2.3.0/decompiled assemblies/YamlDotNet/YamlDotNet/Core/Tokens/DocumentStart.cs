using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000E9 RID: 233
	public sealed class DocumentStart : Token
	{
		// Token: 0x060005FC RID: 1532 RVA: 0x00016EBA File Offset: 0x000150BA
		public DocumentStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00016ECC File Offset: 0x000150CC
		public DocumentStart(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
