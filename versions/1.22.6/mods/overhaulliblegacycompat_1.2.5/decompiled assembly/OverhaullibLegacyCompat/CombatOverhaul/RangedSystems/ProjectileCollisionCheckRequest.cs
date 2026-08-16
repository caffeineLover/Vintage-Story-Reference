using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000C6 RID: 198
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public class ProjectileCollisionCheckRequest
	{
		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000A51 RID: 2641 RVA: 0x0003652F File Offset: 0x0003472F
		// (set) Token: 0x06000A52 RID: 2642 RVA: 0x00036537 File Offset: 0x00034737
		public Guid ProjectileId { get; set; }

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000A53 RID: 2643 RVA: 0x00036540 File Offset: 0x00034740
		// (set) Token: 0x06000A54 RID: 2644 RVA: 0x00036548 File Offset: 0x00034748
		public long ProjectileEntityId { get; set; }

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x00036551 File Offset: 0x00034751
		// (set) Token: 0x06000A56 RID: 2646 RVA: 0x00036559 File Offset: 0x00034759
		public double[] CurrentPosition { get; set; } = Array.Empty<double>();

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000A57 RID: 2647 RVA: 0x00036562 File Offset: 0x00034762
		// (set) Token: 0x06000A58 RID: 2648 RVA: 0x0003656A File Offset: 0x0003476A
		public double[] PreviousPosition { get; set; } = Array.Empty<double>();

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000A59 RID: 2649 RVA: 0x00036573 File Offset: 0x00034773
		// (set) Token: 0x06000A5A RID: 2650 RVA: 0x0003657B File Offset: 0x0003477B
		public double[] Velocity { get; set; } = Array.Empty<double>();

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x00036584 File Offset: 0x00034784
		// (set) Token: 0x06000A5C RID: 2652 RVA: 0x0003658C File Offset: 0x0003478C
		public double[] ShooterPosition { get; set; } = Array.Empty<double>();

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x00036595 File Offset: 0x00034795
		// (set) Token: 0x06000A5E RID: 2654 RVA: 0x0003659D File Offset: 0x0003479D
		public double[] ShotDirection { get; set; } = Array.Empty<double>();

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x000365A6 File Offset: 0x000347A6
		// (set) Token: 0x06000A60 RID: 2656 RVA: 0x000365AE File Offset: 0x000347AE
		public float Radius { get; set; }

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x000365B7 File Offset: 0x000347B7
		// (set) Token: 0x06000A62 RID: 2658 RVA: 0x000365BF File Offset: 0x000347BF
		public float PenetrationDistance { get; set; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x000365C8 File Offset: 0x000347C8
		// (set) Token: 0x06000A64 RID: 2660 RVA: 0x000365D0 File Offset: 0x000347D0
		public float PenetrationStrength { get; set; }

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x000365D9 File Offset: 0x000347D9
		// (set) Token: 0x06000A66 RID: 2662 RVA: 0x000365E1 File Offset: 0x000347E1
		public bool CollideWithShooter { get; set; }

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x000365EA File Offset: 0x000347EA
		// (set) Token: 0x06000A68 RID: 2664 RVA: 0x000365F2 File Offset: 0x000347F2
		public long[] IgnoreEntities { get; set; } = Array.Empty<long>();

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x000365FB File Offset: 0x000347FB
		// (set) Token: 0x06000A6A RID: 2666 RVA: 0x00036603 File Offset: 0x00034803
		public long[] CandidateEntityIds { get; set; } = Array.Empty<long>();

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x0003660C File Offset: 0x0003480C
		// (set) Token: 0x06000A6C RID: 2668 RVA: 0x00036614 File Offset: 0x00034814
		public int PacketVersion { get; set; }

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x0003661D File Offset: 0x0003481D
		// (set) Token: 0x06000A6E RID: 2670 RVA: 0x00036625 File Offset: 0x00034825
		public long ShooterId { get; set; }

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x0003662E File Offset: 0x0003482E
		// (set) Token: 0x06000A70 RID: 2672 RVA: 0x00036636 File Offset: 0x00034836
		public bool CheckAABBOnly { get; set; }

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x0003663F File Offset: 0x0003483F
		// (set) Token: 0x06000A72 RID: 2674 RVA: 0x00036647 File Offset: 0x00034847
		public bool RequireColliderWhenAvailable { get; set; }
	}
}
