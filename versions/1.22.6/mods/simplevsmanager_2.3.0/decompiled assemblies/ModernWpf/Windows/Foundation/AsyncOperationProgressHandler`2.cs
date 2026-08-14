using System;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000034 RID: 52
	// (Invoke) Token: 0x060002FC RID: 764
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal delegate void AsyncOperationProgressHandler<TResult, TProgress>(IAsyncOperationWithProgress<TResult, TProgress> asyncInfo, TProgress progressInfo);
}
