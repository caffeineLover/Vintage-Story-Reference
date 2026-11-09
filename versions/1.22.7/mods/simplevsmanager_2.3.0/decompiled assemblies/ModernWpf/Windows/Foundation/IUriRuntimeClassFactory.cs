using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x0200004B RID: 75
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[Guid("44A9796F-723E-4FDF-A218-033E75B0C084")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal interface IUriRuntimeClassFactory
	{
		// Token: 0x06000380 RID: 896
		Uri CreateUri(string uri);

		// Token: 0x06000381 RID: 897
		Uri CreateWithRelativeUri(string baseUri, string relativeUri);
	}
}
