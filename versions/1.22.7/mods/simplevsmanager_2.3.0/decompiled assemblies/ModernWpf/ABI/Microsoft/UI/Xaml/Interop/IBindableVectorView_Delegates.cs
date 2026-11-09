using System;
using System.ComponentModel;

namespace ABI.Microsoft.UI.Xaml.Interop
{
	// Token: 0x02000177 RID: 375
	[EditorBrowsable(1)]
	internal static class IBindableVectorView_Delegates
	{
		// Token: 0x020003BA RID: 954
		// (Invoke) Token: 0x06001A7A RID: 6778
		public unsafe delegate int GetAt_0(IntPtr thisPtr, uint index, IntPtr* result);

		// Token: 0x020003BB RID: 955
		// (Invoke) Token: 0x06001A7E RID: 6782
		public unsafe delegate int get_Size_1(IntPtr thisPtr, uint* result);

		// Token: 0x020003BC RID: 956
		// (Invoke) Token: 0x06001A82 RID: 6786
		public unsafe delegate int IndexOf_2(IntPtr thisPtr, IntPtr value, uint* index, byte* returnValue);
	}
}
