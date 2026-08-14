using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000049 RID: 73
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[Guid("C1D432BA-C824-4452-A7FD-512BC3BBE9A1")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal interface IUriEscapeStatics
	{
		// Token: 0x0600036D RID: 877
		string UnescapeComponent(string toUnescape);

		// Token: 0x0600036E RID: 878
		string EscapeComponent(string toEscape);
	}
}
