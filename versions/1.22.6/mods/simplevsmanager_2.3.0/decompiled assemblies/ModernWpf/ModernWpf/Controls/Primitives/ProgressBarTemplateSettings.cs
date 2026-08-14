using System;
using System.Windows;
using System.Windows.Media;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020002AF RID: 687
	public sealed class ProgressBarTemplateSettings : DependencyObject
	{
		// Token: 0x060014CD RID: 5325 RVA: 0x0003E12B File Offset: 0x0003C32B
		internal ProgressBarTemplateSettings()
		{
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x060014CE RID: 5326 RVA: 0x0003E133 File Offset: 0x0003C333
		// (set) Token: 0x060014CF RID: 5327 RVA: 0x0003E145 File Offset: 0x0003C345
		public double ContainerAnimationStartPosition
		{
			get
			{
				return (double)base.GetValue(ProgressBarTemplateSettings.ContainerAnimationStartPositionProperty);
			}
			internal set
			{
				base.SetValue(ProgressBarTemplateSettings.ContainerAnimationStartPositionPropertyKey, value);
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x060014D0 RID: 5328 RVA: 0x0003E158 File Offset: 0x0003C358
		// (set) Token: 0x060014D1 RID: 5329 RVA: 0x0003E16A File Offset: 0x0003C36A
		public double ContainerAnimationEndPosition
		{
			get
			{
				return (double)base.GetValue(ProgressBarTemplateSettings.ContainerAnimationEndPositionProperty);
			}
			internal set
			{
				base.SetValue(ProgressBarTemplateSettings.ContainerAnimationEndPositionPropertyKey, value);
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x060014D2 RID: 5330 RVA: 0x0003E17D File Offset: 0x0003C37D
		// (set) Token: 0x060014D3 RID: 5331 RVA: 0x0003E18F File Offset: 0x0003C38F
		public double Container2AnimationStartPosition
		{
			get
			{
				return (double)base.GetValue(ProgressBarTemplateSettings.Container2AnimationStartPositionProperty);
			}
			internal set
			{
				base.SetValue(ProgressBarTemplateSettings.Container2AnimationStartPositionPropertyKey, value);
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x060014D4 RID: 5332 RVA: 0x0003E1A2 File Offset: 0x0003C3A2
		// (set) Token: 0x060014D5 RID: 5333 RVA: 0x0003E1B4 File Offset: 0x0003C3B4
		public double Container2AnimationEndPosition
		{
			get
			{
				return (double)base.GetValue(ProgressBarTemplateSettings.Container2AnimationEndPositionProperty);
			}
			internal set
			{
				base.SetValue(ProgressBarTemplateSettings.Container2AnimationEndPositionPropertyKey, value);
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x060014D6 RID: 5334 RVA: 0x0003E1C7 File Offset: 0x0003C3C7
		// (set) Token: 0x060014D7 RID: 5335 RVA: 0x0003E1D9 File Offset: 0x0003C3D9
		public double ContainerAnimationMidPosition
		{
			get
			{
				return (double)base.GetValue(ProgressBarTemplateSettings.ContainerAnimationMidPositionProperty);
			}
			internal set
			{
				base.SetValue(ProgressBarTemplateSettings.ContainerAnimationMidPositionPropertyKey, value);
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x060014D8 RID: 5336 RVA: 0x0003E1EC File Offset: 0x0003C3EC
		// (set) Token: 0x060014D9 RID: 5337 RVA: 0x0003E1FE File Offset: 0x0003C3FE
		public double IndicatorLengthDelta
		{
			get
			{
				return (double)base.GetValue(ProgressBarTemplateSettings.IndicatorLengthDeltaProperty);
			}
			internal set
			{
				base.SetValue(ProgressBarTemplateSettings.IndicatorLengthDeltaPropertyKey, value);
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x060014DA RID: 5338 RVA: 0x0003E211 File Offset: 0x0003C411
		// (set) Token: 0x060014DB RID: 5339 RVA: 0x0003E223 File Offset: 0x0003C423
		public RectangleGeometry ClipRect
		{
			get
			{
				return (RectangleGeometry)base.GetValue(ProgressBarTemplateSettings.ClipRectProperty);
			}
			internal set
			{
				base.SetValue(ProgressBarTemplateSettings.ClipRectPropertyKey, value);
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x060014DC RID: 5340 RVA: 0x0003E231 File Offset: 0x0003C431
		// (set) Token: 0x060014DD RID: 5341 RVA: 0x0003E243 File Offset: 0x0003C443
		public double EllipseAnimationEndPosition
		{
			get
			{
				return (double)base.GetValue(ProgressBarTemplateSettings.EllipseAnimationEndPositionProperty);
			}
			internal set
			{
				base.SetValue(ProgressBarTemplateSettings.EllipseAnimationEndPositionPropertyKey, value);
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x060014DE RID: 5342 RVA: 0x0003E256 File Offset: 0x0003C456
		// (set) Token: 0x060014DF RID: 5343 RVA: 0x0003E268 File Offset: 0x0003C468
		public double EllipseAnimationWellPosition
		{
			get
			{
				return (double)base.GetValue(ProgressBarTemplateSettings.EllipseAnimationWellPositionProperty);
			}
			internal set
			{
				base.SetValue(ProgressBarTemplateSettings.EllipseAnimationWellPositionPropertyKey, value);
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x060014E0 RID: 5344 RVA: 0x0003E27B File Offset: 0x0003C47B
		// (set) Token: 0x060014E1 RID: 5345 RVA: 0x0003E28D File Offset: 0x0003C48D
		public double EllipseDiameter
		{
			get
			{
				return (double)base.GetValue(ProgressBarTemplateSettings.EllipseDiameterProperty);
			}
			internal set
			{
				base.SetValue(ProgressBarTemplateSettings.EllipseDiameterPropertyKey, value);
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x060014E2 RID: 5346 RVA: 0x0003E2A0 File Offset: 0x0003C4A0
		// (set) Token: 0x060014E3 RID: 5347 RVA: 0x0003E2B2 File Offset: 0x0003C4B2
		public double EllipseOffset
		{
			get
			{
				return (double)base.GetValue(ProgressBarTemplateSettings.EllipseOffsetProperty);
			}
			internal set
			{
				base.SetValue(ProgressBarTemplateSettings.EllipseOffsetPropertyKey, value);
			}
		}

		// Token: 0x040006FC RID: 1788
		private static readonly DependencyPropertyKey ContainerAnimationStartPositionPropertyKey = DependencyProperty.RegisterReadOnly("ContainerAnimationStartPosition", typeof(double), typeof(ProgressBarTemplateSettings), null);

		// Token: 0x040006FD RID: 1789
		public static readonly DependencyProperty ContainerAnimationStartPositionProperty = ProgressBarTemplateSettings.ContainerAnimationStartPositionPropertyKey.DependencyProperty;

		// Token: 0x040006FE RID: 1790
		private static readonly DependencyPropertyKey ContainerAnimationEndPositionPropertyKey = DependencyProperty.RegisterReadOnly("ContainerAnimationEndPosition", typeof(double), typeof(ProgressBarTemplateSettings), null);

		// Token: 0x040006FF RID: 1791
		public static readonly DependencyProperty ContainerAnimationEndPositionProperty = ProgressBarTemplateSettings.ContainerAnimationEndPositionPropertyKey.DependencyProperty;

		// Token: 0x04000700 RID: 1792
		private static readonly DependencyPropertyKey Container2AnimationStartPositionPropertyKey = DependencyProperty.RegisterReadOnly("Container2AnimationStartPosition", typeof(double), typeof(ProgressBarTemplateSettings), null);

		// Token: 0x04000701 RID: 1793
		public static readonly DependencyProperty Container2AnimationStartPositionProperty = ProgressBarTemplateSettings.Container2AnimationStartPositionPropertyKey.DependencyProperty;

		// Token: 0x04000702 RID: 1794
		private static readonly DependencyPropertyKey Container2AnimationEndPositionPropertyKey = DependencyProperty.RegisterReadOnly("Container2AnimationEndPosition", typeof(double), typeof(ProgressBarTemplateSettings), null);

		// Token: 0x04000703 RID: 1795
		public static readonly DependencyProperty Container2AnimationEndPositionProperty = ProgressBarTemplateSettings.Container2AnimationEndPositionPropertyKey.DependencyProperty;

		// Token: 0x04000704 RID: 1796
		private static readonly DependencyPropertyKey ContainerAnimationMidPositionPropertyKey = DependencyProperty.RegisterReadOnly("ContainerAnimationMidPosition", typeof(double), typeof(ProgressBarTemplateSettings), null);

		// Token: 0x04000705 RID: 1797
		public static readonly DependencyProperty ContainerAnimationMidPositionProperty = ProgressBarTemplateSettings.ContainerAnimationMidPositionPropertyKey.DependencyProperty;

		// Token: 0x04000706 RID: 1798
		private static readonly DependencyPropertyKey IndicatorLengthDeltaPropertyKey = DependencyProperty.RegisterReadOnly("IndicatorLengthDelta", typeof(double), typeof(ProgressBarTemplateSettings), null);

		// Token: 0x04000707 RID: 1799
		public static readonly DependencyProperty IndicatorLengthDeltaProperty = ProgressBarTemplateSettings.IndicatorLengthDeltaPropertyKey.DependencyProperty;

		// Token: 0x04000708 RID: 1800
		private static readonly DependencyPropertyKey ClipRectPropertyKey = DependencyProperty.RegisterReadOnly("ClipRect", typeof(RectangleGeometry), typeof(ProgressBarTemplateSettings), null);

		// Token: 0x04000709 RID: 1801
		public static readonly DependencyProperty ClipRectProperty = ProgressBarTemplateSettings.ClipRectPropertyKey.DependencyProperty;

		// Token: 0x0400070A RID: 1802
		private static readonly DependencyPropertyKey EllipseAnimationEndPositionPropertyKey = DependencyProperty.RegisterReadOnly("EllipseAnimationEndPosition", typeof(double), typeof(ProgressBarTemplateSettings), null);

		// Token: 0x0400070B RID: 1803
		public static readonly DependencyProperty EllipseAnimationEndPositionProperty = ProgressBarTemplateSettings.EllipseAnimationEndPositionPropertyKey.DependencyProperty;

		// Token: 0x0400070C RID: 1804
		private static readonly DependencyPropertyKey EllipseAnimationWellPositionPropertyKey = DependencyProperty.RegisterReadOnly("EllipseAnimationWellPosition", typeof(double), typeof(ProgressBarTemplateSettings), null);

		// Token: 0x0400070D RID: 1805
		public static readonly DependencyProperty EllipseAnimationWellPositionProperty = ProgressBarTemplateSettings.EllipseAnimationWellPositionPropertyKey.DependencyProperty;

		// Token: 0x0400070E RID: 1806
		private static readonly DependencyPropertyKey EllipseDiameterPropertyKey = DependencyProperty.RegisterReadOnly("EllipseDiameter", typeof(double), typeof(ProgressBarTemplateSettings), null);

		// Token: 0x0400070F RID: 1807
		public static readonly DependencyProperty EllipseDiameterProperty = ProgressBarTemplateSettings.EllipseDiameterPropertyKey.DependencyProperty;

		// Token: 0x04000710 RID: 1808
		private static readonly DependencyPropertyKey EllipseOffsetPropertyKey = DependencyProperty.RegisterReadOnly("EllipseOffset", typeof(double), typeof(ProgressBarTemplateSettings), null);

		// Token: 0x04000711 RID: 1809
		public static readonly DependencyProperty EllipseOffsetProperty = ProgressBarTemplateSettings.EllipseOffsetPropertyKey.DependencyProperty;
	}
}
