using System;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using Vintagestory.API.Common;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000117 RID: 279
	public class DirectionalTypedDamageSource : DamageSource, ILocationalDamage, ITypedDamage, IWeaponDamageSource, IArmorPiercing
	{
		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000D95 RID: 3477 RVA: 0x00041BE6 File Offset: 0x0003FDE6
		// (set) Token: 0x06000D96 RID: 3478 RVA: 0x00041BEE File Offset: 0x0003FDEE
		public Vector3d Position { get; set; }

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000D97 RID: 3479 RVA: 0x00041BF7 File Offset: 0x0003FDF7
		// (set) Token: 0x06000D98 RID: 3480 RVA: 0x00041BFF File Offset: 0x0003FDFF
		[Nullable(1)]
		public string Collider { [NullableContext(1)] get; [NullableContext(1)] set; } = "";

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000D99 RID: 3481 RVA: 0x00041C08 File Offset: 0x0003FE08
		// (set) Token: 0x06000D9A RID: 3482 RVA: 0x00041C10 File Offset: 0x0003FE10
		public DamageData DamageTypeData { get; set; }

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000D9B RID: 3483 RVA: 0x00041C19 File Offset: 0x0003FE19
		// (set) Token: 0x06000D9C RID: 3484 RVA: 0x00041C21 File Offset: 0x0003FE21
		[Nullable(2)]
		public ItemStack Weapon { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000D9D RID: 3485 RVA: 0x00041C2A File Offset: 0x0003FE2A
		public int ArmorPiercingTier
		{
			get
			{
				return this.DamageTypeData.ArmorPiercingTier;
			}
		}
	}
}
