using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations.TextState
{
	// Token: 0x02000126 RID: 294
	public class SetTextRise : IGraphicsStateOperation
	{
		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x00023246 File Offset: 0x00021446
		public string Operator
		{
			get
			{
				return "Ts";
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x0002324D File Offset: 0x0002144D
		public decimal Rise { get; }

		// Token: 0x06000681 RID: 1665 RVA: 0x00023255 File Offset: 0x00021455
		public SetTextRise(decimal rise)
		{
			this.Rise = rise;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00023264 File Offset: 0x00021464
		public void Run(IOperationContext operationContext)
		{
			operationContext.SetTextRise((double)this.Rise);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00023278 File Offset: 0x00021478
		public void Write(Stream stream)
		{
			stream.WriteNumberText(this.Rise, "Ts");
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0002328C File Offset: 0x0002148C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Rise);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("Ts");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400032F RID: 815
		public const string Symbol = "Ts";
	}
}
