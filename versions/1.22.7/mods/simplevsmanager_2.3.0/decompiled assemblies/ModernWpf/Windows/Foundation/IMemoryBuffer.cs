using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000044 RID: 68
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[Guid("FBC4DD2A-245B-11E4-AF98-689423260CF8")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal interface IMemoryBuffer : IDisposable
	{
		// Token: 0x06000340 RID: 832
		IMemoryBufferReference CreateReference();
	}
}
