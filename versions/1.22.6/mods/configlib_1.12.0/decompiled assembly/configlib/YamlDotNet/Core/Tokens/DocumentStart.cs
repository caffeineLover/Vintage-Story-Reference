using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001DC RID: 476
	public sealed class DocumentStart : Token
	{
		// Token: 0x06000ABD RID: 2749 RVA: 0x000257AE File Offset: 0x000239AE
		public DocumentStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x0002570F File Offset: 0x0002390F
		public DocumentStart(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
