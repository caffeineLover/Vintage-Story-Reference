using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000EB RID: 235
	[NullableContext(1)]
	[Nullable(0)]
	public class MeleeAttackStats
	{
		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x0003D5D8 File Offset: 0x0003B7D8
		// (set) Token: 0x06000C0B RID: 3083 RVA: 0x0003D5E0 File Offset: 0x0003B7E0
		public bool StopOnTerrainHit { get; set; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x0003D5E9 File Offset: 0x0003B7E9
		// (set) Token: 0x06000C0D RID: 3085 RVA: 0x0003D5F1 File Offset: 0x0003B7F1
		public bool StopOnEntityHit { get; set; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000C0E RID: 3086 RVA: 0x0003D5FA File Offset: 0x0003B7FA
		// (set) Token: 0x06000C0F RID: 3087 RVA: 0x0003D602 File Offset: 0x0003B802
		public bool CollideWithTerrain { get; set; } = true;

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000C10 RID: 3088 RVA: 0x0003D60B File Offset: 0x0003B80B
		// (set) Token: 0x06000C11 RID: 3089 RVA: 0x0003D613 File Offset: 0x0003B813
		public bool HitOnlyOneEntity { get; set; }

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000C12 RID: 3090 RVA: 0x0003D61C File Offset: 0x0003B81C
		// (set) Token: 0x06000C13 RID: 3091 RVA: 0x0003D624 File Offset: 0x0003B824
		public float MaxReach { get; set; } = 6f;

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000C14 RID: 3092 RVA: 0x0003D62D File Offset: 0x0003B82D
		// (set) Token: 0x06000C15 RID: 3093 RVA: 0x0003D635 File Offset: 0x0003B835
		[Nullable(2)]
		public ImpaleAttackStats Impale { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000C16 RID: 3094 RVA: 0x0003D63E File Offset: 0x0003B83E
		// (set) Token: 0x06000C17 RID: 3095 RVA: 0x0003D646 File Offset: 0x0003B846
		public MeleeDamageTypeJson[] DamageTypes { get; set; } = Array.Empty<MeleeDamageTypeJson>();
	}
}
