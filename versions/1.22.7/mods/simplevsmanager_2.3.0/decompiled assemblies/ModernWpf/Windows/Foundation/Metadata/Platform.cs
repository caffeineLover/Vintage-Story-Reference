using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000077 RID: 119
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal enum Platform
	{
		// Token: 0x04000105 RID: 261
		Windows,
		// Token: 0x04000106 RID: 262
		WindowsPhone
	}
}
