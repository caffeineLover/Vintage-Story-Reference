using System;

namespace PetAI
{
	// Token: 0x02000016 RID: 22
	public class TamingItem
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000080 RID: 128 RVA: 0x000053E0 File Offset: 0x000035E0
		// (set) Token: 0x06000081 RID: 129 RVA: 0x000053E8 File Offset: 0x000035E8
		public string Name { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000082 RID: 130 RVA: 0x000053F1 File Offset: 0x000035F1
		// (set) Token: 0x06000083 RID: 131 RVA: 0x000053F9 File Offset: 0x000035F9
		public string Domain { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00005402 File Offset: 0x00003602
		// (set) Token: 0x06000085 RID: 133 RVA: 0x0000540A File Offset: 0x0000360A
		public float Progress { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00005413 File Offset: 0x00003613
		// (set) Token: 0x06000087 RID: 135 RVA: 0x0000541B File Offset: 0x0000361B
		public long Cooldown { get; set; }
	}
}
