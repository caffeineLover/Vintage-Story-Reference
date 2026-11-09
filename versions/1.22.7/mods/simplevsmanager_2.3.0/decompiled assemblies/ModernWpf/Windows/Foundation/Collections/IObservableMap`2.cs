using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation.Collections
{
	// Token: 0x0200008C RID: 140
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[Guid("65DF2BF5-BF39-41B5-AEBC-5A9D865E472B")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal interface IObservableMap<K, V> : IDictionary<K, V>, ICollection<KeyValuePair<K, V>>, IEnumerable<KeyValuePair<K, V>>, IEnumerable
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000476 RID: 1142
		// (remove) Token: 0x06000477 RID: 1143
		event MapChangedEventHandler<K, V> MapChanged;
	}
}
