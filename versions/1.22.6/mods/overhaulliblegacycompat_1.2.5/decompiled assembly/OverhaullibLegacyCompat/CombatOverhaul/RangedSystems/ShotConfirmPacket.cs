using System;
using ProtoBuf;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000CE RID: 206
	[ProtoContract(ImplicitFields = 1)]
	public class ShotConfirmPacket
	{
		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x00038C12 File Offset: 0x00036E12
		// (set) Token: 0x06000AD5 RID: 2773 RVA: 0x00038C1A File Offset: 0x00036E1A
		public int ShotId { get; set; }

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x00038C23 File Offset: 0x00036E23
		// (set) Token: 0x06000AD7 RID: 2775 RVA: 0x00038C2B File Offset: 0x00036E2B
		public bool Success { get; set; }
	}
}
