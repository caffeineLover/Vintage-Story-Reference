using System;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020000EC RID: 236
	internal class FlyoutBaseClosingRevoker : EventRevoker<FlyoutBase, TypedEventHandler<FlyoutBase, FlyoutBaseClosingEventArgs>>
	{
		// Token: 0x06000C1A RID: 3098 RVA: 0x0002965F File Offset: 0x0002785F
		public FlyoutBaseClosingRevoker(FlyoutBase source, TypedEventHandler<FlyoutBase, FlyoutBaseClosingEventArgs> handler) : base(source, handler)
		{
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00029669 File Offset: 0x00027869
		protected override void AddHandler(FlyoutBase source, TypedEventHandler<FlyoutBase, FlyoutBaseClosingEventArgs> handler)
		{
			source.Closing += handler;
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00029672 File Offset: 0x00027872
		protected override void RemoveHandler(FlyoutBase source, TypedEventHandler<FlyoutBase, FlyoutBaseClosingEventArgs> handler)
		{
			source.Closing -= handler;
		}
	}
}
