using System;
using System.Collections.Specialized;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000C7 RID: 199
	internal interface IVirtualizingLayoutOverrides
	{
		// Token: 0x060009DC RID: 2524
		void InitializeForContextCore(VirtualizingLayoutContext context);

		// Token: 0x060009DD RID: 2525
		void UninitializeForContextCore(VirtualizingLayoutContext context);

		// Token: 0x060009DE RID: 2526
		Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize);

		// Token: 0x060009DF RID: 2527
		Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize);

		// Token: 0x060009E0 RID: 2528
		void OnItemsChangedCore(VirtualizingLayoutContext context, object source, NotifyCollectionChangedEventArgs args);
	}
}
