using System;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Graphics.Operations.TextState
{
	// Token: 0x02000125 RID: 293
	public class SetTextRenderingMode : IGraphicsStateOperation
	{
		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x000231AE File Offset: 0x000213AE
		public string Operator
		{
			get
			{
				return "Tr";
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x000231B5 File Offset: 0x000213B5
		public TextRenderingMode Mode { get; }

		// Token: 0x0600067A RID: 1658 RVA: 0x000231BD File Offset: 0x000213BD
		public SetTextRenderingMode(TextRenderingMode mode)
		{
			this.Mode = mode;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x000231CC File Offset: 0x000213CC
		public SetTextRenderingMode(int mode)
		{
			this.Mode = mode;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x000231DB File Offset: 0x000213DB
		public void Run(IOperationContext operationContext)
		{
			operationContext.SetTextRenderingMode(this.Mode);
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x000231E9 File Offset: 0x000213E9
		public void Write(Stream stream)
		{
			stream.WriteNumberText(this.Mode, "Tr");
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00023204 File Offset: 0x00021404
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TextRenderingMode>(this.Mode);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("Tr");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400032D RID: 813
		public const string Symbol = "Tr";
	}
}
