using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x0200006F RID: 111
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(5148, AllowMultiple = false)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class InternalAttribute : Attribute
	{
	}
}
