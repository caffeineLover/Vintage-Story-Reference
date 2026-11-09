using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ModernWpf.Controls
{
	// Token: 0x0200026F RID: 623
	public class BitmapIcon : IconElement
	{
		// Token: 0x060011BE RID: 4542 RVA: 0x00033FE8 File Offset: 0x000321E8
		static BitmapIcon()
		{
			IconElement.ForegroundProperty.OverrideMetadata(typeof(BitmapIcon), new FrameworkPropertyMetadata(new PropertyChangedCallback(BitmapIcon.OnForegroundChanged)));
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x060011C0 RID: 4544 RVA: 0x00034086 File Offset: 0x00032286
		// (set) Token: 0x060011C1 RID: 4545 RVA: 0x00034098 File Offset: 0x00032298
		public Uri UriSource
		{
			get
			{
				return (Uri)base.GetValue(BitmapIcon.UriSourceProperty);
			}
			set
			{
				base.SetValue(BitmapIcon.UriSourceProperty, value);
			}
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x000340A6 File Offset: 0x000322A6
		private static void OnUriSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((BitmapIcon)d).ApplyUriSource();
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x060011C3 RID: 4547 RVA: 0x000340B3 File Offset: 0x000322B3
		// (set) Token: 0x060011C4 RID: 4548 RVA: 0x000340C5 File Offset: 0x000322C5
		public bool ShowAsMonochrome
		{
			get
			{
				return (bool)base.GetValue(BitmapIcon.ShowAsMonochromeProperty);
			}
			set
			{
				base.SetValue(BitmapIcon.ShowAsMonochromeProperty, value);
			}
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x000340D8 File Offset: 0x000322D8
		private static void OnShowAsMonochromeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((BitmapIcon)d).ApplyShowAsMonochrome();
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x000340E8 File Offset: 0x000322E8
		private protected override void InitializeChildren()
		{
			this._image = new Image
			{
				Visibility = 1
			};
			this._opacityMask = new ImageBrush();
			this._foreground = new Rectangle
			{
				OpacityMask = this._opacityMask
			};
			this.ApplyForeground();
			this.ApplyUriSource();
			base.Children.Add(this._image);
			this.ApplyShowAsMonochrome();
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x0003414D File Offset: 0x0003234D
		private protected override void OnShouldInheritForegroundFromVisualParentChanged()
		{
			this.ApplyForeground();
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x00034155 File Offset: 0x00032355
		private protected override void OnVisualParentForegroundPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			if (base.ShouldInheritForegroundFromVisualParent)
			{
				this.ApplyForeground();
			}
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x00034165 File Offset: 0x00032365
		private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((BitmapIcon)d).ApplyForeground();
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x00034172 File Offset: 0x00032372
		private void ApplyForeground()
		{
			if (this._foreground != null)
			{
				this._foreground.Fill = (base.ShouldInheritForegroundFromVisualParent ? base.VisualParentForeground : base.Foreground);
			}
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x000341A0 File Offset: 0x000323A0
		private void ApplyUriSource()
		{
			if (this._image != null && this._opacityMask != null)
			{
				Uri uriSource = this.UriSource;
				if (uriSource != null)
				{
					BitmapImage bitmapImage = new BitmapImage(uriSource);
					this._image.Source = bitmapImage;
					this._opacityMask.ImageSource = bitmapImage;
					return;
				}
				this._image.ClearValue(Image.SourceProperty);
				this._opacityMask.ClearValue(ImageBrush.ImageSourceProperty);
			}
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00034210 File Offset: 0x00032410
		private void ApplyShowAsMonochrome()
		{
			bool showAsMonochrome = this.ShowAsMonochrome;
			if (this._image != null)
			{
				this._image.Visibility = (showAsMonochrome ? 1 : 0);
			}
			if (this._foreground != null)
			{
				if (showAsMonochrome)
				{
					if (!base.Children.Contains(this._foreground))
					{
						base.Children.Add(this._foreground);
						return;
					}
				}
				else
				{
					base.Children.Remove(this._foreground);
				}
			}
		}

		// Token: 0x040004F1 RID: 1265
		public static readonly DependencyProperty UriSourceProperty = BitmapImage.UriSourceProperty.AddOwner(typeof(BitmapIcon), new FrameworkPropertyMetadata(new PropertyChangedCallback(BitmapIcon.OnUriSourceChanged)));

		// Token: 0x040004F2 RID: 1266
		public static readonly DependencyProperty ShowAsMonochromeProperty = DependencyProperty.Register("ShowAsMonochrome", typeof(bool), typeof(BitmapIcon), new PropertyMetadata(true, new PropertyChangedCallback(BitmapIcon.OnShowAsMonochromeChanged)));

		// Token: 0x040004F3 RID: 1267
		private Image _image;

		// Token: 0x040004F4 RID: 1268
		private Rectangle _foreground;

		// Token: 0x040004F5 RID: 1269
		private ImageBrush _opacityMask;
	}
}
