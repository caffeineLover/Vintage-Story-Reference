using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000040 RID: 64
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[Guid("D6269732-3B7F-46A7-B40B-4FDCA2A2C693")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal interface IDeferral : IDisposable
	{
		// Token: 0x0600033A RID: 826
		void Complete();
	}
}
