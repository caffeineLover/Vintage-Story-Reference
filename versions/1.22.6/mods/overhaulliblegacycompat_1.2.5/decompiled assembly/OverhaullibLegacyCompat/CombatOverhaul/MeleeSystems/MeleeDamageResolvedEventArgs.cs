using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000F9 RID: 249
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MeleeDamageResolvedEventArgs
	{
		// Token: 0x06000CA1 RID: 3233 RVA: 0x0003ED4F File Offset: 0x0003CF4F
		public MeleeDamageResolvedEventArgs(Entity target, DamageSource damageSource, [Nullable(2)] ItemSlot slot, float damage, bool damageReceived, [Nullable(2)] DamageBlockEventArgs block)
		{
			this.Target = target;
			this.DamageSource = damageSource;
			this.Slot = slot;
			this.Damage = damage;
			this.DamageReceived = damageReceived;
			this.Block = block;
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x0003ED84 File Offset: 0x0003CF84
		public Entity Target { get; }

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x0003ED8C File Offset: 0x0003CF8C
		public DamageSource DamageSource { get; }

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x0003ED94 File Offset: 0x0003CF94
		[Nullable(2)]
		public ItemSlot Slot { [NullableContext(2)] get; }

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x0003ED9C File Offset: 0x0003CF9C
		public float Damage { get; }

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x0003EDA4 File Offset: 0x0003CFA4
		public bool DamageReceived { get; }

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x0003EDAC File Offset: 0x0003CFAC
		[Nullable(2)]
		public DamageBlockEventArgs Block { [NullableContext(2)] get; }

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x0003EDB4 File Offset: 0x0003CFB4
		public bool WasBlocked
		{
			get
			{
				return this.Block != null;
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x0003EDBF File Offset: 0x0003CFBF
		public bool WasParried
		{
			get
			{
				DamageBlockEventArgs block = this.Block;
				return block != null && block.Kind == EnumDamageBlockKind.Parry;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x0003EDD5 File Offset: 0x0003CFD5
		public bool IsLegalHit
		{
			get
			{
				return this.DamageReceived && this.Block == null;
			}
		}
	}
}
