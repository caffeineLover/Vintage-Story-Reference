using System;

namespace ModernWpf.Controls
{
	// Token: 0x02000018 RID: 24
	public sealed class AutoSuggestBoxTextChangedEventArgs : EventArgs
	{
		// Token: 0x060000E1 RID: 225 RVA: 0x0000415F File Offset: 0x0000235F
		public AutoSuggestBoxTextChangedEventArgs()
		{
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004167 File Offset: 0x00002367
		internal AutoSuggestBoxTextChangedEventArgs(AutoSuggestBox source, string value, AutoSuggestionBoxTextChangeReason reason)
		{
			this.m_source = new WeakReference<AutoSuggestBox>(source);
			this.m_value = value;
			this.Reason = reason;
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00004189 File Offset: 0x00002389
		public AutoSuggestionBoxTextChangeReason Reason { get; }

		// Token: 0x060000E4 RID: 228 RVA: 0x00004194 File Offset: 0x00002394
		public bool CheckCurrent()
		{
			AutoSuggestBox autoSuggestBox;
			return this.m_source != null && this.m_source.TryGetTarget(ref autoSuggestBox) && autoSuggestBox.Text == this.m_value;
		}

		// Token: 0x0400002F RID: 47
		private readonly WeakReference<AutoSuggestBox> m_source;

		// Token: 0x04000030 RID: 48
		private readonly string m_value;
	}
}
