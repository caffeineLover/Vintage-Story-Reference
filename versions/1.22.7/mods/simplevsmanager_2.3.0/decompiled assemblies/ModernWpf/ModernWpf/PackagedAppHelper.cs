using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ModernWpf
{
	// Token: 0x0200021E RID: 542
	internal static class PackagedAppHelper
	{
		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x0002D00C File Offset: 0x0002B20C
		public static bool IsPackagedApp
		{
			get
			{
				if (OSVersionHelper.IsWindows8OrGreater)
				{
					int length = 0;
					StringBuilder stringBuilder = new StringBuilder(0);
					PackagedAppHelper.GetCurrentPackageFullName(ref length, stringBuilder);
					stringBuilder.Length = length;
					return (long)PackagedAppHelper.GetCurrentPackageFullName(ref length, stringBuilder) != 15700L;
				}
				return false;
			}
		}

		// Token: 0x06000FC9 RID: 4041
		[DllImport("kernel32.dll", CharSet = 3, SetLastError = true)]
		private static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder packageFullName);

		// Token: 0x04000410 RID: 1040
		private const long APPMODEL_ERROR_NO_PACKAGE = 15700L;
	}
}
