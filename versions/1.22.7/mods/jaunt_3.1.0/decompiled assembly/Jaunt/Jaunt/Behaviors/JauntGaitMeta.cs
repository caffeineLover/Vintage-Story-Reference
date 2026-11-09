using System;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace Jaunt.Behaviors
{
	// Token: 0x02000010 RID: 16
	public class JauntGaitMeta : GaitMeta
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00004660 File Offset: 0x00002860
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00004668 File Offset: 0x00002868
		public float AscendSpeed { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00004671 File Offset: 0x00002871
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00004679 File Offset: 0x00002879
		public float DescendSpeed { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00004682 File Offset: 0x00002882
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x0000468A File Offset: 0x0000288A
		public AssetLocation IconTexture { get; set; }
	}
}
