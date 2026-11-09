using System;
using System.Windows;
using MS.Win32;

namespace MS.Internal
{
	// Token: 0x0200020A RID: 522
	internal static class PointUtil
	{
		// Token: 0x06000F2A RID: 3882 RVA: 0x0002B98C File Offset: 0x00029B8C
		internal static Rect ToRect(NativeMethods.RECT rc)
		{
			Rect result = default(Rect);
			result.X = (double)rc.left;
			result.Y = (double)rc.top;
			result.Width = (double)(rc.right - rc.left);
			result.Height = (double)(rc.bottom - rc.top);
			return result;
		}
	}
}
