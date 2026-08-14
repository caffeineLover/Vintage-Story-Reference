using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Tokens
{
	// Token: 0x0200000A RID: 10
	public class InlineImageDataToken : IDataToken<IReadOnlyList<byte>>, IToken, IEquatable<IToken>
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002A2E File Offset: 0x00000C2E
		public IReadOnlyList<byte> Data { get; }

		// Token: 0x06000033 RID: 51 RVA: 0x00002A36 File Offset: 0x00000C36
		public InlineImageDataToken(IReadOnlyList<byte> data)
		{
			this.Data = data;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002A48 File Offset: 0x00000C48
		public bool Equals(IToken obj)
		{
			if (this == obj)
			{
				return true;
			}
			InlineImageDataToken inlineImageDataToken = obj as InlineImageDataToken;
			if (inlineImageDataToken == null)
			{
				return false;
			}
			if (this.Data.Count != inlineImageDataToken.Data.Count)
			{
				return false;
			}
			for (int i = 0; i < this.Data.Count; i++)
			{
				if (this.Data[i] != inlineImageDataToken.Data[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}
