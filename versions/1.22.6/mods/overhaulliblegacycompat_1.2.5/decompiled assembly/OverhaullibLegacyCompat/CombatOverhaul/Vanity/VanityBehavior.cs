using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Armor;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.Vanity
{
	// Token: 0x020000AF RID: 175
	[NullableContext(2)]
	[Nullable(0)]
	public class VanityBehavior : EntityBehavior
	{
		// Token: 0x06000954 RID: 2388 RVA: 0x000329D6 File Offset: 0x00030BD6
		[NullableContext(1)]
		public VanityBehavior(Entity entity) : base(entity)
		{
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x000329DF File Offset: 0x00030BDF
		[NullableContext(1)]
		public override string PropertyName()
		{
			return "VanityBehavior";
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x000329E6 File Offset: 0x00030BE6
		// (set) Token: 0x06000957 RID: 2391 RVA: 0x000329EE File Offset: 0x00030BEE
		public ArmorInventory VanityInventory { get; set; }
	}
}
