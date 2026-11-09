using System;
using ProtoBuf;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000D9 RID: 217
	[ProtoContract(ImplicitFields = 1)]
	public class ShotConfirmPacket
	{
		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x00039C02 File Offset: 0x00037E02
		// (set) Token: 0x06000B31 RID: 2865 RVA: 0x00039C0A File Offset: 0x00037E0A
		public int ShotId { get; set; }

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000B32 RID: 2866 RVA: 0x00039C13 File Offset: 0x00037E13
		// (set) Token: 0x06000B33 RID: 2867 RVA: 0x00039C1B File Offset: 0x00037E1B
		public bool Success { get; set; }
	}
}
