using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x02000283 RID: 643
	public static class CalendarHelper
	{
		// Token: 0x060012CA RID: 4810 RVA: 0x00037123 File Offset: 0x00035323
		public static bool GetAutoReleaseMouseCapture(Calendar calendar)
		{
			return (bool)calendar.GetValue(CalendarHelper.AutoReleaseMouseCaptureProperty);
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x00037135 File Offset: 0x00035335
		public static void SetAutoReleaseMouseCapture(Calendar calendar, bool value)
		{
			calendar.SetValue(CalendarHelper.AutoReleaseMouseCaptureProperty, value);
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x00037148 File Offset: 0x00035348
		private static void OnAutoReleaseMouseCaptureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Calendar calendar = (Calendar)d;
			if ((bool)e.NewValue)
			{
				calendar.GotMouseCapture += new MouseEventHandler(CalendarHelper.OnCalendarGotMouseCapture);
				return;
			}
			calendar.GotMouseCapture -= new MouseEventHandler(CalendarHelper.OnCalendarGotMouseCapture);
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x00037190 File Offset: 0x00035390
		private static void OnCalendarGotMouseCapture(object sender, MouseEventArgs e)
		{
			if (((Calendar)sender).SelectionMode != 2)
			{
				UIElement uielement = e.OriginalSource as UIElement;
				if (uielement is CalendarDayButton || uielement is CalendarItem)
				{
					uielement.ReleaseMouseCapture();
				}
			}
		}

		// Token: 0x04000625 RID: 1573
		public static readonly DependencyProperty AutoReleaseMouseCaptureProperty = DependencyProperty.RegisterAttached("AutoReleaseMouseCapture", typeof(bool), typeof(CalendarHelper), new PropertyMetadata(new PropertyChangedCallback(CalendarHelper.OnAutoReleaseMouseCaptureChanged)));
	}
}
