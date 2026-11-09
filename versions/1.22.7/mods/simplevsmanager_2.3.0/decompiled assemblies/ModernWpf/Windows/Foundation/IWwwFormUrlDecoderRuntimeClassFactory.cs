using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x0200004F RID: 79
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[Guid("5B8C6B3D-24AE-41B5-A1BF-F0C3D544845B")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal interface IWwwFormUrlDecoderRuntimeClassFactory
	{
		// Token: 0x06000387 RID: 903
		WwwFormUrlDecoder CreateWwwFormUrlDecoder(string query);
	}
}
