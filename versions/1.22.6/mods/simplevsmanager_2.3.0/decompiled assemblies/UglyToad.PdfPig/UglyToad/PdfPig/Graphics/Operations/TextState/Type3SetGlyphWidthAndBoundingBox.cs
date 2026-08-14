using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations.TextState
{
	// Token: 0x02000129 RID: 297
	public class Type3SetGlyphWidthAndBoundingBox : IGraphicsStateOperation
	{
		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x00023407 File Offset: 0x00021607
		public string Operator
		{
			get
			{
				return "d1";
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x0002340E File Offset: 0x0002160E
		public decimal HorizontalDisplacement { get; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x00023416 File Offset: 0x00021616
		public decimal VerticalDisplacement { get; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x0002341E File Offset: 0x0002161E
		public decimal LowerLeftX { get; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x00023426 File Offset: 0x00021626
		public decimal LowerLeftY { get; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x0002342E File Offset: 0x0002162E
		public decimal UpperRightX { get; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x00023436 File Offset: 0x00021636
		public decimal UpperRightY { get; }

		// Token: 0x06000699 RID: 1689 RVA: 0x0002343E File Offset: 0x0002163E
		public Type3SetGlyphWidthAndBoundingBox(decimal horizontalDisplacement, decimal verticalDisplacement, decimal lowerLeftX, decimal lowerLeftY, decimal upperRightX, decimal upperRightY)
		{
			this.HorizontalDisplacement = horizontalDisplacement;
			this.VerticalDisplacement = verticalDisplacement;
			this.LowerLeftX = lowerLeftX;
			this.LowerLeftY = lowerLeftY;
			this.UpperRightX = upperRightX;
			this.UpperRightY = upperRightY;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00023473 File Offset: 0x00021673
		public void Run(IOperationContext operationContext)
		{
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00023478 File Offset: 0x00021678
		public void Write(Stream stream)
		{
			stream.WriteDecimal(this.HorizontalDisplacement);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.VerticalDisplacement);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.LowerLeftX);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.LowerLeftY);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.UpperRightX);
			stream.WriteWhiteSpace();
			stream.WriteNumberText(this.UpperRightY, "d1");
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x000234F0 File Offset: 0x000216F0
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(6, 7);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.HorizontalDisplacement);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.VerticalDisplacement);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.LowerLeftX);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.LowerLeftY);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.UpperRightX);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.UpperRightY);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("d1");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000336 RID: 822
		public const string Symbol = "d1";
	}
}
