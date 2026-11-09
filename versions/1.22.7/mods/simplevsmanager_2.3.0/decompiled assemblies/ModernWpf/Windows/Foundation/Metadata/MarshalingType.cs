using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000072 RID: 114
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal enum MarshalingType
	{
		// Token: 0x040000FF RID: 255
		None = 1,
		// Token: 0x04000100 RID: 256
		Agile,
		// Token: 0x04000101 RID: 257
		Standard,
		// Token: 0x04000102 RID: 258
		InvalidMarshaling = 0
	}
}
