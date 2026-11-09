using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Tokens
{
	// Token: 0x02000009 RID: 9
	public class IndirectReferenceToken : IDataToken<IndirectReference>, IToken, IEquatable<IToken>
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002E RID: 46 RVA: 0x000029AA File Offset: 0x00000BAA
		public IndirectReference Data { get; }

		// Token: 0x0600002F RID: 47 RVA: 0x000029B2 File Offset: 0x00000BB2
		public IndirectReferenceToken(IndirectReference data)
		{
			this.Data = data;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000029C4 File Offset: 0x00000BC4
		public bool Equals(IToken obj)
		{
			if (this == obj)
			{
				return true;
			}
			IndirectReferenceToken indirectReferenceToken = obj as IndirectReferenceToken;
			return indirectReferenceToken != null && this.Data.Equals(indirectReferenceToken.Data);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002A04 File Offset: 0x00000C04
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(0, 1);
			defaultInterpolatedStringHandler.AppendFormatted<IndirectReference>(this.Data);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
