using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ModernWpf.Controls
{
	// Token: 0x020000D3 RID: 211
	internal class SelectionNode
	{
		// Token: 0x06000A67 RID: 2663 RVA: 0x000234C8 File Offset: 0x000216C8
		public SelectionNode(SelectionModel manager, SelectionNode parent)
		{
			this.m_manager = manager;
			this.m_parent = parent;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00023508 File Offset: 0x00021708
		~SelectionNode()
		{
			this.UnhookCollectionChangedHandler();
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x00023534 File Offset: 0x00021734
		// (set) Token: 0x06000A6A RID: 2666 RVA: 0x0002353C File Offset: 0x0002173C
		public object Source
		{
			get
			{
				return this.m_source;
			}
			set
			{
				if (this.m_source != value)
				{
					this.ClearSelection();
					this.UnhookCollectionChangedHandler();
					this.m_source = value;
					ItemsSourceView itemsSourceView = value as ItemsSourceView;
					if (value != null && itemsSourceView == null)
					{
						itemsSourceView = new InspectingDataSource(value);
					}
					this.m_dataSource = itemsSourceView;
					this.HookupCollectionChangedHandler();
					this.OnSelectionChanged();
				}
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x0002358C File Offset: 0x0002178C
		public ItemsSourceView ItemsSourceView
		{
			get
			{
				return this.m_dataSource;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x00023594 File Offset: 0x00021794
		public int DataCount
		{
			get
			{
				if (this.m_dataSource != null)
				{
					return this.m_dataSource.Count;
				}
				return 0;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x000235AB File Offset: 0x000217AB
		public int ChildrenNodeCount
		{
			get
			{
				return this.m_childrenNodes.Count;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x000235B8 File Offset: 0x000217B8
		public int RealizedChildrenNodeCount
		{
			get
			{
				return this.m_realizedChildrenNodeCount;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x000235C0 File Offset: 0x000217C0
		// (set) Token: 0x06000A70 RID: 2672 RVA: 0x000235C8 File Offset: 0x000217C8
		public int AnchorIndex
		{
			get
			{
				return this.m_anchorIndex;
			}
			set
			{
				this.m_anchorIndex = value;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x000235D4 File Offset: 0x000217D4
		public IndexPath IndexPath
		{
			get
			{
				List<int> list = new List<int>();
				SelectionNode parent = this.m_parent;
				SelectionNode selectionNode = this;
				while (parent != null)
				{
					int num = parent.m_childrenNodes.IndexOf(selectionNode);
					list.Insert(0, num);
					selectionNode = parent;
					parent = parent.m_parent;
				}
				return new IndexPath(list);
			}
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x0002361C File Offset: 0x0002181C
		public SelectionNode GetAt(int index, bool realizeChild)
		{
			SelectionNode selectionNode = null;
			if (realizeChild)
			{
				if (this.m_childrenNodes.Count == 0 && this.m_dataSource != null)
				{
					for (int i = 0; i < this.m_dataSource.Count; i++)
					{
						this.m_childrenNodes.Add(null);
					}
				}
				if (this.m_childrenNodes[index] == null)
				{
					object at = this.m_dataSource.GetAt(index);
					if (at != null)
					{
						IndexPath dataIndexPath = this.IndexPath.CloneWithChildIndex(index);
						object obj = this.m_manager.ResolvePath(at, dataIndexPath);
						if (obj != null)
						{
							selectionNode = new SelectionNode(this.m_manager, this);
							selectionNode.Source = obj;
						}
						else
						{
							selectionNode = this.m_manager.SharedLeafNode();
						}
					}
					else
					{
						selectionNode = this.m_manager.SharedLeafNode();
					}
					this.m_childrenNodes[index] = selectionNode;
					this.m_realizedChildrenNodeCount++;
				}
				else
				{
					selectionNode = this.m_childrenNodes[index];
				}
			}
			else if (this.m_childrenNodes.Count > 0)
			{
				selectionNode = this.m_childrenNodes[index];
			}
			return selectionNode;
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x0002371F File Offset: 0x0002191F
		public int SelectedCount
		{
			get
			{
				return this.m_selectedCount;
			}
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00023728 File Offset: 0x00021928
		public bool IsSelected(int index)
		{
			bool result = false;
			using (List<IndexRange>.Enumerator enumerator = this.m_selected.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Contains(index))
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00023784 File Offset: 0x00021984
		public bool? IsSelectedWithPartial()
		{
			bool? result = new bool?(false);
			if (this.m_parent != null)
			{
				int num = this.m_parent.m_childrenNodes.IndexOf(this);
				if (num >= 0)
				{
					result = this.m_parent.IsSelectedWithPartial(num);
				}
			}
			return result;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x000237C8 File Offset: 0x000219C8
		public bool? IsSelectedWithPartial(int index)
		{
			SelectionState isSelected;
			if (this.m_childrenNodes.Count == 0 || this.m_childrenNodes.Count <= index || this.m_childrenNodes[index] == null || this.m_childrenNodes[index] == this.m_manager.SharedLeafNode())
			{
				isSelected = (this.IsSelected(index) ? SelectionState.Selected : SelectionState.NotSelected);
			}
			else
			{
				isSelected = this.m_childrenNodes[index].EvaluateIsSelectedBasedOnChildrenNodes();
			}
			return SelectionNode.ConvertToNullableBool(isSelected);
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x00023841 File Offset: 0x00021A41
		public int SelectedIndex()
		{
			if (this.SelectedCount <= 0)
			{
				return -1;
			}
			return this.SelectedIndices()[0];
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0002385A File Offset: 0x00021A5A
		public void SelectedIndex(int value)
		{
			if (this.IsValidIndex(value) && (this.SelectedCount != 1 || !this.IsSelected(value)))
			{
				this.ClearSelection();
				if (value != -1)
				{
					this.Select(value, true);
				}
			}
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0002388C File Offset: 0x00021A8C
		public List<int> SelectedIndices()
		{
			if (!this.m_selectedIndicesCacheIsValid)
			{
				this.m_selectedIndicesCacheIsValid = true;
				foreach (IndexRange indexRange in this.m_selected)
				{
					for (int i = indexRange.Begin; i <= indexRange.End; i++)
					{
						if (!this.m_selectedIndicesCached.Contains(i))
						{
							this.m_selectedIndicesCached.Add(i);
						}
					}
				}
				this.m_selectedIndicesCached.Sort();
			}
			return this.m_selectedIndicesCached;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x00023928 File Offset: 0x00021B28
		public bool Select(int index, bool select)
		{
			return this.Select(index, select, true);
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x00023933 File Offset: 0x00021B33
		public bool ToggleSelect(int index)
		{
			return this.Select(index, !this.IsSelected(index));
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00023948 File Offset: 0x00021B48
		public void SelectAll()
		{
			if (this.m_dataSource != null)
			{
				int count = this.m_dataSource.Count;
				if (count > 0)
				{
					this.SelectRange(new IndexRange(0, count - 1), true);
				}
			}
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0002397E File Offset: 0x00021B7E
		public void Clear()
		{
			this.ClearSelection();
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00023986 File Offset: 0x00021B86
		public bool SelectRange(IndexRange range, bool select)
		{
			if (this.IsValidIndex(range.Begin) && this.IsValidIndex(range.End))
			{
				if (select)
				{
					this.AddRange(range, true);
				}
				else
				{
					this.RemoveRange(range, true);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x000239BC File Offset: 0x00021BBC
		private void HookupCollectionChangedHandler()
		{
			if (this.m_dataSource != null)
			{
				this.m_dataSource.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnSourceListChanged);
			}
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x000239DD File Offset: 0x00021BDD
		private void UnhookCollectionChangedHandler()
		{
			if (this.m_dataSource != null)
			{
				this.m_dataSource.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnSourceListChanged);
			}
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x000239FE File Offset: 0x00021BFE
		private bool IsValidIndex(int index)
		{
			return this.ItemsSourceView == null || (index >= 0 && index < this.ItemsSourceView.Count);
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00023A20 File Offset: 0x00021C20
		private void AddRange(IndexRange addRange, bool raiseOnSelectionChanged)
		{
			int selectedCount = this.SelectedCount;
			for (int i = addRange.Begin; i <= addRange.End; i++)
			{
				if (!this.IsSelected(i))
				{
					this.m_selectedCount++;
				}
			}
			if (selectedCount != this.m_selectedCount)
			{
				this.m_selected.Add(addRange);
				if (raiseOnSelectionChanged)
				{
					this.OnSelectionChanged();
				}
			}
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00023A80 File Offset: 0x00021C80
		private void RemoveRange(IndexRange removeRange, bool raiseOnSelectionChanged)
		{
			int selectedCount = this.m_selectedCount;
			for (int i = removeRange.Begin; i <= removeRange.End; i++)
			{
				if (this.IsSelected(i))
				{
					this.m_selectedCount--;
				}
			}
			if (selectedCount != this.m_selectedCount)
			{
				List<IndexRange> list = new List<IndexRange>();
				List<IndexRange> list2 = new List<IndexRange>();
				foreach (IndexRange indexRange in this.m_selected)
				{
					if (removeRange.Intersects(indexRange))
					{
						IndexRange indexRange2 = new IndexRange(-1, -1);
						IndexRange indexRange3 = new IndexRange(-1, -1);
						IndexRange indexRange4 = new IndexRange(-1, -1);
						if (indexRange.Contains(removeRange.Begin - 1))
						{
							indexRange.Split(removeRange.Begin - 1, ref indexRange2, ref indexRange3);
							list2.Add(indexRange2);
						}
						if (indexRange.Contains(removeRange.End) && indexRange.Split(removeRange.End, ref indexRange3, ref indexRange4))
						{
							list2.Add(indexRange4);
						}
						list.Add(indexRange);
					}
				}
				if (list.Count > 0 || list2.Count > 0)
				{
					foreach (IndexRange indexRange5 in list)
					{
						this.m_selected.Remove(indexRange5);
					}
					foreach (IndexRange indexRange6 in list2)
					{
						this.m_selected.Add(indexRange6);
					}
					if (raiseOnSelectionChanged)
					{
						this.OnSelectionChanged();
					}
				}
			}
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x00023C50 File Offset: 0x00021E50
		private void ClearSelection()
		{
			if (this.m_selected.Count > 0)
			{
				this.m_selected.Clear();
				this.OnSelectionChanged();
			}
			this.m_selectedCount = 0;
			this.AnchorIndex = -1;
			this.m_childrenNodes.Clear();
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x00023C8C File Offset: 0x00021E8C
		private bool Select(int index, bool select, bool raiseOnSelectionChanged)
		{
			if (!this.IsValidIndex(index))
			{
				return false;
			}
			if (this.IsSelected(index) == select)
			{
				return true;
			}
			IndexRange indexRange = new IndexRange(index, index);
			if (select)
			{
				this.AddRange(indexRange, raiseOnSelectionChanged);
			}
			else
			{
				this.RemoveRange(indexRange, raiseOnSelectionChanged);
			}
			return true;
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00023CD0 File Offset: 0x00021ED0
		private void OnSourceListChanged(object dataSource, NotifyCollectionChangedEventArgs args)
		{
			bool flag = false;
			switch (args.Action)
			{
			case 0:
				flag = this.OnItemsAdded(args.NewStartingIndex, args.NewItems.Count);
				break;
			case 1:
				flag = this.OnItemsRemoved(args.OldStartingIndex, args.OldItems.Count);
				break;
			case 2:
				flag = this.OnItemsRemoved(args.OldStartingIndex, args.OldItems.Count);
				flag |= this.OnItemsAdded(args.NewStartingIndex, args.NewItems.Count);
				break;
			case 4:
				this.ClearSelection();
				flag = true;
				break;
			}
			if (flag)
			{
				this.OnSelectionChanged();
				this.m_manager.OnSelectionInvalidatedDueToCollectionChange();
			}
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00023D88 File Offset: 0x00021F88
		private bool OnItemsAdded(int index, int count)
		{
			bool flag = false;
			List<IndexRange> list = new List<IndexRange>();
			for (int i = 0; i < this.m_selected.Count; i++)
			{
				IndexRange indexRange = this.m_selected[i];
				if (indexRange.End >= index)
				{
					int num = indexRange.Begin;
					if (indexRange.Contains(index - 1))
					{
						IndexRange indexRange2 = new IndexRange(-1, -1);
						IndexRange indexRange3 = new IndexRange(-1, -1);
						indexRange.Split(index - 1, ref indexRange2, ref indexRange3);
						list.Add(indexRange2);
						num = index;
					}
					this.m_selected[i] = new IndexRange(num + count, indexRange.End + count);
					flag = true;
				}
			}
			if (list.Count > 0)
			{
				foreach (IndexRange indexRange4 in list)
				{
					this.m_selected.Add(indexRange4);
				}
			}
			if (this.m_childrenNodes.Count > 0)
			{
				flag = true;
				for (int j = 0; j < count; j++)
				{
					this.m_childrenNodes.Insert(index, null);
				}
			}
			if (this.AnchorIndex >= index)
			{
				this.AnchorIndex += count;
			}
			if (!flag)
			{
				for (SelectionNode parent = this.m_parent; parent != null; parent = parent.m_parent)
				{
					bool? flag2 = parent.IsSelectedWithPartial();
					if (flag2 != null && flag2.Value)
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00023EFC File Offset: 0x000220FC
		private bool OnItemsRemoved(int index, int count)
		{
			bool flag = false;
			if (this.ItemsSourceView.Count > 0)
			{
				bool flag2 = false;
				for (int i = index; i <= index + count - 1; i++)
				{
					if (this.IsSelected(i))
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					this.RemoveRange(new IndexRange(index, index + count - 1), false);
					flag = true;
				}
				for (int j = 0; j < this.m_selected.Count; j++)
				{
					IndexRange indexRange = this.m_selected[j];
					if (indexRange.End > index)
					{
						this.m_selected[j] = new IndexRange(indexRange.Begin - count, indexRange.End - count);
						flag = true;
					}
				}
				if (this.m_childrenNodes.Count > 0)
				{
					flag = true;
					for (int k = 0; k < count; k++)
					{
						if (this.m_childrenNodes[index] != null)
						{
							this.m_realizedChildrenNodeCount--;
						}
						this.m_childrenNodes.RemoveAt(index);
					}
				}
				if (this.AnchorIndex >= index)
				{
					this.AnchorIndex -= count;
				}
			}
			else
			{
				this.ClearSelection();
				this.m_realizedChildrenNodeCount = 0;
				flag = true;
			}
			if (!flag)
			{
				for (SelectionNode parent = this.m_parent; parent != null; parent = parent.m_parent)
				{
					if (parent.IsSelectedWithPartial() == null)
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00024046 File Offset: 0x00022246
		private void OnSelectionChanged()
		{
			this.m_selectedIndicesCacheIsValid = false;
			this.m_selectedIndicesCached.Clear();
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x0002405C File Offset: 0x0002225C
		public static bool? ConvertToNullableBool(SelectionState isSelected)
		{
			bool? result = default(bool?);
			if (isSelected == SelectionState.Selected)
			{
				result = new bool?(true);
			}
			else if (isSelected == SelectionState.NotSelected)
			{
				result = new bool?(false);
			}
			return result;
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x0002408C File Offset: 0x0002228C
		public SelectionState EvaluateIsSelectedBasedOnChildrenNodes()
		{
			SelectionState selectionState = SelectionState.NotSelected;
			int realizedChildrenNodeCount = this.RealizedChildrenNodeCount;
			int num = this.SelectedCount;
			if (realizedChildrenNodeCount != 0 || num != 0)
			{
				int dataCount = this.DataCount;
				if (realizedChildrenNodeCount == 0 && num > 0)
				{
					selectionState = ((dataCount != num) ? SelectionState.PartiallySelected : ((dataCount == num) ? SelectionState.Selected : SelectionState.NotSelected));
				}
				else
				{
					num = 0;
					int num2 = 0;
					for (int i = 0; i < this.ChildrenNodeCount; i++)
					{
						if (this.GetAt(i, false) != null)
						{
							bool? flag = this.IsSelectedWithPartial(i);
							if (flag == null)
							{
								selectionState = SelectionState.PartiallySelected;
								break;
							}
							if (flag.Value)
							{
								num++;
							}
							else
							{
								num2++;
							}
						}
						else if (this.IsSelected(i))
						{
							num++;
						}
						else
						{
							num2++;
						}
						if (num > 0 && num2 > 0)
						{
							selectionState = SelectionState.PartiallySelected;
							break;
						}
					}
					if (selectionState != SelectionState.PartiallySelected)
					{
						if (num != 0 && num != dataCount)
						{
							selectionState = SelectionState.PartiallySelected;
						}
						else
						{
							selectionState = ((num == dataCount) ? SelectionState.Selected : SelectionState.NotSelected);
						}
					}
				}
			}
			return selectionState;
		}

		// Token: 0x0400043D RID: 1085
		private SelectionModel m_manager;

		// Token: 0x0400043E RID: 1086
		private List<SelectionNode> m_childrenNodes = new List<SelectionNode>();

		// Token: 0x0400043F RID: 1087
		private SelectionNode m_parent;

		// Token: 0x04000440 RID: 1088
		private List<IndexRange> m_selected = new List<IndexRange>();

		// Token: 0x04000441 RID: 1089
		private object m_source;

		// Token: 0x04000442 RID: 1090
		private ItemsSourceView m_dataSource;

		// Token: 0x04000443 RID: 1091
		private int m_selectedCount;

		// Token: 0x04000444 RID: 1092
		private List<int> m_selectedIndicesCached = new List<int>();

		// Token: 0x04000445 RID: 1093
		private bool m_selectedIndicesCacheIsValid;

		// Token: 0x04000446 RID: 1094
		private int m_anchorIndex = -1;

		// Token: 0x04000447 RID: 1095
		private int m_realizedChildrenNodeCount;
	}
}
