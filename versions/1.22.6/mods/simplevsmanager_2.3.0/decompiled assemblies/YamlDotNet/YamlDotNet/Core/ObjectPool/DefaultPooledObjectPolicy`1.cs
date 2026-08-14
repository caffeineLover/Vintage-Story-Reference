using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.ObjectPool
{
	// Token: 0x020000FA RID: 250
	[NullableContext(1)]
	[Nullable(0)]
	internal class DefaultPooledObjectPolicy<T> : IPooledObjectPolicy<T> where T : class, new()
	{
		// Token: 0x06000632 RID: 1586 RVA: 0x000173AE File Offset: 0x000155AE
		public T Create()
		{
			return Activator.CreateInstance<T>();
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x000173B8 File Offset: 0x000155B8
		public bool Return(T obj)
		{
			IResettable resettable = obj as IResettable;
			return resettable == null || resettable.TryReset();
		}
	}
}
