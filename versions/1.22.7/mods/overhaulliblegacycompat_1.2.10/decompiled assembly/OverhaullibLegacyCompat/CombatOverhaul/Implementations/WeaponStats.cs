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
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x0003077A File Offset: 0x0002E97A
		// (set) Token: 0x0600085F RID: 2143 RVA: 0x00030782 File Offset: 0x0002E982
		public string ReadyAnimation { get; set; } = "";

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x0003078B File Offset: 0x0002E98B
		// (set) Token: 0x06000861 RID: 2145 RVA: 0x00030793 File Offset: 0x0002E993
		public string IdleAnimation { get; set; } = "";

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x0003079C File Offset: 0x0002E99C
		// (set) Token: 0x06000863 RID: 2147 RVA: 0x000307A4 File Offset: 0x0002E9A4
		public string WalkAnimation { get; set; } = "";

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x000307AD File Offset: 0x0002E9AD
		// (set) Token: 0x06000865 RID: 2149 RVA: 0x000307B5 File Offset: 0x0002E9B5
		public string RunAnimation { get; set; } = "";

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x000307BE File Offset: 0x0002E9BE
		// (set) Token: 0x06000867 RID: 2151 RVA: 0x000307C6 File Offset: 0x0002E9C6
		public string SwimAnimation { get; set; } = "";

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000868 RID: 2152 RVA: 0x000307CF File Offset: 0x0002E9CF
		// (set) Token: 0x06000869 RID: 2153 RVA: 0x000307D7 File Offset: 0x0002E9D7
		public string SwimIdleAnimation { get; set; } = "";

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x000307E0 File Offset: 0x0002E9E0
		// (set) Token: 0x0600086B RID: 2155 RVA: 0x000307E8 File Offset: 0x0002E9E8
		public string ProficiencyStat { get; set; } = "";

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x000307F1 File Offset: 0x0002E9F1
		// (set) Token: 0x0600086D RID: 2157 RVA: 0x000307F9 File Offset: 0x0002E9F9
		public string[] ProficiencyStats { get; set; } = Array.Empty<string>();
	}
}
