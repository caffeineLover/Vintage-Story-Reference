using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Helpers;

namespace YamlDotNet.Core
{
	// Token: 0x020001B8 RID: 440
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class InsertionQueue<[Nullable(2)] T> : IEnumerable<!0>, IEnumerable
	{
		// Token: 0x060009AA RID: 2474 RVA: 0x0001FC6C File Offset: 0x0001DE6C
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
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x0001FCD2 File Offset: 0x0001DED2
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x0001FCDA File Offset: 0x0001DEDA
		public int Capacity
		{
			get
			{
				return this.items.Length;
			}
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0001FCE4 File Offset: 0x0001DEE4
		public void Enqueue(T item)
		{
			this.ResizeIfNeeded();
			this.items[this.writePtr] = item;
			this.writePtr = (this.writePtr - 1 & this.mask);
			this.count++;
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0001FD24 File Offset: 0x0001DF24
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

		// Token: 0x060009AF RID: 2479 RVA: 0x0001FD78 File Offset: 0x0001DF78
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

		// Token: 0x060009B0 RID: 2480 RVA: 0x0001FDFC File Offset: 0x0001DFFC
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

		// Token: 0x060009B1 RID: 2481 RVA: 0x0001FE8C File Offset: 0x0001E08C
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

		// Token: 0x060009B2 RID: 2482 RVA: 0x0001FF4D File Offset: 0x0001E14D
		public IEnumerator<T> GetEnumerator()
		{
			InsertionQueue<T>.<GetEnumerator>d__16 <GetEnumerator>d__ = new InsertionQueue<T>.<GetEnumerator>d__16(0);
			<GetEnumerator>d__.<>4__this = this;
			return <GetEnumerator>d__;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0001FF5C File Offset: 0x0001E15C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000400 RID: 1024
		private const int DefaultInitialCapacity = 128;

		// Token: 0x04000401 RID: 1025
		private T[] items;

		// Token: 0x04000402 RID: 1026
		private int readPtr;

		// Token: 0x04000403 RID: 1027
		private int writePtr;

		// Token: 0x04000404 RID: 1028
		private int mask;

		// Token: 0x04000405 RID: 1029
		private int count;
	}
}
