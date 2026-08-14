using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using ModernWpf.Automation.Peers;

namespace ModernWpf.Controls
{
	// Token: 0x020000D8 RID: 216
	public class ToggleSplitButton : SplitButton
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000ABA RID: 2746 RVA: 0x00024D6C File Offset: 0x00022F6C
		// (set) Token: 0x06000ABB RID: 2747 RVA: 0x00024D7E File Offset: 0x00022F7E
		public bool IsChecked
		{
			get
			{
				return (bool)base.GetValue(ToggleSplitButton.IsCheckedProperty);
			}
			set
			{
				base.SetValue(ToggleSplitButton.IsCheckedProperty, value);
			}
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x00024D91 File Offset: 0x00022F91
		private static void OnIsCheckedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ToggleSplitButton)d).OnIsCheckedChanged();
		}

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x06000ABD RID: 2749 RVA: 0x00024DA0 File Offset: 0x00022FA0
		// (remove) Token: 0x06000ABE RID: 2750 RVA: 0x00024DD8 File Offset: 0x00022FD8
		public event TypedEventHandler<ToggleSplitButton, ToggleSplitButtonIsCheckedChangedEventArgs> IsCheckedChanged;

		// Token: 0x06000ABF RID: 2751 RVA: 0x00024E0D File Offset: 0x0002300D
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ToggleSplitButtonAutomationPeer(this);
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00024E18 File Offset: 0x00023018
		private void OnIsCheckedChanged()
		{
			if (this.m_hasLoaded)
			{
				TypedEventHandler<ToggleSplitButton, ToggleSplitButtonIsCheckedChangedEventArgs> isCheckedChanged = this.IsCheckedChanged;
				if (isCheckedChanged != null)
				{
					isCheckedChanged.Invoke(this, new ToggleSplitButtonIsCheckedChangedEventArgs());
				}
				AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(this);
				if (automationPeer != null)
				{
					ToggleState toggleState = this.IsChecked ? 1 : 0;
					ToggleState toggleState2 = (toggleState == 1) ? 0 : 1;
					automationPeer.RaisePropertyChangedEvent(TogglePatternIdentifiers.ToggleStateProperty, toggleState2, toggleState);
				}
			}
			base.UpdateVisualStates(true);
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x00024E82 File Offset: 0x00023082
		internal override void OnClickPrimary(object sender, RoutedEventArgs e)
		{
			this.Toggle();
			base.OnClickPrimary(sender, e);
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x00024E92 File Offset: 0x00023092
		internal override bool InternalIsChecked
		{
			get
			{
				return this.IsChecked;
			}
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x00024E9A File Offset: 0x0002309A
		internal void Toggle()
		{
			base.SetCurrentValue(ToggleSplitButton.IsCheckedProperty, !this.IsChecked);
		}

		// Token: 0x0400045C RID: 1116
		public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(ToggleSplitButton), new FrameworkPropertyMetadata(false, 1280, new PropertyChangedCallback(ToggleSplitButton.OnIsCheckedPropertyChanged)));
	}
}
