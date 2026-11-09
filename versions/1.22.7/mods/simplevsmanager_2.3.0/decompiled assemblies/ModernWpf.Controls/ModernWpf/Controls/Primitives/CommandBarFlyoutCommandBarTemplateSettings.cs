using System;
using System.Windows;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020000E4 RID: 228
	public sealed class CommandBarFlyoutCommandBarTemplateSettings : DependencyObject
	{
		// Token: 0x06000B6B RID: 2923 RVA: 0x00026A50 File Offset: 0x00024C50
		internal CommandBarFlyoutCommandBarTemplateSettings()
		{
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x00026A58 File Offset: 0x00024C58
		// (set) Token: 0x06000B6D RID: 2925 RVA: 0x00026A6A File Offset: 0x00024C6A
		public double CloseAnimationEndPosition
		{
			get
			{
				return (double)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.CloseAnimationEndPositionProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.CloseAnimationEndPositionPropertyKey, value);
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000B6E RID: 2926 RVA: 0x00026A7D File Offset: 0x00024C7D
		// (set) Token: 0x06000B6F RID: 2927 RVA: 0x00026A8F File Offset: 0x00024C8F
		public Rect ContentClipRect
		{
			get
			{
				return (Rect)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.ContentClipRectProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.ContentClipRectPropertyKey, value);
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000B70 RID: 2928 RVA: 0x00026AA2 File Offset: 0x00024CA2
		// (set) Token: 0x06000B71 RID: 2929 RVA: 0x00026AB4 File Offset: 0x00024CB4
		public double CurrentWidth
		{
			get
			{
				return (double)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.CurrentWidthProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.CurrentWidthPropertyKey, value);
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x00026AC7 File Offset: 0x00024CC7
		// (set) Token: 0x06000B73 RID: 2931 RVA: 0x00026AD9 File Offset: 0x00024CD9
		public double ExpandDownAnimationEndPosition
		{
			get
			{
				return (double)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.ExpandDownAnimationEndPositionProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.ExpandDownAnimationEndPositionPropertyKey, value);
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000B74 RID: 2932 RVA: 0x00026AEC File Offset: 0x00024CEC
		// (set) Token: 0x06000B75 RID: 2933 RVA: 0x00026AFE File Offset: 0x00024CFE
		public double ExpandDownAnimationHoldPosition
		{
			get
			{
				return (double)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.ExpandDownAnimationHoldPositionProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.ExpandDownAnimationHoldPositionPropertyKey, value);
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000B76 RID: 2934 RVA: 0x00026B11 File Offset: 0x00024D11
		// (set) Token: 0x06000B77 RID: 2935 RVA: 0x00026B23 File Offset: 0x00024D23
		public double ExpandDownAnimationStartPosition
		{
			get
			{
				return (double)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.ExpandDownAnimationStartPositionProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.ExpandDownAnimationStartPositionPropertyKey, value);
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000B78 RID: 2936 RVA: 0x00026B36 File Offset: 0x00024D36
		// (set) Token: 0x06000B79 RID: 2937 RVA: 0x00026B48 File Offset: 0x00024D48
		public double ExpandDownOverflowVerticalPosition
		{
			get
			{
				return (double)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.ExpandDownOverflowVerticalPositionProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.ExpandDownOverflowVerticalPositionPropertyKey, value);
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000B7A RID: 2938 RVA: 0x00026B5B File Offset: 0x00024D5B
		// (set) Token: 0x06000B7B RID: 2939 RVA: 0x00026B6D File Offset: 0x00024D6D
		public double ExpandedWidth
		{
			get
			{
				return (double)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.ExpandedWidthProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.ExpandedWidthPropertyKey, value);
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000B7C RID: 2940 RVA: 0x00026B80 File Offset: 0x00024D80
		// (set) Token: 0x06000B7D RID: 2941 RVA: 0x00026B92 File Offset: 0x00024D92
		public double ExpandUpAnimationEndPosition
		{
			get
			{
				return (double)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.ExpandUpAnimationEndPositionProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.ExpandUpAnimationEndPositionPropertyKey, value);
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000B7E RID: 2942 RVA: 0x00026BA5 File Offset: 0x00024DA5
		// (set) Token: 0x06000B7F RID: 2943 RVA: 0x00026BB7 File Offset: 0x00024DB7
		public double ExpandUpAnimationHoldPosition
		{
			get
			{
				return (double)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.ExpandUpAnimationHoldPositionProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.ExpandUpAnimationHoldPositionPropertyKey, value);
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x00026BCA File Offset: 0x00024DCA
		// (set) Token: 0x06000B81 RID: 2945 RVA: 0x00026BDC File Offset: 0x00024DDC
		public double ExpandUpAnimationStartPosition
		{
			get
			{
				return (double)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.ExpandUpAnimationStartPositionProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.ExpandUpAnimationStartPositionPropertyKey, value);
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x00026BEF File Offset: 0x00024DEF
		// (set) Token: 0x06000B83 RID: 2947 RVA: 0x00026C01 File Offset: 0x00024E01
		public double ExpandUpOverflowVerticalPosition
		{
			get
			{
				return (double)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.ExpandUpOverflowVerticalPositionProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.ExpandUpOverflowVerticalPositionPropertyKey, value);
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000B84 RID: 2948 RVA: 0x00026C14 File Offset: 0x00024E14
		// (set) Token: 0x06000B85 RID: 2949 RVA: 0x00026C26 File Offset: 0x00024E26
		public double OpenAnimationEndPosition
		{
			get
			{
				return (double)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.OpenAnimationEndPositionProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.OpenAnimationEndPositionPropertyKey, value);
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x00026C39 File Offset: 0x00024E39
		// (set) Token: 0x06000B87 RID: 2951 RVA: 0x00026C4B File Offset: 0x00024E4B
		public double OpenAnimationStartPosition
		{
			get
			{
				return (double)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.OpenAnimationStartPositionProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.OpenAnimationStartPositionPropertyKey, value);
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000B88 RID: 2952 RVA: 0x00026C5E File Offset: 0x00024E5E
		// (set) Token: 0x06000B89 RID: 2953 RVA: 0x00026C70 File Offset: 0x00024E70
		public Rect OverflowContentClipRect
		{
			get
			{
				return (Rect)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.OverflowContentClipRectProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.OverflowContentClipRectPropertyKey, value);
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000B8A RID: 2954 RVA: 0x00026C83 File Offset: 0x00024E83
		// (set) Token: 0x06000B8B RID: 2955 RVA: 0x00026C95 File Offset: 0x00024E95
		public double WidthExpansionAnimationEndPosition
		{
			get
			{
				return (double)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.WidthExpansionAnimationEndPositionProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.WidthExpansionAnimationEndPositionPropertyKey, value);
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000B8C RID: 2956 RVA: 0x00026CA8 File Offset: 0x00024EA8
		// (set) Token: 0x06000B8D RID: 2957 RVA: 0x00026CBA File Offset: 0x00024EBA
		public double WidthExpansionAnimationStartPosition
		{
			get
			{
				return (double)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.WidthExpansionAnimationStartPositionProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.WidthExpansionAnimationStartPositionPropertyKey, value);
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x00026CCD File Offset: 0x00024ECD
		// (set) Token: 0x06000B8F RID: 2959 RVA: 0x00026CDF File Offset: 0x00024EDF
		public double WidthExpansionDelta
		{
			get
			{
				return (double)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.WidthExpansionDeltaProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.WidthExpansionDeltaPropertyKey, value);
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x00026CF2 File Offset: 0x00024EF2
		// (set) Token: 0x06000B91 RID: 2961 RVA: 0x00026D04 File Offset: 0x00024F04
		public double WidthExpansionMoreButtonAnimationEndPosition
		{
			get
			{
				return (double)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.WidthExpansionMoreButtonAnimationEndPositionProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.WidthExpansionMoreButtonAnimationEndPositionPropertyKey, value);
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x00026D17 File Offset: 0x00024F17
		// (set) Token: 0x06000B93 RID: 2963 RVA: 0x00026D29 File Offset: 0x00024F29
		public double WidthExpansionMoreButtonAnimationStartPosition
		{
			get
			{
				return (double)base.GetValue(CommandBarFlyoutCommandBarTemplateSettings.WidthExpansionMoreButtonAnimationStartPositionProperty);
			}
			internal set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettings.WidthExpansionMoreButtonAnimationStartPositionPropertyKey, value);
			}
		}

		// Token: 0x040004A7 RID: 1191
		private static readonly DependencyPropertyKey CloseAnimationEndPositionPropertyKey = DependencyProperty.RegisterReadOnly("CloseAnimationEndPosition", typeof(double), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004A8 RID: 1192
		public static readonly DependencyProperty CloseAnimationEndPositionProperty = CommandBarFlyoutCommandBarTemplateSettings.CloseAnimationEndPositionPropertyKey.DependencyProperty;

		// Token: 0x040004A9 RID: 1193
		private static readonly DependencyPropertyKey ContentClipRectPropertyKey = DependencyProperty.RegisterReadOnly("ContentClipRect", typeof(Rect), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004AA RID: 1194
		public static readonly DependencyProperty ContentClipRectProperty = CommandBarFlyoutCommandBarTemplateSettings.ContentClipRectPropertyKey.DependencyProperty;

		// Token: 0x040004AB RID: 1195
		private static readonly DependencyPropertyKey CurrentWidthPropertyKey = DependencyProperty.RegisterReadOnly("CurrentWidth", typeof(double), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004AC RID: 1196
		public static readonly DependencyProperty CurrentWidthProperty = CommandBarFlyoutCommandBarTemplateSettings.CurrentWidthPropertyKey.DependencyProperty;

		// Token: 0x040004AD RID: 1197
		private static readonly DependencyPropertyKey ExpandDownAnimationEndPositionPropertyKey = DependencyProperty.RegisterReadOnly("ExpandDownAnimationEndPosition", typeof(double), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004AE RID: 1198
		public static readonly DependencyProperty ExpandDownAnimationEndPositionProperty = CommandBarFlyoutCommandBarTemplateSettings.ExpandDownAnimationEndPositionPropertyKey.DependencyProperty;

		// Token: 0x040004AF RID: 1199
		private static readonly DependencyPropertyKey ExpandDownAnimationHoldPositionPropertyKey = DependencyProperty.RegisterReadOnly("ExpandDownAnimationHoldPosition", typeof(double), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004B0 RID: 1200
		public static readonly DependencyProperty ExpandDownAnimationHoldPositionProperty = CommandBarFlyoutCommandBarTemplateSettings.ExpandDownAnimationHoldPositionPropertyKey.DependencyProperty;

		// Token: 0x040004B1 RID: 1201
		private static readonly DependencyPropertyKey ExpandDownAnimationStartPositionPropertyKey = DependencyProperty.RegisterReadOnly("ExpandDownAnimationStartPosition", typeof(double), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004B2 RID: 1202
		public static readonly DependencyProperty ExpandDownAnimationStartPositionProperty = CommandBarFlyoutCommandBarTemplateSettings.ExpandDownAnimationStartPositionPropertyKey.DependencyProperty;

		// Token: 0x040004B3 RID: 1203
		private static readonly DependencyPropertyKey ExpandDownOverflowVerticalPositionPropertyKey = DependencyProperty.RegisterReadOnly("ExpandDownOverflowVerticalPosition", typeof(double), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004B4 RID: 1204
		public static readonly DependencyProperty ExpandDownOverflowVerticalPositionProperty = CommandBarFlyoutCommandBarTemplateSettings.ExpandDownOverflowVerticalPositionPropertyKey.DependencyProperty;

		// Token: 0x040004B5 RID: 1205
		private static readonly DependencyPropertyKey ExpandedWidthPropertyKey = DependencyProperty.RegisterReadOnly("ExpandedWidth", typeof(double), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004B6 RID: 1206
		public static readonly DependencyProperty ExpandedWidthProperty = CommandBarFlyoutCommandBarTemplateSettings.ExpandedWidthPropertyKey.DependencyProperty;

		// Token: 0x040004B7 RID: 1207
		private static readonly DependencyPropertyKey ExpandUpAnimationEndPositionPropertyKey = DependencyProperty.RegisterReadOnly("ExpandUpAnimationEndPosition", typeof(double), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004B8 RID: 1208
		public static readonly DependencyProperty ExpandUpAnimationEndPositionProperty = CommandBarFlyoutCommandBarTemplateSettings.ExpandUpAnimationEndPositionPropertyKey.DependencyProperty;

		// Token: 0x040004B9 RID: 1209
		private static readonly DependencyPropertyKey ExpandUpAnimationHoldPositionPropertyKey = DependencyProperty.RegisterReadOnly("ExpandUpAnimationHoldPosition", typeof(double), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004BA RID: 1210
		public static readonly DependencyProperty ExpandUpAnimationHoldPositionProperty = CommandBarFlyoutCommandBarTemplateSettings.ExpandUpAnimationHoldPositionPropertyKey.DependencyProperty;

		// Token: 0x040004BB RID: 1211
		private static readonly DependencyPropertyKey ExpandUpAnimationStartPositionPropertyKey = DependencyProperty.RegisterReadOnly("ExpandUpAnimationStartPosition", typeof(double), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004BC RID: 1212
		public static readonly DependencyProperty ExpandUpAnimationStartPositionProperty = CommandBarFlyoutCommandBarTemplateSettings.ExpandUpAnimationStartPositionPropertyKey.DependencyProperty;

		// Token: 0x040004BD RID: 1213
		private static readonly DependencyPropertyKey ExpandUpOverflowVerticalPositionPropertyKey = DependencyProperty.RegisterReadOnly("ExpandUpOverflowVerticalPosition", typeof(double), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004BE RID: 1214
		public static readonly DependencyProperty ExpandUpOverflowVerticalPositionProperty = CommandBarFlyoutCommandBarTemplateSettings.ExpandUpOverflowVerticalPositionPropertyKey.DependencyProperty;

		// Token: 0x040004BF RID: 1215
		private static readonly DependencyPropertyKey OpenAnimationEndPositionPropertyKey = DependencyProperty.RegisterReadOnly("OpenAnimationEndPosition", typeof(double), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004C0 RID: 1216
		public static readonly DependencyProperty OpenAnimationEndPositionProperty = CommandBarFlyoutCommandBarTemplateSettings.OpenAnimationEndPositionPropertyKey.DependencyProperty;

		// Token: 0x040004C1 RID: 1217
		private static readonly DependencyPropertyKey OpenAnimationStartPositionPropertyKey = DependencyProperty.RegisterReadOnly("OpenAnimationStartPosition", typeof(double), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004C2 RID: 1218
		public static readonly DependencyProperty OpenAnimationStartPositionProperty = CommandBarFlyoutCommandBarTemplateSettings.OpenAnimationStartPositionPropertyKey.DependencyProperty;

		// Token: 0x040004C3 RID: 1219
		private static readonly DependencyPropertyKey OverflowContentClipRectPropertyKey = DependencyProperty.RegisterReadOnly("OverflowContentClipRect", typeof(Rect), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004C4 RID: 1220
		public static readonly DependencyProperty OverflowContentClipRectProperty = CommandBarFlyoutCommandBarTemplateSettings.OverflowContentClipRectPropertyKey.DependencyProperty;

		// Token: 0x040004C5 RID: 1221
		private static readonly DependencyPropertyKey WidthExpansionAnimationEndPositionPropertyKey = DependencyProperty.RegisterReadOnly("WidthExpansionAnimationEndPosition", typeof(double), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004C6 RID: 1222
		public static readonly DependencyProperty WidthExpansionAnimationEndPositionProperty = CommandBarFlyoutCommandBarTemplateSettings.WidthExpansionAnimationEndPositionPropertyKey.DependencyProperty;

		// Token: 0x040004C7 RID: 1223
		private static readonly DependencyPropertyKey WidthExpansionAnimationStartPositionPropertyKey = DependencyProperty.RegisterReadOnly("WidthExpansionAnimationStartPosition", typeof(double), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004C8 RID: 1224
		public static readonly DependencyProperty WidthExpansionAnimationStartPositionProperty = CommandBarFlyoutCommandBarTemplateSettings.WidthExpansionAnimationStartPositionPropertyKey.DependencyProperty;

		// Token: 0x040004C9 RID: 1225
		private static readonly DependencyPropertyKey WidthExpansionDeltaPropertyKey = DependencyProperty.RegisterReadOnly("WidthExpansionDelta", typeof(double), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004CA RID: 1226
		public static readonly DependencyProperty WidthExpansionDeltaProperty = CommandBarFlyoutCommandBarTemplateSettings.WidthExpansionDeltaPropertyKey.DependencyProperty;

		// Token: 0x040004CB RID: 1227
		private static readonly DependencyPropertyKey WidthExpansionMoreButtonAnimationEndPositionPropertyKey = DependencyProperty.RegisterReadOnly("WidthExpansionMoreButtonAnimationEndPosition", typeof(double), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004CC RID: 1228
		public static readonly DependencyProperty WidthExpansionMoreButtonAnimationEndPositionProperty = CommandBarFlyoutCommandBarTemplateSettings.WidthExpansionMoreButtonAnimationEndPositionPropertyKey.DependencyProperty;

		// Token: 0x040004CD RID: 1229
		private static readonly DependencyPropertyKey WidthExpansionMoreButtonAnimationStartPositionPropertyKey = DependencyProperty.RegisterReadOnly("WidthExpansionMoreButtonAnimationStartPosition", typeof(double), typeof(CommandBarFlyoutCommandBarTemplateSettings), null);

		// Token: 0x040004CE RID: 1230
		public static readonly DependencyProperty WidthExpansionMoreButtonAnimationStartPositionProperty = CommandBarFlyoutCommandBarTemplateSettings.WidthExpansionMoreButtonAnimationStartPositionPropertyKey.DependencyProperty;
	}
}
