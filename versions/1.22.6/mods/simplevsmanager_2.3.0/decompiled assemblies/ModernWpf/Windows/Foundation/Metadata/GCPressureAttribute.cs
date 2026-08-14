using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x0200006B RID: 107
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(4, AllowMultiple = false)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class GCPressureAttribute : Attribute
	{
		// Token: 0x040000FD RID: 253
		public GCPressureAmount amount;
	}
}
