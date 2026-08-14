using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Writer;

namespace UglyToad.PdfPig.Graphics.Operations
{
	// Token: 0x02000116 RID: 278
	public class SetNonStrokeColorAdvanced : IGraphicsStateOperation
	{
		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x00022390 File Offset: 0x00020590
		public string Operator
		{
			get
			{
				return "scn";
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x00022397 File Offset: 0x00020597
		public IReadOnlyList<decimal> Operands { get; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x0002239F File Offset: 0x0002059F
		public NameToken PatternName { get; }

		// Token: 0x0600060E RID: 1550 RVA: 0x000223A7 File Offset: 0x000205A7
		public SetNonStrokeColorAdvanced(IReadOnlyList<decimal> operands)
		{
			this.Operands = operands;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x000223B6 File Offset: 0x000205B6
		public SetNonStrokeColorAdvanced(IReadOnlyList<decimal> operands, NameToken patternName)
		{
			this.Operands = operands;
			this.PatternName = patternName;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x000223CC File Offset: 0x000205CC
		public void Run(IOperationContext operationContext)
		{
			operationContext.GetCurrentState().ColorSpaceContext.SetNonStrokingColor(this.Operands, this.PatternName);
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x000223EC File Offset: 0x000205EC
		public void Write(Stream stream)
		{
			foreach (decimal value in this.Operands)
			{
				stream.WriteDecimal(value);
				stream.WriteWhiteSpace();
			}
			if (this.PatternName != null)
			{
				SetNonStrokeColorAdvanced.TokenWriter.WriteToken(this.PatternName, stream);
			}
			stream.WriteText("scn", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00022470 File Offset: 0x00020670
		public override string ToString()
		{
			string text = string.Join(" ", Enumerable.Select<decimal, string>(this.Operands, (decimal x) => x.ToString("N")));
			if (this.PatternName != null)
			{
				string text2 = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(1, 1);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(this.PatternName);
				text = text2 + defaultInterpolatedStringHandler.ToStringAndClear();
			}
			return text + " scn";
		}

		// Token: 0x040002FE RID: 766
		private static readonly TokenWriter TokenWriter = new TokenWriter();

		// Token: 0x040002FF RID: 767
		public const string Symbol = "scn";
	}
}
