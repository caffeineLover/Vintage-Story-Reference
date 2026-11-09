using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Geometry;

namespace UglyToad.PdfPig.PdfFonts.CidFonts
{
	// Token: 0x020000C0 RID: 192
	internal struct VerticalVectorComponents
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060003AF RID: 943 RVA: 0x00015E4C File Offset: 0x0001404C
		public readonly double Position { get; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x00015E54 File Offset: 0x00014054
		public readonly double Displacement { get; }

		// Token: 0x060003B1 RID: 945 RVA: 0x00015E5C File Offset: 0x0001405C
		public VerticalVectorComponents(double position, double displacement)
		{
			this.Position = position;
			this.Displacement = displacement;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00015E6C File Offset: 0x0001406C
		public PdfVector GetPositionVector(double glyphWidth)
		{
			return new PdfVector(glyphWidth / 2.0, this.Position);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00015E84 File Offset: 0x00014084
		public PdfVector GetDisplacementVector()
		{
			return new PdfVector(0.0, this.Displacement);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00015E9C File Offset: 0x0001409C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(27, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Position: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.Position);
			defaultInterpolatedStringHandler.AppendLiteral(", Displacement: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.Displacement);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040001E8 RID: 488
		public static readonly VerticalVectorComponents Default = new VerticalVectorComponents(800.0, -1000.0);
	}
}
