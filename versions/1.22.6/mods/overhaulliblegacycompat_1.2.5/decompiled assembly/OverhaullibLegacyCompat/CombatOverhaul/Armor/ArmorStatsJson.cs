using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200012C RID: 300
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ArmorStatsJson
	{
		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x0004776D File Offset: 0x0004596D
		// (set) Token: 0x06000E8B RID: 3723 RVA: 0x00047775 File Offset: 0x00045975
		public string[] Layers { get; set; } = Array.Empty<string>();

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000E8C RID: 3724 RVA: 0x0004777E File Offset: 0x0004597E
		// (set) Token: 0x06000E8D RID: 3725 RVA: 0x00047786 File Offset: 0x00045986
		public string[] Zones { get; set; } = Array.Empty<string>();

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000E8E RID: 3726 RVA: 0x0004778F File Offset: 0x0004598F
		// (set) Token: 0x06000E8F RID: 3727 RVA: 0x00047797 File Offset: 0x00045997
		public float[][] AnglesCoverage { get; set; } = Array.Empty<float[]>();

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000E90 RID: 3728 RVA: 0x000477A0 File Offset: 0x000459A0
		// (set) Token: 0x06000E91 RID: 3729 RVA: 0x000477A8 File Offset: 0x000459A8
		public Dictionary<string, float> Resists { get; set; } = new Dictionary<string, float>();

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x000477B1 File Offset: 0x000459B1
		// (set) Token: 0x06000E93 RID: 3731 RVA: 0x000477B9 File Offset: 0x000459B9
		public Dictionary<string, float> FlatReduction { get; set; } = new Dictionary<string, float>();

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000E94 RID: 3732 RVA: 0x000477C2 File Offset: 0x000459C2
		// (set) Token: 0x06000E95 RID: 3733 RVA: 0x000477CA File Offset: 0x000459CA
		public Dictionary<string, float> PlayerStats { get; set; } = new Dictionary<string, float>();

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000E96 RID: 3734 RVA: 0x000477D3 File Offset: 0x000459D3
		// (set) Token: 0x06000E97 RID: 3735 RVA: 0x000477DB File Offset: 0x000459DB
		public Dictionary<string, Dictionary<string, float>> ResistsByZone { get; set; } = new Dictionary<string, Dictionary<string, float>>();
	}
}
