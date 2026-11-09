using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000C9 RID: 201
	internal interface IVirtualizingLayoutContextOverrides
	{
		// Token: 0x060009FC RID: 2556
		int ItemCountCore();

		// Token: 0x060009FD RID: 2557
		object GetItemAtCore(int index);

		// Token: 0x060009FE RID: 2558
		UIElement GetOrCreateElementAtCore(int index, ElementRealizationOptions options);

		// Token: 0x060009FF RID: 2559
		void RecycleElementCore(UIElement element);

		// Token: 0x06000A00 RID: 2560
		Rect RealizationRectCore();

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000A01 RID: 2561
		int RecommendedAnchorIndexCore { get; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000A02 RID: 2562
		// (set) Token: 0x06000A03 RID: 2563
		Point LayoutOriginCore { get; set; }
	}
}
