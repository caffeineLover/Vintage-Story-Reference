using System;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine.Nodes
{
	// Token: 0x02000066 RID: 102
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Binary<[Nullable(2)] TOutput, [Nullable(2)] TIntermediate, [Nullable(2)] TInput> : INode<TOutput, TIntermediate, TInput>
	{
		// Token: 0x060002AC RID: 684 RVA: 0x0000ECDA File Offset: 0x0000CEDA
		public Binary(INode<TIntermediate, TIntermediate, TInput> leftOperand, INode<TIntermediate, TIntermediate, TInput> rightOperand, Func<TIntermediate, TIntermediate, TOutput> operation)
		{
			this.mLeftOperand = leftOperand;
			this.mRightOperand = rightOperand;
			this.mOperation = operation;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000ECF7 File Offset: 0x0000CEF7
		public TOutput Evaluate(IContext<TIntermediate, TInput> context)
		{
			return this.mOperation.Invoke(this.mLeftOperand.Evaluate(context), this.mRightOperand.Evaluate(context));
		}

		// Token: 0x0400014E RID: 334
		private readonly INode<TIntermediate, TIntermediate, TInput> mLeftOperand;

		// Token: 0x0400014F RID: 335
		private readonly INode<TIntermediate, TIntermediate, TInput> mRightOperand;

		// Token: 0x04000150 RID: 336
		private readonly Func<TIntermediate, TIntermediate, TOutput> mOperation;
	}
}
