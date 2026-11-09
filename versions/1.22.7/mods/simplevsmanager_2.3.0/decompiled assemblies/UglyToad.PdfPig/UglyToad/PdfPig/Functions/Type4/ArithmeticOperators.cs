using System;

namespace UglyToad.PdfPig.Functions.Type4
{
	// Token: 0x020001AD RID: 429
	internal sealed class ArithmeticOperators
	{
		// Token: 0x06000A70 RID: 2672 RVA: 0x00034509 File Offset: 0x00032709
		private ArithmeticOperators()
		{
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00034511 File Offset: 0x00032711
		private static double ToRadians(double val)
		{
			return 0.017453292519943295 * val;
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x0003451E File Offset: 0x0003271E
		private static double ToDegrees(double val)
		{
			return 57.29577951308232 * val;
		}

		// Token: 0x02000281 RID: 641
		internal sealed class Abs : Operator
		{
			// Token: 0x06000E7A RID: 3706 RVA: 0x0004297C File Offset: 0x00040B7C
			public void Execute(ExecutionContext context)
			{
				object obj = context.PopNumber();
				if (obj is int)
				{
					int num = (int)obj;
					context.Stack.Push(Math.Abs(num));
					return;
				}
				context.Stack.Push(Math.Abs(Convert.ToDouble(obj)));
			}
		}

		// Token: 0x02000282 RID: 642
		internal sealed class Add : Operator
		{
			// Token: 0x06000E7C RID: 3708 RVA: 0x000429DC File Offset: 0x00040BDC
			public void Execute(ExecutionContext context)
			{
				object obj = context.PopNumber();
				object obj2 = context.PopNumber();
				if (obj2 is int)
				{
					int num = (int)obj2;
					if (obj is int)
					{
						int num2 = (int)obj;
						long num3 = (long)num + (long)num2;
						if (num3 < -2147483648L || num3 > 2147483647L)
						{
							context.Stack.Push((double)num3);
							return;
						}
						context.Stack.Push((int)num3);
						return;
					}
				}
				double num4 = Convert.ToDouble(obj2) + Convert.ToDouble(obj);
				context.Stack.Push(num4);
			}
		}

		// Token: 0x02000283 RID: 643
		internal sealed class Atan : Operator
		{
			// Token: 0x06000E7E RID: 3710 RVA: 0x00042A84 File Offset: 0x00040C84
			public void Execute(ExecutionContext context)
			{
				double num = context.PopReal();
				double num2 = Math.Atan2(context.PopReal(), num);
				num2 = ArithmeticOperators.ToDegrees(num2) % 360.0;
				if (num2 < 0.0)
				{
					num2 += 360.0;
				}
				context.Stack.Push(num2);
			}
		}

		// Token: 0x02000284 RID: 644
		internal sealed class Ceiling : Operator
		{
			// Token: 0x06000E80 RID: 3712 RVA: 0x00042AE8 File Offset: 0x00040CE8
			public void Execute(ExecutionContext context)
			{
				object obj = context.PopNumber();
				if (obj is int)
				{
					int num = (int)obj;
					context.Stack.Push(num);
					return;
				}
				context.Stack.Push(Math.Ceiling(Convert.ToDouble(obj)));
			}
		}

		// Token: 0x02000285 RID: 645
		internal sealed class Cos : Operator
		{
			// Token: 0x06000E82 RID: 3714 RVA: 0x00042B40 File Offset: 0x00040D40
			public void Execute(ExecutionContext context)
			{
				double num = Math.Cos(ArithmeticOperators.ToRadians(context.PopReal()));
				context.Stack.Push(num);
			}
		}

		// Token: 0x02000286 RID: 646
		internal sealed class Cvi : Operator
		{
			// Token: 0x06000E84 RID: 3716 RVA: 0x00042B78 File Offset: 0x00040D78
			public void Execute(ExecutionContext context)
			{
				object obj = context.PopNumber();
				context.Stack.Push((int)Math.Truncate(Convert.ToDouble(obj)));
			}
		}

		// Token: 0x02000287 RID: 647
		internal sealed class Cvr : Operator
		{
			// Token: 0x06000E86 RID: 3718 RVA: 0x00042BB0 File Offset: 0x00040DB0
			public void Execute(ExecutionContext context)
			{
				object obj = context.PopNumber();
				context.Stack.Push(Convert.ToDouble(obj));
			}
		}

		// Token: 0x02000288 RID: 648
		internal sealed class Div : Operator
		{
			// Token: 0x06000E88 RID: 3720 RVA: 0x00042BE4 File Offset: 0x00040DE4
			public void Execute(ExecutionContext context)
			{
				double num = Convert.ToDouble(context.PopNumber());
				double num2 = Convert.ToDouble(context.PopNumber());
				context.Stack.Push(num2 / num);
			}
		}

		// Token: 0x02000289 RID: 649
		internal sealed class Exp : Operator
		{
			// Token: 0x06000E8A RID: 3722 RVA: 0x00042C24 File Offset: 0x00040E24
			public void Execute(ExecutionContext context)
			{
				double num = Convert.ToDouble(context.PopNumber());
				double num2 = Math.Pow(Convert.ToDouble(context.PopNumber()), num);
				context.Stack.Push(num2);
			}
		}

		// Token: 0x0200028A RID: 650
		internal sealed class Floor : Operator
		{
			// Token: 0x06000E8C RID: 3724 RVA: 0x00042C68 File Offset: 0x00040E68
			public void Execute(ExecutionContext context)
			{
				object obj = context.PopNumber();
				if (obj is int)
				{
					int num = (int)obj;
					context.Stack.Push(num);
					return;
				}
				context.Stack.Push(Math.Floor(Convert.ToDouble(obj)));
			}
		}

		// Token: 0x0200028B RID: 651
		internal sealed class IDiv : Operator
		{
			// Token: 0x06000E8E RID: 3726 RVA: 0x00042CC0 File Offset: 0x00040EC0
			public void Execute(ExecutionContext context)
			{
				int num = context.PopInt();
				int num2 = context.PopInt();
				context.Stack.Push(num2 / num);
			}
		}

		// Token: 0x0200028C RID: 652
		internal sealed class Ln : Operator
		{
			// Token: 0x06000E90 RID: 3728 RVA: 0x00042CF8 File Offset: 0x00040EF8
			public void Execute(ExecutionContext context)
			{
				object obj = context.PopNumber();
				context.Stack.Push(Math.Log(Convert.ToDouble(obj)));
			}
		}

		// Token: 0x0200028D RID: 653
		internal sealed class Log : Operator
		{
			// Token: 0x06000E92 RID: 3730 RVA: 0x00042D30 File Offset: 0x00040F30
			public void Execute(ExecutionContext context)
			{
				object obj = context.PopNumber();
				context.Stack.Push(Math.Log10(Convert.ToDouble(obj)));
			}
		}

		// Token: 0x0200028E RID: 654
		internal sealed class Mod : Operator
		{
			// Token: 0x06000E94 RID: 3732 RVA: 0x00042D68 File Offset: 0x00040F68
			public void Execute(ExecutionContext context)
			{
				int num = context.PopInt();
				int num2 = context.PopInt();
				context.Stack.Push(num2 % num);
			}
		}

		// Token: 0x0200028F RID: 655
		internal sealed class Mul : Operator
		{
			// Token: 0x06000E96 RID: 3734 RVA: 0x00042DA0 File Offset: 0x00040FA0
			public void Execute(ExecutionContext context)
			{
				object obj = context.PopNumber();
				object obj2 = context.PopNumber();
				if (obj2 is int)
				{
					int num = (int)obj2;
					if (obj is int)
					{
						int num2 = (int)obj;
						long num3 = (long)num * (long)num2;
						if (num3 >= -2147483648L && num3 <= 2147483647L)
						{
							context.Stack.Push((int)num3);
							return;
						}
						context.Stack.Push((double)num3);
						return;
					}
				}
				double num4 = Convert.ToDouble(obj2) * Convert.ToDouble(obj);
				context.Stack.Push(num4);
			}
		}

		// Token: 0x02000290 RID: 656
		internal sealed class Neg : Operator
		{
			// Token: 0x06000E98 RID: 3736 RVA: 0x00042E48 File Offset: 0x00041048
			public void Execute(ExecutionContext context)
			{
				object obj = context.PopNumber();
				if (!(obj is int))
				{
					context.Stack.Push(-Convert.ToDouble(obj));
					return;
				}
				int num = (int)obj;
				if (num == -2147483648)
				{
					context.Stack.Push(-Convert.ToDouble(num));
					return;
				}
				context.Stack.Push(-num);
			}
		}

		// Token: 0x02000291 RID: 657
		internal sealed class Round : Operator
		{
			// Token: 0x06000E9A RID: 3738 RVA: 0x00042EC0 File Offset: 0x000410C0
			public void Execute(ExecutionContext context)
			{
				object obj = context.PopNumber();
				if (obj is int)
				{
					int num = (int)obj;
					context.Stack.Push(num);
					return;
				}
				double num2 = Convert.ToDouble(obj);
				double num3 = (num2 < 0.0) ? Math.Round(num2) : Math.Round(num2, 1);
				context.Stack.Push(num3);
			}
		}

		// Token: 0x02000292 RID: 658
		internal sealed class Sin : Operator
		{
			// Token: 0x06000E9C RID: 3740 RVA: 0x00042F34 File Offset: 0x00041134
			public void Execute(ExecutionContext context)
			{
				double num = Math.Sin(ArithmeticOperators.ToRadians(context.PopReal()));
				context.Stack.Push(num);
			}
		}

		// Token: 0x02000293 RID: 659
		internal sealed class Sqrt : Operator
		{
			// Token: 0x06000E9E RID: 3742 RVA: 0x00042F6C File Offset: 0x0004116C
			public void Execute(ExecutionContext context)
			{
				double num = context.PopReal();
				if (num < 0.0)
				{
					throw new ArgumentException("argument must be nonnegative");
				}
				context.Stack.Push(Math.Sqrt(num));
			}
		}

		// Token: 0x02000294 RID: 660
		internal sealed class Sub : Operator
		{
			// Token: 0x06000EA0 RID: 3744 RVA: 0x00042FB8 File Offset: 0x000411B8
			public void Execute(ExecutionContext context)
			{
				object obj = context.PopNumber();
				object obj2 = context.PopNumber();
				if (obj2 is int)
				{
					int num = (int)obj2;
					if (obj is int)
					{
						int num2 = (int)obj;
						long num3 = (long)num - (long)num2;
						if (num3 < -2147483648L || num3 > 2147483647L)
						{
							context.Stack.Push((double)num3);
							return;
						}
						context.Stack.Push((int)num3);
						return;
					}
				}
				double num4 = Convert.ToDouble(obj2) - Convert.ToDouble(obj);
				context.Stack.Push(num4);
			}
		}

		// Token: 0x02000295 RID: 661
		internal sealed class Truncate : Operator
		{
			// Token: 0x06000EA2 RID: 3746 RVA: 0x00043060 File Offset: 0x00041260
			public void Execute(ExecutionContext context)
			{
				object obj = context.PopNumber();
				if (obj is int)
				{
					int num = (int)obj;
					context.Stack.Push(num);
					return;
				}
				context.Stack.Push(Math.Truncate(Convert.ToDouble(obj)));
			}
		}
	}
}
