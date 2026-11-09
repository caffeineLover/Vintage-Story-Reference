using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace YamlDotNet.Helpers
{
	// Token: 0x0200018F RID: 399
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerStepThrough]
	internal sealed class ConcurrentObjectPool<T> where T : class
	{
		// Token: 0x0600085B RID: 2139 RVA: 0x0001BED4 File Offset: 0x0001A0D4
		internal ConcurrentObjectPool([Nullable(new byte[]
		{
			1,
			0
		})] ConcurrentObjectPool<T>.Factory factory) : this(factory, Environment.ProcessorCount * 2)
		{
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0001BEE4 File Offset: 0x0001A0E4
		internal ConcurrentObjectPool([Nullable(new byte[]
		{
			1,
			0
		})] ConcurrentObjectPool<T>.Factory factory, int size)
		{
			this.factory = factory;
			this.items = new ConcurrentObjectPool<T>.Element[size - 1];
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0001BF01 File Offset: 0x0001A101
		private T CreateInstance()
		{
			return this.factory.Invoke();
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0001BF10 File Offset: 0x0001A110
		internal T Allocate()
		{
			T t = this.firstItem;
			if (t == null || t != Interlocked.CompareExchange<T>(ref this.firstItem, default(T), t))
			{
				t = this.AllocateSlow();
			}
			return t;
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0001BF58 File Offset: 0x0001A158
		private T AllocateSlow()
		{
			ConcurrentObjectPool<T>.Element[] array = this.items;
			for (int i = 0; i < array.Length; i++)
			{
				T value = array[i].value;
				if (value != null && value == Interlocked.CompareExchange<T>(ref array[i].value, default(T), value))
				{
					return value;
				}
			}
			return this.CreateInstance();
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0001BFBF File Offset: 0x0001A1BF
		internal void Free(T obj)
		{
			if (this.firstItem == null)
			{
				this.firstItem = obj;
				return;
			}
			this.FreeSlow(obj);
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0001BFE0 File Offset: 0x0001A1E0
		private void FreeSlow(T obj)
		{
			ConcurrentObjectPool<T>.Element[] array = this.items;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].value == null)
				{
					array[i].value = obj;
					return;
				}
			}
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0001C024 File Offset: 0x0001A224
		[Conditional("DEBUG")]
		private void Validate(object obj)
		{
			ConcurrentObjectPool<T>.Element[] array = this.items;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].value == null)
				{
					return;
				}
			}
		}

		// Token: 0x0400036C RID: 876
		[Nullable(2)]
		private T firstItem;

		// Token: 0x0400036D RID: 877
		[Nullable(new byte[]
		{
			1,
			0,
			0
		})]
		private readonly ConcurrentObjectPool<T>.Element[] items;

		// Token: 0x0400036E RID: 878
		[Nullable(new byte[]
		{
			1,
			0
		})]
		private readonly ConcurrentObjectPool<T>.Factory factory;

		// Token: 0x02000190 RID: 400
		[NullableContext(0)]
		[DebuggerDisplay("{value,nq}")]
		private struct Element
		{
			// Token: 0x0400036F RID: 879
			[Nullable(2)]
			internal T value;
		}

		// Token: 0x02000191 RID: 401
		// (Invoke) Token: 0x06000864 RID: 2148
		[NullableContext(0)]
		internal delegate T Factory();
	}
}
