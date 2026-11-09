using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000144 RID: 324
	[NullableContext(1)]
	[Nullable(0)]
	public class AdditionalSlotsConfig
	{
		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000F96 RID: 3990 RVA: 0x0004C5BB File Offset: 0x0004A7BB
		// (set) Token: 0x06000F97 RID: 3991 RVA: 0x0004C5C3 File Offset: 0x0004A7C3
		public string[] EnabledSlots { get; set; } = Array.Empty<string>();

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000F98 RID: 3992 RVA: 0x0004C5CC File Offset: 0x0004A7CC
		// (set) Token: 0x06000F99 RID: 3993 RVA: 0x0004C5D4 File Offset: 0x0004A7D4
		public Dictionary<string, string> Icons { get; set; } = new Dictionary<string, string>();

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000F9A RID: 3994 RVA: 0x0004C5DD File Offset: 0x0004A7DD
		// (set) Token: 0x06000F9B RID: 3995 RVA: 0x0004C5E5 File Offset: 0x0004A7E5
		public Dictionary<string, SlotConfigJson> Configs { get; set; } = new Dictionary<string, SlotConfigJson>();
	}
}
