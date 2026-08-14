using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation.Collections
{
	// Token: 0x0200008F RID: 143
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[Guid("575933DF-34FE-4480-AF15-07691F3D5D9B")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal interface IVectorChangedEventArgs
	{
		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x0600047A RID: 1146
		CollectionChange CollectionChange { get; }

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600047B RID: 1147
		uint Index { get; }
	}
}
