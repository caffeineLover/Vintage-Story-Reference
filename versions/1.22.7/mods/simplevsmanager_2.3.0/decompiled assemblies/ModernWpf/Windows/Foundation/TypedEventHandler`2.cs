using System;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000052 RID: 82
	// (Invoke) Token: 0x060003CC RID: 972
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal delegate void TypedEventHandler<TSender, TResult>(TSender sender, TResult args);
}
