using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace ModernWpf.Controls
{
	// Token: 0x02000096 RID: 150
	internal class CachedVisualTreeHelpers
	{
		// Token: 0x06000751 RID: 1873 RVA: 0x0001A17B File Offset: 0x0001837B
		public static Rect GetLayoutSlot(FrameworkElement element)
		{
			return LayoutInformation.GetLayoutSlot(element);
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0001A184 File Offset: 0x00018384
		public static DependencyObject GetParent(DependencyObject element)
		{
			if (element is Visual || element is Visual3D)
			{
				return VisualTreeHelper.GetParent(element);
			}
			FrameworkContentElement frameworkContentElement = element as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				return frameworkContentElement.Parent;
			}
			return null;
		}
	}
}
