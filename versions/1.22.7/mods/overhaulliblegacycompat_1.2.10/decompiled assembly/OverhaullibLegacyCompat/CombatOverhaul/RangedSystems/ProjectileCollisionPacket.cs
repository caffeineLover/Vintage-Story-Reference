using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000D0 RID: 208
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public class ProjectileCollisionPacket
	{
		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000A9C RID: 2716 RVA: 0x0003746E File Offset: 0x0003566E
		// (set) Token: 0x06000A9D RID: 2717 RVA: 0x00037476 File Offset: 0x00035676
		public Guid Id { get; set; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000A9E RID: 2718 RVA: 0x0003747F File Offset: 0x0003567F
		// (set) Token: 0x06000A9F RID: 2719 RVA: 0x00037487 File Offset: 0x00035687
		public double[] CollisionPoint { get; set; } = Array.Empty<double>();

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x00037490 File Offset: 0x00035690
		// (set) Token: 0x06000AA1 RID: 2721 RVA: 0x00037498 File Offset: 0x00035698
		public double[] AfterCollisionVelocity { get; set; } = Array.Empty<double>();

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x000374A1 File Offset: 0x000356A1
		// (set) Token: 0x06000AA3 RID: 2723 RVA: 0x000374A9 File Offset: 0x000356A9
		public double RelativeSpeed { get; set; }

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x000374B2 File Offset: 0x000356B2
		// (set) Token: 0x06000AA5 RID: 2725 RVA: 0x000374BA File Offset: 0x000356BA
		public string Collider { get; set; } = "";

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x000374C3 File Offset: 0x000356C3
		// (set) Token: 0x06000AA7 RID: 2727 RVA: 0x000374CB File Offset: 0x000356CB
		public long ReceiverEntity { get; set; }

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x000374D4 File Offset: 0x000356D4
		// (set) Token: 0x06000AA9 RID: 2729 RVA: 0x000374DC File Offset: 0x000356DC
		public int PacketVersion { get; set; }

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000AAA RID: 2730 RVA: 0x000374E5 File Offset: 0x000356E5
		// (set) Token: 0x06000AAB RID: 2731 RVA: 0x000374ED File Offset: 0x000356ED
		public float PenetrationStrengthLoss { get; set; }
	}
}
