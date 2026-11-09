using System;
using System.Windows.Controls;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x0200002A RID: 42
	internal static class Extensions
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x000064A0 File Offset: 0x000046A0
		public static FlyoutBase Flyout(this Button button)
		{
			return FlyoutService.GetFlyout(button);
		}
	}
}
