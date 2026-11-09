using System;

namespace QuestPDF.Skia
{
	// Token: 0x0200001F RID: 31
	internal struct SkSvgImageSize
	{
		// Token: 0x0400006E RID: 110
		public float Width;

		// Token: 0x0400006F RID: 111
		public float Height;

		// Token: 0x04000070 RID: 112
		public SkSvgImageSize.Unit WidthUnit;

		// Token: 0x04000071 RID: 113
		public SkSvgImageSize.Unit HeightUnit;

		// Token: 0x02000141 RID: 321
		public enum Unit
		{
			// Token: 0x04000430 RID: 1072
			Unknown,
			// Token: 0x04000431 RID: 1073
			Number,
			// Token: 0x04000432 RID: 1074
			Percentage,
			// Token: 0x04000433 RID: 1075
			Pixels,
			// Token: 0x04000434 RID: 1076
			Centimeters,
			// Token: 0x04000435 RID: 1077
			Millimeters,
			// Token: 0x04000436 RID: 1078
			Inches,
			// Token: 0x04000437 RID: 1079
			Points,
			// Token: 0x04000438 RID: 1080
			Picas
		}
	}
}
