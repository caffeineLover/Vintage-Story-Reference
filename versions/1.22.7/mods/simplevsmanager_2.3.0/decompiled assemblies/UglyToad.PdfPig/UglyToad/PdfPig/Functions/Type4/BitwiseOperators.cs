using System;

namespace UglyToad.PdfPig.Functions.Type4
{
	// Token: 0x020001AE RID: 430
	internal sealed class BitwiseOperators
	{
		// Token: 0x06000A73 RID: 2675 RVA: 0x0003452B File Offset: 0x0003272B
		private BitwiseOperators()
		{
		}

		// Token: 0x02000296 RID: 662
		internal abstract class AbstractLogicalOperator : Operator
		{
			// Token: 0x06000EA4 RID: 3748 RVA: 0x000430B8 File Offset: 0x000412B8
			public void Execute(ExecutionContext context)
			{
				object obj = context.Stack.Pop();
				object obj2 = context.Stack.Pop();
				if (obj2 is bool)
				{
					bool @bool = (bool)obj2;
					if (obj is bool)
					{
						bool bool2 = (bool)obj;
						bool flag = this.ApplyForBoolean(@bool, bool2);
						context.Stack.Push(flag);
						return;
					}
				}
				if (obj2 is int)
				{
					int @int = (int)obj2;
					if (obj is int)
					{
						int int2 = (int)obj;
						int num = this.ApplyForInt(@int, int2);
						context.Stack.Push(num);
						return;
					}
				}
				throw new InvalidCastException("Operands must be bool/bool or int/int");
			}

			// Token: 0x06000EA5 RID: 3749
			protected abstract bool ApplyForBoolean(bool bool1, bool bool2);

			// Token: 0x06000EA6 RID: 3750
			protected abstract int ApplyForInt(int int1, int int2);
		}

		// Token: 0x02000297 RID: 663
		internal sealed class And : BitwiseOperators.AbstractLogicalOperator
		{
			// Token: 0x06000EA8 RID: 3752 RVA: 0x00043169 File Offset: 0x00041369
			protected override bool ApplyForBoolean(bool bool1, bool bool2)
			{
				return bool1 && bool2;
			}

			// Token: 0x06000EA9 RID: 3753 RVA: 0x0004316E File Offset: 0x0004136E
			protected override int ApplyForInt(int int1, int int2)
			{
				return int1 & int2;
			}
		}

		// Token: 0x02000298 RID: 664
		internal sealed class Bitshift : Operator
		{
			// Token: 0x06000EAB RID: 3755 RVA: 0x0004317C File Offset: 0x0004137C
			public void Execute(ExecutionContext context)
			{
				int num = Convert.ToInt32(context.Stack.Pop());
				int num2 = Convert.ToInt32(context.Stack.Pop());
				if (num < 0)
				{
					int num3 = num2 >> Math.Abs(num);
					context.Stack.Push(num3);
					return;
				}
				int num4 = num2 << num;
				context.Stack.Push(num4);
			}
		}

		// Token: 0x02000299 RID: 665
		internal sealed class False : Operator
		{
			// Token: 0x06000EAD RID: 3757 RVA: 0x000431ED File Offset: 0x000413ED
			public void Execute(ExecutionContext context)
			{
				context.Stack.Push(false);
			}
		}

		// Token: 0x0200029A RID: 666
		internal sealed class Not : Operator
		{
			// Token: 0x06000EAF RID: 3759 RVA: 0x00043208 File Offset: 0x00041408
			public void Execute(ExecutionContext context)
			{
				object obj = context.Stack.Pop();
				if (obj is bool)
				{
					bool flag = (bool)obj;
					bool flag2 = !flag;
					context.Stack.Push(flag2);
					return;
				}
				if (obj is int)
				{
					int num = (int)obj;
					int num2 = -num;
					context.Stack.Push(num2);
					return;
				}
				throw new InvalidCastException("Operand must be bool or int");
			}
		}

		// Token: 0x0200029B RID: 667
		internal sealed class Or : BitwiseOperators.AbstractLogicalOperator
		{
			// Token: 0x06000EB1 RID: 3761 RVA: 0x0004327F File Offset: 0x0004147F
			protected override bool ApplyForBoolean(bool bool1, bool bool2)
			{
				return bool1 || bool2;
			}

			// Token: 0x06000EB2 RID: 3762 RVA: 0x00043284 File Offset: 0x00041484
			protected override int ApplyForInt(int int1, int int2)
			{
				return int1 | int2;
			}
		}

		// Token: 0x0200029C RID: 668
		internal sealed class True : Operator
		{
			// Token: 0x06000EB4 RID: 3764 RVA: 0x00043291 File Offset: 0x00041491
			public void Execute(ExecutionContext context)
			{
				context.Stack.Push(true);
			}
		}

		// Token: 0x0200029D RID: 669
		internal sealed class Xor : BitwiseOperators.AbstractLogicalOperator
		{
			// Token: 0x06000EB6 RID: 3766 RVA: 0x000432AC File Offset: 0x000414AC
			protected override bool ApplyForBoolean(bool bool1, bool bool2)
			{
				return bool1 ^ bool2;
			}

			// Token: 0x06000EB7 RID: 3767 RVA: 0x000432B1 File Offset: 0x000414B1
			protected override int ApplyForInt(int int1, int int2)
			{
				return int1 ^ int2;
			}
		}
	}
}
