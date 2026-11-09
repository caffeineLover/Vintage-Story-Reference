using System;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine
{
	// Token: 0x02000058 RID: 88
	[NullableContext(2)]
	public interface INode<TOutput, TIntermediate, TInput>
	{
		// Token: 0x06000277 RID: 631
		[NullableContext(1)]
		TOutput Evaluate(IContext<TIntermediate, TInput> context);
	}
}
