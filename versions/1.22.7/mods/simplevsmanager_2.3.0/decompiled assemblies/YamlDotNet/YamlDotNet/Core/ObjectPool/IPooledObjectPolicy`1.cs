using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.ObjectPool
{
	// Token: 0x020000FB RID: 251
	[NullableContext(1)]
	internal interface IPooledObjectPolicy<T>
	{
		// Token: 0x06000635 RID: 1589
		T Create();

		// Token: 0x06000636 RID: 1590
		bool Return(T obj);
	}
}
