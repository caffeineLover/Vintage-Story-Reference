using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000064 RID: 100
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal enum DeprecationType
	{
		// Token: 0x040000F1 RID: 241
		Deprecate,
		// Token: 0x040000F2 RID: 242
		Remove
	}
}
