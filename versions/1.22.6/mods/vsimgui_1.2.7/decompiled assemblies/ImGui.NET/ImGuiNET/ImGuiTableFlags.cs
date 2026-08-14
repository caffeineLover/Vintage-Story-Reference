using System;

namespace ImGuiNET
{
	// Token: 0x02000065 RID: 101
	[Flags]
	public enum ImGuiTableFlags
	{
		// Token: 0x040006BF RID: 1727
		None = 0,
		// Token: 0x040006C0 RID: 1728
		Resizable = 1,
		// Token: 0x040006C1 RID: 1729
		Reorderable = 2,
		// Token: 0x040006C2 RID: 1730
		Hideable = 4,
		// Token: 0x040006C3 RID: 1731
		Sortable = 8,
		// Token: 0x040006C4 RID: 1732
		NoSavedSettings = 16,
		// Token: 0x040006C5 RID: 1733
		ContextMenuInBody = 32,
		// Token: 0x040006C6 RID: 1734
		RowBg = 64,
		// Token: 0x040006C7 RID: 1735
		BordersInnerH = 128,
		// Token: 0x040006C8 RID: 1736
		BordersOuterH = 256,
		// Token: 0x040006C9 RID: 1737
		BordersInnerV = 512,
		// Token: 0x040006CA RID: 1738
		BordersOuterV = 1024,
		// Token: 0x040006CB RID: 1739
		BordersH = 384,
		// Token: 0x040006CC RID: 1740
		BordersV = 1536,
		// Token: 0x040006CD RID: 1741
		BordersInner = 640,
		// Token: 0x040006CE RID: 1742
		BordersOuter = 1280,
		// Token: 0x040006CF RID: 1743
		Borders = 1920,
		// Token: 0x040006D0 RID: 1744
		NoBordersInBody = 2048,
		// Token: 0x040006D1 RID: 1745
		NoBordersInBodyUntilResize = 4096,
		// Token: 0x040006D2 RID: 1746
		SizingFixedFit = 8192,
		// Token: 0x040006D3 RID: 1747
		SizingFixedSame = 16384,
		// Token: 0x040006D4 RID: 1748
		SizingStretchProp = 24576,
		// Token: 0x040006D5 RID: 1749
		SizingStretchSame = 32768,
		// Token: 0x040006D6 RID: 1750
		NoHostExtendX = 65536,
		// Token: 0x040006D7 RID: 1751
		NoHostExtendY = 131072,
		// Token: 0x040006D8 RID: 1752
		NoKeepColumnsVisible = 262144,
		// Token: 0x040006D9 RID: 1753
		PreciseWidths = 524288,
		// Token: 0x040006DA RID: 1754
		NoClip = 1048576,
		// Token: 0x040006DB RID: 1755
		PadOuterX = 2097152,
		// Token: 0x040006DC RID: 1756
		NoPadOuterX = 4194304,
		// Token: 0x040006DD RID: 1757
		NoPadInnerX = 8388608,
		// Token: 0x040006DE RID: 1758
		ScrollX = 16777216,
		// Token: 0x040006DF RID: 1759
		ScrollY = 33554432,
		// Token: 0x040006E0 RID: 1760
		SortMulti = 67108864,
		// Token: 0x040006E1 RID: 1761
		SortTristate = 134217728,
		// Token: 0x040006E2 RID: 1762
		SizingMask = 57344
	}
}
