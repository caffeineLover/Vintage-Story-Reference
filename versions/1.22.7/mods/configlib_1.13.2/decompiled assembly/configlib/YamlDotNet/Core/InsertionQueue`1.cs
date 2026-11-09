using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Helpers;

namespace YamlDotNet.Core
{
	// Token: 0x020001B4 RID: 436
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class InsertionQueue<[Nullable(2)] T> : IEnumerable<!0>, IEnumerable
	{
		// Token: 0x0600096E RID: 2414 RVA: 0x0001F324 File Offset: 0x0001D524
		public InsertionQueue(int initialCapacity = 128)
		{
			if (initialCapacity <= 0)
			{
				throw new ArgumentOutOfRangeException("initialCapacity", "The initial capacity must be a positive number.");
			}
			if (!initialCapacity.IsPowerOfTwo())
			{
				throw new ArgumentException("The initial capacity must be a power of 2.", "initialCapacity");
			}
			this.items = new T[initialCapacity];
			this.readPtr = initialCapacity / 2;
			this.writePtr = initialCapacity / 2;
			this.mask = initialCapacity - 1;
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x0001F38A File Offset: 0x0001D58A
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x0001F392 File Offset: 0x0001D592
		public int Capacity
		{
			get
			{
				return this.items.Length;
			}
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0001F39C File Offset: 0x0001D59C
		public void Enqueue(T item)
		{
			this.ResizeIfNeeded();
			this.items[this.writePtr] = item;
			this.writePtr = (this.writePtr - 1 & this.mask);
			this.count++;
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x0001F3DC File Offset: 0x0001D5DC
		public T Dequeue()
		{
			if (this.count == 0)
			{
				throw new InvalidOperationException("The queue is empty");
			}
			T result = this.items[this.readPtr];
			this.readPtr = (this.readPtr - 1 & this.mask);
			this.count--;
			return result;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0001F430 File Offset: 0x0001D630
		public void Insert(int index, T item)
		{
			if (index > this.count)
			{
				throw new InvalidOperationException("Cannot insert outside of the bounds of the queue");
			}
			this.ResizeIfNeeded();
			int num;
			int num2;
			int num3;
			int num4;
			InsertionQueue<T>.CalculateInsertionParameters(this.mask, this.count, index, ref this.readPtr, ref this.writePtr, ref num, ref num2, ref num3, ref num4);
			if (num4 != 0)
			{
				Array.Copy(this.items, num2, this.items, num2 + num3, num4);
			}
			this.items[num] = item;
			this.count++;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0001F4B4 File Offset: 0x0001D6B4
		private void ResizeIfNeeded()
		{
			int num = this.items.Length;
			if (this.count == num)
			{
				T[] array = new T[num * 2];
				int num2 = this.readPtr + 1;
				if (num2 > 0)
				{
					Array.Copy(this.items, 0, array, 0, num2);
				}
				this.writePtr += num;
				int num3 = num - num2;
				if (num3 > 0)
				{
					Array.Copy(this.items, this.readPtr + 1, array, this.writePtr + 1, num3);
				}
				this.items = array;
				this.mask = this.mask * 2 + 1;
			}
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0001F544 File Offset: 0x0001D744
		internal static void CalculateInsertionParameters(int mask, int count, int index, ref int readPtr, ref int writePtr, out int insertPtr, out int copyIndex, out int copyOffset, out int copyLength)
		{
			int num = readPtr + 1 & mask;
			if (index == 0)
			{
				insertPtr = (readPtr = num);
				copyIndex = 0;
				copyOffset = 0;
				copyLength = 0;
				return;
			}
			insertPtr = (readPtr - index & mask);
			if (index == count)
			{
				writePtr = (writePtr - 1 & mask);
				copyIndex = 0;
				copyOffset = 0;
				copyLength = 0;
				return;
			}
			int num2 = (num >= insertPtr) ? (readPtr - insertPtr) : int.MaxValue;
			int num3 = (writePtr <= insertPtr) ? (insertPtr - writePtr) : int.MaxValue;
			if (num2 <= num3)
			{
				insertPtr++;
				readPtr++;
				copyIndex = insertPtr;
				copyOffset = 1;
				copyLength = num2;
				return;
			}
			copyIndex = writePtr + 1;
			copyOffset = -1;
			copyLength = num3;
			writePtr = (writePtr - 1 & mask);
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0001F605 File Offset: 0x0001D805
		public IEnumerator<T> GetEnumerator()
		{
			InsertionQueue<T>.<GetEnumerator>d__16 <GetEnumerator>d__ = new InsertionQueue<T>.<GetEnumerator>d__16(0);
			<GetEnumerator>d__.<>4__this = this;
			return <GetEnumerator>d__;
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0001F614 File Offset: 0x0001D814
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040003D2 RID: 978
		private const int DefaultInitialCapacity = 128;

		// Token: 0x040003D3 RID: 979
		private T[] items;

		// Token: 0x040003D4 RID: 980
		private int readPtr;

		// Token: 0x040003D5 RID: 981
		private int writePtr;

		// Token: 0x040003D6 RID: 982
		private int mask;

		// Token: 0x040003D7 RID: 983
		private int count;
	}
}
