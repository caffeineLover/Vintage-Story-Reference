using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x0200006C RID: 108
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(5120, AllowMultiple = false)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class GuidAttribute : Attribute
	{
		// Token: 0x06000431 RID: 1073 RVA: 0x00006697 File Offset: 0x00004897
		public GuidAttribute(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
		{
		}
	}
}
