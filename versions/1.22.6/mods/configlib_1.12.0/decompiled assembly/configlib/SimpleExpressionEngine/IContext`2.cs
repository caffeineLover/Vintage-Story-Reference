using System;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine
{
	// Token: 0x0200005B RID: 91
	[NullableContext(1)]
	public interface IContext<[Nullable(2)] out TResult, [Nullable(2)] in TArguments>
	{
		// Token: 0x060002B1 RID: 689
		TResult Resolve(string name, params TArguments[] arguments);

		// Token: 0x060002B2 RID: 690
		bool Resolvable(string name);
	}
}
