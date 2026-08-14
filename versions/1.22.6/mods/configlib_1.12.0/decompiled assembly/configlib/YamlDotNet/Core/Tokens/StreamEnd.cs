using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001E5 RID: 485
	public sealed class StreamEnd : Token
	{
		// Token: 0x06000AD4 RID: 2772 RVA: 0x000258AF File Offset: 0x00023AAF
		public StreamEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x0002570F File Offset: 0x0002390F
		public StreamEnd(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
