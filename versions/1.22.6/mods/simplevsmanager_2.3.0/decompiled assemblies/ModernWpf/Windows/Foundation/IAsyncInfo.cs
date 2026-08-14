using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x0200003D RID: 61
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[Guid("00000036-0000-0000-C000-000000000046")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal interface IAsyncInfo
	{
		// Token: 0x0600032D RID: 813
		void Cancel();

		// Token: 0x0600032E RID: 814
		void Close();

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600032F RID: 815
		Exception ErrorCode { get; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000330 RID: 816
		uint Id { get; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000331 RID: 817
		AsyncStatus Status { get; }
	}
}
