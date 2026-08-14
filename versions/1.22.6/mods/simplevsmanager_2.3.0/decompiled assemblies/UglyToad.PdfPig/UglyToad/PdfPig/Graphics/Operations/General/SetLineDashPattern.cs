using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Graphics.Core;

namespace UglyToad.PdfPig.Graphics.Operations.General
{
	// Token: 0x02000154 RID: 340
	public class SetLineDashPattern : IGraphicsStateOperation
	{
		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x000254DE File Offset: 0x000236DE
		public string Operator
		{
			get
			{
				return "d";
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x000254E5 File Offset: 0x000236E5
		public LineDashPattern Pattern { get; }

		// Token: 0x060007BF RID: 1983 RVA: 0x000254ED File Offset: 0x000236ED
		public SetLineDashPattern(decimal[] array, int phase)
		{
			this.Pattern = new LineDashPattern(phase, array);
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x00025502 File Offset: 0x00023702
		public void Run(IOperationContext operationContext)
		{
			operationContext.SetLineDashPattern(this.Pattern);
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x00025510 File Offset: 0x00023710
		public void Write(Stream stream)
		{
			stream.WriteText("[", false);
			for (int i = 0; i < this.Pattern.Array.Count; i++)
			{
				decimal value = this.Pattern.Array[i];
				stream.WriteDecimal(value);
				if (i < this.Pattern.Array.Count - 1)
				{
					stream.WriteWhiteSpace();
				}
			}
			stream.WriteText("]", false);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Pattern.Phase);
			stream.WriteWhiteSpace();
			stream.WriteText("d", false);
			stream.WriteNewLine();
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x000255C4 File Offset: 0x000237C4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted<IReadOnlyList<decimal>>(this.Pattern.Array);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Pattern.Phase);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("d");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040003B1 RID: 945
		public const string Symbol = "d";
	}
}
