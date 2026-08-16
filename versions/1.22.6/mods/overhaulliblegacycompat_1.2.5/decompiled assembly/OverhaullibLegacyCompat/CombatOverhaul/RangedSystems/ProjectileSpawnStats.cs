using System;
using OpenTK.Mathematics;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000C3 RID: 195
	public struct ProjectileSpawnStats
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000A27 RID: 2599 RVA: 0x0003635A File Offset: 0x0003455A
		// (set) Token: 0x06000A28 RID: 2600 RVA: 0x00036362 File Offset: 0x00034562
		public long ProducerEntityId { readonly get; set; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000A29 RID: 2601 RVA: 0x0003636B File Offset: 0x0003456B
		// (set) Token: 0x06000A2A RID: 2602 RVA: 0x00036373 File Offset: 0x00034573
		public float DamageMultiplier { readonly get; set; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000A2B RID: 2603 RVA: 0x0003637C File Offset: 0x0003457C
		// (set) Token: 0x06000A2C RID: 2604 RVA: 0x00036384 File Offset: 0x00034584
		public int DamageTier { readonly get; set; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000A2D RID: 2605 RVA: 0x0003638D File Offset: 0x0003458D
		// (set) Token: 0x06000A2E RID: 2606 RVA: 0x00036395 File Offset: 0x00034595
		public Vector3d Position { readonly get; set; }

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x0003639E File Offset: 0x0003459E
		// (set) Token: 0x06000A30 RID: 2608 RVA: 0x000363A6 File Offset: 0x000345A6
		public Vector3d Velocity { readonly get; set; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x000363AF File Offset: 0x000345AF
		// (set) Token: 0x06000A32 RID: 2610 RVA: 0x000363B7 File Offset: 0x000345B7
		public float GravityFactorMultiplier { readonly get; set; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x000363C0 File Offset: 0x000345C0
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x000363C9 File Offset: 0x000345C9
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
