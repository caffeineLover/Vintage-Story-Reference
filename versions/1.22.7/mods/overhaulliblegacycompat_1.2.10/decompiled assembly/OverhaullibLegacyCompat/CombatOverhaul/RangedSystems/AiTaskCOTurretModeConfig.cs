using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000E0 RID: 224
	[NullableContext(2)]
	[Nullable(0)]
	[JsonObject(MemberSerialization = 1)]
	public class AiTaskCOTurretModeConfig : AiTaskTurretModeConfig
	{
		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x0003AC21 File Offset: 0x00038E21
		// (set) Token: 0x06000B65 RID: 2917 RVA: 0x0003AC29 File Offset: 0x00038E29
		[JsonProperty]
		public int ProjectilesNumber { get; set; } = 1;

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000B66 RID: 2918 RVA: 0x0003AC32 File Offset: 0x00038E32
		// (set) Token: 0x06000B67 RID: 2919 RVA: 0x0003AC3A File Offset: 0x00038E3A
		[JsonProperty]
		public float ProjectileDamageMultiplier { get; set; } = 1f;

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000B68 RID: 2920 RVA: 0x0003AC43 File Offset: 0x00038E43
		// (set) Token: 0x06000B69 RID: 2921 RVA: 0x0003AC4B File Offset: 0x00038E4B
		[JsonProperty]
		public float DispersionReductionRate { get; set; } = 1f;

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000B6A RID: 2922 RVA: 0x0003AC54 File Offset: 0x00038E54
		// (set) Token: 0x06000B6B RID: 2923 RVA: 0x0003AC5C File Offset: 0x00038E5C
		[JsonProperty]
		public float MinDistanceToRezero { get; set; } = 2f;

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x0003AC65 File Offset: 0x00038E65
		// (set) Token: 0x06000B6D RID: 2925 RVA: 0x0003AC6D File Offset: 0x00038E6D
		[JsonProperty]
		public float MaxDistanceToRezero { get; set; } = 10f;

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000B6E RID: 2926 RVA: 0x0003AC76 File Offset: 0x00038E76
		// (set) Token: 0x06000B6F RID: 2927 RVA: 0x0003AC7E File Offset: 0x00038E7E
		public ItemStack ProjectileStack { get; protected set; }

		// Token: 0x06000B70 RID: 2928 RVA: 0x0003AC88 File Offset: 0x00038E88
		[NullableContext(1)]
		public override void Init(EntityAgent entity)
		{
			base.Init(entity);
			if (this.projectileStack != null && this.projectileStack.Resolve(entity.Api.World, ""))
			{
				this.ProjectileStack = this.projectileStack.ResolvedItemstack;
				this.projectileStack = null;
			}
		}

		// Token: 0x0400054E RID: 1358
		[JsonProperty]
		private JsonItemStack projectileStack;
	}
}
