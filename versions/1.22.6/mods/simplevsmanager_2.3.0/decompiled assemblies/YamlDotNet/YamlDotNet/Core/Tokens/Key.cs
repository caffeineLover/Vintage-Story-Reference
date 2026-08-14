using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000F0 RID: 240
	public sealed class Key : Token
	{
		// Token: 0x0600060A RID: 1546 RVA: 0x00016F7D File Offset: 0x0001517D
		public Key() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00016F8F File Offset: 0x0001518F
		public Key(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
