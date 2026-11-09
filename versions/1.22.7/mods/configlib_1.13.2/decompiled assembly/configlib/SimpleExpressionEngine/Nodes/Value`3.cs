using System;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine.Nodes
{
	// Token: 0x02000069 RID: 105
	[NullableContext(1)]
	[Nullable(0)]
	internal readonly struct Value<[Nullable(2)] TOutput, [Nullable(2)] TIntermediate, [Nullable(2)] TInput> : INode<TOutput, TIntermediate, TInput>
	{
		// Token: 0x060002B2 RID: 690 RVA: 0x0000ED89 File Offset: 0x0000CF89
		public Value(TOutput number)
		{
			this.mValue = number;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000ED92 File Offset: 0x0000CF92
		public TOutput Evaluate(IContext<TIntermediate, TInput> context)
		{
			return this.mValue;
		}

		// Token: 0x04000154 RID: 340
		private readonly TOutput mValue;
	}
}
