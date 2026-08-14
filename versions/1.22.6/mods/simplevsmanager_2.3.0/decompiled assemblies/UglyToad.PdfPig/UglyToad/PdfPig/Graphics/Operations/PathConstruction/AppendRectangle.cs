using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations.PathConstruction
{
	// Token: 0x02000144 RID: 324
	public class AppendRectangle : IGraphicsStateOperation
	{
		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x00024925 File Offset: 0x00022B25
		public string Operator
		{
			get
			{
				return "re";
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x0002492C File Offset: 0x00022B2C
		public decimal LowerLeftX { get; }

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x0600074E RID: 1870 RVA: 0x00024934 File Offset: 0x00022B34
		public decimal LowerLeftY { get; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x0002493C File Offset: 0x00022B3C
		public decimal Width { get; }

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x00024944 File Offset: 0x00022B44
		public decimal Height { get; }

		// Token: 0x06000751 RID: 1873 RVA: 0x0002494C File Offset: 0x00022B4C
		public AppendRectangle(decimal x, decimal y, decimal width, decimal height)
		{
			this.LowerLeftX = x;
			this.LowerLeftY = y;
			this.Width = width;
			this.Height = height;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00024971 File Offset: 0x00022B71
		public void Run(IOperationContext operationContext)
		{
			operationContext.Rectangle((double)this.LowerLeftX, (double)this.LowerLeftY, (double)this.Width, (double)this.Height);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x000249AC File Offset: 0x00022BAC
		public void Write(Stream stream)
		{
			stream.WriteDecimal(this.LowerLeftX);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.LowerLeftY);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Width);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Height);
			stream.WriteWhiteSpace();
			stream.WriteText("re", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x00024A14 File Offset: 0x00022C14
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(4, 5);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.LowerLeftX);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.LowerLeftY);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Width);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Height);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("re");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000381 RID: 897
		public const string Symbol = "re";
	}
}
