using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000C1 RID: 193
	[NullableContext(1)]
	[Nullable(0)]
	public class ProjectileStats
	{
		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000A03 RID: 2563 RVA: 0x00035FB5 File Offset: 0x000341B5
		// (set) Token: 0x06000A04 RID: 2564 RVA: 0x00035FBD File Offset: 0x000341BD
		public int AdditionalDurabilityCost { get; set; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x00035FC6 File Offset: 0x000341C6
		// (set) Token: 0x06000A06 RID: 2566 RVA: 0x00035FCE File Offset: 0x000341CE
		public string ImpactSound { get; set; } = "game:sounds/arrow-impact";

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x00035FD7 File Offset: 0x000341D7
		// (set) Token: 0x06000A08 RID: 2568 RVA: 0x00035FDF File Offset: 0x000341DF
		public string HitSound { get; set; } = "game:sounds/player/projectilehit";

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000A09 RID: 2569 RVA: 0x00035FE8 File Offset: 0x000341E8
		// (set) Token: 0x06000A0A RID: 2570 RVA: 0x00035FF0 File Offset: 0x000341F0
		public float CollisionRadius { get; set; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000A0B RID: 2571 RVA: 0x00035FF9 File Offset: 0x000341F9
		// (set) Token: 0x06000A0C RID: 2572 RVA: 0x00036001 File Offset: 0x00034201
		public float PenetrationDistance { get; set; }

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000A0D RID: 2573 RVA: 0x0003600A File Offset: 0x0003420A
		// (set) Token: 0x06000A0E RID: 2574 RVA: 0x00036012 File Offset: 0x00034212
		public ProjectileDamageDataJson DamageStats { get; set; } = new ProjectileDamageDataJson();

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000A0F RID: 2575 RVA: 0x0003601B File Offset: 0x0003421B
		// (set) Token: 0x06000A10 RID: 2576 RVA: 0x00036023 File Offset: 0x00034223
		public int DamageTierBonus { get; set; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000A11 RID: 2577 RVA: 0x0003602C File Offset: 0x0003422C
		// (set) Token: 0x06000A12 RID: 2578 RVA: 0x00036034 File Offset: 0x00034234
		public float SpeedThreshold { get; set; }

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000A13 RID: 2579 RVA: 0x0003603D File Offset: 0x0003423D
		// (set) Token: 0x06000A14 RID: 2580 RVA: 0x00036045 File Offset: 0x00034245
		public float Knockback { get; set; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0003604E File Offset: 0x0003424E
		// (set) Token: 0x06000A16 RID: 2582 RVA: 0x00036056 File Offset: 0x00034256
		public string EntityCode { get; set; } = "";

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x0003605F File Offset: 0x0003425F
		// (set) Token: 0x06000A18 RID: 2584 RVA: 0x00036067 File Offset: 0x00034267
		public int DurabilityDamage { get; set; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x00036070 File Offset: 0x00034270
		// (set) Token: 0x06000A1A RID: 2586 RVA: 0x00036078 File Offset: 0x00034278
		public float DropChance { get; set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000A1B RID: 2587 RVA: 0x00036081 File Offset: 0x00034281
		// (set) Token: 0x06000A1C RID: 2588 RVA: 0x00036089 File Offset: 0x00034289
		public float PenetrationBonus { get; set; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x00036092 File Offset: 0x00034292
		// (set) Token: 0x06000A1E RID: 2590 RVA: 0x0003609A File Offset: 0x0003429A
		public bool CanBeCollected { get; set; } = true;

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x000360A3 File Offset: 0x000342A3
		// (set) Token: 0x06000A20 RID: 2592 RVA: 0x000360AB File Offset: 0x000342AB
		[Nullable(new byte[]
		{
			2,
			1,
			1
		})]
		public Dictionary<string, ProjectileDamageDataJson> DamageStatsByType { [return: Nullable(new byte[]
		{
			2,
			1,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1,
			1
		})] set; }

		// Token: 0x06000A21 RID: 2593 RVA: 0x000360B4 File Offset: 0x000342B4
		public ProjectileStats()
		{
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x000360F0 File Offset: 0x000342F0
		public ProjectileStats(int additionalDurabilityCost, string impactSound, string hitSound, float collisionRadius, float penetrationDistance, ProjectileDamageDataJson damageStats, int damageTierBonus, float speedThreshold, float knockback, string entityCode, int durabilityDamage, float dropChance, float penetrationBonus, bool canBeCollected)
		{
			this.AdditionalDurabilityCost = additionalDurabilityCost;
			this.ImpactSound = impactSound;
			this.HitSound = hitSound;
			this.CollisionRadius = collisionRadius;
			this.PenetrationDistance = penetrationDistance;
			this.DamageStats = damageStats;
			this.DamageTierBonus = damageTierBonus;
			this.SpeedThreshold = speedThreshold;
			this.Knockback = knockback;
			this.EntityCode = entityCode;
			this.DurabilityDamage = durabilityDamage;
			this.DropChance = dropChance;
			this.PenetrationBonus = penetrationBonus;
			this.CanBeCollected = canBeCollected;
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x000361A4 File Offset: 0x000343A4
		public ProjectileStats Clone()
		{
			return new ProjectileStats(this.AdditionalDurabilityCost, this.ImpactSound, this.HitSound, this.CollisionRadius, this.PenetrationDistance, new ProjectileDamageDataJson
			{
				Damage = this.DamageStats.Damage,
				DamageType = this.DamageStats.DamageType
			}, this.DamageTierBonus, this.SpeedThreshold, this.Knockback, this.EntityCode, this.DurabilityDamage, this.DropChance, this.PenetrationBonus, this.CanBeCollected);
		}
	}
}
