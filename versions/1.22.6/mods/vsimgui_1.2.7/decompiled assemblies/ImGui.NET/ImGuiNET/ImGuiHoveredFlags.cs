using System;

namespace ImGuiNET
{
	// Token: 0x02000039 RID: 57
	[Flags]
	public enum ImGuiHoveredFlags
	{
		// Token: 0x0400019A RID: 410
		None = 0,
		// Token: 0x0400019B RID: 411
		ChildWindows = 1,
		// Token: 0x0400019C RID: 412
		RootWindow = 2,
		// Token: 0x0400019D RID: 413
		AnyWindow = 4,
		// Token: 0x0400019E RID: 414
		NoPopupHierarchy = 8,
		// Token: 0x0400019F RID: 415
		DockHierarchy = 16,
		// Token: 0x040001A0 RID: 416
		AllowWhenBlockedByPopup = 32,
		// Token: 0x040001A1 RID: 417
		AllowWhenBlockedByActiveItem = 128,
		// Token: 0x040001A2 RID: 418
		AllowWhenOverlappedByItem = 256,
		// Token: 0x040001A3 RID: 419
		AllowWhenOverlappedByWindow = 512,
		// Token: 0x040001A4 RID: 420
		AllowWhenDisabled = 1024,
		// Token: 0x040001A5 RID: 421
		NoNavOverride = 2048,
		// Token: 0x040001A6 RID: 422
		AllowWhenOverlapped = 768,
		// Token: 0x040001A7 RID: 423
		RectOnly = 928,
		// Token: 0x040001A8 RID: 424
		RootAndChildWindows = 3,
		// Token: 0x040001A9 RID: 425
		ForTooltip = 4096,
		// Token: 0x040001AA RID: 426
		Stationary = 8192,
		// Token: 0x040001AB RID: 427
		DelayNone = 16384,
		// Token: 0x040001AC RID: 428
		DelayShort = 32768,
		// Token: 0x040001AD RID: 429
		DelayNormal = 65536,
		// Token: 0x040001AE RID: 430
		NoSharedDelay = 131072
	}
}
