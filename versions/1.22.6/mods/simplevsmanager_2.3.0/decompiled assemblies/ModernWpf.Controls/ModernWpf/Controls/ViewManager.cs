using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace ModernWpf.Controls
{
	// Token: 0x020000AB RID: 171
	internal class ViewManager
	{
		// Token: 0x0600084A RID: 2122 RVA: 0x0001CA50 File Offset: 0x0001AC50
		public ViewManager(ItemsRepeater owner)
		{
			this.m_owner = owner;
			this.m_resetPool = new UniqueIdElementPool(owner);
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0001CA8C File Offset: 0x0001AC8C
		public UIElement GetElement(int index, bool forceCreate, bool suppressAutoRecycle)
		{
			UIElement uielement = forceCreate ? null : this.GetElementIfAlreadyHeldByLayout(index);
			if (uielement == null)
			{
				UIElement madeAnchor = this.m_owner.MadeAnchor;
				if (madeAnchor != null && ItemsRepeater.TryGetVirtualizationInfo(madeAnchor).Index == index)
				{
					uielement = madeAnchor;
				}
			}
			if (uielement == null)
			{
				uielement = this.GetElementFromUniqueIdResetPool(index);
			}
			if (uielement == null)
			{
				uielement = this.GetElementFromPinnedElements(index);
			}
			if (uielement == null)
			{
				uielement = this.GetElementFromElementFactory(index);
			}
			VirtualizationInfo virtualizationInfo = ItemsRepeater.TryGetVirtualizationInfo(uielement);
			if (suppressAutoRecycle)
			{
				virtualizationInfo.AutoRecycleCandidate = false;
			}
			else
			{
				virtualizationInfo.AutoRecycleCandidate = true;
				virtualizationInfo.KeepAlive = true;
			}
			return uielement;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0001CB0C File Offset: 0x0001AD0C
		public void ClearElement(UIElement element, bool isClearedDueToCollectionChange)
		{
			VirtualizationInfo virtualizationInfo = ItemsRepeater.GetVirtualizationInfo(element);
			int index = virtualizationInfo.Index;
			if (!this.ClearElementToUniqueIdResetPool(element, virtualizationInfo) && !this.ClearElementToAnimator(element, virtualizationInfo) && !this.ClearElementToPinnedPool(element, virtualizationInfo, isClearedDueToCollectionChange))
			{
				this.ClearElementToElementFactory(element);
			}
			if (index == this.m_firstRealizedElementIndexHeldByLayout && index == this.m_lastRealizedElementIndexHeldByLayout)
			{
				this.InvalidateRealizedIndicesHeldByLayout();
				return;
			}
			if (index == this.m_firstRealizedElementIndexHeldByLayout)
			{
				this.m_firstRealizedElementIndexHeldByLayout++;
				return;
			}
			if (index == this.m_lastRealizedElementIndexHeldByLayout)
			{
				this.m_lastRealizedElementIndexHeldByLayout--;
			}
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0001CB98 File Offset: 0x0001AD98
		public void ClearElementToElementFactory(UIElement element)
		{
			this.m_owner.OnElementClearing(element);
			VirtualizationInfo virtualizationInfo = ItemsRepeater.GetVirtualizationInfo(element);
			virtualizationInfo.MoveOwnershipToElementFactory();
			if (virtualizationInfo.MustClearDataContext)
			{
				FrameworkElement frameworkElement = element as FrameworkElement;
				if (frameworkElement != null)
				{
					frameworkElement.DataContext = null;
				}
			}
			if (this.m_owner.ItemTemplateShim != null)
			{
				if (this.m_ElementFactoryRecycleArgs == null)
				{
					this.m_ElementFactoryRecycleArgs = new ElementFactoryRecycleArgs();
				}
				ElementFactoryRecycleArgs elementFactoryRecycleArgs = this.m_ElementFactoryRecycleArgs;
				elementFactoryRecycleArgs.Element = element;
				elementFactoryRecycleArgs.Parent = this.m_owner;
				this.m_owner.ItemTemplateShim.RecycleElement(elementFactoryRecycleArgs);
				elementFactoryRecycleArgs.Element = null;
				elementFactoryRecycleArgs.Parent = null;
			}
			else
			{
				UIElementCollection children = this.m_owner.Children;
				int num = 0;
				if (!children.IndexOf(element, out num))
				{
					throw new Exception("ItemsRepeater's child not found in its Children collection.");
				}
				children.RemoveAt(num);
			}
			if (this.m_lastFocusedElement == element)
			{
				int index = virtualizationInfo.Index;
				this.MoveFocusFromClearedIndex(index);
			}
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0001CC74 File Offset: 0x0001AE74
		public int GetElementIndex(VirtualizationInfo virtInfo)
		{
			if (virtInfo == null)
			{
				return -1;
			}
			if (!virtInfo.IsRealized && !virtInfo.IsInUniqueIdResetPool)
			{
				return -1;
			}
			return virtInfo.Index;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0001CC94 File Offset: 0x0001AE94
		public void PrunePinnedElements()
		{
			this.EnsureEventSubscriptions();
			for (int i = 0; i < this.m_pinnedPool.Count; i++)
			{
				ViewManager.PinnedElementInfo pinnedElementInfo = this.m_pinnedPool[i];
				if (!pinnedElementInfo.VirtualizationInfo.IsPinned)
				{
					this.m_pinnedPool.RemoveAt(i);
					i--;
					this.ClearElementToElementFactory(pinnedElementInfo.PinnedElement);
				}
			}
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0001CCF8 File Offset: 0x0001AEF8
		public void UpdatePin(UIElement element, bool addPin)
		{
			DependencyObject parent = CachedVisualTreeHelpers.GetParent(element);
			DependencyObject dependencyObject = element;
			while (parent != null)
			{
				ItemsRepeater itemsRepeater = parent as ItemsRepeater;
				if (itemsRepeater != null)
				{
					VirtualizationInfo virtualizationInfo = ItemsRepeater.GetVirtualizationInfo((UIElement)dependencyObject);
					if (virtualizationInfo.IsRealized)
					{
						if (addPin)
						{
							virtualizationInfo.AddPin();
						}
						else if (virtualizationInfo.IsPinned && virtualizationInfo.RemovePin() == 0U)
						{
							itemsRepeater.InvalidateMeasure();
						}
					}
				}
				dependencyObject = parent;
				parent = CachedVisualTreeHelpers.GetParent(dependencyObject);
			}
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0001CD5C File Offset: 0x0001AF5C
		public void OnItemsSourceChanged(object source, NotifyCollectionChangedEventArgs args)
		{
			switch (args.Action)
			{
			case 0:
			{
				int newStartingIndex = args.NewStartingIndex;
				int count = args.NewItems.Count;
				this.EnsureFirstLastRealizedIndices();
				if (newStartingIndex <= this.m_lastRealizedElementIndexHeldByLayout)
				{
					this.m_lastRealizedElementIndexHeldByLayout += count;
					UIElementCollection children = this.m_owner.Children;
					int count2 = children.Count;
					for (int i = 0; i < count2; i++)
					{
						UIElement element = children[i];
						VirtualizationInfo virtualizationInfo = ItemsRepeater.GetVirtualizationInfo(element);
						int index = virtualizationInfo.Index;
						if (virtualizationInfo.IsRealized && index >= newStartingIndex)
						{
							this.UpdateElementIndex(element, virtualizationInfo, index + count);
						}
					}
					return;
				}
				for (int j = 0; j < this.m_pinnedPool.Count; j++)
				{
					ViewManager.PinnedElementInfo pinnedElementInfo = this.m_pinnedPool[j];
					VirtualizationInfo virtualizationInfo2 = pinnedElementInfo.VirtualizationInfo;
					int index2 = virtualizationInfo2.Index;
					if (virtualizationInfo2.IsRealized && index2 >= newStartingIndex)
					{
						UIElement pinnedElement = pinnedElementInfo.PinnedElement;
						this.UpdateElementIndex(pinnedElement, virtualizationInfo2, index2 + count);
					}
				}
				return;
			}
			case 1:
			{
				int oldStartingIndex = args.OldStartingIndex;
				int count3 = args.OldItems.Count;
				UIElementCollection children2 = this.m_owner.Children;
				for (int k = 0; k < children2.Count; k++)
				{
					UIElement element2 = children2[k];
					VirtualizationInfo virtualizationInfo3 = ItemsRepeater.GetVirtualizationInfo(element2);
					int index3 = virtualizationInfo3.Index;
					if (virtualizationInfo3.IsRealized)
					{
						if (virtualizationInfo3.AutoRecycleCandidate && oldStartingIndex <= index3 && index3 < oldStartingIndex + count3)
						{
							this.m_owner.ClearElementImpl(element2);
						}
						else if (index3 >= oldStartingIndex + count3)
						{
							this.UpdateElementIndex(element2, virtualizationInfo3, index3 - count3);
						}
					}
				}
				this.InvalidateRealizedIndicesHeldByLayout();
				return;
			}
			case 2:
			{
				int oldStartingIndex2 = args.OldStartingIndex;
				int newStartingIndex2 = args.NewStartingIndex;
				int count4 = args.OldItems.Count;
				int count5 = args.NewItems.Count;
				if (oldStartingIndex2 != newStartingIndex2)
				{
					throw new Exception("Replace is only allowed with OldStartingIndex equals to NewStartingIndex.");
				}
				if (count4 == 0)
				{
					throw new Exception("Replace notification with args.OldItemsCount value of 0 is not allowed. Use Insert action instead.");
				}
				if (count5 == 0)
				{
					throw new Exception("Replace notification with args.NewItemCount value of 0 is not allowed. Use Remove action instead.");
				}
				int num = count5 - count4;
				if (num != 0)
				{
					UIElementCollection children3 = this.m_owner.Children;
					for (int l = 0; l < children3.Count; l++)
					{
						UIElement element3 = children3[l];
						VirtualizationInfo virtualizationInfo4 = ItemsRepeater.GetVirtualizationInfo(element3);
						int index4 = virtualizationInfo4.Index;
						if (virtualizationInfo4.IsRealized && index4 >= oldStartingIndex2 + count4)
						{
							this.UpdateElementIndex(element3, virtualizationInfo4, index4 + num);
						}
					}
					this.EnsureFirstLastRealizedIndices();
					this.m_lastRealizedElementIndexHeldByLayout += num;
					return;
				}
				break;
			}
			case 3:
				break;
			case 4:
				if (!this.m_isDataSourceStableResetPending)
				{
					if (this.m_owner.ItemsSourceView.HasKeyIndexMapping)
					{
						this.m_isDataSourceStableResetPending = true;
					}
					UIElementCollection children4 = this.m_owner.Children;
					for (int m = 0; m < children4.Count; m++)
					{
						UIElement element4 = children4[m];
						VirtualizationInfo virtualizationInfo5 = ItemsRepeater.GetVirtualizationInfo(element4);
						if (virtualizationInfo5.IsRealized && virtualizationInfo5.AutoRecycleCandidate)
						{
							this.m_owner.ClearElementImpl(element4);
						}
					}
				}
				this.InvalidateRealizedIndicesHeldByLayout();
				break;
			default:
				return;
			}
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0001D074 File Offset: 0x0001B274
		public void OnLayoutChanging()
		{
			if (this.m_owner.ItemsSourceView != null && this.m_owner.ItemsSourceView.HasKeyIndexMapping)
			{
				this.m_isDataSourceStableResetPending = true;
			}
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0001D09C File Offset: 0x0001B29C
		public void OnOwnerArranged()
		{
			if (this.m_isDataSourceStableResetPending)
			{
				this.m_isDataSourceStableResetPending = false;
				foreach (KeyValuePair<string, UIElement> keyValuePair in this.m_resetPool)
				{
					this.ClearElement(keyValuePair.Value, true);
				}
				this.m_resetPool.Clear();
				this.InvalidateRealizedIndicesHeldByLayout();
			}
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0001D110 File Offset: 0x0001B310
		private UIElement GetElementIfAlreadyHeldByLayout(int index)
		{
			UIElement result = null;
			bool flag = this.m_firstRealizedElementIndexHeldByLayout == int.MaxValue;
			bool flag2 = this.m_firstRealizedElementIndexHeldByLayout <= index && index <= this.m_lastRealizedElementIndexHeldByLayout;
			if (flag || flag2)
			{
				UIElementCollection children = this.m_owner.Children;
				for (int i = 0; i < children.Count; i++)
				{
					UIElement uielement = children[i];
					VirtualizationInfo virtualizationInfo = ItemsRepeater.TryGetVirtualizationInfo(uielement);
					if (virtualizationInfo != null && virtualizationInfo.IsHeldByLayout)
					{
						int index2 = virtualizationInfo.Index;
						this.m_firstRealizedElementIndexHeldByLayout = Math.Min(this.m_firstRealizedElementIndexHeldByLayout, index2);
						this.m_lastRealizedElementIndexHeldByLayout = Math.Max(this.m_lastRealizedElementIndexHeldByLayout, index2);
						if (virtualizationInfo.Index == index)
						{
							result = uielement;
							if (!flag)
							{
								break;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0001D1D0 File Offset: 0x0001B3D0
		private UIElement GetElementFromUniqueIdResetPool(int index)
		{
			UIElement uielement = null;
			if (this.m_isDataSourceStableResetPending)
			{
				uielement = this.m_resetPool.Remove(index);
				if (uielement != null)
				{
					VirtualizationInfo virtualizationInfo = ItemsRepeater.GetVirtualizationInfo(uielement);
					virtualizationInfo.MoveOwnershipToLayoutFromUniqueIdResetPool();
					this.UpdateElementIndex(uielement, virtualizationInfo, index);
					this.m_firstRealizedElementIndexHeldByLayout = Math.Min(this.m_firstRealizedElementIndexHeldByLayout, index);
					this.m_lastRealizedElementIndexHeldByLayout = Math.Max(this.m_lastRealizedElementIndexHeldByLayout, index);
				}
			}
			return uielement;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0001D234 File Offset: 0x0001B434
		private UIElement GetElementFromPinnedElements(int index)
		{
			UIElement result = null;
			for (int i = 0; i < this.m_pinnedPool.Count; i++)
			{
				ViewManager.PinnedElementInfo pinnedElementInfo = this.m_pinnedPool[i];
				if (pinnedElementInfo.VirtualizationInfo.Index == index)
				{
					this.m_pinnedPool.RemoveAt(i);
					result = pinnedElementInfo.PinnedElement;
					pinnedElementInfo.VirtualizationInfo.MoveOwnershipToLayoutFromPinnedPool();
					this.m_firstRealizedElementIndexHeldByLayout = Math.Min(this.m_firstRealizedElementIndexHeldByLayout, index);
					this.m_lastRealizedElementIndexHeldByLayout = Math.Max(this.m_lastRealizedElementIndexHeldByLayout, index);
					break;
				}
			}
			return result;
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0001D2BC File Offset: 0x0001B4BC
		private UIElement GetElementFromElementFactory(int index)
		{
			ViewManager.<>c__DisplayClass13_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.index = index;
			CS$<>8__locals1.data = this.m_owner.ItemsSourceView.GetAt(CS$<>8__locals1.index);
			UIElement uielement = this.<GetElementFromElementFactory>g__initElement|13_0(ref CS$<>8__locals1);
			VirtualizationInfo virtualizationInfo = ItemsRepeater.TryGetVirtualizationInfo(uielement);
			if (virtualizationInfo == null)
			{
				virtualizationInfo = ItemsRepeater.CreateAndInitializeVirtualizationInfo(uielement);
			}
			virtualizationInfo.MustClearDataContext = false;
			if (CS$<>8__locals1.data != uielement)
			{
				FrameworkElement frameworkElement = uielement as FrameworkElement;
				if (frameworkElement != null)
				{
					object dataContext = this.<GetElementFromElementFactory>g__initElementDataContext|13_3(ref CS$<>8__locals1);
					frameworkElement.DataContext = dataContext;
					virtualizationInfo.MustClearDataContext = true;
				}
			}
			virtualizationInfo.MoveOwnershipToLayoutFromElementFactory(CS$<>8__locals1.index, this.m_owner.ItemsSourceView.HasKeyIndexMapping ? this.m_owner.ItemsSourceView.KeyFromIndex(CS$<>8__locals1.index) : string.Empty);
			ItemsRepeater owner = this.m_owner;
			UIElementCollection children = owner.Children;
			if (CachedVisualTreeHelpers.GetParent(uielement) != owner)
			{
				children.Add(uielement);
			}
			owner.AnimationManager.OnElementPrepared(uielement);
			owner.OnElementPrepared(uielement, CS$<>8__locals1.index);
			this.m_firstRealizedElementIndexHeldByLayout = Math.Min(this.m_firstRealizedElementIndexHeldByLayout, CS$<>8__locals1.index);
			this.m_lastRealizedElementIndexHeldByLayout = Math.Max(this.m_lastRealizedElementIndexHeldByLayout, CS$<>8__locals1.index);
			return uielement;
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0001D3EA File Offset: 0x0001B5EA
		private bool ClearElementToUniqueIdResetPool(UIElement element, VirtualizationInfo virtInfo)
		{
			if (this.m_isDataSourceStableResetPending)
			{
				this.m_resetPool.Add(element);
				virtInfo.MoveOwnershipToUniqueIdResetPoolFromLayout();
			}
			return this.m_isDataSourceStableResetPending;
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0001D40C File Offset: 0x0001B60C
		private bool ClearElementToAnimator(UIElement element, VirtualizationInfo virtInfo)
		{
			bool flag = this.m_owner.AnimationManager.ClearElement(element);
			if (flag)
			{
				int index = virtInfo.Index;
				virtInfo.MoveOwnershipToAnimator();
				if (this.m_lastFocusedElement == element)
				{
					this.MoveFocusFromClearedIndex(index);
				}
			}
			return flag;
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0001D44A File Offset: 0x0001B64A
		private bool ClearElementToPinnedPool(UIElement element, VirtualizationInfo virtInfo, bool isClearedDueToCollectionChange)
		{
			bool flag = !isClearedDueToCollectionChange && virtInfo.IsPinned;
			if (flag)
			{
				this.m_pinnedPool.Add(new ViewManager.PinnedElementInfo(element));
				virtInfo.MoveOwnershipToPinnedPool();
			}
			return flag;
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0001D474 File Offset: 0x0001B674
		private void UpdateFocusedElement()
		{
			UIElement uielement = null;
			DependencyObject dependencyObject = Keyboard.FocusedElement as DependencyObject;
			if (dependencyObject != null)
			{
				DependencyObject parent = CachedVisualTreeHelpers.GetParent(dependencyObject);
				ItemsRepeater owner = this.m_owner;
				while (parent != null)
				{
					ItemsRepeater itemsRepeater = parent as ItemsRepeater;
					if (itemsRepeater != null)
					{
						UIElement uielement2 = dependencyObject as UIElement;
						if (itemsRepeater == owner && ItemsRepeater.GetVirtualizationInfo(uielement2).IsRealized)
						{
							uielement = uielement2;
							break;
						}
						break;
					}
					else
					{
						dependencyObject = parent;
						parent = CachedVisualTreeHelpers.GetParent(dependencyObject);
					}
				}
			}
			if (this.m_lastFocusedElement != uielement)
			{
				if (this.m_lastFocusedElement != null)
				{
					this.UpdatePin(this.m_lastFocusedElement, false);
				}
				if (uielement != null)
				{
					this.UpdatePin(uielement, true);
				}
				this.m_lastFocusedElement = uielement;
			}
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0001D509 File Offset: 0x0001B709
		private void OnFocusChanged(object sender, RoutedEventArgs args)
		{
			this.UpdateFocusedElement();
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0001D514 File Offset: 0x0001B714
		private void MoveFocusFromClearedIndex(int clearedIndex)
		{
			UIElement uielement = null;
			if (this.FindFocusCandidate(clearedIndex, ref uielement) != null)
			{
				this.m_lastFocusedElement = uielement;
				this.UpdatePin(uielement, true);
				return;
			}
			this.m_lastFocusedElement = null;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0001D548 File Offset: 0x0001B748
		private UIElement FindFocusCandidate(int clearedIndex, ref UIElement focusedChild)
		{
			int num = int.MinValue;
			int num2 = int.MaxValue;
			UIElement uielement = null;
			UIElement uielement2 = null;
			UIElementCollection children = this.m_owner.Children;
			for (int i = 0; i < children.Count; i++)
			{
				UIElement uielement3 = children[i];
				VirtualizationInfo virtualizationInfo = ItemsRepeater.TryGetVirtualizationInfo(uielement3);
				if (virtualizationInfo != null && virtualizationInfo.IsHeldByLayout)
				{
					int index = virtualizationInfo.Index;
					if (index < clearedIndex)
					{
						if (index > num)
						{
							num = index;
							uielement2 = uielement3;
						}
					}
					else if (index >= clearedIndex && index < num2)
					{
						num2 = index;
						uielement = uielement3;
					}
				}
			}
			UIElement uielement4 = null;
			if (uielement != null)
			{
				focusedChild = uielement;
				if (uielement.Focus())
				{
					uielement4 = uielement;
				}
				else if (uielement.MoveFocus(new TraversalRequest(2)))
				{
					uielement4 = (FocusManager.GetFocusedElement(uielement) as UIElement);
				}
			}
			if (uielement4 == null && uielement2 != null)
			{
				focusedChild = uielement2;
				if (uielement2.Focus())
				{
					uielement4 = uielement2;
				}
				else if (uielement2.MoveFocus(new TraversalRequest(3)))
				{
					uielement4 = (FocusManager.GetFocusedElement(uielement2) as UIElement);
				}
			}
			return uielement4;
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0001D63C File Offset: 0x0001B83C
		private void EnsureEventSubscriptions()
		{
			if (!this.m_gotFocus)
			{
				this.m_owner.GotFocus += new RoutedEventHandler(this.OnFocusChanged);
				this.m_gotFocus = true;
				this.m_owner.LostFocus += new RoutedEventHandler(this.OnFocusChanged);
				this.m_lostFocus = true;
			}
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0001D690 File Offset: 0x0001B890
		private void UpdateElementIndex(UIElement element, VirtualizationInfo virtInfo, int index)
		{
			int index2 = virtInfo.Index;
			if (index2 != index)
			{
				virtInfo.UpdateIndex(index);
				this.m_owner.OnElementIndexChanged(element, index2, index);
			}
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0001D6BD File Offset: 0x0001B8BD
		private void InvalidateRealizedIndicesHeldByLayout()
		{
			this.m_firstRealizedElementIndexHeldByLayout = int.MaxValue;
			this.m_lastRealizedElementIndexHeldByLayout = int.MinValue;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0001D6D5 File Offset: 0x0001B8D5
		private void EnsureFirstLastRealizedIndices()
		{
			if (this.m_firstRealizedElementIndexHeldByLayout == 2147483647)
			{
				this.GetElementIfAlreadyHeldByLayout(0);
			}
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0001D6EC File Offset: 0x0001B8EC
		[CompilerGenerated]
		private UIElement <GetElementFromElementFactory>g__initElement|13_0(ref ViewManager.<>c__DisplayClass13_0 A_1)
		{
			ViewManager.<>c__DisplayClass13_1 CS$<>8__locals1;
			CS$<>8__locals1.providedElementFactory = this.m_owner.ItemTemplateShim;
			if (CS$<>8__locals1.providedElementFactory == null)
			{
				UIElement uielement = A_1.data as UIElement;
				if (uielement != null)
				{
					return uielement;
				}
			}
			IElementFactoryShim elementFactoryShim = this.<GetElementFromElementFactory>g__initElementFactory|13_1(ref A_1, ref CS$<>8__locals1);
			ElementFactoryGetArgs elementFactoryGetArgs = this.<GetElementFromElementFactory>g__initArgs|13_2(ref A_1);
			UIElement element;
			try
			{
				elementFactoryGetArgs.Data = A_1.data;
				elementFactoryGetArgs.Parent = this.m_owner;
				elementFactoryGetArgs.Index = A_1.index;
				element = elementFactoryShim.GetElement(elementFactoryGetArgs);
			}
			finally
			{
				elementFactoryGetArgs.Data = null;
				elementFactoryGetArgs.Parent = null;
			}
			return element;
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0001D788 File Offset: 0x0001B988
		[CompilerGenerated]
		private IElementFactoryShim <GetElementFromElementFactory>g__initElementFactory|13_1(ref ViewManager.<>c__DisplayClass13_0 A_1, ref ViewManager.<>c__DisplayClass13_1 A_2)
		{
			if (A_2.providedElementFactory == null)
			{
				DataTemplate itemTemplate = XamlReader.Parse("<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'><TextBlock Text='{Binding}'/></DataTemplate>") as DataTemplate;
				this.m_owner.ItemTemplate = itemTemplate;
				return this.m_owner.ItemTemplateShim;
			}
			return A_2.providedElementFactory;
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0001D7CB File Offset: 0x0001B9CB
		[CompilerGenerated]
		private ElementFactoryGetArgs <GetElementFromElementFactory>g__initArgs|13_2(ref ViewManager.<>c__DisplayClass13_0 A_1)
		{
			if (this.m_ElementFactoryGetArgs == null)
			{
				this.m_ElementFactoryGetArgs = new ElementFactoryGetArgs();
			}
			return this.m_ElementFactoryGetArgs;
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0001D7E8 File Offset: 0x0001B9E8
		[CompilerGenerated]
		private object <GetElementFromElementFactory>g__initElementDataContext|13_3(ref ViewManager.<>c__DisplayClass13_0 A_1)
		{
			FrameworkElement frameworkElement = A_1.data as FrameworkElement;
			if (frameworkElement != null)
			{
				object dataContext = frameworkElement.DataContext;
				if (dataContext != null)
				{
					return dataContext;
				}
			}
			return A_1.data;
		}

		// Token: 0x04000398 RID: 920
		private readonly ItemsRepeater m_owner;

		// Token: 0x04000399 RID: 921
		private readonly List<ViewManager.PinnedElementInfo> m_pinnedPool = new List<ViewManager.PinnedElementInfo>();

		// Token: 0x0400039A RID: 922
		private readonly UniqueIdElementPool m_resetPool;

		// Token: 0x0400039B RID: 923
		private UIElement m_lastFocusedElement;

		// Token: 0x0400039C RID: 924
		private bool m_isDataSourceStableResetPending;

		// Token: 0x0400039D RID: 925
		private bool m_gotFocus;

		// Token: 0x0400039E RID: 926
		private bool m_lostFocus;

		// Token: 0x0400039F RID: 927
		private ElementFactoryGetArgs m_ElementFactoryGetArgs;

		// Token: 0x040003A0 RID: 928
		private ElementFactoryRecycleArgs m_ElementFactoryRecycleArgs;

		// Token: 0x040003A1 RID: 929
		private int m_firstRealizedElementIndexHeldByLayout = int.MaxValue;

		// Token: 0x040003A2 RID: 930
		private int m_lastRealizedElementIndexHeldByLayout = int.MinValue;

		// Token: 0x040003A3 RID: 931
		private const int FirstRealizedElementIndexDefault = 2147483647;

		// Token: 0x040003A4 RID: 932
		private const int LastRealizedElementIndexDefault = -2147483648;

		// Token: 0x0200011C RID: 284
		private struct PinnedElementInfo
		{
			// Token: 0x06000CE1 RID: 3297 RVA: 0x0002B677 File Offset: 0x00029877
			public PinnedElementInfo(UIElement element)
			{
				this.PinnedElement = element;
				this.VirtualizationInfo = ItemsRepeater.GetVirtualizationInfo(element);
			}

			// Token: 0x17000246 RID: 582
			// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x0002B68C File Offset: 0x0002988C
			public readonly UIElement PinnedElement { get; }

			// Token: 0x17000247 RID: 583
			// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x0002B694 File Offset: 0x00029894
			public readonly VirtualizationInfo VirtualizationInfo { get; }
		}
	}
}
