using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000075 RID: 117
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(64, AllowMultiple = false)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class OverloadAttribute : Attribute
	{
		// Token: 0x06000442 RID: 1090 RVA: 0x000066CF File Offset: 0x000048CF
		public OverloadAttribute(string method)
		{
		}
	}
}
