using System;
using System.ComponentModel;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000C1 RID: 193
	[EditorBrowsable(1)]
	internal static class IAsyncActionWithProgress_Delegates
	{
		// Token: 0x02000300 RID: 768
		// (Invoke) Token: 0x060016CE RID: 5838
		public delegate int put_Progress_0(IntPtr thisPtr, IntPtr handler);

		// Token: 0x02000301 RID: 769
		// (Invoke) Token: 0x060016D2 RID: 5842
		public unsafe delegate int get_Progress_1(IntPtr thisPtr, IntPtr* __return_value__);

		// Token: 0x02000302 RID: 770
		// (Invoke) Token: 0x060016D6 RID: 5846
		public delegate int put_Completed_2(IntPtr thisPtr, IntPtr handler);

		// Token: 0x02000303 RID: 771
		// (Invoke) Token: 0x060016DA RID: 5850
		public unsafe delegate int get_Completed_3(IntPtr thisPtr, IntPtr* __return_value__);

		// Token: 0x02000304 RID: 772
		// (Invoke) Token: 0x060016DE RID: 5854
		public delegate int GetResults_4(IntPtr thisPtr);
	}
}
