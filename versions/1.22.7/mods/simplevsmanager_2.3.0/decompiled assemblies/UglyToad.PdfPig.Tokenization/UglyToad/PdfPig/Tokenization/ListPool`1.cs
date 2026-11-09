using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Tokenization
{
	// Token: 0x02000008 RID: 8
	public class ListPool<T>
	{
		// Token: 0x06000017 RID: 23 RVA: 0x00002478 File Offset: 0x00000678
		public ListPool(int capacity = 5)
		{
			this.locker = new object();
			this.pool = new Stack<List<T>>();
			base..ctor();
			this.capacity = capacity;
			for (int i = 0; i < capacity; i++)
			{
				this.pool.Push(new List<T>(10));
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000024C8 File Offset: 0x000006C8
		public List<T> Borrow()
		{
			object obj = this.locker;
			List<T> result;
			lock (obj)
			{
				if (this.pool.Count == 0)
				{
					result = new List<T>();
				}
				else
				{
					result = this.pool.Pop();
				}
			}
			return result;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002524 File Offset: 0x00000724
		public void Return(List<T> instance)
		{
			if (instance == null)
			{
				return;
			}
			instance.Clear();
			object obj = this.locker;
			lock (obj)
			{
				if (this.pool.Count < this.capacity)
				{
					this.pool.Push(instance);
				}
			}
		}

		// Token: 0x04000007 RID: 7
		private readonly int capacity;

		// Token: 0x04000008 RID: 8
		private readonly object locker;

		// Token: 0x04000009 RID: 9
		private readonly Stack<List<T>> pool;
	}
}
