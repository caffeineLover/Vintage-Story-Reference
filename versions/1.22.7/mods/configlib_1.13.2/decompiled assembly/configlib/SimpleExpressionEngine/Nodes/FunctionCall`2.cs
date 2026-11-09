using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine.Nodes
{
	// Token: 0x02000067 RID: 103
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class FunctionCall<[Nullable(2)] TOutput, [Nullable(2)] TInput> : INode<TOutput, TOutput, TInput>
	{
		// Token: 0x060002AE RID: 686 RVA: 0x0000ED1C File Offset: 0x0000CF1C
		public FunctionCall(string functionName, INode<TInput, TOutput, TInput>[] arguments)
		{
			this.mFunctionName = functionName;
			this.mArguments = arguments;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000ED34 File Offset: 0x0000CF34
		public TOutput Evaluate(IContext<TOutput, TInput> context)
		{
			FunctionCall<TOutput, TInput>.<>c__DisplayClass3_0 CS$<>8__locals1 = new FunctionCall<TOutput, TInput>.<>c__DisplayClass3_0();
			CS$<>8__locals1.context = context;
			return CS$<>8__locals1.context.Resolve(this.mFunctionName, Enumerable.ToArray<TInput>(Enumerable.Select<INode<TInput, TOutput, TInput>, TInput>(this.mArguments, new Func<INode<TInput, TOutput, TInput>, TInput>(CS$<>8__locals1.<Evaluate>b__0))));
		}

		// Token: 0x04000151 RID: 337
		private readonly INode<TInput, TOutput, TInput>[] mArguments;

		// Token: 0x04000152 RID: 338
		private readonly string mFunctionName;
	}
}
