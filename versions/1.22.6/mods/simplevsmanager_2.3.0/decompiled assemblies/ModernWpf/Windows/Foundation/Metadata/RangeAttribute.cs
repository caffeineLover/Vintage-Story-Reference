using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x0200007B RID: 123
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(2048, AllowMultiple = false)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class RangeAttribute : Attribute
	{
		// Token: 0x06000448 RID: 1096 RVA: 0x000066FF File Offset: 0x000048FF
		public RangeAttribute(int minValue, int maxValue)
		{
		}
	}
}
