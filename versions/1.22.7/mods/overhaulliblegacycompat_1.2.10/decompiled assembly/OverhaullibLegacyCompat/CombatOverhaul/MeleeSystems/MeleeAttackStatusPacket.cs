using System;
using ProtoBuf;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000FF RID: 255
	[ProtoContract(ImplicitFields = 1)]
	public struct MeleeAttackStatusPacket
	{
		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x0003FCF5 File Offset: 0x0003DEF5
		// (set) Token: 0x06000CF6 RID: 3318 RVA: 0x0003FCFD File Offset: 0x0003DEFD
		public MeleeAttackStatus Status { readonly get; set; }

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x0003FD06 File Offset: 0x0003DF06
		// (set) Token: 0x06000CF8 RID: 3320 RVA: 0x0003FD0E File Offset: 0x0003DF0E
		public bool MainHand { readonly get; set; }
	}
}
