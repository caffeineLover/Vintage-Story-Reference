using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations
{
	// Token: 0x0200011E RID: 286
	public class SetStrokeColorDeviceGray : IGraphicsStateOperation
	{
		// Token: 0x17000195 RID: 405
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x00022CB1 File Offset: 0x00020EB1
		public string Operator
		{
			get
			{
				return "G";
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00022CB8 File Offset: 0x00020EB8
		public decimal Gray { get; }

		// Token: 0x0600064C RID: 1612 RVA: 0x00022CC0 File Offset: 0x00020EC0
		public SetStrokeColorDeviceGray(decimal gray)
		{
			this.Gray = gray;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00022CCF File Offset: 0x00020ECF
		public void Run(IOperationContext operationContext)
		{
			operationContext.GetCurrentState().ColorSpaceContext.SetStrokingColorGray(this.Gray);
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00022CE7 File Offset: 0x00020EE7
		public void Write(Stream stream)
		{
			stream.WriteNumberText(this.Gray, "G");
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00022CFC File Offset: 0x00020EFC
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Gray);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("G");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400031B RID: 795
		public const string Symbol = "G";
	}
}
