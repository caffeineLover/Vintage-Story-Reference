using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.RangedSystems;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.WeaponBuffs
{
	// Token: 0x020000A7 RID: 167
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WeaponBuffProjectileSpawnContext
	{
		// Token: 0x060008C5 RID: 2245 RVA: 0x00030C61 File Offset: 0x0002EE61
		public WeaponBuffProjectileSpawnContext(ProjectileStats projectileStats, ProjectileSpawnStats spawnStats, ItemStack projectileStack, [Nullable(2)] ItemStack weaponStack, Entity shooter, Entity target)
		{
			this.ProjectileStats = projectileStats;
			this.SpawnStats = spawnStats;
			this.ProjectileStack = projectileStack;
			this.WeaponStack = weaponStack;
			this.Shooter = shooter;
			this.Target = target;
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x00030C96 File Offset: 0x0002EE96
		public ProjectileStats ProjectileStats { get; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x00030C9E File Offset: 0x0002EE9E
		// (set) Token: 0x060008C8 RID: 2248 RVA: 0x00030CA6 File Offset: 0x0002EEA6
		public ProjectileSpawnStats SpawnStats { get; set; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x00030CAF File Offset: 0x0002EEAF
		public ItemStack ProjectileStack { get; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x00030CB7 File Offset: 0x0002EEB7
		[Nullable(2)]
		public ItemStack WeaponStack { [NullableContext(2)] get; }

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x00030CBF File Offset: 0x0002EEBF
		public Entity Shooter { get; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x00030CC7 File Offset: 0x0002EEC7
		public Entity Target { get; }
	}
}
