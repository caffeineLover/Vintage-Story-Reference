using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000E6 RID: 230
	public sealed class BlockSequenceStart : Token
	{
		// Token: 0x060005F4 RID: 1524 RVA: 0x00016E35 File Offset: 0x00015035
		public BlockSequenceStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00016E47 File Offset: 0x00015047
		public BlockSequenceStart(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
