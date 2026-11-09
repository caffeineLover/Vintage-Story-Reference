using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace ModernWpf
{
	// Token: 0x0200021A RID: 538
	internal static class IconHelper
	{
		// Token: 0x06000FB5 RID: 4021 RVA: 0x0002CD24 File Offset: 0x0002AF24
		public static void GetDefaultIconHandles(IntPtr[] largeIconHandle, IntPtr[] smallIconHandle)
		{
			IconHelper.NativeMethods.ExtractIconEx(IconHelper.GetModuleFileName(default(HandleRef)), 0, largeIconHandle, smallIconHandle, 1U);
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0002CD49 File Offset: 0x0002AF49
		public static bool DestroyIcon(IntPtr icon)
		{
			bool result = IconHelper.NativeMethods.DestroyIcon(icon);
			Marshal.GetLastWin32Error();
			return result;
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0002CD5C File Offset: 0x0002AF5C
		private static string GetModuleFileName(HandleRef hModule)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			for (;;)
			{
				int moduleFileName = IconHelper.NativeMethods.GetModuleFileName(hModule, stringBuilder, stringBuilder.Capacity);
				if (moduleFileName == 0)
				{
					break;
				}
				if (moduleFileName != stringBuilder.Capacity)
				{
					goto IL_3A;
				}
				stringBuilder.EnsureCapacity(stringBuilder.Capacity * 2);
			}
			throw new Win32Exception();
			IL_3A:
			return stringBuilder.ToString();
		}

		// Token: 0x04000409 RID: 1033
		internal const int MAX_PATH = 260;

		// Token: 0x02000409 RID: 1033
		private static class NativeMethods
		{
			// Token: 0x06001BA8 RID: 7080
			[DllImport("Shell32.dll", CharSet = 4)]
			public static extern int ExtractIconEx(string lpszFile, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);

			// Token: 0x06001BA9 RID: 7081
			[DllImport("user32.dll", CharSet = 4, SetLastError = true)]
			public static extern bool DestroyIcon(IntPtr hIcon);

			// Token: 0x06001BAA RID: 7082
			[DllImport("kernel32.dll", CharSet = 3, SetLastError = true)]
			public static extern int GetModuleFileName(HandleRef hModule, StringBuilder buffer, int length);
		}
	}
}
