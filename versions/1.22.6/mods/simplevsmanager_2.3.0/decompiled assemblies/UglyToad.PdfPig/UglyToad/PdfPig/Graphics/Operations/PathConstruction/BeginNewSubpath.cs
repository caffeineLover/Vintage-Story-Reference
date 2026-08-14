using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations.PathConstruction
{
	// Token: 0x02000147 RID: 327
	public class BeginNewSubpath : IGraphicsStateOperation
	{
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x00024CFF File Offset: 0x00022EFF
		public string Operator
		{
			get
			{
				return "m";
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x00024D06 File Offset: 0x00022F06
		public decimal X { get; }

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x00024D0E File Offset: 0x00022F0E
		public decimal Y { get; }

		// Token: 0x06000768 RID: 1896 RVA: 0x00024D16 File Offset: 0x00022F16
		public BeginNewSubpath(decimal x, decimal y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00024D2C File Offset: 0x00022F2C
		public void Run(IOperationContext operationContext)
		{
			operationContext.MoveTo((double)this.X, (double)this.Y);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00024D4C File Offset: 0x00022F4C
		public void Write(Stream stream)
		{
			stream.WriteDecimal(this.X);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Y);
			stream.WriteWhiteSpace();
			stream.WriteText("m", false);
			stream.WriteNewLine();
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x00024D84 File Offset: 0x00022F84
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.X);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Y);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("m");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400038E RID: 910
		public const string Symbol = "m";
	}
}
