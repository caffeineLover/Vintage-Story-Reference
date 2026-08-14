using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.ObjectPool
{
	// Token: 0x020000FD RID: 253
	[NullableContext(1)]
	[Nullable(0)]
	internal abstract class ObjectPool<T> where T : class
	{
		// Token: 0x06000638 RID: 1592
		public abstract T Get();

		// Token: 0x06000639 RID: 1593
		public abstract void Return(T obj);
	}
}
