using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000060 RID: 96
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[AttributeUsage(12, AllowMultiple = false)]
	[SupportedOSPlatform("Windows10.0.14393.0")]
	[ContractVersion(typeof(UniversalApiContract), 196608U)]
	internal sealed class CreateFromStringAttribute : Attribute
	{
		// Token: 0x040000EF RID: 239
		public string MethodName;
	}
}
