using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000079 RID: 121
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(32767, AllowMultiple = true)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class PreviousContractVersionAttribute : Attribute
	{
		// Token: 0x06000445 RID: 1093 RVA: 0x000066E7 File Offset: 0x000048E7
		public PreviousContractVersionAttribute(string contract, uint versionLow, uint versionHigh, string newContract)
		{
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x000066EF File Offset: 0x000048EF
		public PreviousContractVersionAttribute(string contract, uint versionLow, uint versionHigh)
		{
		}
	}
}
