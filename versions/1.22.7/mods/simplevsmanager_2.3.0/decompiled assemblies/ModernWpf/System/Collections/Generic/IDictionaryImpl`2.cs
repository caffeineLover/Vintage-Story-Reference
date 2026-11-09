using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using ABI.System.Collections.Generic;
using WinRT;

namespace System.Collections.Generic
{
	// Token: 0x02000012 RID: 18
	internal sealed class IDictionaryImpl<K, V> : IDictionary<K, V>, ICollection<KeyValuePair<K, V>>, IEnumerable<KeyValuePair<K, V>>, IEnumerable, IWinRTObject, IDynamicInterfaceCastable
	{
		// Token: 0x06000072 RID: 114 RVA: 0x00003319 File Offset: 0x00001519
		internal IDictionaryImpl(IObjectReference _inner)
		{
			this._inner = _inner;
			this._lookupCache = new Dictionary<K, ValueTuple<IntPtr, V>>();
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003333 File Offset: 0x00001533
		public static IDictionaryImpl<K, V> CreateRcw(IInspectable obj)
		{
			return new IDictionaryImpl<K, V>(obj.ObjRef);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003340 File Offset: 0x00001540
		private IObjectReference Make_IDictionaryObjRef()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.__iDictionaryObjRef, this._inner.As<IDictionary<K, V>.Vftbl>(), null);
			return this.__iDictionaryObjRef;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00003362 File Offset: 0x00001562
		private IObjectReference iDictionaryObjRef
		{
			get
			{
				return this.__iDictionaryObjRef ?? this.Make_IDictionaryObjRef();
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003376 File Offset: 0x00001576
		private IObjectReference Make_IEnumerableObjRef()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.__iEnumerableObjRef, this._inner.As<IEnumerable<KeyValuePair<K, V>>.Vftbl>(), null);
			return this.__iEnumerableObjRef;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003398 File Offset: 0x00001598
		private IObjectReference iEnumerableObjRef
		{
			get
			{
				return this.__iEnumerableObjRef ?? this.Make_IEnumerableObjRef();
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000078 RID: 120 RVA: 0x000033AC File Offset: 0x000015AC
		IObjectReference IWinRTObject.NativeObject
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000079 RID: 121 RVA: 0x000033B4 File Offset: 0x000015B4
		bool IWinRTObject.HasUnwrappableNativeObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000033B7 File Offset: 0x000015B7
		private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
			return this._queryInterfaceCache;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000033D3 File Offset: 0x000015D3
		ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
		{
			get
			{
				return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000033E7 File Offset: 0x000015E7
		private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
			return this._additionalTypeData;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003403 File Offset: 0x00001603
		ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
		{
			get
			{
				return this._additionalTypeData ?? this.MakeAdditionalTypeData();
			}
		}

		// Token: 0x17000015 RID: 21
		public V this[K key]
		{
			get
			{
				return IDictionaryMethods<K, V>.Indexer_Get(this.iDictionaryObjRef, this._lookupCache, key);
			}
			set
			{
				IDictionaryMethods<K, V>.Indexer_Set(this.iDictionaryObjRef, key, value);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000080 RID: 128 RVA: 0x0000343A File Offset: 0x0000163A
		public ICollection<K> Keys
		{
			get
			{
				return IDictionaryMethods<K, V>.get_Keys(this.iDictionaryObjRef);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003447 File Offset: 0x00001647
		public ICollection<V> Values
		{
			get
			{
				return IDictionaryMethods<K, V>.get_Values(this.iDictionaryObjRef);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003454 File Offset: 0x00001654
		public int Count
		{
			get
			{
				return IDictionaryMethods<K, V>.get_Count(this.iDictionaryObjRef);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00003461 File Offset: 0x00001661
		public bool IsReadOnly
		{
			get
			{
				return IDictionaryMethods<K, V>.get_IsReadOnly(this.iDictionaryObjRef);
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000346E File Offset: 0x0000166E
		public void Add(K key, V value)
		{
			IDictionaryMethods<K, V>.Add(this.iDictionaryObjRef, key, value);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000347D File Offset: 0x0000167D
		public void Add(KeyValuePair<K, V> item)
		{
			IDictionaryMethods<K, V>.Add(this.iDictionaryObjRef, item);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000348B File Offset: 0x0000168B
		public void Clear()
		{
			IDictionaryMethods<K, V>.Clear(this.iDictionaryObjRef);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003498 File Offset: 0x00001698
		public bool Contains(KeyValuePair<K, V> item)
		{
			return IDictionaryMethods<K, V>.Contains(this.iDictionaryObjRef, this._lookupCache, item);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000034AC File Offset: 0x000016AC
		public bool ContainsKey(K key)
		{
			return IDictionaryMethods<K, V>.ContainsKey(this.iDictionaryObjRef, key);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000034BA File Offset: 0x000016BA
		public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
		{
			IDictionaryMethods<K, V>.CopyTo(this.iDictionaryObjRef, this.iEnumerableObjRef, array, arrayIndex);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000034CF File Offset: 0x000016CF
		public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
		{
			return IEnumerableMethods<KeyValuePair<K, V>>.GetEnumerator(this.iEnumerableObjRef);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000034DC File Offset: 0x000016DC
		public bool Remove(K key)
		{
			return IDictionaryMethods<K, V>.Remove(this.iDictionaryObjRef, key);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000034EA File Offset: 0x000016EA
		public bool Remove(KeyValuePair<K, V> item)
		{
			return IDictionaryMethods<K, V>.Remove(this.iDictionaryObjRef, item);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000034F8 File Offset: 0x000016F8
		public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
		{
			return IDictionaryMethods<K, V>.TryGetValue(this.iDictionaryObjRef, this._lookupCache, key, ref value);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000350D File Offset: 0x0000170D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000020 RID: 32
		private IObjectReference _inner;

		// Token: 0x04000021 RID: 33
		private Dictionary<K, ValueTuple<IntPtr, V>> _lookupCache;

		// Token: 0x04000022 RID: 34
		private volatile IObjectReference __iDictionaryObjRef;

		// Token: 0x04000023 RID: 35
		private volatile IObjectReference __iEnumerableObjRef;

		// Token: 0x04000024 RID: 36
		private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

		// Token: 0x04000025 RID: 37
		private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
	}
}
