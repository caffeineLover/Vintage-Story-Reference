using System;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine
{
	// Token: 0x0200005D RID: 93
	[NullableContext(1)]
	public interface ITokenizer<[Nullable(2)] out TValue>
	{
		// Token: 0x060002B4 RID: 692
		void NextToken();

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060002B5 RID: 693
		Token Token { get; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060002B6 RID: 694
		TValue Value { get; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060002B7 RID: 695
		string Identifier { get; }
	}
}
