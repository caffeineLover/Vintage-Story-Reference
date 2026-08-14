using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x0200002F RID: 47
	internal class PopupRepositionHelper : IDisposable
	{
		// Token: 0x060001B2 RID: 434 RVA: 0x000065E0 File Offset: 0x000047E0
		public PopupRepositionHelper(Popup popup, UIElement target)
		{
			this.m_popup = popup;
			this.m_target = target;
			this.m_popup.Opened += new EventHandler(this.OnPopupOpened);
			this.m_popup.Closed += new EventHandler(this.OnPopupClosed);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00006630 File Offset: 0x00004830
		public void Dispose()
		{
			this.m_popup.Opened -= new EventHandler(this.OnPopupOpened);
			this.m_popup.Closed -= new EventHandler(this.OnPopupClosed);
			this.m_target.LayoutUpdated -= new EventHandler(this.OnTargetLayoutUpdated);
			this.OnPopupClosed(null, null);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000668C File Offset: 0x0000488C
		private void OnPopupOpened(object sender, EventArgs e)
		{
			if (this.m_isPopupOpen)
			{
				return;
			}
			this.m_isPopupOpen = true;
			this.m_target.LayoutUpdated += new EventHandler(this.OnTargetLayoutUpdated);
			this.m_hostWindow = Window.GetWindow(this.m_target);
			if (this.m_hostWindow != null)
			{
				this.m_hostWindow.LocationChanged += new EventHandler(this.OnHostWindowLocationChanged);
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000066F0 File Offset: 0x000048F0
		private void OnPopupClosed(object sender, EventArgs e)
		{
			this.m_target.LayoutUpdated -= new EventHandler(this.OnTargetLayoutUpdated);
			if (this.m_hostWindow != null)
			{
				this.m_hostWindow.LocationChanged -= new EventHandler(this.OnHostWindowLocationChanged);
				this.m_hostWindow = null;
			}
			this.m_isPopupOpen = false;
			this.m_popupPosition = default(Point);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000674D File Offset: 0x0000494D
		private void OnTargetLayoutUpdated(object sender, EventArgs e)
		{
			this.RepositionPopup();
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00006755 File Offset: 0x00004955
		private void OnHostWindowLocationChanged(object sender, EventArgs e)
		{
			this.RepositionPopup();
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00006760 File Offset: 0x00004960
		private void RepositionPopup()
		{
			if (this.m_popup != null)
			{
				UIElement child = this.m_popup.Child;
				if (child != null && child.IsVisible && this.m_target.IsVisible)
				{
					Point point = child.TranslatePoint(default(Point), this.m_target);
					if (this.m_popupPosition != point)
					{
						this.m_popupPosition = point;
						this.m_popup.Reposition();
					}
				}
			}
		}

		// Token: 0x04000089 RID: 137
		private readonly Popup m_popup;

		// Token: 0x0400008A RID: 138
		private readonly UIElement m_target;

		// Token: 0x0400008B RID: 139
		private bool m_isPopupOpen;

		// Token: 0x0400008C RID: 140
		private Point m_popupPosition;

		// Token: 0x0400008D RID: 141
		private Window m_hostWindow;
	}
}
