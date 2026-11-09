using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ModernWpf.Automation.Peers;

namespace ModernWpf.Controls
{
	// Token: 0x02000097 RID: 151
	[ContentProperty("ItemTemplate")]
	public class ItemsRepeater : Panel
	{
		// Token: 0x06000754 RID: 1876 RVA: 0x0001A1C4 File Offset: 0x000183C4
		static ItemsRepeater()
		{
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ItemsRepeater), new FrameworkPropertyMetadata(1));
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0001A39C File Offset: 0x0001859C
		public ItemsRepeater()
		{
			this.AnimationManager = new AnimationManager(this);
			this.ViewManager = new ViewManager(this);
			this.m_viewportManager = new ViewportManagerDownLevel(this);
			base.Loaded += new RoutedEventHandler(this.OnLoaded);
			base.Unloaded += new RoutedEventHandler(this.OnUnloaded);
			base.SetCurrentValue(ItemsRepeater.LayoutProperty, new StackLayout());
			VirtualizingLayout newValue = this.Layout as VirtualizingLayout;
			this.OnLayoutChanged(null, newValue);
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0001A41C File Offset: 0x0001861C
		~ItemsRepeater()
		{
			this.m_itemTemplate = null;
			this.m_animator = null;
			this.m_layout = null;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0001A458 File Offset: 0x00018658
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new RepeaterAutomationPeer(this);
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0001A460 File Offset: 0x00018660
		protected override Size MeasureOverride(Size availableSize)
		{
			if (this.m_isLayoutInProgress)
			{
				throw new InvalidOperationException("Reentrancy detected during layout.");
			}
			if (this.IsProcessingCollectionChange)
			{
				throw new InvalidOperationException("Cannot run layout in the middle of a collection change.");
			}
			this.m_viewportManager.OnOwnerMeasuring();
			this.m_isLayoutInProgress = true;
			Size result;
			try
			{
				this.ViewManager.PrunePinnedElements();
				Rect layoutExtent = default(Rect);
				Size size = default(Size);
				Layout layout = this.Layout;
				if (layout != null)
				{
					VirtualizingLayoutContext layoutContext = this.GetLayoutContext();
					if (this.m_isItemTemplateEmpty)
					{
						layoutExtent..ctor(this.m_layoutOrigin.X, this.m_layoutOrigin.Y, 0.0, 0.0);
					}
					else
					{
						size = layout.Measure(layoutContext, availableSize);
						layoutExtent..ctor(this.m_layoutOrigin.X, this.m_layoutOrigin.Y, size.Width, size.Height);
					}
					UIElementCollection children = base.Children;
					for (int i = 0; i < children.Count; i++)
					{
						UIElement element = children[i];
						VirtualizationInfo virtualizationInfo = ItemsRepeater.GetVirtualizationInfo(element);
						if (virtualizationInfo.Owner == ElementOwner.Layout && virtualizationInfo.AutoRecycleCandidate && !virtualizationInfo.KeepAlive)
						{
							this.ClearElementImpl(element);
						}
					}
				}
				this.m_viewportManager.SetLayoutExtent(layoutExtent);
				this.m_lastAvailableSize = availableSize;
				result = size;
			}
			finally
			{
				this.m_isLayoutInProgress = false;
			}
			return result;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0001A5D8 File Offset: 0x000187D8
		protected override Size ArrangeOverride(Size finalSize)
		{
			if (this.m_isLayoutInProgress)
			{
				throw new InvalidOperationException("Reentrancy detected during layout.");
			}
			if (this.IsProcessingCollectionChange)
			{
				throw new InvalidOperationException("Cannot run layout in the middle of a collection change.");
			}
			this.m_isLayoutInProgress = true;
			Size result;
			try
			{
				Size size = default(Size);
				Layout layout = this.Layout;
				if (layout != null)
				{
					size = layout.Arrange(this.GetLayoutContext(), finalSize);
				}
				this.ViewManager.OnOwnerArranged();
				UIElementCollection children = base.Children;
				for (int i = 0; i < children.Count; i++)
				{
					UIElement uielement = children[i];
					VirtualizationInfo virtualizationInfo = ItemsRepeater.GetVirtualizationInfo(uielement);
					virtualizationInfo.KeepAlive = false;
					if (virtualizationInfo.Owner == ElementOwner.ElementFactory || virtualizationInfo.Owner == ElementOwner.PinnedPool)
					{
						UIElement uielement2 = uielement;
						double x = ItemsRepeater.ClearedElementsArrangePosition.X;
						result = uielement.DesiredSize;
						double num = x - result.Width;
						double y = ItemsRepeater.ClearedElementsArrangePosition.Y;
						result = uielement.DesiredSize;
						uielement2.Arrange(new Rect(num, y - result.Height, 0.0, 0.0));
					}
					else
					{
						Rect layoutSlot = CachedVisualTreeHelpers.GetLayoutSlot(uielement as FrameworkElement);
						if (virtualizationInfo.ArrangeBounds != ItemsRepeater.InvalidRect && layoutSlot != virtualizationInfo.ArrangeBounds)
						{
							this.AnimationManager.OnElementBoundsChanged(uielement, virtualizationInfo.ArrangeBounds, layoutSlot);
						}
						virtualizationInfo.ArrangeBounds = layoutSlot;
						virtualizationInfo.DesiredSize = uielement.DesiredSize;
					}
				}
				this.m_viewportManager.OnOwnerArranged();
				this.AnimationManager.OnOwnerArranged();
				result = size;
			}
			finally
			{
				this.m_isLayoutInProgress = false;
			}
			return result;
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x0001A788 File Offset: 0x00018988
		// (set) Token: 0x0600075B RID: 1883 RVA: 0x0001A795 File Offset: 0x00018995
		public object ItemsSource
		{
			get
			{
				return base.GetValue(ItemsRepeater.ItemsSourceProperty);
			}
			set
			{
				base.SetValue(ItemsRepeater.ItemsSourceProperty, value);
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x0001A7A3 File Offset: 0x000189A3
		// (set) Token: 0x0600075D RID: 1885 RVA: 0x0001A7AB File Offset: 0x000189AB
		public ItemsSourceView ItemsSourceView { get; private set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x0001A7B4 File Offset: 0x000189B4
		// (set) Token: 0x0600075F RID: 1887 RVA: 0x0001A7C1 File Offset: 0x000189C1
		public object ItemTemplate
		{
			get
			{
				return base.GetValue(ItemsRepeater.ItemTemplateProperty);
			}
			set
			{
				base.SetValue(ItemsRepeater.ItemTemplateProperty, value);
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x0001A7CF File Offset: 0x000189CF
		// (set) Token: 0x06000761 RID: 1889 RVA: 0x0001A7E1 File Offset: 0x000189E1
		public Layout Layout
		{
			get
			{
				return (Layout)base.GetValue(ItemsRepeater.LayoutProperty);
			}
			set
			{
				base.SetValue(ItemsRepeater.LayoutProperty, value);
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x0001A7EF File Offset: 0x000189EF
		// (set) Token: 0x06000763 RID: 1891 RVA: 0x0001A801 File Offset: 0x00018A01
		internal ElementAnimator Animator
		{
			get
			{
				return (ElementAnimator)base.GetValue(ItemsRepeater.AnimatorProperty);
			}
			set
			{
				base.SetValue(ItemsRepeater.AnimatorProperty, value);
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x0001A80F File Offset: 0x00018A0F
		// (set) Token: 0x06000765 RID: 1893 RVA: 0x0001A821 File Offset: 0x00018A21
		public double HorizontalCacheLength
		{
			get
			{
				return (double)base.GetValue(ItemsRepeater.HorizontalCacheLengthProperty);
			}
			set
			{
				base.SetValue(ItemsRepeater.HorizontalCacheLengthProperty, value);
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x0001A834 File Offset: 0x00018A34
		// (set) Token: 0x06000767 RID: 1895 RVA: 0x0001A846 File Offset: 0x00018A46
		public double VerticalCacheLength
		{
			get
			{
				return (double)base.GetValue(ItemsRepeater.VerticalCacheLengthProperty);
			}
			set
			{
				base.SetValue(ItemsRepeater.VerticalCacheLengthProperty, value);
			}
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0001A859 File Offset: 0x00018A59
		public int GetElementIndex(UIElement element)
		{
			return this.GetElementIndexImpl(element);
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0001A862 File Offset: 0x00018A62
		public UIElement TryGetElement(int index)
		{
			return this.GetElementFromIndexImpl(index);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0001A86B File Offset: 0x00018A6B
		internal void PinElement(UIElement element)
		{
			this.ViewManager.UpdatePin(element, true);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0001A87A File Offset: 0x00018A7A
		internal void UnpinElement(UIElement element)
		{
			this.ViewManager.UpdatePin(element, false);
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0001A889 File Offset: 0x00018A89
		public UIElement GetOrCreateElement(int index)
		{
			return this.GetOrCreateElementImpl(index);
		}

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x0600076D RID: 1901 RVA: 0x0001A894 File Offset: 0x00018A94
		// (remove) Token: 0x0600076E RID: 1902 RVA: 0x0001A8CC File Offset: 0x00018ACC
		public event TypedEventHandler<ItemsRepeater, ItemsRepeaterElementClearingEventArgs> ElementClearing;

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x0600076F RID: 1903 RVA: 0x0001A904 File Offset: 0x00018B04
		// (remove) Token: 0x06000770 RID: 1904 RVA: 0x0001A93C File Offset: 0x00018B3C
		public event TypedEventHandler<ItemsRepeater, ItemsRepeaterElementIndexChangedEventArgs> ElementIndexChanged;

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06000771 RID: 1905 RVA: 0x0001A974 File Offset: 0x00018B74
		// (remove) Token: 0x06000772 RID: 1906 RVA: 0x0001A9AC File Offset: 0x00018BAC
		public event TypedEventHandler<ItemsRepeater, ItemsRepeaterElementPreparedEventArgs> ElementPrepared;

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x0001A9E1 File Offset: 0x00018BE1
		internal IElementFactoryShim ItemTemplateShim
		{
			get
			{
				return this.m_itemTemplateWrapper;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x0001A9E9 File Offset: 0x00018BE9
		internal ViewManager ViewManager { get; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x0001A9F1 File Offset: 0x00018BF1
		internal AnimationManager AnimationManager { get; }

		// Token: 0x06000776 RID: 1910 RVA: 0x0001A9F9 File Offset: 0x00018BF9
		internal UIElement GetElementImpl(int index, bool forceCreate, bool suppressAutoRecycle)
		{
			return this.ViewManager.GetElement(index, forceCreate, suppressAutoRecycle);
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0001AA0C File Offset: 0x00018C0C
		internal void ClearElementImpl(UIElement element)
		{
			bool isClearedDueToCollectionChange = this.IsProcessingCollectionChange && (this.m_processingItemsSourceChange.Action == 1 || this.m_processingItemsSourceChange.Action == 2 || this.m_processingItemsSourceChange.Action == 4);
			this.ViewManager.ClearElement(element, isClearedDueToCollectionChange);
			this.m_viewportManager.OnElementCleared(element);
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0001AA6C File Offset: 0x00018C6C
		internal int GetElementIndexImpl(UIElement element)
		{
			if (VisualTreeHelper.GetParent(element) == this)
			{
				VirtualizationInfo virtInfo = ItemsRepeater.TryGetVirtualizationInfo(element);
				return this.ViewManager.GetElementIndex(virtInfo);
			}
			return -1;
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0001AA98 File Offset: 0x00018C98
		internal UIElement GetElementFromIndexImpl(int index)
		{
			UIElement uielement = null;
			UIElementCollection children = base.Children;
			int num = 0;
			while (num < children.Count && uielement == null)
			{
				UIElement uielement2 = children[num];
				VirtualizationInfo virtualizationInfo = ItemsRepeater.TryGetVirtualizationInfo(uielement2);
				if (virtualizationInfo != null && virtualizationInfo.IsRealized && virtualizationInfo.Index == index)
				{
					uielement = uielement2;
				}
				num++;
			}
			return uielement;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0001AAEC File Offset: 0x00018CEC
		internal UIElement GetOrCreateElementImpl(int index)
		{
			if (index >= 0 && index >= this.ItemsSourceView.Count)
			{
				throw new ArgumentException("index", "Argument index is invalid.");
			}
			if (this.m_isLayoutInProgress)
			{
				throw new InvalidOperationException("GetOrCreateElement invocation is not allowed during layout.");
			}
			UIElement uielement = this.GetElementFromIndexImpl(index);
			bool flag = uielement == null;
			if (flag)
			{
				if (this.Layout == null)
				{
					throw new InvalidOperationException("Cannot make an Anchor when there is no attached layout.");
				}
				uielement = this.GetLayoutContext().GetOrCreateElementAt(index);
				uielement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			}
			this.m_viewportManager.OnMakeAnchor(uielement, flag);
			base.InvalidateMeasure();
			return uielement;
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0001AB8F File Offset: 0x00018D8F
		internal static VirtualizationInfo TryGetVirtualizationInfo(UIElement element)
		{
			return (VirtualizationInfo)element.GetValue(ItemsRepeater.VirtualizationInfoProperty);
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0001ABA4 File Offset: 0x00018DA4
		internal static VirtualizationInfo GetVirtualizationInfo(UIElement element)
		{
			VirtualizationInfo virtualizationInfo = ItemsRepeater.TryGetVirtualizationInfo(element);
			if (virtualizationInfo == null)
			{
				virtualizationInfo = ItemsRepeater.CreateAndInitializeVirtualizationInfo(element);
			}
			return virtualizationInfo;
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0001ABC4 File Offset: 0x00018DC4
		internal static VirtualizationInfo CreateAndInitializeVirtualizationInfo(UIElement element)
		{
			VirtualizationInfo virtualizationInfo = new VirtualizationInfo();
			element.SetValue(ItemsRepeater.VirtualizationInfoProperty, virtualizationInfo);
			return virtualizationInfo;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0001ABE4 File Offset: 0x00018DE4
		private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((ItemsRepeater)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0001ABF4 File Offset: 0x00018DF4
		private void PrivateOnPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			DependencyProperty property = args.Property;
			if (property == ItemsRepeater.ItemsSourceProperty)
			{
				if (args.NewValue != args.OldValue)
				{
					object newValue = args.NewValue;
					ItemsSourceView itemsSourceView = newValue as ItemsSourceView;
					if (newValue != null && itemsSourceView == null)
					{
						itemsSourceView = new InspectingDataSource(newValue);
					}
					this.OnDataSourcePropertyChanged(this.ItemsSourceView, itemsSourceView);
					return;
				}
			}
			else
			{
				if (property == ItemsRepeater.ItemTemplateProperty)
				{
					this.OnItemTemplateChanged(args.OldValue, args.NewValue);
					return;
				}
				if (property == ItemsRepeater.LayoutProperty)
				{
					this.OnLayoutChanged((Layout)args.OldValue, (Layout)args.NewValue);
					return;
				}
				if (property == ItemsRepeater.AnimatorProperty)
				{
					this.OnAnimatorChanged((ElementAnimator)args.OldValue, (ElementAnimator)args.NewValue);
					return;
				}
				if (property == ItemsRepeater.HorizontalCacheLengthProperty)
				{
					this.m_viewportManager.HorizontalCacheLength = (double)args.NewValue;
					return;
				}
				if (property == ItemsRepeater.VerticalCacheLengthProperty)
				{
					this.m_viewportManager.VerticalCacheLength = (double)args.NewValue;
				}
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x0001ACF8 File Offset: 0x00018EF8
		// (set) Token: 0x06000781 RID: 1921 RVA: 0x0001AD00 File Offset: 0x00018F00
		internal object LayoutState { get; set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x0001AD09 File Offset: 0x00018F09
		internal Rect VisibleWindow
		{
			get
			{
				return this.m_viewportManager.GetLayoutVisibleWindow();
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x0001AD16 File Offset: 0x00018F16
		internal Rect RealizationWindow
		{
			get
			{
				return this.m_viewportManager.GetLayoutRealizationWindow();
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x0001AD23 File Offset: 0x00018F23
		internal UIElement SuggestedAnchor
		{
			get
			{
				return this.m_viewportManager.SuggestedAnchor;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x0001AD30 File Offset: 0x00018F30
		internal UIElement MadeAnchor
		{
			get
			{
				return this.m_viewportManager.MadeAnchor;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x0001AD3D File Offset: 0x00018F3D
		// (set) Token: 0x06000787 RID: 1927 RVA: 0x0001AD45 File Offset: 0x00018F45
		internal Point LayoutOrigin
		{
			get
			{
				return this.m_layoutOrigin;
			}
			set
			{
				this.m_layoutOrigin = value;
			}
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0001AD50 File Offset: 0x00018F50
		internal void OnElementPrepared(UIElement element, int index)
		{
			this.m_viewportManager.OnElementPrepared(element);
			if (this.ElementPrepared != null)
			{
				if (this.m_elementPreparedArgs == null)
				{
					this.m_elementPreparedArgs = new ItemsRepeaterElementPreparedEventArgs(element, index);
				}
				else
				{
					this.m_elementPreparedArgs.Update(element, index);
				}
				this.ElementPrepared.Invoke(this, this.m_elementPreparedArgs);
			}
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0001ADA7 File Offset: 0x00018FA7
		internal void OnElementClearing(UIElement element)
		{
			if (this.ElementClearing != null)
			{
				if (this.m_elementClearingArgs == null)
				{
					this.m_elementClearingArgs = new ItemsRepeaterElementClearingEventArgs(element);
				}
				else
				{
					this.m_elementClearingArgs.Update(element);
				}
				this.ElementClearing.Invoke(this, this.m_elementClearingArgs);
			}
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0001ADE8 File Offset: 0x00018FE8
		internal void OnElementIndexChanged(UIElement element, int oldIndex, int newIndex)
		{
			if (this.ElementIndexChanged != null)
			{
				if (this.m_elementIndexChangedArgs == null)
				{
					this.m_elementIndexChangedArgs = new ItemsRepeaterElementIndexChangedEventArgs(element, oldIndex, newIndex);
				}
				else
				{
					this.m_elementIndexChangedArgs.Update(element, oldIndex, newIndex);
				}
				this.ElementIndexChanged.Invoke(this, this.m_elementIndexChangedArgs);
			}
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0001AE35 File Offset: 0x00019035
		private void OnLoaded(object sender, RoutedEventArgs args)
		{
			if (this._loadedCounter > this._unloadedCounter)
			{
				this._loadedCounter = this._unloadedCounter;
				base.InvalidateMeasure();
				this.m_viewportManager.ResetScrollers();
			}
			this._loadedCounter++;
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0001AE70 File Offset: 0x00019070
		private void OnUnloaded(object sender, RoutedEventArgs args)
		{
			this._unloadedCounter++;
			if (this._unloadedCounter == this._loadedCounter)
			{
				this.m_viewportManager.ResetScrollers();
			}
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0001AE9C File Offset: 0x0001909C
		private void OnDataSourcePropertyChanged(ItemsSourceView oldValue, ItemsSourceView newValue)
		{
			if (this.m_isLayoutInProgress)
			{
				throw new InvalidOperationException("Cannot set ItemsSourceView during layout.");
			}
			this.ItemsSourceView = newValue;
			if (oldValue != null)
			{
				oldValue.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnItemsSourceViewChanged);
			}
			if (newValue != null)
			{
				newValue.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnItemsSourceViewChanged);
			}
			bool flag = false;
			try
			{
				Layout layout = this.Layout;
				if (layout != null)
				{
					NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(4);
					this.m_processingItemsSourceChange = notifyCollectionChangedEventArgs;
					flag = true;
					VirtualizingLayout virtualizingLayout = layout as VirtualizingLayout;
					if (virtualizingLayout != null)
					{
						((IVirtualizingLayoutOverrides)virtualizingLayout).OnItemsChangedCore(this.GetLayoutContext(), newValue, notifyCollectionChangedEventArgs);
					}
					else if (layout is NonVirtualizingLayout)
					{
						foreach (object obj in base.Children)
						{
							UIElement element = (UIElement)obj;
							if (ItemsRepeater.GetVirtualizationInfo(element).IsRealized)
							{
								this.ClearElementImpl(element);
							}
						}
						base.Children.Clear();
					}
					base.InvalidateMeasure();
				}
			}
			finally
			{
				if (flag)
				{
					this.m_processingItemsSourceChange = null;
				}
			}
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0001AFBC File Offset: 0x000191BC
		private void OnItemTemplateChanged(object oldValue, object newValue)
		{
			if (this.m_isLayoutInProgress && oldValue != null)
			{
				throw new InvalidOperationException("ItemTemplate cannot be changed during layout.");
			}
			bool flag = false;
			try
			{
				Layout layout = this.Layout;
				if (layout != null)
				{
					NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(4);
					this.m_processingItemsSourceChange = notifyCollectionChangedEventArgs;
					flag = true;
					VirtualizingLayout virtualizingLayout = layout as VirtualizingLayout;
					if (virtualizingLayout != null)
					{
						((IVirtualizingLayoutOverrides)virtualizingLayout).OnItemsChangedCore(this.GetLayoutContext(), newValue, notifyCollectionChangedEventArgs);
					}
					else if (layout is NonVirtualizingLayout)
					{
						foreach (object obj in base.Children)
						{
							UIElement element = (UIElement)obj;
							if (ItemsRepeater.GetVirtualizationInfo(element).IsRealized)
							{
								this.ClearElementImpl(element);
							}
						}
					}
				}
				this.m_itemTemplate = newValue;
				this.m_isItemTemplateEmpty = false;
				this.m_itemTemplateWrapper = (newValue as IElementFactoryShim);
				if (this.m_itemTemplateWrapper == null)
				{
					DataTemplate dataTemplate = newValue as DataTemplate;
					if (dataTemplate != null)
					{
						this.m_itemTemplateWrapper = new ItemTemplateWrapper(dataTemplate);
						if (!(dataTemplate.LoadContent() is FrameworkElement))
						{
							this.m_isItemTemplateEmpty = true;
						}
					}
					else
					{
						DataTemplateSelector dataTemplateSelector = newValue as DataTemplateSelector;
						if (dataTemplateSelector == null)
						{
							throw new ArgumentException("newValue", "ItemTemplate");
						}
						this.m_itemTemplateWrapper = new ItemTemplateWrapper(dataTemplateSelector);
					}
				}
				base.InvalidateMeasure();
			}
			finally
			{
				if (flag)
				{
					this.m_processingItemsSourceChange = null;
				}
			}
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0001B138 File Offset: 0x00019338
		private void OnLayoutChanged(Layout oldValue, Layout newValue)
		{
			if (this.m_isLayoutInProgress)
			{
				throw new InvalidOperationException("Layout cannot be changed during layout.");
			}
			this.ViewManager.OnLayoutChanging();
			this.AnimationManager.OnLayoutChanging();
			if (oldValue != null)
			{
				oldValue.UninitializeForContext(this.GetLayoutContext());
				oldValue.MeasureInvalidated -= new TypedEventHandler<Layout, object>(this.InvalidateMeasureForLayout);
				oldValue.ArrangeInvalidated -= new TypedEventHandler<Layout, object>(this.InvalidateArrangeForLayout);
				UIElementCollection children = base.Children;
				for (int i = 0; i < children.Count; i++)
				{
					UIElement element = children[i];
					if (ItemsRepeater.GetVirtualizationInfo(element).IsRealized)
					{
						this.ClearElementImpl(element);
					}
				}
				this.LayoutState = null;
			}
			this.m_layout = newValue;
			if (newValue != null)
			{
				newValue.InitializeForContext(this.GetLayoutContext());
				newValue.MeasureInvalidated += new TypedEventHandler<Layout, object>(this.InvalidateMeasureForLayout);
				newValue.ArrangeInvalidated += new TypedEventHandler<Layout, object>(this.InvalidateArrangeForLayout);
			}
			bool isVirtualizing = newValue != null && newValue is VirtualizingLayout;
			this.m_viewportManager.OnLayoutChanged(isVirtualizing);
			base.InvalidateMeasure();
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0001B238 File Offset: 0x00019438
		private void OnAnimatorChanged(ElementAnimator oldValue, ElementAnimator newValue)
		{
			this.AnimationManager.OnAnimatorChanged(newValue);
			this.m_animator = newValue;
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0001B250 File Offset: 0x00019450
		private void OnItemsSourceViewChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (this.m_isLayoutInProgress)
			{
				throw new InvalidOperationException("Changes in data source are not allowed during layout.");
			}
			if (this.IsProcessingCollectionChange)
			{
				throw new InvalidOperationException("Changes in the data source are not allowed during another change in the data source.");
			}
			this.m_processingItemsSourceChange = args;
			try
			{
				this.AnimationManager.OnItemsSourceChanged(sender, args);
				this.ViewManager.OnItemsSourceChanged(sender, args);
				Layout layout = this.Layout;
				if (layout != null)
				{
					VirtualizingLayout virtualizingLayout = layout as VirtualizingLayout;
					if (virtualizingLayout != null)
					{
						((IVirtualizingLayoutOverrides)virtualizingLayout).OnItemsChangedCore(this.GetLayoutContext(), sender, args);
					}
					else
					{
						base.InvalidateMeasure();
					}
				}
			}
			finally
			{
				this.m_processingItemsSourceChange = null;
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0001B2E8 File Offset: 0x000194E8
		private void InvalidateMeasureForLayout(object sender, object args)
		{
			base.InvalidateMeasure();
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0001B2F0 File Offset: 0x000194F0
		private void InvalidateArrangeForLayout(object sender, object args)
		{
			base.InvalidateArrange();
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0001B2F8 File Offset: 0x000194F8
		private VirtualizingLayoutContext GetLayoutContext()
		{
			if (this.m_layoutContext == null)
			{
				this.m_layoutContext = new RepeaterLayoutContext(this);
			}
			return this.m_layoutContext;
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x0001B314 File Offset: 0x00019514
		private bool IsProcessingCollectionChange
		{
			get
			{
				return this.m_processingItemsSourceChange != null;
			}
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0001B320 File Offset: 0x00019520
		protected override void OnChildDesiredSizeChanged(UIElement child)
		{
			VirtualizationInfo virtualizationInfo = ItemsRepeater.TryGetVirtualizationInfo(child);
			if (virtualizationInfo != null && virtualizationInfo.IsRealized)
			{
				Size desiredSize = virtualizationInfo.DesiredSize;
				if (!desiredSize.IsEmpty)
				{
					Size desiredSize2 = child.DesiredSize;
					Size renderSize = child.RenderSize;
					if (desiredSize == ItemsRepeater.s_zeroSize || desiredSize2 == ItemsRepeater.s_zeroSize || (desiredSize2.Height != desiredSize.Height && renderSize.Height == desiredSize.Height) || (desiredSize2.Width != desiredSize.Width && renderSize.Width == desiredSize.Width))
					{
						base.OnChildDesiredSizeChanged(child);
					}
				}
			}
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0001B3C1 File Offset: 0x000195C1
		protected override UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent)
		{
			return new RepeaterUIElementCollection(this, logicalParent);
		}

		// Token: 0x0400034D RID: 845
		internal static readonly Point ClearedElementsArrangePosition = new Point(-10000.0, -10000.0);

		// Token: 0x0400034E RID: 846
		internal static readonly Rect InvalidRect = Rect.Empty;

		// Token: 0x0400034F RID: 847
		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(ItemsRepeater), new PropertyMetadata(new PropertyChangedCallback(ItemsRepeater.OnPropertyChanged)));

		// Token: 0x04000351 RID: 849
		public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(object), typeof(ItemsRepeater), new PropertyMetadata(new PropertyChangedCallback(ItemsRepeater.OnPropertyChanged)));

		// Token: 0x04000352 RID: 850
		public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register("Layout", typeof(Layout), typeof(ItemsRepeater), new PropertyMetadata(new PropertyChangedCallback(ItemsRepeater.OnPropertyChanged)));

		// Token: 0x04000353 RID: 851
		public static readonly DependencyProperty AnimatorProperty = DependencyProperty.Register("Animator", typeof(ElementAnimator), typeof(ItemsRepeater), new PropertyMetadata(new PropertyChangedCallback(ItemsRepeater.OnPropertyChanged)));

		// Token: 0x04000354 RID: 852
		public static readonly DependencyProperty HorizontalCacheLengthProperty = DependencyProperty.Register("HorizontalCacheLength", typeof(double), typeof(ItemsRepeater), new PropertyMetadata(2.0, new PropertyChangedCallback(ItemsRepeater.OnPropertyChanged)));

		// Token: 0x04000355 RID: 853
		public static readonly DependencyProperty VerticalCacheLengthProperty = DependencyProperty.Register("VerticalCacheLength", typeof(double), typeof(ItemsRepeater), new PropertyMetadata(2.0, new PropertyChangedCallback(ItemsRepeater.OnPropertyChanged)));

		// Token: 0x0400035C RID: 860
		internal static readonly DependencyProperty VirtualizationInfoProperty = DependencyProperty.RegisterAttached("VirtualizationInfo", typeof(VirtualizationInfo), typeof(ItemsRepeater), null);

		// Token: 0x0400035D RID: 861
		private readonly ViewportManager m_viewportManager;

		// Token: 0x0400035E RID: 862
		private IElementFactoryShim m_itemTemplateWrapper;

		// Token: 0x0400035F RID: 863
		private VirtualizingLayoutContext m_layoutContext;

		// Token: 0x04000360 RID: 864
		private NotifyCollectionChangedEventArgs m_processingItemsSourceChange;

		// Token: 0x04000361 RID: 865
		private Size m_lastAvailableSize;

		// Token: 0x04000362 RID: 866
		private bool m_isLayoutInProgress;

		// Token: 0x04000363 RID: 867
		private Point m_layoutOrigin;

		// Token: 0x04000364 RID: 868
		private ItemsRepeaterElementPreparedEventArgs m_elementPreparedArgs;

		// Token: 0x04000365 RID: 869
		private ItemsRepeaterElementClearingEventArgs m_elementClearingArgs;

		// Token: 0x04000366 RID: 870
		private ItemsRepeaterElementIndexChangedEventArgs m_elementIndexChangedArgs;

		// Token: 0x04000367 RID: 871
		private int _loadedCounter;

		// Token: 0x04000368 RID: 872
		private int _unloadedCounter;

		// Token: 0x04000369 RID: 873
		private object m_itemTemplate;

		// Token: 0x0400036A RID: 874
		private Layout m_layout;

		// Token: 0x0400036B RID: 875
		private ElementAnimator m_animator;

		// Token: 0x0400036C RID: 876
		private bool m_isItemTemplateEmpty;

		// Token: 0x0400036D RID: 877
		private static readonly Size s_zeroSize = default(Size);
	}
}
