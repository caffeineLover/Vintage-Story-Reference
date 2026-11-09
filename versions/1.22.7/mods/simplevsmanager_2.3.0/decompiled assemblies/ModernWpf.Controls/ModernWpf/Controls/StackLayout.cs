using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls
{
	// Token: 0x020000BD RID: 189
	public class StackLayout : VirtualizingLayout, IFlowLayoutAlgorithmDelegates
	{
		// Token: 0x06000953 RID: 2387 RVA: 0x00020D60 File Offset: 0x0001EF60
		public StackLayout()
		{
			base.LayoutId = "StackLayout";
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x00020D7E File Offset: 0x0001EF7E
		// (set) Token: 0x06000955 RID: 2389 RVA: 0x00020D90 File Offset: 0x0001EF90
		public bool DisableVirtualization
		{
			get
			{
				return (bool)base.GetValue(StackLayout.DisableVirtualizationProperty);
			}
			set
			{
				base.SetValue(StackLayout.DisableVirtualizationProperty, value);
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x00020DA3 File Offset: 0x0001EFA3
		// (set) Token: 0x06000957 RID: 2391 RVA: 0x00020DB5 File Offset: 0x0001EFB5
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(StackLayout.OrientationProperty);
			}
			set
			{
				base.SetValue(StackLayout.OrientationProperty, value);
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x00020DC8 File Offset: 0x0001EFC8
		// (set) Token: 0x06000959 RID: 2393 RVA: 0x00020DDA File Offset: 0x0001EFDA
		public double Spacing
		{
			get
			{
				return (double)base.GetValue(StackLayout.SpacingProperty);
			}
			set
			{
				base.SetValue(StackLayout.SpacingProperty, value);
			}
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x00020DED File Offset: 0x0001EFED
		private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((StackLayout)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x00020DFC File Offset: 0x0001EFFC
		protected override void InitializeForContextCore(VirtualizingLayoutContext context)
		{
			object layoutState = context.LayoutState;
			StackLayoutState stackLayoutState = null;
			if (layoutState != null)
			{
				stackLayoutState = this.GetAsStackState(layoutState);
			}
			if (stackLayoutState == null)
			{
				if (layoutState != null)
				{
					throw new Exception("LayoutState must derive from StackLayoutState.");
				}
				stackLayoutState = new StackLayoutState();
			}
			stackLayoutState.InitializeForContext(context, this);
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x00020E3C File Offset: 0x0001F03C
		protected override void UninitializeForContextCore(VirtualizingLayoutContext context)
		{
			this.GetAsStackState(context.LayoutState).UninitializeForContext(context);
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x00020E50 File Offset: 0x0001F050
		protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
		{
			this.GetAsStackState(context.LayoutState).OnMeasureStart();
			return this.GetFlowAlgorithm(context).Measure(availableSize, context, false, 0.0, this.m_itemSpacing, uint.MaxValue, this.OM.ScrollOrientation, this.DisableVirtualization, base.LayoutId);
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x00020EA4 File Offset: 0x0001F0A4
		protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
		{
			return this.GetFlowAlgorithm(context).Arrange(finalSize, context, false, FlowLayoutAlgorithm.LineAlignment.Start, base.LayoutId);
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x00020EBC File Offset: 0x0001F0BC
		protected override void OnItemsChangedCore(VirtualizingLayoutContext context, object source, NotifyCollectionChangedEventArgs args)
		{
			this.GetFlowAlgorithm(context).OnItemsSourceChanged(source, args, context);
			this.InvalidateLayout();
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x00020ED4 File Offset: 0x0001F0D4
		private FlowLayoutAnchorInfo GetAnchorForRealizationRect(Size availableSize, VirtualizingLayoutContext context)
		{
			int num = -1;
			double offset = double.NaN;
			int itemCount = context.ItemCount;
			if (itemCount > 0)
			{
				Rect realizationRect = context.RealizationRect;
				StackLayoutState asStackState = this.GetAsStackState(context.LayoutState);
				Rect lastExtent = asStackState.FlowAlgorithm.LastExtent;
				double num2 = this.GetAverageElementSize(availableSize, context, asStackState) + this.m_itemSpacing;
				double num3 = this.OM.MajorStart(realizationRect) - this.OM.MajorStart(lastExtent);
				double num4 = (this.OM.MajorSize(lastExtent) == 0.0) ? Math.Max(0.0, num2 * (double)itemCount - this.m_itemSpacing) : this.OM.MajorSize(lastExtent);
				if (itemCount > 0 && this.OM.MajorSize(realizationRect) >= 0.0 && num3 + this.OM.MajorSize(realizationRect) >= 0.0 && num3 <= num4)
				{
					num = (int)(num3 / num2);
					offset = (double)num * num2 + this.OM.MajorStart(lastExtent);
					num = Math.Max(0, Math.Min(itemCount - 1, num));
				}
			}
			return new FlowLayoutAnchorInfo
			{
				Index = num,
				Offset = offset
			};
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x00021010 File Offset: 0x0001F210
		private Rect GetExtent(Size availableSize, VirtualizingLayoutContext context, UIElement firstRealized, int firstRealizedItemIndex, Rect firstRealizedLayoutBounds, UIElement lastRealized, int lastRealizedItemIndex, Rect lastRealizedLayoutBounds)
		{
			Rect rect = default(Rect);
			int itemCount = context.ItemCount;
			StackLayoutState asStackState = this.GetAsStackState(context.LayoutState);
			double num = this.GetAverageElementSize(availableSize, context, asStackState) + this.m_itemSpacing;
			this.OM.SetMinorSize(ref rect, asStackState.MaxArrangeBounds);
			this.OM.SetMajorSize(ref rect, Math.Max(0.0, (double)itemCount * num - this.m_itemSpacing));
			if (itemCount > 0 && firstRealized != null)
			{
				this.OM.SetMajorStart(ref rect, this.OM.MajorStart(firstRealizedLayoutBounds) - (double)firstRealizedItemIndex * num);
				int num2 = itemCount - lastRealizedItemIndex - 1;
				this.OM.SetMajorSize(ref rect, this.OM.MajorEnd(lastRealizedLayoutBounds) - this.OM.MajorStart(rect) + (double)num2 * num);
			}
			return rect;
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x000210E0 File Offset: 0x0001F2E0
		private void OnElementMeasured(UIElement element, int index, Size availableSize, Size measureSize, Size desiredSize, Size provisionalArrangeSize, VirtualizingLayoutContext context)
		{
			if (context != null)
			{
				this.GetAsStackState(context.LayoutState).OnElementMeasured(index, this.OM.Major(provisionalArrangeSize), this.OM.Minor(provisionalArrangeSize));
			}
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x00021121 File Offset: 0x0001F321
		Size IFlowLayoutAlgorithmDelegates.Algorithm_GetMeasureSize(int index, Size availableSize, VirtualizingLayoutContext context)
		{
			return availableSize;
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x00021124 File Offset: 0x0001F324
		Size IFlowLayoutAlgorithmDelegates.Algorithm_GetProvisionalArrangeSize(int index, Size measureSize, Size desiredSize, VirtualizingLayoutContext context)
		{
			double num = this.OM.Minor(measureSize);
			return this.OM.MinorMajorSize((!double.IsInfinity(num)) ? Math.Max(num, this.OM.Minor(desiredSize)) : this.OM.Minor(desiredSize), this.OM.Major(desiredSize));
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0002117D File Offset: 0x0001F37D
		bool IFlowLayoutAlgorithmDelegates.Algorithm_ShouldBreakLine(int index, double remainingSpace)
		{
			return true;
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x00021180 File Offset: 0x0001F380
		FlowLayoutAnchorInfo IFlowLayoutAlgorithmDelegates.Algorithm_GetAnchorForRealizationRect(Size availableSize, VirtualizingLayoutContext context)
		{
			return this.GetAnchorForRealizationRect(availableSize, context);
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0002118C File Offset: 0x0001F38C
		FlowLayoutAnchorInfo IFlowLayoutAlgorithmDelegates.Algorithm_GetAnchorForTargetElement(int targetIndex, Size availableSize, VirtualizingLayoutContext context)
		{
			double offset = double.NaN;
			int num = -1;
			int itemCount = context.ItemCount;
			if (targetIndex >= 0 && targetIndex < itemCount)
			{
				num = targetIndex;
				StackLayoutState asStackState = this.GetAsStackState(context.LayoutState);
				double num2 = this.GetAverageElementSize(availableSize, context, asStackState) + this.m_itemSpacing;
				offset = (double)num * num2 + this.OM.MajorStart(asStackState.FlowAlgorithm.LastExtent);
			}
			return new FlowLayoutAnchorInfo
			{
				Index = num,
				Offset = offset
			};
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0002120C File Offset: 0x0001F40C
		Rect IFlowLayoutAlgorithmDelegates.Algorithm_GetExtent(Size availableSize, VirtualizingLayoutContext context, UIElement firstRealized, int firstRealizedItemIndex, Rect firstRealizedLayoutBounds, UIElement lastRealized, int lastRealizedItemIndex, Rect lastRealizedLayoutBounds)
		{
			return this.GetExtent(availableSize, context, firstRealized, firstRealizedItemIndex, firstRealizedLayoutBounds, lastRealized, lastRealizedItemIndex, lastRealizedLayoutBounds);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0002122C File Offset: 0x0001F42C
		void IFlowLayoutAlgorithmDelegates.Algorithm_OnElementMeasured(UIElement element, int index, Size availableSize, Size measureSize, Size desiredSize, Size provisionalArrangeSize, VirtualizingLayoutContext context)
		{
			this.OnElementMeasured(element, index, availableSize, measureSize, desiredSize, provisionalArrangeSize, context);
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0002123F File Offset: 0x0001F43F
		void IFlowLayoutAlgorithmDelegates.Algorithm_OnLineArranged(int startIndex, int countInLine, double lineSize, VirtualizingLayoutContext context)
		{
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x00021244 File Offset: 0x0001F444
		private void PrivateOnPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			DependencyProperty property = args.Property;
			if (property == StackLayout.OrientationProperty)
			{
				ScrollOrientation scrollOrientation = ((Orientation)args.NewValue == null) ? ScrollOrientation.Horizontal : ScrollOrientation.Vertical;
				this.OM.ScrollOrientation = scrollOrientation;
			}
			else if (property == StackLayout.SpacingProperty)
			{
				this.m_itemSpacing = (double)args.NewValue;
			}
			this.InvalidateLayout();
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x000212A4 File Offset: 0x0001F4A4
		private double GetAverageElementSize(Size availableSize, VirtualizingLayoutContext context, StackLayoutState stackLayoutState)
		{
			double result = 0.0;
			if (context.ItemCount > 0)
			{
				if (stackLayoutState.TotalElementsMeasured == 0)
				{
					UIElement orCreateElementAt = context.GetOrCreateElementAt(0, ElementRealizationOptions.ForceCreate | ElementRealizationOptions.SuppressAutoRecycle);
					stackLayoutState.FlowAlgorithm.MeasureElement(orCreateElementAt, 0, availableSize, context);
					context.RecycleElement(orCreateElementAt);
				}
				result = Math.Round(stackLayoutState.TotalElementSize / (double)stackLayoutState.TotalElementsMeasured, 1);
			}
			return result;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x00021302 File Offset: 0x0001F502
		private StackLayoutState GetAsStackState(object state)
		{
			return state as StackLayoutState;
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0002130A File Offset: 0x0001F50A
		private void InvalidateLayout()
		{
			base.InvalidateMeasure();
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x00021312 File Offset: 0x0001F512
		private FlowLayoutAlgorithm GetFlowAlgorithm(VirtualizingLayoutContext context)
		{
			return this.GetAsStackState(context.LayoutState).FlowAlgorithm;
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x00021325 File Offset: 0x0001F525
		private OrientationBasedMeasures OM { get; } = new OrientationBasedMeasures();

		// Token: 0x040003E6 RID: 998
		public static readonly DependencyProperty DisableVirtualizationProperty = DependencyProperty.Register("DisableVirtualization", typeof(bool), typeof(StackLayout), new PropertyMetadata(false, new PropertyChangedCallback(StackLayout.OnPropertyChanged)));

		// Token: 0x040003E7 RID: 999
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(StackLayout), new PropertyMetadata(1, new PropertyChangedCallback(StackLayout.OnPropertyChanged)));

		// Token: 0x040003E8 RID: 1000
		public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register("Spacing", typeof(double), typeof(StackLayout), new PropertyMetadata(0.0, new PropertyChangedCallback(StackLayout.OnPropertyChanged)));

		// Token: 0x040003E9 RID: 1001
		private double m_itemSpacing;
	}
}
