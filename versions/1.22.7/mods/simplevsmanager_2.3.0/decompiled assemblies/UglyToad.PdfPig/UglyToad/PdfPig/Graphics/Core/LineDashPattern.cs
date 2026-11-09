using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Graphics.Core
{
	// Token: 0x0200015D RID: 349
	public struct LineDashPattern
	{
		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x000258DC File Offset: 0x00023ADC
		public readonly int Phase { get; }

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x000258E4 File Offset: 0x00023AE4
		[NotNull]
		public readonly IReadOnlyList<decimal> Array { get; }

		// Token: 0x060007F0 RID: 2032 RVA: 0x000258EC File Offset: 0x00023AEC
		public LineDashPattern(int phase, [NotNull] IReadOnlyList<decimal> array)
		{
			this.Phase = phase;
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			this.Array = array;
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0002590B File Offset: 0x00023B0B
		public static LineDashPattern Solid { get; } = new LineDashPattern(0, new decimal[0]);

		// Token: 0x060007F2 RID: 2034 RVA: 0x00025914 File Offset: 0x00023B14
		public override string ToString()
		{
			string text = string.Join(" ", Enumerable.Select<decimal, string>(this.Array, (decimal x) => x.ToString("N")));
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(4, 2);
			defaultInterpolatedStringHandler.AppendLiteral("[");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral("] ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Phase);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
