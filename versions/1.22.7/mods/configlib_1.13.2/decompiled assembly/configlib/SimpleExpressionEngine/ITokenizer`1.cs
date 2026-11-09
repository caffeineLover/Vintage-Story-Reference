using System;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine
{
	// Token: 0x02000059 RID: 89
	[NullableContext(1)]
	public interface ITokenizer<[Nullable(2)] out TValue>
	{
		// Token: 0x06000278 RID: 632
		void NextToken();

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000279 RID: 633
		Token Token { get; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600027A RID: 634
		TValue Value { get; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600027B RID: 635
		string Identifier { get; }
	}
}
