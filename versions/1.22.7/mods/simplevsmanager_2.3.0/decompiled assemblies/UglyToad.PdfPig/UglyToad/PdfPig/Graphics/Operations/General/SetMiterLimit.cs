using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations.General
{
	// Token: 0x02000157 RID: 343
	public class SetMiterLimit : IGraphicsStateOperation
	{
		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x0002576A File Offset: 0x0002396A
		public string Operator
		{
			get
			{
				return "M";
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x00025771 File Offset: 0x00023971
		public decimal Limit { get; }

		// Token: 0x060007D2 RID: 2002 RVA: 0x00025779 File Offset: 0x00023979
		public SetMiterLimit(decimal limit)
		{
			this.Limit = limit;
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x00025788 File Offset: 0x00023988
		public void Run(IOperationContext operationContext)
		{
			operationContext.SetMiterLimit(this.Limit);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00025796 File Offset: 0x00023996
		public void Write(Stream stream)
		{
			stream.WriteNumberText(this.Limit, "M");
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x000257AC File Offset: 0x000239AC
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Limit);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("M");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040003B7 RID: 951
		public const string Symbol = "M";
	}
}
