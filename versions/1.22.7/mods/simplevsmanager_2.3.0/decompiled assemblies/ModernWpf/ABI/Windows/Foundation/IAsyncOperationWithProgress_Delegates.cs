using System;
using System.ComponentModel;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000C6 RID: 198
	[EditorBrowsable(1)]
	internal static class IAsyncOperationWithProgress_Delegates
	{
		// Token: 0x02000306 RID: 774
		// (Invoke) Token: 0x060016F1 RID: 5873
		public delegate int put_Progress_0(IntPtr thisPtr, IntPtr handler);

		// Token: 0x02000307 RID: 775
		// (Invoke) Token: 0x060016F5 RID: 5877
		public unsafe delegate int get_Progress_1(IntPtr thisPtr, IntPtr* __return_value__);

		// Token: 0x02000308 RID: 776
		// (Invoke) Token: 0x060016F9 RID: 5881
		public delegate int put_Completed_2(IntPtr thisPtr, IntPtr handler);

		// Token: 0x02000309 RID: 777
		// (Invoke) Token: 0x060016FD RID: 5885
		public unsafe delegate int get_Completed_3(IntPtr thisPtr, IntPtr* __return_value__);
	}
}
