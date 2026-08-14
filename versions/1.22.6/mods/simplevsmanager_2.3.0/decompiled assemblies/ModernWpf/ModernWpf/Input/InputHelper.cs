using System;
using System.Windows;
using System.Windows.Input;

namespace ModernWpf.Input
{
	// Token: 0x0200025D RID: 605
	internal static class InputHelper
	{
		// Token: 0x06001144 RID: 4420 RVA: 0x00032E90 File Offset: 0x00031090
		public static bool GetIsTapEnabled(UIElement element)
		{
			return (bool)element.GetValue(InputHelper.IsTapEnabledProperty);
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x00032EA2 File Offset: 0x000310A2
		public static void SetIsTapEnabled(UIElement element, bool value)
		{
			element.SetValue(InputHelper.IsTapEnabledProperty, value);
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x00032EB8 File Offset: 0x000310B8
		private static void OnIsTapEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			UIElement uielement = (UIElement)d;
			bool flag = (bool)e.OldValue;
			if ((bool)e.NewValue)
			{
				uielement.MouseLeftButtonDown += new MouseButtonEventHandler(InputHelper.OnMouseLeftButtonDown);
				uielement.MouseLeftButtonUp += new MouseButtonEventHandler(InputHelper.OnMouseLeftButtonUp);
				uielement.LostMouseCapture += new MouseEventHandler(InputHelper.OnLostMouseCapture);
				uielement.MouseLeave += new MouseEventHandler(InputHelper.OnMouseLeave);
				return;
			}
			uielement.MouseLeftButtonDown -= new MouseButtonEventHandler(InputHelper.OnMouseLeftButtonDown);
			uielement.MouseLeftButtonUp -= new MouseButtonEventHandler(InputHelper.OnMouseLeftButtonUp);
			uielement.LostMouseCapture -= new MouseEventHandler(InputHelper.OnLostMouseCapture);
			uielement.MouseLeave -= new MouseEventHandler(InputHelper.OnMouseLeave);
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x00032F78 File Offset: 0x00031178
		private static bool GetIsPressed(UIElement element)
		{
			return (bool)element.GetValue(InputHelper.IsPressedProperty);
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x00032F8A File Offset: 0x0003118A
		private static void SetIsPressed(UIElement element, bool value)
		{
			if (value)
			{
				element.SetValue(InputHelper.IsPressedProperty, value);
				return;
			}
			element.ClearValue(InputHelper.IsPressedProperty);
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x00032FAC File Offset: 0x000311AC
		public static void AddTappedHandler(UIElement element, TappedEventHandler handler)
		{
			element.AddHandler(InputHelper.TappedEvent, handler);
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x00032FBA File Offset: 0x000311BA
		public static void RemoveTappedHandler(UIElement element, TappedEventHandler handler)
		{
			element.RemoveHandler(InputHelper.TappedEvent, handler);
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x00032FC8 File Offset: 0x000311C8
		private static void RaiseTapped(UIElement element, int timestamp)
		{
			TappedRoutedEventArgs tappedRoutedEventArgs = new TappedRoutedEventArgs
			{
				RoutedEvent = InputHelper.TappedEvent,
				Source = element,
				Timestamp = timestamp
			};
			InputHelper._lastTappedArgs = tappedRoutedEventArgs;
			element.RaiseEvent(tappedRoutedEventArgs);
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x00033004 File Offset: 0x00031204
		private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			UIElement element = (UIElement)sender;
			if (!InputHelper.GetIsPressed(element))
			{
				InputHelper.SetIsPressed(element, true);
			}
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x00033028 File Offset: 0x00031228
		private static void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			UIElement uielement = (UIElement)sender;
			if (InputHelper.GetIsPressed(uielement))
			{
				InputHelper.SetIsPressed((UIElement)sender, false);
				TappedRoutedEventArgs lastTappedArgs = InputHelper._lastTappedArgs;
				if (lastTappedArgs == null || !lastTappedArgs.Handled || lastTappedArgs.Timestamp != e.Timestamp)
				{
					Rect rect;
					rect..ctor(default(Point), uielement.RenderSize);
					if (rect.Contains(e.GetPosition(uielement)))
					{
						InputHelper.RaiseTapped(uielement, e.Timestamp);
					}
				}
			}
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x000330A1 File Offset: 0x000312A1
		private static void OnLostMouseCapture(object sender, MouseEventArgs e)
		{
			InputHelper.SetIsPressed((UIElement)sender, false);
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x000330AF File Offset: 0x000312AF
		private static void OnMouseLeave(object sender, MouseEventArgs e)
		{
			InputHelper.SetIsPressed((UIElement)sender, false);
		}

		// Token: 0x040004C0 RID: 1216
		public static readonly DependencyProperty IsTapEnabledProperty = DependencyProperty.RegisterAttached("IsTapEnabled", typeof(bool), typeof(InputHelper), new PropertyMetadata(false, new PropertyChangedCallback(InputHelper.OnIsTapEnabledChanged)));

		// Token: 0x040004C1 RID: 1217
		public static readonly DependencyProperty IsPressedProperty = DependencyProperty.RegisterAttached("IsPressed", typeof(bool), typeof(InputHelper), new PropertyMetadata(false));

		// Token: 0x040004C2 RID: 1218
		public static readonly RoutedEvent TappedEvent = EventManager.RegisterRoutedEvent("Tapped", 1, typeof(TappedEventHandler), typeof(InputHelper));

		// Token: 0x040004C3 RID: 1219
		private static TappedRoutedEventArgs _lastTappedArgs;
	}
}
