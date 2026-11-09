using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200014F RID: 335
	[NullableContext(1)]
	[Nullable(0)]
	public class SlotHotkeyConfig
	{
		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06001064 RID: 4196 RVA: 0x00050324 File Offset: 0x0004E524
		// (set) Token: 0x06001065 RID: 4197 RVA: 0x0005032C File Offset: 0x0004E52C
		public string HotkeyCode { get; set; } = "";

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06001066 RID: 4198 RVA: 0x00050335 File Offset: 0x0004E535
		// (set) Token: 0x06001067 RID: 4199 RVA: 0x0005033D File Offset: 0x0004E53D
		public string HotkeyName { get; set; } = "";

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06001068 RID: 4200 RVA: 0x00050346 File Offset: 0x0004E546
		// (set) Token: 0x06001069 RID: 4201 RVA: 0x0005034E File Offset: 0x0004E54E
		public GlKeys HotkeyKey { get; set; } = 100;

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x0600106A RID: 4202 RVA: 0x00050357 File Offset: 0x0004E557
		// (set) Token: 0x0600106B RID: 4203 RVA: 0x0005035F File Offset: 0x0004E55F
		public string RequiredTypeCode { get; set; } = "";

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x0600106C RID: 4204 RVA: 0x00050368 File Offset: 0x0004E568
		// (set) Token: 0x0600106D RID: 4205 RVA: 0x00050370 File Offset: 0x0004E570
		public string RequiredTypeValue { get; set; } = "";
	}
}
