using System;

namespace ModernWpf.Controls
{
	// Token: 0x02000037 RID: 55
	public sealed class ContentDialogButtonClickDeferral
	{
		// Token: 0x06000248 RID: 584 RVA: 0x00008291 File Offset: 0x00006491
		internal ContentDialogButtonClickDeferral(Action handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			this._handler = handler;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x000082AF File Offset: 0x000064AF
		public void Complete()
		{
			this._handler.Invoke();
		}

		// Token: 0x040000DB RID: 219
		private readonly Action _handler;
	}
}
