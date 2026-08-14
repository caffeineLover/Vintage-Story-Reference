using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020000E2 RID: 226
	public class AutoSuggestBoxListViewItem : ListViewItem
	{
		// Token: 0x06000B57 RID: 2903 RVA: 0x00026812 File Offset: 0x00024A12
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (!e.Handled)
			{
				e.Handled = true;
				this.m_isPressed = true;
			}
			base.OnMouseLeftButtonDown(e);
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x00026831 File Offset: 0x00024A31
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if (!e.Handled)
			{
				e.Handled = true;
				this.HandleMouseUp(0);
				this.m_isPressed = false;
			}
			base.OnMouseLeftButtonUp(e);
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00026857 File Offset: 0x00024A57
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			if (!e.Handled)
			{
				this.m_isPressed = false;
			}
			base.OnMouseLeave(e);
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0002686F File Offset: 0x00024A6F
		private void HandleMouseUp(MouseButton mouseButton)
		{
			if (this.m_isPressed && SelectorHelper.UiGetIsSelectable(this) && base.Focus())
			{
				AutoSuggestBoxListView parentListView = this.ParentListView;
				if (parentListView == null)
				{
					return;
				}
				parentListView.NotifyListItemClicked(this, new MouseButton?(mouseButton));
			}
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x000268A0 File Offset: 0x00024AA0
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Key == 6 && SelectorHelper.UiGetIsSelectable(this) && base.Focus())
			{
				AutoSuggestBoxListView parentListView = this.ParentListView;
				if (parentListView != null)
				{
					parentListView.NotifyListItemClicked(this, default(MouseButton?));
				}
				e.Handled = true;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000B5C RID: 2908 RVA: 0x000268EF File Offset: 0x00024AEF
		private AutoSuggestBoxListView ParentListView
		{
			get
			{
				return this.ParentSelector as AutoSuggestBoxListView;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x000268FC File Offset: 0x00024AFC
		internal Selector ParentSelector
		{
			get
			{
				return ItemsControl.ItemsControlFromItemContainer(this) as Selector;
			}
		}

		// Token: 0x040004A4 RID: 1188
		private bool m_isPressed;
	}
}
