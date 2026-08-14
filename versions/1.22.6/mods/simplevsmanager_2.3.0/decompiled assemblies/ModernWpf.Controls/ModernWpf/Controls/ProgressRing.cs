using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using ModernWpf.Automation.Peers;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000080 RID: 128
	public class ProgressRing : Control
	{
		// Token: 0x06000641 RID: 1601 RVA: 0x00016DD8 File Offset: 0x00014FD8
		static ProgressRing()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressRing), new FrameworkPropertyMetadata(typeof(ProgressRing)));
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00016E6F File Offset: 0x0001506F
		public ProgressRing()
		{
			base.SetValue(ProgressRing.TemplateSettingsPropertyKey, new ProgressRingTemplateSettings());
			base.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x00016E99 File Offset: 0x00015099
		// (set) Token: 0x06000644 RID: 1604 RVA: 0x00016EAB File Offset: 0x000150AB
		public bool IsActive
		{
			get
			{
				return (bool)base.GetValue(ProgressRing.IsActiveProperty);
			}
			set
			{
				base.SetValue(ProgressRing.IsActiveProperty, value);
			}
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00016EBE File Offset: 0x000150BE
		private static void OnIsActivePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((ProgressRing)sender).OnIsActivePropertyChanged(args);
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x00016ECC File Offset: 0x000150CC
		public ProgressRingTemplateSettings TemplateSettings
		{
			get
			{
				return (ProgressRingTemplateSettings)base.GetValue(ProgressRing.TemplateSettingsProperty);
			}
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00016EDE File Offset: 0x000150DE
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ProgressRingAutomationPeer(this);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00016EE6 File Offset: 0x000150E6
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.ChangeVisualState();
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00016EF4 File Offset: 0x000150F4
		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.ApplyTemplateSettings();
			this.ChangeVisualState();
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00016F02 File Offset: 0x00015102
		private void OnIsActivePropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.ChangeVisualState();
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00016F0C File Offset: 0x0001510C
		private void ChangeVisualState()
		{
			VisualStateManager.GoToState(this, this.IsActive ? "Active" : "Inactive", true);
			VisualStateManager.GoToState(this, (this.TemplateSettings.MaxSideLength < 60.0) ? "Small" : "Large", true);
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00016F60 File Offset: 0x00015160
		private void ApplyTemplateSettings()
		{
			ProgressRingTemplateSettings templateSettings = this.TemplateSettings;
			ValueTuple<double, double, double> valueTuple = this.<ApplyTemplateSettings>g__calcSettings|20_0();
			double item = valueTuple.Item1;
			double item2 = valueTuple.Item2;
			double item3 = valueTuple.Item3;
			templateSettings.EllipseDiameter = item2;
			Thickness ellipseOffset;
			ellipseOffset..ctor(0.0, item3, 0.0, 0.0);
			templateSettings.EllipseOffset = ellipseOffset;
			templateSettings.MaxSideLength = item;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00016FC4 File Offset: 0x000151C4
		[CompilerGenerated]
		private ValueTuple<double, double, double> <ApplyTemplateSettings>g__calcSettings|20_0()
		{
			if (base.ActualWidth != 0.0)
			{
				ProgressRing.<>c__DisplayClass20_0 CS$<>8__locals1;
				CS$<>8__locals1.width = Math.Min(base.ActualWidth, base.ActualHeight);
				double num = ProgressRing.<ApplyTemplateSettings>g__init|20_1(ref CS$<>8__locals1);
				double num2 = CS$<>8__locals1.width * 0.1 + num;
				double num3 = CS$<>8__locals1.width * 0.5 - num2;
				return new ValueTuple<double, double, double>(CS$<>8__locals1.width, num2, num3);
			}
			return new ValueTuple<double, double, double>(0.0, 0.0, 0.0);
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00017056 File Offset: 0x00015256
		[CompilerGenerated]
		internal static double <ApplyTemplateSettings>g__init|20_1(ref ProgressRing.<>c__DisplayClass20_0 A_0)
		{
			if (A_0.width <= 40.0)
			{
				return 1.0;
			}
			return 0.0;
		}

		// Token: 0x040002E8 RID: 744
		private const string s_ActiveStateName = "Active";

		// Token: 0x040002E9 RID: 745
		private const string s_InactiveStateName = "Inactive";

		// Token: 0x040002EA RID: 746
		private const string s_SmallStateName = "Small";

		// Token: 0x040002EB RID: 747
		private const string s_LargeStateName = "Large";

		// Token: 0x040002EC RID: 748
		public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(ProgressRing), new FrameworkPropertyMetadata(new PropertyChangedCallback(ProgressRing.OnIsActivePropertyChanged)));

		// Token: 0x040002ED RID: 749
		private static readonly DependencyPropertyKey TemplateSettingsPropertyKey = DependencyProperty.RegisterReadOnly("TemplateSettings", typeof(ProgressRingTemplateSettings), typeof(ProgressRing), null);

		// Token: 0x040002EE RID: 750
		public static readonly DependencyProperty TemplateSettingsProperty = ProgressRing.TemplateSettingsPropertyKey.DependencyProperty;
	}
}
