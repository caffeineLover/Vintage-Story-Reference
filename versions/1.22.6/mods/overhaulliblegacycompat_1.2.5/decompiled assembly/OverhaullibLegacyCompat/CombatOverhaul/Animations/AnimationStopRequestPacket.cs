using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000169 RID: 361
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class AnimationStopRequestPacket
	{
		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06001221 RID: 4641 RVA: 0x000588F7 File Offset: 0x00056AF7
		// (set) Token: 0x06001222 RID: 4642 RVA: 0x000588FF File Offset: 0x00056AFF
		public string Category { get; set; } = "";

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x00058908 File Offset: 0x00056B08
		// (set) Token: 0x06001224 RID: 4644 RVA: 0x00058910 File Offset: 0x00056B10
		public long EntityId { get; set; }

		// Token: 0x06001225 RID: 4645 RVA: 0x00058919 File Offset: 0x00056B19
		public AnimationStopRequestPacket()
		{
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0005892C File Offset: 0x00056B2C
		public AnimationStopRequestPacket(string category, long entityId)
		{
			this.Category = category;
			this.EntityId = entityId;
		}
	}
}
