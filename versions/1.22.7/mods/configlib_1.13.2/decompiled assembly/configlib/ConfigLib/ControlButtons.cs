using System;

namespace ConfigLib
{
	// Token: 0x02000049 RID: 73
	public struct ControlButtons
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000B1F1 File Offset: 0x000093F1
		// (set) Token: 0x0600020E RID: 526 RVA: 0x0000B1F9 File Offset: 0x000093F9
		public bool Save { readonly get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000B202 File Offset: 0x00009402
		// (set) Token: 0x06000210 RID: 528 RVA: 0x0000B20A File Offset: 0x0000940A
		public bool Restore { readonly get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000B213 File Offset: 0x00009413
		// (set) Token: 0x06000212 RID: 530 RVA: 0x0000B21B File Offset: 0x0000941B
		public bool Reload { readonly get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000B224 File Offset: 0x00009424
		// (set) Token: 0x06000214 RID: 532 RVA: 0x0000B22C File Offset: 0x0000942C
		public bool Defaults { readonly get; set; }

		// Token: 0x06000215 RID: 533 RVA: 0x0000B235 File Offset: 0x00009435
		public ControlButtons()
		{
			this.Save = false;
			this.Restore = false;
			this.Reload = false;
			this.Defaults = false;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000B253 File Offset: 0x00009453
		public ControlButtons(bool defaultValue)
		{
			this.Save = false;
			this.Restore = false;
			this.Reload = false;
			this.Defaults = false;
			this.Save = defaultValue;
			this.Restore = defaultValue;
			this.Reload = defaultValue;
			this.Defaults = defaultValue;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000B28D File Offset: 0x0000948D
		public void Reset()
		{
			this.Save = false;
			this.Restore = false;
			this.Defaults = false;
			this.Reload = false;
		}
	}
}
