using System;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine.Nodes
{
	// Token: 0x0200006A RID: 106
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Unary<[Nullable(2)] TOutput, [Nullable(2)] TIntermediate, [Nullable(2)] TInput> : INode<TOutput, TIntermediate, TInput>
	{
		// Token: 0x060002B4 RID: 692 RVA: 0x0000ED9A File Offset: 0x0000CF9A
		public Unary(INode<TIntermediate, TIntermediate, TInput> operand, Func<TIntermediate, TOutput> operation)
		{
			this.mOperand = operand;
			this.mOperation = operation;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000EDB0 File Offset: 0x0000CFB0
		public TOutput Evaluate(IContext<TIntermediate, TInput> context)
		{
			return this.mOperation.Invoke(this.mOperand.Evaluate(context));
		}

		// Token: 0x04000155 RID: 341
		private readonly INode<TIntermediate, TIntermediate, TInput> mOperand;

		// Token: 0x04000156 RID: 342
		private readonly Func<TIntermediate, TOutput> mOperation;
	}
}
