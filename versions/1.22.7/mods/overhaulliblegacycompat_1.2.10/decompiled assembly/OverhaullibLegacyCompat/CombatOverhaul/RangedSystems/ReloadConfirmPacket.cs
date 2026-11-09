using System;
using ProtoBuf;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000D5 RID: 213
	[ProtoContract(ImplicitFields = 1)]
	public class ReloadConfirmPacket
	{
		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x00039B08 File Offset: 0x00037D08
		// (set) Token: 0x06000B16 RID: 2838 RVA: 0x00039B10 File Offset: 0x00037D10
		public int ReloadId { get; set; }

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x00039B19 File Offset: 0x00037D19
		// (set) Token: 0x06000B18 RID: 2840 RVA: 0x00039B21 File Offset: 0x00037D21
		public bool Success { get; set; }
	}
}
