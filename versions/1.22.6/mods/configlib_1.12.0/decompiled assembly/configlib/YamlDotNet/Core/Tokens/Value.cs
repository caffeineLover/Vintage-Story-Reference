using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001EA RID: 490
	public sealed class Value : Token
	{
		// Token: 0x06000AE7 RID: 2791 RVA: 0x00025A78 File Offset: 0x00023C78
		public Value() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0002570F File Offset: 0x0002390F
		public Value(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
