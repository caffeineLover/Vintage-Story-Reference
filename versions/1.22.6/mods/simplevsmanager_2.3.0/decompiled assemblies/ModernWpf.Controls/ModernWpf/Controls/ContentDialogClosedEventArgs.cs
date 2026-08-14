using System;

namespace ModernWpf.Controls
{
	// Token: 0x02000039 RID: 57
	public class ContentDialogClosedEventArgs : EventArgs
	{
		// Token: 0x06000252 RID: 594 RVA: 0x0000833A File Offset: 0x0000653A
		internal ContentDialogClosedEventArgs(ContentDialogResult result)
		{
			this.Result = result;
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000253 RID: 595 RVA: 0x00008349 File Offset: 0x00006549
		public ContentDialogResult Result { get; }
	}
}
