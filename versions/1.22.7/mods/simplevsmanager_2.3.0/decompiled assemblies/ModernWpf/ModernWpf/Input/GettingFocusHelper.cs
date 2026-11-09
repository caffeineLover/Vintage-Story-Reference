using System;
using System.Windows;
using System.Windows.Input;

namespace ModernWpf.Input
{
	// Token: 0x0200025C RID: 604
	internal class GettingFocusHelper : IDisposable
	{
		// Token: 0x0600113F RID: 4415 RVA: 0x00032D6D File Offset: 0x00030F6D
		public GettingFocusHelper(UIElement owner)
		{
			this._owner = owner;
			this._owner.PreviewGotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.OnPreviewGotKeyboardFocus);
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00032D93 File Offset: 0x00030F93
		public void Dispose()
		{
			if (this._owner != null)
			{
				this._owner.PreviewGotKeyboardFocus -= new KeyboardFocusChangedEventHandler(this.OnPreviewGotKeyboardFocus);
				this._owner = null;
			}
		}

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06001141 RID: 4417 RVA: 0x00032DBC File Offset: 0x00030FBC
		// (remove) Token: 0x06001142 RID: 4418 RVA: 0x00032DF4 File Offset: 0x00030FF4
		public event TypedEventHandler<UIElement, GettingFocusEventArgs> GettingFocus;

		// Token: 0x06001143 RID: 4419 RVA: 0x00032E2C File Offset: 0x0003102C
		private void OnPreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (this._ignoreGotFocus)
			{
				return;
			}
			TypedEventHandler<UIElement, GettingFocusEventArgs> gettingFocus = this.GettingFocus;
			if (gettingFocus != null)
			{
				try
				{
					this._ignoreGotFocus = true;
					GettingFocusEventArgs gettingFocusEventArgs = new GettingFocusEventArgs(e);
					gettingFocus.Invoke(sender as UIElement, gettingFocusEventArgs);
					if (gettingFocusEventArgs.Cancel)
					{
						e.Handled = true;
					}
				}
				finally
				{
					this._ignoreGotFocus = false;
				}
			}
		}

		// Token: 0x040004BE RID: 1214
		private UIElement _owner;

		// Token: 0x040004BF RID: 1215
		private bool _ignoreGotFocus;
	}
}
