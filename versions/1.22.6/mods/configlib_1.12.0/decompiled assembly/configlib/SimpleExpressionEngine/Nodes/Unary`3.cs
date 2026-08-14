using System;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine.Nodes
{
	// Token: 0x0200006E RID: 110
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Unary<[Nullable(2)] TOutput, [Nullable(2)] TIntermediate, [Nullable(2)] TInput> : INode<TOutput, TIntermediate, TInput>
	{
		// Token: 0x060002F0 RID: 752 RVA: 0x0000F6E2 File Offset: 0x0000D8E2
		public Unary(INode<TIntermediate, TIntermediate, TInput> operand, Func<TIntermediate, TOutput> operation)
		{
			this.mOperand = operand;
			this.mOperation = operation;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000F6F8 File Offset: 0x0000D8F8
		public TOutput Evaluate(IContext<TIntermediate, TInput> context)
		{
			return this.mOperation.Invoke(this.mOperand.Evaluate(context));
		}

		// Token: 0x04000183 RID: 387
		private readonly INode<TIntermediate, TIntermediate, TInput> mOperand;

		// Token: 0x04000184 RID: 388
		private readonly Func<TIntermediate, TOutput> mOperation;
	}
}
