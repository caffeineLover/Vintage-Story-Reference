using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020000E1 RID: 225
	public class AutoSuggestBoxListView : ListView
	{
		// Token: 0x06000B4B RID: 2891 RVA: 0x00026640 File Offset: 0x00024840
		static AutoSuggestBoxListView()
		{
			ListBox.SelectionModeProperty.OverrideMetadata(typeof(AutoSuggestBoxListView), new FrameworkPropertyMetadata(0));
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000B4C RID: 2892 RVA: 0x0002669A File Offset: 0x0002489A
		// (set) Token: 0x06000B4D RID: 2893 RVA: 0x000266AC File Offset: 0x000248AC
		public bool IsItemClickEnabled
		{
			get
			{
				return (bool)base.GetValue(AutoSuggestBoxListView.IsItemClickEnabledProperty);
			}
			set
			{
				base.SetValue(AutoSuggestBoxListView.IsItemClickEnabledProperty, value);
			}
		}

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06000B4E RID: 2894 RVA: 0x000266C0 File Offset: 0x000248C0
		// (remove) Token: 0x06000B4F RID: 2895 RVA: 0x000266F8 File Offset: 0x000248F8
		public event ItemClickEventHandler ItemClick;

		// Token: 0x06000B50 RID: 2896 RVA: 0x0002672D File Offset: 0x0002492D
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.m_scrollHost = VisualTree.FindDescendant<ScrollViewer>(this);
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x00026741 File Offset: 0x00024941
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is AutoSuggestBoxListViewItem;
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x0002674C File Offset: 0x0002494C
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new AutoSuggestBoxListViewItem();
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x00026754 File Offset: 0x00024954
		internal void NotifyListItemClicked(AutoSuggestBoxListViewItem item, MouseButton? mouseButton = null)
		{
			if (this.IsItemClickEnabled)
			{
				this.OnItemClick(item);
			}
			if (base.SelectionMode != null)
			{
				throw new NotImplementedException();
			}
			if (!item.IsSelected)
			{
				item.SetCurrentValue(Selector.IsSelectedProperty, true);
				return;
			}
			if (mouseButton != null && (Keyboard.Modifiers & 2) == 2)
			{
				item.SetCurrentValue(Selector.IsSelectedProperty, false);
				return;
			}
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x000267BD File Offset: 0x000249BD
		internal void ScrollToTop()
		{
			ScrollViewer scrollHost = this.m_scrollHost;
			if (scrollHost == null)
			{
				return;
			}
			scrollHost.ScrollToTop();
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x000267D0 File Offset: 0x000249D0
		private void OnItemClick(AutoSuggestBoxListViewItem lvi)
		{
			object obj = base.ItemContainerGenerator.ItemFromContainer(lvi);
			if (obj != null)
			{
				ItemClickEventHandler itemClick = this.ItemClick;
				if (itemClick == null)
				{
					return;
				}
				itemClick(this, new ItemClickEventArgs
				{
					ClickedItem = obj
				});
			}
		}

		// Token: 0x040004A1 RID: 1185
		public static readonly DependencyProperty IsItemClickEnabledProperty = DependencyProperty.Register("IsItemClickEnabled", typeof(bool), typeof(AutoSuggestBoxListView), new PropertyMetadata(false));

		// Token: 0x040004A3 RID: 1187
		private ScrollViewer m_scrollHost;
	}
}
