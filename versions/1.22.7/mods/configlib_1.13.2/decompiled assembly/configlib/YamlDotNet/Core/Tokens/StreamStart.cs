using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001E2 RID: 482
	public sealed class StreamStart : Token
	{
		// Token: 0x06000A9A RID: 2714 RVA: 0x00024F79 File Offset: 0x00023179
		public StreamStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x00024DC7 File Offset: 0x00022FC7
		public StreamStart(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
