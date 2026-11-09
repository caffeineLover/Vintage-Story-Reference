using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x02000293 RID: 659
	public class MenuPopup : Popup
	{
		// Token: 0x170003BD RID: 957
		// (get) Token: 0x0600138B RID: 5003 RVA: 0x000399FC File Offset: 0x00037BFC
		// (set) Token: 0x0600138C RID: 5004 RVA: 0x00039A0E File Offset: 0x00037C0E
		public bool IsSuspendingAnimation
		{
			get
			{
				return (bool)base.GetValue(MenuPopup.IsSuspendingAnimationProperty);
			}
			private set
			{
				base.SetValue(MenuPopup.IsSuspendingAnimationPropertyKey, value);
			}
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x00039A21 File Offset: 0x00037C21
		protected override void OnOpened(EventArgs e)
		{
			base.OnOpened(e);
			base.ClearValue(MenuPopup.IsSuspendingAnimationPropertyKey);
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x00039A35 File Offset: 0x00037C35
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			base.ClearValue(MenuPopup.IsSuspendingAnimationPropertyKey);
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x00039A49 File Offset: 0x00037C49
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			if (e.Property == Popup.IsOpenProperty)
			{
				this.OnIsOpenChanged(e);
			}
			base.OnPropertyChanged(e);
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x00039A68 File Offset: 0x00037C68
		private void OnIsOpenChanged(DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				Window window = Window.GetWindow(this);
				if (window != null)
				{
					IInputElement focusedElement = FocusManager.GetFocusedElement(window);
					if (focusedElement is TextBoxBase || focusedElement is PasswordBox)
					{
						this.IsSuspendingAnimation = true;
					}
				}
			}
		}

		// Token: 0x0400068B RID: 1675
		private static readonly DependencyPropertyKey IsSuspendingAnimationPropertyKey = DependencyProperty.RegisterReadOnly("IsSuspendingAnimation", typeof(bool), typeof(MenuPopup), null);

		// Token: 0x0400068C RID: 1676
		public static readonly DependencyProperty IsSuspendingAnimationProperty = MenuPopup.IsSuspendingAnimationPropertyKey.DependencyProperty;
	}
}
