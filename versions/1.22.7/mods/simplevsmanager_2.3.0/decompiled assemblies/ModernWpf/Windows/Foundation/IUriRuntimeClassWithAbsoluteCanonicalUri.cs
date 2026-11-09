using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x0200004C RID: 76
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[Guid("758D9661-221C-480F-A339-50656673F46F")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal interface IUriRuntimeClassWithAbsoluteCanonicalUri
	{
		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000382 RID: 898
		string AbsoluteCanonicalUri { get; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000383 RID: 899
		string DisplayIri { get; }
	}
}
