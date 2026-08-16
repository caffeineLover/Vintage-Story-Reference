using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.MeleeSystems;

namespace Firearms
{
	// Token: 0x0200001A RID: 26
	[NullableContext(1)]
	[Nullable(0)]
	public class MusketStats
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000CE RID: 206 RVA: 0x000051D1 File Offset: 0x000033D1
		// (set) Token: 0x060000CF RID: 207 RVA: 0x000051D9 File Offset: 0x000033D9
		public string BayonetWildcard { get; set; } = "*bayonet-*";

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x000051E2 File Offset: 0x000033E2
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x000051EA File Offset: 0x000033EA
		public MeleeAttackStats BayonetAttack { get; set; } = new MeleeAttackStats();

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x000051F3 File Offset: 0x000033F3
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x000051FB File Offset: 0x000033FB
		public string BayonetAttackAnimation { get; set; } = "";

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00005204 File Offset: 0x00003404
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x0000520C File Offset: 0x0000340C
		public string BayonetAttackTpAnimation { get; set; } = "";

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00005215 File Offset: 0x00003415
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x0000521D File Offset: 0x0000341D
		public float AnimationStaggerOnHitDurationMs { get; set; } = 100f;
	}
}
