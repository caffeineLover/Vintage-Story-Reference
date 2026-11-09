using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace UglyToad.PdfPig.PdfFonts
{
	// Token: 0x0200008B RID: 139
	internal class CharStringStack
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000FC31 File Offset: 0x0000DE31
		public int Length
		{
			get
			{
				return this.stack.Count;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000FC3E File Offset: 0x0000DE3E
		public bool CanPop
		{
			get
			{
				return this.stack.Count > 0;
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000FC50 File Offset: 0x0000DE50
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

		// Token: 0x06000238 RID: 568 RVA: 0x0000FCA5 File Offset: 0x0000DEA5
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

		// Token: 0x06000239 RID: 569 RVA: 0x0000FCD7 File Offset: 0x0000DED7
		public void Push(double value)
		{
			this.stack.Add(value);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000FCE5 File Offset: 0x0000DEE5
		public double CopyElementAt(int index)
		{
			if (index < 0)
			{
				return this.stack[this.stack.Count - 1];
			}
			return this.stack[index];
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000FD10 File Offset: 0x0000DF10
		public void Clear()
		{
			this.stack.Clear();
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000FD1D File Offset: 0x0000DF1D
		public override string ToString()
		{
			return string.Join(" ", Enumerable.Select<double, string>(this.stack, (double x) => x.ToString(CultureInfo.InvariantCulture)));
		}

		// Token: 0x04000115 RID: 277
		private readonly List<double> stack = new List<double>();
	}
}
