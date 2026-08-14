using System;
using System.Collections.Generic;
using System.Linq;

namespace UglyToad.PdfPig.Functions.Type4
{
	// Token: 0x020001B1 RID: 433
	internal sealed class InstructionSequence
	{
		// Token: 0x06000A7D RID: 2685 RVA: 0x0003462A File Offset: 0x0003282A
		public void AddName(string name)
		{
			this.instructions.Add(name);
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00034638 File Offset: 0x00032838
		public void AddInteger(int value)
		{
			this.instructions.Add(value);
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0003464B File Offset: 0x0003284B
		public void AddReal(double value)
		{
			this.instructions.Add(value);
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x0003465E File Offset: 0x0003285E
		public void AddBoolean(bool value)
		{
			this.instructions.Add(value);
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00034671 File Offset: 0x00032871
		public void AddProc(InstructionSequence child)
		{
			this.instructions.Add(child);
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00034680 File Offset: 0x00032880
		public void Execute(ExecutionContext context)
		{
			using (List<object>.Enumerator enumerator = this.instructions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					string text = obj as string;
					if (text != null)
					{
						Operator @operator = context.GetOperators().GetOperator(text);
						if (@operator == null)
						{
							throw new InvalidOperationException("Unknown operator or name: " + text);
						}
						@operator.Execute(context);
					}
					else
					{
						context.Stack.Push(obj);
					}
				}
				goto IL_85;
			}
			IL_6F:
			((InstructionSequence)context.Stack.Pop()).Execute(context);
			IL_85:
			if (!Enumerable.Any<object>(context.Stack) || !(context.Stack.Peek() is InstructionSequence))
			{
				return;
			}
			goto IL_6F;
		}

		// Token: 0x0400051E RID: 1310
		private readonly List<object> instructions = new List<object>();
	}
}
