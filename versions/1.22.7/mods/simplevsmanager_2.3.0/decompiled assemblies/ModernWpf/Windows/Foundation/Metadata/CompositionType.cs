using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x0200005E RID: 94
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal enum CompositionType
	{
		// Token: 0x040000ED RID: 237
		Protected = 1,
		// Token: 0x040000EE RID: 238
		Public
	}
}
