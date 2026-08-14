using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using ABI.System.Collections.Generic;
using WinRT;

namespace System.Collections.Generic
{
	// Token: 0x02000015 RID: 21
	internal sealed class IReadOnlyDictionaryImpl<K, V> : IReadOnlyDictionary<K, V>, IEnumerable<KeyValuePair<K, V>>, IEnumerable, IReadOnlyCollection<KeyValuePair<K, V>>, IWinRTObject, IDynamicInterfaceCastable
	{
		// Token: 0x060000B5 RID: 181 RVA: 0x00003797 File Offset: 0x00001997
		internal IReadOnlyDictionaryImpl(IObjectReference _inner)
		{
			this._inner = _inner;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000037A6 File Offset: 0x000019A6
		public static IReadOnlyDictionaryImpl<K, V> CreateRcw(IInspectable obj)
		{
			return new IReadOnlyDictionaryImpl<K, V>(obj.ObjRef);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000037B3 File Offset: 0x000019B3
		private IObjectReference Make_IDictionaryObjRef()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.__iReadOnlyDictionaryObjRef, this._inner.As<IReadOnlyDictionary<K, V>.Vftbl>(), null);
			return this.__iReadOnlyDictionaryObjRef;
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x000037D5 File Offset: 0x000019D5
		private IObjectReference iReadOnlyDictionaryObjRef
		{
			get
			{
				return this.__iReadOnlyDictionaryObjRef ?? this.Make_IDictionaryObjRef();
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000037E9 File Offset: 0x000019E9
		private IObjectReference Make_IEnumerableObjRef()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.__iEnumerableObjRef, this._inner.As<IEnumerable<KeyValuePair<K, V>>.Vftbl>(), null);
			return this.__iEnumerableObjRef;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000BA RID: 186 RVA: 0x0000380B File Offset: 0x00001A0B
		private IObjectReference iEnumerableObjRef
		{
			get
			{
				return this.__iEnumerableObjRef ?? this.Make_IEnumerableObjRef();
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000BB RID: 187 RVA: 0x0000381F File Offset: 0x00001A1F
		IObjectReference IWinRTObject.NativeObject
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00003827 File Offset: 0x00001A27
		bool IWinRTObject.HasUnwrappableNativeObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000382A File Offset: 0x00001A2A
		private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
			return this._queryInterfaceCache;
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00003846 File Offset: 0x00001A46
		ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
		{
			get
			{
				return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000385A File Offset: 0x00001A5A
		private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
			return this._additionalTypeData;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00003876 File Offset: 0x00001A76
		ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
		{
			get
			{
				return this._additionalTypeData ?? this.MakeAdditionalTypeData();
			}
		}

		// Token: 0x1700002E RID: 46
		public V this[K key]
		{
			get
			{
				return IReadOnlyDictionaryMethods<K, V>.Indexer_Get(this.iReadOnlyDictionaryObjRef, key);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00003898 File Offset: 0x00001A98
		public IEnumerable<K> Keys
		{
			get
			{
				return IReadOnlyDictionaryMethods<K, V>.get_Keys(this.iReadOnlyDictionaryObjRef);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x000038A5 File Offset: 0x00001AA5
		public IEnumerable<V> Values
		{
			get
			{
				return IReadOnlyDictionaryMethods<K, V>.get_Values(this.iReadOnlyDictionaryObjRef);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000038B2 File Offset: 0x00001AB2
		public int Count
		{
			get
			{
				return IReadOnlyDictionaryMethods<K, V>.get_Count(this.iReadOnlyDictionaryObjRef);
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000038BF File Offset: 0x00001ABF
		public bool ContainsKey(K key)
		{
			return IReadOnlyDictionaryMethods<K, V>.ContainsKey(this.iReadOnlyDictionaryObjRef, key);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000038CD File Offset: 0x00001ACD
		public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
		{
			return IEnumerableMethods<KeyValuePair<K, V>>.GetEnumerator(this.iEnumerableObjRef);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000038DA File Offset: 0x00001ADA
		public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
		{
			return IReadOnlyDictionaryMethods<K, V>.TryGetValue(this.iReadOnlyDictionaryObjRef, key, ref value);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000038E9 File Offset: 0x00001AE9
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400002F RID: 47
		private IObjectReference _inner;

		// Token: 0x04000030 RID: 48
		private volatile IObjectReference __iReadOnlyDictionaryObjRef;

		// Token: 0x04000031 RID: 49
		private volatile IObjectReference __iEnumerableObjRef;

		// Token: 0x04000032 RID: 50
		private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

		// Token: 0x04000033 RID: 51
		private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
	}
}
