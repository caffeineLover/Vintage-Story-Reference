using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using ABI.System.Collections.Generic;
using WinRT;

namespace System.Collections.Generic
{
	// Token: 0x02000016 RID: 22
	internal sealed class IReadOnlyListImpl<T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, IWinRTObject, IDynamicInterfaceCastable
	{
		// Token: 0x060000C9 RID: 201 RVA: 0x000038F1 File Offset: 0x00001AF1
		private IObjectReference Make_IListObjRef()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.__iReadOnlyListObjRef, this._inner.As<IReadOnlyList<T>.Vftbl>(), null);
			return this.__iReadOnlyListObjRef;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00003913 File Offset: 0x00001B13
		private IObjectReference iReadOnlyListObjRef
		{
			get
			{
				return this.__iReadOnlyListObjRef ?? this.Make_IListObjRef();
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003927 File Offset: 0x00001B27
		private IObjectReference Make_IEnumerableObjRef()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.__iEnumerableObjRef, this._inner.As<IEnumerable<T>.Vftbl>(), null);
			return this.__iEnumerableObjRef;
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00003949 File Offset: 0x00001B49
		private IObjectReference iEnumerableObjRef
		{
			get
			{
				return this.__iEnumerableObjRef ?? this.Make_IEnumerableObjRef();
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000395D File Offset: 0x00001B5D
		internal IReadOnlyListImpl(IObjectReference _inner)
		{
			this._inner = _inner;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000396C File Offset: 0x00001B6C
		public static IReadOnlyListImpl<T> CreateRcw(IInspectable obj)
		{
			return new IReadOnlyListImpl<T>(obj.ObjRef);
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00003979 File Offset: 0x00001B79
		IObjectReference IWinRTObject.NativeObject
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00003981 File Offset: 0x00001B81
		bool IWinRTObject.HasUnwrappableNativeObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003984 File Offset: 0x00001B84
		private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
			return this._queryInterfaceCache;
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x000039A0 File Offset: 0x00001BA0
		ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
		{
			get
			{
				return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000039B4 File Offset: 0x00001BB4
		private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
			return this._additionalTypeData;
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x000039D0 File Offset: 0x00001BD0
		ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
		{
			get
			{
				return this._additionalTypeData ?? this.MakeAdditionalTypeData();
			}
		}

		// Token: 0x17000038 RID: 56
		public T this[int index]
		{
			get
			{
				return IReadOnlyListMethods<T>.Indexer_Get(this.iReadOnlyListObjRef, index);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x000039F2 File Offset: 0x00001BF2
		public int Count
		{
			get
			{
				return IReadOnlyListMethods<T>.get_Count(this.iReadOnlyListObjRef);
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000039FF File Offset: 0x00001BFF
		public IEnumerator<T> GetEnumerator()
		{
			return IEnumerableMethods<T>.GetEnumerator(this.iEnumerableObjRef);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00003A0C File Offset: 0x00001C0C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000034 RID: 52
		private readonly IObjectReference _inner;

		// Token: 0x04000035 RID: 53
		private volatile IObjectReference __iReadOnlyListObjRef;

		// Token: 0x04000036 RID: 54
		private volatile IObjectReference __iEnumerableObjRef;

		// Token: 0x04000037 RID: 55
		private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

		// Token: 0x04000038 RID: 56
		private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
	}
}
