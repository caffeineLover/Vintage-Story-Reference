using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls
{
	// Token: 0x020000C0 RID: 192
	public class UniformGridLayout : VirtualizingLayout, IFlowLayoutAlgorithmDelegates
	{
		// Token: 0x0600097E RID: 2430 RVA: 0x00021534 File Offset: 0x0001F734
		public UniformGridLayout()
		{
			base.LayoutId = "UniformGridLayout";
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x00021582 File Offset: 0x0001F782
		// (set) Token: 0x06000980 RID: 2432 RVA: 0x00021594 File Offset: 0x0001F794
		public UniformGridLayoutItemsJustification ItemsJustification
		{
			get
			{
				return (UniformGridLayoutItemsJustification)base.GetValue(UniformGridLayout.ItemsJustificationProperty);
			}
			set
			{
				base.SetValue(UniformGridLayout.ItemsJustificationProperty, value);
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x000215A7 File Offset: 0x0001F7A7
		// (set) Token: 0x06000982 RID: 2434 RVA: 0x000215B9 File Offset: 0x0001F7B9
		public UniformGridLayoutItemsStretch ItemsStretch
		{
			get
			{
				return (UniformGridLayoutItemsStretch)base.GetValue(UniformGridLayout.ItemsStretchProperty);
			}
			set
			{
				base.SetValue(UniformGridLayout.ItemsStretchProperty, value);
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x000215CC File Offset: 0x0001F7CC
		// (set) Token: 0x06000984 RID: 2436 RVA: 0x000215DE File Offset: 0x0001F7DE
		public int MaximumRowsOrColumns
		{
			get
			{
				return (int)base.GetValue(UniformGridLayout.MaximumRowsOrColumnsProperty);
			}
			set
			{
				base.SetValue(UniformGridLayout.MaximumRowsOrColumnsProperty, value);
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x000215F1 File Offset: 0x0001F7F1
		// (set) Token: 0x06000986 RID: 2438 RVA: 0x00021603 File Offset: 0x0001F803
		public double MinColumnSpacing
		{
			get
			{
				return (double)base.GetValue(UniformGridLayout.MinColumnSpacingProperty);
			}
			set
			{
				base.SetValue(UniformGridLayout.MinColumnSpacingProperty, value);
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x00021616 File Offset: 0x0001F816
		// (set) Token: 0x06000988 RID: 2440 RVA: 0x00021628 File Offset: 0x0001F828
		public double MinItemHeight
		{
			get
			{
				return (double)base.GetValue(UniformGridLayout.MinItemHeightProperty);
			}
			set
			{
				base.SetValue(UniformGridLayout.MinItemHeightProperty, value);
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x0002163B File Offset: 0x0001F83B
		// (set) Token: 0x0600098A RID: 2442 RVA: 0x0002164D File Offset: 0x0001F84D
		public double MinItemWidth
		{
			get
			{
				return (double)base.GetValue(UniformGridLayout.MinItemWidthProperty);
			}
			set
			{
				base.SetValue(UniformGridLayout.MinItemWidthProperty, value);
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x00021660 File Offset: 0x0001F860
		// (set) Token: 0x0600098C RID: 2444 RVA: 0x00021672 File Offset: 0x0001F872
		public double MinRowSpacing
		{
			get
			{
				return (double)base.GetValue(UniformGridLayout.MinRowSpacingProperty);
			}
			set
			{
				base.SetValue(UniformGridLayout.MinRowSpacingProperty, value);
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x00021685 File Offset: 0x0001F885
		// (set) Token: 0x0600098E RID: 2446 RVA: 0x00021697 File Offset: 0x0001F897
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(UniformGridLayout.OrientationProperty);
			}
			set
			{
				base.SetValue(UniformGridLayout.OrientationProperty, value);
			}
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x000216AA File Offset: 0x0001F8AA
		private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((UniformGridLayout)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x000216B8 File Offset: 0x0001F8B8
		protected override void InitializeForContextCore(VirtualizingLayoutContext context)
		{
			object layoutState = context.LayoutState;
			UniformGridLayoutState uniformGridLayoutState = null;
			if (layoutState != null)
			{
				uniformGridLayoutState = this.GetAsGridState(layoutState);
			}
			if (uniformGridLayoutState == null)
			{
				if (layoutState != null)
				{
					throw new Exception("LayoutState must derive from UniformGridLayoutState.");
				}
				uniformGridLayoutState = new UniformGridLayoutState();
			}
			uniformGridLayoutState.InitializeForContext(context, this);
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x000216F8 File Offset: 0x0001F8F8
		protected override void UninitializeForContextCore(VirtualizingLayoutContext context)
		{
			this.GetAsGridState(context.LayoutState).UninitializeForContext(context);
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0002170C File Offset: 0x0001F90C
		protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
		{
			this.GetAsGridState(context.LayoutState).EnsureElementSize(availableSize, context, this.m_minItemWidth, this.m_minItemHeight, this.m_itemsStretch, this.Orientation, this.MinRowSpacing, this.MinColumnSpacing, this.m_maximumRowsOrColumns);
			return this.GetFlowAlgorithm(context).Measure(availableSize, context, true, this.MinItemSpacing, this.LineSpacing, this.m_maximumRowsOrColumns, this.OM.ScrollOrientation, false, base.LayoutId);
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00021789 File Offset: 0x0001F989
		protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
		{
			return this.GetFlowAlgorithm(context).Arrange(finalSize, context, true, (FlowLayoutAlgorithm.LineAlignment)this.m_itemsJustification, base.LayoutId);
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x000217A6 File Offset: 0x0001F9A6
		protected override void OnItemsChangedCore(VirtualizingLayoutContext context, object source, NotifyCollectionChangedEventArgs args)
		{
			this.GetFlowAlgorithm(context).OnItemsSourceChanged(source, args, context);
			this.InvalidateLayout();
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x000217C0 File Offset: 0x0001F9C0
		Size IFlowLayoutAlgorithmDelegates.Algorithm_GetMeasureSize(int index, Size availableSize, VirtualizingLayoutContext context)
		{
			UniformGridLayoutState asGridState = this.GetAsGridState(context.LayoutState);
			return new Size(asGridState.EffectiveItemWidth, asGridState.EffectiveItemHeight);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x000217EC File Offset: 0x0001F9EC
		Size IFlowLayoutAlgorithmDelegates.Algorithm_GetProvisionalArrangeSize(int index, Size measureSize, Size desiredSize, VirtualizingLayoutContext context)
		{
			UniformGridLayoutState asGridState = this.GetAsGridState(context.LayoutState);
			return new Size(asGridState.EffectiveItemWidth, asGridState.EffectiveItemHeight);
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x00021818 File Offset: 0x0001FA18
		bool IFlowLayoutAlgorithmDelegates.Algorithm_ShouldBreakLine(int index, double remainingSpace)
		{
			return remainingSpace < 0.0;
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x00021828 File Offset: 0x0001FA28
		FlowLayoutAnchorInfo IFlowLayoutAlgorithmDelegates.Algorithm_GetAnchorForRealizationRect(Size availableSize, VirtualizingLayoutContext context)
		{
			Rect layoutRectForDataIndex;
			layoutRectForDataIndex..ctor(double.NaN, double.NaN, double.NaN, double.NaN);
			int index = -1;
			int itemCount = context.ItemCount;
			Rect realizationRect = context.RealizationRect;
			if (itemCount > 0 && this.OM.MajorSize(realizationRect) > 0.0)
			{
				Rect lastExtent = this.GetAsGridState(context.LayoutState).FlowAlgorithm.LastExtent;
				int num = (int)Math.Min(Math.Max(1U, (uint)(this.OM.Minor(availableSize) / this.GetMinorSizeWithSpacing(context))), Math.Max(1U, this.m_maximumRowsOrColumns));
				double num2 = (double)(itemCount / num) * this.GetMajorSizeWithSpacing(context);
				double num3 = this.OM.MajorStart(realizationRect) - this.OM.MajorStart(lastExtent);
				if (num3 + this.OM.MajorSize(realizationRect) >= 0.0 && num3 <= num2)
				{
					int num4 = (int)(Math.Max(0.0, this.OM.MajorStart(realizationRect) - this.OM.MajorStart(lastExtent)) / this.GetMajorSizeWithSpacing(context));
					index = Math.Max(0, Math.Min(itemCount - 1, num4 * num));
					layoutRectForDataIndex = this.GetLayoutRectForDataIndex(availableSize, index, lastExtent, context);
				}
			}
			return new FlowLayoutAnchorInfo
			{
				Index = index,
				Offset = this.OM.MajorStart(layoutRectForDataIndex)
			};
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0002199C File Offset: 0x0001FB9C
		FlowLayoutAnchorInfo IFlowLayoutAlgorithmDelegates.Algorithm_GetAnchorForTargetElement(int targetIndex, Size availableSize, VirtualizingLayoutContext context)
		{
			int index = -1;
			double offset = double.NaN;
			int itemCount = context.ItemCount;
			if (targetIndex >= 0 && targetIndex < itemCount)
			{
				int num = (int)Math.Min(Math.Max(1.0, (uint)this.OM.Minor(availableSize) / this.GetMinorSizeWithSpacing(context)), Math.Max(1U, this.m_maximumRowsOrColumns));
				int num2 = targetIndex / num * num;
				index = num2;
				UniformGridLayoutState asGridState = this.GetAsGridState(context.LayoutState);
				offset = this.OM.MajorStart(this.GetLayoutRectForDataIndex(availableSize, num2, asGridState.FlowAlgorithm.LastExtent, context));
			}
			return new FlowLayoutAnchorInfo
			{
				Index = index,
				Offset = offset
			};
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x00021A54 File Offset: 0x0001FC54
		Rect IFlowLayoutAlgorithmDelegates.Algorithm_GetExtent(Size availableSize, VirtualizingLayoutContext context, UIElement firstRealized, int firstRealizedItemIndex, Rect firstRealizedLayoutBounds, UIElement lastRealized, int lastRealizedItemIndex, Rect lastRealizedLayoutBounds)
		{
			Rect rect = default(Rect);
			int itemCount = context.ItemCount;
			double num = this.OM.Minor(availableSize);
			int num2 = (int)Math.Min(Math.Max(1U, (!double.IsInfinity(num)) ? ((uint)(num / this.GetMinorSizeWithSpacing(context))) : ((uint)itemCount)), Math.Max(1U, this.m_maximumRowsOrColumns));
			double majorSizeWithSpacing = this.GetMajorSizeWithSpacing(context);
			if (itemCount > 0)
			{
				this.OM.SetMinorSize(ref rect, (!double.IsInfinity(num) && this.m_itemsStretch == UniformGridLayoutItemsStretch.Fill) ? num : Math.Max(0.0, (double)num2 * this.GetMinorSizeWithSpacing(context) - this.MinItemSpacing));
				this.OM.SetMajorSize(ref rect, Math.Max(0.0, (double)(itemCount / num2) * majorSizeWithSpacing - this.LineSpacing));
				if (firstRealized != null)
				{
					this.OM.SetMajorStart(ref rect, this.OM.MajorStart(firstRealizedLayoutBounds) - (double)(firstRealizedItemIndex / num2) * majorSizeWithSpacing);
					int num3 = itemCount - lastRealizedItemIndex - 1;
					this.OM.SetMajorSize(ref rect, this.OM.MajorEnd(lastRealizedLayoutBounds) - this.OM.MajorStart(rect) + (double)(num3 / num2) * majorSizeWithSpacing);
				}
			}
			return rect;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x00021B82 File Offset: 0x0001FD82
		void IFlowLayoutAlgorithmDelegates.Algorithm_OnElementMeasured(UIElement element, int index, Size availableSize, Size measureSize, Size desiredSize, Size provisionalArrangeSize, VirtualizingLayoutContext context)
		{
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x00021B84 File Offset: 0x0001FD84
		void IFlowLayoutAlgorithmDelegates.Algorithm_OnLineArranged(int startIndex, int countInLine, double lineSize, VirtualizingLayoutContext context)
		{
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00021B88 File Offset: 0x0001FD88
		private void PrivateOnPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			DependencyProperty property = args.Property;
			if (property == UniformGridLayout.OrientationProperty)
			{
				ScrollOrientation scrollOrientation = ((Orientation)args.NewValue == null) ? ScrollOrientation.Vertical : ScrollOrientation.Horizontal;
				this.OM.ScrollOrientation = scrollOrientation;
			}
			else if (property == UniformGridLayout.MinColumnSpacingProperty)
			{
				this.m_minColumnSpacing = (double)args.NewValue;
			}
			else if (property == UniformGridLayout.MinRowSpacingProperty)
			{
				this.m_minRowSpacing = (double)args.NewValue;
			}
			else if (property == UniformGridLayout.ItemsJustificationProperty)
			{
				this.m_itemsJustification = (UniformGridLayoutItemsJustification)args.NewValue;
			}
			else if (property == UniformGridLayout.ItemsStretchProperty)
			{
				this.m_itemsStretch = (UniformGridLayoutItemsStretch)args.NewValue;
			}
			else if (property == UniformGridLayout.MinItemWidthProperty)
			{
				this.m_minItemWidth = (double)args.NewValue;
			}
			else if (property == UniformGridLayout.MinItemHeightProperty)
			{
				this.m_minItemHeight = (double)args.NewValue;
			}
			else if (property == UniformGridLayout.MaximumRowsOrColumnsProperty)
			{
				this.m_maximumRowsOrColumns = (uint)((int)args.NewValue);
			}
			this.InvalidateLayout();
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x00021C98 File Offset: 0x0001FE98
		private double GetMinorSizeWithSpacing(VirtualizingLayoutContext context)
		{
			double minItemSpacing = this.MinItemSpacing;
			UniformGridLayoutState asGridState = this.GetAsGridState(context.LayoutState);
			if (this.OM.ScrollOrientation != ScrollOrientation.Vertical)
			{
				return asGridState.EffectiveItemHeight + minItemSpacing;
			}
			return asGridState.EffectiveItemWidth + minItemSpacing;
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x00021CD8 File Offset: 0x0001FED8
		private double GetMajorSizeWithSpacing(VirtualizingLayoutContext context)
		{
			double lineSpacing = this.LineSpacing;
			UniformGridLayoutState asGridState = this.GetAsGridState(context.LayoutState);
			if (this.OM.ScrollOrientation != ScrollOrientation.Vertical)
			{
				return asGridState.EffectiveItemWidth + lineSpacing;
			}
			return asGridState.EffectiveItemHeight + lineSpacing;
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x00021D18 File Offset: 0x0001FF18
		private Rect GetLayoutRectForDataIndex(Size availableSize, int index, Rect lastExtent, VirtualizingLayoutContext context)
		{
			int num = (int)Math.Min(Math.Max(1U, (uint)(this.OM.Minor(availableSize) / this.GetMinorSizeWithSpacing(context))), Math.Max(1U, this.m_maximumRowsOrColumns));
			int num2 = index / num;
			int num3 = index - num2 * num;
			UniformGridLayoutState asGridState = this.GetAsGridState(context.LayoutState);
			return this.OM.MinorMajorRect((double)num3 * this.GetMinorSizeWithSpacing(context) + this.OM.MinorStart(lastExtent), (double)num2 * this.GetMajorSizeWithSpacing(context) + this.OM.MajorStart(lastExtent), (this.OM.ScrollOrientation == ScrollOrientation.Vertical) ? asGridState.EffectiveItemWidth : asGridState.EffectiveItemHeight, (this.OM.ScrollOrientation == ScrollOrientation.Vertical) ? asGridState.EffectiveItemHeight : asGridState.EffectiveItemWidth);
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x00021DDC File Offset: 0x0001FFDC
		private UniformGridLayoutState GetAsGridState(object state)
		{
			return state as UniformGridLayoutState;
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00021DE4 File Offset: 0x0001FFE4
		private FlowLayoutAlgorithm GetFlowAlgorithm(VirtualizingLayoutContext context)
		{
			return this.GetAsGridState(context.LayoutState).FlowAlgorithm;
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00021DF7 File Offset: 0x0001FFF7
		private void InvalidateLayout()
		{
			base.InvalidateMeasure();
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x00021DFF File Offset: 0x0001FFFF
		private double LineSpacing
		{
			get
			{
				if (this.Orientation != null)
				{
					return this.m_minColumnSpacing;
				}
				return this.m_minRowSpacing;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x00021E16 File Offset: 0x00020016
		private double MinItemSpacing
		{
			get
			{
				if (this.Orientation != null)
				{
					return this.m_minRowSpacing;
				}
				return this.m_minColumnSpacing;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x00021E2D File Offset: 0x0002002D
		private OrientationBasedMeasures OM { get; } = new OrientationBasedMeasures();

		// Token: 0x040003F3 RID: 1011
		public static readonly DependencyProperty ItemsJustificationProperty = DependencyProperty.Register("ItemsJustification", typeof(UniformGridLayoutItemsJustification), typeof(UniformGridLayout), new PropertyMetadata(UniformGridLayoutItemsJustification.Start, new PropertyChangedCallback(UniformGridLayout.OnPropertyChanged)));

		// Token: 0x040003F4 RID: 1012
		public static readonly DependencyProperty ItemsStretchProperty = DependencyProperty.Register("ItemsStretch", typeof(UniformGridLayoutItemsStretch), typeof(UniformGridLayout), new PropertyMetadata(UniformGridLayoutItemsStretch.None, new PropertyChangedCallback(UniformGridLayout.OnPropertyChanged)));

		// Token: 0x040003F5 RID: 1013
		public static readonly DependencyProperty MaximumRowsOrColumnsProperty = DependencyProperty.Register("MaximumRowsOrColumns", typeof(int), typeof(UniformGridLayout), new PropertyMetadata(-1, new PropertyChangedCallback(UniformGridLayout.OnPropertyChanged)));

		// Token: 0x040003F6 RID: 1014
		public static readonly DependencyProperty MinColumnSpacingProperty = DependencyProperty.Register("MinColumnSpacing", typeof(double), typeof(UniformGridLayout), new PropertyMetadata(new PropertyChangedCallback(UniformGridLayout.OnPropertyChanged)));

		// Token: 0x040003F7 RID: 1015
		public static readonly DependencyProperty MinItemHeightProperty = DependencyProperty.Register("MinItemHeight", typeof(double), typeof(UniformGridLayout), new PropertyMetadata(new PropertyChangedCallback(UniformGridLayout.OnPropertyChanged)));

		// Token: 0x040003F8 RID: 1016
		public static readonly DependencyProperty MinItemWidthProperty = DependencyProperty.Register("MinItemWidth", typeof(double), typeof(UniformGridLayout), new PropertyMetadata(new PropertyChangedCallback(UniformGridLayout.OnPropertyChanged)));

		// Token: 0x040003F9 RID: 1017
		public static readonly DependencyProperty MinRowSpacingProperty = DependencyProperty.Register("MinRowSpacing", typeof(double), typeof(UniformGridLayout), new PropertyMetadata(new PropertyChangedCallback(UniformGridLayout.OnPropertyChanged)));

		// Token: 0x040003FA RID: 1018
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(UniformGridLayout), new PropertyMetadata(0, new PropertyChangedCallback(UniformGridLayout.OnPropertyChanged)));

		// Token: 0x040003FB RID: 1019
		private double m_minItemWidth = double.NaN;

		// Token: 0x040003FC RID: 1020
		private double m_minItemHeight = double.NaN;

		// Token: 0x040003FD RID: 1021
		private double m_minRowSpacing;

		// Token: 0x040003FE RID: 1022
		private double m_minColumnSpacing;

		// Token: 0x040003FF RID: 1023
		private UniformGridLayoutItemsJustification m_itemsJustification;

		// Token: 0x04000400 RID: 1024
		private UniformGridLayoutItemsStretch m_itemsStretch;

		// Token: 0x04000401 RID: 1025
		private uint m_maximumRowsOrColumns = uint.MaxValue;
	}
}
