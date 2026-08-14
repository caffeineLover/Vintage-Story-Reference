using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000081 RID: 129
	[ContentProperty("Items")]
	[TemplatePart(Name = "InnerRepeater", Type = typeof(ItemsRepeater))]
	public class RadioButtons : Control
	{
		// Token: 0x0600064F RID: 1615 RVA: 0x0001707C File Offset: 0x0001527C
		static RadioButtons()
		{
			RadioButtons.SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", 2, typeof(SelectionChangedEventHandler), typeof(RadioButtons));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButtons), new FrameworkPropertyMetadata(typeof(RadioButtons)));
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00017250 File Offset: 0x00015450
		public RadioButtons()
		{
			ObservableCollection<object> observableCollection = new ObservableCollection<object>();
			base.SetValue(RadioButtons.ItemsProperty, observableCollection);
			base.PreviewKeyDown += new KeyEventHandler(this.OnChildPreviewKeyDown);
			this.m_radioButtonsElementFactory = new RadioButtonsElementFactory();
			base.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.OnIsEnabledChanged);
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x000172B2 File Offset: 0x000154B2
		// (set) Token: 0x06000652 RID: 1618 RVA: 0x000172C4 File Offset: 0x000154C4
		public IEnumerable ItemsSource
		{
			get
			{
				return (IEnumerable)base.GetValue(RadioButtons.ItemsSourceProperty);
			}
			set
			{
				base.SetValue(RadioButtons.ItemsSourceProperty, value);
			}
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x000172D2 File Offset: 0x000154D2
		private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((RadioButtons)d).UpdateItemsSource();
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x000172DF File Offset: 0x000154DF
		public IList Items
		{
			get
			{
				return (IList)base.GetValue(RadioButtons.ItemsProperty);
			}
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x000172F1 File Offset: 0x000154F1
		private static void OnItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((RadioButtons)d).UpdateItemsSource();
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x000172FE File Offset: 0x000154FE
		// (set) Token: 0x06000657 RID: 1623 RVA: 0x0001730B File Offset: 0x0001550B
		public object ItemTemplate
		{
			get
			{
				return base.GetValue(RadioButtons.ItemTemplateProperty);
			}
			set
			{
				base.SetValue(RadioButtons.ItemTemplateProperty, value);
			}
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00017319 File Offset: 0x00015519
		private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((RadioButtons)d).UpdateItemTemplate();
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x00017326 File Offset: 0x00015526
		// (set) Token: 0x0600065A RID: 1626 RVA: 0x00017338 File Offset: 0x00015538
		public int SelectedIndex
		{
			get
			{
				return (int)base.GetValue(RadioButtons.SelectedIndexProperty);
			}
			set
			{
				base.SetValue(RadioButtons.SelectedIndexProperty, value);
			}
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0001734B File Offset: 0x0001554B
		private static void OnSelectedIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((RadioButtons)d).UpdateSelectedIndex();
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x00017358 File Offset: 0x00015558
		// (set) Token: 0x0600065D RID: 1629 RVA: 0x00017365 File Offset: 0x00015565
		public object SelectedItem
		{
			get
			{
				return base.GetValue(RadioButtons.SelectedItemProperty);
			}
			set
			{
				base.SetValue(RadioButtons.SelectedItemProperty, value);
			}
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00017373 File Offset: 0x00015573
		private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((RadioButtons)d).UpdateSelectedItem();
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x00017380 File Offset: 0x00015580
		// (set) Token: 0x06000660 RID: 1632 RVA: 0x00017392 File Offset: 0x00015592
		public int MaxColumns
		{
			get
			{
				return (int)base.GetValue(RadioButtons.MaxColumnsProperty);
			}
			set
			{
				base.SetValue(RadioButtons.MaxColumnsProperty, value);
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x000173A5 File Offset: 0x000155A5
		// (set) Token: 0x06000662 RID: 1634 RVA: 0x000173B2 File Offset: 0x000155B2
		public object Header
		{
			get
			{
				return base.GetValue(RadioButtons.HeaderProperty);
			}
			set
			{
				base.SetValue(RadioButtons.HeaderProperty, value);
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x000173C0 File Offset: 0x000155C0
		// (set) Token: 0x06000664 RID: 1636 RVA: 0x000173D2 File Offset: 0x000155D2
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(RadioButtons.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(RadioButtons.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000665 RID: 1637 RVA: 0x000173E0 File Offset: 0x000155E0
		// (remove) Token: 0x06000666 RID: 1638 RVA: 0x000173EE File Offset: 0x000155EE
		public event SelectionChangedEventHandler SelectionChanged
		{
			add
			{
				base.AddHandler(RadioButtons.SelectionChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(RadioButtons.SelectionChangedEvent, value);
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x000173FC File Offset: 0x000155FC
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this.m_repeater != null)
			{
				this.m_repeater.ElementPrepared -= new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementPreparedEventArgs>(this.OnRepeaterElementPrepared);
				this.m_repeater.ElementClearing -= new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementClearingEventArgs>(this.OnRepeaterElementClearing);
				this.m_repeater.ElementIndexChanged -= new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementIndexChangedEventArgs>(this.OnRepeaterElementIndexChanged);
				this.m_repeater.Loaded -= new RoutedEventHandler(this.OnRepeaterLoaded);
				ColumnMajorUniformToLargestGridLayout columnMajorUniformToLargestGridLayout = this.m_repeater.Layout as ColumnMajorUniformToLargestGridLayout;
				if (columnMajorUniformToLargestGridLayout != null)
				{
					columnMajorUniformToLargestGridLayout.ClearValue(ColumnMajorUniformToLargestGridLayout.MaxColumnsProperty);
				}
			}
			this.m_repeater = (base.GetTemplateChild("InnerRepeater") as ItemsRepeater);
			if (this.m_repeater != null)
			{
				this.m_repeater.ItemTemplate = this.m_radioButtonsElementFactory;
				this.m_repeater.ElementPrepared += new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementPreparedEventArgs>(this.OnRepeaterElementPrepared);
				this.m_repeater.ElementClearing += new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementClearingEventArgs>(this.OnRepeaterElementClearing);
				this.m_repeater.ElementIndexChanged += new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementIndexChangedEventArgs>(this.OnRepeaterElementIndexChanged);
				this.m_repeater.Loaded += new RoutedEventHandler(this.OnRepeaterLoaded);
				ColumnMajorUniformToLargestGridLayout columnMajorUniformToLargestGridLayout2 = this.m_repeater.Layout as ColumnMajorUniformToLargestGridLayout;
				if (columnMajorUniformToLargestGridLayout2 != null)
				{
					BindingOperations.SetBinding(columnMajorUniformToLargestGridLayout2, ColumnMajorUniformToLargestGridLayout.MaxColumnsProperty, new Binding
					{
						Path = new PropertyPath(RadioButtons.MaxColumnsProperty),
						Source = this
					});
				}
			}
			this.UpdateItemsSource();
			this.UpdateVisualStateForIsEnabledChange();
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00017568 File Offset: 0x00015768
		protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs args)
		{
			if (!this.m_currentlySettingFocus)
			{
				ItemsRepeater repeater = this.m_repeater;
				if (repeater != null)
				{
					IInputElement oldFocus = args.OldFocus;
					if (oldFocus != null)
					{
						UIElement uielement = oldFocus as UIElement;
						if (uielement != null)
						{
							DependencyObject parent = VisualTreeHelper.GetParent(uielement);
							if (repeater != parent)
							{
								UIElement uielement2 = repeater.TryGetElement(this.m_selectedIndex);
								if (uielement2 == null)
								{
									goto IL_99;
								}
								try
								{
									this.m_currentlySettingFocus = true;
									if (uielement2.Focus())
									{
										args.Handled = true;
									}
									goto IL_99;
								}
								finally
								{
									this.m_currentlySettingFocus = false;
								}
							}
							if ((args.KeyboardDevice.Modifiers & 2) != 2)
							{
								UIElement uielement3 = args.NewFocus as UIElement;
								if (uielement3 != null)
								{
									this.Select(repeater.GetElementIndex(uielement3));
								}
							}
						}
					}
				}
			}
			IL_99:
			base.OnPreviewGotKeyboardFocus(args);
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00017628 File Offset: 0x00015828
		private void OnRepeaterLoaded(object sender, RoutedEventArgs args)
		{
			if (this.m_repeater != null)
			{
				if (this.m_testHooksEnabled)
				{
					this.AttachToLayoutChanged();
				}
				this.m_blockSelecting = false;
				if (this.SelectedIndex == -1 && this.SelectedItem != null)
				{
					this.UpdateSelectedItem();
				}
				else
				{
					this.UpdateSelectedIndex();
				}
				this.OnRepeaterCollectionChanged(null, null);
			}
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0001767C File Offset: 0x0001587C
		private void OnChildPreviewKeyDown(object sender, KeyEventArgs args)
		{
			switch (args.Key)
			{
			case 23:
			{
				UIElement uielement = args.OriginalSource as UIElement;
				if (uielement != null)
				{
					if (uielement.MoveFocus(new TraversalRequest(4)))
					{
						args.Handled = true;
						return;
					}
					args.Handled = this.HandleEdgeCaseFocus(true, args.OriginalSource);
				}
				break;
			}
			case 24:
				if (this.MoveFocusPrevious())
				{
					args.Handled = true;
					return;
				}
				args.Handled = this.HandleEdgeCaseFocus(true, args.OriginalSource);
				return;
			case 25:
			{
				UIElement uielement2 = args.OriginalSource as UIElement;
				if (uielement2 != null)
				{
					if (uielement2.MoveFocus(new TraversalRequest(5)))
					{
						args.Handled = true;
						return;
					}
					args.Handled = this.HandleEdgeCaseFocus(false, args.OriginalSource);
					return;
				}
				break;
			}
			case 26:
				if (this.MoveFocusNext())
				{
					args.Handled = true;
					return;
				}
				args.Handled = this.HandleEdgeCaseFocus(false, args.OriginalSource);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00017764 File Offset: 0x00015964
		private bool HandleEdgeCaseFocus(bool first, object source)
		{
			RadioButtons.<>c__DisplayClass46_0 CS$<>8__locals1;
			CS$<>8__locals1.first = first;
			CS$<>8__locals1.repeater = this.m_repeater;
			if (CS$<>8__locals1.repeater != null)
			{
				UIElement uielement = source as UIElement;
				if (uielement != null)
				{
					int num = RadioButtons.<HandleEdgeCaseFocus>g__calculateIndex|46_0(ref CS$<>8__locals1);
					if (CS$<>8__locals1.repeater.GetElementIndex(uielement) == num)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x000177B4 File Offset: 0x000159B4
		private void OnRepeaterElementPrepared(ItemsRepeater sender, ItemsRepeaterElementPreparedEventArgs args)
		{
			UIElement element = args.Element;
			if (element != null)
			{
				ToggleButton toggleButton = element as ToggleButton;
				if (toggleButton != null)
				{
					toggleButton.Checked += new RoutedEventHandler(this.OnChildChecked);
					toggleButton.Unchecked += new RoutedEventHandler(this.OnChildUnchecked);
					bool? isChecked = toggleButton.IsChecked;
					bool flag = true;
					if (isChecked.GetValueOrDefault() == flag & isChecked != null)
					{
						this.m_blockSelecting = false;
						this.Select(args.Index);
					}
				}
				ItemsRepeater repeater = this.m_repeater;
				if (repeater != null)
				{
					ItemsSourceView itemsSourceView = repeater.ItemsSourceView;
					if (itemsSourceView != null)
					{
						element.SetValue(AutomationProperties.PositionInSetProperty, args.Index + 1);
						element.SetValue(AutomationProperties.SizeOfSetProperty, itemsSourceView.Count);
					}
				}
			}
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00017874 File Offset: 0x00015A74
		private void OnRepeaterElementClearing(ItemsRepeater sender, ItemsRepeaterElementClearingEventArgs args)
		{
			UIElement element = args.Element;
			if (element != null)
			{
				ToggleButton toggleButton = element as ToggleButton;
				if (toggleButton != null)
				{
					toggleButton.Checked -= new RoutedEventHandler(this.OnChildChecked);
					toggleButton.Unchecked -= new RoutedEventHandler(this.OnChildUnchecked);
				}
				ToggleButton toggleButton2 = element as ToggleButton;
				if (toggleButton2 != null)
				{
					bool? isChecked = toggleButton2.IsChecked;
					bool flag = true;
					if (isChecked.GetValueOrDefault() == flag & isChecked != null)
					{
						this.Select(-1);
					}
				}
			}
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x000178EC File Offset: 0x00015AEC
		private void OnRepeaterElementIndexChanged(ItemsRepeater sender, ItemsRepeaterElementIndexChangedEventArgs args)
		{
			UIElement element = args.Element;
			if (element != null)
			{
				element.SetValue(AutomationProperties.PositionInSetProperty, args.NewIndex + 1);
				ToggleButton toggleButton = element as ToggleButton;
				if (toggleButton != null)
				{
					bool? isChecked = toggleButton.IsChecked;
					bool flag = true;
					if (isChecked.GetValueOrDefault() == flag & isChecked != null)
					{
						this.Select(args.NewIndex);
					}
				}
			}
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00017950 File Offset: 0x00015B50
		private void OnRepeaterCollectionChanged(object sender, object args)
		{
			ItemsRepeater repeater = this.m_repeater;
			if (repeater != null)
			{
				ItemsSourceView itemsSourceView = repeater.ItemsSourceView;
				if (itemsSourceView != null)
				{
					int count = itemsSourceView.Count;
					for (int i = 0; i < count; i++)
					{
						UIElement uielement = repeater.TryGetElement(i);
						if (uielement != null)
						{
							uielement.SetValue(AutomationProperties.SizeOfSetProperty, count);
						}
					}
				}
			}
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x000179A4 File Offset: 0x00015BA4
		private void Select(int index)
		{
			if (!this.m_blockSelecting && !this.m_currentlySelecting && this.m_selectedIndex != index)
			{
				try
				{
					this.m_currentlySelecting = true;
					int selectedIndex = this.m_selectedIndex;
					this.m_selectedIndex = index;
					object dataAtIndex = this.GetDataAtIndex(this.m_selectedIndex, true);
					object dataAtIndex2 = this.GetDataAtIndex(selectedIndex, false);
					base.SetCurrentValue(RadioButtons.SelectedIndexProperty, this.m_selectedIndex);
					base.SetCurrentValue(RadioButtons.SelectedItemProperty, dataAtIndex);
					base.RaiseEvent(new SelectionChangedEventArgs(RadioButtons.SelectionChangedEvent, new object[]
					{
						dataAtIndex2
					}, new object[]
					{
						dataAtIndex
					}));
				}
				finally
				{
					this.m_currentlySelecting = false;
				}
			}
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00017A5C File Offset: 0x00015C5C
		private object GetDataAtIndex(int index, bool containerIsChecked)
		{
			ItemsRepeater repeater = this.m_repeater;
			if (repeater != null)
			{
				UIElement uielement = repeater.TryGetElement(index);
				if (uielement != null)
				{
					ToggleButton toggleButton = uielement as ToggleButton;
					if (toggleButton != null)
					{
						toggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, containerIsChecked);
					}
				}
				if (index >= 0)
				{
					ItemsSourceView itemsSourceView = repeater.ItemsSourceView;
					if (itemsSourceView != null && index < itemsSourceView.Count)
					{
						return itemsSourceView.GetAt(index);
					}
				}
			}
			return null;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00017ABC File Offset: 0x00015CBC
		private void OnChildChecked(object sender, RoutedEventArgs args)
		{
			if (!this.m_currentlySelecting)
			{
				ItemsRepeater repeater = this.m_repeater;
				if (repeater != null)
				{
					UIElement uielement = sender as UIElement;
					if (uielement != null)
					{
						this.Select(repeater.GetElementIndex(uielement));
					}
				}
			}
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00017AF4 File Offset: 0x00015CF4
		private void OnChildUnchecked(object sender, RoutedEventArgs args)
		{
			if (!this.m_currentlySelecting)
			{
				ItemsRepeater repeater = this.m_repeater;
				if (repeater != null)
				{
					UIElement uielement = sender as UIElement;
					if (uielement != null && this.m_selectedIndex == repeater.GetElementIndex(uielement))
					{
						this.Select(-1);
					}
				}
			}
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00017B33 File Offset: 0x00015D33
		private bool MoveFocusNext()
		{
			return this.MoveFocus(1);
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00017B3C File Offset: 0x00015D3C
		private bool MoveFocusPrevious()
		{
			return this.MoveFocus(-1);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00017B48 File Offset: 0x00015D48
		private bool MoveFocus(int indexIncrement)
		{
			ItemsRepeater repeater = this.m_repeater;
			if (repeater != null)
			{
				UIElement uielement = Keyboard.FocusedElement as UIElement;
				if (uielement != null)
				{
					int num = repeater.GetElementIndex(uielement);
					if (num >= 0)
					{
						num += indexIncrement;
						int count = repeater.ItemsSourceView.Count;
						while (num >= 0 && num < count)
						{
							UIElement uielement2 = repeater.TryGetElement(num);
							if (uielement2 != null)
							{
								Control control = uielement2 as Control;
								if (control != null && control.Focus())
								{
									return true;
								}
							}
							num += indexIncrement;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00017BBD File Offset: 0x00015DBD
		private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.UpdateVisualStateForIsEnabledChange();
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00017BC8 File Offset: 0x00015DC8
		public UIElement ContainerFromIndex(int index)
		{
			ItemsRepeater repeater = this.m_repeater;
			if (repeater != null)
			{
				return repeater.TryGetElement(index);
			}
			return null;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00017BE8 File Offset: 0x00015DE8
		private void UpdateItemsSource()
		{
			this.Select(-1);
			ItemsRepeater repeater = this.m_repeater;
			if (repeater != null)
			{
				ItemsSourceView itemsSourceView = repeater.ItemsSourceView;
				if (itemsSourceView != null)
				{
					itemsSourceView.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnRepeaterCollectionChanged);
				}
				repeater.ItemsSource = this.GetItemsSource();
				ItemsSourceView itemsSourceView2 = repeater.ItemsSourceView;
				if (itemsSourceView2 != null)
				{
					itemsSourceView2.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnRepeaterCollectionChanged);
				}
			}
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00017C4C File Offset: 0x00015E4C
		private object GetItemsSource()
		{
			IEnumerable itemsSource = this.ItemsSource;
			if (itemsSource != null)
			{
				return itemsSource;
			}
			return this.Items;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00017C6B File Offset: 0x00015E6B
		private void UpdateSelectedIndex()
		{
			if (!this.m_currentlySelecting)
			{
				this.Select(this.SelectedIndex);
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00017C84 File Offset: 0x00015E84
		private void UpdateSelectedItem()
		{
			if (!this.m_currentlySelecting)
			{
				ItemsRepeater repeater = this.m_repeater;
				if (repeater != null)
				{
					ItemsSourceView itemsSourceView = repeater.ItemsSourceView;
					if (itemsSourceView != null)
					{
						this.Select(itemsSourceView.IndexOf(this.SelectedItem));
					}
				}
			}
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00017CBF File Offset: 0x00015EBF
		private void UpdateItemTemplate()
		{
			this.m_radioButtonsElementFactory.UserElementFactory(this.ItemTemplate);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00017CD2 File Offset: 0x00015ED2
		private void UpdateVisualStateForIsEnabledChange()
		{
			VisualStateManager.GoToState(this, base.IsEnabled ? "Normal" : "Disabled", false);
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00017CF0 File Offset: 0x00015EF0
		internal void SetTestHooksEnabled(bool enabled)
		{
			if (this.m_testHooksEnabled != enabled)
			{
				this.m_testHooksEnabled = enabled;
				if (enabled)
				{
					this.AttachToLayoutChanged();
					return;
				}
				this.DetatchFromLayoutChanged();
			}
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00017D12 File Offset: 0x00015F12
		private void OnLayoutChanged(ColumnMajorUniformToLargestGridLayout sender, object args)
		{
			RadioButtonsTestHooks.NotifyLayoutChanged(this);
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00017D1C File Offset: 0x00015F1C
		internal int GetRows()
		{
			ColumnMajorUniformToLargestGridLayout layout = this.GetLayout();
			if (layout != null)
			{
				return layout.GetRows();
			}
			return -1;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00017D3C File Offset: 0x00015F3C
		internal int GetColumns()
		{
			ColumnMajorUniformToLargestGridLayout layout = this.GetLayout();
			if (layout != null)
			{
				return layout.GetColumns();
			}
			return -1;
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00017D5C File Offset: 0x00015F5C
		internal int GetLargerColumns()
		{
			ColumnMajorUniformToLargestGridLayout layout = this.GetLayout();
			if (layout != null)
			{
				return layout.GetLargerColumns();
			}
			return -1;
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00017D7C File Offset: 0x00015F7C
		private void AttachToLayoutChanged()
		{
			ColumnMajorUniformToLargestGridLayout layout = this.GetLayout();
			if (layout != null)
			{
				layout.SetTestHooksEnabled(true);
				layout.LayoutChanged += new TypedEventHandler<ColumnMajorUniformToLargestGridLayout, object>(this.OnLayoutChanged);
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00017DAC File Offset: 0x00015FAC
		private void DetatchFromLayoutChanged()
		{
			ColumnMajorUniformToLargestGridLayout layout = this.GetLayout();
			if (layout != null)
			{
				layout.SetTestHooksEnabled(false);
				layout.LayoutChanged -= new TypedEventHandler<ColumnMajorUniformToLargestGridLayout, object>(this.OnLayoutChanged);
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00017DDC File Offset: 0x00015FDC
		private ColumnMajorUniformToLargestGridLayout GetLayout()
		{
			if (this.m_repeater != null)
			{
				ColumnMajorUniformToLargestGridLayout columnMajorUniformToLargestGridLayout = this.m_repeater.Layout as ColumnMajorUniformToLargestGridLayout;
				if (columnMajorUniformToLargestGridLayout != null)
				{
					return columnMajorUniformToLargestGridLayout;
				}
			}
			return null;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00017E08 File Offset: 0x00016008
		[CompilerGenerated]
		internal static int <HandleEdgeCaseFocus>g__calculateIndex|46_0(ref RadioButtons.<>c__DisplayClass46_0 A_0)
		{
			if (A_0.first)
			{
				return 0;
			}
			ItemsSourceView itemsSourceView = A_0.repeater.ItemsSourceView;
			if (itemsSourceView != null)
			{
				return itemsSourceView.Count - 1;
			}
			return -1;
		}

		// Token: 0x040002EF RID: 751
		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(RadioButtons), new FrameworkPropertyMetadata(new PropertyChangedCallback(RadioButtons.OnItemsSourcePropertyChanged)));

		// Token: 0x040002F0 RID: 752
		public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(IList), typeof(RadioButtons), new FrameworkPropertyMetadata(new PropertyChangedCallback(RadioButtons.OnItemsPropertyChanged)));

		// Token: 0x040002F1 RID: 753
		public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(object), typeof(RadioButtons), new FrameworkPropertyMetadata(new PropertyChangedCallback(RadioButtons.OnItemTemplateChanged)));

		// Token: 0x040002F2 RID: 754
		public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(RadioButtons), new FrameworkPropertyMetadata(-1, 1280, new PropertyChangedCallback(RadioButtons.OnSelectedIndexPropertyChanged)));

		// Token: 0x040002F3 RID: 755
		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(RadioButtons), new FrameworkPropertyMetadata(null, 256, new PropertyChangedCallback(RadioButtons.OnSelectedItemPropertyChanged)));

		// Token: 0x040002F4 RID: 756
		public static readonly DependencyProperty MaxColumnsProperty = DependencyProperty.Register("MaxColumns", typeof(int), typeof(RadioButtons), new FrameworkPropertyMetadata(1));

		// Token: 0x040002F5 RID: 757
		public static readonly DependencyProperty HeaderProperty = ControlHelper.HeaderProperty.AddOwner(typeof(RadioButtons));

		// Token: 0x040002F6 RID: 758
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(RadioButtons), null);

		// Token: 0x040002F8 RID: 760
		private int m_selectedIndex = -1;

		// Token: 0x040002F9 RID: 761
		private bool m_currentlySelecting;

		// Token: 0x040002FA RID: 762
		private bool m_blockSelecting = true;

		// Token: 0x040002FB RID: 763
		private bool m_currentlySettingFocus;

		// Token: 0x040002FC RID: 764
		private ItemsRepeater m_repeater;

		// Token: 0x040002FD RID: 765
		private RadioButtonsElementFactory m_radioButtonsElementFactory;

		// Token: 0x040002FE RID: 766
		private bool m_testHooksEnabled;

		// Token: 0x040002FF RID: 767
		private const string s_repeaterName = "InnerRepeater";
	}
}
