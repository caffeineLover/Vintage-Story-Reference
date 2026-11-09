using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x0200006A RID: 106
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal enum GCPressureAmount
	{
		// Token: 0x040000FA RID: 250
		Low,
		// Token: 0x040000FB RID: 251
		Medium,
		// Token: 0x040000FC RID: 252
		High
	}
}
