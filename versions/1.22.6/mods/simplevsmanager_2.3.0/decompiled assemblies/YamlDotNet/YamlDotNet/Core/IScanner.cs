using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core
{
	// Token: 0x020000CE RID: 206
	[NullableContext(2)]
	public interface IScanner
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600051D RID: 1309
		Mark CurrentPosition { get; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600051E RID: 1310
		Token Current { get; }

		// Token: 0x0600051F RID: 1311
		bool MoveNext();

		// Token: 0x06000520 RID: 1312
		bool MoveNextWithoutConsuming();

		// Token: 0x06000521 RID: 1313
		void ConsumeCurrent();
	}
}
