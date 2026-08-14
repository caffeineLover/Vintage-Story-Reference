using System;

namespace ImGuiNET
{
	// Token: 0x02000017 RID: 23
	[Flags]
	public enum ImDrawFlags
	{
		// Token: 0x0400001D RID: 29
		None = 0,
		// Token: 0x0400001E RID: 30
		Closed = 1,
		// Token: 0x0400001F RID: 31
		RoundCornersTopLeft = 16,
		// Token: 0x04000020 RID: 32
		RoundCornersTopRight = 32,
		// Token: 0x04000021 RID: 33
		RoundCornersBottomLeft = 64,
		// Token: 0x04000022 RID: 34
		RoundCornersBottomRight = 128,
		// Token: 0x04000023 RID: 35
		RoundCornersNone = 256,
		// Token: 0x04000024 RID: 36
		RoundCornersTop = 48,
		// Token: 0x04000025 RID: 37
		RoundCornersBottom = 192,
		// Token: 0x04000026 RID: 38
		RoundCornersLeft = 80,
		// Token: 0x04000027 RID: 39
		RoundCornersRight = 160,
		// Token: 0x04000028 RID: 40
		RoundCornersAll = 240,
		// Token: 0x04000029 RID: 41
		RoundCornersDefault = 240,
		// Token: 0x0400002A RID: 42
		RoundCornersMask = 496
	}
}
