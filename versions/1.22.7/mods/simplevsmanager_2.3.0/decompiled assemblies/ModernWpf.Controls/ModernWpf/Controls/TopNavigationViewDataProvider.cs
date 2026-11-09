using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ModernWpf.Controls
{
	// Token: 0x02000071 RID: 113
	internal class TopNavigationViewDataProvider : SplitDataSourceBase<object, NavigationViewSplitVectorID, double>
	{
		// Token: 0x06000573 RID: 1395 RVA: 0x00013F30 File Offset: 0x00012130
		public TopNavigationViewDataProvider()
		{
			Func<object, int> func = (object value) => this.IndexOf(value);
			SplitVector<object, NavigationViewSplitVectorID> splitVector = new SplitVector<object, NavigationViewSplitVectorID>(NavigationViewSplitVectorID.PrimaryList, func);
			SplitVector<object, NavigationViewSplitVectorID> splitVector2 = new SplitVector<object, NavigationViewSplitVectorID>(NavigationViewSplitVectorID.OverflowList, func);
			List<SplitVector<object, NavigationViewSplitVectorID>> list = new List<SplitVector<object, NavigationViewSplitVectorID>>();
			list.Add(splitVector);
			list.Add(splitVector2);
			base.InitializeSplitVectors(list);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00013F79 File Offset: 0x00012179
		public IList GetPrimaryItems()
		{
			return base.GetVector(NavigationViewSplitVectorID.PrimaryList).GetVector();
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00013F87 File Offset: 0x00012187
		public IList GetOverflowItems()
		{
			return base.GetVector(NavigationViewSplitVectorID.OverflowList).GetVector();
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00013F98 File Offset: 0x00012198
		public void SetDataSource(object rawData)
		{
			if (this.ShouldChangeDataSource(rawData))
			{
				ItemsSourceView itemsSourceView = null;
				if (rawData != null)
				{
					itemsSourceView = new InspectingDataSource(rawData);
				}
				this.ChangeDataSource(itemsSourceView);
				this.m_rawDataSource = rawData;
				if (itemsSourceView != null)
				{
					this.MoveAllItemsToPrimaryList();
				}
			}
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00013FD1 File Offset: 0x000121D1
		public bool ShouldChangeDataSource(object rawData)
		{
			return rawData != this.m_rawDataSource;
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00013FDF File Offset: 0x000121DF
		public void OnRawDataChanged(Action<NotifyCollectionChangedEventArgs> dataChangeCallback)
		{
			this.m_dataChangeCallback = dataChangeCallback;
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00013FE8 File Offset: 0x000121E8
		internal override int IndexOf(object value)
		{
			ItemsSourceView dataSource = this.m_dataSource;
			if (dataSource != null)
			{
				return dataSource.IndexOf(value);
			}
			return -1;
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00014008 File Offset: 0x00012208
		internal override object GetAt(int index)
		{
			ItemsSourceView dataSource = this.m_dataSource;
			if (dataSource != null)
			{
				return dataSource.GetAt(index);
			}
			return null;
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00014028 File Offset: 0x00012228
		internal override int Size()
		{
			ItemsSourceView dataSource = this.m_dataSource;
			if (dataSource != null)
			{
				return dataSource.Count;
			}
			return 0;
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00014047 File Offset: 0x00012247
		internal override NavigationViewSplitVectorID DefaultVectorIDOnInsert()
		{
			return NavigationViewSplitVectorID.NotInitialized;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001404A File Offset: 0x0001224A
		internal override double DefaultAttachedData()
		{
			return double.MinValue;
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00014058 File Offset: 0x00012258
		public void MoveAllItemsToPrimaryList()
		{
			for (int i = 0; i < this.Size(); i++)
			{
				base.MoveItemToVector(i, NavigationViewSplitVectorID.PrimaryList);
			}
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00014080 File Offset: 0x00012280
		public List<int> ConvertPrimaryIndexToIndex(List<int> indexesInPrimary)
		{
			List<int> list = null;
			if (indexesInPrimary.Count > 0)
			{
				SplitVector<object, NavigationViewSplitVectorID> vector = base.GetVector(NavigationViewSplitVectorID.PrimaryList);
				if (vector != null)
				{
					list = Enumerable.ToList<int>(Enumerable.Select<int, int>(indexesInPrimary, (int index) => vector.IndexToIndexInOriginalVector(index)));
				}
			}
			return list ?? new List<int>();
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x000140D5 File Offset: 0x000122D5
		public int ConvertOriginalIndexToIndex(int originalIndex)
		{
			return base.GetVector(this.IsItemInPrimaryList(originalIndex) ? NavigationViewSplitVectorID.PrimaryList : NavigationViewSplitVectorID.OverflowList).IndexFromIndexInOriginalVector(originalIndex);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x000140F0 File Offset: 0x000122F0
		public void MoveItemsOutOfPrimaryList(List<int> indexes)
		{
			this.MoveItemsToList(indexes, NavigationViewSplitVectorID.OverflowList);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x000140FA File Offset: 0x000122FA
		public void MoveItemsToPrimaryList(List<int> indexes)
		{
			this.MoveItemsToList(indexes, NavigationViewSplitVectorID.PrimaryList);
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00014104 File Offset: 0x00012304
		public void MoveItemsToList(List<int> indexes, NavigationViewSplitVectorID vectorID)
		{
			foreach (int num in indexes)
			{
				base.MoveItemToVector(num, vectorID);
			}
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00014154 File Offset: 0x00012354
		public int GetPrimaryListSize()
		{
			return this.GetPrimaryItems().Count;
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00014164 File Offset: 0x00012364
		public int GetNavigationViewItemCountInPrimaryList()
		{
			int num = 0;
			for (int i = 0; i < this.Size(); i++)
			{
				if (this.IsItemInPrimaryList(i) && this.IsContainerNavigationViewItem(i))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0001419C File Offset: 0x0001239C
		public int GetNavigationViewItemCountInTopNav()
		{
			int num = 0;
			for (int i = 0; i < this.Size(); i++)
			{
				if (this.IsContainerNavigationViewItem(i))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x000141CC File Offset: 0x000123CC
		public void UpdateWidthForPrimaryItem(int indexInPrimary, double width)
		{
			SplitVector<object, NavigationViewSplitVectorID> vector = base.GetVector(NavigationViewSplitVectorID.PrimaryList);
			if (vector != null)
			{
				int index = vector.IndexToIndexInOriginalVector(indexInPrimary);
				this.SetWidthForItem(index, width);
			}
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x000141F4 File Offset: 0x000123F4
		public double WidthRequiredToRecoveryAllItemsToPrimary()
		{
			double num = 0.0;
			for (int i = 0; i < this.Size(); i++)
			{
				if (!this.IsItemInPrimaryList(i))
				{
					num += this.GetWidthForItem(i);
				}
			}
			num -= this.m_overflowButtonCachedWidth;
			return Math.Max(0.0, num);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00014248 File Offset: 0x00012448
		public bool HasInvalidWidth(List<int> items)
		{
			bool result = false;
			foreach (int index in items)
			{
				if (!this.IsValidWidthForItem(index))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x000142A0 File Offset: 0x000124A0
		public double GetWidthForItem(int index)
		{
			double num = base.AttachedData(index);
			if (!this.IsValidWidth(num))
			{
				num = 0.0;
			}
			return num;
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x000142CC File Offset: 0x000124CC
		public double CalculateWidthForItems(List<int> items)
		{
			double num = 0.0;
			foreach (int index in items)
			{
				num += this.GetWidthForItem(index);
			}
			return num;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00014328 File Offset: 0x00012528
		public void InvalidWidthCache()
		{
			base.ResetAttachedData(-1.0);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00014339 File Offset: 0x00012539
		public double OverflowButtonWidth()
		{
			return this.m_overflowButtonCachedWidth;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00014341 File Offset: 0x00012541
		public void OverflowButtonWidth(double width)
		{
			this.m_overflowButtonCachedWidth = width;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001434A File Offset: 0x0001254A
		public bool IsItemSelectableInPrimaryList(object value)
		{
			return this.IndexOf(value) != -1;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00014359 File Offset: 0x00012559
		public int IndexOf(object value, NavigationViewSplitVectorID vectorID)
		{
			return base.IndexOfImpl(value, vectorID);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00014364 File Offset: 0x00012564
		private void OnDataSourceChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			switch (args.Action)
			{
			case 0:
				base.OnInsertAt(args.NewStartingIndex, args.NewItems.Count);
				break;
			case 1:
				base.OnRemoveAt(args.OldStartingIndex, args.OldItems.Count);
				break;
			case 2:
				base.OnRemoveAt(args.OldStartingIndex, args.OldItems.Count);
				base.OnInsertAt(args.NewStartingIndex, args.NewItems.Count);
				break;
			case 4:
				base.OnClear();
				break;
			}
			if (this.m_dataChangeCallback != null)
			{
				this.m_dataChangeCallback.Invoke(args);
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00014410 File Offset: 0x00012610
		private bool IsValidWidth(double width)
		{
			return width >= 0.0 && width < double.MaxValue;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001442C File Offset: 0x0001262C
		public bool IsValidWidthForItem(int index)
		{
			double width = base.AttachedData(index);
			return this.IsValidWidth(width);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00014448 File Offset: 0x00012648
		private void SetWidthForItem(int index, double width)
		{
			if (this.IsValidWidth(width))
			{
				base.AttachedData(index, width);
			}
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001445C File Offset: 0x0001265C
		private void ChangeDataSource(ItemsSourceView newValue)
		{
			ItemsSourceView dataSource = this.m_dataSource;
			if (dataSource != newValue)
			{
				if (dataSource != null)
				{
					dataSource.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnDataSourceChanged);
				}
				base.Clear();
				this.m_dataSource = newValue;
				base.SyncAndInitVectorFlagsWithID(NavigationViewSplitVectorID.NotInitialized, this.DefaultAttachedData());
				if (newValue != null)
				{
					newValue.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnDataSourceChanged);
				}
			}
			base.MoveItemsToVector(NavigationViewSplitVectorID.NotInitialized);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x000144BF File Offset: 0x000126BF
		public bool IsItemInPrimaryList(int index)
		{
			return base.GetVectorIDForItem(index) == NavigationViewSplitVectorID.PrimaryList;
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x000144CC File Offset: 0x000126CC
		private bool IsContainerNavigationViewItem(int index)
		{
			bool result = true;
			object at = this.GetAt(index);
			if (at != null && (at is NavigationViewItemHeader || at is NavigationViewItemSeparator))
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x000144FC File Offset: 0x000126FC
		private bool IsContainerNavigationViewHeader(int index)
		{
			bool result = false;
			object at = this.GetAt(index);
			if (at != null && at is NavigationViewItemHeader)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0400028F RID: 655
		private ItemsSourceView m_dataSource;

		// Token: 0x04000290 RID: 656
		private object m_rawDataSource;

		// Token: 0x04000291 RID: 657
		private Action<NotifyCollectionChangedEventArgs> m_dataChangeCallback;

		// Token: 0x04000292 RID: 658
		private double m_overflowButtonCachedWidth;
	}
}
