using System;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation.Collections
{
	// Token: 0x02000094 RID: 148
	// (Invoke) Token: 0x0600050A RID: 1290
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal delegate void VectorChangedEventHandler<T>(IObservableVector<T> sender, IVectorChangedEventArgs @event);
}
