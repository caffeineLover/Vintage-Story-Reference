using System;
using System.Collections.Specialized;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000B1 RID: 177
	internal class FlowLayoutAlgorithm
	{
		// Token: 0x060008DF RID: 2271 RVA: 0x0001F2F1 File Offset: 0x0001D4F1
		public FlowLayoutAlgorithm()
		{
			this.m_elementManager = new ElementManager();
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x0001F31D File Offset: 0x0001D51D
		public Rect LastExtent
		{
			get
			{
				return this.m_lastExtent;
			}
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0001F325 File Offset: 0x0001D525
		public void InitializeForContext(VirtualizingLayoutContext context, IFlowLayoutAlgorithmDelegates callbacks)
		{
			this.m_algorithmCallbacks = callbacks;
			this.m_context = context;
			this.m_elementManager.SetContext(context);
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0001F341 File Offset: 0x0001D541
		public void UninitializeForContext(VirtualizingLayoutContext context)
		{
			if (this.IsVirtualizingContext)
			{
				this.m_elementManager.ClearRealizedRange();
			}
			((ILayoutContextOverrides)context).LayoutStateCore = null;
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0001F360 File Offset: 0x0001D560
		public Size Measure(Size availableSize, VirtualizingLayoutContext context, bool isWrapping, double minItemSpacing, double lineSpacing, uint maxItemsPerLine, ScrollOrientation orientation, bool disableVirtualization, string layoutId)
		{
			this.OM.ScrollOrientation = orientation;
			this.m_scrollOrientationSameAsFlow = double.IsInfinity(this.OM.Minor(availableSize));
			int recommendedAnchorIndex = this.m_context.RecommendedAnchorIndex;
			if (this.m_elementManager.IsIndexValidInData(recommendedAnchorIndex) && !this.m_elementManager.IsDataIndexRealized(recommendedAnchorIndex))
			{
				this.MakeAnchor(this.m_context, recommendedAnchorIndex, availableSize);
			}
			this.m_elementManager.OnBeginMeasure(orientation);
			int anchorIndex = this.GetAnchorIndex(availableSize, isWrapping, minItemSpacing, layoutId);
			this.Generate(FlowLayoutAlgorithm.GenerateDirection.Forward, anchorIndex, availableSize, minItemSpacing, lineSpacing, maxItemsPerLine, disableVirtualization, layoutId);
			this.Generate(FlowLayoutAlgorithm.GenerateDirection.Backward, anchorIndex, availableSize, minItemSpacing, lineSpacing, maxItemsPerLine, disableVirtualization, layoutId);
			if (isWrapping && this.IsReflowRequired)
			{
				Rect layoutBoundsForRealizedIndex = this.m_elementManager.GetLayoutBoundsForRealizedIndex(0);
				this.OM.SetMinorStart(ref layoutBoundsForRealizedIndex, 0.0);
				this.m_elementManager.SetLayoutBoundsForRealizedIndex(0, layoutBoundsForRealizedIndex);
				this.Generate(FlowLayoutAlgorithm.GenerateDirection.Forward, 0, availableSize, minItemSpacing, lineSpacing, maxItemsPerLine, disableVirtualization, layoutId);
			}
			this.RaiseLineArranged();
			this.m_collectionChangePending = false;
			this.m_lastExtent = this.EstimateExtent(availableSize, layoutId);
			this.SetLayoutOrigin();
			return new Size(this.m_lastExtent.Width, this.m_lastExtent.Height);
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0001F494 File Offset: 0x0001D694
		public Size Arrange(Size finalSize, VirtualizingLayoutContext context, bool isWrapping, FlowLayoutAlgorithm.LineAlignment lineAlignment, string layoutId)
		{
			this.ArrangeVirtualizingLayout(finalSize, lineAlignment, isWrapping, layoutId);
			return new Size(Math.Max(finalSize.Width, this.m_lastExtent.Width), Math.Max(finalSize.Height, this.m_lastExtent.Height));
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0001F4E0 File Offset: 0x0001D6E0
		public void OnItemsSourceChanged(object source, NotifyCollectionChangedEventArgs args, VirtualizingLayoutContext context)
		{
			this.m_elementManager.DataSourceChanged(source, args);
			this.m_collectionChangePending = true;
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0001F4F8 File Offset: 0x0001D6F8
		public Size MeasureElement(UIElement element, int index, Size availableSize, VirtualizingLayoutContext context)
		{
			Size size = this.m_algorithmCallbacks.Algorithm_GetMeasureSize(index, availableSize, context);
			element.Measure(size);
			Size size2 = this.m_algorithmCallbacks.Algorithm_GetProvisionalArrangeSize(index, size, element.DesiredSize, context);
			this.m_algorithmCallbacks.Algorithm_OnElementMeasured(element, index, availableSize, size, element.DesiredSize, size2, context);
			return size2;
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0001F54B File Offset: 0x0001D74B
		public UIElement GetElementIfRealized(int dataIndex)
		{
			if (this.m_elementManager.IsDataIndexRealized(dataIndex))
			{
				return this.m_elementManager.GetRealizedElement(dataIndex);
			}
			return null;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0001F569 File Offset: 0x0001D769
		public bool TryAddElement0(UIElement element)
		{
			if (this.m_elementManager.GetRealizedElementCount() == 0)
			{
				this.m_elementManager.Add(element, 0);
				return true;
			}
			return false;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0001F588 File Offset: 0x0001D788
		private int GetAnchorIndex(Size availableSize, bool isWrapping, double minItemSpacing, string layoutId)
		{
			Point point = default(Point);
			int num;
			if (!this.IsVirtualizingContext)
			{
				num = ((((IVirtualizingLayoutContextOverrides)this.m_context).ItemCountCore() > 0) ? 0 : -1);
			}
			else
			{
				bool flag = this.m_elementManager.IsWindowConnected(this.RealizationRect, this.OM.ScrollOrientation, this.m_scrollOrientationSameAsFlow);
				bool flag2 = isWrapping && (this.OM.Minor(this.m_lastAvailableSize) != this.OM.Minor(availableSize) || this.m_lastItemSpacing != minItemSpacing || this.m_collectionChangePending);
				int recommendedAnchorIndex = this.m_context.RecommendedAnchorIndex;
				if (recommendedAnchorIndex >= 0 && this.m_elementManager.IsDataIndexRealized(recommendedAnchorIndex))
				{
					num = this.m_algorithmCallbacks.Algorithm_GetAnchorForTargetElement(recommendedAnchorIndex, availableSize, this.m_context).Index;
					if (this.m_elementManager.IsDataIndexRealized(num))
					{
						Rect layoutBoundsForDataIndex = this.m_elementManager.GetLayoutBoundsForDataIndex(num);
						if (flag2)
						{
							point = this.OM.MinorMajorPoint(0.0, this.OM.MajorStart(layoutBoundsForDataIndex));
						}
						else
						{
							point..ctor(layoutBoundsForDataIndex.X, layoutBoundsForDataIndex.Y);
						}
					}
					else
					{
						for (int i = this.m_elementManager.GetDataIndexFromRealizedRangeIndex(0) - 1; i >= num; i--)
						{
							this.m_elementManager.EnsureElementRealized(false, i, layoutId);
						}
						Rect layoutBoundsForDataIndex2 = this.m_elementManager.GetLayoutBoundsForDataIndex(recommendedAnchorIndex);
						point = this.OM.MinorMajorPoint(0.0, this.OM.MajorStart(layoutBoundsForDataIndex2));
					}
				}
				else if (flag2 || !flag)
				{
					FlowLayoutAnchorInfo flowLayoutAnchorInfo = this.m_algorithmCallbacks.Algorithm_GetAnchorForRealizationRect(availableSize, this.m_context);
					num = flowLayoutAnchorInfo.Index;
					point = this.OM.MinorMajorPoint(0.0, flowLayoutAnchorInfo.Offset);
				}
				else
				{
					num = this.m_elementManager.GetDataIndexFromRealizedRangeIndex(0);
					Rect layoutBoundsForRealizedIndex = this.m_elementManager.GetLayoutBoundsForRealizedIndex(0);
					point..ctor(layoutBoundsForRealizedIndex.X, layoutBoundsForRealizedIndex.Y);
				}
			}
			this.m_firstRealizedDataIndexInsideRealizationWindow = (this.m_lastRealizedDataIndexInsideRealizationWindow = num);
			if (this.m_elementManager.IsIndexValidInData(num))
			{
				if (!this.m_elementManager.IsDataIndexRealized(num))
				{
					this.m_elementManager.ClearRealizedRange();
					UIElement orCreateElementAt = this.m_context.GetOrCreateElementAt(num, ElementRealizationOptions.ForceCreate | ElementRealizationOptions.SuppressAutoRecycle);
					this.m_elementManager.Add(orCreateElementAt, num);
				}
				UIElement realizedElement = this.m_elementManager.GetRealizedElement(num);
				Size size = this.MeasureElement(realizedElement, num, availableSize, this.m_context);
				Rect bounds;
				bounds..ctor(point.X, point.Y, size.Width, size.Height);
				this.m_elementManager.SetLayoutBoundsForDataIndex(num, bounds);
			}
			else
			{
				this.m_elementManager.ClearRealizedRange();
			}
			this.m_lastAvailableSize = availableSize;
			this.m_lastItemSpacing = minItemSpacing;
			return num;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0001F854 File Offset: 0x0001DA54
		private void Generate(FlowLayoutAlgorithm.GenerateDirection direction, int anchorIndex, Size availableSize, double minItemSpacing, double lineSpacing, uint maxItemsPerLine, bool disableVirtualization, string layoutId)
		{
			if (anchorIndex != -1)
			{
				int num = (direction == FlowLayoutAlgorithm.GenerateDirection.Forward) ? 1 : -1;
				int num2 = anchorIndex;
				int num3 = anchorIndex + num;
				Rect layoutBoundsForDataIndex = this.m_elementManager.GetLayoutBoundsForDataIndex(anchorIndex);
				double num4 = this.OM.MajorStart(layoutBoundsForDataIndex);
				double num5 = this.OM.MajorSize(layoutBoundsForDataIndex);
				uint num6 = 1U;
				bool flag = false;
				while (this.m_elementManager.IsIndexValidInData(num3) && (disableVirtualization || this.ShouldContinueFillingUpSpace(num2, direction)))
				{
					this.m_elementManager.EnsureElementRealized(direction == FlowLayoutAlgorithm.GenerateDirection.Forward, num3, layoutId);
					UIElement realizedElement = this.m_elementManager.GetRealizedElement(num3);
					Size size = this.MeasureElement(realizedElement, num3, availableSize, this.m_context);
					this.m_elementManager.GetRealizedElement(num2);
					Rect rect;
					rect..ctor(0.0, 0.0, size.Width, size.Height);
					Rect layoutBoundsForDataIndex2 = this.m_elementManager.GetLayoutBoundsForDataIndex(num2);
					if (direction == FlowLayoutAlgorithm.GenerateDirection.Forward)
					{
						double remainingSpace = this.OM.Minor(availableSize) - (this.OM.MinorStart(layoutBoundsForDataIndex2) + this.OM.MinorSize(layoutBoundsForDataIndex2) + minItemSpacing + this.OM.Minor(size));
						if (num6 >= maxItemsPerLine || this.m_algorithmCallbacks.Algorithm_ShouldBreakLine(num3, remainingSpace))
						{
							this.OM.SetMinorStart(ref rect, 0.0);
							this.OM.SetMajorStart(ref rect, this.OM.MajorStart(layoutBoundsForDataIndex2) + num5 + lineSpacing);
							if (flag)
							{
								for (uint num7 = 0U; num7 < num6; num7 += 1U)
								{
									long num8 = (long)(num3 - 1) - (long)((ulong)num7);
									Rect layoutBoundsForDataIndex3 = this.m_elementManager.GetLayoutBoundsForDataIndex((int)num8);
									this.OM.SetMajorSize(ref layoutBoundsForDataIndex3, num5);
									this.m_elementManager.SetLayoutBoundsForDataIndex((int)num8, layoutBoundsForDataIndex3);
								}
							}
							num5 = this.OM.MajorSize(rect);
							num4 = this.OM.MajorStart(rect);
							flag = false;
							num6 = 1U;
						}
						else
						{
							this.OM.SetMinorStart(ref rect, this.OM.MinorStart(layoutBoundsForDataIndex2) + this.OM.MinorSize(layoutBoundsForDataIndex2) + minItemSpacing);
							this.OM.SetMajorStart(ref rect, num4);
							num5 = Math.Max(num5, this.OM.MajorSize(rect));
							flag = (this.OM.MajorSize(layoutBoundsForDataIndex2) != this.OM.MajorSize(rect));
							num6 += 1U;
						}
					}
					else
					{
						double remainingSpace2 = this.OM.MinorStart(layoutBoundsForDataIndex2) - (this.OM.Minor(size) + minItemSpacing);
						if (num6 >= maxItemsPerLine || this.m_algorithmCallbacks.Algorithm_ShouldBreakLine(num3, remainingSpace2))
						{
							double num9 = this.OM.Minor(availableSize);
							this.OM.SetMinorStart(ref rect, (!double.IsInfinity(num9)) ? (num9 - this.OM.Minor(size)) : 0.0);
							this.OM.SetMajorStart(ref rect, num4 - this.OM.Major(size) - lineSpacing);
							if (flag)
							{
								double num10 = this.OM.MajorStart(this.m_elementManager.GetLayoutBoundsForDataIndex((int)((long)num3 + (long)((ulong)num6) + 1L)));
								for (uint num11 = 0U; num11 < num6; num11 += 1U)
								{
									int num12 = num3 + 1 + (int)num11;
									if (num12 != anchorIndex)
									{
										Rect layoutBoundsForDataIndex4 = this.m_elementManager.GetLayoutBoundsForDataIndex(num12);
										this.OM.SetMajorStart(ref layoutBoundsForDataIndex4, num10 - num5 - lineSpacing);
										this.OM.SetMajorSize(ref layoutBoundsForDataIndex4, num5);
										this.m_elementManager.SetLayoutBoundsForDataIndex(num12, layoutBoundsForDataIndex4);
									}
								}
							}
							num5 = this.OM.MajorSize(rect);
							num4 = this.OM.MajorStart(rect);
							flag = false;
							num6 = 1U;
						}
						else
						{
							this.OM.SetMinorStart(ref rect, this.OM.MinorStart(layoutBoundsForDataIndex2) - this.OM.Minor(size) - minItemSpacing);
							this.OM.SetMajorStart(ref rect, num4);
							num5 = Math.Max(num5, this.OM.MajorSize(rect));
							flag = (this.OM.MajorSize(layoutBoundsForDataIndex2) != this.OM.MajorSize(rect));
							num6 += 1U;
						}
					}
					this.m_elementManager.SetLayoutBoundsForDataIndex(num3, rect);
					num2 = num3;
					num3 += num;
				}
				if (direction == FlowLayoutAlgorithm.GenerateDirection.Forward)
				{
					int itemCount = this.m_context.ItemCount;
					this.m_lastRealizedDataIndexInsideRealizationWindow = ((num2 == itemCount - 1) ? (itemCount - 1) : (num2 - 1));
					this.m_lastRealizedDataIndexInsideRealizationWindow = Math.Max(0, this.m_lastRealizedDataIndexInsideRealizationWindow);
				}
				else
				{
					int itemCount2 = this.m_context.ItemCount;
					this.m_firstRealizedDataIndexInsideRealizationWindow = ((num2 == 0) ? 0 : (num2 + 1));
					this.m_firstRealizedDataIndexInsideRealizationWindow = Math.Min(itemCount2 - 1, this.m_firstRealizedDataIndexInsideRealizationWindow);
				}
				this.m_elementManager.DiscardElementsOutsideWindow(direction == FlowLayoutAlgorithm.GenerateDirection.Forward, num3);
			}
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0001FD10 File Offset: 0x0001DF10
		private void MakeAnchor(VirtualizingLayoutContext context, int index, Size availableSize)
		{
			this.m_elementManager.ClearRealizedRange();
			for (int i = this.m_algorithmCallbacks.Algorithm_GetAnchorForTargetElement(index, availableSize, context).Index; i < index + 1; i++)
			{
				UIElement orCreateElementAt = context.GetOrCreateElementAt(i, ElementRealizationOptions.ForceCreate | ElementRealizationOptions.SuppressAutoRecycle);
				orCreateElementAt.Measure(this.m_algorithmCallbacks.Algorithm_GetMeasureSize(i, availableSize, context));
				this.m_elementManager.Add(orCreateElementAt, i);
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x0001FD74 File Offset: 0x0001DF74
		private bool IsReflowRequired
		{
			get
			{
				return this.m_elementManager.GetRealizedElementCount() > 0 && this.m_elementManager.GetDataIndexFromRealizedRangeIndex(0) == 0 && this.OM.MinorStart(this.m_elementManager.GetLayoutBoundsForRealizedIndex(0)) != 0.0;
			}
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0001FDC4 File Offset: 0x0001DFC4
		private bool ShouldContinueFillingUpSpace(int index, FlowLayoutAlgorithm.GenerateDirection direction)
		{
			bool result;
			if (!this.IsVirtualizingContext)
			{
				result = true;
			}
			else
			{
				Rect realizationRect = this.m_context.RealizationRect;
				Rect layoutBoundsForDataIndex = this.m_elementManager.GetLayoutBoundsForDataIndex(index);
				double num = this.OM.MajorStart(layoutBoundsForDataIndex);
				double num2 = this.OM.MajorEnd(layoutBoundsForDataIndex);
				double num3 = this.OM.MajorStart(realizationRect);
				double num4 = this.OM.MajorEnd(realizationRect);
				double num5 = this.OM.MinorStart(layoutBoundsForDataIndex);
				double num6 = this.OM.MinorEnd(layoutBoundsForDataIndex);
				double num7 = this.OM.MinorStart(realizationRect);
				double num8 = this.OM.MinorEnd(realizationRect);
				result = ((direction == FlowLayoutAlgorithm.GenerateDirection.Forward && num < num4 && num5 < num8) || (direction == FlowLayoutAlgorithm.GenerateDirection.Backward && num2 > num3 && num6 > num7));
			}
			return result;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0001FE90 File Offset: 0x0001E090
		private Rect EstimateExtent(Size availableSize, string layoutId)
		{
			UIElement firstRealized = null;
			Rect firstRealizedLayoutBounds = default(Rect);
			UIElement lastRealized = null;
			Rect lastRealizedLayoutBounds = default(Rect);
			int firstRealizedItemIndex = -1;
			int lastRealizedItemIndex = -1;
			if (this.m_elementManager.GetRealizedElementCount() > 0)
			{
				firstRealized = this.m_elementManager.GetAt(0);
				firstRealizedLayoutBounds = this.m_elementManager.GetLayoutBoundsForRealizedIndex(0);
				firstRealizedItemIndex = this.m_elementManager.GetDataIndexFromRealizedRangeIndex(0);
				int num = this.m_elementManager.GetRealizedElementCount() - 1;
				lastRealized = this.m_elementManager.GetAt(num);
				lastRealizedItemIndex = this.m_elementManager.GetDataIndexFromRealizedRangeIndex(num);
				lastRealizedLayoutBounds = this.m_elementManager.GetLayoutBoundsForRealizedIndex(num);
			}
			return this.m_algorithmCallbacks.Algorithm_GetExtent(availableSize, this.m_context, firstRealized, firstRealizedItemIndex, firstRealizedLayoutBounds, lastRealized, lastRealizedItemIndex, lastRealizedLayoutBounds);
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0001FF44 File Offset: 0x0001E144
		private void RaiseLineArranged()
		{
			Rect realizationRect = this.RealizationRect;
			if ((realizationRect.Width != 0.0 || realizationRect.Height != 0.0) && this.m_elementManager.GetRealizedElementCount() > 0)
			{
				int num = 0;
				Rect layoutBoundsForDataIndex = this.m_elementManager.GetLayoutBoundsForDataIndex(this.m_firstRealizedDataIndexInsideRealizationWindow);
				double num2 = this.OM.MajorStart(layoutBoundsForDataIndex);
				double num3 = this.OM.MajorSize(layoutBoundsForDataIndex);
				for (int i = this.m_firstRealizedDataIndexInsideRealizationWindow; i <= this.m_lastRealizedDataIndexInsideRealizationWindow; i++)
				{
					Rect layoutBoundsForDataIndex2 = this.m_elementManager.GetLayoutBoundsForDataIndex(i);
					if (this.OM.MajorStart(layoutBoundsForDataIndex2) != num2)
					{
						this.m_algorithmCallbacks.Algorithm_OnLineArranged(i - num, num, num3, this.m_context);
						num = 0;
						num2 = this.OM.MajorStart(layoutBoundsForDataIndex2);
						num3 = 0.0;
					}
					num3 = Math.Max(num3, this.OM.MajorSize(layoutBoundsForDataIndex2));
					num++;
				}
				this.m_algorithmCallbacks.Algorithm_OnLineArranged(this.m_lastRealizedDataIndexInsideRealizationWindow - num + 1, num, num3, this.m_context);
			}
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00020064 File Offset: 0x0001E264
		private void ArrangeVirtualizingLayout(Size finalSize, FlowLayoutAlgorithm.LineAlignment lineAlignment, bool isWrapping, string layoutId)
		{
			int realizedElementCount = this.m_elementManager.GetRealizedElementCount();
			if (realizedElementCount > 0)
			{
				int num = 1;
				Rect rect = this.m_elementManager.GetLayoutBoundsForRealizedIndex(0);
				double num2 = this.OM.MajorStart(rect);
				double spaceAtLineStart = this.OM.MinorStart(rect);
				double num3 = this.OM.MajorSize(rect);
				for (int i = 1; i < realizedElementCount; i++)
				{
					Rect layoutBoundsForRealizedIndex = this.m_elementManager.GetLayoutBoundsForRealizedIndex(i);
					if (this.OM.MajorStart(layoutBoundsForRealizedIndex) != num2)
					{
						double spaceAtLineEnd = this.OM.Minor(finalSize) - this.OM.MinorStart(rect) - this.OM.MinorSize(rect);
						this.PerformLineAlignment(i - num, num, spaceAtLineStart, spaceAtLineEnd, num3, lineAlignment, isWrapping, finalSize, layoutId);
						spaceAtLineStart = this.OM.MinorStart(layoutBoundsForRealizedIndex);
						num = 0;
						num2 = this.OM.MajorStart(layoutBoundsForRealizedIndex);
						num3 = 0.0;
					}
					num++;
					num3 = Math.Max(num3, this.OM.MajorSize(layoutBoundsForRealizedIndex));
					rect = layoutBoundsForRealizedIndex;
				}
				if (num > 0)
				{
					double spaceAtLineEnd2 = this.OM.Minor(finalSize) - this.OM.MinorStart(rect) - this.OM.MinorSize(rect);
					this.PerformLineAlignment(realizedElementCount - num, num, spaceAtLineStart, spaceAtLineEnd2, num3, lineAlignment, isWrapping, finalSize, layoutId);
				}
			}
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x000201C4 File Offset: 0x0001E3C4
		private void PerformLineAlignment(int lineStartIndex, int countInLine, double spaceAtLineStart, double spaceAtLineEnd, double lineSize, FlowLayoutAlgorithm.LineAlignment lineAlignment, bool isWrapping, Size finalSize, string layoutId)
		{
			for (int i = lineStartIndex; i < lineStartIndex + countInLine; i++)
			{
				Rect layoutBoundsForRealizedIndex = this.m_elementManager.GetLayoutBoundsForRealizedIndex(i);
				this.OM.SetMajorSize(ref layoutBoundsForRealizedIndex, lineSize);
				if (!this.m_scrollOrientationSameAsFlow && (spaceAtLineStart != 0.0 || spaceAtLineEnd != 0.0))
				{
					double num = spaceAtLineStart + spaceAtLineEnd;
					switch (lineAlignment)
					{
					case FlowLayoutAlgorithm.LineAlignment.Start:
						this.OM.SetMinorStart(ref layoutBoundsForRealizedIndex, this.OM.MinorStart(layoutBoundsForRealizedIndex) - spaceAtLineStart);
						break;
					case FlowLayoutAlgorithm.LineAlignment.Center:
						this.OM.SetMinorStart(ref layoutBoundsForRealizedIndex, this.OM.MinorStart(layoutBoundsForRealizedIndex) - spaceAtLineStart);
						this.OM.SetMinorStart(ref layoutBoundsForRealizedIndex, this.OM.MinorStart(layoutBoundsForRealizedIndex) + num / 2.0);
						break;
					case FlowLayoutAlgorithm.LineAlignment.End:
						this.OM.SetMinorStart(ref layoutBoundsForRealizedIndex, this.OM.MinorStart(layoutBoundsForRealizedIndex) + spaceAtLineEnd);
						break;
					case FlowLayoutAlgorithm.LineAlignment.SpaceAround:
					{
						double num2 = (countInLine >= 1) ? (num / (double)(countInLine * 2)) : 0.0;
						this.OM.SetMinorStart(ref layoutBoundsForRealizedIndex, this.OM.MinorStart(layoutBoundsForRealizedIndex) - spaceAtLineStart);
						this.OM.SetMinorStart(ref layoutBoundsForRealizedIndex, this.OM.MinorStart(layoutBoundsForRealizedIndex) + num2 * (double)((i - lineStartIndex + 1) * 2 - 1));
						break;
					}
					case FlowLayoutAlgorithm.LineAlignment.SpaceBetween:
					{
						double num3 = (countInLine > 1) ? (num / (double)(countInLine - 1)) : 0.0;
						this.OM.SetMinorStart(ref layoutBoundsForRealizedIndex, this.OM.MinorStart(layoutBoundsForRealizedIndex) - spaceAtLineStart);
						this.OM.SetMinorStart(ref layoutBoundsForRealizedIndex, this.OM.MinorStart(layoutBoundsForRealizedIndex) + num3 * (double)(i - lineStartIndex));
						break;
					}
					case FlowLayoutAlgorithm.LineAlignment.SpaceEvenly:
					{
						double num4 = (countInLine >= 1) ? (num / (double)(countInLine + 1)) : 0.0;
						this.OM.SetMinorStart(ref layoutBoundsForRealizedIndex, this.OM.MinorStart(layoutBoundsForRealizedIndex) - spaceAtLineStart);
						this.OM.SetMinorStart(ref layoutBoundsForRealizedIndex, this.OM.MinorStart(layoutBoundsForRealizedIndex) + num4 * (double)(i - lineStartIndex + 1));
						break;
					}
					}
				}
				layoutBoundsForRealizedIndex.X -= this.m_lastExtent.X;
				layoutBoundsForRealizedIndex.Y -= this.m_lastExtent.Y;
				if (!isWrapping)
				{
					this.OM.SetMinorSize(ref layoutBoundsForRealizedIndex, Math.Max(this.OM.MinorSize(layoutBoundsForRealizedIndex), this.OM.Minor(finalSize)));
				}
				this.m_elementManager.GetAt(i).Arrange(layoutBoundsForRealizedIndex);
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x00020450 File Offset: 0x0001E650
		private Rect RealizationRect
		{
			get
			{
				if (!this.IsVirtualizingContext)
				{
					return new Rect(0.0, 0.0, double.PositiveInfinity, double.PositiveInfinity);
				}
				return this.m_context.RealizationRect;
			}
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0002048F File Offset: 0x0001E68F
		private void SetLayoutOrigin()
		{
			if (this.IsVirtualizingContext)
			{
				this.m_context.LayoutOrigin = new Point(this.m_lastExtent.X, this.m_lastExtent.Y);
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x000204C0 File Offset: 0x0001E6C0
		private bool IsVirtualizingContext
		{
			get
			{
				if (this.m_context != null)
				{
					Rect realizationRect = this.m_context.RealizationRect;
					return !double.IsInfinity(realizationRect.Height) && !double.IsInfinity(realizationRect.Width);
				}
				return false;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x00020503 File Offset: 0x0001E703
		private OrientationBasedMeasures OM { get; } = new OrientationBasedMeasures();

		// Token: 0x040003CA RID: 970
		private readonly ElementManager m_elementManager;

		// Token: 0x040003CB RID: 971
		private Size m_lastAvailableSize;

		// Token: 0x040003CC RID: 972
		private double m_lastItemSpacing;

		// Token: 0x040003CD RID: 973
		private bool m_collectionChangePending;

		// Token: 0x040003CE RID: 974
		private VirtualizingLayoutContext m_context;

		// Token: 0x040003CF RID: 975
		private IFlowLayoutAlgorithmDelegates m_algorithmCallbacks;

		// Token: 0x040003D0 RID: 976
		private Rect m_lastExtent;

		// Token: 0x040003D1 RID: 977
		private int m_firstRealizedDataIndexInsideRealizationWindow = -1;

		// Token: 0x040003D2 RID: 978
		private int m_lastRealizedDataIndexInsideRealizationWindow = -1;

		// Token: 0x040003D3 RID: 979
		private bool m_scrollOrientationSameAsFlow;

		// Token: 0x02000120 RID: 288
		public enum LineAlignment
		{
			// Token: 0x040005B7 RID: 1463
			Start,
			// Token: 0x040005B8 RID: 1464
			Center,
			// Token: 0x040005B9 RID: 1465
			End,
			// Token: 0x040005BA RID: 1466
			SpaceAround,
			// Token: 0x040005BB RID: 1467
			SpaceBetween,
			// Token: 0x040005BC RID: 1468
			SpaceEvenly
		}

		// Token: 0x02000121 RID: 289
		private enum GenerateDirection
		{
			// Token: 0x040005BE RID: 1470
			Forward,
			// Token: 0x040005BF RID: 1471
			Backward
		}
	}
}
