using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ABI.Windows.Foundation.Collections;
using WinRT;

namespace ABI.System.Collections.Generic
{
	// Token: 0x02000163 RID: 355
	internal static class IReadOnlyDictionaryMethods<K, V>
	{
		// Token: 0x06000B7F RID: 2943 RVA: 0x000202B4 File Offset: 0x0001E4B4
		public static int get_Count(IObjectReference obj)
		{
			uint num = IMapViewMethods<K, V>.get_Size(obj);
			if (2147483647U < num)
			{
				throw new InvalidOperationException(WinRTRuntimeErrorStrings.InvalidOperation_CollectionBackingDictionaryTooLarge);
			}
			return (int)num;
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x000202DC File Offset: 0x0001E4DC
		public static V Indexer_Get(IObjectReference obj, K key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return IReadOnlyDictionaryMethods<K, V>.Lookup(obj, key);
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x000202F8 File Offset: 0x0001E4F8
		public static IEnumerable<K> get_Keys(IObjectReference obj)
		{
			return new IReadOnlyDictionaryMethods<K, V>.ReadOnlyDictionaryKeyCollection(obj);
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00020300 File Offset: 0x0001E500
		public static IEnumerable<V> get_Values(IObjectReference obj)
		{
			return new IReadOnlyDictionaryMethods<K, V>.ReadOnlyDictionaryValueCollection(obj);
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x00020308 File Offset: 0x0001E508
		public static bool ContainsKey(IObjectReference obj, K key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return IMapViewMethods<K, V>.HasKey(obj, key);
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x00020324 File Offset: 0x0001E524
		public static bool TryGetValue(IObjectReference obj, K key, out V value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!IMapViewMethods<K, V>.HasKey(obj, key))
			{
				value = default(V);
				return false;
			}
			bool result;
			try
			{
				value = IMapViewMethods<K, V>.Lookup(obj, key);
				result = true;
			}
			catch (Exception ex)
			{
				if (-2147483637 != ex.HResult)
				{
					throw;
				}
				value = default(V);
				result = false;
			}
			return result;
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x00020394 File Offset: 0x0001E594
		public static V Lookup(IObjectReference obj, K key)
		{
			V result;
			try
			{
				result = IMapViewMethods<K, V>.Lookup(obj, key);
			}
			catch (Exception ex)
			{
				if (-2147483637 == ex.HResult)
				{
					throw new KeyNotFoundException(string.Format(WinRTRuntimeErrorStrings.Arg_KeyNotFoundWithKey, key.ToString()));
				}
				throw;
			}
			return result;
		}

		// Token: 0x02000396 RID: 918
		private sealed class ReadOnlyDictionaryKeyCollection : IEnumerable<!0>, IEnumerable
		{
			// Token: 0x060019C4 RID: 6596 RVA: 0x00047400 File Offset: 0x00045600
			public ReadOnlyDictionaryKeyCollection(IObjectReference iReadOnlyDictionaryObjRef)
			{
				if (iReadOnlyDictionaryObjRef == null)
				{
					throw new ArgumentNullException("iReadOnlyDictionaryObjRef");
				}
				this.iReadOnlyDictionaryObjRef = iReadOnlyDictionaryObjRef;
			}

			// Token: 0x060019C5 RID: 6597 RVA: 0x0004741D File Offset: 0x0004561D
			private IObjectReference Make_IEnumerableObjRef()
			{
				Interlocked.CompareExchange<IObjectReference>(ref this.__iEnumerableObjRef, this.iReadOnlyDictionaryObjRef.As<IEnumerable<KeyValuePair<K, V>>.Vftbl>(), null);
				return this.__iEnumerableObjRef;
			}

			// Token: 0x170004DA RID: 1242
			// (get) Token: 0x060019C6 RID: 6598 RVA: 0x0004743F File Offset: 0x0004563F
			private IObjectReference iEnumerableObjRef
			{
				get
				{
					return this.__iEnumerableObjRef ?? this.Make_IEnumerableObjRef();
				}
			}

			// Token: 0x060019C7 RID: 6599 RVA: 0x00047453 File Offset: 0x00045653
			public IEnumerator<K> GetEnumerator()
			{
				return new IReadOnlyDictionaryMethods<K, V>.ReadOnlyDictionaryKeyCollection.ReadOnlyDictionaryKeyEnumerator(this.iEnumerableObjRef);
			}

			// Token: 0x060019C8 RID: 6600 RVA: 0x00047460 File Offset: 0x00045660
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x04000919 RID: 2329
			private readonly IObjectReference iReadOnlyDictionaryObjRef;

			// Token: 0x0400091A RID: 2330
			private volatile IObjectReference __iEnumerableObjRef;

			// Token: 0x02000431 RID: 1073
			private sealed class ReadOnlyDictionaryKeyEnumerator : IEnumerator<K>, IEnumerator, IDisposable
			{
				// Token: 0x06001C68 RID: 7272 RVA: 0x0004BAFF File Offset: 0x00049CFF
				public ReadOnlyDictionaryKeyEnumerator(IObjectReference iEnumerableObjRef)
				{
					this.iEnumerableObjRef = iEnumerableObjRef;
					this.enumeration = IEnumerableMethods<KeyValuePair<K, V>>.GetEnumerator(iEnumerableObjRef);
				}

				// Token: 0x06001C69 RID: 7273 RVA: 0x0004BB1A File Offset: 0x00049D1A
				void IDisposable.Dispose()
				{
					this.enumeration.Dispose();
				}

				// Token: 0x06001C6A RID: 7274 RVA: 0x0004BB27 File Offset: 0x00049D27
				public bool MoveNext()
				{
					return this.enumeration.MoveNext();
				}

				// Token: 0x1700052E RID: 1326
				// (get) Token: 0x06001C6B RID: 7275 RVA: 0x0004BB34 File Offset: 0x00049D34
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				// Token: 0x1700052F RID: 1327
				// (get) Token: 0x06001C6C RID: 7276 RVA: 0x0004BB44 File Offset: 0x00049D44
				public K Current
				{
					get
					{
						KeyValuePair<K, V> keyValuePair = this.enumeration.Current;
						return keyValuePair.Key;
					}
				}

				// Token: 0x06001C6D RID: 7277 RVA: 0x0004BB64 File Offset: 0x00049D64
				public void Reset()
				{
					this.enumeration = IEnumerableMethods<KeyValuePair<K, V>>.GetEnumerator(this.iEnumerableObjRef);
				}

				// Token: 0x04000A50 RID: 2640
				private readonly IObjectReference iEnumerableObjRef;

				// Token: 0x04000A51 RID: 2641
				private IEnumerator<KeyValuePair<K, V>> enumeration;
			}
		}

		// Token: 0x02000397 RID: 919
		private sealed class ReadOnlyDictionaryValueCollection : IEnumerable<V>, IEnumerable
		{
			// Token: 0x060019C9 RID: 6601 RVA: 0x00047468 File Offset: 0x00045668
			public ReadOnlyDictionaryValueCollection(IObjectReference iDictionaryObjRef)
			{
				this.iDictionaryObjRef = iDictionaryObjRef;
			}

			// Token: 0x060019CA RID: 6602 RVA: 0x00047477 File Offset: 0x00045677
			private IObjectReference Make_IEnumerableObjRef()
			{
				Interlocked.CompareExchange<IObjectReference>(ref this.__iEnumerableObjRef, this.iDictionaryObjRef.As<IEnumerable<KeyValuePair<K, V>>.Vftbl>(), null);
				return this.__iEnumerableObjRef;
			}

			// Token: 0x170004DB RID: 1243
			// (get) Token: 0x060019CB RID: 6603 RVA: 0x00047499 File Offset: 0x00045699
			private IObjectReference iEnumerableObjRef
			{
				get
				{
					return this.__iEnumerableObjRef ?? this.Make_IEnumerableObjRef();
				}
			}

			// Token: 0x060019CC RID: 6604 RVA: 0x000474AD File Offset: 0x000456AD
			public IEnumerator<V> GetEnumerator()
			{
				return new IReadOnlyDictionaryMethods<K, V>.ReadOnlyDictionaryValueCollection.ReadOnlyDictionaryValueEnumerator(this.iEnumerableObjRef);
			}

			// Token: 0x060019CD RID: 6605 RVA: 0x000474BA File Offset: 0x000456BA
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x0400091B RID: 2331
			private readonly IObjectReference iDictionaryObjRef;

			// Token: 0x0400091C RID: 2332
			private volatile IObjectReference __iEnumerableObjRef;

			// Token: 0x02000432 RID: 1074
			private sealed class ReadOnlyDictionaryValueEnumerator : IEnumerator<V>, IEnumerator, IDisposable
			{
				// Token: 0x06001C6E RID: 7278 RVA: 0x0004BB77 File Offset: 0x00049D77
				public ReadOnlyDictionaryValueEnumerator(IObjectReference iEnumerableObjRef)
				{
					this.iEnumerableObjRef = iEnumerableObjRef;
					this.enumeration = IEnumerableMethods<KeyValuePair<K, V>>.GetEnumerator(iEnumerableObjRef);
				}

				// Token: 0x06001C6F RID: 7279 RVA: 0x0004BB92 File Offset: 0x00049D92
				void IDisposable.Dispose()
				{
					this.enumeration.Dispose();
				}

				// Token: 0x06001C70 RID: 7280 RVA: 0x0004BB9F File Offset: 0x00049D9F
				public bool MoveNext()
				{
					return this.enumeration.MoveNext();
				}

				// Token: 0x17000530 RID: 1328
				// (get) Token: 0x06001C71 RID: 7281 RVA: 0x0004BBAC File Offset: 0x00049DAC
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				// Token: 0x17000531 RID: 1329
				// (get) Token: 0x06001C72 RID: 7282 RVA: 0x0004BBBC File Offset: 0x00049DBC
				public V Current
				{
					get
					{
						KeyValuePair<K, V> keyValuePair = this.enumeration.Current;
						return keyValuePair.Value;
					}
				}

				// Token: 0x06001C73 RID: 7283 RVA: 0x0004BBDC File Offset: 0x00049DDC
				public void Reset()
				{
					this.enumeration = IEnumerableMethods<KeyValuePair<K, V>>.GetEnumerator(this.iEnumerableObjRef);
				}

				// Token: 0x04000A52 RID: 2642
				private readonly IObjectReference iEnumerableObjRef;

				// Token: 0x04000A53 RID: 2643
				private IEnumerator<KeyValuePair<K, V>> enumeration;
			}
		}
	}
}
