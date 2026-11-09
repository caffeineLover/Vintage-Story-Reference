using System;

namespace UglyToad.PdfPig.Annotations
{
	// Token: 0x020001FC RID: 508
	[Flags]
	public enum AnnotationFlags
	{
		// Token: 0x04000694 RID: 1684
		Invisible = 1,
		// Token: 0x04000695 RID: 1685
		Hidden = 2,
		// Token: 0x04000696 RID: 1686
		Print = 4,
		// Token: 0x04000697 RID: 1687
		NoZoom = 8,
		// Token: 0x04000698 RID: 1688
		NoRotate = 16,
		// Token: 0x04000699 RID: 1689
		NoView = 32,
		// Token: 0x0400069A RID: 1690
		ReadOnly = 64,
		// Token: 0x0400069B RID: 1691
		Locked = 128,
		// Token: 0x0400069C RID: 1692
		ToggleNoView = 256,
		// Token: 0x0400069D RID: 1693
		LockedContents = 512
	}
}
