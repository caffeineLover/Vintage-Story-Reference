using System;

namespace ModernWpf.Controls
{
	// Token: 0x0200003B RID: 59
	public sealed class ContentDialogClosingEventArgs : EventArgs
	{
		// Token: 0x06000256 RID: 598 RVA: 0x0000837C File Offset: 0x0000657C
		internal ContentDialogClosingEventArgs(ContentDialogResult result)
		{
			this.Result = result;
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000838B File Offset: 0x0000658B
		// (set) Token: 0x06000258 RID: 600 RVA: 0x00008393 File Offset: 0x00006593
		public bool Cancel { get; set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000839C File Offset: 0x0000659C
		public ContentDialogResult Result { get; }

		// Token: 0x0600025A RID: 602 RVA: 0x000083A4 File Offset: 0x000065A4
		public ContentDialogClosingDeferral GetDeferral()
		{
			this._deferralCount++;
			return new ContentDialogClosingDeferral(delegate
			{
				this.DecrementDeferralCount();
			});
		}

		// Token: 0x0600025B RID: 603 RVA: 0x000083C5 File Offset: 0x000065C5
		internal void SetDeferral(ContentDialogClosingDeferral deferral)
		{
			this._deferral = deferral;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x000083CE File Offset: 0x000065CE
		internal void DecrementDeferralCount()
		{
			this._deferralCount--;
			if (this._deferralCount == 0)
			{
				this._deferral.Complete();
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x000083F1 File Offset: 0x000065F1
		internal void IncrementDeferralCount()
		{
			this._deferralCount++;
		}

		// Token: 0x040000E1 RID: 225
		private ContentDialogClosingDeferral _deferral;

		// Token: 0x040000E2 RID: 226
		private int _deferralCount;
	}
}
