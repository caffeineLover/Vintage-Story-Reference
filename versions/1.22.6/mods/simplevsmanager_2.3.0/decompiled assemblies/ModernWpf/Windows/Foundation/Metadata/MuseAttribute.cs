using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000074 RID: 116
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(4, AllowMultiple = false)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class MuseAttribute : Attribute
	{
		// Token: 0x04000103 RID: 259
		public uint Version;
	}
}
