using System;
using System.Windows;
using System.Windows.Controls;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x0200004E RID: 78
	public class ListViewBase : ListBox
	{
		// Token: 0x060002CE RID: 718 RVA: 0x000098F8 File Offset: 0x00007AF8
		static ListViewBase()
		{
			ListBox.SelectionModeProperty.OverrideMetadata(typeof(ListViewBase), new FrameworkPropertyMetadata(new PropertyChangedCallback(ListViewBase.OnSelectionModePropertyChanged)));
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00009A17 File Offset: 0x00007C17
		protected ListViewBase()
		{
			this.UpdateMultiSelectEnabled();
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x00009A25 File Offset: 0x00007C25
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x00009A37 File Offset: 0x00007C37
		public bool IsItemClickEnabled
		{
			get
			{
				return (bool)base.GetValue(ListViewBase.IsItemClickEnabledProperty);
			}
			set
			{
				base.SetValue(ListViewBase.IsItemClickEnabledProperty, value);
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x00009A4A File Offset: 0x00007C4A
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x00009A5C File Offset: 0x00007C5C
		public bool IsSelectionEnabled
		{
			get
			{
				return (bool)base.GetValue(ListViewBase.IsSelectionEnabledProperty);
			}
			set
			{
				base.SetValue(ListViewBase.IsSelectionEnabledProperty, value);
			}
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00009A70 File Offset: 0x00007C70
		private static void OnIsSelectionEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ListViewBase listViewBase = (ListViewBase)d;
			listViewBase.UpdateMultiSelectEnabled();
			if (!(bool)e.NewValue && listViewBase.SelectedItems.Count > 0)
			{
				listViewBase.UnselectAll();
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x00009AAC File Offset: 0x00007CAC
		// (set) Token: 0x060002D6 RID: 726 RVA: 0x00009ABE File Offset: 0x00007CBE
		public bool IsMultiSelectCheckBoxEnabled
		{
			get
			{
				return (bool)base.GetValue(ListViewBase.IsMultiSelectCheckBoxEnabledProperty);
			}
			set
			{
				base.SetValue(ListViewBase.IsMultiSelectCheckBoxEnabledProperty, value);
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00009AD1 File Offset: 0x00007CD1
		private static void OnIsMultiSelectCheckBoxEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ListViewBase)d).UpdateMultiSelectEnabled();
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x00009ADE File Offset: 0x00007CDE
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x00009AF0 File Offset: 0x00007CF0
		public bool UseSystemFocusVisuals
		{
			get
			{
				return (bool)base.GetValue(ListViewBase.UseSystemFocusVisualsProperty);
			}
			set
			{
				base.SetValue(ListViewBase.UseSystemFocusVisualsProperty, value);
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002DA RID: 730 RVA: 0x00009B03 File Offset: 0x00007D03
		// (set) Token: 0x060002DB RID: 731 RVA: 0x00009B15 File Offset: 0x00007D15
		public Thickness FocusVisualMargin
		{
			get
			{
				return (Thickness)base.GetValue(ListViewBase.FocusVisualMarginProperty);
			}
			set
			{
				base.SetValue(ListViewBase.FocusVisualMarginProperty, value);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060002DC RID: 732 RVA: 0x00009B28 File Offset: 0x00007D28
		// (set) Token: 0x060002DD RID: 733 RVA: 0x00009B3A File Offset: 0x00007D3A
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(ListViewBase.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(ListViewBase.CornerRadiusProperty, value);
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00009B4D File Offset: 0x00007D4D
		// (set) Token: 0x060002DF RID: 735 RVA: 0x00009B55 File Offset: 0x00007D55
		internal bool MultiSelectEnabled
		{
			get
			{
				return this.m_multiSelectEnabled;
			}
			set
			{
				if (this.m_multiSelectEnabled != value)
				{
					this.m_multiSelectEnabled = value;
					EventHandler multiSelectEnabledChanged = this.MultiSelectEnabledChanged;
					if (multiSelectEnabledChanged == null)
					{
						return;
					}
					multiSelectEnabledChanged.Invoke(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060002E0 RID: 736 RVA: 0x00009B80 File Offset: 0x00007D80
		// (remove) Token: 0x060002E1 RID: 737 RVA: 0x00009BB8 File Offset: 0x00007DB8
		public event ItemClickEventHandler ItemClick;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060002E2 RID: 738 RVA: 0x00009BF0 File Offset: 0x00007DF0
		// (remove) Token: 0x060002E3 RID: 739 RVA: 0x00009C28 File Offset: 0x00007E28
		internal event EventHandler MultiSelectEnabledChanged;

		// Token: 0x060002E4 RID: 740 RVA: 0x00009C60 File Offset: 0x00007E60
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			ListViewBaseItem listViewBaseItem = element as ListViewBaseItem;
			if (listViewBaseItem != null)
			{
				listViewBaseItem.SubscribeToMultiSelectEnabledChanged(this);
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00009C88 File Offset: 0x00007E88
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
			ListViewBaseItem listViewBaseItem = element as ListViewBaseItem;
			if (listViewBaseItem != null)
			{
				listViewBaseItem.UnsubscribeFromMultiSelectEnabledChanged(this);
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00009CAE File Offset: 0x00007EAE
		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			if (this.IsSelectionEnabled)
			{
				base.OnSelectionChanged(e);
				return;
			}
			if (base.SelectedItems.Count > 0)
			{
				base.UnselectAll();
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00009CD4 File Offset: 0x00007ED4
		internal void NotifyListItemClicked(ListViewBaseItem item)
		{
			if (this.IsItemClickEnabled)
			{
				object clickedItem = base.ItemContainerGenerator.ItemFromContainer(item);
				ItemClickEventHandler itemClick = this.ItemClick;
				if (itemClick == null)
				{
					return;
				}
				itemClick(this, new ItemClickEventArgs
				{
					ClickedItem = clickedItem
				});
			}
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00009D13 File Offset: 0x00007F13
		private static void OnSelectionModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ListViewBase)d).UpdateMultiSelectEnabled();
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00009D20 File Offset: 0x00007F20
		private void UpdateMultiSelectEnabled()
		{
			this.MultiSelectEnabled = (this.IsSelectionEnabled && base.SelectionMode == 1 && this.IsMultiSelectCheckBoxEnabled);
		}

		// Token: 0x0400010D RID: 269
		public static readonly DependencyProperty IsItemClickEnabledProperty = DependencyProperty.Register("IsItemClickEnabled", typeof(bool), typeof(ListViewBase), new PropertyMetadata(false));

		// Token: 0x0400010E RID: 270
		public static readonly DependencyProperty IsSelectionEnabledProperty = DependencyProperty.Register("IsSelectionEnabled", typeof(bool), typeof(ListViewBase), new PropertyMetadata(true, new PropertyChangedCallback(ListViewBase.OnIsSelectionEnabledChanged)));

		// Token: 0x0400010F RID: 271
		public static readonly DependencyProperty IsMultiSelectCheckBoxEnabledProperty = DependencyProperty.Register("IsMultiSelectCheckBoxEnabled", typeof(bool), typeof(ListViewBase), new PropertyMetadata(true, new PropertyChangedCallback(ListViewBase.OnIsMultiSelectCheckBoxEnabledChanged)));

		// Token: 0x04000110 RID: 272
		public static readonly DependencyProperty UseSystemFocusVisualsProperty = FocusVisualHelper.UseSystemFocusVisualsProperty.AddOwner(typeof(ListViewBase));

		// Token: 0x04000111 RID: 273
		public static readonly DependencyProperty FocusVisualMarginProperty = FocusVisualHelper.FocusVisualMarginProperty.AddOwner(typeof(ListViewBase));

		// Token: 0x04000112 RID: 274
		public static readonly DependencyProperty CornerRadiusProperty = ControlHelper.CornerRadiusProperty.AddOwner(typeof(ListViewBase));

		// Token: 0x04000115 RID: 277
		private bool m_multiSelectEnabled;
	}
}
