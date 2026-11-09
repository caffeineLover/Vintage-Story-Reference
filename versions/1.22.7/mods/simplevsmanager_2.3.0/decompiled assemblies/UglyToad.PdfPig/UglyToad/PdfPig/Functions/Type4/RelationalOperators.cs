using System;

namespace UglyToad.PdfPig.Functions.Type4
{
	// Token: 0x020001B6 RID: 438
	internal sealed class RelationalOperators
	{
		// Token: 0x06000A90 RID: 2704 RVA: 0x00034DE3 File Offset: 0x00032FE3
		private RelationalOperators()
		{
		}

		// Token: 0x020002A4 RID: 676
		internal class Eq : Operator
		{
			// Token: 0x06000ED1 RID: 3793 RVA: 0x000436D8 File Offset: 0x000418D8
			public void Execute(ExecutionContext context)
			{
				object op = context.Stack.Pop();
				object op2 = context.Stack.Pop();
				bool flag = this.IsEqual(op2, op);
				context.Stack.Push(flag);
			}

			// Token: 0x06000ED2 RID: 3794 RVA: 0x00043718 File Offset: 0x00041918
			protected virtual bool IsEqual(object op1, object op2)
			{
				if (op1 is double)
				{
					double num = (double)op1;
					if (op2 is double)
					{
						double num2 = (double)op2;
						return num.Equals(num2);
					}
				}
				return op1.Equals(op2);
			}
		}

		// Token: 0x020002A5 RID: 677
		internal abstract class AbstractNumberComparisonOperator : Operator
		{
			// Token: 0x06000ED4 RID: 3796 RVA: 0x00043760 File Offset: 0x00041960
			public void Execute(ExecutionContext context)
			{
				object obj = context.Stack.Pop();
				double num = Convert.ToDouble(context.Stack.Pop());
				double num2 = Convert.ToDouble(obj);
				bool flag = this.Compare(num, num2);
				context.Stack.Push(flag);
			}

			// Token: 0x06000ED5 RID: 3797
			protected abstract bool Compare(double num1, double num2);
		}

		// Token: 0x020002A6 RID: 678
		internal sealed class Ge : RelationalOperators.AbstractNumberComparisonOperator
		{
			// Token: 0x06000ED7 RID: 3799 RVA: 0x000437B1 File Offset: 0x000419B1
			protected override bool Compare(double num1, double num2)
			{
				return num1 >= num2;
			}
		}

		// Token: 0x020002A7 RID: 679
		internal sealed class Gt : RelationalOperators.AbstractNumberComparisonOperator
		{
			// Token: 0x06000ED9 RID: 3801 RVA: 0x000437C2 File Offset: 0x000419C2
			protected override bool Compare(double num1, double num2)
			{
				return num1 > num2;
			}
		}

		// Token: 0x020002A8 RID: 680
		internal sealed class Le : RelationalOperators.AbstractNumberComparisonOperator
		{
			// Token: 0x06000EDB RID: 3803 RVA: 0x000437D0 File Offset: 0x000419D0
			protected override bool Compare(double num1, double num2)
			{
				return num1 <= num2;
			}
		}

		// Token: 0x020002A9 RID: 681
		internal sealed class Lt : RelationalOperators.AbstractNumberComparisonOperator
		{
			// Token: 0x06000EDD RID: 3805 RVA: 0x000437E1 File Offset: 0x000419E1
			protected override bool Compare(double num1, double num2)
			{
				return num1 < num2;
			}
		}

		// Token: 0x020002AA RID: 682
		internal sealed class Ne : RelationalOperators.Eq
		{
			// Token: 0x06000EDF RID: 3807 RVA: 0x000437EF File Offset: 0x000419EF
			protected override bool IsEqual(object op1, object op2)
			{
				return !base.IsEqual(op1, op2);
			}
		}
	}
}
