using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x0200004E RID: 78
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[Guid("D45A0451-F225-4542-9296-0E1DF5D254DF")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal interface IWwwFormUrlDecoderRuntimeClass : IEnumerable<IWwwFormUrlDecoderEntry>, IEnumerable, IReadOnlyList<IWwwFormUrlDecoderEntry>, IReadOnlyCollection<IWwwFormUrlDecoderEntry>
	{
		// Token: 0x06000386 RID: 902
		string GetFirstValueByName(string name);
	}
}
