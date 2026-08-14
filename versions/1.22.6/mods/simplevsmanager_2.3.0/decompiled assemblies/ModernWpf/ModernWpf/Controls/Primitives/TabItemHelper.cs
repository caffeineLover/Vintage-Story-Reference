using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020002A7 RID: 679
	public static class TabItemHelper
	{
		// Token: 0x06001456 RID: 5206 RVA: 0x0003C731 File Offset: 0x0003A931
		public static object GetIcon(TabItem tabItem)
		{
			return tabItem.GetValue(TabItemHelper.IconProperty);
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x0003C73E File Offset: 0x0003A93E
		public static void SetIcon(TabItem tabItem, object value)
		{
			tabItem.SetValue(TabItemHelper.IconProperty, value);
		}

		// Token: 0x040006D0 RID: 1744
		public static readonly DependencyProperty IconProperty = DependencyProperty.RegisterAttached("Icon", typeof(object), typeof(TabItemHelper));
	}
}
