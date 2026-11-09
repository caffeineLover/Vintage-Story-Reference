using System;

namespace Standard
{
	// Token: 0x02000204 RID: 516
	internal static class Utility
	{
		// Token: 0x06000F1E RID: 3870 RVA: 0x0002B90B File Offset: 0x00029B0B
		public static int GET_X_LPARAM(IntPtr lParam)
		{
			return Utility.LOWORD(lParam.ToInt32());
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x0002B919 File Offset: 0x00029B19
		public static int GET_Y_LPARAM(IntPtr lParam)
		{
			return Utility.HIWORD(lParam.ToInt32());
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x0002B927 File Offset: 0x00029B27
		public static int HIWORD(int i)
		{
			return (int)((short)(i >> 16));
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x0002B92E File Offset: 0x00029B2E
		public static int LOWORD(int i)
		{
			return (int)((short)(i & 65535));
		}
	}
}
