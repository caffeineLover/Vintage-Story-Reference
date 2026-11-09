using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000F9 RID: 249
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public class MeleeCollisionPacket
	{
		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x0003F25E File Offset: 0x0003D45E
		// (set) Token: 0x06000CC2 RID: 3266 RVA: 0x0003F266 File Offset: 0x0003D466
		public int PushTier { get; set; }

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x0003F26F File Offset: 0x0003D46F
		// (set) Token: 0x06000CC4 RID: 3268 RVA: 0x0003F277 File Offset: 0x0003D477
		public string Collider { get; set; } = "";

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x0003F280 File Offset: 0x0003D480
		// (set) Token: 0x06000CC6 RID: 3270 RVA: 0x0003F288 File Offset: 0x0003D488
		public int ColliderType { get; set; }

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x0003F291 File Offset: 0x0003D491
		// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x0003F299 File Offset: 0x0003D499
		public long AttackerEntityId { get; set; }

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x0003F2A2 File Offset: 0x0003D4A2
		// (set) Token: 0x06000CCA RID: 3274 RVA: 0x0003F2AA File Offset: 0x0003D4AA
		public long TargetEntityId { get; set; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x0003F2B3 File Offset: 0x0003D4B3
		// (set) Token: 0x06000CCC RID: 3276 RVA: 0x0003F2BB File Offset: 0x0003D4BB
		public bool MainHand { get; set; }
	}
}
