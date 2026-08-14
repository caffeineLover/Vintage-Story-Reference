using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x0200003C RID: 60
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[Guid("1F6DB258-E803-48A1-9546-EB7353398884")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal interface IAsyncActionWithProgress<TProgress> : IAsyncInfo
	{
		// Token: 0x06000328 RID: 808
		void GetResults();

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000329 RID: 809
		// (set) Token: 0x0600032A RID: 810
		AsyncActionProgressHandler<TProgress> Progress { get; set; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600032B RID: 811
		// (set) Token: 0x0600032C RID: 812
		AsyncActionWithProgressCompletedHandler<TProgress> Completed { get; set; }
	}
}
