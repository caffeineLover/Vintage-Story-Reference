using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001EA RID: 490
	public class MediaBox
	{
		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x0003A2A0 File Offset: 0x000384A0
		public PdfRectangle Bounds { get; }

		// Token: 0x06000BCB RID: 3019 RVA: 0x0003A2A8 File Offset: 0x000384A8
		public MediaBox(PdfRectangle? bounds)
		{
			PdfRectangle? pdfRectangle = bounds;
			if (pdfRectangle == null)
			{
				throw new ArgumentNullException("bounds");
			}
			this.Bounds = pdfRectangle.GetValueOrDefault();
		}

		// Token: 0x0400061D RID: 1565
		private const double PointsPerInch = 72.0;

		// Token: 0x0400061E RID: 1566
		private const double PointsPerMm = 2.8346456692913384;

		// Token: 0x0400061F RID: 1567
		public static readonly MediaBox Letter = new MediaBox(new PdfRectangle?(new PdfRectangle(0.0, 0.0, 612.0, 792.0)));

		// Token: 0x04000620 RID: 1568
		public static readonly MediaBox Legal = new MediaBox(new PdfRectangle?(new PdfRectangle(0.0, 0.0, 612.0, 1008.0)));

		// Token: 0x04000621 RID: 1569
		public static readonly MediaBox A0 = new MediaBox(new PdfRectangle?(new PdfRectangle(0.0, 0.0, 2383.9370078740158, 3370.3937007874015)));

		// Token: 0x04000622 RID: 1570
		public static readonly MediaBox A1 = new MediaBox(new PdfRectangle?(new PdfRectangle(0.0, 0.0, 1683.779527559055, 2383.9370078740158)));

		// Token: 0x04000623 RID: 1571
		public static readonly MediaBox A2 = new MediaBox(new PdfRectangle?(new PdfRectangle(0.0, 0.0, 1190.551181102362, 1683.779527559055)));

		// Token: 0x04000624 RID: 1572
		public static readonly MediaBox A3 = new MediaBox(new PdfRectangle?(new PdfRectangle(0.0, 0.0, 841.8897637795275, 1190.551181102362)));

		// Token: 0x04000625 RID: 1573
		public static readonly MediaBox A4 = new MediaBox(new PdfRectangle?(new PdfRectangle(0.0, 0.0, 595.275590551181, 841.8897637795275)));

		// Token: 0x04000626 RID: 1574
		public static readonly MediaBox A5 = new MediaBox(new PdfRectangle?(new PdfRectangle(0.0, 0.0, 419.5275590551181, 595.275590551181)));

		// Token: 0x04000627 RID: 1575
		public static readonly MediaBox A6 = new MediaBox(new PdfRectangle?(new PdfRectangle(0.0, 0.0, 297.6377952755905, 419.5275590551181)));
	}
}
