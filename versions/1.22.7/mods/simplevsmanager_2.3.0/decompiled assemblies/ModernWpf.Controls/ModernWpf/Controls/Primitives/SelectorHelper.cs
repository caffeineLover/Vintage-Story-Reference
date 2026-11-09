using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020000EB RID: 235
	internal static class SelectorHelper
	{
		// Token: 0x06000C18 RID: 3096 RVA: 0x0002960C File Offset: 0x0002780C
		internal static bool ItemGetIsSelectable(object item)
		{
			return item != null && !(item is Separator);
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00029620 File Offset: 0x00027820
		internal static bool UiGetIsSelectable(DependencyObject o)
		{
			if (o != null)
			{
				if (!SelectorHelper.ItemGetIsSelectable(o))
				{
					return false;
				}
				ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(o);
				if (itemsControl != null)
				{
					object obj = itemsControl.ItemContainerGenerator.ItemFromContainer(o);
					return obj == o || SelectorHelper.ItemGetIsSelectable(obj);
				}
			}
			return false;
		}
	}
}
