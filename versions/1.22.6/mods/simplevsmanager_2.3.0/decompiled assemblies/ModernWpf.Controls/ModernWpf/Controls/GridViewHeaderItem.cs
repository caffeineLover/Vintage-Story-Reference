using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x02000049 RID: 73
	public class GridViewHeaderItem : ListViewBaseHeaderItem
	{
		// Token: 0x060002BF RID: 703 RVA: 0x00009843 File Offset: 0x00007A43
		static GridViewHeaderItem()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(GridViewHeaderItem), new FrameworkPropertyMetadata(typeof(GridViewHeaderItem)));
		}
	}
}
