using System;
using OpenTK.Mathematics;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000CE RID: 206
	public struct ProjectileSpawnStats
	{
		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000A83 RID: 2691 RVA: 0x0003734A File Offset: 0x0003554A
		// (set) Token: 0x06000A84 RID: 2692 RVA: 0x00037352 File Offset: 0x00035552
		public long ProducerEntityId { readonly get; set; }

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000A85 RID: 2693 RVA: 0x0003735B File Offset: 0x0003555B
		// (set) Token: 0x06000A86 RID: 2694 RVA: 0x00037363 File Offset: 0x00035563
		public float DamageMultiplier { readonly get; set; }

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x0003736C File Offset: 0x0003556C
		// (set) Token: 0x06000A88 RID: 2696 RVA: 0x00037374 File Offset: 0x00035574
		public int DamageTier { readonly get; set; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x0003737D File Offset: 0x0003557D
		// (set) Token: 0x06000A8A RID: 2698 RVA: 0x00037385 File Offset: 0x00035585
		public Vector3d Position { readonly get; set; }

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000A8B RID: 2699 RVA: 0x0003738E File Offset: 0x0003558E
		// (set) Token: 0x06000A8C RID: 2700 RVA: 0x00037396 File Offset: 0x00035596
		public Vector3d Velocity { readonly get; set; }

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000A8D RID: 2701 RVA: 0x0003739F File Offset: 0x0003559F
		// (set) Token: 0x06000A8E RID: 2702 RVA: 0x000373A7 File Offset: 0x000355A7
		public float GravityFactorMultiplier { readonly get; set; }

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000A8F RID: 2703 RVA: 0x000373B0 File Offset: 0x000355B0
		// (set) Token: 0x06000A90 RID: 2704 RVA: 0x000373B9 File Offset: 0x000355B9
		[Obsolete("Use DamageTier instead")]
		public float DamageStrength
		{
			get
			{
				return (float)this.DamageTier;
			}
			set
			{
				this.DamageTier = (int)value;
			}
		}
	}
}
