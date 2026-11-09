using System;
using System.Collections.Generic;
using System.Linq;

namespace UglyToad.PdfPig.Functions.Type4
{
	// Token: 0x020001B0 RID: 432
	internal sealed class ExecutionContext
	{
		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x0003453B File Offset: 0x0003273B
		// (set) Token: 0x06000A76 RID: 2678 RVA: 0x00034543 File Offset: 0x00032743
		public Stack<object> Stack { get; private set; } = new Stack<object>();

		// Token: 0x06000A77 RID: 2679 RVA: 0x0003454C File Offset: 0x0003274C
		public ExecutionContext(Operators operatorSet)
		{
			this.operators = operatorSet;
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00034568 File Offset: 0x00032768
		internal void AddAllToStack(IEnumerable<object> values)
		{
			List<object> list = Enumerable.ToList<object>(values);
			list.AddRange(this.Stack);
			list.Reverse();
			this.Stack = new Stack<object>(list);
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0003459A File Offset: 0x0003279A
		public Operators GetOperators()
		{
			return this.operators;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x000345A4 File Offset: 0x000327A4
		public object PopNumber()
		{
			object obj = this.Stack.Pop();
			if (obj is int || obj is double || obj is float)
			{
				return obj;
			}
			throw new InvalidCastException("The object popped is neither an integer or a real.");
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x000345E4 File Offset: 0x000327E4
		public int PopInt()
		{
			object obj = this.Stack.Pop();
			if (obj is int)
			{
				return (int)obj;
			}
			throw new InvalidCastException("PopInt cannot be done as the value is not integer");
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00034618 File Offset: 0x00032818
		public double PopReal()
		{
			return Convert.ToDouble(this.Stack.Pop());
		}

		// Token: 0x0400051C RID: 1308
		private readonly Operators operators;
	}
}
