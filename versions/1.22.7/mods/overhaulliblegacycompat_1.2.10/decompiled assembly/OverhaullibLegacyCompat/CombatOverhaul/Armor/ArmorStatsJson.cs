using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000138 RID: 312
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ArmorStatsJson
	{
		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000EFE RID: 3838 RVA: 0x0004906D File Offset: 0x0004726D
		// (set) Token: 0x06000EFF RID: 3839 RVA: 0x00049075 File Offset: 0x00047275
		public string[] Layers { get; set; } = Array.Empty<string>();

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000F00 RID: 3840 RVA: 0x0004907E File Offset: 0x0004727E
		// (set) Token: 0x06000F01 RID: 3841 RVA: 0x00049086 File Offset: 0x00047286
		public string[] Zones { get; set; } = Array.Empty<string>();

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000F02 RID: 3842 RVA: 0x0004908F File Offset: 0x0004728F
		// (set) Token: 0x06000F03 RID: 3843 RVA: 0x00049097 File Offset: 0x00047297
		public float[][] AnglesCoverage { get; set; } = Array.Empty<float[]>();

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000F04 RID: 3844 RVA: 0x000490A0 File Offset: 0x000472A0
		// (set) Token: 0x06000F05 RID: 3845 RVA: 0x000490A8 File Offset: 0x000472A8
		public Dictionary<string, float> Resists { get; set; } = new Dictionary<string, float>();

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000F06 RID: 3846 RVA: 0x000490B1 File Offset: 0x000472B1
		// (set) Token: 0x06000F07 RID: 3847 RVA: 0x000490B9 File Offset: 0x000472B9
		public Dictionary<string, float> FlatReduction { get; set; } = new Dictionary<string, float>();

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000F08 RID: 3848 RVA: 0x000490C2 File Offset: 0x000472C2
		// (set) Token: 0x06000F09 RID: 3849 RVA: 0x000490CA File Offset: 0x000472CA
		public Dictionary<string, float> PlayerStats { get; set; } = new Dictionary<string, float>();

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000F0A RID: 3850 RVA: 0x000490D3 File Offset: 0x000472D3
		// (set) Token: 0x06000F0B RID: 3851 RVA: 0x000490DB File Offset: 0x000472DB
		public Dictionary<string, Dictionary<string, float>> ResistsByZone { get; set; } = new Dictionary<string, Dictionary<string, float>>();
	}
}
