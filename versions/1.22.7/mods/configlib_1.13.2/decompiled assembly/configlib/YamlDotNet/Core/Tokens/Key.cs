using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001DF RID: 479
	public sealed class Key : Token
	{
		// Token: 0x06000A8F RID: 2703 RVA: 0x00024EED File Offset: 0x000230ED
		public Key() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x00024DC7 File Offset: 0x00022FC7
		public Key(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
