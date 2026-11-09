using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.AdobeFontMetrics
{
	// Token: 0x020000AE RID: 174
	public class AdobeFontMetricsCharacterSize
	{
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x0001E039 File Offset: 0x0001C239
		public double X { get; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x0001E041 File Offset: 0x0001C241
		public double Y { get; }

		// Token: 0x06000472 RID: 1138 RVA: 0x0001E049 File Offset: 0x0001C249
		public AdobeFontMetricsCharacterSize(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0001E060 File Offset: 0x0001C260
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.X);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.Y);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
