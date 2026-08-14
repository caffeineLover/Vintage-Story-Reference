using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000068 RID: 104
	[Flags]
	internal enum CollectionAccessType
	{
		// Token: 0x040000DF RID: 223
		None = 0,
		// Token: 0x040000E0 RID: 224
		Read = 1,
		// Token: 0x040000E1 RID: 225
		ModifyExistingContent = 2,
		// Token: 0x040000E2 RID: 226
		UpdatedContent = 6
	}
}
