using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Helpers;

namespace YamlDotNet.Core
{
	// Token: 0x020000CC RID: 204
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class InsertionQueue<[Nullable(2)] T> : IEnumerable<T>, IEnumerable
	{
		// Token: 0x06000511 RID: 1297 RVA: 0x000117F8 File Offset: 0x0000F9F8
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

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x0001185E File Offset: 0x0000FA5E
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x00011866 File Offset: 0x0000FA66
		public int Capacity
		{
			get
			{
				return this.items.Length;
			}
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00011870 File Offset: 0x0000FA70
		public void Enqueue(T item)
		{
			this.ResizeIfNeeded();
			this.items[this.writePtr] = item;
			this.writePtr = (this.writePtr - 1 & this.mask);
			this.count++;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x000118B0 File Offset: 0x0000FAB0
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

		// Token: 0x06000516 RID: 1302 RVA: 0x00011908 File Offset: 0x0000FB08
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

		// Token: 0x06000517 RID: 1303 RVA: 0x0001198C File Offset: 0x0000FB8C
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

		// Token: 0x06000518 RID: 1304 RVA: 0x00011A1C File Offset: 0x0000FC1C
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

		// Token: 0x06000519 RID: 1305 RVA: 0x00011AE6 File Offset: 0x0000FCE6
		public IEnumerator<T> GetEnumerator()
		{
			InsertionQueue<T>.<GetEnumerator>d__16 <GetEnumerator>d__ = new InsertionQueue<T>.<GetEnumerator>d__16(0);
			<GetEnumerator>d__.<>4__this = this;
			return <GetEnumerator>d__;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00011AF5 File Offset: 0x0000FCF5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400015E RID: 350
		private const int DefaultInitialCapacity = 128;

		// Token: 0x0400015F RID: 351
		private T[] items;

		// Token: 0x04000160 RID: 352
		private int readPtr;

		// Token: 0x04000161 RID: 353
		private int writePtr;

		// Token: 0x04000162 RID: 354
		private int mask;

		// Token: 0x04000163 RID: 355
		private int count;
	}
}
