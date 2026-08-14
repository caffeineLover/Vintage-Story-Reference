using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.PdfFonts.Cmap
{
	// Token: 0x020000B1 RID: 177
	internal struct CidCharacterMapping
	{
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600032E RID: 814 RVA: 0x00014BB6 File Offset: 0x00012DB6
		public readonly int SourceCharacterCode { get; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600032F RID: 815 RVA: 0x00014BBE File Offset: 0x00012DBE
		public readonly int DestinationCid { get; }

		// Token: 0x06000330 RID: 816 RVA: 0x00014BC6 File Offset: 0x00012DC6
		public CidCharacterMapping(int sourceCharacterCode, int destinationCid)
		{
			this.SourceCharacterCode = sourceCharacterCode;
			this.DestinationCid = destinationCid;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00014BD8 File Offset: 0x00012DD8
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(13, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Code ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.SourceCharacterCode);
			defaultInterpolatedStringHandler.AppendLiteral(" -> CID ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.DestinationCid);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
