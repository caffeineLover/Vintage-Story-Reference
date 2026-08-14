using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x0200004A RID: 74
	public class GridViewItem : ListViewBaseItem
	{
		// Token: 0x060002C1 RID: 705 RVA: 0x00009870 File Offset: 0x00007A70
		static GridViewItem()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(GridViewItem), new FrameworkPropertyMetadata(typeof(GridViewItem)));
		}
	}
}
