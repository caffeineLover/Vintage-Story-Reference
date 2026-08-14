using System;
using System.Collections.Generic;
using System.Text;

namespace UglyToad.PdfPig.Tokenization
{
	// Token: 0x0200000C RID: 12
	internal class StringBuilderPool
	{
		// Token: 0x06000025 RID: 37 RVA: 0x00002F8C File Offset: 0x0000118C
		public StringBuilderPool(int capacity = 5)
		{
			this.capacity = capacity;
			for (int i = 0; i < capacity; i++)
			{
				this.pool.Push(new StringBuilder());
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002FD8 File Offset: 0x000011D8
		public StringBuilder Borrow()
		{
			object obj = this.locker;
			StringBuilder result;
			lock (obj)
			{
				if (this.pool.Count == 0)
				{
					result = new StringBuilder();
				}
				else
				{
					result = this.pool.Pop();
				}
			}
			return result;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003034 File Offset: 0x00001234
		public void Return(StringBuilder instance)
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

		// Token: 0x04000012 RID: 18
		private readonly int capacity;

		// Token: 0x04000013 RID: 19
		private readonly object locker = new object();

		// Token: 0x04000014 RID: 20
		private readonly Stack<StringBuilder> pool = new Stack<StringBuilder>();
	}
}
