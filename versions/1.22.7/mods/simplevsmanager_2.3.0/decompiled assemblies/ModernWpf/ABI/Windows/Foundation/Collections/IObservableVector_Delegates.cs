using System;
using System.ComponentModel;
using WinRT;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x020000FD RID: 253
	[EditorBrowsable(1)]
	internal static class IObservableVector_Delegates
	{
		// Token: 0x0200031A RID: 794
		// (Invoke) Token: 0x06001752 RID: 5970
		public unsafe delegate int add_VectorChanged_0(IntPtr thisPtr, IntPtr vhnd, EventRegistrationToken* __return_value__);

		// Token: 0x0200031B RID: 795
		// (Invoke) Token: 0x06001756 RID: 5974
		public delegate int remove_VectorChanged_1(IntPtr thisPtr, EventRegistrationToken token);
	}
}
