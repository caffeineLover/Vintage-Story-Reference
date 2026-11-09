using System;
using ProtoBuf;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000D8 RID: 216
	[ProtoContract(ImplicitFields = 1)]
	public class RangedWeaponStatusPacket
	{
		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000B2B RID: 2859 RVA: 0x00039BD8 File Offset: 0x00037DD8
		// (set) Token: 0x06000B2C RID: 2860 RVA: 0x00039BE0 File Offset: 0x00037DE0
		public RangedWeaponStatus Status { get; set; }

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000B2D RID: 2861 RVA: 0x00039BE9 File Offset: 0x00037DE9
		// (set) Token: 0x06000B2E RID: 2862 RVA: 0x00039BF1 File Offset: 0x00037DF1
		public bool MainHand { get; set; }
	}
}
