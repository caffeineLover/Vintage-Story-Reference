using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000071 RID: 113
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(4, AllowMultiple = false)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class MarshalingBehaviorAttribute : Attribute
	{
		// Token: 0x0600043F RID: 1087 RVA: 0x000066B7 File Offset: 0x000048B7
		public MarshalingBehaviorAttribute(MarshalingType behavior)
		{
		}
	}
}
