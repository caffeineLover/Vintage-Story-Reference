using System;
using ProtoBuf;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000194 RID: 404
	[ProtoContract(ImplicitFields = 1)]
	public sealed class AttachableSwitchModelsPacket
	{
		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x060013AF RID: 5039 RVA: 0x00061DE1 File Offset: 0x0005FFE1
		// (set) Token: 0x060013B0 RID: 5040 RVA: 0x00061DE9 File Offset: 0x0005FFE9
		public long EntityId { get; set; }

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x060013B1 RID: 5041 RVA: 0x00061DF2 File Offset: 0x0005FFF2
		// (set) Token: 0x060013B2 RID: 5042 RVA: 0x00061DFA File Offset: 0x0005FFFA
		public bool SwitchModels { get; set; }

		// Token: 0x060013B3 RID: 5043 RVA: 0x00061E03 File Offset: 0x00060003
		public AttachableSwitchModelsPacket()
		{
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x00061E0B File Offset: 0x0006000B
		public AttachableSwitchModelsPacket(long entityId, bool switchModels)
		{
			this.EntityId = entityId;
			this.SwitchModels = switchModels;
		}
	}
}
