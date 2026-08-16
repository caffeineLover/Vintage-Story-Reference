using System;
using System.Runtime.CompilerServices;
using System.Text;
using Vintagestory.API.Common;

namespace CombatOverhaul.WeaponBuffs
{
	// Token: 0x020000A8 RID: 168
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WeaponBuffTooltipContext
	{
		// Token: 0x060008CD RID: 2253 RVA: 0x00030CCF File Offset: 0x0002EECF
		public WeaponBuffTooltipContext(ItemStack stack, StringBuilder description, IWorldAccessor world, bool withDebugInfo)
		{
			this.Stack = stack;
			this.Description = description;
			this.World = world;
			this.WithDebugInfo = withDebugInfo;
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x00030CF4 File Offset: 0x0002EEF4
		public ItemStack Stack { get; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x00030CFC File Offset: 0x0002EEFC
		public StringBuilder Description { get; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x00030D04 File Offset: 0x0002EF04
		public IWorldAccessor World { get; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x00030D0C File Offset: 0x0002EF0C
		public bool WithDebugInfo { get; }
	}
}
