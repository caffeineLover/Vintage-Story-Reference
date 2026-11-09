using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000AE RID: 174
	internal class ElementManager
	{
		// Token: 0x0600089A RID: 2202 RVA: 0x0001E1EA File Offset: 0x0001C3EA
		public void SetContext(VirtualizingLayoutContext virtualContext)
		{
			this.m_context = virtualContext;
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0001E1F4 File Offset: 0x0001C3F4
		public void OnBeginMeasure(ScrollOrientation orientation)
		{
			if (this.m_context != null)
			{
				if (this.IsVirtualizingContext)
				{
					this.DiscardElementsOutsideWindow(this.m_context.RealizationRect, orientation);
					return;
				}
				int itemCount = this.m_context.ItemCount;
				if (this.m_realizedElementLayoutBounds.Count != itemCount)
				{
					this.m_realizedElementLayoutBounds.Resize(itemCount, default(Rect));
				}
			}
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0001E253 File Offset: 0x0001C453
		public int GetRealizedElementCount()
		{
			if (!this.IsVirtualizingContext)
			{
				return this.m_context.ItemCount;
			}
			return this.m_realizedElements.Count;
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0001E274 File Offset: 0x0001C474
		public UIElement GetAt(int realizedIndex)
		{
			UIElement uielement;
			if (this.IsVirtualizingContext)
			{
				if (this.m_realizedElements[realizedIndex] == null)
				{
					int dataIndexFromRealizedRangeIndex = this.GetDataIndexFromRealizedRangeIndex(realizedIndex);
					uielement = this.m_context.GetOrCreateElementAt(dataIndexFromRealizedRangeIndex, ElementRealizationOptions.ForceCreate | ElementRealizationOptions.SuppressAutoRecycle);
					this.m_realizedElements[realizedIndex] = uielement;
				}
				else
				{
					uielement = this.m_realizedElements[realizedIndex];
				}
			}
			else
			{
				uielement = this.m_context.GetOrCreateElementAt(realizedIndex, ElementRealizationOptions.ForceCreate | ElementRealizationOptions.SuppressAutoRecycle);
			}
			return uielement;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0001E2DC File Offset: 0x0001C4DC
		public void Add(UIElement element, int dataIndex)
		{
			if (this.m_realizedElements.Count == 0)
			{
				this.m_firstRealizedDataIndex = dataIndex;
			}
			this.m_realizedElements.Add(element);
			this.m_realizedElementLayoutBounds.Add(default(Rect));
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0001E31D File Offset: 0x0001C51D
		public void Insert(int realizedIndex, int dataIndex, UIElement element)
		{
			if (realizedIndex == 0)
			{
				this.m_firstRealizedDataIndex = dataIndex;
			}
			this.m_realizedElements.Insert(realizedIndex, element);
			this.m_realizedElementLayoutBounds.Insert(realizedIndex, Rect.Empty);
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0001E348 File Offset: 0x0001C548
		public void ClearRealizedRange(int realizedIndex, int count)
		{
			for (int i = 0; i < count; i++)
			{
				int num = (realizedIndex == 0) ? (realizedIndex + i) : (realizedIndex + count - 1 - i);
				UIElement uielement = this.m_realizedElements[num];
				if (uielement != null)
				{
					this.m_context.RecycleElement(uielement);
				}
			}
			this.m_realizedElements.RemoveRange(realizedIndex, count);
			this.m_realizedElementLayoutBounds.RemoveRange(realizedIndex, count);
			if (realizedIndex == 0)
			{
				this.m_firstRealizedDataIndex = ((this.m_realizedElements.Count == 0) ? -1 : (this.m_firstRealizedDataIndex + count));
			}
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0001E3C8 File Offset: 0x0001C5C8
		public void DiscardElementsOutsideWindow(bool forward, int startIndex)
		{
			if (this.IsDataIndexRealized(startIndex))
			{
				int realizedRangeIndexFromDataIndex = this.GetRealizedRangeIndexFromDataIndex(startIndex);
				if (forward)
				{
					this.ClearRealizedRange(realizedRangeIndexFromDataIndex, this.GetRealizedElementCount() - realizedRangeIndexFromDataIndex);
					return;
				}
				this.ClearRealizedRange(0, realizedRangeIndexFromDataIndex + 1);
			}
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0001E403 File Offset: 0x0001C603
		public void ClearRealizedRange()
		{
			this.ClearRealizedRange(0, this.GetRealizedElementCount());
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0001E414 File Offset: 0x0001C614
		public Rect GetLayoutBoundsForDataIndex(int dataIndex)
		{
			int realizedRangeIndexFromDataIndex = this.GetRealizedRangeIndexFromDataIndex(dataIndex);
			return this.m_realizedElementLayoutBounds[realizedRangeIndexFromDataIndex];
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0001E438 File Offset: 0x0001C638
		public void SetLayoutBoundsForDataIndex(int dataIndex, Rect bounds)
		{
			int realizedRangeIndexFromDataIndex = this.GetRealizedRangeIndexFromDataIndex(dataIndex);
			this.m_realizedElementLayoutBounds[realizedRangeIndexFromDataIndex] = bounds;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0001E45A File Offset: 0x0001C65A
		public Rect GetLayoutBoundsForRealizedIndex(int realizedIndex)
		{
			return this.m_realizedElementLayoutBounds[realizedIndex];
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0001E468 File Offset: 0x0001C668
		public void SetLayoutBoundsForRealizedIndex(int realizedIndex, Rect bounds)
		{
			this.m_realizedElementLayoutBounds[realizedIndex] = bounds;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0001E478 File Offset: 0x0001C678
		public bool IsDataIndexRealized(int index)
		{
			if (this.IsVirtualizingContext)
			{
				int realizedElementCount = this.GetRealizedElementCount();
				return realizedElementCount > 0 && this.GetDataIndexFromRealizedRangeIndex(0) <= index && this.GetDataIndexFromRealizedRangeIndex(realizedElementCount - 1) >= index;
			}
			return index >= 0 && index < this.m_context.ItemCount;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0001E4C8 File Offset: 0x0001C6C8
		public bool IsIndexValidInData(int currentIndex)
		{
			return currentIndex >= 0 && currentIndex < this.m_context.ItemCount;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0001E4DE File Offset: 0x0001C6DE
		public UIElement GetRealizedElement(int dataIndex)
		{
			if (!this.IsVirtualizingContext)
			{
				return this.m_context.GetOrCreateElementAt(dataIndex, ElementRealizationOptions.ForceCreate | ElementRealizationOptions.SuppressAutoRecycle);
			}
			return this.GetAt(this.GetRealizedRangeIndexFromDataIndex(dataIndex));
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0001E504 File Offset: 0x0001C704
		public void EnsureElementRealized(bool forward, int dataIndex, string layoutId)
		{
			if (!this.IsDataIndexRealized(dataIndex))
			{
				UIElement orCreateElementAt = this.m_context.GetOrCreateElementAt(dataIndex, ElementRealizationOptions.ForceCreate | ElementRealizationOptions.SuppressAutoRecycle);
				if (forward)
				{
					this.Add(orCreateElementAt, dataIndex);
					return;
				}
				this.Insert(0, dataIndex, orCreateElementAt);
			}
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0001E540 File Offset: 0x0001C740
		public bool IsWindowConnected(Rect window, ScrollOrientation orientation, bool scrollOrientationSameAsFlow)
		{
			bool result = false;
			if (this.m_realizedElementLayoutBounds.Count > 0)
			{
				Rect layoutBoundsForRealizedIndex = this.GetLayoutBoundsForRealizedIndex(0);
				Rect layoutBoundsForRealizedIndex2 = this.GetLayoutBoundsForRealizedIndex(this.GetRealizedElementCount() - 1);
				ScrollOrientation scrollOrientation = scrollOrientationSameAsFlow ? ((orientation == ScrollOrientation.Vertical) ? ScrollOrientation.Horizontal : ScrollOrientation.Vertical) : orientation;
				double num = (scrollOrientation == ScrollOrientation.Vertical) ? window.Y : window.X;
				double num2 = (scrollOrientation == ScrollOrientation.Vertical) ? (window.Y + window.Height) : (window.X + window.Width);
				double num3 = (scrollOrientation == ScrollOrientation.Vertical) ? layoutBoundsForRealizedIndex.Y : layoutBoundsForRealizedIndex.X;
				double num4 = (scrollOrientation == ScrollOrientation.Vertical) ? (layoutBoundsForRealizedIndex2.Y + layoutBoundsForRealizedIndex2.Height) : (layoutBoundsForRealizedIndex2.X + layoutBoundsForRealizedIndex2.Width);
				result = (num3 <= num2 && num4 >= num);
			}
			return result;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0001E60C File Offset: 0x0001C80C
		public void DataSourceChanged(object source, NotifyCollectionChangedEventArgs args)
		{
			if (this.m_realizedElements.Count > 0)
			{
				switch (args.Action)
				{
				case 0:
					this.OnItemsAdded(args.NewStartingIndex, args.NewItems.Count);
					return;
				case 1:
					this.OnItemsRemoved(args.OldStartingIndex, args.OldItems.Count);
					return;
				case 2:
				{
					int count = args.OldItems.Count;
					int count2 = args.NewItems.Count;
					int oldStartingIndex = args.OldStartingIndex;
					int newStartingIndex = args.NewStartingIndex;
					if (count == count2 && oldStartingIndex == newStartingIndex && this.IsDataIndexRealized(oldStartingIndex) && this.IsDataIndexRealized(oldStartingIndex + count - 1))
					{
						int realizedRangeIndexFromDataIndex = this.GetRealizedRangeIndexFromDataIndex(oldStartingIndex);
						for (int i = realizedRangeIndexFromDataIndex; i < realizedRangeIndexFromDataIndex + count; i++)
						{
							UIElement uielement = this.m_realizedElements[i];
							if (uielement != null)
							{
								this.m_context.RecycleElement(uielement);
								this.m_realizedElements[i] = null;
							}
						}
						return;
					}
					this.OnItemsRemoved(oldStartingIndex, count);
					this.OnItemsAdded(newStartingIndex, count2);
					return;
				}
				case 3:
				{
					int count3 = (args.OldItems != null) ? args.OldItems.Count : 1;
					this.OnItemsRemoved(args.OldStartingIndex, count3);
					this.OnItemsAdded(args.NewStartingIndex, count3);
					break;
				}
				case 4:
					this.ClearRealizedRange();
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0001E760 File Offset: 0x0001C960
		public int GetElementDataIndex(UIElement suggestedAnchor)
		{
			int num = this.m_realizedElements.IndexOf(suggestedAnchor);
			if (num < 0)
			{
				return -1;
			}
			return this.GetDataIndexFromRealizedRangeIndex(num);
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0001E787 File Offset: 0x0001C987
		public int GetDataIndexFromRealizedRangeIndex(int rangeIndex)
		{
			if (!this.IsVirtualizingContext)
			{
				return rangeIndex;
			}
			return rangeIndex + this.m_firstRealizedDataIndex;
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0001E79B File Offset: 0x0001C99B
		private int GetRealizedRangeIndexFromDataIndex(int dataIndex)
		{
			if (!this.IsVirtualizingContext)
			{
				return dataIndex;
			}
			return dataIndex - this.m_firstRealizedDataIndex;
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0001E7B0 File Offset: 0x0001C9B0
		private void DiscardElementsOutsideWindow(Rect window, ScrollOrientation orientation)
		{
			int realizedElementCount = this.GetRealizedElementCount();
			int num = -1;
			int num2 = realizedElementCount;
			int num3 = 0;
			while (num3 < realizedElementCount && !ElementManager.Intersects(window, this.m_realizedElementLayoutBounds[num3], orientation))
			{
				num++;
				num3++;
			}
			int num4 = realizedElementCount - 1;
			while (num4 >= 0 && !ElementManager.Intersects(window, this.m_realizedElementLayoutBounds[num4], orientation))
			{
				num2--;
				num4--;
			}
			if (num2 < realizedElementCount - 1)
			{
				this.ClearRealizedRange(num2 + 1, realizedElementCount - num2 - 1);
			}
			if (num > 0)
			{
				this.ClearRealizedRange(0, Math.Min(num, this.GetRealizedElementCount()));
			}
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0001E844 File Offset: 0x0001CA44
		private static bool Intersects(Rect lhs, Rect rhs, ScrollOrientation orientation)
		{
			double num = (orientation == ScrollOrientation.Vertical) ? lhs.Y : lhs.X;
			double num2 = (orientation == ScrollOrientation.Vertical) ? (lhs.Y + lhs.Height) : (lhs.X + lhs.Width);
			double num3 = (orientation == ScrollOrientation.Vertical) ? rhs.Y : rhs.X;
			double num4 = (orientation == ScrollOrientation.Vertical) ? (rhs.Y + rhs.Height) : (rhs.X + rhs.Width);
			return num2 >= num3 && num <= num4;
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0001E8CC File Offset: 0x0001CACC
		private void OnItemsAdded(int index, int count)
		{
			int num = this.m_firstRealizedDataIndex + this.GetRealizedElementCount() - 1;
			if (index >= this.m_firstRealizedDataIndex && index <= num)
			{
				int num2 = index - this.m_firstRealizedDataIndex;
				for (int i = 0; i < count; i++)
				{
					int realizedIndex = num2 + i;
					int dataIndex = index + i;
					this.Insert(realizedIndex, dataIndex, null);
				}
				return;
			}
			if (index <= this.m_firstRealizedDataIndex)
			{
				this.m_firstRealizedDataIndex += count;
			}
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0001E93C File Offset: 0x0001CB3C
		private void OnItemsRemoved(int index, int count)
		{
			int num = this.m_firstRealizedDataIndex + this.m_realizedElements.Count - 1;
			int num2 = Math.Max(this.m_firstRealizedDataIndex, index);
			int num3 = Math.Min(num, index + count - 1);
			bool flag = index <= this.m_firstRealizedDataIndex;
			if (num3 >= num2)
			{
				this.ClearRealizedRange(this.GetRealizedRangeIndexFromDataIndex(num2), num3 - num2 + 1);
			}
			if (flag && this.m_firstRealizedDataIndex != -1)
			{
				this.m_firstRealizedDataIndex -= count;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x0001E9B0 File Offset: 0x0001CBB0
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

		// Token: 0x040003B7 RID: 951
		private readonly List<UIElement> m_realizedElements = new List<UIElement>();

		// Token: 0x040003B8 RID: 952
		private readonly List<Rect> m_realizedElementLayoutBounds = new List<Rect>();

		// Token: 0x040003B9 RID: 953
		private int m_firstRealizedDataIndex = -1;

		// Token: 0x040003BA RID: 954
		private VirtualizingLayoutContext m_context;
	}
}
