using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Colliders
{
	// Token: 0x02000167 RID: 359
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ColliderTypesJson
	{
		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x060011D9 RID: 4569 RVA: 0x0005527C File Offset: 0x0005347C
		// (set) Token: 0x060011DA RID: 4570 RVA: 0x00055284 File Offset: 0x00053484
		public string[] Torso { get; set; } = Array.Empty<string>();

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x060011DB RID: 4571 RVA: 0x0005528D File Offset: 0x0005348D
		// (set) Token: 0x060011DC RID: 4572 RVA: 0x00055295 File Offset: 0x00053495
		public string[] Head { get; set; } = Array.Empty<string>();

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x060011DD RID: 4573 RVA: 0x0005529E File Offset: 0x0005349E
		// (set) Token: 0x060011DE RID: 4574 RVA: 0x000552A6 File Offset: 0x000534A6
		public string[] Arm { get; set; } = Array.Empty<string>();

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x060011DF RID: 4575 RVA: 0x000552AF File Offset: 0x000534AF
		// (set) Token: 0x060011E0 RID: 4576 RVA: 0x000552B7 File Offset: 0x000534B7
		public string[] Leg { get; set; } = Array.Empty<string>();

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x060011E1 RID: 4577 RVA: 0x000552C0 File Offset: 0x000534C0
		// (set) Token: 0x060011E2 RID: 4578 RVA: 0x000552C8 File Offset: 0x000534C8
		public string[] Critical { get; set; } = Array.Empty<string>();

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x060011E3 RID: 4579 RVA: 0x000552D1 File Offset: 0x000534D1
		// (set) Token: 0x060011E4 RID: 4580 RVA: 0x000552D9 File Offset: 0x000534D9
		public string[] Resistant { get; set; } = Array.Empty<string>();
	}
}
