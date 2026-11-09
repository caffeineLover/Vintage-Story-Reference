using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000F3 RID: 243
	public sealed class StreamStart : Token
	{
		// Token: 0x06000615 RID: 1557 RVA: 0x0001701D File Offset: 0x0001521D
		public StreamStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0001702F File Offset: 0x0001522F
		public StreamStart(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
