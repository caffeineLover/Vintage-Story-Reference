using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000066 RID: 102
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(1024, AllowMultiple = false)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class ExclusiveToAttribute : Attribute
	{
		// Token: 0x0600042D RID: 1069 RVA: 0x00006677 File Offset: 0x00004877
		public ExclusiveToAttribute(Type typeName)
		{
		}
	}
}
