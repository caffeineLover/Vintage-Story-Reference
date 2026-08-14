using System;

namespace QuestPDF.Skia.Text
{
	// Token: 0x02000029 RID: 41
	internal struct SkPlaceholderStyle
	{
		// Token: 0x060000E0 RID: 224 RVA: 0x00003C79 File Offset: 0x00001E79
		public SkPlaceholderStyle()
		{
			this.Width = 0f;
			this.Height = 0f;
			this.Alignment = SkPlaceholderStyle.PlaceholderAlignment.AboveBaseline;
			this.Baseline = SkPlaceholderStyle.PlaceholderBaseline.Alphabetic;
			this.BaselineOffset = 0f;
		}

		// Token: 0x04000082 RID: 130
		public float Width;

		// Token: 0x04000083 RID: 131
		public float Height;

		// Token: 0x04000084 RID: 132
		public SkPlaceholderStyle.PlaceholderAlignment Alignment;

		// Token: 0x04000085 RID: 133
		public SkPlaceholderStyle.PlaceholderBaseline Baseline;

		// Token: 0x04000086 RID: 134
		public float BaselineOffset;

		// Token: 0x0200014B RID: 331
		internal enum PlaceholderAlignment
		{
			// Token: 0x04000445 RID: 1093
			Baseline,
			// Token: 0x04000446 RID: 1094
			AboveBaseline,
			// Token: 0x04000447 RID: 1095
			BelowBaseline,
			// Token: 0x04000448 RID: 1096
			Top,
			// Token: 0x04000449 RID: 1097
			Bottom,
			// Token: 0x0400044A RID: 1098
			Middle
		}

		// Token: 0x0200014C RID: 332
		internal enum PlaceholderBaseline
		{
			// Token: 0x0400044C RID: 1100
			Alphabetic,
			// Token: 0x0400044D RID: 1101
			Ideographic
		}
	}
}
