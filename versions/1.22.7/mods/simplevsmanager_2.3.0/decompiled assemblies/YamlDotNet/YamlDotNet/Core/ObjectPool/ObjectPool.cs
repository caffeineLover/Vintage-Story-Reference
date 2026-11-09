using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.ObjectPool
{
	// Token: 0x020000FE RID: 254
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ObjectPool
	{
		// Token: 0x0600063B RID: 1595 RVA: 0x000173EC File Offset: 0x000155EC
		public static ObjectPool<T> Create<T>([Nullable(new byte[]
		{
			2,
			1
		})] IPooledObjectPolicy<T> policy = null) where T : class, new()
		{
			return new DefaultObjectPool<T>(policy ?? new DefaultPooledObjectPolicy<T>());
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x000173FD File Offset: 0x000155FD
		public static ObjectPool<T> Create<T>(int maximumRetained, [Nullable(new byte[]
		{
			2,
			1
		})] IPooledObjectPolicy<T> policy = null) where T : class, new()
		{
			return new DefaultObjectPool<T>(policy ?? new DefaultPooledObjectPolicy<T>(), maximumRetained);
		}
	}
}
