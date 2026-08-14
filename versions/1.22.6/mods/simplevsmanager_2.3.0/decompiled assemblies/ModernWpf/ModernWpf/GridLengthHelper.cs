using System;
using System.Windows;

namespace ModernWpf
{
	// Token: 0x02000217 RID: 535
	internal static class GridLengthHelper
	{
		// Token: 0x06000FAA RID: 4010 RVA: 0x0002CB8D File Offset: 0x0002AD8D
		public static GridLength FromPixels(double pixels)
		{
			return new GridLength(pixels);
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x0002CB95 File Offset: 0x0002AD95
		public static GridLength FromValueAndType(double value, GridUnitType type)
		{
			return new GridLength(value, type);
		}
	}
}
