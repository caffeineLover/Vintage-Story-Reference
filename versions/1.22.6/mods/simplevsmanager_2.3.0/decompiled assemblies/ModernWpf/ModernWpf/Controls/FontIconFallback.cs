using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModernWpf.Controls
{
	// Token: 0x02000264 RID: 612
	[EditorBrowsable(1)]
	public class FontIconFallback : Control
	{
		// Token: 0x0600117F RID: 4479 RVA: 0x00033634 File Offset: 0x00031834
		static FontIconFallback()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FontIconFallback), new FrameworkPropertyMetadata(typeof(FontIconFallback)));
			UIElement.FocusableProperty.OverrideMetadata(typeof(FontIconFallback), new FrameworkPropertyMetadata(false));
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001180 RID: 4480 RVA: 0x000336A7 File Offset: 0x000318A7
		// (set) Token: 0x06001181 RID: 4481 RVA: 0x000336B9 File Offset: 0x000318B9
		public Geometry Data
		{
			get
			{
				return (Geometry)base.GetValue(FontIconFallback.DataProperty);
			}
			set
			{
				base.SetValue(FontIconFallback.DataProperty, value);
			}
		}

		// Token: 0x040004D2 RID: 1234
		public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(Geometry), typeof(FontIconFallback), null);
	}
}
