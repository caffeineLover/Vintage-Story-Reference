using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x0200004C RID: 76
	public sealed class ItemClickEventArgs : RoutedEventArgs
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x000098A5 File Offset: 0x00007AA5
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x000098AD File Offset: 0x00007AAD
		public object ClickedItem { get; internal set; }
	}
}
