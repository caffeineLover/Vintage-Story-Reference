using System;
using System.Collections.Generic;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000053 RID: 83
	public class ShuffleBag<T>
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0002447A File Offset: 0x0002267A
		public int Size
		{
			get
			{
				return this.data.Count;
			}
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00024487 File Offset: 0x00022687
		public ShuffleBag(int initCapacity)
		{
			this.random = new Random();
			this.currentPosition = -1;
			base..ctor();
			this.data = new List<T>(initCapacity);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x000244AD File Offset: 0x000226AD
		public ShuffleBag(int initCapacity, Random random)
		{
			this.random = new Random();
			this.currentPosition = -1;
			base..ctor();
			this.random = random;
			this.data = new List<T>(initCapacity);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x000244DC File Offset: 0x000226DC
		public void Add(T item, int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				this.data.Add(item);
			}
			this.currentPosition = this.Size - 1;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00024510 File Offset: 0x00022710
		public T Next()
		{
			if (this.currentPosition < 1)
			{
				this.currentPosition = this.Size - 1;
				this.currentItem = this.data[0];
				return this.currentItem;
			}
			int pos = this.random.Next(this.currentPosition);
			this.currentItem = this.data[pos];
			this.data[pos] = this.data[this.currentPosition];
			this.data[this.currentPosition] = this.currentItem;
			this.currentPosition--;
			return this.currentItem;
		}

		// Token: 0x04000181 RID: 385
		private readonly Random random;

		// Token: 0x04000182 RID: 386
		private readonly List<T> data;

		// Token: 0x04000183 RID: 387
		private T currentItem;

		// Token: 0x04000184 RID: 388
		private int currentPosition;
	}
}
