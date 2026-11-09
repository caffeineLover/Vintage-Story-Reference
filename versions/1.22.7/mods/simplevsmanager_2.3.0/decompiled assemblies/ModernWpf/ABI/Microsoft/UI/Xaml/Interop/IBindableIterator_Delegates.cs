using System;
using System.ComponentModel;

namespace ABI.Microsoft.UI.Xaml.Interop
{
	// Token: 0x02000175 RID: 373
	[EditorBrowsable(1)]
	internal static class IBindableIterator_Delegates
	{
		// Token: 0x020003B4 RID: 948
		// (Invoke) Token: 0x06001A5D RID: 6749
		public unsafe delegate int get_Current_0(IntPtr thisPtr, IntPtr* result);

		// Token: 0x020003B5 RID: 949
		// (Invoke) Token: 0x06001A61 RID: 6753
		public unsafe delegate int get_HasCurrent_1(IntPtr thisPtr, byte* result);

		// Token: 0x020003B6 RID: 950
		// (Invoke) Token: 0x06001A65 RID: 6757
		public unsafe delegate int MoveNext_2(IntPtr thisPtr, byte* result);

		// Token: 0x020003B7 RID: 951
		// (Invoke) Token: 0x06001A69 RID: 6761
		public unsafe delegate int GetMany_3(IntPtr thisPtr, int itemSize, IntPtr items, uint* result);
	}
}
