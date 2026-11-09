using System;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables
{
	// Token: 0x0200003C RID: 60
	public interface ITrueTypeTable
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060001AC RID: 428
		string Tag { get; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060001AD RID: 429
		TrueTypeHeaderTable DirectoryTable { get; }
	}
}
