using System;
using System.Runtime.InteropServices;

namespace MS.Win32
{
	// Token: 0x02000208 RID: 520
	internal class UnsafeNativeMethods
	{
		// Token: 0x06000F25 RID: 3877
		[DllImport("user32.dll", CharSet = 4, ExactSpelling = true, SetLastError = true)]
		public static extern bool SetWindowPos(HandleRef hWnd, HandleRef hWndInsertAfter, int x, int y, int cx, int cy, int flags);

		// Token: 0x06000F26 RID: 3878
		[DllImport("user32.dll", CharSet = 4, EntryPoint = "ClientToScreen", ExactSpelling = true, SetLastError = true)]
		private static extern int IntClientToScreen(HandleRef hWnd, [In] [Out] NativeMethods.POINT pt);

		// Token: 0x06000F27 RID: 3879
		[DllImport("user32.dll", CharSet = 4, ExactSpelling = true)]
		public static extern IntPtr GetActiveWindow();
	}
}
