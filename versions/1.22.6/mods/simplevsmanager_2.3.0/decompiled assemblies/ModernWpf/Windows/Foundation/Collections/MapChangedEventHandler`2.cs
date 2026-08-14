using System;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation.Collections
{
	// Token: 0x02000090 RID: 144
	// (Invoke) Token: 0x0600047D RID: 1149
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal delegate void MapChangedEventHandler<K, V>(IObservableMap<K, V> sender, IMapChangedEventArgs<K> @event);
}
