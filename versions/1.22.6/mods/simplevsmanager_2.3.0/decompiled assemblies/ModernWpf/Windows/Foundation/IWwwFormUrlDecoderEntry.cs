using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x0200004D RID: 77
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[Guid("125E7431-F678-4E8E-B670-20A9B06C512D")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal interface IWwwFormUrlDecoderEntry
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000384 RID: 900
		string Name { get; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000385 RID: 901
		string Value { get; }
	}
}
