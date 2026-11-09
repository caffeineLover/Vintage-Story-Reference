using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000CF RID: 207
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class RangedDamageResolvedEventArgs
	{
		// Token: 0x06000A91 RID: 2705 RVA: 0x000373C3 File Offset: 0x000355C3
		public RangedDamageResolvedEventArgs([Nullable(1)] Entity target, [Nullable(1)] DamageSource damageSource, ItemStack weaponStack, ItemStack projectileStack, float damage, bool damageReceived, DamageBlockEventArgs block)
		{
			this.Target = target;
			this.DamageSource = damageSource;
			this.WeaponStack = weaponStack;
			this.ProjectileStack = projectileStack;
			this.Damage = damage;
			this.DamageReceived = damageReceived;
			this.Block = block;
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x00037400 File Offset: 0x00035600
		[Nullable(1)]
		public Entity Target { [NullableContext(1)] get; }

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000A93 RID: 2707 RVA: 0x00037408 File Offset: 0x00035608
		[Nullable(1)]
		public DamageSource DamageSource { [NullableContext(1)] get; }

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x00037410 File Offset: 0x00035610
		public ItemStack WeaponStack { get; }

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000A95 RID: 2709 RVA: 0x00037418 File Offset: 0x00035618
		public ItemStack ProjectileStack { get; }

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x00037420 File Offset: 0x00035620
		public float Damage { get; }

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000A97 RID: 2711 RVA: 0x00037428 File Offset: 0x00035628
		public bool DamageReceived { get; }

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x00037430 File Offset: 0x00035630
		public DamageBlockEventArgs Block { get; }

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000A99 RID: 2713 RVA: 0x00037438 File Offset: 0x00035638
		public bool WasBlocked
		{
			get
			{
				return this.Block != null;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000A9A RID: 2714 RVA: 0x00037443 File Offset: 0x00035643
		public bool WasParried
		{
			get
			{
				DamageBlockEventArgs block = this.Block;
				return block != null && block.Kind == EnumDamageBlockKind.Parry;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000A9B RID: 2715 RVA: 0x00037459 File Offset: 0x00035659
		public bool IsLegalHit
		{
			get
			{
				return this.DamageReceived && this.Block == null;
			}
		}
	}
}
