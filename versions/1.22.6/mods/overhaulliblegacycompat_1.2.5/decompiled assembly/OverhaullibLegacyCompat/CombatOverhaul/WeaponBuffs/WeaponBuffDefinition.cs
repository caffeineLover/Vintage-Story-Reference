using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.WeaponBuffs
{
	// Token: 0x020000A2 RID: 162
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WeaponBuffDefinition
	{
		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x00030899 File Offset: 0x0002EA99
		// (set) Token: 0x06000879 RID: 2169 RVA: 0x000308A1 File Offset: 0x0002EAA1
		public string Code { get; set; } = "";

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x000308AA File Offset: 0x0002EAAA
		// (set) Token: 0x0600087B RID: 2171 RVA: 0x000308B2 File Offset: 0x0002EAB2
		public string SourceModId { get; set; } = "";

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x000308BB File Offset: 0x0002EABB
		// (set) Token: 0x0600087D RID: 2173 RVA: 0x000308C3 File Offset: 0x0002EAC3
		public string DisplayName { get; set; } = "";

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x000308CC File Offset: 0x0002EACC
		// (set) Token: 0x0600087F RID: 2175 RVA: 0x000308D4 File Offset: 0x0002EAD4
		public string DisplayNameLangCode { get; set; } = "";

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x000308DD File Offset: 0x0002EADD
		// (set) Token: 0x06000881 RID: 2177 RVA: 0x000308E5 File Offset: 0x0002EAE5
		public string Description { get; set; } = "";

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x000308EE File Offset: 0x0002EAEE
		// (set) Token: 0x06000883 RID: 2179 RVA: 0x000308F6 File Offset: 0x0002EAF6
		public string DescriptionLangCode { get; set; } = "";

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x000308FF File Offset: 0x0002EAFF
		// (set) Token: 0x06000885 RID: 2181 RVA: 0x00030907 File Offset: 0x0002EB07
		public double? DurationHours { get; set; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x00030910 File Offset: 0x0002EB10
		// (set) Token: 0x06000887 RID: 2183 RVA: 0x00030918 File Offset: 0x0002EB18
		public int? Uses { get; set; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x00030921 File Offset: 0x0002EB21
		// (set) Token: 0x06000889 RID: 2185 RVA: 0x00030929 File Offset: 0x0002EB29
		public int Priority { get; set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x00030932 File Offset: 0x0002EB32
		// (set) Token: 0x0600088B RID: 2187 RVA: 0x0003093A File Offset: 0x0002EB3A
		public List<WeaponStatModifier> Modifiers { get; set; } = new List<WeaponStatModifier>();

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x00030943 File Offset: 0x0002EB43
		// (set) Token: 0x0600088D RID: 2189 RVA: 0x0003094B File Offset: 0x0002EB4B
		public List<WeaponBuffConsumptionTrigger> ConsumeOn { get; set; } = new List<WeaponBuffConsumptionTrigger>();

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x00030954 File Offset: 0x0002EB54
		// (set) Token: 0x0600088F RID: 2191 RVA: 0x0003095C File Offset: 0x0002EB5C
		[Nullable(2)]
		public ITreeAttribute Data { [NullableContext(2)] get; [NullableContext(2)] set; }
	}
}
