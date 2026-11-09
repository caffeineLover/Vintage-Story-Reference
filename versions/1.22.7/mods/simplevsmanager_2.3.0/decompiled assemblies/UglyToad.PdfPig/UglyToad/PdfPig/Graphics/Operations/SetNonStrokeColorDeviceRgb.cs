using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations
{
	// Token: 0x02000119 RID: 281
	public class SetNonStrokeColorDeviceRgb : IGraphicsStateOperation
	{
		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x00022702 File Offset: 0x00020902
		public string Operator
		{
			get
			{
				return "rg";
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x00022709 File Offset: 0x00020909
		public decimal R { get; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x00022711 File Offset: 0x00020911
		public decimal G { get; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x00022719 File Offset: 0x00020919
		public decimal B { get; }

		// Token: 0x06000627 RID: 1575 RVA: 0x00022721 File Offset: 0x00020921
		public SetNonStrokeColorDeviceRgb(decimal r, decimal g, decimal b)
		{
			this.R = r;
			this.G = g;
			this.B = b;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0002273E File Offset: 0x0002093E
		public void Run(IOperationContext operationContext)
		{
			operationContext.GetCurrentState().ColorSpaceContext.SetNonStrokingColorRgb(this.R, this.G, this.B);
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00022764 File Offset: 0x00020964
		public void Write(Stream stream)
		{
			stream.WriteDecimal(this.R);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.G);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.B);
			stream.WriteWhiteSpace();
			stream.WriteText("rg", false);
			stream.WriteNewLine();
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x000227BC File Offset: 0x000209BC
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(3, 4);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.R);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.G);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.B);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("rg");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000309 RID: 777
		public const string Symbol = "rg";
	}
}
