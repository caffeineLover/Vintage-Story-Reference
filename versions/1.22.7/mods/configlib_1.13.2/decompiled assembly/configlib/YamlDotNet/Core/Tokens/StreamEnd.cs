using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001E1 RID: 481
	public sealed class StreamEnd : Token
	{
		// Token: 0x06000A98 RID: 2712 RVA: 0x00024F67 File Offset: 0x00023167
		public StreamEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x00024DC7 File Offset: 0x00022FC7
		public StreamEnd(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
