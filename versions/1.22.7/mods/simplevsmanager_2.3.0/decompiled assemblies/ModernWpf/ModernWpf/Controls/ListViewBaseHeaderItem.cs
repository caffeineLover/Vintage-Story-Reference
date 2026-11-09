using System;
using System.Windows;
using System.Windows.Controls;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000265 RID: 613
	public class ListViewBaseHeaderItem : ContentControl
	{
		// Token: 0x06001183 RID: 4483 RVA: 0x000336CF File Offset: 0x000318CF
		internal ListViewBaseHeaderItem()
		{
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06001184 RID: 4484 RVA: 0x000336D7 File Offset: 0x000318D7
		// (set) Token: 0x06001185 RID: 4485 RVA: 0x000336E9 File Offset: 0x000318E9
		public bool UseSystemFocusVisuals
		{
			get
			{
				return (bool)base.GetValue(ListViewBaseHeaderItem.UseSystemFocusVisualsProperty);
			}
			set
			{
				base.SetValue(ListViewBaseHeaderItem.UseSystemFocusVisualsProperty, value);
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06001186 RID: 4486 RVA: 0x000336FC File Offset: 0x000318FC
		// (set) Token: 0x06001187 RID: 4487 RVA: 0x0003370E File Offset: 0x0003190E
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(ListViewBaseHeaderItem.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(ListViewBaseHeaderItem.CornerRadiusProperty, value);
			}
		}

		// Token: 0x040004D3 RID: 1235
		public static readonly DependencyProperty UseSystemFocusVisualsProperty = FocusVisualHelper.UseSystemFocusVisualsProperty.AddOwner(typeof(ListViewBaseHeaderItem));

		// Token: 0x040004D4 RID: 1236
		public static readonly DependencyProperty CornerRadiusProperty = ControlHelper.CornerRadiusProperty.AddOwner(typeof(ListViewBaseHeaderItem));
	}
}
