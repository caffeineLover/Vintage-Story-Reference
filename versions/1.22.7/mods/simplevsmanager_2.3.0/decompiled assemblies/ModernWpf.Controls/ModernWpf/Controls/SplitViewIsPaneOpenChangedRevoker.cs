using System;

namespace ModernWpf.Controls
{
	// Token: 0x020000D9 RID: 217
	internal class SplitViewIsPaneOpenChangedRevoker : EventRevoker<SplitView, DependencyPropertyChangedCallback>
	{
		// Token: 0x06000AC5 RID: 2757 RVA: 0x00024F04 File Offset: 0x00023104
		public SplitViewIsPaneOpenChangedRevoker(SplitView source, DependencyPropertyChangedCallback handler) : base(source, handler)
		{
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00024F0E File Offset: 0x0002310E
		protected override void AddHandler(SplitView source, DependencyPropertyChangedCallback handler)
		{
			source.IsPaneOpenChanged += handler;
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x00024F17 File Offset: 0x00023117
		protected override void RemoveHandler(SplitView source, DependencyPropertyChangedCallback handler)
		{
			source.IsPaneOpenChanged -= handler;
		}
	}
}
