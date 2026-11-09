using System;

namespace ModernWpf.Controls
{
	// Token: 0x02000038 RID: 56
	public class ContentDialogButtonClickEventArgs : EventArgs
	{
		// Token: 0x0600024A RID: 586 RVA: 0x000082BC File Offset: 0x000064BC
		internal ContentDialogButtonClickEventArgs()
		{
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600024B RID: 587 RVA: 0x000082C4 File Offset: 0x000064C4
		// (set) Token: 0x0600024C RID: 588 RVA: 0x000082CC File Offset: 0x000064CC
		public bool Cancel { get; set; }

		// Token: 0x0600024D RID: 589 RVA: 0x000082D5 File Offset: 0x000064D5
		public ContentDialogButtonClickDeferral GetDeferral()
		{
			this._deferralCount++;
			return new ContentDialogButtonClickDeferral(delegate
			{
				this.DecrementDeferralCount();
			});
		}

		// Token: 0x0600024E RID: 590 RVA: 0x000082F6 File Offset: 0x000064F6
		internal void SetDeferral(ContentDialogButtonClickDeferral deferral)
		{
			this._deferral = deferral;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x000082FF File Offset: 0x000064FF
		internal void DecrementDeferralCount()
		{
			this._deferralCount--;
			if (this._deferralCount == 0)
			{
				this._deferral.Complete();
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00008322 File Offset: 0x00006522
		internal void IncrementDeferralCount()
		{
			this._deferralCount++;
		}

		// Token: 0x040000DC RID: 220
		private ContentDialogButtonClickDeferral _deferral;

		// Token: 0x040000DD RID: 221
		private int _deferralCount;
	}
}
