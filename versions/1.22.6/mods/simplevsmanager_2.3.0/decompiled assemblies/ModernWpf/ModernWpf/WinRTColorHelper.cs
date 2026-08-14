using System;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Windows.UI;

namespace ModernWpf
{
	// Token: 0x02000224 RID: 548
	internal static class WinRTColorHelper
	{
		// Token: 0x06000FD9 RID: 4057 RVA: 0x0002D40C File Offset: 0x0002B60C
		[MethodImpl(8)]
		public static Color ToColor(this Color color)
		{
			return Color.FromArgb(color.A, color.R, color.G, color.B);
		}
	}
}
