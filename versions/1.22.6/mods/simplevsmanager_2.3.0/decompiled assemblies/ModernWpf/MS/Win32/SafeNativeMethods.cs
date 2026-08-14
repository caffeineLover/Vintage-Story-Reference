using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MS.Win32
{
	// Token: 0x02000207 RID: 519
	internal static class SafeNativeMethods
	{
		// Token: 0x06000F23 RID: 3875 RVA: 0x0002B940 File Offset: 0x00029B40
		public static IntPtr MonitorFromRect(ref NativeMethods.RECT rect, int flags)
		{
			return SafeNativeMethods.SafeNativeMethodsPrivate.MonitorFromRect(ref rect, flags);
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x0002B949 File Offset: 0x00029B49
		internal static void GetWindowRect(HandleRef hWnd, [In] [Out] ref NativeMethods.RECT rect)
		{
			if (!SafeNativeMethods.SafeNativeMethodsPrivate.IntGetWindowRect(hWnd, ref rect))
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x02000406 RID: 1030
		private class SafeNativeMethodsPrivate
		{
			// Token: 0x06001B9F RID: 7071
			[DllImport("user32.dll", CharSet = 4, EntryPoint = "GetWindowRect", ExactSpelling = true, SetLastError = true)]
			public static extern bool IntGetWindowRect(HandleRef hWnd, [In] [Out] ref NativeMethods.RECT rect);

			// Token: 0x06001BA0 RID: 7072
			[DllImport("user32.dll", ExactSpelling = true)]
			public static extern IntPtr MonitorFromRect(ref NativeMethods.RECT rect, int flags);

			// Token: 0x06001BA1 RID: 7073
			[DllImport("user32.dll", CharSet = 4, EntryPoint = "ScreenToClient", ExactSpelling = true, SetLastError = true)]
			public static extern int IntScreenToClient(HandleRef hWnd, [In] [Out] NativeMethods.POINT pt);
		}
	}
}
