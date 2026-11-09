using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ModernWpf.Controls
{
	// Token: 0x02000276 RID: 630
	public class BitmapIconSource : IconSource
	{
		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06001217 RID: 4631 RVA: 0x00034CF9 File Offset: 0x00032EF9
		// (set) Token: 0x06001218 RID: 4632 RVA: 0x00034D0B File Offset: 0x00032F0B
		public Uri UriSource
		{
			get
			{
				return (Uri)base.GetValue(BitmapIconSource.UriSourceProperty);
			}
			set
			{
				base.SetValue(BitmapIconSource.UriSourceProperty, value);
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06001219 RID: 4633 RVA: 0x00034D19 File Offset: 0x00032F19
		// (set) Token: 0x0600121A RID: 4634 RVA: 0x00034D2B File Offset: 0x00032F2B
		public bool ShowAsMonochrome
		{
			get
			{
				return (bool)base.GetValue(BitmapIconSource.ShowAsMonochromeProperty);
			}
			set
			{
				base.SetValue(BitmapIconSource.ShowAsMonochromeProperty, value);
			}
		}

		// Token: 0x040005CC RID: 1484
		public static readonly DependencyProperty UriSourceProperty = BitmapImage.UriSourceProperty.AddOwner(typeof(BitmapIconSource));

		// Token: 0x040005CD RID: 1485
		public static readonly DependencyProperty ShowAsMonochromeProperty = DependencyProperty.Register("ShowAsMonochrome", typeof(bool), typeof(BitmapIconSource), new PropertyMetadata(true));
	}
}
