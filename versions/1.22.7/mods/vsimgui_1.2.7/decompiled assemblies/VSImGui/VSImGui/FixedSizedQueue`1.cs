using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VSImGui
{
	// Token: 0x02000020 RID: 32
	[NullableContext(1)]
	[Nullable(0)]
	public class FixedSizedQueue<[Nullable(2)] TElement> : IEnumerable<TElement>, IEnumerable
	{
		// Token: 0x06000263 RID: 611 RVA: 0x0000D077 File Offset: 0x0000B277
		public FixedSizedQueue(int limit)
		{
			this.Queue = new Queue<TElement>();
			base..ctor();
			this.Limit = limit;
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000D091 File Offset: 0x0000B291
		// (set) Token: 0x06000265 RID: 613 RVA: 0x0000D099 File Offset: 0x0000B299
		public int Limit
		{
			get
			{
				return this._limit;
			}
			set
			{
				this._limit = value;
				this.Shrink();
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000D0A8 File Offset: 0x0000B2A8
		public int Count
		{
			get
			{
				return this.Queue.Count;
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000D0B5 File Offset: 0x0000B2B5
		public void Enqueue(TElement obj)
		{
			this.Queue.Enqueue(obj);
			this.Shrink();
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000D0C9 File Offset: 0x0000B2C9
		public IEnumerator<TElement> GetEnumerator()
		{
			return this.Queue.GetEnumerator();
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000D0DB File Offset: 0x0000B2DB
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Queue.GetEnumerator();
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000D0ED File Offset: 0x0000B2ED
		public void Clear()
		{
			this.Queue.Clear();
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000D0FC File Offset: 0x0000B2FC
		private void Shrink()
		{
			TElement telement;
			while (this.Queue.Count > this._limit && this.Queue.TryDequeue(ref telement))
			{
			}
		}

		// Token: 0x04000047 RID: 71
		public readonly Queue<TElement> Queue;

		// Token: 0x04000048 RID: 72
		private int _limit;
	}
}
