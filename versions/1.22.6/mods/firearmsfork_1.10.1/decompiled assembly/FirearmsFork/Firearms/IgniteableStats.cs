using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace Firearms
{
	// Token: 0x02000008 RID: 8
	[NullableContext(1)]
	[Nullable(0)]
	public class IgniteableStats
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002DEA File Offset: 0x00000FEA
		// (set) Token: 0x06000053 RID: 83 RVA: 0x00002DF2 File Offset: 0x00000FF2
		public JsonItemStack TransitionStack { get; set; } = new JsonItemStack();

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002DFB File Offset: 0x00000FFB
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00002E03 File Offset: 0x00001003
		public string Sound { get; set; } = "";

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00002E0C File Offset: 0x0000100C
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00002E14 File Offset: 0x00001014
		public string[] IgniteFrom { get; set; } = Array.Empty<string>();
	}
}
