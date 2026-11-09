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
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x000308D1 File Offset: 0x0002EAD1
		// (set) Token: 0x06000879 RID: 2169 RVA: 0x000308D9 File Offset: 0x0002EAD9
		public string Code { get; set; } = "";

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x000308E2 File Offset: 0x0002EAE2
		// (set) Token: 0x0600087B RID: 2171 RVA: 0x000308EA File Offset: 0x0002EAEA
		public string SourceModId { get; set; } = "";

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x000308F3 File Offset: 0x0002EAF3
		// (set) Token: 0x0600087D RID: 2173 RVA: 0x000308FB File Offset: 0x0002EAFB
		public string DisplayName { get; set; } = "";

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x00030904 File Offset: 0x0002EB04
		// (set) Token: 0x0600087F RID: 2175 RVA: 0x0003090C File Offset: 0x0002EB0C
		public string DisplayNameLangCode { get; set; } = "";

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x00030915 File Offset: 0x0002EB15
		// (set) Token: 0x06000881 RID: 2177 RVA: 0x0003091D File Offset: 0x0002EB1D
		public string Description { get; set; } = "";

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x00030926 File Offset: 0x0002EB26
		// (set) Token: 0x06000883 RID: 2179 RVA: 0x0003092E File Offset: 0x0002EB2E
		public string DescriptionLangCode { get; set; } = "";

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x00030937 File Offset: 0x0002EB37
		// (set) Token: 0x06000885 RID: 2181 RVA: 0x0003093F File Offset: 0x0002EB3F
		public double? DurationHours { get; set; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x00030948 File Offset: 0x0002EB48
		// (set) Token: 0x06000887 RID: 2183 RVA: 0x00030950 File Offset: 0x0002EB50
		public int? Uses { get; set; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x00030959 File Offset: 0x0002EB59
		// (set) Token: 0x06000889 RID: 2185 RVA: 0x00030961 File Offset: 0x0002EB61
		public int Priority { get; set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x0003096A File Offset: 0x0002EB6A
		// (set) Token: 0x0600088B RID: 2187 RVA: 0x00030972 File Offset: 0x0002EB72
		public List<WeaponStatModifier> Modifiers { get; set; } = new List<WeaponStatModifier>();

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x0003097B File Offset: 0x0002EB7B
		// (set) Token: 0x0600088D RID: 2189 RVA: 0x00030983 File Offset: 0x0002EB83
		public List<WeaponBuffConsumptionTrigger> ConsumeOn { get; set; } = new List<WeaponBuffConsumptionTrigger>();

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x0003098C File Offset: 0x0002EB8C
		// (set) Token: 0x0600088F RID: 2191 RVA: 0x00030994 File Offset: 0x0002EB94
		[Nullable(2)]
		public ITreeAttribute Data { [NullableContext(2)] get; [NullableContext(2)] set; }
	}
}
