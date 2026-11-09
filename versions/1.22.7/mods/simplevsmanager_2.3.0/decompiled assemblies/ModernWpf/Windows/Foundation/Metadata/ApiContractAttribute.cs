using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x0200005B RID: 91
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(16, AllowMultiple = false)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class ApiContractAttribute : Attribute
	{
	}
}
