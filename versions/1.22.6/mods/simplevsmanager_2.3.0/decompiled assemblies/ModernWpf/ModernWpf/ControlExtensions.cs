using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModernWpf
{
	// Token: 0x02000212 RID: 530
	internal static class ControlExtensions
	{
		// Token: 0x06000F9D RID: 3997 RVA: 0x0002C97C File Offset: 0x0002AB7C
		public static FrameworkElement GetTemplateRoot(this Control control)
		{
			if (VisualTreeHelper.GetChildrenCount(control) > 0)
			{
				return VisualTreeHelper.GetChild(control, 0) as FrameworkElement;
			}
			return null;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0002C995 File Offset: 0x0002AB95
		public static T GetTemplateChild<T>(this Control control, string childName) where T : DependencyObject
		{
			ControlTemplate template = control.Template;
			return ((template != null) ? template.FindName(childName, control) : null) as T;
		}
	}
}
