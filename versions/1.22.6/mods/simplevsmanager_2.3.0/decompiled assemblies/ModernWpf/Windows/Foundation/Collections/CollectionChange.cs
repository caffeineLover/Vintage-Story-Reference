using System;
using System.Runtime.Versioning;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation.Collections
{
	// Token: 0x0200008A RID: 138
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal enum CollectionChange
	{
		// Token: 0x0400010D RID: 269
		Reset,
		// Token: 0x0400010E RID: 270
		ItemInserted,
		// Token: 0x0400010F RID: 271
		ItemRemoved,
		// Token: 0x04000110 RID: 272
		ItemChanged
	}
}
