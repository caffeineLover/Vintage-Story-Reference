using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.WeaponBuffs
{
	// Token: 0x020000A4 RID: 164
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WeaponBuffInstance
	{
		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x00030A81 File Offset: 0x0002EC81
		// (set) Token: 0x0600089D RID: 2205 RVA: 0x00030A89 File Offset: 0x0002EC89
		public string InstanceId { get; set; } = "";

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x00030A92 File Offset: 0x0002EC92
		// (set) Token: 0x0600089F RID: 2207 RVA: 0x00030A9A File Offset: 0x0002EC9A
		public string Code { get; set; } = "";

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x00030AA3 File Offset: 0x0002ECA3
		// (set) Token: 0x060008A1 RID: 2209 RVA: 0x00030AAB File Offset: 0x0002ECAB
		public string SourceModId { get; set; } = "";

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x00030AB4 File Offset: 0x0002ECB4
		// (set) Token: 0x060008A3 RID: 2211 RVA: 0x00030ABC File Offset: 0x0002ECBC
		public string DisplayName { get; set; } = "";

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00030AC5 File Offset: 0x0002ECC5
		// (set) Token: 0x060008A5 RID: 2213 RVA: 0x00030ACD File Offset: 0x0002ECCD
		public string DisplayNameLangCode { get; set; } = "";

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x00030AD6 File Offset: 0x0002ECD6
		// (set) Token: 0x060008A7 RID: 2215 RVA: 0x00030ADE File Offset: 0x0002ECDE
		public string Description { get; set; } = "";

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00030AE7 File Offset: 0x0002ECE7
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x00030AEF File Offset: 0x0002ECEF
		public string DescriptionLangCode { get; set; } = "";

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x00030AF8 File Offset: 0x0002ECF8
		// (set) Token: 0x060008AB RID: 2219 RVA: 0x00030B00 File Offset: 0x0002ED00
		public double? ExpiresAtTotalHours { get; set; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x00030B09 File Offset: 0x0002ED09
		// (set) Token: 0x060008AD RID: 2221 RVA: 0x00030B11 File Offset: 0x0002ED11
		public int? UsesRemaining { get; set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x00030B1A File Offset: 0x0002ED1A
		// (set) Token: 0x060008AF RID: 2223 RVA: 0x00030B22 File Offset: 0x0002ED22
		public int Priority { get; set; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x00030B2B File Offset: 0x0002ED2B
		// (set) Token: 0x060008B1 RID: 2225 RVA: 0x00030B33 File Offset: 0x0002ED33
		public List<WeaponStatModifier> Modifiers { get; set; } = new List<WeaponStatModifier>();

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x00030B3C File Offset: 0x0002ED3C
		// (set) Token: 0x060008B3 RID: 2227 RVA: 0x00030B44 File Offset: 0x0002ED44
		public List<WeaponBuffConsumptionTrigger> ConsumeOn { get; set; } = new List<WeaponBuffConsumptionTrigger>();

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x00030B4D File Offset: 0x0002ED4D
		// (set) Token: 0x060008B5 RID: 2229 RVA: 0x00030B55 File Offset: 0x0002ED55
		[Nullable(2)]
		public ITreeAttribute Data { [NullableContext(2)] get; [NullableContext(2)] set; }
	}
}
