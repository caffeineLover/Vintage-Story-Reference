using System;
using System.Runtime.Versioning;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000036 RID: 54
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal enum AsyncStatus
	{
		// Token: 0x040000B5 RID: 181
		Canceled = 2,
		// Token: 0x040000B6 RID: 182
		Completed = 1,
		// Token: 0x040000B7 RID: 183
		Error = 3,
		// Token: 0x040000B8 RID: 184
		Started = 0
	}
}
