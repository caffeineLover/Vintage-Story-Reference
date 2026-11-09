using System;

namespace UglyToad.PdfPig.AcroForms.Fields
{
	// Token: 0x02000212 RID: 530
	[Flags]
	public enum AcroChoiceFieldFlags : uint
	{
		// Token: 0x040006F1 RID: 1777
		ReadOnly = 1U,
		// Token: 0x040006F2 RID: 1778
		Required = 2U,
		// Token: 0x040006F3 RID: 1779
		NoExport = 4U,
		// Token: 0x040006F4 RID: 1780
		Combo = 131072U,
		// Token: 0x040006F5 RID: 1781
		Edit = 262144U,
		// Token: 0x040006F6 RID: 1782
		Sort = 524288U,
		// Token: 0x040006F7 RID: 1783
		MultiSelect = 2097152U,
		// Token: 0x040006F8 RID: 1784
		DoNotSpellCheck = 4194304U,
		// Token: 0x040006F9 RID: 1785
		CommitOnSelectionChange = 67108864U
	}
}
