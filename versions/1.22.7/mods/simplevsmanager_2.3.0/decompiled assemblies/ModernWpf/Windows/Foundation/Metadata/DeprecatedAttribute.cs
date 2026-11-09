using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000063 RID: 99
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(6108, AllowMultiple = true)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class DeprecatedAttribute : Attribute
	{
		// Token: 0x06000429 RID: 1065 RVA: 0x00006657 File Offset: 0x00004857
		public DeprecatedAttribute(string message, DeprecationType type, uint version)
		{
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000665F File Offset: 0x0000485F
		public DeprecatedAttribute(string message, DeprecationType type, uint version, Platform platform)
		{
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00006667 File Offset: 0x00004867
		public DeprecatedAttribute(string message, DeprecationType type, uint version, string contract)
		{
		}
	}
}
