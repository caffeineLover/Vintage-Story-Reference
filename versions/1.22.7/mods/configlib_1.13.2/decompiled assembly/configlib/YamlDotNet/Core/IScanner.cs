using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core
{
	// Token: 0x020001B7 RID: 439
	[NullableContext(2)]
	public interface IScanner
	{
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000980 RID: 2432
		Mark CurrentPosition { get; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000981 RID: 2433
		Token Current { get; }

		// Token: 0x06000982 RID: 2434
		bool MoveNext();

		// Token: 0x06000983 RID: 2435
		bool MoveNextWithoutConsuming();

		// Token: 0x06000984 RID: 2436
		void ConsumeCurrent();
	}
}
