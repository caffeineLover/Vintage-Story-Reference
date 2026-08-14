using System;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine
{
	// Token: 0x0200005C RID: 92
	[NullableContext(2)]
	public interface INode<TOutput, TIntermediate, TInput>
	{
		// Token: 0x060002B3 RID: 691
		[NullableContext(1)]
		TOutput Evaluate(IContext<TIntermediate, TInput> context);
	}
}
