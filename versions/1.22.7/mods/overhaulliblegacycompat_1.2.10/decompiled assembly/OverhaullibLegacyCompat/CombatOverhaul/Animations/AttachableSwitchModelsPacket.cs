using System;
using ProtoBuf;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001A2 RID: 418
	[ProtoContract(ImplicitFields = 1)]
	public sealed class AttachableSwitchModelsPacket
	{
		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001427 RID: 5159 RVA: 0x00063861 File Offset: 0x00061A61
		// (set) Token: 0x06001428 RID: 5160 RVA: 0x00063869 File Offset: 0x00061A69
		public long EntityId { get; set; }

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001429 RID: 5161 RVA: 0x00063872 File Offset: 0x00061A72
		// (set) Token: 0x0600142A RID: 5162 RVA: 0x0006387A File Offset: 0x00061A7A
		public bool SwitchModels { get; set; }

		// Token: 0x0600142B RID: 5163 RVA: 0x00063883 File Offset: 0x00061A83
		public AttachableSwitchModelsPacket()
		{
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x0006388B File Offset: 0x00061A8B
		public AttachableSwitchModelsPacket(long entityId, bool switchModels)
		{
			this.EntityId = entityId;
			this.SwitchModels = switchModels;
		}
	}
}
