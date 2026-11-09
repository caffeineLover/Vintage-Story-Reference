using System;
using ProtoBuf;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001A1 RID: 417
	[ProtoContract(ImplicitFields = 1)]
	public sealed class AttachableClearPacket
	{
		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001423 RID: 5155 RVA: 0x00063839 File Offset: 0x00061A39
		// (set) Token: 0x06001424 RID: 5156 RVA: 0x00063841 File Offset: 0x00061A41
		public long EntityId { get; set; }

		// Token: 0x06001425 RID: 5157 RVA: 0x0006384A File Offset: 0x00061A4A
		public AttachableClearPacket()
		{
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x00063852 File Offset: 0x00061A52
		public AttachableClearPacket(long entityId)
		{
			this.EntityId = entityId;
		}
	}
}
