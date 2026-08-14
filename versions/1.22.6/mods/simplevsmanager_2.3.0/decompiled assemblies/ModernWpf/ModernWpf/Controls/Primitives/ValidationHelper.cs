using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020002AC RID: 684
	public static class ValidationHelper
	{
		// Token: 0x060014B5 RID: 5301 RVA: 0x0003DC74 File Offset: 0x0003BE74
		public static bool GetIsTemplateValidationAdornerSite(FrameworkElement element)
		{
			return (bool)element.GetValue(ValidationHelper.IsTemplateValidationAdornerSiteProperty);
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x0003DC86 File Offset: 0x0003BE86
		public static void SetIsTemplateValidationAdornerSite(FrameworkElement element, bool value)
		{
			element.SetValue(ValidationHelper.IsTemplateValidationAdornerSiteProperty, value);
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x0003DC9C File Offset: 0x0003BE9C
		private static void OnIsTemplateValidationAdornerSiteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			if ((bool)e.NewValue)
			{
				Validation.SetErrorTemplate(frameworkElement, null);
				Validation.SetValidationAdornerSiteFor(frameworkElement, frameworkElement.TemplatedParent);
				return;
			}
			frameworkElement.ClearValue(Validation.ErrorTemplateProperty);
			frameworkElement.ClearValue(Validation.ValidationAdornerSiteForProperty);
		}

		// Token: 0x040006F4 RID: 1780
		public static readonly DependencyProperty IsTemplateValidationAdornerSiteProperty = DependencyProperty.RegisterAttached("IsTemplateValidationAdornerSite", typeof(bool), typeof(ValidationHelper), new PropertyMetadata(new PropertyChangedCallback(ValidationHelper.OnIsTemplateValidationAdornerSiteChanged)));
	}
}
