using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine.Nodes
{
	// Token: 0x0200006B RID: 107
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class FunctionCall<[Nullable(2)] TOutput, [Nullable(2)] TInput> : INode<TOutput, TOutput, TInput>
	{
		// Token: 0x060002EA RID: 746 RVA: 0x0000F664 File Offset: 0x0000D864
		public FunctionCall(string functionName, INode<TInput, TOutput, TInput>[] arguments)
		{
			this.mFunctionName = functionName;
			this.mArguments = arguments;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000F67C File Offset: 0x0000D87C
		public TOutput Evaluate(IContext<TOutput, TInput> context)
		{
			FunctionCall<TOutput, TInput>.<>c__DisplayClass3_0 CS$<>8__locals1 = new FunctionCall<TOutput, TInput>.<>c__DisplayClass3_0();
			CS$<>8__locals1.context = context;
			return CS$<>8__locals1.context.Resolve(this.mFunctionName, Enumerable.ToArray<TInput>(Enumerable.Select<INode<TInput, TOutput, TInput>, TInput>(this.mArguments, new Func<INode<TInput, TOutput, TInput>, TInput>(CS$<>8__locals1.<Evaluate>b__0))));
		}

		// Token: 0x0400017F RID: 383
		private readonly INode<TInput, TOutput, TInput>[] mArguments;

		// Token: 0x04000180 RID: 384
		private readonly string mFunctionName;
	}
}
