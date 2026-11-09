using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x0200005F RID: 95
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(32767, AllowMultiple = true)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class ContractVersionAttribute : Attribute
	{
		// Token: 0x06000423 RID: 1059 RVA: 0x00006627 File Offset: 0x00004827
		public ContractVersionAttribute(uint version)
		{
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000662F File Offset: 0x0000482F
		public ContractVersionAttribute(Type contract, uint version)
		{
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00006637 File Offset: 0x00004837
		public ContractVersionAttribute(string contract, uint version)
		{
		}
	}
}
