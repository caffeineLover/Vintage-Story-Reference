using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Writer;

namespace UglyToad.PdfPig.Graphics.Operations
{
	// Token: 0x0200011C RID: 284
	public class SetStrokeColorAdvanced : IGraphicsStateOperation
	{
		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x000229CC File Offset: 0x00020BCC
		public string Operator
		{
			get
			{
				return "SCN";
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x000229D3 File Offset: 0x00020BD3
		public IReadOnlyList<decimal> Operands { get; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x000229DB File Offset: 0x00020BDB
		public NameToken PatternName { get; }

		// Token: 0x0600063B RID: 1595 RVA: 0x000229E3 File Offset: 0x00020BE3
		public SetStrokeColorAdvanced(IReadOnlyList<decimal> operands)
		{
			this.Operands = operands;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x000229F2 File Offset: 0x00020BF2
		public SetStrokeColorAdvanced(IReadOnlyList<decimal> operands, NameToken patternName)
		{
			this.Operands = operands;
			this.PatternName = patternName;
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00022A08 File Offset: 0x00020C08
		public void Run(IOperationContext operationContext)
		{
			operationContext.GetCurrentState().ColorSpaceContext.SetStrokingColor(this.Operands, this.PatternName);
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00022A28 File Offset: 0x00020C28
		public void Write(Stream stream)
		{
			foreach (decimal value in this.Operands)
			{
				stream.WriteDecimal(value);
				stream.WriteWhiteSpace();
			}
			if (this.PatternName != null)
			{
				SetStrokeColorAdvanced.TokenWriter.WriteToken(this.PatternName, stream);
			}
			stream.WriteText("SCN", false);
			stream.WriteNewLine();
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00022AAC File Offset: 0x00020CAC
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
			return text + " SCN";
		}

		// Token: 0x04000312 RID: 786
		private static readonly TokenWriter TokenWriter = new TokenWriter();

		// Token: 0x04000313 RID: 787
		public const string Symbol = "SCN";
	}
}
