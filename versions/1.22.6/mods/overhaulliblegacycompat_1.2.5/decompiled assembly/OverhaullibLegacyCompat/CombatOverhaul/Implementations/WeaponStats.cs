using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Implementations
{
	// Token: 0x0200009E RID: 158
	[NullableContext(1)]
	[Nullable(0)]
	public class WeaponStats
	{
		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x00030742 File Offset: 0x0002E942
		// (set) Token: 0x0600085F RID: 2143 RVA: 0x0003074A File Offset: 0x0002E94A
		public string ReadyAnimation { get; set; } = "";

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x00030753 File Offset: 0x0002E953
		// (set) Token: 0x06000861 RID: 2145 RVA: 0x0003075B File Offset: 0x0002E95B
		public string IdleAnimation { get; set; } = "";

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x00030764 File Offset: 0x0002E964
		// (set) Token: 0x06000863 RID: 2147 RVA: 0x0003076C File Offset: 0x0002E96C
		public string WalkAnimation { get; set; } = "";

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x00030775 File Offset: 0x0002E975
		// (set) Token: 0x06000865 RID: 2149 RVA: 0x0003077D File Offset: 0x0002E97D
		public string RunAnimation { get; set; } = "";

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x00030786 File Offset: 0x0002E986
		// (set) Token: 0x06000867 RID: 2151 RVA: 0x0003078E File Offset: 0x0002E98E
		public string SwimAnimation { get; set; } = "";

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000868 RID: 2152 RVA: 0x00030797 File Offset: 0x0002E997
		// (set) Token: 0x06000869 RID: 2153 RVA: 0x0003079F File Offset: 0x0002E99F
		public string SwimIdleAnimation { get; set; } = "";

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x000307A8 File Offset: 0x0002E9A8
		// (set) Token: 0x0600086B RID: 2155 RVA: 0x000307B0 File Offset: 0x0002E9B0
		public string ProficiencyStat { get; set; } = "";

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x000307B9 File Offset: 0x0002E9B9
		// (set) Token: 0x0600086D RID: 2157 RVA: 0x000307C1 File Offset: 0x0002E9C1
		public string[] ProficiencyStats { get; set; } = Array.Empty<string>();
	}
}
