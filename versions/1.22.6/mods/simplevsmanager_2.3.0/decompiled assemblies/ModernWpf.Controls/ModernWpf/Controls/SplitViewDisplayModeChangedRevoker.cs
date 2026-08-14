using System;

namespace ModernWpf.Controls
{
	// Token: 0x020000DA RID: 218
	internal class SplitViewDisplayModeChangedRevoker : EventRevoker<SplitView, DependencyPropertyChangedCallback>
	{
		// Token: 0x06000AC8 RID: 2760 RVA: 0x00024F20 File Offset: 0x00023120
		public SplitViewDisplayModeChangedRevoker(SplitView source, DependencyPropertyChangedCallback handler) : base(source, handler)
		{
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x00024F2A File Offset: 0x0002312A
		protected override void AddHandler(SplitView source, DependencyPropertyChangedCallback handler)
		{
			source.DisplayModeChanged += handler;
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00024F33 File Offset: 0x00023133
		protected override void RemoveHandler(SplitView source, DependencyPropertyChangedCallback handler)
		{
			source.DisplayModeChanged -= handler;
		}
	}
}
