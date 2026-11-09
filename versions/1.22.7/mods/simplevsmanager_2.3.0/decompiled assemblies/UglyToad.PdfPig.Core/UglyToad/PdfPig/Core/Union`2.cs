using System;
using System.Diagnostics;

namespace UglyToad.PdfPig.Core
{
	// Token: 0x02000015 RID: 21
	public abstract class Union<A, B>
	{
		// Token: 0x060000A6 RID: 166
		public abstract void Match(Action<A> first, Action<B> second);

		// Token: 0x060000A7 RID: 167
		public abstract TResult Match<TResult>(Func<A, TResult> first, Func<B, TResult> second);

		// Token: 0x060000A8 RID: 168
		public abstract bool TryGetFirst(out A a);

		// Token: 0x060000A9 RID: 169
		public abstract bool TryGetSecond(out B b);

		// Token: 0x060000AA RID: 170 RVA: 0x000050C8 File Offset: 0x000032C8
		private Union()
		{
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000050D0 File Offset: 0x000032D0
		public static Union<A, B>.Case1 One(A item)
		{
			return new Union<A, B>.Case1(item);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000050D8 File Offset: 0x000032D8
		public static Union<A, B>.Case2 Two(B item)
		{
			return new Union<A, B>.Case2(item);
		}

		// Token: 0x02000021 RID: 33
		public sealed class Case1 : Union<A, B>
		{
			// Token: 0x060000F2 RID: 242 RVA: 0x00006A27 File Offset: 0x00004C27
			public Case1(A item)
			{
				this.Item = item;
			}

			// Token: 0x060000F3 RID: 243 RVA: 0x00006A36 File Offset: 0x00004C36
			[DebuggerStepThrough]
			public override void Match(Action<A> first, Action<B> second)
			{
				first.Invoke(this.Item);
			}

			// Token: 0x060000F4 RID: 244 RVA: 0x00006A44 File Offset: 0x00004C44
			[DebuggerStepThrough]
			public override TResult Match<TResult>(Func<A, TResult> first, Func<B, TResult> second)
			{
				return first.Invoke(this.Item);
			}

			// Token: 0x060000F5 RID: 245 RVA: 0x00006A52 File Offset: 0x00004C52
			public override bool TryGetFirst(out A a)
			{
				a = this.Item;
				return true;
			}

			// Token: 0x060000F6 RID: 246 RVA: 0x00006A61 File Offset: 0x00004C61
			public override bool TryGetSecond(out B b)
			{
				b = default(B);
				return false;
			}

			// Token: 0x060000F7 RID: 247 RVA: 0x00006A6C File Offset: 0x00004C6C
			public override string ToString()
			{
				A item = this.Item;
				return ((item != null) ? item.ToString() : null) ?? string.Empty;
			}

			// Token: 0x0400005B RID: 91
			public readonly A Item;
		}

		// Token: 0x02000022 RID: 34
		public sealed class Case2 : Union<A, B>
		{
			// Token: 0x060000F8 RID: 248 RVA: 0x00006AA7 File Offset: 0x00004CA7
			public Case2(B item)
			{
				this.Item = item;
			}

			// Token: 0x060000F9 RID: 249 RVA: 0x00006AB6 File Offset: 0x00004CB6
			[DebuggerStepThrough]
			public override void Match(Action<A> first, Action<B> second)
			{
				second.Invoke(this.Item);
			}

			// Token: 0x060000FA RID: 250 RVA: 0x00006AC4 File Offset: 0x00004CC4
			[DebuggerStepThrough]
			public override TResult Match<TResult>(Func<A, TResult> first, Func<B, TResult> second)
			{
				return second.Invoke(this.Item);
			}

			// Token: 0x060000FB RID: 251 RVA: 0x00006AD2 File Offset: 0x00004CD2
			public override bool TryGetFirst(out A a)
			{
				a = default(A);
				return false;
			}

			// Token: 0x060000FC RID: 252 RVA: 0x00006ADC File Offset: 0x00004CDC
			public override bool TryGetSecond(out B b)
			{
				b = this.Item;
				return true;
			}

			// Token: 0x060000FD RID: 253 RVA: 0x00006AEC File Offset: 0x00004CEC
			public override string ToString()
			{
				B item = this.Item;
				return ((item != null) ? item.ToString() : null) ?? string.Empty;
			}

			// Token: 0x0400005C RID: 92
			public readonly B Item;
		}
	}
}
