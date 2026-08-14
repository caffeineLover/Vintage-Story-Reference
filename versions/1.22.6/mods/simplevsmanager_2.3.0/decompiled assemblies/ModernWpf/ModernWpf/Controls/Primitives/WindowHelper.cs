using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020002AE RID: 686
	public static class WindowHelper
	{
		// Token: 0x060014C6 RID: 5318 RVA: 0x0003DF24 File Offset: 0x0003C124
		public static bool GetUseModernWindowStyle(Window window)
		{
			return (bool)window.GetValue(WindowHelper.UseModernWindowStyleProperty);
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x0003DF36 File Offset: 0x0003C136
		public static void SetUseModernWindowStyle(Window window, bool value)
		{
			window.SetValue(WindowHelper.UseModernWindowStyleProperty, value);
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x0003DF4C File Offset: 0x0003C14C
		private static void OnUseModernWindowStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			bool flag = (bool)e.NewValue;
			if (DesignerProperties.GetIsInDesignMode(d))
			{
				Control control = d as Control;
				if (control != null)
				{
					if (!flag)
					{
						control.ClearValue(FrameworkElement.StyleProperty);
						return;
					}
					Style style = control.TryFindResource("DefaultWindowStyle") as Style;
					if (style != null)
					{
						Style style2 = new Style();
						foreach (SetterBase setterBase in style.Setters)
						{
							Setter setter = (Setter)setterBase;
							if (setter.Property == Control.BackgroundProperty || setter.Property == Control.ForegroundProperty)
							{
								style2.Setters.Add(setter);
							}
						}
						control.Style = style2;
						return;
					}
				}
			}
			else
			{
				Window window = (Window)d;
				if (flag)
				{
					window.SetResourceReference(FrameworkElement.StyleProperty, "DefaultWindowStyle");
					return;
				}
				window.ClearValue(FrameworkElement.StyleProperty);
			}
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x0003E04C File Offset: 0x0003C24C
		[EditorBrowsable(1)]
		public static bool GetFixMaximizedWindow(Window window)
		{
			return (bool)window.GetValue(WindowHelper.FixMaximizedWindowProperty);
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0003E05E File Offset: 0x0003C25E
		[EditorBrowsable(1)]
		public static void SetFixMaximizedWindow(Window window, bool value)
		{
			window.SetValue(WindowHelper.FixMaximizedWindowProperty, value);
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0003E074 File Offset: 0x0003C274
		private static void OnFixMaximizedWindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;
			if (window != null)
			{
				if ((bool)e.NewValue)
				{
					MaximizedWindowFixer.SetMaximizedWindowFixer(window, new MaximizedWindowFixer());
					return;
				}
				window.ClearValue(MaximizedWindowFixer.MaximizedWindowFixerProperty);
			}
		}

		// Token: 0x040006F9 RID: 1785
		private const string DefaultWindowStyleKey = "DefaultWindowStyle";

		// Token: 0x040006FA RID: 1786
		public static readonly DependencyProperty UseModernWindowStyleProperty = DependencyProperty.RegisterAttached("UseModernWindowStyle", typeof(bool), typeof(WindowHelper), new PropertyMetadata(new PropertyChangedCallback(WindowHelper.OnUseModernWindowStyleChanged)));

		// Token: 0x040006FB RID: 1787
		[EditorBrowsable(1)]
		public static readonly DependencyProperty FixMaximizedWindowProperty = DependencyProperty.RegisterAttached("FixMaximizedWindow", typeof(bool), typeof(WindowHelper), new PropertyMetadata(false, new PropertyChangedCallback(WindowHelper.OnFixMaximizedWindowChanged)));
	}
}
