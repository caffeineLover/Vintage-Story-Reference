using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000041 RID: 65
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[Guid("65A1ECC5-3FB5-4832-8CA9-F061B281D13A")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal interface IDeferralFactory
	{
		// Token: 0x0600033B RID: 827
		Deferral Create(DeferralCompletedHandler handler);
	}
}
