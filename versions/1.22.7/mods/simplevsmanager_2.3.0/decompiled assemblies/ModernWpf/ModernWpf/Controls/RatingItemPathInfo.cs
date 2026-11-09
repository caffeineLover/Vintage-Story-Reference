using System;
using System.Windows;
using System.Windows.Media;

namespace ModernWpf.Controls
{
	// Token: 0x0200026A RID: 618
	public class RatingItemPathInfo : RatingItemInfo
	{
		// Token: 0x1700037B RID: 891
		// (get) Token: 0x0600119D RID: 4509 RVA: 0x00033986 File Offset: 0x00031B86
		// (set) Token: 0x0600119E RID: 4510 RVA: 0x00033998 File Offset: 0x00031B98
		public Geometry DisabledData
		{
			get
			{
				return (Geometry)base.GetValue(RatingItemPathInfo.DisabledDataProperty);
			}
			set
			{
				base.SetValue(RatingItemPathInfo.DisabledDataProperty, value);
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x0600119F RID: 4511 RVA: 0x000339A6 File Offset: 0x00031BA6
		// (set) Token: 0x060011A0 RID: 4512 RVA: 0x000339B8 File Offset: 0x00031BB8
		public Geometry Data
		{
			get
			{
				return (Geometry)base.GetValue(RatingItemPathInfo.DataProperty);
			}
			set
			{
				base.SetValue(RatingItemPathInfo.DataProperty, value);
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x060011A1 RID: 4513 RVA: 0x000339C6 File Offset: 0x00031BC6
		// (set) Token: 0x060011A2 RID: 4514 RVA: 0x000339D8 File Offset: 0x00031BD8
		public Geometry PlaceholderData
		{
			get
			{
				return (Geometry)base.GetValue(RatingItemPathInfo.PlaceholderDataProperty);
			}
			set
			{
				base.SetValue(RatingItemPathInfo.PlaceholderDataProperty, value);
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060011A3 RID: 4515 RVA: 0x000339E6 File Offset: 0x00031BE6
		// (set) Token: 0x060011A4 RID: 4516 RVA: 0x000339F8 File Offset: 0x00031BF8
		public Geometry PointerOverData
		{
			get
			{
				return (Geometry)base.GetValue(RatingItemPathInfo.PointerOverDataProperty);
			}
			set
			{
				base.SetValue(RatingItemPathInfo.PointerOverDataProperty, value);
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060011A5 RID: 4517 RVA: 0x00033A06 File Offset: 0x00031C06
		// (set) Token: 0x060011A6 RID: 4518 RVA: 0x00033A18 File Offset: 0x00031C18
		public Geometry PointerOverPlaceholderData
		{
			get
			{
				return (Geometry)base.GetValue(RatingItemPathInfo.PointerOverPlaceholderDataProperty);
			}
			set
			{
				base.SetValue(RatingItemPathInfo.PointerOverPlaceholderDataProperty, value);
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x060011A7 RID: 4519 RVA: 0x00033A26 File Offset: 0x00031C26
		// (set) Token: 0x060011A8 RID: 4520 RVA: 0x00033A38 File Offset: 0x00031C38
		public Geometry UnsetData
		{
			get
			{
				return (Geometry)base.GetValue(RatingItemPathInfo.UnsetDataProperty);
			}
			set
			{
				base.SetValue(RatingItemPathInfo.UnsetDataProperty, value);
			}
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x00033A46 File Offset: 0x00031C46
		protected override Freezable CreateInstanceCore()
		{
			return new RatingItemPathInfo();
		}

		// Token: 0x040004DF RID: 1247
		public static readonly DependencyProperty DisabledDataProperty = DependencyProperty.Register("DisabledData", typeof(Geometry), typeof(RatingItemPathInfo), null);

		// Token: 0x040004E0 RID: 1248
		public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(Geometry), typeof(RatingItemPathInfo), null);

		// Token: 0x040004E1 RID: 1249
		public static readonly DependencyProperty PlaceholderDataProperty = DependencyProperty.Register("PlaceholderData", typeof(Geometry), typeof(RatingItemPathInfo), null);

		// Token: 0x040004E2 RID: 1250
		public static readonly DependencyProperty PointerOverDataProperty = DependencyProperty.Register("PointerOverData", typeof(Geometry), typeof(RatingItemPathInfo), null);

		// Token: 0x040004E3 RID: 1251
		public static readonly DependencyProperty PointerOverPlaceholderDataProperty = DependencyProperty.Register("PointerOverPlaceholderData", typeof(Geometry), typeof(RatingItemPathInfo), null);

		// Token: 0x040004E4 RID: 1252
		public static readonly DependencyProperty UnsetDataProperty = DependencyProperty.Register("UnsetData", typeof(Geometry), typeof(RatingItemPathInfo), null);
	}
}
