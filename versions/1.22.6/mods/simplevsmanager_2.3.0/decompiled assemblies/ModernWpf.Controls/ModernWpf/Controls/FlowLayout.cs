using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls
{
	// Token: 0x020000AF RID: 175
	public class FlowLayout : VirtualizingLayout, IFlowLayoutAlgorithmDelegates
	{
		// Token: 0x060008B6 RID: 2230 RVA: 0x0001EA18 File Offset: 0x0001CC18
		public FlowLayout()
		{
			base.LayoutId = "FlowLayout";
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x0001EA36 File Offset: 0x0001CC36
		// (set) Token: 0x060008B8 RID: 2232 RVA: 0x0001EA48 File Offset: 0x0001CC48
		public FlowLayoutLineAlignment LineAlignment
		{
			get
			{
				return (FlowLayoutLineAlignment)base.GetValue(FlowLayout.LineAlignmentProperty);
			}
			set
			{
				base.SetValue(FlowLayout.LineAlignmentProperty, value);
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x0001EA5B File Offset: 0x0001CC5B
		// (set) Token: 0x060008BA RID: 2234 RVA: 0x0001EA6D File Offset: 0x0001CC6D
		public double MinColumnSpacing
		{
			get
			{
				return (double)base.GetValue(FlowLayout.MinColumnSpacingProperty);
			}
			set
			{
				base.SetValue(FlowLayout.MinColumnSpacingProperty, value);
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x0001EA80 File Offset: 0x0001CC80
		// (set) Token: 0x060008BC RID: 2236 RVA: 0x0001EA92 File Offset: 0x0001CC92
		public double MinRowSpacing
		{
			get
			{
				return (double)base.GetValue(FlowLayout.MinRowSpacingProperty);
			}
			set
			{
				base.SetValue(FlowLayout.MinRowSpacingProperty, value);
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x0001EAA5 File Offset: 0x0001CCA5
		// (set) Token: 0x060008BE RID: 2238 RVA: 0x0001EAB7 File Offset: 0x0001CCB7
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(FlowLayout.OrientationProperty);
			}
			set
			{
				base.SetValue(FlowLayout.OrientationProperty, value);
			}
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0001EACA File Offset: 0x0001CCCA
		private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((FlowLayout)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0001EAD8 File Offset: 0x0001CCD8
		protected override void InitializeForContextCore(VirtualizingLayoutContext context)
		{
			object layoutState = context.LayoutState;
			FlowLayoutState flowLayoutState = null;
			if (layoutState != null)
			{
				flowLayoutState = this.GetAsFlowState(layoutState);
			}
			if (flowLayoutState == null)
			{
				if (layoutState != null)
				{
					throw new Exception("LayoutState must derive from FlowLayoutState.");
				}
				flowLayoutState = new FlowLayoutState();
			}
			flowLayoutState.InitializeForContext(context, this);
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0001EB18 File Offset: 0x0001CD18
		protected override void UninitializeForContextCore(VirtualizingLayoutContext context)
		{
			this.GetAsFlowState(context.LayoutState).UninitializeForContext(context);
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0001EB2C File Offset: 0x0001CD2C
		protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
		{
			return this.GetFlowAlgorithm(context).Measure(availableSize, context, true, this.MinItemSpacing, this.LineSpacing, uint.MaxValue, this.OM.ScrollOrientation, false, base.LayoutId);
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0001EB67 File Offset: 0x0001CD67
		protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
		{
			return this.GetFlowAlgorithm(context).Arrange(finalSize, context, true, (FlowLayoutAlgorithm.LineAlignment)this.m_lineAlignment, base.LayoutId);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0001EB84 File Offset: 0x0001CD84
		protected override void OnItemsChangedCore(VirtualizingLayoutContext context, object source, NotifyCollectionChangedEventArgs args)
		{
			this.GetFlowAlgorithm(context).OnItemsSourceChanged(source, args, context);
			this.InvalidateLayout();
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0001EB9B File Offset: 0x0001CD9B
		protected virtual Size GetMeasureSize(int index, Size availableSize)
		{
			return availableSize;
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0001EB9E File Offset: 0x0001CD9E
		protected virtual Size GetProvisionalArrangeSize(int index, Size measureSize, Size desiredSize)
		{
			return desiredSize;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0001EBA1 File Offset: 0x0001CDA1
		protected virtual bool ShouldBreakLine(int index, double remainingSpace)
		{
			return remainingSpace < 0.0;
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0001EBB0 File Offset: 0x0001CDB0
		protected virtual FlowLayoutAnchorInfo GetAnchorForRealizationRect(Size availableSize, VirtualizingLayoutContext context)
		{
			int num = -1;
			double offset = double.NaN;
			int itemCount = context.ItemCount;
			if (itemCount > 0)
			{
				Rect realizationRect = context.RealizationRect;
				object layoutState = context.LayoutState;
				FlowLayoutState asFlowState = this.GetAsFlowState(layoutState);
				Rect lastExtent = asFlowState.FlowAlgorithm.LastExtent;
				double num2 = 0.0;
				double num3 = this.GetAverageLineInfo(availableSize, context, asFlowState, ref num2) + this.LineSpacing;
				double majorSize = (this.OM.MajorSize(lastExtent) == 0.0) ? ((double)itemCount / num2 * num3) : this.OM.MajorSize(lastExtent);
				if (itemCount > 0 && this.OM.MajorSize(realizationRect) > 0.0 && this.DoesRealizationWindowOverlapExtent(realizationRect, this.OM.MinorMajorRect(this.OM.MinorStart(lastExtent), this.OM.MajorStart(lastExtent), this.OM.Minor(availableSize), majorSize)))
				{
					double num4 = this.OM.MajorStart(realizationRect) - this.OM.MajorStart(lastExtent);
					int num5 = Math.Max(0, (int)(num4 / num3));
					num = (int)((double)num5 * num2);
					num = Math.Max(0, Math.Min(itemCount - 1, num));
					offset = (double)num5 * num3 + this.OM.MajorStart(lastExtent);
				}
			}
			return new FlowLayoutAnchorInfo
			{
				Index = num,
				Offset = offset
			};
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0001ED18 File Offset: 0x0001CF18
		protected virtual FlowLayoutAnchorInfo GetAnchorForTargetElement(int targetIndex, Size availableSize, VirtualizingLayoutContext context)
		{
			double offset = double.NaN;
			int index = -1;
			int itemCount = context.ItemCount;
			if (targetIndex >= 0 && targetIndex < itemCount)
			{
				index = targetIndex;
				object layoutState = context.LayoutState;
				FlowLayoutState asFlowState = this.GetAsFlowState(layoutState);
				double num = 0.0;
				double num2 = this.GetAverageLineInfo(availableSize, context, asFlowState, ref num) + this.LineSpacing;
				offset = (double)((int)((double)targetIndex / num)) * num2 + this.OM.MajorStart(asFlowState.FlowAlgorithm.LastExtent);
			}
			return new FlowLayoutAnchorInfo
			{
				Index = index,
				Offset = offset
			};
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0001EDB0 File Offset: 0x0001CFB0
		protected virtual Rect GetExtent(Size availableSize, VirtualizingLayoutContext context, UIElement firstRealized, int firstRealizedItemIndex, Rect firstRealizedLayoutBounds, UIElement lastRealized, int lastRealizedItemIndex, Rect lastRealizedLayoutBounds)
		{
			Rect rect = default(Rect);
			int itemCount = context.ItemCount;
			if (itemCount > 0)
			{
				double num = this.OM.Minor(availableSize);
				object layoutState = context.LayoutState;
				FlowLayoutState asFlowState = this.GetAsFlowState(layoutState);
				double num2 = 0.0;
				double num3 = this.GetAverageLineInfo(availableSize, context, asFlowState, ref num2) + this.LineSpacing;
				if (firstRealized != null)
				{
					int num4 = (int)((double)firstRealizedItemIndex / num2);
					double value = this.OM.MajorStart(firstRealizedLayoutBounds) - (double)num4 * num3;
					this.OM.SetMajorStart(ref rect, value);
					int num5 = (int)((double)(itemCount - lastRealizedItemIndex - 1) / num2);
					double value2 = this.OM.MajorEnd(lastRealizedLayoutBounds) - this.OM.MajorStart(rect) + (double)num5 * num3;
					this.OM.SetMajorSize(ref rect, value2);
					this.OM.SetMinorSize(ref rect, (!double.IsInfinity(num)) ? num : Math.Max(0.0, this.OM.MinorEnd(lastRealizedLayoutBounds)));
				}
				else
				{
					double lineSpacing = this.LineSpacing;
					double minItemSpacing = this.MinItemSpacing;
					int num6 = (int)Math.Ceiling((double)itemCount / num2);
					rect = ((!double.IsInfinity(num)) ? this.OM.MinorMajorRect(0.0, 0.0, num, Math.Max(0.0, (double)num6 * num3 - lineSpacing)) : this.OM.MinorMajorRect(0.0, 0.0, Math.Max(0.0, (this.OM.Minor(asFlowState.SpecialElementDesiredSize) + minItemSpacing) * (double)itemCount - minItemSpacing), Math.Max(0.0, num3 - lineSpacing)));
				}
			}
			return rect;
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0001EF72 File Offset: 0x0001D172
		protected virtual void OnElementMeasured(UIElement element, int index, Size availableSize, Size measureSize, Size desiredSize, Size provisionalArrangeSize, VirtualizingLayoutContext context)
		{
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0001EF74 File Offset: 0x0001D174
		protected virtual void OnLineArranged(int startIndex, int countInLine, double lineSize, VirtualizingLayoutContext context)
		{
			this.GetAsFlowState(context.LayoutState).OnLineArranged(startIndex, countInLine, lineSize, context);
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0001EF8D File Offset: 0x0001D18D
		Size IFlowLayoutAlgorithmDelegates.Algorithm_GetMeasureSize(int index, Size availableSize, VirtualizingLayoutContext context)
		{
			return this.GetMeasureSize(index, availableSize);
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0001EF97 File Offset: 0x0001D197
		Size IFlowLayoutAlgorithmDelegates.Algorithm_GetProvisionalArrangeSize(int index, Size measureSize, Size desiredSize, VirtualizingLayoutContext context)
		{
			return this.GetProvisionalArrangeSize(index, measureSize, desiredSize);
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0001EFA2 File Offset: 0x0001D1A2
		bool IFlowLayoutAlgorithmDelegates.Algorithm_ShouldBreakLine(int index, double remainingSpace)
		{
			return this.ShouldBreakLine(index, remainingSpace);
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0001EFAC File Offset: 0x0001D1AC
		FlowLayoutAnchorInfo IFlowLayoutAlgorithmDelegates.Algorithm_GetAnchorForRealizationRect(Size availableSize, VirtualizingLayoutContext context)
		{
			return this.GetAnchorForRealizationRect(availableSize, context);
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0001EFB6 File Offset: 0x0001D1B6
		FlowLayoutAnchorInfo IFlowLayoutAlgorithmDelegates.Algorithm_GetAnchorForTargetElement(int targetIndex, Size availableSize, VirtualizingLayoutContext context)
		{
			return this.GetAnchorForTargetElement(targetIndex, availableSize, context);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0001EFC4 File Offset: 0x0001D1C4
		Rect IFlowLayoutAlgorithmDelegates.Algorithm_GetExtent(Size availableSize, VirtualizingLayoutContext context, UIElement firstRealized, int firstRealizedItemIndex, Rect firstRealizedLayoutBounds, UIElement lastRealized, int lastRealizedItemIndex, Rect lastRealizedLayoutBounds)
		{
			return this.GetExtent(availableSize, context, firstRealized, firstRealizedItemIndex, firstRealizedLayoutBounds, lastRealized, lastRealizedItemIndex, lastRealizedLayoutBounds);
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0001EFE4 File Offset: 0x0001D1E4
		void IFlowLayoutAlgorithmDelegates.Algorithm_OnElementMeasured(UIElement element, int index, Size availableSize, Size measureSize, Size desiredSize, Size provisionalArrangeSize, VirtualizingLayoutContext context)
		{
			this.OnElementMeasured(element, index, availableSize, measureSize, desiredSize, provisionalArrangeSize, context);
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0001EFF7 File Offset: 0x0001D1F7
		void IFlowLayoutAlgorithmDelegates.Algorithm_OnLineArranged(int startIndex, int countInLine, double lineSize, VirtualizingLayoutContext context)
		{
			this.OnLineArranged(startIndex, countInLine, lineSize, context);
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0001F004 File Offset: 0x0001D204
		private void PrivateOnPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			DependencyProperty property = args.Property;
			if (property == FlowLayout.OrientationProperty)
			{
				ScrollOrientation scrollOrientation = ((Orientation)args.NewValue == null) ? ScrollOrientation.Vertical : ScrollOrientation.Horizontal;
				this.OM.ScrollOrientation = scrollOrientation;
			}
			else if (property == FlowLayout.MinColumnSpacingProperty)
			{
				this.m_minColumnSpacing = (double)args.NewValue;
			}
			else if (property == FlowLayout.MinRowSpacingProperty)
			{
				this.m_minRowSpacing = (double)args.NewValue;
			}
			else if (property == FlowLayout.LineAlignmentProperty)
			{
				this.m_lineAlignment = (FlowLayoutLineAlignment)args.NewValue;
			}
			this.InvalidateLayout();
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0001F09C File Offset: 0x0001D29C
		private double GetAverageLineInfo(Size availableSize, VirtualizingLayoutContext context, FlowLayoutState flowState, ref double avgCountInLine)
		{
			avgCountInLine = 1.0;
			if (flowState.TotalLinesMeasured == 0)
			{
				UIElement orCreateElementAt = context.GetOrCreateElementAt(0, ElementRealizationOptions.ForceCreate | ElementRealizationOptions.SuppressAutoRecycle);
				Size size = flowState.FlowAlgorithm.MeasureElement(orCreateElementAt, 0, availableSize, context);
				context.RecycleElement(orCreateElementAt);
				int countInLine = Math.Max(1, (int)(this.OM.Minor(availableSize) / this.OM.Minor(size)));
				flowState.OnLineArranged(0, countInLine, this.OM.Major(size), context);
				flowState.SpecialElementDesiredSize = size;
			}
			avgCountInLine = Math.Max(1.0, flowState.TotalItemsPerLine / (double)flowState.TotalLinesMeasured);
			return Math.Round(flowState.TotalLineSize / (double)flowState.TotalLinesMeasured);
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0001F14C File Offset: 0x0001D34C
		private FlowLayoutState GetAsFlowState(object state)
		{
			return (FlowLayoutState)state;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0001F154 File Offset: 0x0001D354
		private void InvalidateLayout()
		{
			base.InvalidateMeasure();
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0001F15C File Offset: 0x0001D35C
		private FlowLayoutAlgorithm GetFlowAlgorithm(VirtualizingLayoutContext context)
		{
			return this.GetAsFlowState(context.LayoutState).FlowAlgorithm;
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0001F16F File Offset: 0x0001D36F
		private bool DoesRealizationWindowOverlapExtent(Rect realizationWindow, Rect extent)
		{
			return this.OM.MajorEnd(realizationWindow) >= this.OM.MajorStart(extent) && this.OM.MajorStart(realizationWindow) <= this.OM.MajorEnd(extent);
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x0001F1AA File Offset: 0x0001D3AA
		private double LineSpacing
		{
			get
			{
				if (this.OM.ScrollOrientation != ScrollOrientation.Vertical)
				{
					return this.m_minColumnSpacing;
				}
				return this.m_minRowSpacing;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x0001F1C6 File Offset: 0x0001D3C6
		private double MinItemSpacing
		{
			get
			{
				if (this.OM.ScrollOrientation != ScrollOrientation.Vertical)
				{
					return this.m_minRowSpacing;
				}
				return this.m_minColumnSpacing;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x0001F1E2 File Offset: 0x0001D3E2
		private OrientationBasedMeasures OM { get; } = new OrientationBasedMeasures();

		// Token: 0x040003BB RID: 955
		public static readonly DependencyProperty LineAlignmentProperty = DependencyProperty.Register("LineAlignment", typeof(FlowLayoutLineAlignment), typeof(FlowLayout), new PropertyMetadata(FlowLayoutLineAlignment.Start, new PropertyChangedCallback(FlowLayout.OnPropertyChanged)));

		// Token: 0x040003BC RID: 956
		public static readonly DependencyProperty MinColumnSpacingProperty = DependencyProperty.Register("MinColumnSpacing", typeof(double), typeof(FlowLayout), new PropertyMetadata(0.0, new PropertyChangedCallback(FlowLayout.OnPropertyChanged)));

		// Token: 0x040003BD RID: 957
		public static readonly DependencyProperty MinRowSpacingProperty = DependencyProperty.Register("MinRowSpacing", typeof(double), typeof(FlowLayout), new PropertyMetadata(0.0, new PropertyChangedCallback(FlowLayout.OnPropertyChanged)));

		// Token: 0x040003BE RID: 958
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(FlowLayout), new PropertyMetadata(0, new PropertyChangedCallback(FlowLayout.OnPropertyChanged)));

		// Token: 0x040003BF RID: 959
		private double m_minRowSpacing;

		// Token: 0x040003C0 RID: 960
		private double m_minColumnSpacing;

		// Token: 0x040003C1 RID: 961
		private FlowLayoutLineAlignment m_lineAlignment;
	}
}
