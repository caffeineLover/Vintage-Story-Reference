using System;
using System.Runtime.CompilerServices;

namespace VSImGui
{
	// Token: 0x02000016 RID: 22
	public class StyleApplier : IDisposable
	{
		// Token: 0x060001EC RID: 492 RVA: 0x0000BA1B File Offset: 0x00009C1B
		[NullableContext(1)]
		public StyleApplier(Style style)
		{
			this.mStyle = style;
			this.mStyle.Push();
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000BA35 File Offset: 0x00009C35
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					this.mStyle.Pop();
				}
				this.disposedValue = true;
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000BA54 File Offset: 0x00009C54
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0400003E RID: 62
		[Nullable(1)]
		private readonly Style mStyle;

		// Token: 0x0400003F RID: 63
		private bool disposedValue;
	}
}
