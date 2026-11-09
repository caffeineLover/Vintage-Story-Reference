using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x02000048 RID: 72
	public class GridView : ListViewBase
	{
		// Token: 0x060002BB RID: 699 RVA: 0x00009804 File Offset: 0x00007A04
		static GridView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(GridView), new FrameworkPropertyMetadata(typeof(GridView)));
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00009831 File Offset: 0x00007A31
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is GridViewItem;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000983C File Offset: 0x00007A3C
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new GridViewItem();
		}
	}
}
