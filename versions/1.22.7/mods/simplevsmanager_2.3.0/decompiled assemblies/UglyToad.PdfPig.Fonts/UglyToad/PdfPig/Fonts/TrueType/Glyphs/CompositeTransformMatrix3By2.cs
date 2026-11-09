using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.TrueType.Glyphs
{
	// Token: 0x02000065 RID: 101
	internal struct CompositeTransformMatrix3By2
	{
		// Token: 0x0600027C RID: 636 RVA: 0x0000CDFD File Offset: 0x0000AFFD
		public CompositeTransformMatrix3By2(double r0C0, double r0C1, double r1C0, double r1C1, double r2C0, double r2C1)
		{
			this.r0c0 = r0C0;
			this.r0c1 = r0C1;
			this.r1c0 = r1C0;
			this.r1c1 = r1C1;
			this.r2c0 = r2C0;
			this.r2c1 = r2C1;
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0000CE2C File Offset: 0x0000B02C
		public static CompositeTransformMatrix3By2 Identity { get; } = new CompositeTransformMatrix3By2(1.0, 0.0, 0.0, 1.0, 0.0, 0.0);

		// Token: 0x0600027E RID: 638 RVA: 0x0000CE33 File Offset: 0x0000B033
		public static CompositeTransformMatrix3By2 CreateTranslation(double x, double y)
		{
			return new CompositeTransformMatrix3By2(1.0, 0.0, 0.0, 1.0, x, y);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000CE60 File Offset: 0x0000B060
		public CompositeTransformMatrix3By2 WithTranslation(double x, double y)
		{
			return new CompositeTransformMatrix3By2(this.r0c0, this.r0c1, this.r1c0, this.r1c1, x, y);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000CE84 File Offset: 0x0000B084
		public PdfPoint ScaleAndRotate(PdfPoint source)
		{
			double num = source.X * this.r0c0 + source.Y * this.r1c0;
			double num2 = source.X * this.r0c1 + source.Y * this.r1c1;
			return new PdfPoint(num, num2);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000CED2 File Offset: 0x0000B0D2
		public PdfPoint Translate(PdfPoint source)
		{
			return new PdfPoint(source.X + this.r2c0, source.Y + this.r2c1);
		}

		// Token: 0x040002AF RID: 687
		private readonly double r0c0;

		// Token: 0x040002B0 RID: 688
		private readonly double r0c1;

		// Token: 0x040002B1 RID: 689
		private readonly double r1c0;

		// Token: 0x040002B2 RID: 690
		private readonly double r1c1;

		// Token: 0x040002B3 RID: 691
		private readonly double r2c0;

		// Token: 0x040002B4 RID: 692
		private readonly double r2c1;
	}
}
