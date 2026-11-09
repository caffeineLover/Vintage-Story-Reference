using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x02000104 RID: 260
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MeleeDamageResolvedEventArgs
	{
		// Token: 0x06000CFD RID: 3325 RVA: 0x0003FD3F File Offset: 0x0003DF3F
		public MeleeDamageResolvedEventArgs(Entity target, DamageSource damageSource, [Nullable(2)] ItemSlot slot, float damage, bool damageReceived, [Nullable(2)] DamageBlockEventArgs block)
		{
			this.Target = target;
			this.DamageSource = damageSource;
			this.Slot = slot;
			this.Damage = damage;
			this.DamageReceived = damageReceived;
			this.Block = block;
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x0003FD74 File Offset: 0x0003DF74
		public Entity Target { get; }

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000CFF RID: 3327 RVA: 0x0003FD7C File Offset: 0x0003DF7C
		public DamageSource DamageSource { get; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x0003FD84 File Offset: 0x0003DF84
		[Nullable(2)]
		public ItemSlot Slot { [NullableContext(2)] get; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x0003FD8C File Offset: 0x0003DF8C
		public float Damage { get; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x0003FD94 File Offset: 0x0003DF94
		public bool DamageReceived { get; }

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x0003FD9C File Offset: 0x0003DF9C
		[Nullable(2)]
		public DamageBlockEventArgs Block { [NullableContext(2)] get; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x0003FDA4 File Offset: 0x0003DFA4
		public bool WasBlocked
		{
			get
			{
				return this.Block != null;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x0003FDAF File Offset: 0x0003DFAF
		public bool WasParried
		{
			get
			{
				DamageBlockEventArgs block = this.Block;
				return block != null && block.Kind == EnumDamageBlockKind.Parry;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x0003FDC5 File Offset: 0x0003DFC5
		public bool IsLegalHit
		{
			get
			{
				return this.DamageReceived && this.Block == null;
			}
		}
	}
}
