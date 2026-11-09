using System;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts
{
	// Token: 0x0200008C RID: 140
	public class DescriptorFontFile
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000FD66 File Offset: 0x0000DF66
		public IndirectReferenceToken ObjectKey { get; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000FD6E File Offset: 0x0000DF6E
		public DescriptorFontFile.FontFileType FileType { get; }

		// Token: 0x06000240 RID: 576 RVA: 0x0000FD76 File Offset: 0x0000DF76
		public DescriptorFontFile(IndirectReferenceToken key, DescriptorFontFile.FontFileType fileType)
		{
			this.ObjectKey = key;
			this.FileType = fileType;
		}

		// Token: 0x02000251 RID: 593
		public enum FontFileType
		{
			// Token: 0x040007F6 RID: 2038
			Type1,
			// Token: 0x040007F7 RID: 2039
			TrueType,
			// Token: 0x040007F8 RID: 2040
			FromSubtype
		}
	}
}
