using System;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using Vintagestory.API.Common;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200010C RID: 268
	public class DirectionalTypedDamageSource : DamageSource, ILocationalDamage, ITypedDamage, IWeaponDamageSource, IArmorPiercing
	{
		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000D39 RID: 3385 RVA: 0x00040BC2 File Offset: 0x0003EDC2
		// (set) Token: 0x06000D3A RID: 3386 RVA: 0x00040BCA File Offset: 0x0003EDCA
		public Vector3d Position { get; set; }

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000D3B RID: 3387 RVA: 0x00040BD3 File Offset: 0x0003EDD3
		// (set) Token: 0x06000D3C RID: 3388 RVA: 0x00040BDB File Offset: 0x0003EDDB
		[Nullable(1)]
		public string Collider { [NullableContext(1)] get; [NullableContext(1)] set; } = "";

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000D3D RID: 3389 RVA: 0x00040BE4 File Offset: 0x0003EDE4
		// (set) Token: 0x06000D3E RID: 3390 RVA: 0x00040BEC File Offset: 0x0003EDEC
		public DamageData DamageTypeData { get; set; }

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000D3F RID: 3391 RVA: 0x00040BF5 File Offset: 0x0003EDF5
		// (set) Token: 0x06000D40 RID: 3392 RVA: 0x00040BFD File Offset: 0x0003EDFD
		[Nullable(2)]
		public ItemStack Weapon { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000D41 RID: 3393 RVA: 0x00040C06 File Offset: 0x0003EE06
		public int ArmorPiercingTier
		{
			get
			{
				return this.DamageTypeData.ArmorPiercingTier;
			}
		}
	}
}
