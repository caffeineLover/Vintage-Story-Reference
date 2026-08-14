using System;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine.Nodes
{
	// Token: 0x02000069 RID: 105
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Ternary<[Nullable(2)] TOutput, [Nullable(2)] TIntermediate, [Nullable(2)] TInput> : INode<TOutput, TIntermediate, TInput>
	{
		// Token: 0x060002E6 RID: 742 RVA: 0x0000F5CC File Offset: 0x0000D7CC
		public Ternary(INode<TIntermediate, TIntermediate, TInput> firstOperand, INode<TIntermediate, TIntermediate, TInput> secondOperand, INode<TIntermediate, TIntermediate, TInput> thirdOperand, Func<TIntermediate, TIntermediate, TIntermediate, TOutput> operation)
		{
			this.mFirstOperand = firstOperand;
			this.mSecondOperand = secondOperand;
			this.mThirdOperand = thirdOperand;
			this.mOperation = operation;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000F5F1 File Offset: 0x0000D7F1
		public TOutput Evaluate(IContext<TIntermediate, TInput> context)
		{
			return this.mOperation.Invoke(this.mFirstOperand.Evaluate(context), this.mSecondOperand.Evaluate(context), this.mThirdOperand.Evaluate(context));
		}

		// Token: 0x04000178 RID: 376
		private readonly INode<TIntermediate, TIntermediate, TInput> mFirstOperand;

		// Token: 0x04000179 RID: 377
		private readonly INode<TIntermediate, TIntermediate, TInput> mSecondOperand;

		// Token: 0x0400017A RID: 378
		private readonly INode<TIntermediate, TIntermediate, TInput> mThirdOperand;

		// Token: 0x0400017B RID: 379
		private readonly Func<TIntermediate, TIntermediate, TIntermediate, TOutput> mOperation;
	}
}
