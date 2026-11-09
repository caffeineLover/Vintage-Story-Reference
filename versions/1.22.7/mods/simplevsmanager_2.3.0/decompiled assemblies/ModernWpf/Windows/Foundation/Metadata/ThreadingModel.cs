using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x0200007F RID: 127
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal enum ThreadingModel
	{
		// Token: 0x04000108 RID: 264
		STA = 1,
		// Token: 0x04000109 RID: 265
		MTA,
		// Token: 0x0400010A RID: 266
		Both,
		// Token: 0x0400010B RID: 267
		InvalidThreading = 0
	}
}
