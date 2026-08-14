using System;

namespace ImGuiNET
{
	// Token: 0x02000037 RID: 55
	[Flags]
	public enum ImGuiDragDropFlags
	{
		// Token: 0x04000186 RID: 390
		None = 0,
		// Token: 0x04000187 RID: 391
		SourceNoPreviewTooltip = 1,
		// Token: 0x04000188 RID: 392
		SourceNoDisableHover = 2,
		// Token: 0x04000189 RID: 393
		SourceNoHoldToOpenOthers = 4,
		// Token: 0x0400018A RID: 394
		SourceAllowNullID = 8,
		// Token: 0x0400018B RID: 395
		SourceExtern = 16,
		// Token: 0x0400018C RID: 396
		SourceAutoExpirePayload = 32,
		// Token: 0x0400018D RID: 397
		AcceptBeforeDelivery = 1024,
		// Token: 0x0400018E RID: 398
		AcceptNoDrawDefaultRect = 2048,
		// Token: 0x0400018F RID: 399
		AcceptNoPreviewTooltip = 4096,
		// Token: 0x04000190 RID: 400
		AcceptPeekOnly = 3072
	}
}
