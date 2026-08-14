using System;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine.Nodes
{
	// Token: 0x0200006A RID: 106
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Binary<[Nullable(2)] TOutput, [Nullable(2)] TIntermediate, [Nullable(2)] TInput> : INode<TOutput, TIntermediate, TInput>
	{
		// Token: 0x060002E8 RID: 744 RVA: 0x0000F622 File Offset: 0x0000D822
		public Binary(INode<TIntermediate, TIntermediate, TInput> leftOperand, INode<TIntermediate, TIntermediate, TInput> rightOperand, Func<TIntermediate, TIntermediate, TOutput> operation)
		{
			this.mLeftOperand = leftOperand;
			this.mRightOperand = rightOperand;
			this.mOperation = operation;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000F63F File Offset: 0x0000D83F
		public TOutput Evaluate(IContext<TIntermediate, TInput> context)
		{
			return this.mOperation.Invoke(this.mLeftOperand.Evaluate(context), this.mRightOperand.Evaluate(context));
		}

		// Token: 0x0400017C RID: 380
		private readonly INode<TIntermediate, TIntermediate, TInput> mLeftOperand;

		// Token: 0x0400017D RID: 381
		private readonly INode<TIntermediate, TIntermediate, TInput> mRightOperand;

		// Token: 0x0400017E RID: 382
		private readonly Func<TIntermediate, TIntermediate, TOutput> mOperation;
	}
}
