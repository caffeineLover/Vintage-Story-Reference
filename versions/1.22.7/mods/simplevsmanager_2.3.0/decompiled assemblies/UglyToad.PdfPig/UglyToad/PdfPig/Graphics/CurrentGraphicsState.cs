using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Graphics.Core;

namespace UglyToad.PdfPig.Graphics
{
	// Token: 0x02000106 RID: 262
	public class CurrentGraphicsState : IDeepCloneable<CurrentGraphicsState>
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x0001F7CD File Offset: 0x0001D9CD
		// (set) Token: 0x0600054F RID: 1359 RVA: 0x0001F7D5 File Offset: 0x0001D9D5
		public PdfPath CurrentClippingPath { get; set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x0001F7DE File Offset: 0x0001D9DE
		// (set) Token: 0x06000551 RID: 1361 RVA: 0x0001F7E6 File Offset: 0x0001D9E6
		public CurrentFontState FontState { get; set; } = new CurrentFontState();

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x0001F7EF File Offset: 0x0001D9EF
		// (set) Token: 0x06000553 RID: 1363 RVA: 0x0001F7F7 File Offset: 0x0001D9F7
		public decimal LineWidth { get; set; } = 1m;

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x0001F800 File Offset: 0x0001DA00
		// (set) Token: 0x06000555 RID: 1365 RVA: 0x0001F808 File Offset: 0x0001DA08
		public LineCapStyle CapStyle { get; set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x0001F811 File Offset: 0x0001DA11
		// (set) Token: 0x06000557 RID: 1367 RVA: 0x0001F819 File Offset: 0x0001DA19
		public LineJoinStyle JoinStyle { get; set; }

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x0001F822 File Offset: 0x0001DA22
		// (set) Token: 0x06000559 RID: 1369 RVA: 0x0001F82A File Offset: 0x0001DA2A
		public decimal MiterLimit { get; set; } = 10m;

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x0001F833 File Offset: 0x0001DA33
		// (set) Token: 0x0600055B RID: 1371 RVA: 0x0001F83B File Offset: 0x0001DA3B
		public LineDashPattern LineDashPattern { get; set; } = LineDashPattern.Solid;

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x0001F844 File Offset: 0x0001DA44
		// (set) Token: 0x0600055D RID: 1373 RVA: 0x0001F84C File Offset: 0x0001DA4C
		public RenderingIntent RenderingIntent { get; set; } = RenderingIntent.RelativeColorimetric;

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x0001F855 File Offset: 0x0001DA55
		// (set) Token: 0x0600055F RID: 1375 RVA: 0x0001F85D File Offset: 0x0001DA5D
		public bool StrokeAdjustment { get; set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x0001F866 File Offset: 0x0001DA66
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x0001F86E File Offset: 0x0001DA6E
		public decimal AlphaConstantStroking { get; set; } = 1m;

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x0001F877 File Offset: 0x0001DA77
		// (set) Token: 0x06000563 RID: 1379 RVA: 0x0001F87F File Offset: 0x0001DA7F
		public decimal AlphaConstantNonStroking { get; set; } = 1m;

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x0001F888 File Offset: 0x0001DA88
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x0001F890 File Offset: 0x0001DA90
		public bool AlphaSource { get; set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x0001F899 File Offset: 0x0001DA99
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x0001F8A1 File Offset: 0x0001DAA1
		public TransformationMatrix CurrentTransformationMatrix { get; set; } = TransformationMatrix.Identity;

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x0001F8AA File Offset: 0x0001DAAA
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x0001F8B2 File Offset: 0x0001DAB2
		public IColorSpaceContext ColorSpaceContext { get; set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x0001F8BB File Offset: 0x0001DABB
		// (set) Token: 0x0600056B RID: 1387 RVA: 0x0001F8C3 File Offset: 0x0001DAC3
		public IColor CurrentStrokingColor { get; set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x0001F8CC File Offset: 0x0001DACC
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x0001F8D4 File Offset: 0x0001DAD4
		public IColor CurrentNonStrokingColor { get; set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x0001F8DD File Offset: 0x0001DADD
		// (set) Token: 0x0600056F RID: 1391 RVA: 0x0001F8E5 File Offset: 0x0001DAE5
		public bool Overprint { get; set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x0001F8EE File Offset: 0x0001DAEE
		// (set) Token: 0x06000571 RID: 1393 RVA: 0x0001F8F6 File Offset: 0x0001DAF6
		public bool NonStrokingOverprint { get; set; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x0001F8FF File Offset: 0x0001DAFF
		// (set) Token: 0x06000573 RID: 1395 RVA: 0x0001F907 File Offset: 0x0001DB07
		public decimal OverprintMode { get; set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x0001F910 File Offset: 0x0001DB10
		// (set) Token: 0x06000575 RID: 1397 RVA: 0x0001F918 File Offset: 0x0001DB18
		public decimal Flatness { get; set; } = 1m;

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x0001F921 File Offset: 0x0001DB21
		// (set) Token: 0x06000577 RID: 1399 RVA: 0x0001F929 File Offset: 0x0001DB29
		public decimal Smoothness { get; set; }

		// Token: 0x06000578 RID: 1400 RVA: 0x0001F934 File Offset: 0x0001DB34
		public CurrentGraphicsState DeepClone()
		{
			CurrentGraphicsState currentGraphicsState = new CurrentGraphicsState();
			CurrentFontState fontState = this.FontState;
			currentGraphicsState.FontState = ((fontState != null) ? fontState.DeepClone() : null);
			currentGraphicsState.RenderingIntent = this.RenderingIntent;
			currentGraphicsState.LineDashPattern = this.LineDashPattern;
			currentGraphicsState.CurrentTransformationMatrix = this.CurrentTransformationMatrix;
			currentGraphicsState.LineWidth = this.LineWidth;
			currentGraphicsState.JoinStyle = this.JoinStyle;
			currentGraphicsState.Overprint = this.Overprint;
			currentGraphicsState.CapStyle = this.CapStyle;
			currentGraphicsState.MiterLimit = this.MiterLimit;
			currentGraphicsState.Flatness = this.Flatness;
			currentGraphicsState.AlphaConstantStroking = this.AlphaConstantStroking;
			currentGraphicsState.AlphaConstantNonStroking = this.AlphaConstantNonStroking;
			currentGraphicsState.AlphaSource = this.AlphaSource;
			currentGraphicsState.NonStrokingOverprint = this.NonStrokingOverprint;
			currentGraphicsState.OverprintMode = this.OverprintMode;
			currentGraphicsState.Smoothness = this.Smoothness;
			currentGraphicsState.StrokeAdjustment = this.StrokeAdjustment;
			currentGraphicsState.CurrentStrokingColor = this.CurrentStrokingColor;
			currentGraphicsState.CurrentNonStrokingColor = this.CurrentNonStrokingColor;
			currentGraphicsState.CurrentClippingPath = this.CurrentClippingPath;
			IColorSpaceContext colorSpaceContext = this.ColorSpaceContext;
			currentGraphicsState.ColorSpaceContext = ((colorSpaceContext != null) ? colorSpaceContext.DeepClone() : null);
			return currentGraphicsState;
		}
	}
}
