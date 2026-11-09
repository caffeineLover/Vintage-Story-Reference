using System;
using System.Windows;

namespace ModernWpf
{
	// Token: 0x02000222 RID: 546
	internal static class ThicknessHelper
	{
		// Token: 0x06000FCF RID: 4047 RVA: 0x0002D214 File Offset: 0x0002B414
		public static Thickness FromLengths(double left, double top, double right, double bottom)
		{
			return new Thickness(left, top, right, bottom);
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0002D21F File Offset: 0x0002B41F
		public static Thickness FromUniformLength(double uniformLength)
		{
			return new Thickness(uniformLength);
		}
	}
}
