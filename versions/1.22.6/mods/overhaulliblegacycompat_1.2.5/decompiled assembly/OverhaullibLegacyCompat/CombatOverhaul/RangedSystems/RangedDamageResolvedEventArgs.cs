using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000C4 RID: 196
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class RangedDamageResolvedEventArgs
	{
		// Token: 0x06000A35 RID: 2613 RVA: 0x000363D3 File Offset: 0x000345D3
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

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000A36 RID: 2614 RVA: 0x00036410 File Offset: 0x00034610
		[Nullable(1)]
		public Entity Target { [NullableContext(1)] get; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x00036418 File Offset: 0x00034618
		[Nullable(1)]
		public DamageSource DamageSource { [NullableContext(1)] get; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x00036420 File Offset: 0x00034620
		public ItemStack WeaponStack { get; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x00036428 File Offset: 0x00034628
		public ItemStack ProjectileStack { get; }

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x00036430 File Offset: 0x00034630
		public float Damage { get; }

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x00036438 File Offset: 0x00034638
		public bool DamageReceived { get; }

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x00036440 File Offset: 0x00034640
		public DamageBlockEventArgs Block { get; }

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x00036448 File Offset: 0x00034648
		public bool WasBlocked
		{
			get
			{
				return this.Block != null;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x00036453 File Offset: 0x00034653
		public bool WasParried
		{
			get
			{
				DamageBlockEventArgs block = this.Block;
				return block != null && block.Kind == EnumDamageBlockKind.Parry;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x00036469 File Offset: 0x00034669
		public bool IsLegalHit
		{
			get
			{
				return this.DamageReceived && this.Block == null;
			}
		}
	}
}
