using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000B3 RID: 179
	internal interface IFlowLayoutAlgorithmDelegates
	{
		// Token: 0x06000904 RID: 2308
		Size Algorithm_GetMeasureSize(int index, Size availableSize, VirtualizingLayoutContext context);

		// Token: 0x06000905 RID: 2309
		Size Algorithm_GetProvisionalArrangeSize(int index, Size measureSize, Size desiredSize, VirtualizingLayoutContext context);

		// Token: 0x06000906 RID: 2310
		bool Algorithm_ShouldBreakLine(int index, double remainingSpace);

		// Token: 0x06000907 RID: 2311
		FlowLayoutAnchorInfo Algorithm_GetAnchorForRealizationRect(Size availableSize, VirtualizingLayoutContext context);

		// Token: 0x06000908 RID: 2312
		FlowLayoutAnchorInfo Algorithm_GetAnchorForTargetElement(int targetIndex, Size availableSize, VirtualizingLayoutContext context);

		// Token: 0x06000909 RID: 2313
		Rect Algorithm_GetExtent(Size availableSize, VirtualizingLayoutContext context, UIElement firstRealized, int firstRealizedItemIndex, Rect firstRealizedLayoutBounds, UIElement lastRealized, int lastRealizedItemIndex, Rect lastRealizedLayoutBounds);

		// Token: 0x0600090A RID: 2314
		void Algorithm_OnElementMeasured(UIElement element, int index, Size availableSize, Size measureSize, Size desiredSize, Size provisionalArrangeSize, VirtualizingLayoutContext context);

		// Token: 0x0600090B RID: 2315
		void Algorithm_OnLineArranged(int startIndex, int countInLine, double lineSize, VirtualizingLayoutContext context);
	}
}
