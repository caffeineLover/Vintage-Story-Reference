using System;
using System.ComponentModel;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000C9 RID: 201
	[EditorBrowsable(1)]
	internal static class IAsyncOperation_Delegates
	{
		// Token: 0x0200030B RID: 779
		// (Invoke) Token: 0x0600170A RID: 5898
		public delegate int put_Completed_0(IntPtr thisPtr, IntPtr handler);

		// Token: 0x0200030C RID: 780
		// (Invoke) Token: 0x0600170E RID: 5902
		public unsafe delegate int get_Completed_1(IntPtr thisPtr, IntPtr* __return_value__);
	}
}
