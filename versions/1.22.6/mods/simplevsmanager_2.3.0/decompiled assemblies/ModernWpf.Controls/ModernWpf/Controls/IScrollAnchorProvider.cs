using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x02000090 RID: 144
	public interface IScrollAnchorProvider
	{
		// Token: 0x0600071A RID: 1818
		void RegisterAnchorCandidate(UIElement element);

		// Token: 0x0600071B RID: 1819
		void UnregisterAnchorCandidate(UIElement element);

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600071C RID: 1820
		UIElement CurrentAnchor { get; }
	}
}
