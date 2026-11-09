using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000D1 RID: 209
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public class ProjectileCollisionCheckRequest
	{
		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000AAD RID: 2733 RVA: 0x0003751F File Offset: 0x0003571F
		// (set) Token: 0x06000AAE RID: 2734 RVA: 0x00037527 File Offset: 0x00035727
		public Guid ProjectileId { get; set; }

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000AAF RID: 2735 RVA: 0x00037530 File Offset: 0x00035730
		// (set) Token: 0x06000AB0 RID: 2736 RVA: 0x00037538 File Offset: 0x00035738
		public long ProjectileEntityId { get; set; }

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x00037541 File Offset: 0x00035741
		// (set) Token: 0x06000AB2 RID: 2738 RVA: 0x00037549 File Offset: 0x00035749
		public double[] CurrentPosition { get; set; } = Array.Empty<double>();

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x00037552 File Offset: 0x00035752
		// (set) Token: 0x06000AB4 RID: 2740 RVA: 0x0003755A File Offset: 0x0003575A
		public double[] PreviousPosition { get; set; } = Array.Empty<double>();

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x00037563 File Offset: 0x00035763
		// (set) Token: 0x06000AB6 RID: 2742 RVA: 0x0003756B File Offset: 0x0003576B
		public double[] Velocity { get; set; } = Array.Empty<double>();

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x00037574 File Offset: 0x00035774
		// (set) Token: 0x06000AB8 RID: 2744 RVA: 0x0003757C File Offset: 0x0003577C
		public double[] ShooterPosition { get; set; } = Array.Empty<double>();

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x00037585 File Offset: 0x00035785
		// (set) Token: 0x06000ABA RID: 2746 RVA: 0x0003758D File Offset: 0x0003578D
		public double[] ShotDirection { get; set; } = Array.Empty<double>();

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x00037596 File Offset: 0x00035796
		// (set) Token: 0x06000ABC RID: 2748 RVA: 0x0003759E File Offset: 0x0003579E
		public float Radius { get; set; }

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x000375A7 File Offset: 0x000357A7
		// (set) Token: 0x06000ABE RID: 2750 RVA: 0x000375AF File Offset: 0x000357AF
		public float PenetrationDistance { get; set; }

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x000375B8 File Offset: 0x000357B8
		// (set) Token: 0x06000AC0 RID: 2752 RVA: 0x000375C0 File Offset: 0x000357C0
		public float PenetrationStrength { get; set; }

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x000375C9 File Offset: 0x000357C9
		// (set) Token: 0x06000AC2 RID: 2754 RVA: 0x000375D1 File Offset: 0x000357D1
		public bool CollideWithShooter { get; set; }

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x000375DA File Offset: 0x000357DA
		// (set) Token: 0x06000AC4 RID: 2756 RVA: 0x000375E2 File Offset: 0x000357E2
		public long[] IgnoreEntities { get; set; } = Array.Empty<long>();

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x000375EB File Offset: 0x000357EB
		// (set) Token: 0x06000AC6 RID: 2758 RVA: 0x000375F3 File Offset: 0x000357F3
		public long[] CandidateEntityIds { get; set; } = Array.Empty<long>();

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x000375FC File Offset: 0x000357FC
		// (set) Token: 0x06000AC8 RID: 2760 RVA: 0x00037604 File Offset: 0x00035804
		public int PacketVersion { get; set; }

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x0003760D File Offset: 0x0003580D
		// (set) Token: 0x06000ACA RID: 2762 RVA: 0x00037615 File Offset: 0x00035815
		public long ShooterId { get; set; }

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x0003761E File Offset: 0x0003581E
		// (set) Token: 0x06000ACC RID: 2764 RVA: 0x00037626 File Offset: 0x00035826
		public bool CheckAABBOnly { get; set; }

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x0003762F File Offset: 0x0003582F
		// (set) Token: 0x06000ACE RID: 2766 RVA: 0x00037637 File Offset: 0x00035837
		public bool RequireColliderWhenAvailable { get; set; }
	}
}
