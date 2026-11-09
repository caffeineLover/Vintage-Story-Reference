using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001E5 RID: 485
	public abstract class Token
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x00025100 File Offset: 0x00023300
		public Mark Start { get; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000AA9 RID: 2729 RVA: 0x00025108 File Offset: 0x00023308
		public Mark End { get; }

		// Token: 0x06000AAA RID: 2730 RVA: 0x00025110 File Offset: 0x00023310
		protected Token(in Mark start, in Mark end)
		{
			this.Start = start;
			this.End = end;
		}
	}
}
