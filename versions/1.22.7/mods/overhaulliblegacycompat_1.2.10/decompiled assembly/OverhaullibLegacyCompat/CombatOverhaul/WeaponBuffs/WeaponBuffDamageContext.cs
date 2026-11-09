using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.WeaponBuffs
{
	// Token: 0x020000A6 RID: 166
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class WeaponBuffDamageContext
	{
		// Token: 0x060008BD RID: 2237 RVA: 0x00030C2B File Offset: 0x0002EE2B
		public WeaponBuffDamageContext([Nullable(1)] Entity target, [Nullable(1)] DamageSource damageSource, ItemSlot slot, ItemStack weaponStack, float damage, ItemStack projectileStack = null)
		{
			this.Target = target;
			this.DamageSource = damageSource;
			this.Slot = slot;
			this.WeaponStack = weaponStack;
			this.ProjectileStack = projectileStack;
			this.Damage = damage;
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x00030C60 File Offset: 0x0002EE60
		[Nullable(1)]
		public Entity Target { [NullableContext(1)] get; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x00030C68 File Offset: 0x0002EE68
		[Nullable(1)]
		public DamageSource DamageSource { [NullableContext(1)] get; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x00030C70 File Offset: 0x0002EE70
		public ItemSlot Slot { get; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x00030C78 File Offset: 0x0002EE78
		public ItemStack WeaponStack { get; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x00030C80 File Offset: 0x0002EE80
		public ItemStack ProjectileStack { get; }

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x00030C88 File Offset: 0x0002EE88
		// (set) Token: 0x060008C4 RID: 2244 RVA: 0x00030C90 File Offset: 0x0002EE90
		public float Damage { get; set; }
	}
}
