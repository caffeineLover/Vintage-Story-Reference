using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace VintageStoryModManager.Views
{
	// Token: 0x02000019 RID: 25
	[NullableContext(1)]
	[Nullable(0)]
	public class MultiSelectDropdown : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060003B5 RID: 949 RVA: 0x0001DAA0 File Offset: 0x0001BCA0
		// (remove) Token: 0x060003B6 RID: 950 RVA: 0x0001DAD8 File Offset: 0x0001BCD8
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event EventHandler SelectionChanged;

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x0001DB0D File Offset: 0x0001BD0D
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x0001DB1F File Offset: 0x0001BD1F
		public IEnumerable ItemsSource
		{
			get
			{
				return (IEnumerable)base.GetValue(MultiSelectDropdown.ItemsSourceProperty);
			}
			set
			{
				base.SetValue(MultiSelectDropdown.ItemsSourceProperty, value);
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x0001DB2D File Offset: 0x0001BD2D
		// (set) Token: 0x060003BA RID: 954 RVA: 0x0001DB3F File Offset: 0x0001BD3F
		public IList SelectedItems
		{
			get
			{
				return (IList)base.GetValue(MultiSelectDropdown.SelectedItemsProperty);
			}
			set
			{
				base.SetValue(MultiSelectDropdown.SelectedItemsProperty, value);
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060003BB RID: 955 RVA: 0x0001DB4D File Offset: 0x0001BD4D
		// (set) Token: 0x060003BC RID: 956 RVA: 0x0001DB5F File Offset: 0x0001BD5F
		public string PlaceholderText
		{
			get
			{
				return (string)base.GetValue(MultiSelectDropdown.PlaceholderTextProperty);
			}
			set
			{
				base.SetValue(MultiSelectDropdown.PlaceholderTextProperty, value);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060003BD RID: 957 RVA: 0x0001DB6D File Offset: 0x0001BD6D
		// (set) Token: 0x060003BE RID: 958 RVA: 0x0001DB7F File Offset: 0x0001BD7F
		public string DisplayMemberPath
		{
			get
			{
				return (string)base.GetValue(MultiSelectDropdown.DisplayMemberPathProperty);
			}
			set
			{
				base.SetValue(MultiSelectDropdown.DisplayMemberPathProperty, value);
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060003BF RID: 959 RVA: 0x0001DB8D File Offset: 0x0001BD8D
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x0001DB9F File Offset: 0x0001BD9F
		public string DisplayText
		{
			get
			{
				return (string)base.GetValue(MultiSelectDropdown.DisplayTextProperty);
			}
			private set
			{
				base.SetValue(MultiSelectDropdown.DisplayTextProperty, value);
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x0001DBAD File Offset: 0x0001BDAD
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x0001DBBF File Offset: 0x0001BDBF
		public Brush DisplayTextBrush
		{
			get
			{
				return (Brush)base.GetValue(MultiSelectDropdown.DisplayTextBrushProperty);
			}
			private set
			{
				base.SetValue(MultiSelectDropdown.DisplayTextBrushProperty, value);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x0001DBCD File Offset: 0x0001BDCD
		// (set) Token: 0x060003C4 RID: 964 RVA: 0x0001DBDF File Offset: 0x0001BDDF
		public double MaxDropDownWidth
		{
			get
			{
				return (double)base.GetValue(MultiSelectDropdown.MaxDropDownWidthProperty);
			}
			set
			{
				base.SetValue(MultiSelectDropdown.MaxDropDownWidthProperty, value);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x0001DBF2 File Offset: 0x0001BDF2
		// (set) Token: 0x060003C6 RID: 966 RVA: 0x0001DC04 File Offset: 0x0001BE04
		public bool ShowClearSelectionOption
		{
			get
			{
				return (bool)base.GetValue(MultiSelectDropdown.ShowClearSelectionOptionProperty);
			}
			set
			{
				base.SetValue(MultiSelectDropdown.ShowClearSelectionOptionProperty, value);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x0001DC17 File Offset: 0x0001BE17
		// (set) Token: 0x060003C8 RID: 968 RVA: 0x0001DC29 File Offset: 0x0001BE29
		public string ClearSelectionText
		{
			get
			{
				return (string)base.GetValue(MultiSelectDropdown.ClearSelectionTextProperty);
			}
			set
			{
				base.SetValue(MultiSelectDropdown.ClearSelectionTextProperty, value);
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x0001DC38 File Offset: 0x0001BE38
		private Brush TextPrimaryBrush
		{
			get
			{
				Brush result;
				if ((result = this._textPrimaryBrush) == null)
				{
					result = (this._textPrimaryBrush = ((base.TryFindResource("TextPrimaryBrush") as Brush) ?? MultiSelectDropdown.FallbackTextPrimaryBrush));
				}
				return result;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060003CA RID: 970 RVA: 0x0001DC74 File Offset: 0x0001BE74
		private Brush TextSecondaryBrush
		{
			get
			{
				Brush result;
				if ((result = this._textSecondaryBrush) == null)
				{
					result = (this._textSecondaryBrush = ((base.TryFindResource("TextSecondaryBrush") as Brush) ?? MultiSelectDropdown.FallbackTextSecondaryBrush));
				}
				return result;
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0001DCB0 File Offset: 0x0001BEB0
		public MultiSelectDropdown()
		{
			this.InitializeComponent();
			this.ItemsList.ItemsSource = this._selectableItems;
			base.Loaded += delegate(object _, RoutedEventArgs _)
			{
				this.UpdateDisplayText();
			};
			base.Loaded += delegate(object _, RoutedEventArgs _)
			{
				if (!this._isEventSubscribed)
				{
					Window window = Window.GetWindow(this);
					if (window != null)
					{
						window.PreviewMouseDown += new MouseButtonEventHandler(this.Window_PreviewMouseDown);
						this._isEventSubscribed = true;
					}
				}
			};
			base.Unloaded += delegate(object _, RoutedEventArgs _)
			{
				if (this._isEventSubscribed)
				{
					Window window = Window.GetWindow(this);
					if (window != null)
					{
						window.PreviewMouseDown -= new MouseButtonEventHandler(this.Window_PreviewMouseDown);
						this._isEventSubscribed = false;
					}
				}
			};
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0001DD1C File Offset: 0x0001BF1C
		private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (!this.DropdownPopup.IsOpen)
			{
				return;
			}
			UIElement child = this.DropdownPopup.Child;
			ToggleButton dropdownToggle = this.DropdownToggle;
			if (child != null && dropdownToggle != null)
			{
				DependencyObject child2 = e.OriginalSource as DependencyObject;
				bool flag = this.IsDescendantOf(child2, child);
				if (this.IsDescendantOf(child2, dropdownToggle))
				{
					this._suppressNextToggleClick = true;
					this.DropdownToggle.IsChecked = new bool?(false);
					e.Handled = true;
					return;
				}
				if (!flag)
				{
					this.DropdownToggle.IsChecked = new bool?(false);
				}
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0001DDA4 File Offset: 0x0001BFA4
		private bool IsDescendantOf([Nullable(2)] DependencyObject child, DependencyObject parent)
		{
			if (child == null || parent == null)
			{
				return false;
			}
			for (DependencyObject dependencyObject = child; dependencyObject != null; dependencyObject = VisualTreeHelper.GetParent(dependencyObject))
			{
				if (dependencyObject == parent)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0001DDD0 File Offset: 0x0001BFD0
		private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MultiSelectDropdown multiSelectDropdown = d as MultiSelectDropdown;
			if (multiSelectDropdown != null)
			{
				INotifyCollectionChanged notifyCollectionChanged = e.OldValue as INotifyCollectionChanged;
				if (notifyCollectionChanged != null)
				{
					notifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(multiSelectDropdown.OnItemsSourceCollectionChanged);
				}
				INotifyCollectionChanged notifyCollectionChanged2 = e.NewValue as INotifyCollectionChanged;
				if (notifyCollectionChanged2 != null)
				{
					notifyCollectionChanged2.CollectionChanged += new NotifyCollectionChangedEventHandler(multiSelectDropdown.OnItemsSourceCollectionChanged);
				}
				multiSelectDropdown.UpdateSelectableItems();
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0001DE34 File Offset: 0x0001C034
		private void OnItemsSourceCollectionChanged([Nullable(2)] object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case 0:
			{
				if (e.NewItems == null)
				{
					return;
				}
				int num = (e.NewStartingIndex >= 0) ? e.NewStartingIndex : this._selectableItems.Count;
				using (IEnumerator enumerator = e.NewItems.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						string displayText = this.GetDisplayText(obj);
						IList selectedItems = this.SelectedItems;
						bool isSelected = selectedItems != null && selectedItems.Contains(obj);
						SelectableItem selectableItem = new SelectableItem
						{
							Item = obj,
							DisplayText = displayText,
							IsSelected = isSelected
						};
						if (num <= this._selectableItems.Count)
						{
							this._selectableItems.Insert(num, selectableItem);
							num++;
						}
						else
						{
							this._selectableItems.Add(selectableItem);
						}
					}
					return;
				}
				break;
			}
			case 1:
				break;
			case 2:
				if (e.NewItems != null && e.NewStartingIndex >= 0 && e.NewStartingIndex < this._selectableItems.Count)
				{
					for (int i = 0; i < e.NewItems.Count; i++)
					{
						int num2 = e.NewStartingIndex + i;
						if (num2 < this._selectableItems.Count)
						{
							object obj2 = e.NewItems[i];
							string displayText2 = this.GetDisplayText(obj2);
							IList selectedItems2 = this.SelectedItems;
							bool isSelected2 = selectedItems2 != null && selectedItems2.Contains(obj2);
							this._selectableItems[num2] = new SelectableItem
							{
								Item = obj2,
								DisplayText = displayText2,
								IsSelected = isSelected2
							};
						}
					}
					return;
				}
				return;
			case 3:
			{
				if (e.OldStartingIndex < 0 || e.NewStartingIndex < 0 || e.OldStartingIndex >= this._selectableItems.Count)
				{
					return;
				}
				SelectableItem selectableItem2 = this._selectableItems[e.OldStartingIndex];
				this._selectableItems.RemoveAt(e.OldStartingIndex);
				if (e.NewStartingIndex <= this._selectableItems.Count)
				{
					this._selectableItems.Insert(e.NewStartingIndex, selectableItem2);
					return;
				}
				this._selectableItems.Add(selectableItem2);
				return;
			}
			case 4:
				this.UpdateSelectableItems();
				return;
			default:
				return;
			}
			if (e.OldItems != null && e.OldStartingIndex >= 0)
			{
				for (int j = 0; j < e.OldItems.Count; j++)
				{
					if (e.OldStartingIndex < this._selectableItems.Count)
					{
						this._selectableItems.RemoveAt(e.OldStartingIndex);
					}
				}
				return;
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0001E0D4 File Offset: 0x0001C2D4
		private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MultiSelectDropdown multiSelectDropdown = d as MultiSelectDropdown;
			if (multiSelectDropdown != null)
			{
				INotifyCollectionChanged notifyCollectionChanged = e.OldValue as INotifyCollectionChanged;
				if (notifyCollectionChanged != null)
				{
					notifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(multiSelectDropdown.OnSelectedItemsCollectionChanged);
				}
				INotifyCollectionChanged notifyCollectionChanged2 = e.NewValue as INotifyCollectionChanged;
				if (notifyCollectionChanged2 != null)
				{
					notifyCollectionChanged2.CollectionChanged += new NotifyCollectionChangedEventHandler(multiSelectDropdown.OnSelectedItemsCollectionChanged);
				}
				multiSelectDropdown.UpdateDisplayText();
				multiSelectDropdown.UpdateSelectableItems();
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0001E13C File Offset: 0x0001C33C
		private void OnSelectedItemsCollectionChanged([Nullable(2)] object sender, NotifyCollectionChangedEventArgs e)
		{
			this.UpdateDisplayText();
			switch (e.Action)
			{
			case 0:
				if (e.NewItems == null)
				{
					return;
				}
				using (IEnumerator enumerator = e.NewItems.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object item = enumerator.Current;
						SelectableItem selectableItem = Enumerable.FirstOrDefault<SelectableItem>(this._selectableItems, (SelectableItem si) => si.Item == item);
						if (selectableItem != null)
						{
							selectableItem.IsSelected = true;
						}
					}
					return;
				}
				break;
			case 1:
				break;
			case 2:
				goto IL_15C;
			case 3:
				return;
			case 4:
				goto IL_10A;
			default:
				return;
			}
			if (e.OldItems == null)
			{
				return;
			}
			using (IEnumerator enumerator = e.OldItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object item = enumerator.Current;
					SelectableItem selectableItem2 = Enumerable.FirstOrDefault<SelectableItem>(this._selectableItems, (SelectableItem si) => si.Item == item);
					if (selectableItem2 != null)
					{
						selectableItem2.IsSelected = false;
					}
				}
				return;
			}
			IL_10A:
			using (IEnumerator<SelectableItem> enumerator2 = this._selectableItems.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					SelectableItem selectableItem3 = enumerator2.Current;
					SelectableItem selectableItem4 = selectableItem3;
					IList selectedItems = this.SelectedItems;
					selectableItem4.IsSelected = (selectedItems != null && selectedItems.Contains(selectableItem3.Item));
				}
				return;
			}
			IL_15C:
			if (e.OldItems != null)
			{
				using (IEnumerator enumerator = e.OldItems.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object item = enumerator.Current;
						SelectableItem selectableItem5 = Enumerable.FirstOrDefault<SelectableItem>(this._selectableItems, (SelectableItem si) => si.Item == item);
						if (selectableItem5 != null)
						{
							selectableItem5.IsSelected = false;
						}
					}
				}
			}
			if (e.NewItems != null)
			{
				using (IEnumerator enumerator = e.NewItems.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object item = enumerator.Current;
						SelectableItem selectableItem6 = Enumerable.FirstOrDefault<SelectableItem>(this._selectableItems, (SelectableItem si) => si.Item == item);
						if (selectableItem6 != null)
						{
							selectableItem6.IsSelected = true;
						}
					}
				}
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0001E3C4 File Offset: 0x0001C5C4
		private static void OnPlaceholderTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MultiSelectDropdown multiSelectDropdown = d as MultiSelectDropdown;
			if (multiSelectDropdown != null)
			{
				multiSelectDropdown.UpdateDisplayText();
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0001E3E4 File Offset: 0x0001C5E4
		private static void OnDropDownSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MultiSelectDropdown multiSelectDropdown = d as MultiSelectDropdown;
			if (multiSelectDropdown != null)
			{
				multiSelectDropdown.UpdateDropDownSize();
			}
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0001E404 File Offset: 0x0001C604
		private static void OnClearSelectionStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MultiSelectDropdown multiSelectDropdown = d as MultiSelectDropdown;
			if (multiSelectDropdown != null)
			{
				multiSelectDropdown.UpdateClearSelectionOption();
			}
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0001E424 File Offset: 0x0001C624
		private void UpdateDisplayText()
		{
			this.UpdateClearSelectionOption();
			if (this.SelectedItems == null || this.SelectedItems.Count == 0)
			{
				this.DisplayText = this.PlaceholderText;
				this.DisplayTextBrush = this.TextSecondaryBrush;
				return;
			}
			if (this.SelectedItems.Count == 1)
			{
				this.DisplayText = this.GetDisplayText(this.SelectedItems[0]);
				this.DisplayTextBrush = this.TextPrimaryBrush;
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(9, 1);
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.SelectedItems.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" selected");
			this.DisplayText = defaultInterpolatedStringHandler.ToStringAndClear();
			this.DisplayTextBrush = this.TextPrimaryBrush;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0001E4DC File Offset: 0x0001C6DC
		private void UpdateSelectableItems()
		{
			this._selectableItems.Clear();
			if (this.ItemsSource == null)
			{
				return;
			}
			foreach (object obj in this.ItemsSource)
			{
				string displayText = this.GetDisplayText(obj);
				IList selectedItems = this.SelectedItems;
				bool isSelected = selectedItems != null && selectedItems.Contains(obj);
				this._selectableItems.Add(new SelectableItem
				{
					Item = obj,
					DisplayText = displayText,
					IsSelected = isSelected
				});
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0001E584 File Offset: 0x0001C784
		private string GetDisplayText(object item)
		{
			if (string.IsNullOrEmpty(this.DisplayMemberPath))
			{
				return item.ToString() ?? string.Empty;
			}
			PropertyInfo property = item.GetType().GetProperty(this.DisplayMemberPath);
			string text;
			if (property == null)
			{
				text = null;
			}
			else
			{
				object value = property.GetValue(item);
				text = ((value != null) ? value.ToString() : null);
			}
			string result;
			if ((result = text) == null)
			{
				result = (item.ToString() ?? string.Empty);
			}
			return result;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0001E5EC File Offset: 0x0001C7EC
		private void DropdownToggle_Click(object sender, RoutedEventArgs e)
		{
			if (this._suppressNextToggleClick)
			{
				this._suppressNextToggleClick = false;
				e.Handled = true;
				return;
			}
			if (this.DropdownToggle.IsChecked.GetValueOrDefault())
			{
				foreach (SelectableItem selectableItem in this._selectableItems)
				{
					SelectableItem selectableItem2 = selectableItem;
					IList selectedItems = this.SelectedItems;
					selectableItem2.IsSelected = (selectedItems != null && selectedItems.Contains(selectableItem.Item));
				}
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0001E67C File Offset: 0x0001C87C
		private void Item_Click(object sender, MouseButtonEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement != null)
			{
				SelectableItem selectableItem = frameworkElement.DataContext as SelectableItem;
				if (selectableItem != null)
				{
					selectableItem.IsSelected = !selectableItem.IsSelected;
					if (this.SelectedItems != null)
					{
						if (selectableItem.IsSelected)
						{
							if (!this.SelectedItems.Contains(selectableItem.Item))
							{
								this.SelectedItems.Add(selectableItem.Item);
							}
						}
						else
						{
							this.SelectedItems.Remove(selectableItem.Item);
						}
					}
					this.UpdateDisplayText();
					EventHandler selectionChanged = this.SelectionChanged;
					if (selectionChanged != null)
					{
						selectionChanged.Invoke(this, EventArgs.Empty);
					}
					e.Handled = true;
				}
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0001E720 File Offset: 0x0001C920
		private void UpdateDropDownSize()
		{
			if (this.DropdownBorder == null)
			{
				return;
			}
			this.DropdownBorder.MaxWidth = this.MaxDropDownWidth;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0001E73C File Offset: 0x0001C93C
		private void UpdateClearSelectionOption()
		{
			if (this.ClearSelectionOption == null || this.ClearSelectionTextBlock == null)
			{
				return;
			}
			UIElement clearSelectionOption = this.ClearSelectionOption;
			Visibility visibility;
			if (this.ShowClearSelectionOption)
			{
				IList selectedItems = this.SelectedItems;
				if (selectedItems != null && selectedItems.Count > 0)
				{
					visibility = 0;
					goto IL_3A;
				}
			}
			visibility = 2;
			IL_3A:
			clearSelectionOption.Visibility = visibility;
			this.ClearSelectionTextBlock.Text = this.ClearSelectionText;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0001E79C File Offset: 0x0001C99C
		private void ClearSelection_Click(object sender, MouseButtonEventArgs e)
		{
			if (this.SelectedItems == null || this.SelectedItems.Count == 0)
			{
				e.Handled = true;
				return;
			}
			this.SelectedItems.Clear();
			foreach (SelectableItem selectableItem in this._selectableItems)
			{
				selectableItem.IsSelected = false;
			}
			this.UpdateDisplayText();
			EventHandler selectionChanged = this.SelectionChanged;
			if (selectionChanged != null)
			{
				selectionChanged.Invoke(this, EventArgs.Empty);
			}
			e.Handled = true;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0001E834 File Offset: 0x0001CA34
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/multiselectdropdown.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0001E864 File Offset: 0x0001CA64
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.DropdownToggle = (ToggleButton)target;
				this.DropdownToggle.Click += new RoutedEventHandler(this.DropdownToggle_Click);
				return;
			case 2:
				this.DropdownPopup = (Popup)target;
				return;
			case 3:
				this.DropdownBorder = (Border)target;
				return;
			case 4:
				this.ClearSelectionOption = (Border)target;
				this.ClearSelectionOption.MouseLeftButtonDown += new MouseButtonEventHandler(this.ClearSelection_Click);
				return;
			case 5:
				this.ClearSelectionTextBlock = (TextBlock)target;
				return;
			case 6:
				this.ItemsList = (ItemsControl)target;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0001E916 File Offset: 0x0001CB16
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 7)
			{
				((Border)target).MouseLeftButtonDown += new MouseButtonEventHandler(this.Item_Click);
			}
		}

		// Token: 0x040001E2 RID: 482
		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(MultiSelectDropdown), new PropertyMetadata(null, new PropertyChangedCallback(MultiSelectDropdown.OnItemsSourceChanged)));

		// Token: 0x040001E3 RID: 483
		public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(IList), typeof(MultiSelectDropdown), new FrameworkPropertyMetadata(null, 256, new PropertyChangedCallback(MultiSelectDropdown.OnSelectedItemsChanged)));

		// Token: 0x040001E4 RID: 484
		public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register("PlaceholderText", typeof(string), typeof(MultiSelectDropdown), new PropertyMetadata("Select items...", new PropertyChangedCallback(MultiSelectDropdown.OnPlaceholderTextChanged)));

		// Token: 0x040001E5 RID: 485
		public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(MultiSelectDropdown), new PropertyMetadata("Name"));

		// Token: 0x040001E6 RID: 486
		public static readonly DependencyProperty DisplayTextProperty = DependencyProperty.Register("DisplayText", typeof(string), typeof(MultiSelectDropdown), new PropertyMetadata("Select items..."));

		// Token: 0x040001E7 RID: 487
		public static readonly DependencyProperty DisplayTextBrushProperty = DependencyProperty.Register("DisplayTextBrush", typeof(Brush), typeof(MultiSelectDropdown), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(161, 161, 170))));

		// Token: 0x040001E8 RID: 488
		public static readonly DependencyProperty MaxDropDownWidthProperty = DependencyProperty.Register("MaxDropDownWidth", typeof(double), typeof(MultiSelectDropdown), new PropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(MultiSelectDropdown.OnDropDownSizeChanged)));

		// Token: 0x040001E9 RID: 489
		public static readonly DependencyProperty ShowClearSelectionOptionProperty = DependencyProperty.Register("ShowClearSelectionOption", typeof(bool), typeof(MultiSelectDropdown), new PropertyMetadata(false, new PropertyChangedCallback(MultiSelectDropdown.OnClearSelectionStateChanged)));

		// Token: 0x040001EA RID: 490
		public static readonly DependencyProperty ClearSelectionTextProperty = DependencyProperty.Register("ClearSelectionText", typeof(string), typeof(MultiSelectDropdown), new PropertyMetadata("Clear", new PropertyChangedCallback(MultiSelectDropdown.OnClearSelectionStateChanged)));

		// Token: 0x040001EB RID: 491
		private ObservableCollection<SelectableItem> _selectableItems = new ObservableCollection<SelectableItem>();

		// Token: 0x040001EC RID: 492
		[Nullable(2)]
		private Brush _textPrimaryBrush;

		// Token: 0x040001ED RID: 493
		[Nullable(2)]
		private Brush _textSecondaryBrush;

		// Token: 0x040001EE RID: 494
		private bool _isEventSubscribed;

		// Token: 0x040001EF RID: 495
		private bool _suppressNextToggleClick;

		// Token: 0x040001F0 RID: 496
		private static readonly Brush FallbackTextPrimaryBrush = new SolidColorBrush(Color.FromRgb(250, 250, 250));

		// Token: 0x040001F1 RID: 497
		private static readonly Brush FallbackTextSecondaryBrush = new SolidColorBrush(Color.FromRgb(161, 161, 170));

		// Token: 0x040001F2 RID: 498
		[Nullable(0)]
		internal ToggleButton DropdownToggle;

		// Token: 0x040001F3 RID: 499
		[Nullable(0)]
		internal Popup DropdownPopup;

		// Token: 0x040001F4 RID: 500
		[Nullable(0)]
		internal Border DropdownBorder;

		// Token: 0x040001F5 RID: 501
		[Nullable(0)]
		internal Border ClearSelectionOption;

		// Token: 0x040001F6 RID: 502
		[Nullable(0)]
		internal TextBlock ClearSelectionTextBlock;

		// Token: 0x040001F7 RID: 503
		[Nullable(0)]
		internal ItemsControl ItemsList;

		// Token: 0x040001F8 RID: 504
		private bool _contentLoaded;
	}
}
