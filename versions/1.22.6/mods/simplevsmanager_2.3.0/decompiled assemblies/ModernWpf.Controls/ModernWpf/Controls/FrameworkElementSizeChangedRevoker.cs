using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x02000029 RID: 41
	internal class FrameworkElementSizeChangedRevoker : EventRevoker<FrameworkElement, SizeChangedEventHandler>
	{
		// Token: 0x060001A6 RID: 422 RVA: 0x00006484 File Offset: 0x00004684
		public FrameworkElementSizeChangedRevoker(FrameworkElement source, SizeChangedEventHandler handler) : base(source, handler)
		{
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000648E File Offset: 0x0000468E
		protected override void AddHandler(FrameworkElement source, SizeChangedEventHandler handler)
		{
			source.SizeChanged += handler;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00006497 File Offset: 0x00004697
		protected override void RemoveHandler(FrameworkElement source, SizeChangedEventHandler handler)
		{
			source.SizeChanged -= handler;
		}
	}
}
