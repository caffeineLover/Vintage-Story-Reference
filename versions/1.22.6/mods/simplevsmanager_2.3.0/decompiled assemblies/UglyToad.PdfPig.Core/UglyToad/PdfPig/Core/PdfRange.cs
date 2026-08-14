using System;
using System.Collections.Generic;
using System.Linq;

namespace UglyToad.PdfPig.Core
{
	// Token: 0x0200000E RID: 14
	public struct PdfRange
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002897 File Offset: 0x00000A97
		public PdfRange(IEnumerable<decimal> range)
		{
			this = new PdfRange(Enumerable.Select<decimal, double>(range, (decimal v) => (double)v), 0);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000028C5 File Offset: 0x00000AC5
		public PdfRange(IEnumerable<decimal> range, int index)
		{
			this = new PdfRange(Enumerable.Select<decimal, double>(range, (decimal v) => (double)v), index);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000028F3 File Offset: 0x00000AF3
		public PdfRange(IEnumerable<double> range)
		{
			this = new PdfRange(range, 0);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000028FD File Offset: 0x00000AFD
		public PdfRange(IEnumerable<double> range, int index)
		{
			this.rangeArray = Enumerable.ToArray<double>(range);
			this.startingIndex = index;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002912 File Offset: 0x00000B12
		public double Min
		{
			get
			{
				return this.rangeArray[this.startingIndex * 2];
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002927 File Offset: 0x00000B27
		public double Max
		{
			get
			{
				return this.rangeArray[this.startingIndex * 2 + 1];
			}
		}

		// Token: 0x04000012 RID: 18
		private readonly IReadOnlyList<double> rangeArray;

		// Token: 0x04000013 RID: 19
		private readonly int startingIndex;
	}
}
