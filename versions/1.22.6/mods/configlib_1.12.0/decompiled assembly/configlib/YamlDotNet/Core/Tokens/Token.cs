using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001E9 RID: 489
	public abstract class Token
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x00025A48 File Offset: 0x00023C48
		public Mark Start { get; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x00025A50 File Offset: 0x00023C50
		public Mark End { get; }

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00025A58 File Offset: 0x00023C58
		protected Token(in Mark start, in Mark end)
		{
			this.Start = start;
			this.End = end;
		}
	}
}
