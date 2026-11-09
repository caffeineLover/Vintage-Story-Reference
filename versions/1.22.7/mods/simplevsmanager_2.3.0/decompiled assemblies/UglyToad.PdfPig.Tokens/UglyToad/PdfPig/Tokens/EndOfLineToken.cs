using System;

namespace UglyToad.PdfPig.Tokens
{
	// Token: 0x02000006 RID: 6
	public class EndOfLineToken : IToken, IEquatable<IToken>
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000021 RID: 33 RVA: 0x0000269E File Offset: 0x0000089E
		public static EndOfLineToken Token { get; } = new EndOfLineToken();

		// Token: 0x06000022 RID: 34 RVA: 0x000026A5 File Offset: 0x000008A5
		private EndOfLineToken()
		{
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000026AD File Offset: 0x000008AD
		public bool Equals(IToken obj)
		{
			return this == obj || obj is EndOfLineToken;
		}
	}
}
