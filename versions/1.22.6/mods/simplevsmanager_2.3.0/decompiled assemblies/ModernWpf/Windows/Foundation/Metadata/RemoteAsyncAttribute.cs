using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x0200007C RID: 124
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(68, AllowMultiple = false)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class RemoteAsyncAttribute : Attribute
	{
	}
}
