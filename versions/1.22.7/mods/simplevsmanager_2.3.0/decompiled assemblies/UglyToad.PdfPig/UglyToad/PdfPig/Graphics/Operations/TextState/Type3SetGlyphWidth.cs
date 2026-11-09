using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations.TextState
{
	// Token: 0x02000128 RID: 296
	public class Type3SetGlyphWidth : IGraphicsStateOperation
	{
		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x00023356 File Offset: 0x00021556
		public string Operator
		{
			get
			{
				return "d0";
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x0002335D File Offset: 0x0002155D
		public decimal HorizontalDisplacement { get; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x00023365 File Offset: 0x00021565
		public decimal VerticalDisplacement { get; }

		// Token: 0x0600068E RID: 1678 RVA: 0x0002336D File Offset: 0x0002156D
		public Type3SetGlyphWidth(decimal horizontalDisplacement, decimal verticalDisplacement)
		{
			this.HorizontalDisplacement = horizontalDisplacement;
			this.VerticalDisplacement = verticalDisplacement;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00023383 File Offset: 0x00021583
		public void Run(IOperationContext operationContext)
		{
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00023385 File Offset: 0x00021585
		public void Write(Stream stream)
		{
			stream.WriteDecimal(this.HorizontalDisplacement);
			stream.WriteWhiteSpace();
			stream.WriteNumberText(this.VerticalDisplacement, "d0");
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x000233AC File Offset: 0x000215AC
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.HorizontalDisplacement);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.VerticalDisplacement);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("d0");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000333 RID: 819
		public const string Symbol = "d0";
	}
}
