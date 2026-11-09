using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000045 RID: 69
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[Guid("FBC4DD2B-245B-11E4-AF98-689423260CF8")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal interface IMemoryBufferFactory
	{
		// Token: 0x06000341 RID: 833
		MemoryBuffer Create(uint capacity);
	}
}
