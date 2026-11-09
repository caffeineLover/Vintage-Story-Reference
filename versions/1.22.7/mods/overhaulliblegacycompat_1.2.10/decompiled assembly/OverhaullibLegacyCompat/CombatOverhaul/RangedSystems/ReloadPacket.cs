using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000D4 RID: 212
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public class ReloadPacket
	{
		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x00039A73 File Offset: 0x00037C73
		// (set) Token: 0x06000B07 RID: 2823 RVA: 0x00039A7B File Offset: 0x00037C7B
		public string InventoryId { get; set; } = "";

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x00039A84 File Offset: 0x00037C84
		// (set) Token: 0x06000B09 RID: 2825 RVA: 0x00039A8C File Offset: 0x00037C8C
		public int? SlotId { get; set; }

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000B0A RID: 2826 RVA: 0x00039A95 File Offset: 0x00037C95
		// (set) Token: 0x06000B0B RID: 2827 RVA: 0x00039A9D File Offset: 0x00037C9D
		public int Amount { get; set; }

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x00039AA6 File Offset: 0x00037CA6
		// (set) Token: 0x06000B0D RID: 2829 RVA: 0x00039AAE File Offset: 0x00037CAE
		public int ItemId { get; set; }

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000B0E RID: 2830 RVA: 0x00039AB7 File Offset: 0x00037CB7
		// (set) Token: 0x06000B0F RID: 2831 RVA: 0x00039ABF File Offset: 0x00037CBF
		public bool RightHand { get; set; }

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000B10 RID: 2832 RVA: 0x00039AC8 File Offset: 0x00037CC8
		// (set) Token: 0x06000B11 RID: 2833 RVA: 0x00039AD0 File Offset: 0x00037CD0
		public int ReloadId { get; set; }

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000B12 RID: 2834 RVA: 0x00039AD9 File Offset: 0x00037CD9
		// (set) Token: 0x06000B13 RID: 2835 RVA: 0x00039AE1 File Offset: 0x00037CE1
		public byte[] Data { get; set; } = Array.Empty<byte>();
	}
}
