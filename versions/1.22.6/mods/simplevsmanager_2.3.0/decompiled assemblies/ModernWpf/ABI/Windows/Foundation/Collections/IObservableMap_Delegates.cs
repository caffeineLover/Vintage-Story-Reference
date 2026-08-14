using System;
using System.ComponentModel;
using WinRT;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x020000FA RID: 250
	[EditorBrowsable(1)]
	internal static class IObservableMap_Delegates
	{
		// Token: 0x02000316 RID: 790
		// (Invoke) Token: 0x0600173E RID: 5950
		public unsafe delegate int add_MapChanged_0(IntPtr thisPtr, IntPtr vhnd, EventRegistrationToken* __return_value__);

		// Token: 0x02000317 RID: 791
		// (Invoke) Token: 0x06001742 RID: 5954
		public delegate int remove_MapChanged_1(IntPtr thisPtr, EventRegistrationToken token);
	}
}
