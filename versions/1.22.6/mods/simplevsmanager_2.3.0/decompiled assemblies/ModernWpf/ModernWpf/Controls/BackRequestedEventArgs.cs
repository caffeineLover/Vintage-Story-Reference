using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x0200027F RID: 639
	public sealed class BackRequestedEventArgs : RoutedEventArgs
	{
		// Token: 0x06001298 RID: 4760 RVA: 0x00036AB4 File Offset: 0x00034CB4
		internal BackRequestedEventArgs() : base(TitleBar.BackRequestedEvent)
		{
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x00036AC1 File Offset: 0x00034CC1
		internal BackRequestedEventArgs(object source) : base(TitleBar.BackRequestedEvent, source)
		{
		}
	}
}
