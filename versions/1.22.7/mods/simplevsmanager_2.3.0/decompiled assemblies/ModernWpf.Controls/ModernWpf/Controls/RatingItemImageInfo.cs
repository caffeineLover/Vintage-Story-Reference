using System;
using System.Windows;
using System.Windows.Media;

namespace ModernWpf.Controls
{
	// Token: 0x02000088 RID: 136
	public class RatingItemImageInfo : RatingItemInfo
	{
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x0001964D File Offset: 0x0001784D
		// (set) Token: 0x060006F0 RID: 1776 RVA: 0x0001965F File Offset: 0x0001785F
		public ImageSource DisabledImage
		{
			get
			{
				return (ImageSource)base.GetValue(RatingItemImageInfo.DisabledImageProperty);
			}
			set
			{
				base.SetValue(RatingItemImageInfo.DisabledImageProperty, value);
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0001966D File Offset: 0x0001786D
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x0001967F File Offset: 0x0001787F
		public ImageSource Image
		{
			get
			{
				return (ImageSource)base.GetValue(RatingItemImageInfo.ImageProperty);
			}
			set
			{
				base.SetValue(RatingItemImageInfo.ImageProperty, value);
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x0001968D File Offset: 0x0001788D
		// (set) Token: 0x060006F4 RID: 1780 RVA: 0x0001969F File Offset: 0x0001789F
		public ImageSource PlaceholderImage
		{
			get
			{
				return (ImageSource)base.GetValue(RatingItemImageInfo.PlaceholderImageProperty);
			}
			set
			{
				base.SetValue(RatingItemImageInfo.PlaceholderImageProperty, value);
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x000196AD File Offset: 0x000178AD
		// (set) Token: 0x060006F6 RID: 1782 RVA: 0x000196BF File Offset: 0x000178BF
		public ImageSource PointerOverImage
		{
			get
			{
				return (ImageSource)base.GetValue(RatingItemImageInfo.PointerOverImageProperty);
			}
			set
			{
				base.SetValue(RatingItemImageInfo.PointerOverImageProperty, value);
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x000196CD File Offset: 0x000178CD
		// (set) Token: 0x060006F8 RID: 1784 RVA: 0x000196DF File Offset: 0x000178DF
		public ImageSource PointerOverPlaceholderImage
		{
			get
			{
				return (ImageSource)base.GetValue(RatingItemImageInfo.PointerOverPlaceholderImageProperty);
			}
			set
			{
				base.SetValue(RatingItemImageInfo.PointerOverPlaceholderImageProperty, value);
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x000196ED File Offset: 0x000178ED
		// (set) Token: 0x060006FA RID: 1786 RVA: 0x000196FF File Offset: 0x000178FF
		public ImageSource UnsetImage
		{
			get
			{
				return (ImageSource)base.GetValue(RatingItemImageInfo.UnsetImageProperty);
			}
			set
			{
				base.SetValue(RatingItemImageInfo.UnsetImageProperty, value);
			}
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0001970D File Offset: 0x0001790D
		protected override Freezable CreateInstanceCore()
		{
			return new RatingItemImageInfo();
		}

		// Token: 0x0400032C RID: 812
		public static readonly DependencyProperty DisabledImageProperty = DependencyProperty.Register("DisabledImage", typeof(ImageSource), typeof(RatingItemImageInfo), null);

		// Token: 0x0400032D RID: 813
		public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(RatingItemImageInfo), null);

		// Token: 0x0400032E RID: 814
		public static readonly DependencyProperty PlaceholderImageProperty = DependencyProperty.Register("PlaceholderImage", typeof(ImageSource), typeof(RatingItemImageInfo), null);

		// Token: 0x0400032F RID: 815
		public static readonly DependencyProperty PointerOverImageProperty = DependencyProperty.Register("PointerOverImage", typeof(ImageSource), typeof(RatingItemImageInfo), null);

		// Token: 0x04000330 RID: 816
		public static readonly DependencyProperty PointerOverPlaceholderImageProperty = DependencyProperty.Register("PointerOverPlaceholderImage", typeof(ImageSource), typeof(RatingItemImageInfo), null);

		// Token: 0x04000331 RID: 817
		public static readonly DependencyProperty UnsetImageProperty = DependencyProperty.Register("UnsetImage", typeof(ImageSource), typeof(RatingItemImageInfo), null);
	}
}
