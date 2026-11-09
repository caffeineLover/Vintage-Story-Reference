using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x0200007D RID: 125
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(4, AllowMultiple = true)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class StaticAttribute : Attribute
	{
		// Token: 0x0600044A RID: 1098 RVA: 0x0000670F File Offset: 0x0000490F
		public StaticAttribute(Type type, uint version)
		{
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00006717 File Offset: 0x00004917
		public StaticAttribute(Type type, uint version, Platform platform)
		{
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000671F File Offset: 0x0000491F
		public StaticAttribute(Type type, uint version, string contractName)
		{
		}
	}
}
