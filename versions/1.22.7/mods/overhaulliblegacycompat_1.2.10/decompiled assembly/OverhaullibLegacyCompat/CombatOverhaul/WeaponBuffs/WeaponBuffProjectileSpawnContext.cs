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
		// Token: 0x060008C5 RID: 2245 RVA: 0x00030C99 File Offset: 0x0002EE99
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
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x00030CCE File Offset: 0x0002EECE
		public ProjectileStats ProjectileStats { get; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x00030CD6 File Offset: 0x0002EED6
		// (set) Token: 0x060008C8 RID: 2248 RVA: 0x00030CDE File Offset: 0x0002EEDE
		public ProjectileSpawnStats SpawnStats { get; set; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x00030CE7 File Offset: 0x0002EEE7
		public ItemStack ProjectileStack { get; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x00030CEF File Offset: 0x0002EEEF
		[Nullable(2)]
		public ItemStack WeaponStack { [NullableContext(2)] get; }

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x00030CF7 File Offset: 0x0002EEF7
		public Entity Shooter { get; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x00030CFF File Offset: 0x0002EEFF
		public Entity Target { get; }
	}
}
