using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace ModernWpf.Controls
{
	// Token: 0x020000CF RID: 207
	public class SelectionModel : INotifyPropertyChanged
	{
		// Token: 0x06000A2B RID: 2603 RVA: 0x00022A11 File Offset: 0x00020C11
		public SelectionModel()
		{
			this.m_rootNode = new SelectionNode(this, null);
			this.m_leafNode = new SelectionNode(this, null);
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x00022A34 File Offset: 0x00020C34
		~SelectionModel()
		{
			this.ClearSelection(false, false);
			this.m_rootNode = null;
			this.m_leafNode = null;
			this.m_selectedIndicesCached = null;
			this.m_selectedItemsCached = null;
		}

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06000A2D RID: 2605 RVA: 0x00022A80 File Offset: 0x00020C80
		// (remove) Token: 0x06000A2E RID: 2606 RVA: 0x00022AB8 File Offset: 0x00020CB8
		public event TypedEventHandler<SelectionModel, SelectionModelSelectionChangedEventArgs> SelectionChanged;

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06000A2F RID: 2607 RVA: 0x00022AF0 File Offset: 0x00020CF0
		// (remove) Token: 0x06000A30 RID: 2608 RVA: 0x00022B28 File Offset: 0x00020D28
		public event TypedEventHandler<SelectionModel, SelectionModelChildrenRequestedEventArgs> ChildrenRequested;

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x00022B5D File Offset: 0x00020D5D
		// (set) Token: 0x06000A32 RID: 2610 RVA: 0x00022B6A File Offset: 0x00020D6A
		public object Source
		{
			get
			{
				return this.m_rootNode.Source;
			}
			set
			{
				this.ClearSelection(true, false);
				this.m_rootNode.Source = value;
				this.OnSelectionChanged();
				this.RaisePropertyChanged("Source");
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x00022B91 File Offset: 0x00020D91
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x00022B9C File Offset: 0x00020D9C
		public bool SingleSelect
		{
			get
			{
				return this.m_singleSelect;
			}
			set
			{
				if (this.m_singleSelect != value)
				{
					this.m_singleSelect = value;
					IReadOnlyList<IndexPath> selectedIndices = this.SelectedIndices;
					if (value && selectedIndices != null && selectedIndices.Count > 1)
					{
						IndexPath index = selectedIndices[0];
						this.ClearSelection(true, false);
						this.SelectWithPathImpl(index, true, true);
					}
					this.RaisePropertyChanged("SingleSelect");
				}
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x00022BF4 File Offset: 0x00020DF4
		// (set) Token: 0x06000A36 RID: 2614 RVA: 0x00022C50 File Offset: 0x00020E50
		public IndexPath AnchorIndex
		{
			get
			{
				IndexPath result = null;
				if (this.m_rootNode.AnchorIndex >= 0)
				{
					List<int> list = new List<int>();
					SelectionNode selectionNode = this.m_rootNode;
					while (selectionNode != null && selectionNode.AnchorIndex >= 0)
					{
						list.Add(selectionNode.AnchorIndex);
						selectionNode = selectionNode.GetAt(selectionNode.AnchorIndex, false);
					}
					result = new IndexPath(list);
				}
				return result;
			}
			set
			{
				if (value != null)
				{
					SelectionTreeHelper.TraverseIndexPath(this.m_rootNode, value, true, delegate(SelectionNode currentNode, IndexPath path, int depth, int childIndex)
					{
						currentNode.AnchorIndex = path.GetAt(depth);
					});
				}
				else
				{
					this.m_rootNode.AnchorIndex = -1;
				}
				this.RaisePropertyChanged("AnchorIndex");
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x00022CA8 File Offset: 0x00020EA8
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x00022CD4 File Offset: 0x00020ED4
		public IndexPath SelectedIndex
		{
			get
			{
				IndexPath result = null;
				IReadOnlyList<IndexPath> selectedIndices = this.SelectedIndices;
				if (selectedIndices != null && selectedIndices.Count > 0)
				{
					result = selectedIndices[0];
				}
				return result;
			}
			set
			{
				bool? flag = this.IsSelectedAt(value);
				if (flag == null || !flag.Value)
				{
					this.ClearSelection(true, false);
					this.SelectWithPathImpl(value, true, false);
					this.OnSelectionChanged();
				}
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x00022D14 File Offset: 0x00020F14
		public object SelectedItem
		{
			get
			{
				object result = null;
				IReadOnlyList<object> selectedItems = this.SelectedItems;
				if (selectedItems != null && selectedItems.Count > 0)
				{
					result = selectedItems[0];
				}
				return result;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x00022D40 File Offset: 0x00020F40
		public IReadOnlyList<object> SelectedItems
		{
			get
			{
				if (this.m_selectedItemsCached == null)
				{
					List<SelectedItemInfo> selectedInfos = new List<SelectedItemInfo>();
					if (this.m_rootNode.Source != null)
					{
						SelectionTreeHelper.Traverse(this.m_rootNode, false, delegate(SelectionTreeHelper.TreeWalkNodeInfo currentInfo)
						{
							if (currentInfo.Node.SelectedCount > 0)
							{
								selectedInfos.Add(new SelectedItemInfo(currentInfo.Node, currentInfo.Path));
							}
						});
					}
					SelectedItems<object> selectedItemsCached = new SelectedItems<object>(selectedInfos, delegate(List<SelectedItemInfo> infos, int index)
					{
						int num = 0;
						object result = null;
						using (List<SelectedItemInfo>.Enumerator enumerator = infos.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								SelectionNode selectionNode;
								if (!enumerator.Current.Node.TryGetTarget(ref selectionNode))
								{
									throw new Exception("selection has changed since SelectedItems property was read.");
								}
								int selectedCount = selectionNode.SelectedCount;
								if (index >= num && index < num + selectedCount)
								{
									int index2 = selectionNode.SelectedIndices()[index - num];
									result = selectionNode.ItemsSourceView.GetAt(index2);
									break;
								}
								num += selectedCount;
							}
						}
						return result;
					});
					this.m_selectedItemsCached = selectedItemsCached;
				}
				return this.m_selectedItemsCached;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x00022DC4 File Offset: 0x00020FC4
		public IReadOnlyList<IndexPath> SelectedIndices
		{
			get
			{
				if (this.m_selectedIndicesCached == null)
				{
					List<SelectedItemInfo> selectedInfos = new List<SelectedItemInfo>();
					SelectionTreeHelper.Traverse(this.m_rootNode, false, delegate(SelectionTreeHelper.TreeWalkNodeInfo currentInfo)
					{
						if (currentInfo.Node.SelectedCount > 0)
						{
							selectedInfos.Add(new SelectedItemInfo(currentInfo.Node, currentInfo.Path));
						}
					});
					SelectedItems<IndexPath> selectedIndicesCached = new SelectedItems<IndexPath>(selectedInfos, delegate(List<SelectedItemInfo> infos, int index)
					{
						int num = 0;
						IndexPath result = null;
						foreach (SelectedItemInfo selectedItemInfo in infos)
						{
							SelectionNode selectionNode;
							if (!selectedItemInfo.Node.TryGetTarget(ref selectionNode))
							{
								throw new Exception("selection has changed since SelectedIndices property was read.");
							}
							int selectedCount = selectionNode.SelectedCount;
							if (index >= num && index < num + selectedCount)
							{
								int childIndex = selectionNode.SelectedIndices()[index - num];
								result = selectedItemInfo.Path.CloneWithChildIndex(childIndex);
								break;
							}
							num += selectedCount;
						}
						return result;
					});
					this.m_selectedIndicesCached = selectedIndicesCached;
				}
				return this.m_selectedIndicesCached;
			}
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x00022E3A File Offset: 0x0002103A
		public void SetAnchorIndex(int index)
		{
			this.AnchorIndex = new IndexPath(index);
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x00022E48 File Offset: 0x00021048
		public void SetAnchorIndex(int groupIndex, int itemIndex)
		{
			this.AnchorIndex = new IndexPath(groupIndex, itemIndex);
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x00022E57 File Offset: 0x00021057
		public void Select(int index)
		{
			this.SelectImpl(index, true);
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x00022E61 File Offset: 0x00021061
		public void Select(int groupIndex, int itemIndex)
		{
			this.SelectWithGroupImpl(groupIndex, itemIndex, true);
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00022E6C File Offset: 0x0002106C
		public void SelectAt(IndexPath index)
		{
			this.SelectWithPathImpl(index, true, true);
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00022E77 File Offset: 0x00021077
		public void Deselect(int index)
		{
			this.SelectImpl(index, false);
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x00022E81 File Offset: 0x00021081
		public void Deselect(int groupIndex, int itemIndex)
		{
			this.SelectWithGroupImpl(groupIndex, itemIndex, false);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x00022E8C File Offset: 0x0002108C
		public void DeselectAt(IndexPath index)
		{
			this.SelectWithPathImpl(index, false, true);
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00022E97 File Offset: 0x00021097
		public bool? IsSelected(int index)
		{
			return this.m_rootNode.IsSelectedWithPartial(index);
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x00022EA8 File Offset: 0x000210A8
		public bool? IsSelected(int groupIndex, int itemIndex)
		{
			bool? result = new bool?(false);
			SelectionNode at = this.m_rootNode.GetAt(groupIndex, false);
			if (at != null)
			{
				result = at.IsSelectedWithPartial(itemIndex);
			}
			return result;
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00022ED8 File Offset: 0x000210D8
		public bool? IsSelectedAt(IndexPath index)
		{
			bool flag = true;
			SelectionNode selectionNode = this.m_rootNode;
			for (int i = 0; i < index.GetSize() - 1; i++)
			{
				int at = index.GetAt(i);
				selectionNode = selectionNode.GetAt(at, false);
				if (selectionNode == null)
				{
					flag = false;
					break;
				}
			}
			bool? result = new bool?(false);
			if (flag)
			{
				int size = index.GetSize();
				if (size == 0)
				{
					result = SelectionNode.ConvertToNullableBool(selectionNode.EvaluateIsSelectedBasedOnChildrenNodes());
				}
				else
				{
					result = selectionNode.IsSelectedWithPartial(index.GetAt(size - 1));
				}
			}
			return result;
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00022F59 File Offset: 0x00021159
		public void SelectRangeFromAnchor(int index)
		{
			this.SelectRangeFromAnchorImpl(index, true);
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00022F63 File Offset: 0x00021163
		public void SelectRangeFromAnchor(int groupIndex, int itemIndex)
		{
			this.SelectRangeFromAnchorWithGroupImpl(groupIndex, itemIndex, true);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00022F6E File Offset: 0x0002116E
		public void SelectRangeFromAnchorTo(IndexPath index)
		{
			this.SelectRangeImpl(this.AnchorIndex, index, true);
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x00022F7E File Offset: 0x0002117E
		public void DeselectRangeFromAnchor(int index)
		{
			this.SelectRangeFromAnchorImpl(index, false);
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x00022F88 File Offset: 0x00021188
		public void DeselectRangeFromAnchor(int groupIndex, int itemIndex)
		{
			this.SelectRangeFromAnchorWithGroupImpl(groupIndex, itemIndex, false);
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x00022F93 File Offset: 0x00021193
		public void DeselectRangeFromAnchorTo(IndexPath index)
		{
			this.SelectRangeImpl(this.AnchorIndex, index, false);
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x00022FA3 File Offset: 0x000211A3
		public void SelectRange(IndexPath start, IndexPath end)
		{
			this.SelectRangeImpl(start, end, true);
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x00022FAE File Offset: 0x000211AE
		public void DeselectRange(IndexPath start, IndexPath end)
		{
			this.SelectRangeImpl(start, end, false);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00022FB9 File Offset: 0x000211B9
		public void SelectAll()
		{
			SelectionTreeHelper.Traverse(this.m_rootNode, true, delegate(SelectionTreeHelper.TreeWalkNodeInfo info)
			{
				if (info.Node.DataCount > 0)
				{
					info.Node.SelectAll();
				}
			});
			this.OnSelectionChanged();
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x00022FEC File Offset: 0x000211EC
		public void ClearSelection()
		{
			this.ClearSelection(true, true);
		}

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06000A51 RID: 2641 RVA: 0x00022FF8 File Offset: 0x000211F8
		// (remove) Token: 0x06000A52 RID: 2642 RVA: 0x00023030 File Offset: 0x00021230
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06000A53 RID: 2643 RVA: 0x00023065 File Offset: 0x00021265
		protected void OnPropertyChanged(string propertyName)
		{
			this.RaisePropertyChanged(propertyName);
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0002306E File Offset: 0x0002126E
		private void RaisePropertyChanged(string name)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x00023087 File Offset: 0x00021287
		internal void OnSelectionInvalidatedDueToCollectionChange()
		{
			this.OnSelectionChanged();
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0002308F File Offset: 0x0002128F
		internal SelectionNode SharedLeafNode()
		{
			return this.m_leafNode;
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00023098 File Offset: 0x00021298
		internal object ResolvePath(object data, IndexPath dataIndexPath)
		{
			object result = null;
			TypedEventHandler<SelectionModel, SelectionModelChildrenRequestedEventArgs> childrenRequested = this.ChildrenRequested;
			if (childrenRequested != null)
			{
				if (this.m_childrenRequestedEventArgs == null)
				{
					this.m_childrenRequestedEventArgs = new SelectionModelChildrenRequestedEventArgs(data, dataIndexPath, false);
				}
				else
				{
					this.m_childrenRequestedEventArgs.Initialize(data, dataIndexPath, false);
				}
				childrenRequested.Invoke(this, this.m_childrenRequestedEventArgs);
				result = this.m_childrenRequestedEventArgs.Children;
				this.m_childrenRequestedEventArgs.Initialize(null, null, true);
			}
			else if (data is ItemsSourceView || data is IList || data is IEnumerable)
			{
				result = data;
			}
			return result;
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0002311B File Offset: 0x0002131B
		private void ClearSelection(bool resetAnchor, bool raiseSelectionChanged)
		{
			SelectionTreeHelper.Traverse(this.m_rootNode, false, delegate(SelectionTreeHelper.TreeWalkNodeInfo info)
			{
				info.Node.Clear();
			});
			if (resetAnchor)
			{
				this.AnchorIndex = null;
			}
			if (raiseSelectionChanged)
			{
				this.OnSelectionChanged();
			}
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0002315C File Offset: 0x0002135C
		private void OnSelectionChanged()
		{
			this.m_selectedIndicesCached = null;
			this.m_selectedItemsCached = null;
			TypedEventHandler<SelectionModel, SelectionModelSelectionChangedEventArgs> selectionChanged = this.SelectionChanged;
			if (selectionChanged != null)
			{
				if (this.m_selectionChangedEventArgs == null)
				{
					this.m_selectionChangedEventArgs = new SelectionModelSelectionChangedEventArgs();
				}
				selectionChanged.Invoke(this, this.m_selectionChangedEventArgs);
			}
			this.RaisePropertyChanged("SelectedIndex");
			this.RaisePropertyChanged("SelectedIndices");
			if (this.m_rootNode.Source != null)
			{
				this.RaisePropertyChanged("SelectedItem");
				this.RaisePropertyChanged("SelectedItems");
			}
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x000231DC File Offset: 0x000213DC
		private void SelectImpl(int index, bool select)
		{
			if (this.m_rootNode.IsSelected(index) != select)
			{
				if (this.m_singleSelect)
				{
					this.ClearSelection(true, false);
				}
				if (this.m_rootNode.Select(index, select))
				{
					this.AnchorIndex = new IndexPath(index);
				}
				this.OnSelectionChanged();
			}
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00023229 File Offset: 0x00021429
		private void SelectWithGroupImpl(int groupIndex, int itemIndex, bool select)
		{
			if (this.m_singleSelect)
			{
				this.ClearSelection(true, false);
			}
			if (this.m_rootNode.GetAt(groupIndex, true).Select(itemIndex, select))
			{
				this.AnchorIndex = new IndexPath(groupIndex, itemIndex);
			}
			this.OnSelectionChanged();
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00023264 File Offset: 0x00021464
		private void SelectWithPathImpl(IndexPath index, bool select, bool raiseSelectionChanged)
		{
			bool flag = true;
			if (this.m_singleSelect)
			{
				IndexPath selectedIndex = this.SelectedIndex;
				if (selectedIndex != null)
				{
					if (select && selectedIndex.CompareTo(index) == 0)
					{
						flag = false;
					}
				}
				else
				{
					flag = select;
				}
			}
			if (flag)
			{
				bool selected = false;
				bool changedSelection = false;
				if (this.m_singleSelect & select)
				{
					this.ClearSelection(true, false);
				}
				SelectionTreeHelper.TraverseIndexPath(this.m_rootNode, index, true, delegate(SelectionNode currentNode, IndexPath path, int depth, int childIndex)
				{
					if (depth == path.GetSize() - 1)
					{
						if (currentNode.IsSelected(childIndex) != select)
						{
							changedSelection = true;
						}
						selected = currentNode.Select(childIndex, select);
					}
				});
				if (selected)
				{
					this.AnchorIndex = index;
				}
				this.m_selectedIndicesCached = null;
				this.m_selectedItemsCached = null;
				if (raiseSelectionChanged & changedSelection)
				{
					this.OnSelectionChanged();
				}
			}
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x0002331C File Offset: 0x0002151C
		private void SelectRangeFromAnchorImpl(int index, bool select)
		{
			int begin = 0;
			IndexPath anchorIndex = this.AnchorIndex;
			if (anchorIndex != null)
			{
				begin = anchorIndex.GetAt(0);
			}
			if (this.m_rootNode.SelectRange(new IndexRange(begin, index), select))
			{
				this.OnSelectionChanged();
			}
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00023358 File Offset: 0x00021558
		private void SelectRangeFromAnchorWithGroupImpl(int endGroupIndex, int endItemIndex, bool select)
		{
			int num = 0;
			int num2 = 0;
			IndexPath anchorIndex = this.AnchorIndex;
			if (anchorIndex != null)
			{
				num = anchorIndex.GetAt(0);
				num2 = anchorIndex.GetAt(1);
			}
			if (num > endGroupIndex || (num == endGroupIndex && num2 > endItemIndex))
			{
				int num3 = num;
				num = endGroupIndex;
				endGroupIndex = num3;
				int num4 = num2;
				num2 = endItemIndex;
				endItemIndex = num4;
			}
			bool flag = false;
			for (int i = num; i <= endGroupIndex; i++)
			{
				SelectionNode at = this.m_rootNode.GetAt(i, true);
				int begin = (i == num) ? num2 : 0;
				int end = (i == endGroupIndex) ? endItemIndex : (at.DataCount - 1);
				flag |= at.SelectRange(new IndexRange(begin, end), select);
			}
			if (flag)
			{
				this.OnSelectionChanged();
			}
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x000233F8 File Offset: 0x000215F8
		private void SelectRangeImpl(IndexPath start, IndexPath end, bool select)
		{
			IndexPath indexPath = start;
			IndexPath indexPath2 = end;
			if (indexPath2.CompareTo(indexPath) == -1)
			{
				IndexPath indexPath3 = indexPath;
				indexPath = indexPath2;
				indexPath2 = indexPath3;
			}
			SelectionTreeHelper.TraverseRangeRealizeChildren(this.m_rootNode, indexPath, indexPath2, delegate(SelectionTreeHelper.TreeWalkNodeInfo info)
			{
				if (info.Node.DataCount == 0)
				{
					info.ParentNode.Select(info.Path.GetAt(info.Path.GetSize() - 1), select);
				}
			});
			this.OnSelectionChanged();
		}

		// Token: 0x0400042E RID: 1070
		private SelectionNode m_rootNode;

		// Token: 0x0400042F RID: 1071
		private bool m_singleSelect;

		// Token: 0x04000430 RID: 1072
		private IReadOnlyList<IndexPath> m_selectedIndicesCached;

		// Token: 0x04000431 RID: 1073
		private IReadOnlyList<object> m_selectedItemsCached;

		// Token: 0x04000432 RID: 1074
		private SelectionModelChildrenRequestedEventArgs m_childrenRequestedEventArgs;

		// Token: 0x04000433 RID: 1075
		private SelectionModelSelectionChangedEventArgs m_selectionChangedEventArgs;

		// Token: 0x04000434 RID: 1076
		private SelectionNode m_leafNode;
	}
}
