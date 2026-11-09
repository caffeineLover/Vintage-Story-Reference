using System;

namespace ModernWpf.Controls
{
	// Token: 0x0200003A RID: 58
	public sealed class ContentDialogClosingDeferral
	{
		// Token: 0x06000254 RID: 596 RVA: 0x00008351 File Offset: 0x00006551
		internal ContentDialogClosingDeferral(Action handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			this._handler = handler;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000836F File Offset: 0x0000656F
		public void Complete()
		{
			this._handler.Invoke();
		}

		// Token: 0x040000E0 RID: 224
		private readonly Action _handler;
	}
}
