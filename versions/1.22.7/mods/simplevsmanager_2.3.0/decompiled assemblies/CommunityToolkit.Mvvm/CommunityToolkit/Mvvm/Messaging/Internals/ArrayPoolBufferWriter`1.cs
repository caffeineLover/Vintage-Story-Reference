using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Messaging.Internals
{
	// Token: 0x0200001A RID: 26
	[NullableContext(1)]
	[Nullable(0)]
	[CompilerFeatureRequired("RefStructs")]
	internal ref struct ArrayPoolBufferWriter<[Nullable(2)] T>
	{
		// Token: 0x0600009C RID: 156 RVA: 0x000044FC File Offset: 0x000026FC
		[MethodImpl(256)]
		[return: Nullable(new byte[]
		{
			0,
			1
		})]
		public static ArrayPoolBufferWriter<T> Create()
		{
			ArrayPoolBufferWriter<T> result;
			result.span = (result.array = ArrayPool<T>.Shared.Rent(128));
			result.index = 0;
			return result;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00004537 File Offset: 0x00002737
		[Nullable(new byte[]
		{
			0,
			1
		})]
		public ReadOnlySpan<T> Span
		{
			[MethodImpl(256)]
			[return: Nullable(new byte[]
			{
				0,
				1
			})]
			get
			{
				return this.span.Slice(0, this.index);
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004550 File Offset: 0x00002750
		[MethodImpl(256)]
		public unsafe void Add(T item)
		{
			Span<T> span = this.span;
			int num = this.index;
			if (num < span.Length)
			{
				*span[num] = item;
				this.index = num + 1;
				return;
			}
			this.ResizeBufferAndAdd(item);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004594 File Offset: 0x00002794
		public void Reset()
		{
			Array.Clear(this.array, 0, this.index);
			this.index = 0;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000045B0 File Offset: 0x000027B0
		[MethodImpl(8)]
		private unsafe void ResizeBufferAndAdd(T item)
		{
			T[] array = ArrayPool<T>.Shared.Rent(this.index << 2);
			Array.Copy(this.array, 0, array, 0, this.index);
			Array.Clear(this.array, 0, this.index);
			ArrayPool<T>.Shared.Return(this.array, false);
			this.span = (this.array = array);
			int num = this.index;
			this.index = num + 1;
			*this.span[num] = item;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000463E File Offset: 0x0000283E
		public void Dispose()
		{
			Array.Clear(this.array, 0, this.index);
			ArrayPool<T>.Shared.Return(this.array, false);
		}

		// Token: 0x04000034 RID: 52
		private const int DefaultInitialBufferSize = 128;

		// Token: 0x04000035 RID: 53
		private T[] array;

		// Token: 0x04000036 RID: 54
		[Nullable(new byte[]
		{
			0,
			1
		})]
		private Span<T> span;

		// Token: 0x04000037 RID: 55
		private int index;
	}
}
