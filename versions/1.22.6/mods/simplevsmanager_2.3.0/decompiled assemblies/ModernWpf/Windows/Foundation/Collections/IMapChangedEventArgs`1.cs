using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation.Collections
{
	// Token: 0x0200008B RID: 139
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[Guid("9939F4DF-050A-4C0F-AA60-77075F9C4777")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal interface IMapChangedEventArgs<K>
	{
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000474 RID: 1140
		CollectionChange CollectionChange { get; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000475 RID: 1141
		K Key { get; }
	}
}
