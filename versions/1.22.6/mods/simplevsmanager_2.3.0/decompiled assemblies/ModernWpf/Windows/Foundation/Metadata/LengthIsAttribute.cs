using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000070 RID: 112
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(2048, AllowMultiple = false)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class LengthIsAttribute : Attribute
	{
		// Token: 0x0600043E RID: 1086 RVA: 0x000066AF File Offset: 0x000048AF
		public LengthIsAttribute(int indexLengthParameter)
		{
		}
	}
}
