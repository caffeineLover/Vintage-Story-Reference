using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x0200004F RID: 79
	public class ListViewBaseItem : ListBoxItem
	{
		// Token: 0x060002EA RID: 746 RVA: 0x00009D42 File Offset: 0x00007F42
		protected ListViewBaseItem()
		{
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060002EB RID: 747 RVA: 0x00009D4A File Offset: 0x00007F4A
		// (set) Token: 0x060002EC RID: 748 RVA: 0x00009D5C File Offset: 0x00007F5C
		public bool UseSystemFocusVisuals
		{
			get
			{
				return (bool)base.GetValue(ListViewBaseItem.UseSystemFocusVisualsProperty);
			}
			set
			{
				base.SetValue(ListViewBaseItem.UseSystemFocusVisualsProperty, value);
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060002ED RID: 749 RVA: 0x00009D6F File Offset: 0x00007F6F
		// (set) Token: 0x060002EE RID: 750 RVA: 0x00009D81 File Offset: 0x00007F81
		public Thickness FocusVisualMargin
		{
			get
			{
				return (Thickness)base.GetValue(ListViewBaseItem.FocusVisualMarginProperty);
			}
			set
			{
				base.SetValue(ListViewBaseItem.FocusVisualMarginProperty, value);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060002EF RID: 751 RVA: 0x00009D94 File Offset: 0x00007F94
		// (set) Token: 0x060002F0 RID: 752 RVA: 0x00009DA6 File Offset: 0x00007FA6
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(ListViewBaseItem.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(ListViewBaseItem.CornerRadiusProperty, value);
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00009DB9 File Offset: 0x00007FB9
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.UpdateMultiSelectStates(this.ParentListViewBase, false);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00009DCE File Offset: 0x00007FCE
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (!e.Handled)
			{
				this.m_isPressed = true;
			}
			base.OnMouseLeftButtonDown(e);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00009DE6 File Offset: 0x00007FE6
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if (!e.Handled)
			{
				this.HandleMouseUp(e);
				this.m_isPressed = false;
			}
			base.OnMouseLeftButtonUp(e);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00009E05 File Offset: 0x00008005
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			if (!e.Handled)
			{
				this.m_isPressed = false;
			}
			base.OnMouseLeave(e);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00009E1D File Offset: 0x0000801D
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Key == 6)
			{
				this.OnClick();
				e.Handled = true;
			}
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00009E3C File Offset: 0x0000803C
		internal void SubscribeToMultiSelectEnabledChanged(ListViewBase parent)
		{
			parent.MultiSelectEnabledChanged += new EventHandler(this.OnMultiSelectEnabledChanged);
			this.UpdateMultiSelectStates(parent, true);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00009E58 File Offset: 0x00008058
		internal void UnsubscribeFromMultiSelectEnabledChanged(ListViewBase parent)
		{
			parent.MultiSelectEnabledChanged -= new EventHandler(this.OnMultiSelectEnabledChanged);
			this.UpdateMultiSelectStates(parent, true);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00009E74 File Offset: 0x00008074
		private void OnMultiSelectEnabledChanged(object sender, EventArgs e)
		{
			this.UpdateMultiSelectStates((ListViewBase)sender, true);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00009E84 File Offset: 0x00008084
		private void UpdateMultiSelectStates(ListViewBase parent, bool useTransitions = true)
		{
			if (parent != null)
			{
				VisualStateManager.GoToState(this, (parent.MultiSelectEnabled && parent.IsMultiSelectCheckBoxEnabled) ? "MultiSelectEnabled" : "MultiSelectDisabled", useTransitions);
			}
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00009EC0 File Offset: 0x000080C0
		private void HandleMouseUp(MouseButtonEventArgs e)
		{
			if (this.m_isPressed)
			{
				Rect rect;
				rect..ctor(default(Point), base.RenderSize);
				if (rect.Contains(e.GetPosition(this)))
				{
					this.OnClick();
				}
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00009F01 File Offset: 0x00008101
		private void OnClick()
		{
			ListViewBase parentListViewBase = this.ParentListViewBase;
			if (parentListViewBase == null)
			{
				return;
			}
			parentListViewBase.NotifyListItemClicked(this);
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060002FC RID: 764 RVA: 0x00009F14 File Offset: 0x00008114
		private ListViewBase ParentListViewBase
		{
			get
			{
				return ItemsControl.ItemsControlFromItemContainer(this) as ListViewBase;
			}
		}

		// Token: 0x04000116 RID: 278
		public static readonly DependencyProperty UseSystemFocusVisualsProperty = FocusVisualHelper.UseSystemFocusVisualsProperty.AddOwner(typeof(ListViewBaseItem));

		// Token: 0x04000117 RID: 279
		public static readonly DependencyProperty FocusVisualMarginProperty = FocusVisualHelper.FocusVisualMarginProperty.AddOwner(typeof(ListViewBaseItem));

		// Token: 0x04000118 RID: 280
		public static readonly DependencyProperty CornerRadiusProperty = ControlHelper.CornerRadiusProperty.AddOwner(typeof(ListViewBaseItem));

		// Token: 0x04000119 RID: 281
		private bool m_isPressed;
	}
}
