using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Windows.Foundation.Collections
{
	// Token: 0x02000083 RID: 131
	[Guid("3C2925FE-8519-45C1-AA79-197B6718C1C1")]
	internal interface IMap<K, V> : IIterable<IKeyValuePair<K, V>>
	{
		// Token: 0x06000452 RID: 1106
		V Lookup(K key);

		// Token: 0x06000453 RID: 1107
		bool HasKey(K key);

		// Token: 0x06000454 RID: 1108
		IReadOnlyDictionary<K, V> GetView();

		// Token: 0x06000455 RID: 1109
		bool Insert(K key, V value);

		// Token: 0x06000456 RID: 1110
		void _Remove(K key);

		// Token: 0x06000457 RID: 1111
		void Clear();

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000458 RID: 1112
		uint Size { get; }
	}
}
