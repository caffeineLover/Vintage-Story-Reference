using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x02000062 RID: 98
	internal class NavigationViewItemHelper<T> : NavigationViewItemHelper
	{
		// Token: 0x0600048B RID: 1163 RVA: 0x000119C5 File Offset: 0x0000FBC5
		public UIElement GetSelectionIndicator()
		{
			return this.m_selectionIndicator;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x000119CD File Offset: 0x0000FBCD
		public void Init(IControlProtected controlProtected)
		{
			this.m_selectionIndicator = (controlProtected.GetTemplateChild("SelectionIndicator") as UIElement);
		}

		// Token: 0x0400021D RID: 541
		private UIElement m_selectionIndicator;

		// Token: 0x0400021E RID: 542
		private const string c_selectionIndicatorName = "SelectionIndicator";
	}
}
