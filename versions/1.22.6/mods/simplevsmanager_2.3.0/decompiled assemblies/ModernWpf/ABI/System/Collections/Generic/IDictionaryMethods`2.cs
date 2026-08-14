using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ABI.Windows.Foundation.Collections;
using WinRT;

namespace ABI.System.Collections.Generic
{
	// Token: 0x02000157 RID: 343
	internal static class IDictionaryMethods<K, V>
	{
		// Token: 0x06000AEC RID: 2796 RVA: 0x0001E714 File Offset: 0x0001C914
		public static int get_Count(IObjectReference obj)
		{
			uint num = IMapMethods<K, V>.get_Size(obj);
			if (2147483647U < num)
			{
				throw new InvalidOperationException(WinRTRuntimeErrorStrings.InvalidOperation_CollectionBackingDictionaryTooLarge);
			}
			return (int)num;
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x0001E73C File Offset: 0x0001C93C
		public static bool get_IsReadOnly(IObjectReference _)
		{
			return false;
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0001E73F File Offset: 0x0001C93F
		public static void Add(IObjectReference obj, KeyValuePair<K, V> item)
		{
			IMapMethods<K, V>.Insert(obj, item.Key, item.Value);
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0001E756 File Offset: 0x0001C956
		public static void Clear(IObjectReference obj)
		{
			IMapMethods<K, V>.Clear(obj);
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0001E760 File Offset: 0x0001C960
		public static bool Contains(IObjectReference obj, Dictionary<K, ValueTuple<IntPtr, V>> __lookupCache, KeyValuePair<K, V> item)
		{
			if (!IMapMethods<K, V>.HasKey(obj, item.Key))
			{
				return false;
			}
			V v = IMapMethods<K, V>.Lookup(obj, __lookupCache, item.Key);
			return EqualityComparer<V>.Default.Equals(v, item.Value);
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0001E7A0 File Offset: 0x0001C9A0
		public static void CopyTo(IObjectReference obj, IObjectReference iEnumerableObjRef, KeyValuePair<K, V>[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			if (array.Length <= arrayIndex && IDictionaryMethods<K, V>.get_Count(obj) > 0)
			{
				throw new ArgumentException(WinRTRuntimeErrorStrings.Argument_IndexOutOfArrayBounds);
			}
			if (array.Length - arrayIndex < IDictionaryMethods<K, V>.get_Count(obj))
			{
				throw new ArgumentException(WinRTRuntimeErrorStrings.Argument_InsufficientSpaceToCopyCollection);
			}
			foreach (KeyValuePair<K, V> keyValuePair in new IEnumerableImpl<KeyValuePair<K, V>>(iEnumerableObjRef))
			{
				array[arrayIndex++] = keyValuePair;
			}
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0001E844 File Offset: 0x0001CA44
		public static bool Remove(IObjectReference obj, KeyValuePair<K, V> item)
		{
			IMapMethods<K, V>.Remove(obj, item.Key);
			return true;
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0001E854 File Offset: 0x0001CA54
		public static V Indexer_Get(IObjectReference obj, Dictionary<K, ValueTuple<IntPtr, V>> __lookupCache, K key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return IDictionaryMethods<K, V>.Lookup(obj, __lookupCache, key);
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0001E871 File Offset: 0x0001CA71
		public static void Indexer_Set(IObjectReference obj, K key, V value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			IDictionaryMethods<K, V>.Insert(obj, key, value);
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0001E88F File Offset: 0x0001CA8F
		public static ICollection<K> get_Keys(IObjectReference obj)
		{
			return new IDictionaryMethods<K, V>.DictionaryKeyCollection(obj);
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0001E897 File Offset: 0x0001CA97
		public static ICollection<V> get_Values(IObjectReference obj)
		{
			return new IDictionaryMethods<K, V>.DictionaryValueCollection(obj);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0001E89F File Offset: 0x0001CA9F
		public static bool ContainsKey(IObjectReference obj, K key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return IMapMethods<K, V>.HasKey(obj, key);
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0001E8BB File Offset: 0x0001CABB
		public static void Add(IObjectReference obj, K key, V value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (IDictionaryMethods<K, V>.ContainsKey(obj, key))
			{
				throw new ArgumentException(WinRTRuntimeErrorStrings.Argument_AddingDuplicate);
			}
			IDictionaryMethods<K, V>.Insert(obj, key, value);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0001E8F0 File Offset: 0x0001CAF0
		public static bool Remove(IObjectReference obj, K key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!IMapMethods<K, V>.HasKey(obj, key))
			{
				return false;
			}
			bool result;
			try
			{
				IMapMethods<K, V>.Remove(obj, key);
				result = true;
			}
			catch (Exception ex)
			{
				if (-2147483637 != ex.HResult)
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0001E94C File Offset: 0x0001CB4C
		public static bool TryGetValue(IObjectReference obj, Dictionary<K, ValueTuple<IntPtr, V>> __lookupCache, K key, out V value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!IMapMethods<K, V>.HasKey(obj, key))
			{
				value = default(V);
				return false;
			}
			bool result;
			try
			{
				value = IDictionaryMethods<K, V>.Lookup(obj, __lookupCache, key);
				result = true;
			}
			catch (KeyNotFoundException)
			{
				value = default(V);
				result = false;
			}
			return result;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0001E9B0 File Offset: 0x0001CBB0
		private static V Lookup(IObjectReference obj, Dictionary<K, ValueTuple<IntPtr, V>> __lookupCache, K key)
		{
			V result;
			try
			{
				result = IMapMethods<K, V>.Lookup(obj, __lookupCache, key);
			}
			catch (Exception ex)
			{
				if (-2147483637 == ex.HResult)
				{
					throw new KeyNotFoundException(WinRTRuntimeErrorStrings.Arg_KeyNotFound);
				}
				throw;
			}
			return result;
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0001E9F4 File Offset: 0x0001CBF4
		private static bool Insert(IObjectReference obj, K key, V value)
		{
			return IMapMethods<K, V>.Insert(obj, key, value);
		}

		// Token: 0x0200037F RID: 895
		private sealed class DictionaryKeyCollection : ICollection<!0>, IEnumerable<!0>, IEnumerable
		{
			// Token: 0x06001918 RID: 6424 RVA: 0x000451E2 File Offset: 0x000433E2
			public DictionaryKeyCollection(IObjectReference iDictionaryObjRef)
			{
				if (iDictionaryObjRef == null)
				{
					throw new ArgumentNullException("iDictionaryObjRef");
				}
				this.iDictionaryObjRef = iDictionaryObjRef;
			}

			// Token: 0x06001919 RID: 6425 RVA: 0x000451FF File Offset: 0x000433FF
			private IObjectReference Make_IEnumerableObjRef()
			{
				Interlocked.CompareExchange<IObjectReference>(ref this.__iEnumerableObjRef, this.iDictionaryObjRef.As<IEnumerable<KeyValuePair<K, V>>.Vftbl>(), null);
				return this.__iEnumerableObjRef;
			}

			// Token: 0x170004C1 RID: 1217
			// (get) Token: 0x0600191A RID: 6426 RVA: 0x00045221 File Offset: 0x00043421
			private IObjectReference iEnumerableObjRef
			{
				get
				{
					return this.__iEnumerableObjRef ?? this.Make_IEnumerableObjRef();
				}
			}

			// Token: 0x0600191B RID: 6427 RVA: 0x00045238 File Offset: 0x00043438
			public void CopyTo(K[] array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (array.Length <= index && this.Count > 0)
				{
					throw new ArgumentException(WinRTRuntimeErrorStrings.Arg_IndexOutOfRangeException);
				}
				if (array.Length - index < IDictionaryMethods<K, V>.get_Count(this.iDictionaryObjRef))
				{
					throw new ArgumentException(WinRTRuntimeErrorStrings.Argument_InsufficientSpaceToCopyCollection);
				}
				int num = index;
				foreach (KeyValuePair<K, V> keyValuePair in new IEnumerableImpl<KeyValuePair<K, V>>(this.iEnumerableObjRef))
				{
					array[num++] = keyValuePair.Key;
				}
			}

			// Token: 0x170004C2 RID: 1218
			// (get) Token: 0x0600191C RID: 6428 RVA: 0x000452EC File Offset: 0x000434EC
			public int Count
			{
				get
				{
					return IDictionaryMethods<K, V>.get_Count(this.iDictionaryObjRef);
				}
			}

			// Token: 0x170004C3 RID: 1219
			// (get) Token: 0x0600191D RID: 6429 RVA: 0x000452F9 File Offset: 0x000434F9
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x0600191E RID: 6430 RVA: 0x000452FC File Offset: 0x000434FC
			void ICollection<!0>.Add(K item)
			{
				throw new NotSupportedException(WinRTRuntimeErrorStrings.NotSupported_KeyCollectionSet);
			}

			// Token: 0x0600191F RID: 6431 RVA: 0x00045308 File Offset: 0x00043508
			void ICollection<!0>.Clear()
			{
				throw new NotSupportedException(WinRTRuntimeErrorStrings.NotSupported_KeyCollectionSet);
			}

			// Token: 0x06001920 RID: 6432 RVA: 0x00045314 File Offset: 0x00043514
			public bool Contains(K item)
			{
				return IDictionaryMethods<K, V>.ContainsKey(this.iDictionaryObjRef, item);
			}

			// Token: 0x06001921 RID: 6433 RVA: 0x00045322 File Offset: 0x00043522
			bool ICollection<!0>.Remove(K item)
			{
				throw new NotSupportedException(WinRTRuntimeErrorStrings.NotSupported_KeyCollectionSet);
			}

			// Token: 0x06001922 RID: 6434 RVA: 0x0004532E File Offset: 0x0004352E
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06001923 RID: 6435 RVA: 0x00045336 File Offset: 0x00043536
			public IEnumerator<K> GetEnumerator()
			{
				return new IDictionaryMethods<K, V>.DictionaryKeyCollection.DictionaryKeyEnumerator(this.iEnumerableObjRef);
			}

			// Token: 0x040008D0 RID: 2256
			private readonly IObjectReference iDictionaryObjRef;

			// Token: 0x040008D1 RID: 2257
			private volatile IObjectReference __iEnumerableObjRef;

			// Token: 0x0200042C RID: 1068
			private sealed class DictionaryKeyEnumerator : IEnumerator<K>, IEnumerator, IDisposable
			{
				// Token: 0x06001C53 RID: 7251 RVA: 0x0004B9BC File Offset: 0x00049BBC
				public DictionaryKeyEnumerator(IObjectReference iEnumerableObjRef)
				{
					this.iEnumerableObjRef = iEnumerableObjRef;
					this.enumeration = IEnumerableMethods<KeyValuePair<K, V>>.GetEnumerator(iEnumerableObjRef);
				}

				// Token: 0x06001C54 RID: 7252 RVA: 0x0004B9D7 File Offset: 0x00049BD7
				public void Dispose()
				{
					this.enumeration.Dispose();
				}

				// Token: 0x06001C55 RID: 7253 RVA: 0x0004B9E4 File Offset: 0x00049BE4
				public bool MoveNext()
				{
					return this.enumeration.MoveNext();
				}

				// Token: 0x1700052A RID: 1322
				// (get) Token: 0x06001C56 RID: 7254 RVA: 0x0004B9F1 File Offset: 0x00049BF1
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				// Token: 0x1700052B RID: 1323
				// (get) Token: 0x06001C57 RID: 7255 RVA: 0x0004BA00 File Offset: 0x00049C00
				public K Current
				{
					get
					{
						KeyValuePair<K, V> keyValuePair = this.enumeration.Current;
						return keyValuePair.Key;
					}
				}

				// Token: 0x06001C58 RID: 7256 RVA: 0x0004BA20 File Offset: 0x00049C20
				public void Reset()
				{
					this.enumeration = IEnumerableMethods<KeyValuePair<K, V>>.GetEnumerator(this.iEnumerableObjRef);
				}

				// Token: 0x04000A46 RID: 2630
				private readonly IObjectReference iEnumerableObjRef;

				// Token: 0x04000A47 RID: 2631
				private IEnumerator<KeyValuePair<K, V>> enumeration;
			}
		}

		// Token: 0x02000380 RID: 896
		private sealed class DictionaryValueCollection : ICollection<V>, IEnumerable<V>, IEnumerable
		{
			// Token: 0x06001924 RID: 6436 RVA: 0x00045343 File Offset: 0x00043543
			public DictionaryValueCollection(IObjectReference iDictionaryObjRef)
			{
				this.iDictionaryObjRef = iDictionaryObjRef;
			}

			// Token: 0x06001925 RID: 6437 RVA: 0x00045352 File Offset: 0x00043552
			private IObjectReference Make_IEnumerableObjRef()
			{
				Interlocked.CompareExchange<IObjectReference>(ref this.__iEnumerableObjRef, this.iDictionaryObjRef.As<IEnumerable<KeyValuePair<K, V>>.Vftbl>(), null);
				return this.__iEnumerableObjRef;
			}

			// Token: 0x170004C4 RID: 1220
			// (get) Token: 0x06001926 RID: 6438 RVA: 0x00045374 File Offset: 0x00043574
			private IObjectReference iEnumerableObjRef
			{
				get
				{
					return this.__iEnumerableObjRef ?? this.Make_IEnumerableObjRef();
				}
			}

			// Token: 0x06001927 RID: 6439 RVA: 0x00045388 File Offset: 0x00043588
			public void CopyTo(V[] array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (array.Length <= index && this.Count > 0)
				{
					throw new ArgumentException(WinRTRuntimeErrorStrings.Arg_IndexOutOfRangeException);
				}
				if (array.Length - index < IDictionaryMethods<K, V>.get_Count(this.iDictionaryObjRef))
				{
					throw new ArgumentException(WinRTRuntimeErrorStrings.Argument_InsufficientSpaceToCopyCollection);
				}
				int num = index;
				foreach (KeyValuePair<K, V> keyValuePair in new IEnumerableImpl<KeyValuePair<K, V>>(this.iEnumerableObjRef))
				{
					array[num++] = keyValuePair.Value;
				}
			}

			// Token: 0x170004C5 RID: 1221
			// (get) Token: 0x06001928 RID: 6440 RVA: 0x0004543C File Offset: 0x0004363C
			public int Count
			{
				get
				{
					return IDictionaryMethods<K, V>.get_Count(this.iDictionaryObjRef);
				}
			}

			// Token: 0x170004C6 RID: 1222
			// (get) Token: 0x06001929 RID: 6441 RVA: 0x00045449 File Offset: 0x00043649
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x0600192A RID: 6442 RVA: 0x0004544C File Offset: 0x0004364C
			void ICollection<!1>.Add(V item)
			{
				throw new NotSupportedException(WinRTRuntimeErrorStrings.NotSupported_ValueCollectionSet);
			}

			// Token: 0x0600192B RID: 6443 RVA: 0x00045458 File Offset: 0x00043658
			void ICollection<!1>.Clear()
			{
				throw new NotSupportedException(WinRTRuntimeErrorStrings.NotSupported_ValueCollectionSet);
			}

			// Token: 0x0600192C RID: 6444 RVA: 0x00045464 File Offset: 0x00043664
			public bool Contains(V item)
			{
				EqualityComparer<V> @default = EqualityComparer<V>.Default;
				foreach (V v in this)
				{
					if (@default.Equals(item, v))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x0600192D RID: 6445 RVA: 0x000454BC File Offset: 0x000436BC
			bool ICollection<!1>.Remove(V item)
			{
				throw new NotSupportedException(WinRTRuntimeErrorStrings.NotSupported_ValueCollectionSet);
			}

			// Token: 0x0600192E RID: 6446 RVA: 0x000454C8 File Offset: 0x000436C8
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x0600192F RID: 6447 RVA: 0x000454D0 File Offset: 0x000436D0
			public IEnumerator<V> GetEnumerator()
			{
				return new IDictionaryMethods<K, V>.DictionaryValueCollection.DictionaryValueEnumerator(this.iEnumerableObjRef);
			}

			// Token: 0x040008D2 RID: 2258
			private readonly IObjectReference iDictionaryObjRef;

			// Token: 0x040008D3 RID: 2259
			private volatile IObjectReference __iEnumerableObjRef;

			// Token: 0x0200042D RID: 1069
			private sealed class DictionaryValueEnumerator : IEnumerator<V>, IEnumerator, IDisposable
			{
				// Token: 0x06001C59 RID: 7257 RVA: 0x0004BA33 File Offset: 0x00049C33
				public DictionaryValueEnumerator(IObjectReference iEnumerableObjRef)
				{
					this.iEnumerableObjRef = iEnumerableObjRef;
					this.enumeration = IEnumerableMethods<KeyValuePair<K, V>>.GetEnumerator(iEnumerableObjRef);
				}

				// Token: 0x06001C5A RID: 7258 RVA: 0x0004BA4E File Offset: 0x00049C4E
				public void Dispose()
				{
					this.enumeration.Dispose();
				}

				// Token: 0x06001C5B RID: 7259 RVA: 0x0004BA5B File Offset: 0x00049C5B
				public bool MoveNext()
				{
					return this.enumeration.MoveNext();
				}

				// Token: 0x1700052C RID: 1324
				// (get) Token: 0x06001C5C RID: 7260 RVA: 0x0004BA68 File Offset: 0x00049C68
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				// Token: 0x1700052D RID: 1325
				// (get) Token: 0x06001C5D RID: 7261 RVA: 0x0004BA78 File Offset: 0x00049C78
				public V Current
				{
					get
					{
						KeyValuePair<K, V> keyValuePair = this.enumeration.Current;
						return keyValuePair.Value;
					}
				}

				// Token: 0x06001C5E RID: 7262 RVA: 0x0004BA98 File Offset: 0x00049C98
				public void Reset()
				{
					this.enumeration = IEnumerableMethods<KeyValuePair<K, V>>.GetEnumerator(this.iEnumerableObjRef);
				}

				// Token: 0x04000A48 RID: 2632
				private readonly IObjectReference iEnumerableObjRef;

				// Token: 0x04000A49 RID: 2633
				private IEnumerator<KeyValuePair<K, V>> enumeration;
			}
		}
	}
}
