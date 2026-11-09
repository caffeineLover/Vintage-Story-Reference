using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.AdobeFontMetrics
{
	// Token: 0x020000B4 RID: 180
	public struct AdobeFontMetricsVector
	{
		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x0001F19A File Offset: 0x0001D39A
		public readonly double X { get; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x0001F1A2 File Offset: 0x0001D3A2
		public readonly double Y { get; }

		// Token: 0x060004A6 RID: 1190 RVA: 0x0001F1AA File Offset: 0x0001D3AA
		public AdobeFontMetricsVector(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0001F1BC File Offset: 0x0001D3BC
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
