using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001E6 RID: 486
	public sealed class StreamStart : Token
	{
		// Token: 0x06000AD6 RID: 2774 RVA: 0x000258C1 File Offset: 0x00023AC1
		public StreamStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0002570F File Offset: 0x0002390F
		public StreamStart(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
