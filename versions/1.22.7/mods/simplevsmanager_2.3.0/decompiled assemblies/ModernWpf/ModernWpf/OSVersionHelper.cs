using System;
using System.Runtime.InteropServices;

namespace ModernWpf
{
	// Token: 0x0200021D RID: 541
	internal static class OSVersionHelper
	{
		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x0002CF19 File Offset: 0x0002B119
		internal static bool IsWindowsNT { get; } = Environment.OSVersion.Platform == 2;

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x0002CF20 File Offset: 0x0002B120
		internal static bool IsWindows8OrGreater { get; } = OSVersionHelper.IsWindowsNT && OSVersionHelper._osVersion >= new Version(6, 2);

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x0002CF27 File Offset: 0x0002B127
		internal static bool IsWindows10OrGreater { get; } = OSVersionHelper.IsWindowsNT && OSVersionHelper._osVersion >= new Version(10, 0);

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x0002CF2E File Offset: 0x0002B12E
		internal static bool IsWindows11OrGreater { get; } = OSVersionHelper.IsWindowsNT && OSVersionHelper._osVersion >= new Version(10, 0, 22000);

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0002CF38 File Offset: 0x0002B138
		private static Version GetOSVersion()
		{
			OSVersionHelper.RTL_OSVERSIONINFOEX rtl_OSVERSIONINFOEX = default(OSVersionHelper.RTL_OSVERSIONINFOEX);
			rtl_OSVERSIONINFOEX.dwOSVersionInfoSize = (uint)Marshal.SizeOf<OSVersionHelper.RTL_OSVERSIONINFOEX>(rtl_OSVERSIONINFOEX);
			OSVersionHelper.RtlGetVersion(out rtl_OSVERSIONINFOEX);
			return new Version((int)rtl_OSVERSIONINFOEX.dwMajorVersion, (int)rtl_OSVERSIONINFOEX.dwMinorVersion, (int)rtl_OSVERSIONINFOEX.dwBuildNumber);
		}

		// Token: 0x06000FC6 RID: 4038
		[DllImport("ntdll.dll")]
		private static extern int RtlGetVersion(out OSVersionHelper.RTL_OSVERSIONINFOEX lpVersionInformation);

		// Token: 0x0400040B RID: 1035
		private static readonly Version _osVersion = OSVersionHelper.GetOSVersion();

		// Token: 0x0200040A RID: 1034
		private struct RTL_OSVERSIONINFOEX
		{
			// Token: 0x040009E9 RID: 2537
			internal uint dwOSVersionInfoSize;

			// Token: 0x040009EA RID: 2538
			internal uint dwMajorVersion;

			// Token: 0x040009EB RID: 2539
			internal uint dwMinorVersion;

			// Token: 0x040009EC RID: 2540
			internal uint dwBuildNumber;

			// Token: 0x040009ED RID: 2541
			internal uint dwPlatformId;

			// Token: 0x040009EE RID: 2542
			[MarshalAs(23, SizeConst = 128)]
			internal string szCSDVersion;
		}
	}
}
