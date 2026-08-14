using System;
using System.Collections.Generic;
using System.Linq;

namespace UglyToad.PdfPig.Functions.Type4
{
	// Token: 0x020001B7 RID: 439
	internal sealed class StackOperators
	{
		// Token: 0x06000A91 RID: 2705 RVA: 0x00034DEB File Offset: 0x00032FEB
		private StackOperators()
		{
		}

		// Token: 0x020002AB RID: 683
		internal sealed class Copy : Operator
		{
			// Token: 0x06000EE1 RID: 3809 RVA: 0x00043804 File Offset: 0x00041A04
			public void Execute(ExecutionContext context)
			{
				int num = (int)context.Stack.Pop();
				if (num > 0)
				{
					int count = context.Stack.Count;
					List<object> range = Enumerable.ToList<object>(context.Stack).GetRange(count - num - 1, num);
					context.AddAllToStack(range);
				}
			}
		}

		// Token: 0x020002AC RID: 684
		internal sealed class Dup : Operator
		{
			// Token: 0x06000EE3 RID: 3811 RVA: 0x00043858 File Offset: 0x00041A58
			public void Execute(ExecutionContext context)
			{
				context.Stack.Push(context.Stack.Peek());
			}
		}

		// Token: 0x020002AD RID: 685
		internal sealed class Exch : Operator
		{
			// Token: 0x06000EE5 RID: 3813 RVA: 0x00043878 File Offset: 0x00041A78
			public void Execute(ExecutionContext context)
			{
				object obj = context.Stack.Pop();
				object obj2 = context.Stack.Pop();
				context.Stack.Push(obj);
				context.Stack.Push(obj2);
			}
		}

		// Token: 0x020002AE RID: 686
		internal sealed class Index : Operator
		{
			// Token: 0x06000EE7 RID: 3815 RVA: 0x000438C0 File Offset: 0x00041AC0
			public void Execute(ExecutionContext context)
			{
				int num = Convert.ToInt32(context.Stack.Pop());
				if (num < 0)
				{
					throw new ArgumentException("rangecheck: " + num.ToString());
				}
				context.Stack.Push(Enumerable.ElementAt<object>(context.Stack, num));
			}
		}

		// Token: 0x020002AF RID: 687
		internal sealed class Pop : Operator
		{
			// Token: 0x06000EE9 RID: 3817 RVA: 0x00043918 File Offset: 0x00041B18
			public void Execute(ExecutionContext context)
			{
				context.Stack.Pop();
			}
		}

		// Token: 0x020002B0 RID: 688
		internal sealed class Roll : Operator
		{
			// Token: 0x06000EEB RID: 3819 RVA: 0x00043930 File Offset: 0x00041B30
			public void Execute(ExecutionContext context)
			{
				int num = (int)context.Stack.Pop();
				int num2 = (int)context.Stack.Pop();
				if (num == 0)
				{
					return;
				}
				if (num2 < 0)
				{
					throw new ArgumentException("rangecheck: " + num2.ToString());
				}
				List<object> list = new List<object>();
				List<object> list2 = new List<object>();
				if (num < 0)
				{
					int num3 = num2 + num;
					for (int i = 0; i < num3; i++)
					{
						list2.Add(context.Stack.Pop());
					}
					for (int j = num; j < 0; j++)
					{
						list.Add(context.Stack.Pop());
					}
					context.AddAllToStack(list2);
					context.AddAllToStack(list);
					return;
				}
				int num4 = num2 - num;
				for (int k = num; k > 0; k--)
				{
					list.Add(context.Stack.Pop());
				}
				for (int l = 0; l < num4; l++)
				{
					list2.Add(context.Stack.Pop());
				}
				context.AddAllToStack(list);
				context.AddAllToStack(list2);
			}
		}
	}
}
