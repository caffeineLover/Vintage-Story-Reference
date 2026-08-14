using System;

namespace ModernWpf.Controls
{
	// Token: 0x02000094 RID: 148
	internal class ItemsRepeaterElementPreparedRevoker : EventRevoker<ItemsRepeater, TypedEventHandler<ItemsRepeater, ItemsRepeaterElementPreparedEventArgs>>
	{
		// Token: 0x0600074B RID: 1867 RVA: 0x0001A143 File Offset: 0x00018343
		public ItemsRepeaterElementPreparedRevoker(ItemsRepeater source, TypedEventHandler<ItemsRepeater, ItemsRepeaterElementPreparedEventArgs> handler) : base(source, handler)
		{
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0001A14D File Offset: 0x0001834D
		protected override void AddHandler(ItemsRepeater source, TypedEventHandler<ItemsRepeater, ItemsRepeaterElementPreparedEventArgs> handler)
		{
			source.ElementPrepared += handler;
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0001A156 File Offset: 0x00018356
		protected override void RemoveHandler(ItemsRepeater source, TypedEventHandler<ItemsRepeater, ItemsRepeaterElementPreparedEventArgs> handler)
		{
			source.ElementPrepared -= handler;
		}
	}
}
