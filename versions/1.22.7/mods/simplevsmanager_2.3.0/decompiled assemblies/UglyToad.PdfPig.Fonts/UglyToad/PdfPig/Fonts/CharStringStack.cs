using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace UglyToad.PdfPig.Fonts
{
	// Token: 0x02000003 RID: 3
	internal class CharStringStack
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000224B File Offset: 0x0000044B
		public int Length
		{
			get
			{
				return this.stack.Count;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002258 File Offset: 0x00000458
		public bool CanPop
		{
			get
			{
				return this.stack.Count > 0;
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002268 File Offset: 0x00000468
		public double PopTop()
		{
			if (this.stack.Count == 0)
			{
				throw new InvalidOperationException("Cannot pop from the top of an empty stack, invalid charstring parsed.");
			}
			double result = this.stack[this.stack.Count - 1];
			this.stack.RemoveAt(this.stack.Count - 1);
			return result;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000022BD File Offset: 0x000004BD
		public double PopBottom()
		{
			if (this.stack.Count == 0)
			{
				throw new InvalidOperationException("Cannot pop from the bottom of an empty stack, invalid charstring parsed.");
			}
			double result = this.stack[0];
			this.stack.RemoveAt(0);
			return result;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000022EF File Offset: 0x000004EF
		public void Push(double value)
		{
			this.stack.Add(value);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000022FD File Offset: 0x000004FD
		public double CopyElementAt(int index)
		{
			if (index < 0)
			{
				return this.stack[this.stack.Count - 1];
			}
			return this.stack[index];
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002328 File Offset: 0x00000528
		public void Clear()
		{
			this.stack.Clear();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002335 File Offset: 0x00000535
		public override string ToString()
		{
			return string.Join(" ", Enumerable.Select<double, string>(this.stack, (double x) => x.ToString(CultureInfo.InvariantCulture)));
		}

		// Token: 0x04000014 RID: 20
		private readonly List<double> stack = new List<double>();
	}
}
