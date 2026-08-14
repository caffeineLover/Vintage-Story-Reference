using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000061 RID: 97
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(32767, AllowMultiple = false)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class DefaultAttribute : Attribute
	{
	}
}
