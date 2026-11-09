using System;
using System.Windows;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020000F3 RID: 243
	public sealed class SplitViewTemplateSettings : DependencyObject
	{
		// Token: 0x06000C83 RID: 3203 RVA: 0x0002AC84 File Offset: 0x00028E84
		internal SplitViewTemplateSettings()
		{
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x0002AC8C File Offset: 0x00028E8C
		// (set) Token: 0x06000C85 RID: 3205 RVA: 0x0002AC9E File Offset: 0x00028E9E
		public GridLength CompactPaneGridLength
		{
			get
			{
				return (GridLength)base.GetValue(SplitViewTemplateSettings.CompactPaneGridLengthProperty);
			}
			internal set
			{
				base.SetValue(SplitViewTemplateSettings.CompactPaneGridLengthPropertyKey, value);
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x0002ACB1 File Offset: 0x00028EB1
		// (set) Token: 0x06000C87 RID: 3207 RVA: 0x0002ACC3 File Offset: 0x00028EC3
		public double NegativeOpenPaneLength
		{
			get
			{
				return (double)base.GetValue(SplitViewTemplateSettings.NegativeOpenPaneLengthProperty);
			}
			internal set
			{
				base.SetValue(SplitViewTemplateSettings.NegativeOpenPaneLengthPropertyKey, value);
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x0002ACD6 File Offset: 0x00028ED6
		// (set) Token: 0x06000C89 RID: 3209 RVA: 0x0002ACE8 File Offset: 0x00028EE8
		public double NegativeOpenPaneLengthMinusCompactLength
		{
			get
			{
				return (double)base.GetValue(SplitViewTemplateSettings.NegativeOpenPaneLengthMinusCompactLengthProperty);
			}
			internal set
			{
				base.SetValue(SplitViewTemplateSettings.NegativeOpenPaneLengthMinusCompactLengthPropertyKey, value);
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000C8A RID: 3210 RVA: 0x0002ACFB File Offset: 0x00028EFB
		// (set) Token: 0x06000C8B RID: 3211 RVA: 0x0002AD0D File Offset: 0x00028F0D
		public GridLength OpenPaneGridLength
		{
			get
			{
				return (GridLength)base.GetValue(SplitViewTemplateSettings.OpenPaneGridLengthProperty);
			}
			internal set
			{
				base.SetValue(SplitViewTemplateSettings.OpenPaneGridLengthPropertyKey, value);
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000C8C RID: 3212 RVA: 0x0002AD20 File Offset: 0x00028F20
		// (set) Token: 0x06000C8D RID: 3213 RVA: 0x0002AD32 File Offset: 0x00028F32
		public double OpenPaneLength
		{
			get
			{
				return (double)base.GetValue(SplitViewTemplateSettings.OpenPaneLengthProperty);
			}
			internal set
			{
				base.SetValue(SplitViewTemplateSettings.OpenPaneLengthPropertyKey, value);
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x0002AD45 File Offset: 0x00028F45
		// (set) Token: 0x06000C8F RID: 3215 RVA: 0x0002AD57 File Offset: 0x00028F57
		public double OpenPaneLengthMinusCompactLength
		{
			get
			{
				return (double)base.GetValue(SplitViewTemplateSettings.OpenPaneLengthMinusCompactLengthProperty);
			}
			internal set
			{
				base.SetValue(SplitViewTemplateSettings.OpenPaneLengthMinusCompactLengthPropertyKey, value);
			}
		}

		// Token: 0x04000546 RID: 1350
		private static readonly DependencyPropertyKey CompactPaneGridLengthPropertyKey = DependencyProperty.RegisterReadOnly("CompactPaneGridLength", typeof(GridLength), typeof(SplitViewTemplateSettings), null);

		// Token: 0x04000547 RID: 1351
		public static readonly DependencyProperty CompactPaneGridLengthProperty = SplitViewTemplateSettings.CompactPaneGridLengthPropertyKey.DependencyProperty;

		// Token: 0x04000548 RID: 1352
		private static readonly DependencyPropertyKey NegativeOpenPaneLengthPropertyKey = DependencyProperty.RegisterReadOnly("NegativeOpenPaneLength", typeof(double), typeof(SplitViewTemplateSettings), null);

		// Token: 0x04000549 RID: 1353
		public static readonly DependencyProperty NegativeOpenPaneLengthProperty = SplitViewTemplateSettings.NegativeOpenPaneLengthPropertyKey.DependencyProperty;

		// Token: 0x0400054A RID: 1354
		private static readonly DependencyPropertyKey NegativeOpenPaneLengthMinusCompactLengthPropertyKey = DependencyProperty.RegisterReadOnly("NegativeOpenPaneLengthMinusCompactLength", typeof(double), typeof(SplitViewTemplateSettings), null);

		// Token: 0x0400054B RID: 1355
		public static readonly DependencyProperty NegativeOpenPaneLengthMinusCompactLengthProperty = SplitViewTemplateSettings.NegativeOpenPaneLengthMinusCompactLengthPropertyKey.DependencyProperty;

		// Token: 0x0400054C RID: 1356
		private static readonly DependencyPropertyKey OpenPaneGridLengthPropertyKey = DependencyProperty.RegisterReadOnly("OpenPaneGridLength", typeof(GridLength), typeof(SplitViewTemplateSettings), null);

		// Token: 0x0400054D RID: 1357
		public static readonly DependencyProperty OpenPaneGridLengthProperty = SplitViewTemplateSettings.OpenPaneGridLengthPropertyKey.DependencyProperty;

		// Token: 0x0400054E RID: 1358
		private static readonly DependencyPropertyKey OpenPaneLengthPropertyKey = DependencyProperty.RegisterReadOnly("OpenPaneLength", typeof(double), typeof(SplitViewTemplateSettings), null);

		// Token: 0x0400054F RID: 1359
		public static readonly DependencyProperty OpenPaneLengthProperty = SplitViewTemplateSettings.OpenPaneLengthPropertyKey.DependencyProperty;

		// Token: 0x04000550 RID: 1360
		private static readonly DependencyPropertyKey OpenPaneLengthMinusCompactLengthPropertyKey = DependencyProperty.RegisterReadOnly("OpenPaneLengthMinusCompactLength", typeof(double), typeof(SplitViewTemplateSettings), null);

		// Token: 0x04000551 RID: 1361
		public static readonly DependencyProperty OpenPaneLengthMinusCompactLengthProperty = SplitViewTemplateSettings.OpenPaneLengthMinusCompactLengthPropertyKey.DependencyProperty;
	}
}
