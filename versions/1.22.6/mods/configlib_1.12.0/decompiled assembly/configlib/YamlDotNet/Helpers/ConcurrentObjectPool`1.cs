using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace YamlDotNet.Helpers
{
	// Token: 0x02000193 RID: 403
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerStepThrough]
	internal sealed class ConcurrentObjectPool<T> where T : class
	{
		// Token: 0x06000897 RID: 2199 RVA: 0x0001C81C File Offset: 0x0001AA1C
		internal ConcurrentObjectPool([Nullable(new byte[]
		{
			1,
			0
		})] ConcurrentObjectPool<T>.Factory factory) : this(factory, Environment.ProcessorCount * 2)
		{
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0001C82C File Offset: 0x0001AA2C
		internal ConcurrentObjectPool([Nullable(new byte[]
		{
			1,
			0
		})] ConcurrentObjectPool<T>.Factory factory, int size)
		{
			this.factory = factory;
			this.items = new ConcurrentObjectPool<T>.Element[size - 1];
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0001C849 File Offset: 0x0001AA49
		private T CreateInstance()
		{
			return this.factory.Invoke();
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0001C858 File Offset: 0x0001AA58
		internal T Allocate()
		{
			T t = this.firstItem;
			if (t == null || t != Interlocked.CompareExchange<T>(ref this.firstItem, default(T), t))
			{
				t = this.AllocateSlow();
			}
			return t;
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0001C8A0 File Offset: 0x0001AAA0
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

		// Token: 0x0600089C RID: 2204 RVA: 0x0001C907 File Offset: 0x0001AB07
		internal void Free(T obj)
		{
			if (this.firstItem == null)
			{
				this.firstItem = obj;
				return;
			}
			this.FreeSlow(obj);
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0001C928 File Offset: 0x0001AB28
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

		// Token: 0x0600089E RID: 2206 RVA: 0x0001C96C File Offset: 0x0001AB6C
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

		// Token: 0x0400039A RID: 922
		[Nullable(2)]
		private T firstItem;

		// Token: 0x0400039B RID: 923
		[Nullable(new byte[]
		{
			1,
			0,
			0
		})]
		private readonly ConcurrentObjectPool<T>.Element[] items;

		// Token: 0x0400039C RID: 924
		[Nullable(new byte[]
		{
			1,
			0
		})]
		private readonly ConcurrentObjectPool<T>.Factory factory;

		// Token: 0x02000194 RID: 404
		[NullableContext(0)]
		[DebuggerDisplay("{value,nq}")]
		private struct Element
		{
			// Token: 0x0400039D RID: 925
			[Nullable(2)]
			internal T value;
		}

		// Token: 0x02000195 RID: 405
		// (Invoke) Token: 0x060008A0 RID: 2208
		[NullableContext(0)]
		internal delegate T Factory();
	}
}
