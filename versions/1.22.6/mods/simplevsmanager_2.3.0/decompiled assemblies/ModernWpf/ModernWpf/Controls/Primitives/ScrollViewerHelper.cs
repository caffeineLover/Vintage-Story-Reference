using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020002A3 RID: 675
	public static class ScrollViewerHelper
	{
		// Token: 0x06001437 RID: 5175 RVA: 0x0003C0D0 File Offset: 0x0003A2D0
		public static bool GetIsEnabled(ScrollViewer scrollViewer)
		{
			return (bool)scrollViewer.GetValue(ScrollViewerHelper.IsEnabledProperty);
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x0003C0E2 File Offset: 0x0003A2E2
		public static void SetIsEnabled(ScrollViewer scrollViewer, bool value)
		{
			scrollViewer.SetValue(ScrollViewerHelper.IsEnabledProperty, value);
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x0003C0F8 File Offset: 0x0003A2F8
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ScrollViewer scrollViewer = (ScrollViewer)d;
			if ((bool)e.NewValue)
			{
				scrollViewer.Loaded += new RoutedEventHandler(ScrollViewerHelper.OnLoaded);
				return;
			}
			scrollViewer.Loaded -= new RoutedEventHandler(ScrollViewerHelper.OnLoaded);
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0003C13F File Offset: 0x0003A33F
		public static bool GetAutoHideScrollBars(DependencyObject element)
		{
			return (bool)element.GetValue(ScrollViewerHelper.AutoHideScrollBarsProperty);
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x0003C151 File Offset: 0x0003A351
		public static void SetAutoHideScrollBars(DependencyObject element, bool value)
		{
			element.SetValue(ScrollViewerHelper.AutoHideScrollBarsProperty, value);
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x0003C164 File Offset: 0x0003A364
		private static void OnAutoHideScrollBarsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ScrollViewer scrollViewer = d as ScrollViewer;
			if (scrollViewer != null)
			{
				ScrollViewerHelper.UpdateVisualState(scrollViewer, true);
			}
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x0003C182 File Offset: 0x0003A382
		private static void OnLoaded(object sender, RoutedEventArgs e)
		{
			ScrollViewer scrollViewer = (ScrollViewer)sender;
			scrollViewer.ApplyTemplate();
			ScrollViewerHelper.UpdateVisualState(scrollViewer, false);
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x0003C198 File Offset: 0x0003A398
		private static void UpdateVisualState(ScrollViewer sv, bool useTransitions = true)
		{
			string text = ScrollViewerHelper.GetAutoHideScrollBars(sv) ? "NoIndicator" : "MouseIndicator";
			VisualStateManager.GoToState(sv, text, useTransitions);
		}

		// Token: 0x040006C8 RID: 1736
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(ScrollViewerHelper), new PropertyMetadata(false, new PropertyChangedCallback(ScrollViewerHelper.OnIsEnabledChanged)));

		// Token: 0x040006C9 RID: 1737
		public static readonly DependencyProperty AutoHideScrollBarsProperty = DependencyProperty.RegisterAttached("AutoHideScrollBars", typeof(bool), typeof(ScrollViewerHelper), new PropertyMetadata(false, new PropertyChangedCallback(ScrollViewerHelper.OnAutoHideScrollBarsChanged)));
	}
}
