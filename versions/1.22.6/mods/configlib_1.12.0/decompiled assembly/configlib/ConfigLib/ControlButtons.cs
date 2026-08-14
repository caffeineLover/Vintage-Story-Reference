using System;

namespace ConfigLib
{
	// Token: 0x02000047 RID: 71
	public struct ControlButtons
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000AED5 File Offset: 0x000090D5
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x0000AEDD File Offset: 0x000090DD
		public bool Save { readonly get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000AEE6 File Offset: 0x000090E6
		// (set) Token: 0x060001EB RID: 491 RVA: 0x0000AEEE File Offset: 0x000090EE
		public bool Restore { readonly get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000AEF7 File Offset: 0x000090F7
		// (set) Token: 0x060001ED RID: 493 RVA: 0x0000AEFF File Offset: 0x000090FF
		public bool Reload { readonly get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000AF08 File Offset: 0x00009108
		// (set) Token: 0x060001EF RID: 495 RVA: 0x0000AF10 File Offset: 0x00009110
		public bool Defaults { readonly get; set; }

		// Token: 0x060001F0 RID: 496 RVA: 0x0000AF19 File Offset: 0x00009119
		public ControlButtons()
		{
			this.Save = false;
			this.Restore = false;
			this.Reload = false;
			this.Defaults = false;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000AF37 File Offset: 0x00009137
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

		// Token: 0x060001F2 RID: 498 RVA: 0x0000AF71 File Offset: 0x00009171
		public void Reset()
		{
			this.Save = false;
			this.Restore = false;
			this.Defaults = false;
			this.Reload = false;
		}
	}
}
