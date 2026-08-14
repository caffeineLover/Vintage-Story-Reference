using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core
{
	// Token: 0x020001BB RID: 443
	[NullableContext(2)]
	public interface IScanner
	{
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060009BC RID: 2492
		Mark CurrentPosition { get; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060009BD RID: 2493
		Token Current { get; }

		// Token: 0x060009BE RID: 2494
		bool MoveNext();

		// Token: 0x060009BF RID: 2495
		bool MoveNextWithoutConsuming();

		// Token: 0x060009C0 RID: 2496
		void ConsumeCurrent();
	}
}
