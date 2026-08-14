using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x0200003E RID: 62
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[Guid("B5D036D7-E297-498F-BA60-0289E76E23DD")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal interface IAsyncOperationWithProgress<TResult, TProgress> : IAsyncInfo
	{
		// Token: 0x06000332 RID: 818
		TResult GetResults();

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000333 RID: 819
		// (set) Token: 0x06000334 RID: 820
		AsyncOperationProgressHandler<TResult, TProgress> Progress { get; set; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000335 RID: 821
		// (set) Token: 0x06000336 RID: 822
		AsyncOperationWithProgressCompletedHandler<TResult, TProgress> Completed { get; set; }
	}
}
