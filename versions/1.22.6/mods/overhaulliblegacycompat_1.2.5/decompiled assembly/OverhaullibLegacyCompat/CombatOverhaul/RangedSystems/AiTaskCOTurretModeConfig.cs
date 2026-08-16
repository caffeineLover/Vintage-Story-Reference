using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000D5 RID: 213
	[NullableContext(2)]
	[Nullable(0)]
	[JsonObject(MemberSerialization = 1)]
	public class AiTaskCOTurretModeConfig : AiTaskTurretModeConfig
	{
		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x00039C31 File Offset: 0x00037E31
		// (set) Token: 0x06000B09 RID: 2825 RVA: 0x00039C39 File Offset: 0x00037E39
		[JsonProperty]
		public int ProjectilesNumber { get; set; } = 1;

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000B0A RID: 2826 RVA: 0x00039C42 File Offset: 0x00037E42
		// (set) Token: 0x06000B0B RID: 2827 RVA: 0x00039C4A File Offset: 0x00037E4A
		[JsonProperty]
		public float ProjectileDamageMultiplier { get; set; } = 1f;

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x00039C53 File Offset: 0x00037E53
		// (set) Token: 0x06000B0D RID: 2829 RVA: 0x00039C5B File Offset: 0x00037E5B
		[JsonProperty]
		public float DispersionReductionRate { get; set; } = 1f;

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000B0E RID: 2830 RVA: 0x00039C64 File Offset: 0x00037E64
		// (set) Token: 0x06000B0F RID: 2831 RVA: 0x00039C6C File Offset: 0x00037E6C
		[JsonProperty]
		public float MinDistanceToRezero { get; set; } = 2f;

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000B10 RID: 2832 RVA: 0x00039C75 File Offset: 0x00037E75
		// (set) Token: 0x06000B11 RID: 2833 RVA: 0x00039C7D File Offset: 0x00037E7D
		[JsonProperty]
		public float MaxDistanceToRezero { get; set; } = 10f;

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000B12 RID: 2834 RVA: 0x00039C86 File Offset: 0x00037E86
		// (set) Token: 0x06000B13 RID: 2835 RVA: 0x00039C8E File Offset: 0x00037E8E
		public ItemStack ProjectileStack { get; protected set; }

		// Token: 0x06000B14 RID: 2836 RVA: 0x00039C98 File Offset: 0x00037E98
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

		// Token: 0x0400052C RID: 1324
		[JsonProperty]
		private JsonItemStack projectileStack;
	}
}
