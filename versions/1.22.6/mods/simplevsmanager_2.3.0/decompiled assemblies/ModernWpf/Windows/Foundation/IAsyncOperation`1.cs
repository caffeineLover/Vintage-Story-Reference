using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x0200003F RID: 63
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[Guid("9FC2B0BB-E446-44E2-AA61-9CAB8F636AF2")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal interface IAsyncOperation<TResult> : IAsyncInfo
	{
		// Token: 0x06000337 RID: 823
		TResult GetResults();

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000338 RID: 824
		// (set) Token: 0x06000339 RID: 825
		AsyncOperationCompletedHandler<TResult> Completed { get; set; }
	}
}
