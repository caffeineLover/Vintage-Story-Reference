using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Tokenization.Scanner
{
	// Token: 0x02000086 RID: 134
	internal struct PossibleStreamEndLocation
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000FAFF File Offset: 0x0000DCFF
		public readonly long Offset { get; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000FB07 File Offset: 0x0000DD07
		public readonly OperatorToken Type { get; }

		// Token: 0x0600022A RID: 554 RVA: 0x0000FB0F File Offset: 0x0000DD0F
		public PossibleStreamEndLocation(long offset, OperatorToken type)
		{
			this.Offset = offset;
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.Type = type;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000FB30 File Offset: 0x0000DD30
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<long>(this.Offset);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<OperatorToken>(this.Type);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
