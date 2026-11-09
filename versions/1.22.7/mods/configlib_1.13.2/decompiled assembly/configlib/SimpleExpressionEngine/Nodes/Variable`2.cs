using System;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine.Nodes
{
	// Token: 0x0200006B RID: 107
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Variable<[Nullable(2)] TOutput, [Nullable(2)] TInput> : INode<TOutput, TOutput, TInput>
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x0000EDC9 File Offset: 0x0000CFC9
		public Variable(string variableName)
		{
			this.mVariableName = variableName;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000EDD8 File Offset: 0x0000CFD8
		public TOutput Evaluate(IContext<TOutput, TInput> context)
		{
			return context.Resolve(this.mVariableName, Array.Empty<TInput>());
		}

		// Token: 0x04000157 RID: 343
		private readonly string mVariableName;
	}
}
