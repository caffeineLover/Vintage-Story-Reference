using System;
using System.Runtime.InteropServices;

namespace Windows.Foundation.Collections
{
	// Token: 0x02000087 RID: 135
	[Guid("E480CE40-A338-4ADA-ADCF-272272E48CB9")]
	internal interface IMapView<K, V> : IIterable<IKeyValuePair<K, V>>
	{
		// Token: 0x0600046A RID: 1130
		V Lookup(K key);

		// Token: 0x0600046B RID: 1131
		bool HasKey(K key);

		// Token: 0x0600046C RID: 1132
		void Split(out IMapView<K, V> first, out IMapView<K, V> second);

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x0600046D RID: 1133
		uint Size { get; }
	}
}
