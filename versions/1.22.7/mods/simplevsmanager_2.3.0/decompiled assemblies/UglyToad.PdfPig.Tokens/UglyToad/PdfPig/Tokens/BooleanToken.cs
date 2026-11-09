using System;

namespace UglyToad.PdfPig.Tokens
{
	// Token: 0x02000003 RID: 3
	public sealed class BooleanToken : IDataToken<bool>, IToken, IEquatable<IToken>
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002209 File Offset: 0x00000409
		public static BooleanToken True { get; } = new BooleanToken(true);

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002210 File Offset: 0x00000410
		public static BooleanToken False { get; } = new BooleanToken(false);

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002217 File Offset: 0x00000417
		public bool Data { get; }

		// Token: 0x0600000A RID: 10 RVA: 0x0000221F File Offset: 0x0000041F
		private BooleanToken(bool data)
		{
			this.Data = data;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002230 File Offset: 0x00000430
		public override int GetHashCode()
		{
			return this.Data.GetHashCode();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000224C File Offset: 0x0000044C
		public override string ToString()
		{
			return this.Data.ToString();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002268 File Offset: 0x00000468
		public bool Equals(IToken obj)
		{
			if (this == obj)
			{
				return true;
			}
			BooleanToken booleanToken = obj as BooleanToken;
			return booleanToken != null && booleanToken.Data == this.Data;
		}
	}
}
