using System;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine.Nodes
{
	// Token: 0x02000065 RID: 101
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Ternary<[Nullable(2)] TOutput, [Nullable(2)] TIntermediate, [Nullable(2)] TInput> : INode<TOutput, TIntermediate, TInput>
	{
		// Token: 0x060002AA RID: 682 RVA: 0x0000EC84 File Offset: 0x0000CE84
		public Ternary(INode<TIntermediate, TIntermediate, TInput> firstOperand, INode<TIntermediate, TIntermediate, TInput> secondOperand, INode<TIntermediate, TIntermediate, TInput> thirdOperand, Func<TIntermediate, TIntermediate, TIntermediate, TOutput> operation)
		{
			this.mFirstOperand = firstOperand;
			this.mSecondOperand = secondOperand;
			this.mThirdOperand = thirdOperand;
			this.mOperation = operation;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000ECA9 File Offset: 0x0000CEA9
		public TOutput Evaluate(IContext<TIntermediate, TInput> context)
		{
			return this.mOperation.Invoke(this.mFirstOperand.Evaluate(context), this.mSecondOperand.Evaluate(context), this.mThirdOperand.Evaluate(context));
		}

		// Token: 0x0400014A RID: 330
		private readonly INode<TIntermediate, TIntermediate, TInput> mFirstOperand;

		// Token: 0x0400014B RID: 331
		private readonly INode<TIntermediate, TIntermediate, TInput> mSecondOperand;

		// Token: 0x0400014C RID: 332
		private readonly INode<TIntermediate, TIntermediate, TInput> mThirdOperand;

		// Token: 0x0400014D RID: 333
		private readonly Func<TIntermediate, TIntermediate, TIntermediate, TOutput> mOperation;
	}
}
