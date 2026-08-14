using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000058 RID: 88
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(4, AllowMultiple = true)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class ActivatableAttribute : Attribute
	{
		// Token: 0x06000409 RID: 1033 RVA: 0x000064D3 File Offset: 0x000046D3
		public ActivatableAttribute(uint version)
		{
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x000064DB File Offset: 0x000046DB
		public ActivatableAttribute(uint version, string type)
		{
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x000064E3 File Offset: 0x000046E3
		public ActivatableAttribute(uint version, Platform platform)
		{
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000064EB File Offset: 0x000046EB
		public ActivatableAttribute(Type type, uint version)
		{
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x000064F3 File Offset: 0x000046F3
		public ActivatableAttribute(Type type, uint version, string contractName)
		{
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000064FB File Offset: 0x000046FB
		public ActivatableAttribute(Type type, uint version, Platform platform)
		{
		}
	}
}
