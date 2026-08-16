using System;
using ProtoBuf;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000E5 RID: 229
	[ProtoContract(ImplicitFields = 1)]
	public sealed class ImpaleAttachPacket
	{
		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x0003C787 File Offset: 0x0003A987
		// (set) Token: 0x06000BDB RID: 3035 RVA: 0x0003C78F File Offset: 0x0003A98F
		public long AttackerEntityId { get; set; }

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x0003C798 File Offset: 0x0003A998
		// (set) Token: 0x06000BDD RID: 3037 RVA: 0x0003C7A0 File Offset: 0x0003A9A0
		public long TargetEntityId { get; set; }

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x0003C7A9 File Offset: 0x0003A9A9
		// (set) Token: 0x06000BDF RID: 3039 RVA: 0x0003C7B1 File Offset: 0x0003A9B1
		public bool MainHand { get; set; }
	}
}
