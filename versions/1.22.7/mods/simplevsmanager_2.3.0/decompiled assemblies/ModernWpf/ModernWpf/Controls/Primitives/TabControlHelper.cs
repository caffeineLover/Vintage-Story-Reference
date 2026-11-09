using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020002A6 RID: 678
	public static class TabControlHelper
	{
		// Token: 0x0600144D RID: 5197 RVA: 0x0003C620 File Offset: 0x0003A820
		public static object GetTabStripHeader(TabControl tabControl)
		{
			return tabControl.GetValue(TabControlHelper.TabStripHeaderProperty);
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0003C62D File Offset: 0x0003A82D
		public static void SetTabStripHeader(TabControl tabControl, object value)
		{
			tabControl.SetValue(TabControlHelper.TabStripHeaderProperty, value);
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0003C63B File Offset: 0x0003A83B
		public static DataTemplate GetTabStripHeaderTemplate(TabControl tabControl)
		{
			return (DataTemplate)tabControl.GetValue(TabControlHelper.TabStripHeaderTemplateProperty);
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0003C64D File Offset: 0x0003A84D
		public static void SetTabStripHeaderTemplate(TabControl tabControl, DataTemplate value)
		{
			tabControl.SetValue(TabControlHelper.TabStripHeaderTemplateProperty, value);
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0003C65B File Offset: 0x0003A85B
		public static object GetTabStripFooter(TabControl tabControl)
		{
			return tabControl.GetValue(TabControlHelper.TabStripFooterProperty);
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0003C668 File Offset: 0x0003A868
		public static void SetTabStripFooter(TabControl tabControl, object value)
		{
			tabControl.SetValue(TabControlHelper.TabStripFooterProperty, value);
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x0003C676 File Offset: 0x0003A876
		public static DataTemplate GetTabStripFooterTemplate(TabControl tabControl)
		{
			return (DataTemplate)tabControl.GetValue(TabControlHelper.TabStripFooterTemplateProperty);
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x0003C688 File Offset: 0x0003A888
		public static void SetTabStripFooterTemplate(TabControl tabControl, DataTemplate value)
		{
			tabControl.SetValue(TabControlHelper.TabStripFooterTemplateProperty, value);
		}

		// Token: 0x040006CC RID: 1740
		public static readonly DependencyProperty TabStripHeaderProperty = DependencyProperty.RegisterAttached("TabStripHeader", typeof(object), typeof(TabControlHelper));

		// Token: 0x040006CD RID: 1741
		public static readonly DependencyProperty TabStripHeaderTemplateProperty = DependencyProperty.RegisterAttached("TabStripHeaderTemplate", typeof(DataTemplate), typeof(TabControlHelper));

		// Token: 0x040006CE RID: 1742
		public static readonly DependencyProperty TabStripFooterProperty = DependencyProperty.RegisterAttached("TabStripFooter", typeof(object), typeof(TabControlHelper));

		// Token: 0x040006CF RID: 1743
		public static readonly DependencyProperty TabStripFooterTemplateProperty = DependencyProperty.RegisterAttached("TabStripFooterTemplate", typeof(DataTemplate), typeof(TabControlHelper));
	}
}
