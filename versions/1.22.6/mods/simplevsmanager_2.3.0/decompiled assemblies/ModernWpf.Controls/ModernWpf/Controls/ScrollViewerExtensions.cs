using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls
{
	// Token: 0x020000CC RID: 204
	internal static class ScrollViewerExtensions
	{
		// Token: 0x06000A21 RID: 2593 RVA: 0x000228C9 File Offset: 0x00020AC9
		public static UIElement GetContentTemplateRoot(this ScrollViewer scrollViewer)
		{
			return scrollViewer.Content as UIElement;
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x000228D6 File Offset: 0x00020AD6
		public static bool ChangeView(this ScrollViewer scrollViewer, double? horizontalOffset, double? verticalOffset, float? zoomFactor)
		{
			return scrollViewer.ChangeView(horizontalOffset, verticalOffset, zoomFactor, false);
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x000228E2 File Offset: 0x00020AE2
		public static bool ChangeView(this ScrollViewer scrollViewer, double? horizontalOffset, double? verticalOffset, float? zoomFactor, bool disableAnimation)
		{
			if (horizontalOffset != null)
			{
				scrollViewer.ScrollToHorizontalOffset(horizontalOffset.Value);
			}
			if (verticalOffset != null)
			{
				scrollViewer.ScrollToVerticalOffset(verticalOffset.Value);
			}
			return true;
		}
	}
}
