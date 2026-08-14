using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000F6 RID: 246
	public abstract class Token
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x000171B0 File Offset: 0x000153B0
		public Mark Start { get; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x000171B8 File Offset: 0x000153B8
		public Mark End { get; }

		// Token: 0x06000625 RID: 1573 RVA: 0x000171C0 File Offset: 0x000153C0
		protected Token(in Mark start, in Mark end)
		{
			this.Start = start;
			this.End = end;
		}
	}
}
