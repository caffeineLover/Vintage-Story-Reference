using System;
using ProtoBuf;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000CD RID: 205
	[ProtoContract(ImplicitFields = 1)]
	public class RangedWeaponStatusPacket
	{
		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x00038BE8 File Offset: 0x00036DE8
		// (set) Token: 0x06000AD0 RID: 2768 RVA: 0x00038BF0 File Offset: 0x00036DF0
		public RangedWeaponStatus Status { get; set; }

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x00038BF9 File Offset: 0x00036DF9
		// (set) Token: 0x06000AD2 RID: 2770 RVA: 0x00038C01 File Offset: 0x00036E01
		public bool MainHand { get; set; }
	}
}
