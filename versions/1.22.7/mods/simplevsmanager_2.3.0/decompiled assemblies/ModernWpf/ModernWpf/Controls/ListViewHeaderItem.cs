using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x02000266 RID: 614
	public class ListViewHeaderItem : ListViewBaseHeaderItem
	{
		// Token: 0x06001189 RID: 4489 RVA: 0x00033755 File Offset: 0x00031955
		static ListViewHeaderItem()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ListViewHeaderItem), new FrameworkPropertyMetadata(typeof(ListViewHeaderItem)));
		}
	}
}
