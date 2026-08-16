using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000C9 RID: 201
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public class ReloadPacket
	{
		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000AAA RID: 2730 RVA: 0x00038A83 File Offset: 0x00036C83
		// (set) Token: 0x06000AAB RID: 2731 RVA: 0x00038A8B File Offset: 0x00036C8B
		public string InventoryId { get; set; } = "";

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000AAC RID: 2732 RVA: 0x00038A94 File Offset: 0x00036C94
		// (set) Token: 0x06000AAD RID: 2733 RVA: 0x00038A9C File Offset: 0x00036C9C
		public int? SlotId { get; set; }

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x00038AA5 File Offset: 0x00036CA5
		// (set) Token: 0x06000AAF RID: 2735 RVA: 0x00038AAD File Offset: 0x00036CAD
		public int Amount { get; set; }

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x00038AB6 File Offset: 0x00036CB6
		// (set) Token: 0x06000AB1 RID: 2737 RVA: 0x00038ABE File Offset: 0x00036CBE
		public int ItemId { get; set; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x00038AC7 File Offset: 0x00036CC7
		// (set) Token: 0x06000AB3 RID: 2739 RVA: 0x00038ACF File Offset: 0x00036CCF
		public bool RightHand { get; set; }

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x00038AD8 File Offset: 0x00036CD8
		// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x00038AE0 File Offset: 0x00036CE0
		public int ReloadId { get; set; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x00038AE9 File Offset: 0x00036CE9
		// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x00038AF1 File Offset: 0x00036CF1
		public byte[] Data { get; set; } = Array.Empty<byte>();
	}
}
