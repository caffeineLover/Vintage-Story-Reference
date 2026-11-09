using System;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine
{
	// Token: 0x02000057 RID: 87
	[NullableContext(1)]
	public interface IContext<[Nullable(2)] out TResult, [Nullable(2)] in TArguments>
	{
		// Token: 0x06000275 RID: 629
		TResult Resolve(string name, params TArguments[] arguments);

		// Token: 0x06000276 RID: 630
		bool Resolvable(string name);
	}
}
