using System;
using System.Windows;

namespace ModernWpf.Input
{
	// Token: 0x0200025F RID: 607
	internal sealed class TappedRoutedEventArgs : RoutedEventArgs
	{
		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06001156 RID: 4438 RVA: 0x00033161 File Offset: 0x00031361
		// (set) Token: 0x06001157 RID: 4439 RVA: 0x00033169 File Offset: 0x00031369
		internal int Timestamp { get; set; }
	}
}
