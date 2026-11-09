using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x0200006D RID: 109
	public class NavigationViewTemplateSettings : DependencyObject
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x000135B6 File Offset: 0x000117B6
		// (set) Token: 0x0600053A RID: 1338 RVA: 0x000135C8 File Offset: 0x000117C8
		public double TopPadding
		{
			get
			{
				return (double)base.GetValue(NavigationViewTemplateSettings.TopPaddingProperty);
			}
			internal set
			{
				base.SetValue(NavigationViewTemplateSettings.TopPaddingPropertyKey, value);
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x000135DB File Offset: 0x000117DB
		// (set) Token: 0x0600053C RID: 1340 RVA: 0x000135ED File Offset: 0x000117ED
		public Visibility OverflowButtonVisibility
		{
			get
			{
				return (Visibility)base.GetValue(NavigationViewTemplateSettings.OverflowButtonVisibilityProperty);
			}
			internal set
			{
				base.SetValue(NavigationViewTemplateSettings.OverflowButtonVisibilityPropertyKey, value);
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x00013600 File Offset: 0x00011800
		// (set) Token: 0x0600053E RID: 1342 RVA: 0x00013612 File Offset: 0x00011812
		public Visibility PaneToggleButtonVisibility
		{
			get
			{
				return (Visibility)base.GetValue(NavigationViewTemplateSettings.PaneToggleButtonVisibilityProperty);
			}
			internal set
			{
				base.SetValue(NavigationViewTemplateSettings.PaneToggleButtonVisibilityPropertyKey, value);
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x00013625 File Offset: 0x00011825
		// (set) Token: 0x06000540 RID: 1344 RVA: 0x00013637 File Offset: 0x00011837
		public Visibility BackButtonVisibility
		{
			get
			{
				return (Visibility)base.GetValue(NavigationViewTemplateSettings.BackButtonVisibilityProperty);
			}
			internal set
			{
				base.SetValue(NavigationViewTemplateSettings.BackButtonVisibilityPropertyKey, value);
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x0001364A File Offset: 0x0001184A
		// (set) Token: 0x06000542 RID: 1346 RVA: 0x0001365C File Offset: 0x0001185C
		public Visibility TopPaneVisibility
		{
			get
			{
				return (Visibility)base.GetValue(NavigationViewTemplateSettings.TopPaneVisibilityProperty);
			}
			internal set
			{
				base.SetValue(NavigationViewTemplateSettings.TopPaneVisibilityPropertyKey, value);
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x0001366F File Offset: 0x0001186F
		// (set) Token: 0x06000544 RID: 1348 RVA: 0x00013681 File Offset: 0x00011881
		public Visibility LeftPaneVisibility
		{
			get
			{
				return (Visibility)base.GetValue(NavigationViewTemplateSettings.LeftPaneVisibilityProperty);
			}
			internal set
			{
				base.SetValue(NavigationViewTemplateSettings.LeftPaneVisibilityPropertyKey, value);
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x00013694 File Offset: 0x00011894
		// (set) Token: 0x06000546 RID: 1350 RVA: 0x000136A6 File Offset: 0x000118A6
		public bool SingleSelectionFollowsFocus
		{
			get
			{
				return (bool)base.GetValue(NavigationViewTemplateSettings.SingleSelectionFollowsFocusProperty);
			}
			internal set
			{
				base.SetValue(NavigationViewTemplateSettings.SingleSelectionFollowsFocusPropertyKey, value);
			}
		}

		// Token: 0x04000273 RID: 627
		private static readonly DependencyPropertyKey TopPaddingPropertyKey = DependencyProperty.RegisterReadOnly("TopPadding", typeof(double), typeof(NavigationViewTemplateSettings), new PropertyMetadata(0.0));

		// Token: 0x04000274 RID: 628
		public static readonly DependencyProperty TopPaddingProperty = NavigationViewTemplateSettings.TopPaddingPropertyKey.DependencyProperty;

		// Token: 0x04000275 RID: 629
		private static readonly DependencyPropertyKey OverflowButtonVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("OverflowButtonVisibility", typeof(Visibility), typeof(NavigationViewTemplateSettings), new PropertyMetadata(2));

		// Token: 0x04000276 RID: 630
		public static readonly DependencyProperty OverflowButtonVisibilityProperty = NavigationViewTemplateSettings.OverflowButtonVisibilityPropertyKey.DependencyProperty;

		// Token: 0x04000277 RID: 631
		private static readonly DependencyPropertyKey PaneToggleButtonVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("PaneToggleButtonVisibility", typeof(Visibility), typeof(NavigationViewTemplateSettings), new PropertyMetadata(0));

		// Token: 0x04000278 RID: 632
		public static readonly DependencyProperty PaneToggleButtonVisibilityProperty = NavigationViewTemplateSettings.PaneToggleButtonVisibilityPropertyKey.DependencyProperty;

		// Token: 0x04000279 RID: 633
		private static readonly DependencyPropertyKey BackButtonVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("BackButtonVisibility", typeof(Visibility), typeof(NavigationViewTemplateSettings), new PropertyMetadata(2));

		// Token: 0x0400027A RID: 634
		public static readonly DependencyProperty BackButtonVisibilityProperty = NavigationViewTemplateSettings.BackButtonVisibilityPropertyKey.DependencyProperty;

		// Token: 0x0400027B RID: 635
		private static readonly DependencyPropertyKey TopPaneVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("TopPaneVisibility", typeof(Visibility), typeof(NavigationViewTemplateSettings), new PropertyMetadata(2));

		// Token: 0x0400027C RID: 636
		public static readonly DependencyProperty TopPaneVisibilityProperty = NavigationViewTemplateSettings.TopPaneVisibilityPropertyKey.DependencyProperty;

		// Token: 0x0400027D RID: 637
		private static readonly DependencyPropertyKey LeftPaneVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("LeftPaneVisibility", typeof(Visibility), typeof(NavigationViewTemplateSettings), new PropertyMetadata(0));

		// Token: 0x0400027E RID: 638
		public static readonly DependencyProperty LeftPaneVisibilityProperty = NavigationViewTemplateSettings.LeftPaneVisibilityPropertyKey.DependencyProperty;

		// Token: 0x0400027F RID: 639
		private static readonly DependencyPropertyKey SingleSelectionFollowsFocusPropertyKey = DependencyProperty.RegisterReadOnly("SingleSelectionFollowsFocus", typeof(bool), typeof(NavigationViewTemplateSettings), null);

		// Token: 0x04000280 RID: 640
		public static readonly DependencyProperty SingleSelectionFollowsFocusProperty = NavigationViewTemplateSettings.SingleSelectionFollowsFocusPropertyKey.DependencyProperty;
	}
}
