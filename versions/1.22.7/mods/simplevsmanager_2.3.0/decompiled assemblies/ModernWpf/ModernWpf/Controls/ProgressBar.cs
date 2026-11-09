using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using ModernWpf.Automation.Peers;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x0200027D RID: 637
	[TemplatePart(Name = "LayoutRoot", Type = typeof(Grid))]
	[TemplatePart(Name = "DeterminateProgressBarIndicator", Type = typeof(Rectangle))]
	[TemplatePart(Name = "IndeterminateProgressBarIndicator", Type = typeof(Rectangle))]
	[TemplatePart(Name = "IndeterminateProgressBarIndicator2", Type = typeof(Rectangle))]
	[TemplateVisualState(GroupName = "CommonStates", Name = "Determinate")]
	[TemplateVisualState(GroupName = "CommonStates", Name = "Indeterminate")]
	public class ProgressBar : RangeBase
	{
		// Token: 0x0600126A RID: 4714 RVA: 0x00035B70 File Offset: 0x00033D70
		static ProgressBar()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(typeof(ProgressBar)));
			Control.PaddingProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(new PropertyChangedCallback(ProgressBar.OnPaddingChanged)));
			Control.BackgroundProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata
			{
				CoerceValueCallback = new CoerceValueCallback(ProgressBar.CoerceBrush)
			});
			Control.ForegroundProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata
			{
				CoerceValueCallback = new CoerceValueCallback(ProgressBar.CoerceBrush)
			});
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x00035D09 File Offset: 0x00033F09
		public ProgressBar()
		{
			base.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
			base.SetValue(ProgressBar.TemplateSettingsPropertyKey, new ProgressBarTemplateSettings());
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x0600126C RID: 4716 RVA: 0x00035D33 File Offset: 0x00033F33
		// (set) Token: 0x0600126D RID: 4717 RVA: 0x00035D45 File Offset: 0x00033F45
		public bool IsIndeterminate
		{
			get
			{
				return (bool)base.GetValue(ProgressBar.IsIndeterminateProperty);
			}
			set
			{
				base.SetValue(ProgressBar.IsIndeterminateProperty, value);
			}
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x00035D58 File Offset: 0x00033F58
		private static void OnIsIndeterminatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((ProgressBar)sender).OnIsIndeterminatePropertyChanged(args);
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x0600126F RID: 4719 RVA: 0x00035D66 File Offset: 0x00033F66
		// (set) Token: 0x06001270 RID: 4720 RVA: 0x00035D78 File Offset: 0x00033F78
		public bool ShowError
		{
			get
			{
				return (bool)base.GetValue(ProgressBar.ShowErrorProperty);
			}
			set
			{
				base.SetValue(ProgressBar.ShowErrorProperty, value);
			}
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x00035D8B File Offset: 0x00033F8B
		private static void OnShowErrorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((ProgressBar)sender).OnShowErrorPropertyChanged(args);
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06001272 RID: 4722 RVA: 0x00035D99 File Offset: 0x00033F99
		// (set) Token: 0x06001273 RID: 4723 RVA: 0x00035DAB File Offset: 0x00033FAB
		public bool ShowPaused
		{
			get
			{
				return (bool)base.GetValue(ProgressBar.ShowPausedProperty);
			}
			set
			{
				base.SetValue(ProgressBar.ShowPausedProperty, value);
			}
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x00035DBE File Offset: 0x00033FBE
		private static void OnShowPausedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((ProgressBar)sender).OnShowPausedPropertyChanged(args);
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001275 RID: 4725 RVA: 0x00035DCC File Offset: 0x00033FCC
		// (set) Token: 0x06001276 RID: 4726 RVA: 0x00035DDE File Offset: 0x00033FDE
		public ProgressBarTemplateSettings TemplateSettings
		{
			get
			{
				return (ProgressBarTemplateSettings)base.GetValue(ProgressBar.TemplateSettingsProperty);
			}
			private set
			{
				base.SetValue(ProgressBar.TemplateSettingsPropertyKey, value);
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06001277 RID: 4727 RVA: 0x00035DEC File Offset: 0x00033FEC
		// (set) Token: 0x06001278 RID: 4728 RVA: 0x00035DFE File Offset: 0x00033FFE
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(ProgressBar.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(ProgressBar.CornerRadiusProperty, value);
			}
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x00035E11 File Offset: 0x00034011
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ProgressBarAutomationPeer(this);
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x00035E1C File Offset: 0x0003401C
		public override void OnApplyTemplate()
		{
			ThemeManager.RemoveActualThemeChangedHandler(this, new RoutedEventHandler(this.OnActualThemeChanged));
			base.OnApplyTemplate();
			this.m_layoutRoot = (base.GetTemplateChild("LayoutRoot") as Grid);
			this.m_determinateProgressBarIndicator = (base.GetTemplateChild("DeterminateProgressBarIndicator") as Rectangle);
			this.m_indeterminateProgressBarIndicator = (base.GetTemplateChild("IndeterminateProgressBarIndicator") as Rectangle);
			this.m_indeterminateProgressBarIndicator2 = (base.GetTemplateChild("IndeterminateProgressBarIndicator2") as Rectangle);
			this.UpdateStates(true);
			ThemeManager.AddActualThemeChangedHandler(this, new RoutedEventHandler(this.OnActualThemeChanged));
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00035EB2 File Offset: 0x000340B2
		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.SetProgressBarIndicatorWidth();
			this.UpdateWidthBasedTemplateSettings();
			this.ReapplyIndeterminateStoryboard();
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x00035EC6 File Offset: 0x000340C6
		protected override void OnValueChanged(double oldValue, double newValue)
		{
			base.OnValueChanged(oldValue, newValue);
			this.OnIndicatorWidthComponentChanged();
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x00035ED6 File Offset: 0x000340D6
		protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
		{
			base.OnMinimumChanged(oldMinimum, newMinimum);
			this.OnIndicatorWidthComponentChanged();
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x00035EE6 File Offset: 0x000340E6
		protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
		{
			base.OnMaximumChanged(oldMaximum, newMaximum);
			this.OnIndicatorWidthComponentChanged();
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00035EF6 File Offset: 0x000340F6
		private static void OnPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ProgressBar)d).OnIndicatorWidthComponentChanged();
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x00035F03 File Offset: 0x00034103
		private void OnIndicatorWidthComponentChanged()
		{
			this.SetProgressBarIndicatorWidth();
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x00035F0B File Offset: 0x0003410B
		private void OnIsIndeterminatePropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.SetProgressBarIndicatorWidth();
			this.UpdateStates(true);
			this.ReapplyIndeterminateStoryboard();
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00035F20 File Offset: 0x00034120
		private void OnShowPausedPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.UpdateStates(true);
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x00035F29 File Offset: 0x00034129
		private void OnShowErrorPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.UpdateStates(true);
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x00035F34 File Offset: 0x00034134
		private void UpdateStates(bool useTransitions = true)
		{
			if (this.IsIndeterminate)
			{
				if (this.ShowError)
				{
					VisualStateManager.GoToState(this, "IndeterminateError", true);
				}
				else if (this.ShowPaused)
				{
					VisualStateManager.GoToState(this, "IndeterminatePaused", true);
				}
				else
				{
					VisualStateManager.GoToState(this, "Indeterminate", true);
				}
				this.UpdateWidthBasedTemplateSettings();
				return;
			}
			if (this.ShowError)
			{
				VisualStateManager.GoToState(this, "Error", true);
				return;
			}
			if (this.ShowPaused)
			{
				VisualStateManager.GoToState(this, "Paused", true);
				return;
			}
			VisualStateManager.GoToState(this, "Determinate", true);
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x00035FC4 File Offset: 0x000341C4
		private void SetProgressBarIndicatorWidth()
		{
			ProgressBarTemplateSettings templateSettings = this.TemplateSettings;
			Grid layoutRoot = this.m_layoutRoot;
			if (layoutRoot != null)
			{
				Rectangle determinateProgressBarIndicator = this.m_determinateProgressBarIndicator;
				if (determinateProgressBarIndicator != null)
				{
					double actualWidth = layoutRoot.ActualWidth;
					double actualWidth2 = determinateProgressBarIndicator.ActualWidth;
					double maximum = base.Maximum;
					double minimum = base.Minimum;
					Thickness padding = base.Padding;
					if (this.ShowError)
					{
						VisualStateManager.GoToState(this, "UpdatingError", true);
					}
					else
					{
						VisualStateManager.GoToState(this, "Updating", true);
					}
					if (this.IsIndeterminate)
					{
						this.m_determinateProgressBarIndicator.Width = 0.0;
						if (this.m_indeterminateProgressBarIndicator != null)
						{
							this.m_indeterminateProgressBarIndicator.Width = actualWidth * 0.4;
						}
						if (this.m_indeterminateProgressBarIndicator2 != null)
						{
							this.m_indeterminateProgressBarIndicator2.Width = actualWidth * 0.6;
						}
					}
					else if (Math.Abs(maximum - minimum) > 5E-324)
					{
						double num = (actualWidth - (padding.Left + padding.Right)) / (maximum - minimum) * (base.Value - minimum);
						double num2 = num - actualWidth2;
						templateSettings.IndicatorLengthDelta = -num2;
						this.m_determinateProgressBarIndicator.Width = num;
					}
					else
					{
						this.m_determinateProgressBarIndicator.Width = 0.0;
					}
					this.UpdateStates(true);
				}
			}
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x0003610C File Offset: 0x0003430C
		private void UpdateWidthBasedTemplateSettings()
		{
			ProgressBarTemplateSettings templateSettings = this.TemplateSettings;
			double num;
			double num2;
			if (this.m_layoutRoot != null)
			{
				num = this.m_layoutRoot.ActualWidth;
				num2 = this.m_layoutRoot.ActualHeight;
			}
			else
			{
				num = 0.0;
				num2 = 0.0;
			}
			double num3 = num * 0.4;
			double num4 = num * 0.6;
			templateSettings.ContainerAnimationStartPosition = num3 * -1.0;
			templateSettings.ContainerAnimationEndPosition = num3 * 3.0;
			templateSettings.Container2AnimationStartPosition = num4 * -1.5;
			templateSettings.Container2AnimationEndPosition = num4 * 1.66;
			templateSettings.ContainerAnimationMidPosition = num * 0.2;
			Thickness padding = base.Padding;
			RectangleGeometry rectangleGeometry = new RectangleGeometry(new Rect(padding.Left, padding.Top, num - (padding.Right + padding.Left), num2 - (padding.Bottom + padding.Top)));
			if (this.m_indeterminateProgressBarIndicator != null)
			{
				rectangleGeometry.RadiusX = this.m_indeterminateProgressBarIndicator.RadiusX;
				rectangleGeometry.RadiusY = this.m_indeterminateProgressBarIndicator.RadiusY;
			}
			templateSettings.ClipRect = rectangleGeometry;
			templateSettings.EllipseAnimationEndPosition = 0.3333333333333333 * num;
			templateSettings.EllipseAnimationWellPosition = 0.6666666666666666 * num;
			if (num <= 180.0)
			{
				templateSettings.EllipseDiameter = 4.0;
				templateSettings.EllipseOffset = 4.0;
				return;
			}
			if (num <= 280.0)
			{
				templateSettings.EllipseDiameter = 5.0;
				templateSettings.EllipseOffset = 7.0;
				return;
			}
			templateSettings.EllipseDiameter = 6.0;
			templateSettings.EllipseOffset = 9.0;
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x000362D4 File Offset: 0x000344D4
		private static object CoerceBrush(DependencyObject d, object baseValue)
		{
			Brush brush = baseValue as Brush;
			if (brush != null && !brush.IsFrozen)
			{
				Brush brush2 = brush.CloneCurrentValue();
				brush2.Freeze();
				return brush2;
			}
			return baseValue;
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x00036301 File Offset: 0x00034501
		private void OnActualThemeChanged(object sender, RoutedEventArgs e)
		{
			base.Dispatcher.BeginInvoke(new Action(this.RefreshStates), 7, Array.Empty<object>());
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x00036321 File Offset: 0x00034521
		private void RefreshStates()
		{
			VisualStateManager.GoToState(this, "Updating", false);
			this.UpdateStates(false);
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x00036337 File Offset: 0x00034537
		private void ReapplyIndeterminateStoryboard()
		{
			base.Dispatcher.BeginInvoke(delegate()
			{
				if (this.IsIndeterminate)
				{
					this.RefreshStates();
				}
			}, 7, Array.Empty<object>());
		}

		// Token: 0x040005EE RID: 1518
		public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register("IsIndeterminate", typeof(bool), typeof(ProgressBar), new PropertyMetadata(false, new PropertyChangedCallback(ProgressBar.OnIsIndeterminatePropertyChanged)));

		// Token: 0x040005EF RID: 1519
		public static readonly DependencyProperty ShowErrorProperty = DependencyProperty.Register("ShowError", typeof(bool), typeof(ProgressBar), new PropertyMetadata(new PropertyChangedCallback(ProgressBar.OnShowErrorPropertyChanged)));

		// Token: 0x040005F0 RID: 1520
		public static readonly DependencyProperty ShowPausedProperty = DependencyProperty.Register("ShowPaused", typeof(bool), typeof(ProgressBar), new PropertyMetadata(new PropertyChangedCallback(ProgressBar.OnShowPausedPropertyChanged)));

		// Token: 0x040005F1 RID: 1521
		private static readonly DependencyPropertyKey TemplateSettingsPropertyKey = DependencyProperty.RegisterReadOnly("TemplateSettings", typeof(ProgressBarTemplateSettings), typeof(ProgressBar), null);

		// Token: 0x040005F2 RID: 1522
		public static readonly DependencyProperty TemplateSettingsProperty = ProgressBar.TemplateSettingsPropertyKey.DependencyProperty;

		// Token: 0x040005F3 RID: 1523
		public static readonly DependencyProperty CornerRadiusProperty = ControlHelper.CornerRadiusProperty.AddOwner(typeof(ProgressBar));

		// Token: 0x040005F4 RID: 1524
		private Grid m_layoutRoot;

		// Token: 0x040005F5 RID: 1525
		private Rectangle m_determinateProgressBarIndicator;

		// Token: 0x040005F6 RID: 1526
		private Rectangle m_indeterminateProgressBarIndicator;

		// Token: 0x040005F7 RID: 1527
		private Rectangle m_indeterminateProgressBarIndicator2;

		// Token: 0x040005F8 RID: 1528
		private const string s_LayoutRootName = "LayoutRoot";

		// Token: 0x040005F9 RID: 1529
		private const string s_DeterminateProgressBarIndicatorName = "DeterminateProgressBarIndicator";

		// Token: 0x040005FA RID: 1530
		private const string s_IndeterminateProgressBarIndicatorName = "IndeterminateProgressBarIndicator";

		// Token: 0x040005FB RID: 1531
		private const string s_IndeterminateProgressBarIndicator2Name = "IndeterminateProgressBarIndicator2";

		// Token: 0x040005FC RID: 1532
		private const string s_ErrorStateName = "Error";

		// Token: 0x040005FD RID: 1533
		private const string s_PausedStateName = "Paused";

		// Token: 0x040005FE RID: 1534
		private const string s_IndeterminateStateName = "Indeterminate";

		// Token: 0x040005FF RID: 1535
		private const string s_IndeterminateErrorStateName = "IndeterminateError";

		// Token: 0x04000600 RID: 1536
		private const string s_IndeterminatePausedStateName = "IndeterminatePaused";

		// Token: 0x04000601 RID: 1537
		private const string s_DeterminateStateName = "Determinate";

		// Token: 0x04000602 RID: 1538
		private const string s_UpdatingStateName = "Updating";

		// Token: 0x04000603 RID: 1539
		private const string s_UpdatingWithErrorStateName = "UpdatingError";
	}
}
