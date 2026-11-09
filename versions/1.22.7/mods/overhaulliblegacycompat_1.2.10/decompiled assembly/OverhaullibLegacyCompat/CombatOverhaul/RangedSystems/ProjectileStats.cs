using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000CC RID: 204
	[NullableContext(1)]
	[Nullable(0)]
	public class ProjectileStats
	{
		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x00036FA3 File Offset: 0x000351A3
		// (set) Token: 0x06000A60 RID: 2656 RVA: 0x00036FAB File Offset: 0x000351AB
		public int AdditionalDurabilityCost { get; set; }

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x00036FB4 File Offset: 0x000351B4
		// (set) Token: 0x06000A62 RID: 2658 RVA: 0x00036FBC File Offset: 0x000351BC
		public string ImpactSound { get; set; } = "game:sounds/arrow-impact";

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x00036FC5 File Offset: 0x000351C5
		// (set) Token: 0x06000A64 RID: 2660 RVA: 0x00036FCD File Offset: 0x000351CD
		public string HitSound { get; set; } = "game:sounds/player/projectilehit";

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x00036FD6 File Offset: 0x000351D6
		// (set) Token: 0x06000A66 RID: 2662 RVA: 0x00036FDE File Offset: 0x000351DE
		public float CollisionRadius { get; set; }

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x00036FE7 File Offset: 0x000351E7
		// (set) Token: 0x06000A68 RID: 2664 RVA: 0x00036FEF File Offset: 0x000351EF
		public float PenetrationDistance { get; set; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x00036FF8 File Offset: 0x000351F8
		// (set) Token: 0x06000A6A RID: 2666 RVA: 0x00037000 File Offset: 0x00035200
		public ProjectileDamageDataJson DamageStats { get; set; } = new ProjectileDamageDataJson();

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x00037009 File Offset: 0x00035209
		// (set) Token: 0x06000A6C RID: 2668 RVA: 0x00037011 File Offset: 0x00035211
		public int DamageTierBonus { get; set; }

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x0003701A File Offset: 0x0003521A
		// (set) Token: 0x06000A6E RID: 2670 RVA: 0x00037022 File Offset: 0x00035222
		public float SpeedThreshold { get; set; }

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x0003702B File Offset: 0x0003522B
		// (set) Token: 0x06000A70 RID: 2672 RVA: 0x00037033 File Offset: 0x00035233
		public float Knockback { get; set; }

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x0003703C File Offset: 0x0003523C
		// (set) Token: 0x06000A72 RID: 2674 RVA: 0x00037044 File Offset: 0x00035244
		public string EntityCode { get; set; } = "";

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x0003704D File Offset: 0x0003524D
		// (set) Token: 0x06000A74 RID: 2676 RVA: 0x00037055 File Offset: 0x00035255
		public int DurabilityDamage { get; set; }

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x0003705E File Offset: 0x0003525E
		// (set) Token: 0x06000A76 RID: 2678 RVA: 0x00037066 File Offset: 0x00035266
		public float DropChance { get; set; }

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x0003706F File Offset: 0x0003526F
		// (set) Token: 0x06000A78 RID: 2680 RVA: 0x00037077 File Offset: 0x00035277
		public float PenetrationBonus { get; set; }

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x00037080 File Offset: 0x00035280
		// (set) Token: 0x06000A7A RID: 2682 RVA: 0x00037088 File Offset: 0x00035288
		public bool CanBeCollected { get; set; } = true;

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x00037091 File Offset: 0x00035291
		// (set) Token: 0x06000A7C RID: 2684 RVA: 0x00037099 File Offset: 0x00035299
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

		// Token: 0x06000A7D RID: 2685 RVA: 0x000370A2 File Offset: 0x000352A2
		public ProjectileStats()
		{
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x000370E0 File Offset: 0x000352E0
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

		// Token: 0x06000A7F RID: 2687 RVA: 0x00037194 File Offset: 0x00035394
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
