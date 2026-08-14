using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using ABI.WinRT.Interop;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020001B4 RID: 436
	internal sealed class Cache
	{
		// Token: 0x06000E00 RID: 3584 RVA: 0x0002A7BE File Offset: 0x000289BE
		private Cache(WinRT.Interop.IWeakReference target, int index, WeakReference<State> state)
		{
			this.target = target;
			this.SetState(index, state);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0002A7E0 File Offset: 0x000289E0
		private Cache Update(WinRT.Interop.IWeakReference target, int index, WeakReference<State> state)
		{
			lock (this)
			{
				using (IObjectReference objectReference = this.target.Resolve(InterfaceIIDs.IUnknown_IID))
				{
					if (objectReference == null)
					{
						this.target = target;
						this.states.Clear();
					}
				}
			}
			this.SetState(index, state);
			return this;
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0002A85C File Offset: 0x00028A5C
		private WeakReference<State> GetState(int index)
		{
			lock (this)
			{
				using (IObjectReference objectReference = this.target.Resolve(InterfaceIIDs.IUnknown_IID))
				{
					if (objectReference == null)
					{
						return null;
					}
				}
			}
			WeakReference<State> result;
			if (this.states.TryGetValue(index, ref result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0002A8D8 File Offset: 0x00028AD8
		private void SetState(int index, WeakReference<State> state)
		{
			this.states[index] = state;
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0002A8E8 File Offset: 0x00028AE8
		public static void Create(IObjectReference obj, int index, WeakReference<State> state)
		{
			WinRT.Interop.IWeakReference target = null;
			ObjectReference<IUnknownVftbl> obj2;
			if (obj.TryAs<IUnknownVftbl>(InterfaceIIDs.IWeakReferenceSource_IID, out obj2) != 0)
			{
				return;
			}
			target = IWeakReferenceSourceMethods.GetWeakReference(obj2);
			Cache.cachesLock.EnterReadLock();
			try
			{
				Cache.caches.AddOrUpdate(obj.ThisPtr, (IntPtr ThisPtr) => new Cache(target, index, state), (IntPtr ThisPtr, Cache cache) => cache.Update(target, index, state));
			}
			finally
			{
				Cache.cachesLock.ExitReadLock();
			}
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0002A97C File Offset: 0x00028B7C
		public static WeakReference<State> GetState(IObjectReference obj, int index)
		{
			Cache cache;
			if (Cache.caches.TryGetValue(obj.ThisPtr, ref cache))
			{
				return cache.GetState(index);
			}
			return null;
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0002A9A8 File Offset: 0x00028BA8
		public static void Remove(IntPtr thisPtr, int index, WeakReference<State> state)
		{
			Cache cache;
			if (Cache.caches.TryGetValue(thisPtr, ref cache))
			{
				cache.states.TryRemove(new KeyValuePair<int, WeakReference<State>>(index, state));
				if (cache.states.IsEmpty)
				{
					Cache.cachesLock.EnterWriteLock();
					try
					{
						if (cache.states.IsEmpty)
						{
							Cache cache2;
							Cache.caches.TryRemove(thisPtr, ref cache2);
						}
					}
					finally
					{
						Cache.cachesLock.ExitWriteLock();
					}
				}
			}
		}

		// Token: 0x040002B8 RID: 696
		private WinRT.Interop.IWeakReference target;

		// Token: 0x040002B9 RID: 697
		private readonly ConcurrentDictionary<int, WeakReference<State>> states = new ConcurrentDictionary<int, WeakReference<State>>();

		// Token: 0x040002BA RID: 698
		private static readonly ReaderWriterLockSlim cachesLock = new ReaderWriterLockSlim();

		// Token: 0x040002BB RID: 699
		private static readonly ConcurrentDictionary<IntPtr, Cache> caches = new ConcurrentDictionary<IntPtr, Cache>();
	}
}
