using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.WeaponBuffs
{
	// Token: 0x020000A5 RID: 165
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class WeaponBuffQueryContext
	{
		// Token: 0x060008B7 RID: 2231 RVA: 0x00030BD6 File Offset: 0x0002EDD6
		public WeaponBuffQueryContext([Nullable(1)] ItemStack stack, [Nullable(1)] string usage, ItemSlot slot = null, Entity actor = null, Entity target = null)
		{
			this.Stack = stack;
			this.Usage = usage;
			this.Slot = slot;
			this.Actor = actor;
			this.Target = target;
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x00030C03 File Offset: 0x0002EE03
		[Nullable(1)]
		public ItemStack Stack { [NullableContext(1)] get; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x00030C0B File Offset: 0x0002EE0B
		[Nullable(1)]
		public string Usage { [NullableContext(1)] get; }

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x00030C13 File Offset: 0x0002EE13
		public ItemSlot Slot { get; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x00030C1B File Offset: 0x0002EE1B
		public Entity Actor { get; }

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x00030C23 File Offset: 0x0002EE23
		public Entity Target { get; }
	}
}
