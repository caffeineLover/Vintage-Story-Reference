using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.WeaponBuffs
{
	// Token: 0x020000A1 RID: 161
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WeaponStatModifier
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x0003086F File Offset: 0x0002EA6F
		// (set) Token: 0x06000870 RID: 2160 RVA: 0x00030877 File Offset: 0x0002EA77
		public string StatCode { get; set; } = "";

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x00030880 File Offset: 0x0002EA80
		// (set) Token: 0x06000872 RID: 2162 RVA: 0x00030888 File Offset: 0x0002EA88
		public float Add { get; set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000873 RID: 2163 RVA: 0x00030891 File Offset: 0x0002EA91
		// (set) Token: 0x06000874 RID: 2164 RVA: 0x00030899 File Offset: 0x0002EA99
		public float Multiply { get; set; } = 1f;

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x000308A2 File Offset: 0x0002EAA2
		// (set) Token: 0x06000876 RID: 2166 RVA: 0x000308AA File Offset: 0x0002EAAA
		public int Priority { get; set; }
	}
}
