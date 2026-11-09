using System;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000031 RID: 49
	// (Invoke) Token: 0x060002F0 RID: 752
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal delegate void AsyncActionProgressHandler<TProgress>(IAsyncActionWithProgress<TProgress> asyncInfo, TProgress progressInfo);
}
