using System;

namespace VSImGui
{
	// Token: 0x02000003 RID: 3
	public class ImGuiConfig
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000216A File Offset: 0x0000036A
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002172 File Offset: 0x00000372
		public int Version { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000217B File Offset: 0x0000037B
		// (set) Token: 0x0600000A RID: 10 RVA: 0x00002183 File Offset: 0x00000383
		public bool MultiViewportSupport { get; set; } = true;
	}
}
