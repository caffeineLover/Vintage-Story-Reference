using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000F2 RID: 242
	public sealed class StreamEnd : Token
	{
		// Token: 0x06000613 RID: 1555 RVA: 0x00017001 File Offset: 0x00015201
		public StreamEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00017013 File Offset: 0x00015213
		public StreamEnd(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
