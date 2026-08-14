using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Collections;
using WinRT;
using WinRT.Interop;

namespace ABI.System.Collections.Generic
{
	// Token: 0x02000164 RID: 356
	[DynamicInterfaceCastableImplementation]
	[Guid("E480CE40-A338-4ADA-ADCF-272272E48CB9")]
	internal interface IReadOnlyDictionary<K, V> : IReadOnlyDictionary<K, V>, IEnumerable<KeyValuePair<!0, !1>>, IEnumerable, IReadOnlyCollection<KeyValuePair<K, V>>, IMapView<K, V>, IIterable<IKeyValuePair<!0, !1>>
	{
		// Token: 0x06000B86 RID: 2950 RVA: 0x000203EC File Offset: 0x0001E5EC
		public static IObjectReference CreateMarshaler(IReadOnlyDictionary<K, V> obj)
		{
			if (obj != null)
			{
				return ComWrappersSupport.CreateCCWForObject<IReadOnlyDictionary<K, V>.Vftbl>(obj, IReadOnlyDictionary<K, V>.PIID);
			}
			return null;
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x000203FE File Offset: 0x0001E5FE
		public static ObjectReferenceValue CreateMarshaler2(IReadOnlyDictionary<K, V> obj)
		{
			return ComWrappersSupport.CreateCCWForObjectForMarshaling(obj, IReadOnlyDictionary<K, V>.PIID);
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0002040B File Offset: 0x0001E60B
		public static IntPtr GetAbi(IObjectReference objRef)
		{
			if (objRef == null)
			{
				return IntPtr.Zero;
			}
			return objRef.ThisPtr;
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0002041C File Offset: 0x0001E61C
		public static IntPtr FromManaged(IReadOnlyDictionary<K, V> value)
		{
			if (value != null)
			{
				return IReadOnlyDictionary<K, V>.CreateMarshaler2(value).Detach();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x00020440 File Offset: 0x0001E640
		public static void DisposeMarshaler(IObjectReference objRef)
		{
			if (objRef != null)
			{
				objRef.Dispose();
			}
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0002044B File Offset: 0x0001E64B
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<IMapView<K, V>>.DisposeAbi(abi);
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x00020453 File Offset: 0x0001E653
		public static string GetGuidSignature()
		{
			return GuidGenerator.GetSignature(typeof(IReadOnlyDictionary<K, V>));
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x00020464 File Offset: 0x0001E664
		public static ObjectReference<IReadOnlyDictionary<K, V>.Vftbl> ObjRefFromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			IReadOnlyDictionary<K, V>.Vftbl vftbl = new IReadOnlyDictionary<K, V>.Vftbl(thisPtr);
			return ObjectReference<IReadOnlyDictionary<K, V>.Vftbl>.FromAbi(thisPtr, vftbl);
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x0002048F File Offset: 0x0001E68F
		IEnumerable<K> IReadOnlyDictionary<!0, !1>.Keys
		{
			get
			{
				return IReadOnlyDictionaryMethods<K, V>.get_Keys((ObjectReference<IReadOnlyDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IReadOnlyDictionary<!0, !1>).TypeHandle));
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000B8F RID: 2959 RVA: 0x000204B5 File Offset: 0x0001E6B5
		IEnumerable<V> IReadOnlyDictionary<!0, !1>.Values
		{
			get
			{
				return IReadOnlyDictionaryMethods<K, V>.get_Values((ObjectReference<IReadOnlyDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IReadOnlyDictionary<!0, !1>).TypeHandle));
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x000204DB File Offset: 0x0001E6DB
		int IReadOnlyCollection<KeyValuePair<!0, !1>>.Count
		{
			get
			{
				return IReadOnlyDictionaryMethods<K, V>.get_Count((ObjectReference<IReadOnlyDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IReadOnlyDictionary<!0, !1>).TypeHandle));
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000B91 RID: 2961 RVA: 0x00020501 File Offset: 0x0001E701
		V IReadOnlyDictionary<!0, !1>.Item
		{
			get
			{
				return IReadOnlyDictionaryMethods<K, V>.Indexer_Get((ObjectReference<IReadOnlyDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IReadOnlyDictionary<!0, !1>).TypeHandle), key);
			}
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00020528 File Offset: 0x0001E728
		bool ContainsKey(K key)
		{
			return IReadOnlyDictionaryMethods<K, V>.ContainsKey((ObjectReference<IReadOnlyDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IReadOnlyDictionary<!0, !1>).TypeHandle), key);
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0002054F File Offset: 0x0001E74F
		bool TryGetValue(K key, out V value)
		{
			return IReadOnlyDictionaryMethods<K, V>.TryGetValue((ObjectReference<IReadOnlyDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IReadOnlyDictionary<!0, !1>).TypeHandle), key, ref value);
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x00020578 File Offset: 0x0001E778
		IEnumerator<KeyValuePair<K, V>> GetEnumerator()
		{
			((IWinRTObject)this).IsInterfaceImplemented(typeof(IEnumerable<KeyValuePair<K, V>>).TypeHandle, true);
			return IEnumerableMethods<KeyValuePair<K, V>>.GetEnumerator((ObjectReference<IReadOnlyDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IEnumerable<KeyValuePair<K, V>>).TypeHandle));
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x000205C5 File Offset: 0x0001E7C5
		IEnumerator GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x000205CD File Offset: 0x0001E7CD
		static IReadOnlyDictionary()
		{
			IReadOnlyDictionary<K, V>.PIID = IReadOnlyDictionary<K, V>.Vftbl.PIID;
		}

		// Token: 0x040001F3 RID: 499
		public static Guid PIID;

		// Token: 0x02000398 RID: 920
		private sealed class ReadOnlyDictionaryKeyCollection : IEnumerable<!0>, IEnumerable
		{
			// Token: 0x060019CE RID: 6606 RVA: 0x000474C2 File Offset: 0x000456C2
			public ReadOnlyDictionaryKeyCollection(IReadOnlyDictionary<K, V> dictionary)
			{
				if (dictionary == null)
				{
					throw new ArgumentNullException("dictionary");
				}
				this.dictionary = dictionary;
			}

			// Token: 0x060019CF RID: 6607 RVA: 0x000474DF File Offset: 0x000456DF
			public IEnumerator<K> GetEnumerator()
			{
				return new IReadOnlyDictionary<K, V>.ReadOnlyDictionaryKeyCollection.ReadOnlyDictionaryKeyEnumerator(this.dictionary);
			}

			// Token: 0x060019D0 RID: 6608 RVA: 0x000474EC File Offset: 0x000456EC
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x0400091D RID: 2333
			private readonly IReadOnlyDictionary<K, V> dictionary;

			// Token: 0x02000433 RID: 1075
			private sealed class ReadOnlyDictionaryKeyEnumerator : IEnumerator<K>, IEnumerator, IDisposable
			{
				// Token: 0x06001C74 RID: 7284 RVA: 0x0004BBEF File Offset: 0x00049DEF
				public ReadOnlyDictionaryKeyEnumerator(IReadOnlyDictionary<K, V> dictionary)
				{
					if (dictionary == null)
					{
						throw new ArgumentNullException("dictionary");
					}
					this.dictionary = dictionary;
					this.enumeration = dictionary.GetEnumerator();
				}

				// Token: 0x06001C75 RID: 7285 RVA: 0x0004BC18 File Offset: 0x00049E18
				void IDisposable.Dispose()
				{
					this.enumeration.Dispose();
				}

				// Token: 0x06001C76 RID: 7286 RVA: 0x0004BC25 File Offset: 0x00049E25
				public bool MoveNext()
				{
					return this.enumeration.MoveNext();
				}

				// Token: 0x17000532 RID: 1330
				// (get) Token: 0x06001C77 RID: 7287 RVA: 0x0004BC32 File Offset: 0x00049E32
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				// Token: 0x17000533 RID: 1331
				// (get) Token: 0x06001C78 RID: 7288 RVA: 0x0004BC40 File Offset: 0x00049E40
				public K Current
				{
					get
					{
						KeyValuePair<K, V> keyValuePair = this.enumeration.Current;
						return keyValuePair.Key;
					}
				}

				// Token: 0x06001C79 RID: 7289 RVA: 0x0004BC60 File Offset: 0x00049E60
				public void Reset()
				{
					this.enumeration = this.dictionary.GetEnumerator();
				}

				// Token: 0x04000A54 RID: 2644
				private readonly IReadOnlyDictionary<K, V> dictionary;

				// Token: 0x04000A55 RID: 2645
				private IEnumerator<KeyValuePair<K, V>> enumeration;
			}
		}

		// Token: 0x02000399 RID: 921
		private sealed class ReadOnlyDictionaryValueCollection : IEnumerable<V>, IEnumerable
		{
			// Token: 0x060019D1 RID: 6609 RVA: 0x000474F4 File Offset: 0x000456F4
			public ReadOnlyDictionaryValueCollection(IReadOnlyDictionary<K, V> dictionary)
			{
				if (dictionary == null)
				{
					throw new ArgumentNullException("dictionary");
				}
				this.dictionary = dictionary;
			}

			// Token: 0x060019D2 RID: 6610 RVA: 0x00047511 File Offset: 0x00045711
			public IEnumerator<V> GetEnumerator()
			{
				return new IReadOnlyDictionary<K, V>.ReadOnlyDictionaryValueCollection.ReadOnlyDictionaryValueEnumerator(this.dictionary);
			}

			// Token: 0x060019D3 RID: 6611 RVA: 0x0004751E File Offset: 0x0004571E
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x0400091E RID: 2334
			private readonly IReadOnlyDictionary<K, V> dictionary;

			// Token: 0x02000434 RID: 1076
			private sealed class ReadOnlyDictionaryValueEnumerator : IEnumerator<V>, IEnumerator, IDisposable
			{
				// Token: 0x06001C7A RID: 7290 RVA: 0x0004BC73 File Offset: 0x00049E73
				public ReadOnlyDictionaryValueEnumerator(IReadOnlyDictionary<K, V> dictionary)
				{
					if (dictionary == null)
					{
						throw new ArgumentNullException("dictionary");
					}
					this.dictionary = dictionary;
					this.enumeration = dictionary.GetEnumerator();
				}

				// Token: 0x06001C7B RID: 7291 RVA: 0x0004BC9C File Offset: 0x00049E9C
				void IDisposable.Dispose()
				{
					this.enumeration.Dispose();
				}

				// Token: 0x06001C7C RID: 7292 RVA: 0x0004BCA9 File Offset: 0x00049EA9
				public bool MoveNext()
				{
					return this.enumeration.MoveNext();
				}

				// Token: 0x17000534 RID: 1332
				// (get) Token: 0x06001C7D RID: 7293 RVA: 0x0004BCB6 File Offset: 0x00049EB6
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				// Token: 0x17000535 RID: 1333
				// (get) Token: 0x06001C7E RID: 7294 RVA: 0x0004BCC4 File Offset: 0x00049EC4
				public V Current
				{
					get
					{
						KeyValuePair<K, V> keyValuePair = this.enumeration.Current;
						return keyValuePair.Value;
					}
				}

				// Token: 0x06001C7F RID: 7295 RVA: 0x0004BCE4 File Offset: 0x00049EE4
				public void Reset()
				{
					this.enumeration = this.dictionary.GetEnumerator();
				}

				// Token: 0x04000A56 RID: 2646
				private readonly IReadOnlyDictionary<K, V> dictionary;

				// Token: 0x04000A57 RID: 2647
				private IEnumerator<KeyValuePair<K, V>> enumeration;
			}
		}

		// Token: 0x0200039A RID: 922
		public sealed class ToAbiHelper : IMapView<K, V>, IIterable<IKeyValuePair<!0, !1>>
		{
			// Token: 0x060019D4 RID: 6612 RVA: 0x00047526 File Offset: 0x00045726
			internal ToAbiHelper(IReadOnlyDictionary<K, V> dictionary)
			{
				this._dictionary = dictionary;
			}

			// Token: 0x170004DC RID: 1244
			// (get) Token: 0x060019D5 RID: 6613 RVA: 0x00047535 File Offset: 0x00045735
			uint IMapView<!0, !1>.Size
			{
				get
				{
					return (uint)this._dictionary.Count;
				}
			}

			// Token: 0x060019D6 RID: 6614 RVA: 0x00047542 File Offset: 0x00045742
			IEnumerator<IKeyValuePair<K, V>> IIterable<IKeyValuePair<!0, !1>>.First()
			{
				return new KeyValuePair<K, V>.Enumerator(this._dictionary.GetEnumerator());
			}

			// Token: 0x060019D7 RID: 6615 RVA: 0x0004755C File Offset: 0x0004575C
			public V Lookup(K key)
			{
				V result;
				if (!this._dictionary.TryGetValue(key, ref result))
				{
					KeyNotFoundException ex = new KeyNotFoundException(string.Format(WinRTRuntimeErrorStrings.Arg_KeyNotFoundWithKey, key.ToString()));
					ex.SetHResult(-2147483637);
					throw ex;
				}
				return result;
			}

			// Token: 0x060019D8 RID: 6616 RVA: 0x000475A2 File Offset: 0x000457A2
			public uint Size()
			{
				return (uint)this._dictionary.Count;
			}

			// Token: 0x060019D9 RID: 6617 RVA: 0x000475AF File Offset: 0x000457AF
			public bool HasKey(K key)
			{
				return this._dictionary.ContainsKey(key);
			}

			// Token: 0x060019DA RID: 6618 RVA: 0x000475C0 File Offset: 0x000457C0
			void IMapView<!0, !1>.Split(out IMapView<K, V> first, out IMapView<K, V> second)
			{
				if (this._dictionary.Count < 2)
				{
					first = null;
					second = null;
					return;
				}
				IReadOnlyDictionary<K, V>.ToAbiHelper.ConstantSplittableMap constantSplittableMap = this._dictionary as IReadOnlyDictionary<K, V>.ToAbiHelper.ConstantSplittableMap;
				if (constantSplittableMap == null)
				{
					constantSplittableMap = new IReadOnlyDictionary<K, V>.ToAbiHelper.ConstantSplittableMap(this._dictionary);
				}
				constantSplittableMap.Split(ref first, ref second);
			}

			// Token: 0x0400091F RID: 2335
			private readonly IReadOnlyDictionary<K, V> _dictionary;

			// Token: 0x02000435 RID: 1077
			private sealed class ConstantSplittableMap : IMapView<!0, !1>, IIterable<IKeyValuePair<!0, !1>>, IReadOnlyDictionary<!0, !1>, IEnumerable<KeyValuePair<!0, !1>>, IEnumerable, IReadOnlyCollection<KeyValuePair<!0, !1>>
			{
				// Token: 0x06001C80 RID: 7296 RVA: 0x0004BCF8 File Offset: 0x00049EF8
				internal ConstantSplittableMap(IReadOnlyDictionary<K, V> data)
				{
					if (data == null)
					{
						throw new ArgumentNullException("data");
					}
					this.firstItemIndex = 0;
					this.lastItemIndex = data.Count - 1;
					this.items = this.CreateKeyValueArray(data.Count, data.GetEnumerator());
				}

				// Token: 0x06001C81 RID: 7297 RVA: 0x0004BD46 File Offset: 0x00049F46
				private ConstantSplittableMap(KeyValuePair<K, V>[] items, int firstItemIndex, int lastItemIndex)
				{
					this.items = items;
					this.firstItemIndex = firstItemIndex;
					this.lastItemIndex = lastItemIndex;
				}

				// Token: 0x06001C82 RID: 7298 RVA: 0x0004BD64 File Offset: 0x00049F64
				private KeyValuePair<K, V>[] CreateKeyValueArray(int count, IEnumerator<KeyValuePair<K, V>> data)
				{
					KeyValuePair<K, V>[] array = new KeyValuePair<K, V>[count];
					int num = 0;
					while (data.MoveNext())
					{
						KeyValuePair<K, V> keyValuePair = data.Current;
						array[num++] = keyValuePair;
					}
					Array.Sort<KeyValuePair<K, V>>(array, IReadOnlyDictionary<K, V>.ToAbiHelper.ConstantSplittableMap.keyValuePairComparator);
					return array;
				}

				// Token: 0x17000536 RID: 1334
				// (get) Token: 0x06001C83 RID: 7299 RVA: 0x0004BDA1 File Offset: 0x00049FA1
				public uint Size
				{
					get
					{
						return (uint)(this.lastItemIndex - this.firstItemIndex + 1);
					}
				}

				// Token: 0x17000537 RID: 1335
				// (get) Token: 0x06001C84 RID: 7300 RVA: 0x0004BDB2 File Offset: 0x00049FB2
				public IEnumerable<K> Keys
				{
					get
					{
						return new IReadOnlyDictionary<K, V>.ReadOnlyDictionaryKeyCollection(this);
					}
				}

				// Token: 0x17000538 RID: 1336
				// (get) Token: 0x06001C85 RID: 7301 RVA: 0x0004BDBA File Offset: 0x00049FBA
				public IEnumerable<V> Values
				{
					get
					{
						return new IReadOnlyDictionary<K, V>.ReadOnlyDictionaryValueCollection(this);
					}
				}

				// Token: 0x17000539 RID: 1337
				// (get) Token: 0x06001C86 RID: 7302 RVA: 0x0004BDC2 File Offset: 0x00049FC2
				public int Count
				{
					get
					{
						return this.lastItemIndex - this.firstItemIndex + 1;
					}
				}

				// Token: 0x1700053A RID: 1338
				public V this[K key]
				{
					get
					{
						return this.Lookup(key);
					}
				}

				// Token: 0x06001C88 RID: 7304 RVA: 0x0004BDDC File Offset: 0x00049FDC
				public V Lookup(K key)
				{
					V result;
					if (!this.TryGetValue(key, ref result))
					{
						KeyNotFoundException ex = new KeyNotFoundException(string.Format(WinRTRuntimeErrorStrings.Arg_KeyNotFoundWithKey, key.ToString()));
						ex.SetHResult(-2147483637);
						throw ex;
					}
					return result;
				}

				// Token: 0x06001C89 RID: 7305 RVA: 0x0004BE20 File Offset: 0x0004A020
				public bool HasKey(K key)
				{
					V v;
					return this.TryGetValue(key, ref v);
				}

				// Token: 0x06001C8A RID: 7306 RVA: 0x0004BE36 File Offset: 0x0004A036
				public IEnumerator<KeyValuePair<K, V>> First()
				{
					return this.GetEnumerator();
				}

				// Token: 0x06001C8B RID: 7307 RVA: 0x0004BE40 File Offset: 0x0004A040
				public void Split(out IMapView<K, V> firstPartition, out IMapView<K, V> secondPartition)
				{
					if (this.Count < 2)
					{
						firstPartition = null;
						secondPartition = null;
						return;
					}
					int num = (int)(((long)this.firstItemIndex + (long)this.lastItemIndex) / 2L);
					firstPartition = new IReadOnlyDictionary<K, V>.ToAbiHelper.ConstantSplittableMap(this.items, this.firstItemIndex, num);
					secondPartition = new IReadOnlyDictionary<K, V>.ToAbiHelper.ConstantSplittableMap(this.items, num + 1, this.lastItemIndex);
				}

				// Token: 0x06001C8C RID: 7308 RVA: 0x0004BE9C File Offset: 0x0004A09C
				public bool TryGetValue(K key, out V value)
				{
					KeyValuePair<K, V> keyValuePair = new KeyValuePair<K, V>(key, default(V));
					int num = Array.BinarySearch<KeyValuePair<K, V>>(this.items, this.firstItemIndex, this.Count, keyValuePair, IReadOnlyDictionary<K, V>.ToAbiHelper.ConstantSplittableMap.keyValuePairComparator);
					if (num < 0)
					{
						value = default(V);
						return false;
					}
					value = this.items[num].Value;
					return true;
				}

				// Token: 0x06001C8D RID: 7309 RVA: 0x0004BEFD File Offset: 0x0004A0FD
				public bool ContainsKey(K key)
				{
					return this.HasKey(key);
				}

				// Token: 0x06001C8E RID: 7310 RVA: 0x0004BF08 File Offset: 0x0004A108
				IEnumerator<IKeyValuePair<K, V>> IIterable<IKeyValuePair<!0, !1>>.First()
				{
					IKeyValuePair<K, V>[] array = new IKeyValuePair<K, V>[this.items.Length];
					for (int i = 0; i < this.items.Length; i++)
					{
						array[i] = new KeyValuePair<K, V>.ToIKeyValuePair(ref this.items[i]);
					}
					return new IReadOnlyDictionary<K, V>.ToAbiHelper.Enumerator<IKeyValuePair<K, V>>(array, this.firstItemIndex, this.lastItemIndex);
				}

				// Token: 0x06001C8F RID: 7311 RVA: 0x0004BF61 File Offset: 0x0004A161
				public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
				{
					return new IReadOnlyDictionary<K, V>.ToAbiHelper.Enumerator<KeyValuePair<K, V>>(this.items, this.firstItemIndex, this.lastItemIndex);
				}

				// Token: 0x06001C90 RID: 7312 RVA: 0x0004BF7F File Offset: 0x0004A17F
				IEnumerator IEnumerable.GetEnumerator()
				{
					return new IReadOnlyDictionary<K, V>.ToAbiHelper.Enumerator<KeyValuePair<K, V>>(this.items, this.firstItemIndex, this.lastItemIndex);
				}

				// Token: 0x06001C91 RID: 7313 RVA: 0x0004BF9D File Offset: 0x0004A19D
				// Note: this type is marked as 'beforefieldinit'.
				static ConstantSplittableMap()
				{
					IReadOnlyDictionary<K, V>.ToAbiHelper.ConstantSplittableMap.keyValuePairComparator = new IReadOnlyDictionary<K, V>.ToAbiHelper.ConstantSplittableMap.KeyValuePairComparator();
				}

				// Token: 0x04000A58 RID: 2648
				private static readonly IReadOnlyDictionary<K, V>.ToAbiHelper.ConstantSplittableMap.KeyValuePairComparator keyValuePairComparator;

				// Token: 0x04000A59 RID: 2649
				private readonly KeyValuePair<K, V>[] items;

				// Token: 0x04000A5A RID: 2650
				private readonly int firstItemIndex;

				// Token: 0x04000A5B RID: 2651
				private readonly int lastItemIndex;

				// Token: 0x0200043B RID: 1083
				private sealed class KeyValuePairComparator : IComparer<KeyValuePair<K, V>>
				{
					// Token: 0x06001CA2 RID: 7330 RVA: 0x0004C0C2 File Offset: 0x0004A2C2
					public int Compare(KeyValuePair<K, V> x, KeyValuePair<K, V> y)
					{
						return IReadOnlyDictionary<K, V>.ToAbiHelper.ConstantSplittableMap.KeyValuePairComparator.keyComparator.Compare(x.Key, y.Key);
					}

					// Token: 0x06001CA4 RID: 7332 RVA: 0x0004C0E4 File Offset: 0x0004A2E4
					// Note: this type is marked as 'beforefieldinit'.
					static KeyValuePairComparator()
					{
						IReadOnlyDictionary<K, V>.ToAbiHelper.ConstantSplittableMap.KeyValuePairComparator.keyComparator = Comparer<K>.Default;
					}

					// Token: 0x04000A66 RID: 2662
					private static readonly IComparer<K> keyComparator;
				}
			}

			// Token: 0x02000436 RID: 1078
			internal struct Enumerator<T> : IEnumerator<T>, IEnumerator, IDisposable
			{
				// Token: 0x06001C92 RID: 7314 RVA: 0x0004BFA9 File Offset: 0x0004A1A9
				internal Enumerator(T[] items, int first, int end)
				{
					this._array = items;
					this._start = first;
					this._end = end;
					this._current = this._start - 1;
				}

				// Token: 0x06001C93 RID: 7315 RVA: 0x0004BFCE File Offset: 0x0004A1CE
				public bool MoveNext()
				{
					if (this._current < this._end)
					{
						this._current++;
						return true;
					}
					return false;
				}

				// Token: 0x1700053B RID: 1339
				// (get) Token: 0x06001C94 RID: 7316 RVA: 0x0004BFF0 File Offset: 0x0004A1F0
				public T Current
				{
					get
					{
						if (this._current < this._start)
						{
							throw new InvalidOperationException(WinRTRuntimeErrorStrings.InvalidOperation_EnumNotStarted);
						}
						if (this._current > this._end)
						{
							throw new InvalidOperationException(WinRTRuntimeErrorStrings.InvalidOperation_EnumEnded);
						}
						return this._array[this._current];
					}
				}

				// Token: 0x1700053C RID: 1340
				// (get) Token: 0x06001C95 RID: 7317 RVA: 0x0004C040 File Offset: 0x0004A240
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				// Token: 0x06001C96 RID: 7318 RVA: 0x0004C04D File Offset: 0x0004A24D
				void IEnumerator.Reset()
				{
					this._current = this._start - 1;
				}

				// Token: 0x06001C97 RID: 7319 RVA: 0x0004C05D File Offset: 0x0004A25D
				public void Dispose()
				{
				}

				// Token: 0x04000A5C RID: 2652
				private readonly T[] _array;

				// Token: 0x04000A5D RID: 2653
				private readonly int _start;

				// Token: 0x04000A5E RID: 2654
				private readonly int _end;

				// Token: 0x04000A5F RID: 2655
				private int _current;
			}
		}

		// Token: 0x0200039B RID: 923
		[Guid("E480CE40-A338-4ADA-ADCF-272272E48CB9")]
		public struct Vftbl
		{
			// Token: 0x170004DD RID: 1245
			// (get) Token: 0x060019DB RID: 6619 RVA: 0x00047605 File Offset: 0x00045805
			// (set) Token: 0x060019DC RID: 6620 RVA: 0x0004760D File Offset: 0x0004580D
			internal method GetSize_1
			{
				get
				{
					return this._get_Size_1;
				}
				set
				{
					this._get_Size_1 = value;
				}
			}

			// Token: 0x170004DE RID: 1246
			// (get) Token: 0x060019DD RID: 6621 RVA: 0x00047616 File Offset: 0x00045816
			// (set) Token: 0x060019DE RID: 6622 RVA: 0x0004761E File Offset: 0x0004581E
			internal method Split_3
			{
				get
				{
					return this._split_3;
				}
				set
				{
					this._split_3 = value;
				}
			}

			// Token: 0x060019DF RID: 6623 RVA: 0x00047628 File Offset: 0x00045828
			internal unsafe Vftbl(IntPtr thisPtr)
			{
				this = default(IReadOnlyDictionary<K, V>.Vftbl);
				VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
				IntPtr* ptr = (IntPtr*)((void*)vftblPtr.Vftbl);
				this.IInspectableVftbl = Marshal.PtrToStructure<IInspectable.Vftbl>(vftblPtr.Vftbl);
				this.Lookup_0 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IReadOnlyDictionary<K, V>.Vftbl.Lookup_0_Type);
				this.GetSize_1 = (void*)ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.HasKey_2 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IReadOnlyDictionary<K, V>.Vftbl.HasKey_2_Type);
				this.Split_3 = (void*)ptr[(IntPtr)9 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
			}

			// Token: 0x060019E0 RID: 6624 RVA: 0x000476C8 File Offset: 0x000458C8
			unsafe static Vftbl()
			{
				IReadOnlyDictionary<K, V>.Vftbl.PIID = GuidGenerator.CreateIID(typeof(IReadOnlyDictionary<K, V>));
				IReadOnlyDictionary<K, V>.Vftbl.Lookup_0_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					Marshaler<K>.AbiType,
					Marshaler<V>.AbiType.MakeByRefType(),
					typeof(int)
				});
				IReadOnlyDictionary<K, V>.Vftbl.HasKey_2_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					Marshaler<K>.AbiType,
					typeof(byte).MakeByRefType(),
					typeof(int)
				});
				IReadOnlyDictionary<K, V>.Vftbl.DelegateCache = new Delegate[2];
				IReadOnlyDictionary<K, V>.Vftbl._adapterTable = new ConditionalWeakTable<IReadOnlyDictionary<K, V>, IReadOnlyDictionary<K, V>.ToAbiHelper>();
				IReadOnlyDictionary<K, V>.Vftbl.AbiToProjectionVftable = new IReadOnlyDictionary<K, V>.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					Lookup_0 = Delegate.CreateDelegate(IReadOnlyDictionary<K, V>.Vftbl.Lookup_0_Type, typeof(IReadOnlyDictionary<K, V>.Vftbl).GetMethod("Do_Abi_Lookup_0", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<K>.AbiType,
						Marshaler<V>.AbiType
					})),
					_get_Size_1 = (void*)Marshal.GetFunctionPointerForDelegate(IReadOnlyDictionary<K, V>.Vftbl.DelegateCache[0] = new _get_PropertyAsUInt32(IReadOnlyDictionary<K, V>.Vftbl.Do_Abi_get_Size_1)),
					HasKey_2 = Delegate.CreateDelegate(IReadOnlyDictionary<K, V>.Vftbl.HasKey_2_Type, typeof(IReadOnlyDictionary<K, V>.Vftbl).GetMethod("Do_Abi_HasKey_2", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<K>.AbiType
					})),
					_split_3 = (void*)Marshal.GetFunctionPointerForDelegate(IReadOnlyDictionary<K, V>.Vftbl.DelegateCache[1] = new IReadOnlyDictionary_Delegates.Split_3(IReadOnlyDictionary<K, V>.Vftbl.Do_Abi_Split_3))
				};
				IntPtr* ptr = (IntPtr*)((void*)Marshal.AllocCoTaskMem(Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 4));
				Marshal.StructureToPtr<IInspectable.Vftbl>(IReadOnlyDictionary<K, V>.Vftbl.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
				ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IReadOnlyDictionary<K, V>.Vftbl.AbiToProjectionVftable.Lookup_0);
				ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IReadOnlyDictionary<K, V>.Vftbl.AbiToProjectionVftable.GetSize_1;
				ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IReadOnlyDictionary<K, V>.Vftbl.AbiToProjectionVftable.HasKey_2);
				ptr[(IntPtr)9 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IReadOnlyDictionary<K, V>.Vftbl.AbiToProjectionVftable.Split_3;
				IReadOnlyDictionary<K, V>.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x060019E1 RID: 6625 RVA: 0x0004790C File Offset: 0x00045B0C
			private static IMapView<K, V> FindAdapter(IntPtr thisPtr)
			{
				IReadOnlyDictionary<K, V> readOnlyDictionary = ComWrappersSupport.FindObject<IReadOnlyDictionary<K, V>>(thisPtr);
				ConditionalWeakTable<IReadOnlyDictionary<K, V>, IReadOnlyDictionary<K, V>.ToAbiHelper> adapterTable = IReadOnlyDictionary<K, V>.Vftbl._adapterTable;
				IReadOnlyDictionary<K, V> readOnlyDictionary2 = readOnlyDictionary;
				ConditionalWeakTable<IReadOnlyDictionary<K, V>, IReadOnlyDictionary<K, V>.ToAbiHelper>.CreateValueCallback createValueCallback;
				if ((createValueCallback = IReadOnlyDictionary<K, V>.Vftbl.<>c.<>9__20_0) == null)
				{
					createValueCallback = (IReadOnlyDictionary<K, V>.Vftbl.<>c.<>9__20_0 = new ConditionalWeakTable<IReadOnlyDictionary<K, V>, IReadOnlyDictionary<K, V>.ToAbiHelper>.CreateValueCallback(IReadOnlyDictionary<K, V>.Vftbl.<>c.<>9.<FindAdapter>b__20_0));
				}
				return adapterTable.GetValue(readOnlyDictionary2, createValueCallback);
			}

			// Token: 0x060019E2 RID: 6626 RVA: 0x0004794C File Offset: 0x00045B4C
			private unsafe static int Do_Abi_Lookup_0<KAbi, VAbi>(void* thisPtr, KAbi key, out VAbi __return_value__)
			{
				V v = default(V);
				__return_value__ = default(VAbi);
				try
				{
					v = IReadOnlyDictionary<K, V>.Vftbl.FindAdapter(new IntPtr(thisPtr)).Lookup(Marshaler<K>.FromAbi.Invoke(key));
					__return_value__ = (VAbi)((object)Marshaler<V>.FromManaged.Invoke(v));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060019E3 RID: 6627 RVA: 0x000479C4 File Offset: 0x00045BC4
			private unsafe static int Do_Abi_HasKey_2<KAbi>(void* thisPtr, KAbi key, out byte __return_value__)
			{
				__return_value__ = 0;
				try
				{
					__return_value__ = (IReadOnlyDictionary<K, V>.Vftbl.FindAdapter(new IntPtr(thisPtr)).HasKey(Marshaler<K>.FromAbi.Invoke(key)) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060019E4 RID: 6628 RVA: 0x00047A24 File Offset: 0x00045C24
			private static int Do_Abi_Split_3(IntPtr thisPtr, out IntPtr first, out IntPtr second)
			{
				first = default(IntPtr);
				second = default(IntPtr);
				IMapView<K, V> mapView = null;
				IMapView<K, V> mapView2 = null;
				try
				{
					IReadOnlyDictionary<K, V>.Vftbl.FindAdapter(thisPtr).Split(ref mapView, ref mapView2);
					first = MarshalInterface<IMapView<K, V>>.FromManaged(mapView);
					second = MarshalInterface<IMapView<K, V>>.FromManaged(mapView2);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060019E5 RID: 6629 RVA: 0x00047A88 File Offset: 0x00045C88
			private static int Do_Abi_get_Size_1(IntPtr thisPtr, out uint __return_value__)
			{
				__return_value__ = 0U;
				try
				{
					uint size = IReadOnlyDictionary<K, V>.Vftbl.FindAdapter(thisPtr).Size;
					__return_value__ = size;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000920 RID: 2336
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000921 RID: 2337
			public Delegate Lookup_0;

			// Token: 0x04000922 RID: 2338
			private unsafe void* _get_Size_1;

			// Token: 0x04000923 RID: 2339
			public Delegate HasKey_2;

			// Token: 0x04000924 RID: 2340
			private unsafe void* _split_3;

			// Token: 0x04000925 RID: 2341
			public static Guid PIID;

			// Token: 0x04000926 RID: 2342
			private static readonly Type Lookup_0_Type;

			// Token: 0x04000927 RID: 2343
			private static readonly Type HasKey_2_Type;

			// Token: 0x04000928 RID: 2344
			private static readonly IReadOnlyDictionary<K, V>.Vftbl AbiToProjectionVftable;

			// Token: 0x04000929 RID: 2345
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x0400092A RID: 2346
			private static readonly Delegate[] DelegateCache;

			// Token: 0x0400092B RID: 2347
			private static ConditionalWeakTable<IReadOnlyDictionary<K, V>, IReadOnlyDictionary<K, V>.ToAbiHelper> _adapterTable;
		}
	}
}
