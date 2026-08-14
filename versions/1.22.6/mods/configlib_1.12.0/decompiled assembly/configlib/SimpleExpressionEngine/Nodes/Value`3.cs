using System;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine.Nodes
{
	// Token: 0x0200006D RID: 109
	[NullableContext(1)]
	[Nullable(0)]
	internal readonly struct Value<[Nullable(2)] TOutput, [Nullable(2)] TIntermediate, [Nullable(2)] TInput> : INode<TOutput, TIntermediate, TInput>
	{
		// Token: 0x060002EE RID: 750 RVA: 0x0000F6D1 File Offset: 0x0000D8D1
		public Value(TOutput number)
		{
			this.mValue = number;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000F6DA File Offset: 0x0000D8DA
		public TOutput Evaluate(IContext<TIntermediate, TInput> context)
		{
			return this.mValue;
		}

		// Token: 0x04000182 RID: 386
		private readonly TOutput mValue;
	}
}
