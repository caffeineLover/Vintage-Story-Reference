using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001E3 RID: 483
	public sealed class Key : Token
	{
		// Token: 0x06000ACB RID: 2763 RVA: 0x00025835 File Offset: 0x00023A35
		public Key() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0002570F File Offset: 0x0002390F
		public Key(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
