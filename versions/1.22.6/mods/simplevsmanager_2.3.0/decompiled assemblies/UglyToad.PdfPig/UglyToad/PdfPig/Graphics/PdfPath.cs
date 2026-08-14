using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Graphics.Core;

namespace UglyToad.PdfPig.Graphics
{
	// Token: 0x0200010C RID: 268
	public class PdfPath : List<PdfSubpath>
	{
		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x0001FF9F File Offset: 0x0001E19F
		// (set) Token: 0x060005C4 RID: 1476 RVA: 0x0001FFA7 File Offset: 0x0001E1A7
		public FillingRule FillingRule { get; private set; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x0001FFB0 File Offset: 0x0001E1B0
		// (set) Token: 0x060005C6 RID: 1478 RVA: 0x0001FFB8 File Offset: 0x0001E1B8
		public bool IsClipping { get; private set; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x0001FFC1 File Offset: 0x0001E1C1
		// (set) Token: 0x060005C8 RID: 1480 RVA: 0x0001FFC9 File Offset: 0x0001E1C9
		public bool IsFilled { get; private set; }

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x0001FFD2 File Offset: 0x0001E1D2
		// (set) Token: 0x060005CA RID: 1482 RVA: 0x0001FFDA File Offset: 0x0001E1DA
		public IColor FillColor { get; internal set; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x0001FFE3 File Offset: 0x0001E1E3
		// (set) Token: 0x060005CC RID: 1484 RVA: 0x0001FFEB File Offset: 0x0001E1EB
		public bool IsStroked { get; private set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x0001FFF4 File Offset: 0x0001E1F4
		// (set) Token: 0x060005CE RID: 1486 RVA: 0x0001FFFC File Offset: 0x0001E1FC
		public IColor StrokeColor { get; internal set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x00020005 File Offset: 0x0001E205
		// (set) Token: 0x060005D0 RID: 1488 RVA: 0x0002000D File Offset: 0x0001E20D
		public decimal LineWidth { get; internal set; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x00020016 File Offset: 0x0001E216
		// (set) Token: 0x060005D2 RID: 1490 RVA: 0x0002001E File Offset: 0x0001E21E
		public LineDashPattern? LineDashPattern { get; internal set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x00020027 File Offset: 0x0001E227
		// (set) Token: 0x060005D4 RID: 1492 RVA: 0x0002002F File Offset: 0x0001E22F
		public LineCapStyle LineCapStyle { get; internal set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x00020038 File Offset: 0x0001E238
		// (set) Token: 0x060005D6 RID: 1494 RVA: 0x00020040 File Offset: 0x0001E240
		public LineJoinStyle LineJoinStyle { get; internal set; }

		// Token: 0x060005D7 RID: 1495 RVA: 0x00020049 File Offset: 0x0001E249
		public void SetClipping(FillingRule fillingRule)
		{
			this.IsFilled = false;
			this.IsStroked = false;
			this.IsClipping = true;
			this.FillingRule = fillingRule;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00020067 File Offset: 0x0001E267
		public void SetFilled(FillingRule fillingRule)
		{
			this.IsFilled = true;
			this.FillingRule = fillingRule;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00020077 File Offset: 0x0001E277
		public void SetStroked()
		{
			this.IsStroked = true;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00020080 File Offset: 0x0001E280
		internal PdfPath CloneEmpty()
		{
			PdfPath pdfPath = new PdfPath();
			if (this.IsClipping)
			{
				pdfPath.SetClipping(this.FillingRule);
			}
			else
			{
				if (this.IsFilled)
				{
					pdfPath.SetFilled(this.FillingRule);
					pdfPath.FillColor = this.FillColor;
				}
				if (this.IsStroked)
				{
					pdfPath.SetStroked();
					pdfPath.LineCapStyle = this.LineCapStyle;
					pdfPath.LineDashPattern = this.LineDashPattern;
					pdfPath.LineJoinStyle = this.LineJoinStyle;
					pdfPath.LineWidth = this.LineWidth;
					pdfPath.StrokeColor = this.StrokeColor;
				}
			}
			return pdfPath;
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00020114 File Offset: 0x0001E314
		public PdfRectangle? GetBoundingRectangle()
		{
			return PdfSubpath.GetBoundingRectangle(this);
		}
	}
}
