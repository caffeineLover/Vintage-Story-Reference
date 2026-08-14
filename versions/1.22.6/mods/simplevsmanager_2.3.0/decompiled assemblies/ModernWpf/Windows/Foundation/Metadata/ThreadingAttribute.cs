using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x0200007E RID: 126
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(4, AllowMultiple = false)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class ThreadingAttribute : Attribute
	{
		// Token: 0x0600044D RID: 1101 RVA: 0x00006727 File Offset: 0x00004927
		public ThreadingAttribute(ThreadingModel model)
		{
		}
	}
}
