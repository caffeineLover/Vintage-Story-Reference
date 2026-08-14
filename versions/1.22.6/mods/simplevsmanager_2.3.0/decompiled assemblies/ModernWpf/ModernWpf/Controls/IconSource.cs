using System;
using System.Windows;
using System.Windows.Media;

namespace ModernWpf.Controls
{
	// Token: 0x02000278 RID: 632
	public class IconSource : DependencyObject
	{
		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06001228 RID: 4648 RVA: 0x00034F52 File Offset: 0x00033152
		// (set) Token: 0x06001229 RID: 4649 RVA: 0x00034F64 File Offset: 0x00033164
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(IconSource.ForegroundProperty);
			}
			set
			{
				base.SetValue(IconSource.ForegroundProperty, value);
			}
		}

		// Token: 0x040005D4 RID: 1492
		public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Brush), typeof(IconSource));
	}
}
