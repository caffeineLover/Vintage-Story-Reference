using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000203 RID: 515
	internal static class NativeMethods
	{
		// Token: 0x06000F19 RID: 3865
		[DllImport("user32.dll", EntryPoint = "GetMonitorInfo", SetLastError = true)]
		[return: MarshalAs(2)]
		private static extern bool _GetMonitorInfo(IntPtr hMonitor, [In] [Out] MONITORINFO lpmi);

		// Token: 0x06000F1A RID: 3866 RVA: 0x0002B8C4 File Offset: 0x00029AC4
		public static MONITORINFO GetMonitorInfo(IntPtr hMonitor)
		{
			MONITORINFO monitorinfo = new MONITORINFO();
			if (!NativeMethods._GetMonitorInfo(hMonitor, monitorinfo))
			{
				throw new Win32Exception();
			}
			return monitorinfo;
		}

		// Token: 0x06000F1B RID: 3867
		[DllImport("user32.dll")]
		public static extern int GetSystemMetrics(SM nIndex);

		// Token: 0x06000F1C RID: 3868
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(2)]
		private static extern bool GetWindowPlacement(IntPtr hwnd, WINDOWPLACEMENT lpwndpl);

		// Token: 0x06000F1D RID: 3869 RVA: 0x0002B8E8 File Offset: 0x00029AE8
		public static WINDOWPLACEMENT GetWindowPlacement(IntPtr hwnd)
		{
			WINDOWPLACEMENT windowplacement = new WINDOWPLACEMENT();
			if (NativeMethods.GetWindowPlacement(hwnd, windowplacement))
			{
				return windowplacement;
			}
			throw new Win32Exception();
		}
	}
}
