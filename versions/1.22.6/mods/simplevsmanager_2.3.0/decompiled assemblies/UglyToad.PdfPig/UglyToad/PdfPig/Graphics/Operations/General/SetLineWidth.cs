using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations.General
{
	// Token: 0x02000156 RID: 342
	public class SetLineWidth : IGraphicsStateOperation
	{
		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060007CA RID: 1994 RVA: 0x000256E6 File Offset: 0x000238E6
		public string Operator
		{
			get
			{
				return "w";
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x000256ED File Offset: 0x000238ED
		public decimal Width { get; }

		// Token: 0x060007CC RID: 1996 RVA: 0x000256F5 File Offset: 0x000238F5
		public SetLineWidth(decimal width)
		{
			this.Width = width;
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00025704 File Offset: 0x00023904
		public void Run(IOperationContext operationContext)
		{
			operationContext.SetLineWidth(this.Width);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00025712 File Offset: 0x00023912
		public void Write(Stream stream)
		{
			stream.WriteNumberText(this.Width, "w");
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00025728 File Offset: 0x00023928
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Width);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("w");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040003B5 RID: 949
		public const string Symbol = "w";
	}
}
