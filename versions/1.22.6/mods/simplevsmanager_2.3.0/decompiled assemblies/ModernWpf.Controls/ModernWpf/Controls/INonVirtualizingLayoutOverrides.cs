using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000BB RID: 187
	internal interface INonVirtualizingLayoutOverrides
	{
		// Token: 0x0600094B RID: 2379
		void InitializeForContextCore(NonVirtualizingLayoutContext context);

		// Token: 0x0600094C RID: 2380
		void UninitializeForContextCore(NonVirtualizingLayoutContext context);

		// Token: 0x0600094D RID: 2381
		Size MeasureOverride(NonVirtualizingLayoutContext context, Size availableSize);

		// Token: 0x0600094E RID: 2382
		Size ArrangeOverride(NonVirtualizingLayoutContext context, Size finalSize);
	}
}
