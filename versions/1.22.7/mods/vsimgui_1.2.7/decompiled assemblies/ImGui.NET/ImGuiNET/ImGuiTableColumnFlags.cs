using System;

namespace ImGuiNET
{
	// Token: 0x02000062 RID: 98
	[Flags]
	public enum ImGuiTableColumnFlags
	{
		// Token: 0x0400069E RID: 1694
		None = 0,
		// Token: 0x0400069F RID: 1695
		Disabled = 1,
		// Token: 0x040006A0 RID: 1696
		DefaultHide = 2,
		// Token: 0x040006A1 RID: 1697
		DefaultSort = 4,
		// Token: 0x040006A2 RID: 1698
		WidthStretch = 8,
		// Token: 0x040006A3 RID: 1699
		WidthFixed = 16,
		// Token: 0x040006A4 RID: 1700
		NoResize = 32,
		// Token: 0x040006A5 RID: 1701
		NoReorder = 64,
		// Token: 0x040006A6 RID: 1702
		NoHide = 128,
		// Token: 0x040006A7 RID: 1703
		NoClip = 256,
		// Token: 0x040006A8 RID: 1704
		NoSort = 512,
		// Token: 0x040006A9 RID: 1705
		NoSortAscending = 1024,
		// Token: 0x040006AA RID: 1706
		NoSortDescending = 2048,
		// Token: 0x040006AB RID: 1707
		NoHeaderLabel = 4096,
		// Token: 0x040006AC RID: 1708
		NoHeaderWidth = 8192,
		// Token: 0x040006AD RID: 1709
		PreferSortAscending = 16384,
		// Token: 0x040006AE RID: 1710
		PreferSortDescending = 32768,
		// Token: 0x040006AF RID: 1711
		IndentEnable = 65536,
		// Token: 0x040006B0 RID: 1712
		IndentDisable = 131072,
		// Token: 0x040006B1 RID: 1713
		IsEnabled = 16777216,
		// Token: 0x040006B2 RID: 1714
		IsVisible = 33554432,
		// Token: 0x040006B3 RID: 1715
		IsSorted = 67108864,
		// Token: 0x040006B4 RID: 1716
		IsHovered = 134217728,
		// Token: 0x040006B5 RID: 1717
		WidthMask = 24,
		// Token: 0x040006B6 RID: 1718
		IndentMask = 196608,
		// Token: 0x040006B7 RID: 1719
		StatusMask = 251658240,
		// Token: 0x040006B8 RID: 1720
		NoDirectResize = 1073741824
	}
}
