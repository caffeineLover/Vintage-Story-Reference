using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x0200004D RID: 77
	public class ListView : ListViewBase
	{
		// Token: 0x060002CA RID: 714 RVA: 0x000098B6 File Offset: 0x00007AB6
		static ListView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ListView), new FrameworkPropertyMetadata(typeof(ListView)));
		}

		// Token: 0x060002CC RID: 716 RVA: 0x000098E3 File Offset: 0x00007AE3
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is ListViewItem;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x000098EE File Offset: 0x00007AEE
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new ListViewItem();
		}
	}
}
