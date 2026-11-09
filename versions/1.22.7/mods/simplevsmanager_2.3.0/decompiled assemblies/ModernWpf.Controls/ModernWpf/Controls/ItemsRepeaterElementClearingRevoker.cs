using System;

namespace ModernWpf.Controls
{
	// Token: 0x02000095 RID: 149
	internal class ItemsRepeaterElementClearingRevoker : EventRevoker<ItemsRepeater, TypedEventHandler<ItemsRepeater, ItemsRepeaterElementClearingEventArgs>>
	{
		// Token: 0x0600074E RID: 1870 RVA: 0x0001A15F File Offset: 0x0001835F
		public ItemsRepeaterElementClearingRevoker(ItemsRepeater source, TypedEventHandler<ItemsRepeater, ItemsRepeaterElementClearingEventArgs> handler) : base(source, handler)
		{
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0001A169 File Offset: 0x00018369
		protected override void AddHandler(ItemsRepeater source, TypedEventHandler<ItemsRepeater, ItemsRepeaterElementClearingEventArgs> handler)
		{
			source.ElementClearing += handler;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0001A172 File Offset: 0x00018372
		protected override void RemoveHandler(ItemsRepeater source, TypedEventHandler<ItemsRepeater, ItemsRepeaterElementClearingEventArgs> handler)
		{
			source.ElementClearing -= handler;
		}
	}
}
