using System;
using System.Windows;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020000F1 RID: 241
	public sealed class ProgressRingTemplateSettings : DependencyObject
	{
		// Token: 0x06000C62 RID: 3170 RVA: 0x0002A599 File Offset: 0x00028799
		internal ProgressRingTemplateSettings()
		{
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000C63 RID: 3171 RVA: 0x0002A5A1 File Offset: 0x000287A1
		// (set) Token: 0x06000C64 RID: 3172 RVA: 0x0002A5B3 File Offset: 0x000287B3
		public double EllipseDiameter
		{
			get
			{
				return (double)base.GetValue(ProgressRingTemplateSettings.EllipseDiameterProperty);
			}
			internal set
			{
				base.SetValue(ProgressRingTemplateSettings.EllipseDiameterPropertyKey, value);
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000C65 RID: 3173 RVA: 0x0002A5C6 File Offset: 0x000287C6
		// (set) Token: 0x06000C66 RID: 3174 RVA: 0x0002A5D8 File Offset: 0x000287D8
		public Thickness EllipseOffset
		{
			get
			{
				return (Thickness)base.GetValue(ProgressRingTemplateSettings.EllipseOffsetProperty);
			}
			internal set
			{
				base.SetValue(ProgressRingTemplateSettings.EllipseOffsetPropertyKey, value);
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x0002A5EB File Offset: 0x000287EB
		// (set) Token: 0x06000C68 RID: 3176 RVA: 0x0002A5FD File Offset: 0x000287FD
		public double MaxSideLength
		{
			get
			{
				return (double)base.GetValue(ProgressRingTemplateSettings.MaxSideLengthProperty);
			}
			internal set
			{
				base.SetValue(ProgressRingTemplateSettings.MaxSideLengthPropertyKey, value);
			}
		}

		// Token: 0x04000536 RID: 1334
		private static readonly DependencyPropertyKey EllipseDiameterPropertyKey = DependencyProperty.RegisterReadOnly("EllipseDiameter", typeof(double), typeof(ProgressRingTemplateSettings), null);

		// Token: 0x04000537 RID: 1335
		public static readonly DependencyProperty EllipseDiameterProperty = ProgressRingTemplateSettings.EllipseDiameterPropertyKey.DependencyProperty;

		// Token: 0x04000538 RID: 1336
		private static readonly DependencyPropertyKey EllipseOffsetPropertyKey = DependencyProperty.RegisterReadOnly("EllipseOffset", typeof(Thickness), typeof(ProgressRingTemplateSettings), null);

		// Token: 0x04000539 RID: 1337
		public static readonly DependencyProperty EllipseOffsetProperty = ProgressRingTemplateSettings.EllipseOffsetPropertyKey.DependencyProperty;

		// Token: 0x0400053A RID: 1338
		private static readonly DependencyPropertyKey MaxSideLengthPropertyKey = DependencyProperty.RegisterReadOnly("MaxSideLength", typeof(double), typeof(ProgressRingTemplateSettings), null);

		// Token: 0x0400053B RID: 1339
		public static readonly DependencyProperty MaxSideLengthProperty = ProgressRingTemplateSettings.MaxSideLengthPropertyKey.DependencyProperty;
	}
}
