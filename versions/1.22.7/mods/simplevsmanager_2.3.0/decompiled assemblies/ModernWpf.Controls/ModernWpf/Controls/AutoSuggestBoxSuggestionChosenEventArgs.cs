using System;

namespace ModernWpf.Controls
{
	// Token: 0x02000016 RID: 22
	public sealed class AutoSuggestBoxSuggestionChosenEventArgs : EventArgs
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000DF RID: 223 RVA: 0x0000414E File Offset: 0x0000234E
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x00004156 File Offset: 0x00002356
		public object SelectedItem { get; internal set; }
	}
}
