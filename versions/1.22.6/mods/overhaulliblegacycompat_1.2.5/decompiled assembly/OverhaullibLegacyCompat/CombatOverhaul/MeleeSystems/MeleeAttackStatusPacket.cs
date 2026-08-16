using System;
using ProtoBuf;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000F4 RID: 244
	[ProtoContract(ImplicitFields = 1)]
	public struct MeleeAttackStatusPacket
	{
		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000C99 RID: 3225 RVA: 0x0003ED05 File Offset: 0x0003CF05
		// (set) Token: 0x06000C9A RID: 3226 RVA: 0x0003ED0D File Offset: 0x0003CF0D
		public MeleeAttackStatus Status { readonly get; set; }

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x0003ED16 File Offset: 0x0003CF16
		// (set) Token: 0x06000C9C RID: 3228 RVA: 0x0003ED1E File Offset: 0x0003CF1E
		public bool MainHand { readonly get; set; }
	}
}
