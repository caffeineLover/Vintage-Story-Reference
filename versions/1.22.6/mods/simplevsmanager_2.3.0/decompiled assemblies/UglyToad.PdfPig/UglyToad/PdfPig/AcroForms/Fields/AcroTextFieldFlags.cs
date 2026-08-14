using System;

namespace UglyToad.PdfPig.AcroForms.Fields
{
	// Token: 0x0200021F RID: 543
	[Flags]
	public enum AcroTextFieldFlags : uint
	{
		// Token: 0x0400072A RID: 1834
		ReadOnly = 1U,
		// Token: 0x0400072B RID: 1835
		Required = 2U,
		// Token: 0x0400072C RID: 1836
		NoExport = 4U,
		// Token: 0x0400072D RID: 1837
		Multiline = 4096U,
		// Token: 0x0400072E RID: 1838
		Password = 8192U,
		// Token: 0x0400072F RID: 1839
		FileSelect = 1048576U,
		// Token: 0x04000730 RID: 1840
		DoNotSpellCheck = 4194304U,
		// Token: 0x04000731 RID: 1841
		DoNotScroll = 8388608U,
		// Token: 0x04000732 RID: 1842
		Comb = 16777216U,
		// Token: 0x04000733 RID: 1843
		RichText = 33554432U
	}
}
