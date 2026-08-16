using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul
{
	// Token: 0x0200001D RID: 29
	[NullableContext(1)]
	[Nullable(0)]
	public class TexturesFromAttributesProperties
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000870C File Offset: 0x0000690C
		// (set) Token: 0x06000207 RID: 519 RVA: 0x00008714 File Offset: 0x00006914
		public TextureConfig[] Textures { get; set; } = Array.Empty<TextureConfig>();

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000871D File Offset: 0x0000691D
		// (set) Token: 0x06000209 RID: 521 RVA: 0x00008725 File Offset: 0x00006925
		public Dictionary<string, TextureConfig[]> TexturesByType { get; set; } = new Dictionary<string, TextureConfig[]>();

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000872E File Offset: 0x0000692E
		// (set) Token: 0x0600020B RID: 523 RVA: 0x00008736 File Offset: 0x00006936
		public string[] CreativeTabs { get; set; } = Array.Empty<string>();

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000873F File Offset: 0x0000693F
		// (set) Token: 0x0600020D RID: 525 RVA: 0x00008747 File Offset: 0x00006947
		public bool AddNoAttributesItem { get; set; } = true;
	}
}
