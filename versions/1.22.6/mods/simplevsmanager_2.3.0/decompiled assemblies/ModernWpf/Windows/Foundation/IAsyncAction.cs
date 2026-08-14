using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x0200003B RID: 59
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[Guid("5A648006-843A-4DA9-865B-9D26E5DFAD7B")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal interface IAsyncAction : IAsyncInfo
	{
		// Token: 0x06000325 RID: 805
		void GetResults();

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000326 RID: 806
		// (set) Token: 0x06000327 RID: 807
		AsyncActionCompletedHandler Completed { get; set; }
	}
}
