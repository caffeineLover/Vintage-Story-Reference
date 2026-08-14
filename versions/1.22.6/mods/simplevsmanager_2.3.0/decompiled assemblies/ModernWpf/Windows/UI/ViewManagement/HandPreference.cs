using System;
using System.Runtime.Versioning;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.UI.ViewManagement
{
	// Token: 0x0200001D RID: 29
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal enum HandPreference
	{
		// Token: 0x04000049 RID: 73
		LeftHanded,
		// Token: 0x0400004A RID: 74
		RightHanded
	}
}
