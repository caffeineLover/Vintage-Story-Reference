using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000175 RID: 373
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class AnimationStopRequestPacket
	{
		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001295 RID: 4757 RVA: 0x0005A217 File Offset: 0x00058417
		// (set) Token: 0x06001296 RID: 4758 RVA: 0x0005A21F File Offset: 0x0005841F
		public string Category { get; set; } = "";

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001297 RID: 4759 RVA: 0x0005A228 File Offset: 0x00058428
		// (set) Token: 0x06001298 RID: 4760 RVA: 0x0005A230 File Offset: 0x00058430
		public long EntityId { get; set; }

		// Token: 0x06001299 RID: 4761 RVA: 0x0005A239 File Offset: 0x00058439
		public AnimationStopRequestPacket()
		{
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x0005A24C File Offset: 0x0005844C
		public AnimationStopRequestPacket(string category, long entityId)
		{
			this.Category = category;
			this.EntityId = entityId;
		}
	}
}
