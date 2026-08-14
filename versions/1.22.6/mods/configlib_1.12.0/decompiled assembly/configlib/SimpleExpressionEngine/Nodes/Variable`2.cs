using System;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine.Nodes
{
	// Token: 0x0200006F RID: 111
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Variable<[Nullable(2)] TOutput, [Nullable(2)] TInput> : INode<TOutput, TOutput, TInput>
	{
		// Token: 0x060002F2 RID: 754 RVA: 0x0000F711 File Offset: 0x0000D911
		public Variable(string variableName)
		{
			this.mVariableName = variableName;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000F720 File Offset: 0x0000D920
		public TOutput Evaluate(IContext<TOutput, TInput> context)
		{
			return context.Resolve(this.mVariableName, Array.Empty<TInput>());
		}

		// Token: 0x04000185 RID: 389
		private readonly string mVariableName;
	}
}
