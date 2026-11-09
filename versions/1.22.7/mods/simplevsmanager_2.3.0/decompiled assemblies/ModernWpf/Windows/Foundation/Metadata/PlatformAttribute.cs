using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000078 RID: 120
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class PlatformAttribute : Attribute
	{
		// Token: 0x06000444 RID: 1092 RVA: 0x000066DF File Offset: 0x000048DF
		public PlatformAttribute(Platform platform)
		{
		}
	}
}
