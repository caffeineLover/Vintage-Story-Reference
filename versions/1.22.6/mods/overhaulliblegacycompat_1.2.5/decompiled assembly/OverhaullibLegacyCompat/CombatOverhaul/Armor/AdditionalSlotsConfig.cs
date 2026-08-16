using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000138 RID: 312
	[NullableContext(1)]
	[Nullable(0)]
	public class AdditionalSlotsConfig
	{
		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000F22 RID: 3874 RVA: 0x0004ACBB File Offset: 0x00048EBB
		// (set) Token: 0x06000F23 RID: 3875 RVA: 0x0004ACC3 File Offset: 0x00048EC3
		public string[] EnabledSlots { get; set; } = Array.Empty<string>();

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000F24 RID: 3876 RVA: 0x0004ACCC File Offset: 0x00048ECC
		// (set) Token: 0x06000F25 RID: 3877 RVA: 0x0004ACD4 File Offset: 0x00048ED4
		public Dictionary<string, string> Icons { get; set; } = new Dictionary<string, string>();

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x0004ACDD File Offset: 0x00048EDD
		// (set) Token: 0x06000F27 RID: 3879 RVA: 0x0004ACE5 File Offset: 0x00048EE5
		public Dictionary<string, SlotConfigJson> Configs { get; set; } = new Dictionary<string, SlotConfigJson>();
	}
}
