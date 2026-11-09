using System;
using System.ComponentModel;
using WinRT;

namespace ABI.System.Windows.Input
{
	// Token: 0x02000144 RID: 324
	[EditorBrowsable(1)]
	internal static class ICommand_Delegates
	{
		// Token: 0x02000355 RID: 853
		// (Invoke) Token: 0x0600183D RID: 6205
		public unsafe delegate int add_CanExecuteChanged_0(IntPtr thisPtr, IntPtr handler, EventRegistrationToken* token);

		// Token: 0x02000356 RID: 854
		// (Invoke) Token: 0x06001841 RID: 6209
		public unsafe delegate int CanExecute_2(IntPtr thisPtr, IntPtr parameter, byte* result);

		// Token: 0x02000357 RID: 855
		// (Invoke) Token: 0x06001845 RID: 6213
		public delegate int Execute_3(IntPtr thisPtr, IntPtr parameter);
	}
}
