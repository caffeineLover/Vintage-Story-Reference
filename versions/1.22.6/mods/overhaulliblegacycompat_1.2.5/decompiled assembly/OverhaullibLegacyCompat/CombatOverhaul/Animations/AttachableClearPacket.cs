using System;
using ProtoBuf;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000193 RID: 403
	[ProtoContract(ImplicitFields = 1)]
	public sealed class AttachableClearPacket
	{
		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x060013AB RID: 5035 RVA: 0x00061DB9 File Offset: 0x0005FFB9
		// (set) Token: 0x060013AC RID: 5036 RVA: 0x00061DC1 File Offset: 0x0005FFC1
		public long EntityId { get; set; }

		// Token: 0x060013AD RID: 5037 RVA: 0x00061DCA File Offset: 0x0005FFCA
		public AttachableClearPacket()
		{
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x00061DD2 File Offset: 0x0005FFD2
		public AttachableClearPacket(long entityId)
		{
			this.EntityId = entityId;
		}
	}
}
