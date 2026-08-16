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
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x00030A49 File Offset: 0x0002EC49
		// (set) Token: 0x0600089D RID: 2205 RVA: 0x00030A51 File Offset: 0x0002EC51
		public string InstanceId { get; set; } = "";

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x00030A5A File Offset: 0x0002EC5A
		// (set) Token: 0x0600089F RID: 2207 RVA: 0x00030A62 File Offset: 0x0002EC62
		public string Code { get; set; } = "";

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x00030A6B File Offset: 0x0002EC6B
		// (set) Token: 0x060008A1 RID: 2209 RVA: 0x00030A73 File Offset: 0x0002EC73
		public string SourceModId { get; set; } = "";

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x00030A7C File Offset: 0x0002EC7C
		// (set) Token: 0x060008A3 RID: 2211 RVA: 0x00030A84 File Offset: 0x0002EC84
		public string DisplayName { get; set; } = "";

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00030A8D File Offset: 0x0002EC8D
		// (set) Token: 0x060008A5 RID: 2213 RVA: 0x00030A95 File Offset: 0x0002EC95
		public string DisplayNameLangCode { get; set; } = "";

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x00030A9E File Offset: 0x0002EC9E
		// (set) Token: 0x060008A7 RID: 2215 RVA: 0x00030AA6 File Offset: 0x0002ECA6
		public string Description { get; set; } = "";

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00030AAF File Offset: 0x0002ECAF
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x00030AB7 File Offset: 0x0002ECB7
		public string DescriptionLangCode { get; set; } = "";

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x00030AC0 File Offset: 0x0002ECC0
		// (set) Token: 0x060008AB RID: 2219 RVA: 0x00030AC8 File Offset: 0x0002ECC8
		public double? ExpiresAtTotalHours { get; set; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x00030AD1 File Offset: 0x0002ECD1
		// (set) Token: 0x060008AD RID: 2221 RVA: 0x00030AD9 File Offset: 0x0002ECD9
		public int? UsesRemaining { get; set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x00030AE2 File Offset: 0x0002ECE2
		// (set) Token: 0x060008AF RID: 2223 RVA: 0x00030AEA File Offset: 0x0002ECEA
		public int Priority { get; set; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x00030AF3 File Offset: 0x0002ECF3
		// (set) Token: 0x060008B1 RID: 2225 RVA: 0x00030AFB File Offset: 0x0002ECFB
		public List<WeaponStatModifier> Modifiers { get; set; } = new List<WeaponStatModifier>();

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x00030B04 File Offset: 0x0002ED04
		// (set) Token: 0x060008B3 RID: 2227 RVA: 0x00030B0C File Offset: 0x0002ED0C
		public List<WeaponBuffConsumptionTrigger> ConsumeOn { get; set; } = new List<WeaponBuffConsumptionTrigger>();

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x00030B15 File Offset: 0x0002ED15
		// (set) Token: 0x060008B5 RID: 2229 RVA: 0x00030B1D File Offset: 0x0002ED1D
		[Nullable(2)]
		public ITreeAttribute Data { [NullableContext(2)] get; [NullableContext(2)] set; }
	}
}
