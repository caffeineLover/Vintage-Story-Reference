using System;

namespace ModernWpf.Controls
{
	// Token: 0x02000015 RID: 21
	public sealed class AutoSuggestBoxQuerySubmittedEventArgs : EventArgs
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00004124 File Offset: 0x00002324
		// (set) Token: 0x060000DB RID: 219 RVA: 0x0000412C File Offset: 0x0000232C
		public object ChosenSuggestion { get; internal set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00004135 File Offset: 0x00002335
		// (set) Token: 0x060000DD RID: 221 RVA: 0x0000413D File Offset: 0x0000233D
		public string QueryText { get; internal set; } = string.Empty;
	}
}
