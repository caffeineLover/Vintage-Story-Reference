using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020002A0 RID: 672
	public static class ScrollBarHelper
	{
		// Token: 0x06001425 RID: 5157 RVA: 0x0003BD8B File Offset: 0x00039F8B
		public static bool GetIsEnabled(ScrollBar scrollBar)
		{
			return (bool)scrollBar.GetValue(ScrollBarHelper.IsEnabledProperty);
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x0003BD9D File Offset: 0x00039F9D
		public static void SetIsEnabled(ScrollBar scrollBar, bool value)
		{
			scrollBar.SetValue(ScrollBarHelper.IsEnabledProperty, value);
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x0003BDB0 File Offset: 0x00039FB0
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ScrollBar scrollBar = (ScrollBar)d;
			bool flag = (bool)e.OldValue;
			if ((bool)e.NewValue)
			{
				scrollBar.IsVisibleChanged += new DependencyPropertyChangedEventHandler(ScrollBarHelper.OnIsVisibleChanged);
				scrollBar.MouseEnter += new MouseEventHandler(ScrollBarHelper.OnIsMouseOverChanged);
				scrollBar.MouseLeave += new MouseEventHandler(ScrollBarHelper.OnIsMouseOverChanged);
				scrollBar.IsEnabledChanged += new DependencyPropertyChangedEventHandler(ScrollBarHelper.OnIsEnabledChanged);
				if (scrollBar.IsLoaded)
				{
					ScrollBarHelper.UpdateVisualState(scrollBar, true);
					return;
				}
			}
			else
			{
				scrollBar.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(ScrollBarHelper.OnIsVisibleChanged);
				scrollBar.MouseEnter -= new MouseEventHandler(ScrollBarHelper.OnIsMouseOverChanged);
				scrollBar.MouseLeave -= new MouseEventHandler(ScrollBarHelper.OnIsMouseOverChanged);
				scrollBar.IsEnabledChanged -= new DependencyPropertyChangedEventHandler(ScrollBarHelper.OnIsEnabledChanged);
			}
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x0003BE7F File Offset: 0x0003A07F
		public static ScrollingIndicatorMode GetIndicatorMode(ScrollBar scrollBar)
		{
			return (ScrollingIndicatorMode)scrollBar.GetValue(ScrollBarHelper.IndicatorModeProperty);
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x0003BE91 File Offset: 0x0003A091
		public static void SetIndicatorMode(ScrollBar scrollBar, ScrollingIndicatorMode value)
		{
			scrollBar.SetValue(ScrollBarHelper.IndicatorModeProperty, value);
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x0003BEA4 File Offset: 0x0003A0A4
		private static void OnIndicatorModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ScrollBarHelper.UpdateVisualState((ScrollBar)d, true);
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0003BEB2 File Offset: 0x0003A0B2
		public static Color? GetCollapsedThumbBackgroundColor(ScrollBar scrollBar)
		{
			return (Color?)scrollBar.GetValue(ScrollBarHelper.CollapsedThumbBackgroundColorProperty);
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x0003BEC4 File Offset: 0x0003A0C4
		public static void SetCollapsedThumbBackgroundColor(ScrollBar scrollBar, Color? value)
		{
			scrollBar.SetValue(ScrollBarHelper.CollapsedThumbBackgroundColorProperty, value);
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0003BED7 File Offset: 0x0003A0D7
		public static Color? GetExpandedThumbBackgroundColor(ScrollBar scrollBar)
		{
			return (Color?)scrollBar.GetValue(ScrollBarHelper.ExpandedThumbBackgroundColorProperty);
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x0003BEE9 File Offset: 0x0003A0E9
		public static void SetExpandedThumbBackgroundColor(ScrollBar scrollBar, Color? value)
		{
			scrollBar.SetValue(ScrollBarHelper.ExpandedThumbBackgroundColorProperty, value);
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x0003BEFC File Offset: 0x0003A0FC
		private static void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				ScrollBar scrollBar = (ScrollBar)sender;
				scrollBar.ApplyTemplate();
				ScrollBarHelper.UpdateVisualState(scrollBar, false);
			}
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x0003BF20 File Offset: 0x0003A120
		private static void OnIsMouseOverChanged(object sender, MouseEventArgs e)
		{
			ScrollBar scrollBar = (ScrollBar)sender;
			if (scrollBar.IsEnabled)
			{
				ScrollBarHelper.UpdateVisualState(scrollBar, true);
			}
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x0003BF43 File Offset: 0x0003A143
		private static void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			ScrollBarHelper.UpdateVisualState((ScrollBar)sender, true);
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x0003BF54 File Offset: 0x0003A154
		private static void UpdateVisualState(ScrollBar scrollBar, bool useTransitions = true)
		{
			string text;
			if (scrollBar.IsEnabled)
			{
				if (ScrollBarHelper.GetIndicatorMode(scrollBar) != ScrollingIndicatorMode.MouseIndicator)
				{
					text = (scrollBar.IsMouseOver ? "Expanded" : "Collapsed");
				}
				else
				{
					text = "Expanded";
				}
			}
			else
			{
				text = "Collapsed";
				useTransitions = false;
			}
			VisualStateManager.GoToState(scrollBar, text, useTransitions);
		}

		// Token: 0x040006BD RID: 1725
		private const string StateExpanded = "Expanded";

		// Token: 0x040006BE RID: 1726
		private const string StateCollapsed = "Collapsed";

		// Token: 0x040006BF RID: 1727
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(ScrollBarHelper), new PropertyMetadata(false, new PropertyChangedCallback(ScrollBarHelper.OnIsEnabledChanged)));

		// Token: 0x040006C0 RID: 1728
		public static readonly DependencyProperty IndicatorModeProperty = DependencyProperty.RegisterAttached("IndicatorMode", typeof(ScrollingIndicatorMode), typeof(ScrollBarHelper), new PropertyMetadata(ScrollingIndicatorMode.MouseIndicator, new PropertyChangedCallback(ScrollBarHelper.OnIndicatorModeChanged)));

		// Token: 0x040006C1 RID: 1729
		public static readonly DependencyProperty CollapsedThumbBackgroundColorProperty = DependencyProperty.RegisterAttached("CollapsedThumbBackgroundColor", typeof(Color?), typeof(ScrollBarHelper), new PropertyMetadata(null));

		// Token: 0x040006C2 RID: 1730
		public static readonly DependencyProperty ExpandedThumbBackgroundColorProperty = DependencyProperty.RegisterAttached("ExpandedThumbBackgroundColor", typeof(Color?), typeof(ScrollBarHelper), new PropertyMetadata(null));
	}
}
