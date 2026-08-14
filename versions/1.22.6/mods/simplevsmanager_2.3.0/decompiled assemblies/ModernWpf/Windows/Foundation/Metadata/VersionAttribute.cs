using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000081 RID: 129
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(32767, AllowMultiple = true)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class VersionAttribute : Attribute
	{
		// Token: 0x0600044F RID: 1103 RVA: 0x00006737 File Offset: 0x00004937
		public VersionAttribute(uint version)
		{
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000673F File Offset: 0x0000493F
		public VersionAttribute(uint version, Platform platform)
		{
		}
	}
}
