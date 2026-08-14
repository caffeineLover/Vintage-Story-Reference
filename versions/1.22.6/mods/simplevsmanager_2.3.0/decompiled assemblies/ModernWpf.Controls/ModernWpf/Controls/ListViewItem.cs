using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x02000050 RID: 80
	public class ListViewItem : ListViewBaseItem
	{
		// Token: 0x060002FE RID: 766 RVA: 0x00009F7C File Offset: 0x0000817C
		static ListViewItem()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ListViewItem), new FrameworkPropertyMetadata(typeof(ListViewItem)));
		}
	}
}
