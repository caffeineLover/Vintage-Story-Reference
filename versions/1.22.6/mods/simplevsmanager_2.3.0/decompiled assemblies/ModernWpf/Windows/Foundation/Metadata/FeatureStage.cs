using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000069 RID: 105
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[SupportedOSPlatform("Windows10.0.15063.0")]
	[ContractVersion(typeof(FoundationContract), 196608U)]
	internal enum FeatureStage
	{
		// Token: 0x040000F5 RID: 245
		AlwaysDisabled,
		// Token: 0x040000F6 RID: 246
		DisabledByDefault,
		// Token: 0x040000F7 RID: 247
		EnabledByDefault,
		// Token: 0x040000F8 RID: 248
		AlwaysEnabled
	}
}
