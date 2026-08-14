using System;

namespace ModernWpf
{
	// Token: 0x0200020D RID: 525
	internal abstract class EventRevoker<TSource, TDelegate> where TSource : class where TDelegate : Delegate
	{
		// Token: 0x06000F78 RID: 3960 RVA: 0x0002C19D File Offset: 0x0002A39D
		protected EventRevoker(TSource source, TDelegate handler)
		{
			this._source = new WeakReference<TSource>(source);
			this._handler = new WeakReference<TDelegate>(handler);
			this.AddHandler(source, handler);
		}

		// Token: 0x06000F79 RID: 3961
		protected abstract void AddHandler(TSource source, TDelegate handler);

		// Token: 0x06000F7A RID: 3962
		protected abstract void RemoveHandler(TSource source, TDelegate handler);

		// Token: 0x06000F7B RID: 3963 RVA: 0x0002C1C8 File Offset: 0x0002A3C8
		public void Revoke()
		{
			TSource tsource;
			TDelegate tdelegate;
			if (this._source != null && this._handler != null && this._source.TryGetTarget(ref tsource) && this._handler.TryGetTarget(ref tdelegate))
			{
				this.RemoveHandler(tsource, tdelegate);
			}
			this._source = null;
			this._handler = null;
		}

		// Token: 0x04000358 RID: 856
		private WeakReference<TSource> _source;

		// Token: 0x04000359 RID: 857
		private WeakReference<TDelegate> _handler;
	}
}
