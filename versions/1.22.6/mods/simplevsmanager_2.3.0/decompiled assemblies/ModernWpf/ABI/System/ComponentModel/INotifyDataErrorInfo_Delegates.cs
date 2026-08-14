using System;
using WinRT;

namespace ABI.System.ComponentModel
{
	// Token: 0x02000147 RID: 327
	internal static class INotifyDataErrorInfo_Delegates
	{
		// Token: 0x0200035B RID: 859
		// (Invoke) Token: 0x06001854 RID: 6228
		public unsafe delegate int get_HasErrors_0(IntPtr thisPtr, byte* value);

		// Token: 0x0200035C RID: 860
		// (Invoke) Token: 0x06001858 RID: 6232
		public unsafe delegate int add_ErrorsChanged_1(IntPtr thisPtr, IntPtr handler, EventRegistrationToken* token);

		// Token: 0x0200035D RID: 861
		// (Invoke) Token: 0x0600185C RID: 6236
		public delegate int remove_ErrorsChanged_2(IntPtr thisPtr, EventRegistrationToken token);

		// Token: 0x0200035E RID: 862
		// (Invoke) Token: 0x06001860 RID: 6240
		public unsafe delegate int GetErrors_3(IntPtr thisPtr, IntPtr propertyName, IntPtr* result);
	}
}
