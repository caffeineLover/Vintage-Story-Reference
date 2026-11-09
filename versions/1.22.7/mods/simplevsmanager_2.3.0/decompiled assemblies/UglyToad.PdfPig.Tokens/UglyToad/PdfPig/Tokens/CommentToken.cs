using System;

namespace UglyToad.PdfPig.Tokens
{
	// Token: 0x02000004 RID: 4
	public class CommentToken : IDataToken<string>, IToken, IEquatable<IToken>
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000022AD File Offset: 0x000004AD
		public string Data { get; }

		// Token: 0x06000010 RID: 16 RVA: 0x000022B5 File Offset: 0x000004B5
		public CommentToken(string data)
		{
			this.Data = (data ?? string.Empty);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000022CD File Offset: 0x000004CD
		public override string ToString()
		{
			return this.Data;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022D8 File Offset: 0x000004D8
		public bool Equals(IToken obj)
		{
			if (this == obj)
			{
				return true;
			}
			CommentToken commentToken = obj as CommentToken;
			return commentToken != null && commentToken.Data == this.Data;
		}
	}
}
