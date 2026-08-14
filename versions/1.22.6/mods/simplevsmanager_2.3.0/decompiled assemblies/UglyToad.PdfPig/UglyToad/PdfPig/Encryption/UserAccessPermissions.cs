using System;

namespace UglyToad.PdfPig.Encryption
{
	// Token: 0x020001D6 RID: 470
	[Flags]
	internal enum UserAccessPermissions : long
	{
		// Token: 0x040005B3 RID: 1459
		Print = 4L,
		// Token: 0x040005B4 RID: 1460
		Modify = 8L,
		// Token: 0x040005B5 RID: 1461
		CopyTextAndGraphics = 16L,
		// Token: 0x040005B6 RID: 1462
		AddOrModifyTextAnnotationsAndFillFormFields = 32L,
		// Token: 0x040005B7 RID: 1463
		FillExistingFormFields = 256L,
		// Token: 0x040005B8 RID: 1464
		ExtractTextAndGraphics = 512L,
		// Token: 0x040005B9 RID: 1465
		AssembleDocument = 1024L,
		// Token: 0x040005BA RID: 1466
		PrintHighQuality = 4096L
	}
}
