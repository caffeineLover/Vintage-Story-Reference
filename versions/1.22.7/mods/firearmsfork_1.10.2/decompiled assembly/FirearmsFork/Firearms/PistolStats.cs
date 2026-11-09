using System;
using System.Runtime.CompilerServices;

namespace Firearms
{
	// Token: 0x02000025 RID: 37
	[NullableContext(1)]
	[Nullable(0)]
	public class PistolStats
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x000098B8 File Offset: 0x00007AB8
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x000098C0 File Offset: 0x00007AC0
		public string[] ReplacementItemWildcards { get; set; } = Array.Empty<string>();

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x000098C9 File Offset: 0x00007AC9
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x000098D1 File Offset: 0x00007AD1
		public string[] ReplacementBackpackCategoryWildcards { get; set; } = Array.Empty<string>();
	}
}
