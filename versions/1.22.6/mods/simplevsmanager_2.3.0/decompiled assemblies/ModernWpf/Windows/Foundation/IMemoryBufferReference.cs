using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000046 RID: 70
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[Guid("FBC4DD29-245B-11E4-AF98-689423260CF8")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal interface IMemoryBufferReference : IDisposable
	{
		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000342 RID: 834
		uint Capacity { get; }

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000343 RID: 835
		// (remove) Token: 0x06000344 RID: 836
		event TypedEventHandler<IMemoryBufferReference, object> Closed;
	}
}
