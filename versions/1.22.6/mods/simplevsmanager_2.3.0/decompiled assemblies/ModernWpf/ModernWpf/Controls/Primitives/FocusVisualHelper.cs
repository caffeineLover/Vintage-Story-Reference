using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x0200028F RID: 655
	public static class FocusVisualHelper
	{
		// Token: 0x06001349 RID: 4937 RVA: 0x00038ADC File Offset: 0x00036CDC
		public static Brush GetFocusVisualPrimaryBrush(FrameworkElement element)
		{
			return (Brush)element.GetValue(FocusVisualHelper.FocusVisualPrimaryBrushProperty);
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x00038AEE File Offset: 0x00036CEE
		public static void SetFocusVisualPrimaryBrush(FrameworkElement element, Brush value)
		{
			element.SetValue(FocusVisualHelper.FocusVisualPrimaryBrushProperty, value);
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x00038AFC File Offset: 0x00036CFC
		public static Brush GetFocusVisualSecondaryBrush(FrameworkElement element)
		{
			return (Brush)element.GetValue(FocusVisualHelper.FocusVisualSecondaryBrushProperty);
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x00038B0E File Offset: 0x00036D0E
		public static void SetFocusVisualSecondaryBrush(FrameworkElement element, Brush value)
		{
			element.SetValue(FocusVisualHelper.FocusVisualSecondaryBrushProperty, value);
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x00038B1C File Offset: 0x00036D1C
		public static Thickness GetFocusVisualPrimaryThickness(FrameworkElement element)
		{
			return (Thickness)element.GetValue(FocusVisualHelper.FocusVisualPrimaryThicknessProperty);
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x00038B2E File Offset: 0x00036D2E
		public static void SetFocusVisualPrimaryThickness(FrameworkElement element, Thickness value)
		{
			element.SetValue(FocusVisualHelper.FocusVisualPrimaryThicknessProperty, value);
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x00038B41 File Offset: 0x00036D41
		public static Thickness GetFocusVisualSecondaryThickness(FrameworkElement element)
		{
			return (Thickness)element.GetValue(FocusVisualHelper.FocusVisualSecondaryThicknessProperty);
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x00038B53 File Offset: 0x00036D53
		public static void SetFocusVisualSecondaryThickness(FrameworkElement element, Thickness value)
		{
			element.SetValue(FocusVisualHelper.FocusVisualSecondaryThicknessProperty, value);
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x00038B66 File Offset: 0x00036D66
		public static Thickness GetFocusVisualMargin(FrameworkElement element)
		{
			return (Thickness)element.GetValue(FocusVisualHelper.FocusVisualMarginProperty);
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x00038B78 File Offset: 0x00036D78
		public static void SetFocusVisualMargin(FrameworkElement element, Thickness value)
		{
			element.SetValue(FocusVisualHelper.FocusVisualMarginProperty, value);
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x00038B8B File Offset: 0x00036D8B
		public static bool GetUseSystemFocusVisuals(Control control)
		{
			return (bool)control.GetValue(FocusVisualHelper.UseSystemFocusVisualsProperty);
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x00038B9D File Offset: 0x00036D9D
		public static void SetUseSystemFocusVisuals(Control control, bool value)
		{
			control.SetValue(FocusVisualHelper.UseSystemFocusVisualsProperty, value);
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x00038BB0 File Offset: 0x00036DB0
		public static bool GetIsTemplateFocusTarget(FrameworkElement element)
		{
			return (bool)element.GetValue(FocusVisualHelper.IsTemplateFocusTargetProperty);
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x00038BC2 File Offset: 0x00036DC2
		public static void SetIsTemplateFocusTarget(FrameworkElement element, bool value)
		{
			element.SetValue(FocusVisualHelper.IsTemplateFocusTargetProperty, value);
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x00038BD8 File Offset: 0x00036DD8
		private static void OnIsTemplateFocusTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			Control control = frameworkElement.TemplatedParent as Control;
			if (control != null)
			{
				if ((bool)e.NewValue)
				{
					FocusVisualHelper.SetTemplateFocusTarget(control, frameworkElement);
					return;
				}
				control.ClearValue(FocusVisualHelper.TemplateFocusTargetProperty);
			}
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x00038C1C File Offset: 0x00036E1C
		public static bool GetIsSystemFocusVisual(Control control)
		{
			return (bool)control.GetValue(FocusVisualHelper.IsSystemFocusVisualProperty);
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x00038C2E File Offset: 0x00036E2E
		public static void SetIsSystemFocusVisual(Control control, bool value)
		{
			control.SetValue(FocusVisualHelper.IsSystemFocusVisualProperty, value);
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x00038C44 File Offset: 0x00036E44
		private static void OnIsSystemFocusVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Control control = (Control)d;
			if ((bool)e.NewValue)
			{
				control.IsVisibleChanged += new DependencyPropertyChangedEventHandler(FocusVisualHelper.OnFocusVisualIsVisibleChanged);
				return;
			}
			control.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(FocusVisualHelper.OnFocusVisualIsVisibleChanged);
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x00038C8B File Offset: 0x00036E8B
		public static bool GetShowFocusVisual(FrameworkElement element)
		{
			return (bool)element.GetValue(FocusVisualHelper.ShowFocusVisualProperty);
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x00038C9D File Offset: 0x00036E9D
		private static void SetShowFocusVisual(FrameworkElement element, bool value)
		{
			element.SetValue(FocusVisualHelper.ShowFocusVisualPropertyKey, value);
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x00038CB0 File Offset: 0x00036EB0
		private static void OnShowFocusVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Control control = d as Control;
			if (control != null)
			{
				FrameworkElement templateFocusTarget = FocusVisualHelper.GetTemplateFocusTarget(control);
				if (templateFocusTarget != null)
				{
					if ((bool)e.NewValue)
					{
						bool flag = true;
						Control control2 = templateFocusTarget as Control;
						if (control2 != null)
						{
							flag = FocusVisualHelper.GetUseSystemFocusVisuals(control2);
						}
						if (flag)
						{
							FocusVisualHelper.<OnShowFocusVisualChanged>g__ShowFocusVisual|30_1(control, templateFocusTarget);
							return;
						}
					}
					else
					{
						FocusVisualHelper.<OnShowFocusVisualChanged>g__HideFocusVisual|30_0();
					}
				}
			}
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x00038D02 File Offset: 0x00036F02
		private static FrameworkElement GetFocusedElement(Control focusVisual)
		{
			return (FrameworkElement)focusVisual.GetValue(FocusVisualHelper.FocusedElementProperty);
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x00038D14 File Offset: 0x00036F14
		private static void SetFocusedElement(Control focusVisual, FrameworkElement value)
		{
			focusVisual.SetValue(FocusVisualHelper.FocusedElementProperty, value);
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x00038D22 File Offset: 0x00036F22
		private static FrameworkElement GetTemplateFocusTarget(Control control)
		{
			return (FrameworkElement)control.GetValue(FocusVisualHelper.TemplateFocusTargetProperty);
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x00038D34 File Offset: 0x00036F34
		private static void SetTemplateFocusTarget(Control control, FrameworkElement value)
		{
			control.SetValue(FocusVisualHelper.TemplateFocusTargetProperty, value);
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x00038D44 File Offset: 0x00036F44
		private static void OnFocusVisualIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			Control control = (Control)sender;
			if ((bool)e.NewValue)
			{
				Adorner adorner = VisualTreeHelper.GetParent(control) as Adorner;
				FrameworkElement frameworkElement = ((adorner != null) ? adorner.AdornedElement : null) as FrameworkElement;
				if (frameworkElement != null)
				{
					FocusVisualHelper.SetShowFocusVisual(frameworkElement, true);
					Control control2 = frameworkElement as Control;
					if (control2 != null && (!FocusVisualHelper.GetUseSystemFocusVisuals(control2) || FocusVisualHelper.GetTemplateFocusTarget(control2) != null))
					{
						control.Template = null;
					}
					else
					{
						FocusVisualHelper.TransferValue(frameworkElement, control, FocusVisualHelper.FocusVisualPrimaryBrushProperty);
						FocusVisualHelper.TransferValue(frameworkElement, control, FocusVisualHelper.FocusVisualPrimaryThicknessProperty);
						FocusVisualHelper.TransferValue(frameworkElement, control, FocusVisualHelper.FocusVisualSecondaryBrushProperty);
						FocusVisualHelper.TransferValue(frameworkElement, control, FocusVisualHelper.FocusVisualSecondaryThicknessProperty);
						control.Margin = FocusVisualHelper.GetFocusVisualMargin(frameworkElement);
					}
					FocusVisualHelper.SetFocusedElement(control, frameworkElement);
					return;
				}
			}
			else
			{
				FrameworkElement focusedElement = FocusVisualHelper.GetFocusedElement(control);
				if (focusedElement != null)
				{
					focusedElement.ClearValue(FocusVisualHelper.ShowFocusVisualPropertyKey);
					control.ClearValue(FocusVisualHelper.FocusVisualPrimaryBrushProperty);
					control.ClearValue(FocusVisualHelper.FocusVisualPrimaryThicknessProperty);
					control.ClearValue(FocusVisualHelper.FocusVisualSecondaryBrushProperty);
					control.ClearValue(FocusVisualHelper.FocusVisualSecondaryThicknessProperty);
					control.ClearValue(FrameworkElement.MarginProperty);
					control.ClearValue(Control.TemplateProperty);
					control.ClearValue(FocusVisualHelper.FocusedElementProperty);
				}
			}
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x00038E5C File Offset: 0x0003705C
		private static void TransferValue(DependencyObject source, DependencyObject target, DependencyProperty dp)
		{
			if (!source.HasDefaultValue(dp))
			{
				target.SetValue(dp, source.GetValue(dp));
			}
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x0003909C File Offset: 0x0003729C
		[CompilerGenerated]
		internal static void <OnShowFocusVisualChanged>g__HideFocusVisual|30_0()
		{
			if (FocusVisualHelper._focusVisualAdornerCache != null)
			{
				AdornerLayer adornerLayer = VisualTreeHelper.GetParent(FocusVisualHelper._focusVisualAdornerCache) as AdornerLayer;
				if (adornerLayer != null)
				{
					adornerLayer.Remove(FocusVisualHelper._focusVisualAdornerCache);
				}
				FocusVisualHelper._focusVisualAdornerCache = null;
			}
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x000390D4 File Offset: 0x000372D4
		[CompilerGenerated]
		internal static void <OnShowFocusVisualChanged>g__ShowFocusVisual|30_1(Control control, FrameworkElement target)
		{
			FocusVisualHelper.<OnShowFocusVisualChanged>g__HideFocusVisual|30_0();
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(target);
			if (adornerLayer == null)
			{
				return;
			}
			Style style = target.FocusVisualStyle;
			if (style != null && style.BasedOn == null && style.Setters.Count == 0)
			{
				style = (target.TryFindResource(SystemParameters.FocusVisualStyleKey) as Style);
			}
			if (style != null)
			{
				FocusVisualHelper._focusVisualAdornerCache = new FocusVisualHelper.FocusVisualAdorner(control, target, style);
				adornerLayer.Add(FocusVisualHelper._focusVisualAdornerCache);
				control.IsVisibleChanged += new DependencyPropertyChangedEventHandler(FocusVisualHelper.<OnShowFocusVisualChanged>g__OnControlIsVisibleChanged|30_2);
			}
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x0003914E File Offset: 0x0003734E
		[CompilerGenerated]
		internal static void <OnShowFocusVisualChanged>g__OnControlIsVisibleChanged|30_2(object sender, DependencyPropertyChangedEventArgs e)
		{
			((Control)sender).IsVisibleChanged -= new DependencyPropertyChangedEventHandler(FocusVisualHelper.<OnShowFocusVisualChanged>g__OnControlIsVisibleChanged|30_2);
			if (FocusVisualHelper._focusVisualAdornerCache != null && FocusVisualHelper._focusVisualAdornerCache.FocusedElement == sender)
			{
				FocusVisualHelper.<OnShowFocusVisualChanged>g__HideFocusVisual|30_0();
			}
		}

		// Token: 0x04000673 RID: 1651
		public static readonly DependencyProperty FocusVisualPrimaryBrushProperty = DependencyProperty.RegisterAttached("FocusVisualPrimaryBrush", typeof(Brush), typeof(FocusVisualHelper));

		// Token: 0x04000674 RID: 1652
		public static readonly DependencyProperty FocusVisualSecondaryBrushProperty = DependencyProperty.RegisterAttached("FocusVisualSecondaryBrush", typeof(Brush), typeof(FocusVisualHelper));

		// Token: 0x04000675 RID: 1653
		public static readonly DependencyProperty FocusVisualPrimaryThicknessProperty = DependencyProperty.RegisterAttached("FocusVisualPrimaryThickness", typeof(Thickness), typeof(FocusVisualHelper), new FrameworkPropertyMetadata(new Thickness(2.0)));

		// Token: 0x04000676 RID: 1654
		public static readonly DependencyProperty FocusVisualSecondaryThicknessProperty = DependencyProperty.RegisterAttached("FocusVisualSecondaryThickness", typeof(Thickness), typeof(FocusVisualHelper), new FrameworkPropertyMetadata(new Thickness(1.0)));

		// Token: 0x04000677 RID: 1655
		public static readonly DependencyProperty FocusVisualMarginProperty = DependencyProperty.RegisterAttached("FocusVisualMargin", typeof(Thickness), typeof(FocusVisualHelper), new FrameworkPropertyMetadata(default(Thickness)));

		// Token: 0x04000678 RID: 1656
		public static readonly DependencyProperty UseSystemFocusVisualsProperty = DependencyProperty.RegisterAttached("UseSystemFocusVisuals", typeof(bool), typeof(FocusVisualHelper), new PropertyMetadata(false));

		// Token: 0x04000679 RID: 1657
		public static readonly DependencyProperty IsTemplateFocusTargetProperty = DependencyProperty.RegisterAttached("IsTemplateFocusTarget", typeof(bool), typeof(FocusVisualHelper), new PropertyMetadata(new PropertyChangedCallback(FocusVisualHelper.OnIsTemplateFocusTargetChanged)));

		// Token: 0x0400067A RID: 1658
		public static readonly DependencyProperty IsSystemFocusVisualProperty = DependencyProperty.RegisterAttached("IsSystemFocusVisual", typeof(bool), typeof(FocusVisualHelper), new PropertyMetadata(new PropertyChangedCallback(FocusVisualHelper.OnIsSystemFocusVisualChanged)));

		// Token: 0x0400067B RID: 1659
		private static readonly DependencyPropertyKey ShowFocusVisualPropertyKey = DependencyProperty.RegisterAttachedReadOnly("ShowFocusVisual", typeof(bool), typeof(FocusVisualHelper), new PropertyMetadata(new PropertyChangedCallback(FocusVisualHelper.OnShowFocusVisualChanged)));

		// Token: 0x0400067C RID: 1660
		public static readonly DependencyProperty ShowFocusVisualProperty = FocusVisualHelper.ShowFocusVisualPropertyKey.DependencyProperty;

		// Token: 0x0400067D RID: 1661
		private static readonly DependencyProperty FocusedElementProperty = DependencyProperty.RegisterAttached("FocusedElement", typeof(FrameworkElement), typeof(FocusVisualHelper));

		// Token: 0x0400067E RID: 1662
		private static readonly DependencyProperty TemplateFocusTargetProperty = DependencyProperty.RegisterAttached("TemplateFocusTarget", typeof(FrameworkElement), typeof(FocusVisualHelper));

		// Token: 0x0400067F RID: 1663
		private static FocusVisualHelper.FocusVisualAdorner _focusVisualAdornerCache = null;

		// Token: 0x02000417 RID: 1047
		private sealed class FocusVisualAdorner : Adorner
		{
			// Token: 0x06001C00 RID: 7168 RVA: 0x0004B004 File Offset: 0x00049204
			public FocusVisualAdorner(Control focusedElement, UIElement adornedElement, Style focusVisualStyle) : base(adornedElement)
			{
				this.FocusedElement = focusedElement;
				Control control = new Control();
				FocusVisualHelper.SetIsSystemFocusVisual(control, false);
				control.Style = focusVisualStyle;
				control.Margin = FocusVisualHelper.GetFocusVisualMargin(focusedElement);
				FocusVisualHelper.TransferValue(focusedElement, control, FocusVisualHelper.FocusVisualPrimaryBrushProperty);
				FocusVisualHelper.TransferValue(focusedElement, control, FocusVisualHelper.FocusVisualPrimaryThicknessProperty);
				FocusVisualHelper.TransferValue(focusedElement, control, FocusVisualHelper.FocusVisualSecondaryBrushProperty);
				FocusVisualHelper.TransferValue(focusedElement, control, FocusVisualHelper.FocusVisualSecondaryThicknessProperty);
				this._adorderChild = control;
				base.IsClipEnabled = true;
				base.IsHitTestVisible = false;
				base.IsEnabled = false;
				base.AddVisualChild(this._adorderChild);
			}

			// Token: 0x17000518 RID: 1304
			// (get) Token: 0x06001C01 RID: 7169 RVA: 0x0004B097 File Offset: 0x00049297
			public Control FocusedElement { get; }

			// Token: 0x06001C02 RID: 7170 RVA: 0x0004B0A0 File Offset: 0x000492A0
			protected override Size MeasureOverride(Size constraint)
			{
				Size renderSize = base.AdornedElement.RenderSize;
				((UIElement)this.GetVisualChild(0)).Measure(renderSize);
				return renderSize;
			}

			// Token: 0x06001C03 RID: 7171 RVA: 0x0004B0CC File Offset: 0x000492CC
			protected override Size ArrangeOverride(Size size)
			{
				Size size2 = base.ArrangeOverride(size);
				((UIElement)this.GetVisualChild(0)).Arrange(new Rect(default(Point), size2));
				return size2;
			}

			// Token: 0x17000519 RID: 1305
			// (get) Token: 0x06001C04 RID: 7172 RVA: 0x0004B102 File Offset: 0x00049302
			protected override int VisualChildrenCount
			{
				get
				{
					return 1;
				}
			}

			// Token: 0x06001C05 RID: 7173 RVA: 0x0004B105 File Offset: 0x00049305
			protected override Visual GetVisualChild(int index)
			{
				if (index == 0)
				{
					return this._adorderChild;
				}
				throw new ArgumentOutOfRangeException("index");
			}

			// Token: 0x04000A17 RID: 2583
			private UIElement _adorderChild;
		}
	}
}
