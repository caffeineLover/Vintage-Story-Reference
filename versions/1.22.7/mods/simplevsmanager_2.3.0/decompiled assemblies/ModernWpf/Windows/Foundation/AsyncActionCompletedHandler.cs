using System;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000030 RID: 48
	// (Invoke) Token: 0x060002EC RID: 748
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal delegate void AsyncActionCompletedHandler(IAsyncAction asyncInfo, AsyncStatus asyncStatus);
}
