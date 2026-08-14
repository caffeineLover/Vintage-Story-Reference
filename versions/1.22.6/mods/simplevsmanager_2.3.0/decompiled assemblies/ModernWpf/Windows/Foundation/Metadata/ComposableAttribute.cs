using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x0200005D RID: 93
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(4, AllowMultiple = true)]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class ComposableAttribute : Attribute
	{
		// Token: 0x06000420 RID: 1056 RVA: 0x0000660F File Offset: 0x0000480F
		public ComposableAttribute(Type type, CompositionType compositionType, uint version)
		{
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00006617 File Offset: 0x00004817
		public ComposableAttribute(Type type, CompositionType compositionType, uint version, Platform platform)
		{
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000661F File Offset: 0x0000481F
		public ComposableAttribute(Type type, CompositionType compositionType, uint version, string contract)
		{
		}
	}
}
