using System;
using System.Runtime.Versioning;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.UI.ViewManagement
{
	// Token: 0x02000024 RID: 36
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal enum UIColorType
	{
		// Token: 0x0400004C RID: 76
		Background,
		// Token: 0x0400004D RID: 77
		Foreground,
		// Token: 0x0400004E RID: 78
		AccentDark3,
		// Token: 0x0400004F RID: 79
		AccentDark2,
		// Token: 0x04000050 RID: 80
		AccentDark1,
		// Token: 0x04000051 RID: 81
		Accent,
		// Token: 0x04000052 RID: 82
		AccentLight1,
		// Token: 0x04000053 RID: 83
		AccentLight2,
		// Token: 0x04000054 RID: 84
		AccentLight3,
		// Token: 0x04000055 RID: 85
		Complement
	}
}
