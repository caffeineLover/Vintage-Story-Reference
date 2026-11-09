using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace Firearms
{
	// Token: 0x02000012 RID: 18
	[NullableContext(1)]
	[Nullable(0)]
	public class WettableStats
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000046FB File Offset: 0x000028FB
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00004703 File Offset: 0x00002903
		public JsonItemStack TransitionStack { get; set; } = new JsonItemStack();

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000470C File Offset: 0x0000290C
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00004714 File Offset: 0x00002914
		public string Sound { get; set; } = "";
	}
}
