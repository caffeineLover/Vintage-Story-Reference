using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000073 RID: 115
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(5124, AllowMultiple = false)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class MetadataMarshalAttribute : Attribute
	{
	}
}
