using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation.Collections
{
	// Token: 0x0200008D RID: 141
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[Guid("5917EB53-50B4-4A0D-B309-65862B3F1DBC")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal interface IObservableVector<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000478 RID: 1144
		// (remove) Token: 0x06000479 RID: 1145
		event VectorChangedEventHandler<T> VectorChanged;
	}
}
