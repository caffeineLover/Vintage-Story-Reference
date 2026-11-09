using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000062 RID: 98
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(64, AllowMultiple = false)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class DefaultOverloadAttribute : Attribute
	{
	}
}
