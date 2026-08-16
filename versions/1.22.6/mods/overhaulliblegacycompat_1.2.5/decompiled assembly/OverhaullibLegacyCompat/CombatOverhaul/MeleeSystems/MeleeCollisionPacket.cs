using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000EE RID: 238
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public class MeleeCollisionPacket
	{
		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000C65 RID: 3173 RVA: 0x0003E26E File Offset: 0x0003C46E
		// (set) Token: 0x06000C66 RID: 3174 RVA: 0x0003E276 File Offset: 0x0003C476
		public int PushTier { get; set; }

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x0003E27F File Offset: 0x0003C47F
		// (set) Token: 0x06000C68 RID: 3176 RVA: 0x0003E287 File Offset: 0x0003C487
		public string Collider { get; set; } = "";

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000C69 RID: 3177 RVA: 0x0003E290 File Offset: 0x0003C490
		// (set) Token: 0x06000C6A RID: 3178 RVA: 0x0003E298 File Offset: 0x0003C498
		public int ColliderType { get; set; }

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x0003E2A1 File Offset: 0x0003C4A1
		// (set) Token: 0x06000C6C RID: 3180 RVA: 0x0003E2A9 File Offset: 0x0003C4A9
		public long AttackerEntityId { get; set; }

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x0003E2B2 File Offset: 0x0003C4B2
		// (set) Token: 0x06000C6E RID: 3182 RVA: 0x0003E2BA File Offset: 0x0003C4BA
		public long TargetEntityId { get; set; }

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x0003E2C3 File Offset: 0x0003C4C3
		// (set) Token: 0x06000C70 RID: 3184 RVA: 0x0003E2CB File Offset: 0x0003C4CB
		public bool MainHand { get; set; }
	}
}
