using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;

namespace YamlDotNet.Core.ObjectPool
{
	// Token: 0x020000F9 RID: 249
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1
	})]
	internal class DefaultObjectPool<T> : ObjectPool<T> where T : class
	{
		// Token: 0x0600062D RID: 1581 RVA: 0x00017263 File Offset: 0x00015463
		public DefaultObjectPool(IPooledObjectPolicy<T> policy) : this(policy, Environment.ProcessorCount * 2)
		{
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00017274 File Offset: 0x00015474
		public DefaultObjectPool(IPooledObjectPolicy<T> policy, int maximumRetained)
		{
			this.items = new ConcurrentQueue<T>();
			base..ctor();
			this.createFunc = new Func<T>(policy.Create);
			this.returnFunc = new Func<T, bool>(policy.Return);
			this.maxCapacity = maximumRetained - 1;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x000172C4 File Offset: 0x000154C4
		public override T Get()
		{
			T t = this.fastItem;
			if (t != null && Interlocked.CompareExchange<T>(ref this.fastItem, default(T), t) == t)
			{
				return t;
			}
			if (this.items.TryDequeue(ref t))
			{
				Interlocked.Decrement(ref this.numItems);
				return t;
			}
			return this.createFunc.Invoke();
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0001732C File Offset: 0x0001552C
		public override void Return(T obj)
		{
			this.ReturnCore(obj);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00017338 File Offset: 0x00015538
		private protected bool ReturnCore(T obj)
		{
			if (!this.returnFunc.Invoke(obj))
			{
				return false;
			}
			if (this.fastItem == null && Interlocked.CompareExchange<T>(ref this.fastItem, obj, default(T)) == null)
			{
				return true;
			}
			if (Interlocked.Increment(ref this.numItems) <= this.maxCapacity)
			{
				this.items.Enqueue(obj);
				return true;
			}
			Interlocked.Decrement(ref this.numItems);
			return false;
		}

		// Token: 0x040001D4 RID: 468
		private readonly Func<T> createFunc;

		// Token: 0x040001D5 RID: 469
		private readonly Func<T, bool> returnFunc;

		// Token: 0x040001D6 RID: 470
		private readonly int maxCapacity;

		// Token: 0x040001D7 RID: 471
		private int numItems;

		// Token: 0x040001D8 RID: 472
		private protected readonly ConcurrentQueue<T> items;

		// Token: 0x040001D9 RID: 473
		[Nullable(2)]
		private protected T fastItem;
	}
}
