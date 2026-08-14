using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using ABI.System.Collections.Generic;
using WinRT;

namespace System.Collections.Generic
{
	// Token: 0x02000014 RID: 20
	internal sealed class IListImpl<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IWinRTObject, IDynamicInterfaceCastable
	{
		// Token: 0x0600009B RID: 155 RVA: 0x000035E7 File Offset: 0x000017E7
		private IObjectReference Make_IListObjRef()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.__iListObjRef, this._inner.As<IList<T>.Vftbl>(), null);
			return this.__iListObjRef;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00003609 File Offset: 0x00001809
		private IObjectReference iListObjRef
		{
			get
			{
				return this.__iListObjRef ?? this.Make_IListObjRef();
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000361D File Offset: 0x0000181D
		private IObjectReference Make_IEnumerableObjRef()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.__iEnumerableObjRef, this._inner.As<IEnumerable<T>.Vftbl>(), null);
			return this.__iEnumerableObjRef;
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600009E RID: 158 RVA: 0x0000363F File Offset: 0x0000183F
		private IObjectReference iEnumerableObjRef
		{
			get
			{
				return this.__iEnumerableObjRef ?? this.Make_IEnumerableObjRef();
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003653 File Offset: 0x00001853
		internal IListImpl(IObjectReference _inner)
		{
			this._inner = _inner;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003662 File Offset: 0x00001862
		public static IListImpl<T> CreateRcw(IInspectable obj)
		{
			return new IListImpl<T>(obj.ObjRef);
		}

		// Token: 0x17000021 RID: 33
		public T this[int index]
		{
			get
			{
				return IListMethods<T>.Indexer_Get(this.iListObjRef, index);
			}
			set
			{
				IListMethods<T>.Indexer_Set(this.iListObjRef, index, value);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000368C File Offset: 0x0000188C
		public int Count
		{
			get
			{
				return IListMethods<T>.get_Count(this.iListObjRef);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00003699 File Offset: 0x00001899
		public bool IsReadOnly
		{
			get
			{
				return IListMethods<T>.get_IsReadOnly(this.iListObjRef);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x000036A6 File Offset: 0x000018A6
		IObjectReference IWinRTObject.NativeObject
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x000036AE File Offset: 0x000018AE
		bool IWinRTObject.HasUnwrappableNativeObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000036B1 File Offset: 0x000018B1
		private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
			return this._queryInterfaceCache;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x000036CD File Offset: 0x000018CD
		ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
		{
			get
			{
				return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000036E1 File Offset: 0x000018E1
		private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
			return this._additionalTypeData;
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000036FD File Offset: 0x000018FD
		ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
		{
			get
			{
				return this._additionalTypeData ?? this.MakeAdditionalTypeData();
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003711 File Offset: 0x00001911
		public void Add(T item)
		{
			IListMethods<T>.Add(this.iListObjRef, item);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000371F File Offset: 0x0000191F
		public void Clear()
		{
			IListMethods<T>.Clear(this.iListObjRef);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000372C File Offset: 0x0000192C
		public bool Contains(T item)
		{
			return IListMethods<T>.Contains(this.iListObjRef, item);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000373A File Offset: 0x0000193A
		public void CopyTo(T[] array, int arrayIndex)
		{
			IListMethods<T>.CopyTo(this.iListObjRef, array, arrayIndex);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003749 File Offset: 0x00001949
		public IEnumerator<T> GetEnumerator()
		{
			return IEnumerableMethods<T>.GetEnumerator(this.iEnumerableObjRef);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003756 File Offset: 0x00001956
		public int IndexOf(T item)
		{
			return IListMethods<T>.IndexOf(this.iListObjRef, item);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003764 File Offset: 0x00001964
		public void Insert(int index, T item)
		{
			IListMethods<T>.Insert(this.iListObjRef, index, item);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003773 File Offset: 0x00001973
		public bool Remove(T item)
		{
			return IListMethods<T>.Remove(this.iListObjRef, item);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003781 File Offset: 0x00001981
		public void RemoveAt(int index)
		{
			IListMethods<T>.RemoveAt(this.iListObjRef, index);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000378F File Offset: 0x0000198F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400002A RID: 42
		private readonly IObjectReference _inner;

		// Token: 0x0400002B RID: 43
		private volatile IObjectReference __iListObjRef;

		// Token: 0x0400002C RID: 44
		private volatile IObjectReference __iEnumerableObjRef;

		// Token: 0x0400002D RID: 45
		private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

		// Token: 0x0400002E RID: 46
		private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
	}
}
