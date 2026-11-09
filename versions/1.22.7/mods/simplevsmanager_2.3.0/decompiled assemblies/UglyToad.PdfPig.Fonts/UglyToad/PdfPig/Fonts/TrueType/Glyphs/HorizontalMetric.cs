using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.TrueType.Glyphs
{
	// Token: 0x02000068 RID: 104
	public struct HorizontalMetric
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000D5FA File Offset: 0x0000B7FA
		public readonly ushort AdvanceWidth { get; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000D602 File Offset: 0x0000B802
		public readonly short LeftSideBearing { get; }

		// Token: 0x0600029D RID: 669 RVA: 0x0000D60A File Offset: 0x0000B80A
		public HorizontalMetric(ushort advanceWidth, short leftSideBearing)
		{
			this.AdvanceWidth = advanceWidth;
			this.LeftSideBearing = leftSideBearing;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000D61C File Offset: 0x0000B81C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(15, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Width: ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(this.AdvanceWidth);
			defaultInterpolatedStringHandler.AppendLiteral(". LSB: ");
			defaultInterpolatedStringHandler.AppendFormatted<short>(this.LeftSideBearing);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
