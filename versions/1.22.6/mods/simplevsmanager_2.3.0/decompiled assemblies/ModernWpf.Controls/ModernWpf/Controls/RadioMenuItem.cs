using System;
using System.Windows;
using System.Windows.Controls;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000084 RID: 132
	public class RadioMenuItem : MenuItem
	{
		// Token: 0x06000693 RID: 1683 RVA: 0x00017F4C File Offset: 0x0001614C
		static RadioMenuItem()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioMenuItem), new FrameworkPropertyMetadata(typeof(RadioMenuItem)));
			MenuItem.IsCheckableProperty.OverrideMetadata(typeof(RadioMenuItem), new FrameworkPropertyMetadata(true, null, new CoerceValueCallback(RadioMenuItem.CoerceIsCheckable)));
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00017FFA File Offset: 0x000161FA
		private static object CoerceIsCheckable(DependencyObject d, object baseValue)
		{
			return true;
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x00018002 File Offset: 0x00016202
		// (set) Token: 0x06000696 RID: 1686 RVA: 0x00018014 File Offset: 0x00016214
		public string GroupName
		{
			get
			{
				return (string)base.GetValue(RadioMenuItem.GroupNameProperty);
			}
			set
			{
				base.SetValue(RadioMenuItem.GroupNameProperty, value);
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00018022 File Offset: 0x00016222
		private static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((RadioMenuItem)d).UpdateSiblings();
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x0001802F File Offset: 0x0001622F
		// (set) Token: 0x06000699 RID: 1689 RVA: 0x00018041 File Offset: 0x00016241
		public bool UseSystemFocusVisuals
		{
			get
			{
				return (bool)base.GetValue(RadioMenuItem.UseSystemFocusVisualsProperty);
			}
			set
			{
				base.SetValue(RadioMenuItem.UseSystemFocusVisualsProperty, value);
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00018054 File Offset: 0x00016254
		protected override void OnChecked(RoutedEventArgs e)
		{
			if (this.m_surpressOnChecked)
			{
				e.Handled = true;
				return;
			}
			this.UpdateSiblings();
			base.OnChecked(e);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00018073 File Offset: 0x00016273
		protected override void OnUnchecked(RoutedEventArgs e)
		{
			if (!this.m_isSafeUncheck)
			{
				this.m_surpressOnChecked = true;
				base.SetCurrentValue(MenuItem.IsCheckedProperty, true);
				this.m_surpressOnChecked = false;
				e.Handled = true;
				return;
			}
			base.OnUnchecked(e);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x000180AC File Offset: 0x000162AC
		private void UpdateSiblings()
		{
			if (base.IsChecked)
			{
				ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(this);
				if (itemsControl != null)
				{
					int count = itemsControl.Items.Count;
					for (int i = 0; i < count; i++)
					{
						RadioMenuItem radioMenuItem = itemsControl.Items[i] as RadioMenuItem;
						if (radioMenuItem != null && radioMenuItem != this && radioMenuItem.GroupName == this.GroupName)
						{
							radioMenuItem.m_isSafeUncheck = true;
							radioMenuItem.SetCurrentValue(MenuItem.IsCheckedProperty, false);
							radioMenuItem.m_isSafeUncheck = false;
						}
					}
				}
			}
		}

		// Token: 0x04000303 RID: 771
		public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(string), typeof(RadioMenuItem), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(RadioMenuItem.OnGroupNameChanged)));

		// Token: 0x04000304 RID: 772
		public static readonly DependencyProperty UseSystemFocusVisualsProperty = FocusVisualHelper.UseSystemFocusVisualsProperty.AddOwner(typeof(RadioMenuItem));

		// Token: 0x04000305 RID: 773
		private bool m_isSafeUncheck;

		// Token: 0x04000306 RID: 774
		private bool m_surpressOnChecked;
	}
}
