using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Windows.Foundation.Collections;
using WinRT;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x020000F9 RID: 249
	[DynamicInterfaceCastableImplementation]
	[Guid("65DF2BF5-BF39-41B5-AEBC-5A9D865E472B")]
	internal interface IObservableMap<K, V> : Windows.Foundation.Collections.IObservableMap<K, V>, IDictionary<K, V>, ICollection<KeyValuePair<K, V>>, IEnumerable<KeyValuePair<K, V>>, IEnumerable
	{
		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060008CC RID: 2252 RVA: 0x000198E2 File Offset: 0x00017AE2
		// (remove) Token: 0x060008CD RID: 2253 RVA: 0x00019919 File Offset: 0x00017B19
		event Windows.Foundation.Collections.MapChangedEventHandler<K, V> Windows.Foundation.Collections.IObservableMap<!0, !1>.MapChanged
		{
			add
			{
				IObservableMapMethods<K, V>.Get_MapChanged((ObjectReference<IObservableMap<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.Collections.IObservableMap<K, V>).TypeHandle), (IWinRTObject)this).Item1.Invoke(value);
			}
			remove
			{
				IObservableMapMethods<K, V>.Get_MapChanged((ObjectReference<IObservableMap<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.Collections.IObservableMap<!0, !1>).TypeHandle), (IWinRTObject)this).Item2.Invoke(value);
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x00019950 File Offset: 0x00017B50
		ICollection<K> IDictionary<!0, !1>.Keys
		{
			get
			{
				return ((IDictionary<!0, !1>)((IWinRTObject)this)).Keys;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x00019962 File Offset: 0x00017B62
		ICollection<V> IDictionary<!0, !1>.Values
		{
			get
			{
				return ((IDictionary<!0, !1>)((IWinRTObject)this)).Values;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x00019974 File Offset: 0x00017B74
		int ICollection<KeyValuePair<!0, !1>>.Count
		{
			get
			{
				return ((IDictionary<!0, !1>)((IWinRTObject)this)).Count;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x00019986 File Offset: 0x00017B86
		bool ICollection<KeyValuePair<!0, !1>>.IsReadOnly
		{
			get
			{
				return ((IDictionary<!0, !1>)((IWinRTObject)this)).IsReadOnly;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x00019998 File Offset: 0x00017B98
		// (set) Token: 0x060008D3 RID: 2259 RVA: 0x000199AB File Offset: 0x00017BAB
		V IDictionary<!0, !1>.Item
		{
			get
			{
				return ((IDictionary<!0, !1>)((IWinRTObject)this))[key];
			}
			set
			{
				((IDictionary<!0, !1>)((IWinRTObject)this))[key] = value;
			}
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x000199BF File Offset: 0x00017BBF
		void Add(K key, V value)
		{
			((IDictionary<!0, !1>)((IWinRTObject)this)).Add(key, value);
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x000199D3 File Offset: 0x00017BD3
		bool ContainsKey(K key)
		{
			return ((IDictionary<!0, !1>)((IWinRTObject)this)).ContainsKey(key);
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x000199E6 File Offset: 0x00017BE6
		bool Remove(K key)
		{
			return ((IDictionary<!0, !1>)((IWinRTObject)this)).Remove(key);
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x000199F9 File Offset: 0x00017BF9
		bool TryGetValue(K key, out V value)
		{
			return ((IDictionary<!0, !1>)((IWinRTObject)this)).TryGetValue(key, ref value);
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00019A0D File Offset: 0x00017C0D
		void Add(KeyValuePair<K, V> item)
		{
			((IDictionary<!0, !1>)((IWinRTObject)this)).Add(item);
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x00019A20 File Offset: 0x00017C20
		void Clear()
		{
			((IDictionary<!0, !1>)((IWinRTObject)this)).Clear();
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x00019A32 File Offset: 0x00017C32
		bool Contains(KeyValuePair<K, V> item)
		{
			return ((IDictionary<!0, !1>)((IWinRTObject)this)).Contains(item);
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x00019A45 File Offset: 0x00017C45
		void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
		{
			((IDictionary<!0, !1>)((IWinRTObject)this)).CopyTo(array, arrayIndex);
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00019A59 File Offset: 0x00017C59
		bool Remove(KeyValuePair<K, V> item)
		{
			return ((IDictionary<!0, !1>)((IWinRTObject)this)).Remove(item);
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x00019A6C File Offset: 0x00017C6C
		IEnumerator<KeyValuePair<K, V>> GetEnumerator()
		{
			return ((IDictionary<!0, !1>)((IWinRTObject)this)).GetEnumerator();
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00019A7E File Offset: 0x00017C7E
		IEnumerator GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00019A86 File Offset: 0x00017C86
		static IObservableMap()
		{
			IObservableMap<K, V>.PIID = IObservableMap<K, V>.Vftbl.PIID;
		}

		// Token: 0x04000197 RID: 407
		public static Guid PIID;

		// Token: 0x02000315 RID: 789
		[Guid("65DF2BF5-BF39-41B5-AEBC-5A9D865E472B")]
		public struct Vftbl
		{
			// Token: 0x17000464 RID: 1124
			// (get) Token: 0x06001733 RID: 5939 RVA: 0x0004158B File Offset: 0x0003F78B
			// (set) Token: 0x06001734 RID: 5940 RVA: 0x00041593 File Offset: 0x0003F793
			public method add_MapChanged_0
			{
				get
				{
					return this._add_MapChanged_0;
				}
				set
				{
					this._add_MapChanged_0 = value;
				}
			}

			// Token: 0x17000465 RID: 1125
			// (get) Token: 0x06001735 RID: 5941 RVA: 0x0004159C File Offset: 0x0003F79C
			// (set) Token: 0x06001736 RID: 5942 RVA: 0x000415A4 File Offset: 0x0003F7A4
			public method remove_MapChanged_1
			{
				get
				{
					return this._remove_MapChanged_1;
				}
				set
				{
					this._remove_MapChanged_1 = value;
				}
			}

			// Token: 0x06001737 RID: 5943 RVA: 0x000415B0 File Offset: 0x0003F7B0
			internal unsafe Vftbl(IntPtr thisPtr)
			{
				this = default(IObservableMap<K, V>.Vftbl);
				VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
				IntPtr* ptr = (IntPtr*)((void*)vftblPtr.Vftbl);
				this.IInspectableVftbl = Marshal.PtrToStructure<IInspectable.Vftbl>(vftblPtr.Vftbl);
				this.add_MapChanged_0 = (void*)ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.remove_MapChanged_1 = (void*)ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
			}

			// Token: 0x06001738 RID: 5944 RVA: 0x00041618 File Offset: 0x0003F818
			unsafe static Vftbl()
			{
				IObservableMap<K, V>.Vftbl.PIID = GuidGenerator.CreateIID(typeof(IObservableMap<K, V>));
				IObservableMap<K, V>.Vftbl.DelegateCache = new Delegate[2];
				IObservableMap<K, V>.Vftbl.AbiToProjectionVftable = new IObservableMap<K, V>.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					_add_MapChanged_0 = (void*)Marshal.GetFunctionPointerForDelegate(IObservableMap<K, V>.Vftbl.DelegateCache[0] = new IObservableMap_Delegates.add_MapChanged_0(IObservableMap<K, V>.Vftbl.Do_Abi_add_MapChanged_0)),
					_remove_MapChanged_1 = (void*)Marshal.GetFunctionPointerForDelegate(IObservableMap<K, V>.Vftbl.DelegateCache[1] = new IObservableMap_Delegates.remove_MapChanged_1(IObservableMap<K, V>.Vftbl.Do_Abi_remove_MapChanged_1))
				};
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(IObservableMap<K, V>.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 2));
				Marshal.StructureToPtr<IInspectable.Vftbl>(IObservableMap<K, V>.Vftbl.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
				ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IObservableMap<K, V>.Vftbl.AbiToProjectionVftable._add_MapChanged_0;
				ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IObservableMap<K, V>.Vftbl.AbiToProjectionVftable._remove_MapChanged_1;
				IObservableMap<K, V>.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06001739 RID: 5945 RVA: 0x00041726 File Offset: 0x0003F926
			private static ConditionalWeakTable<Windows.Foundation.Collections.IObservableMap<K, V>, EventRegistrationTokenTable<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>> MakeMapChangedTable()
			{
				Interlocked.CompareExchange<ConditionalWeakTable<Windows.Foundation.Collections.IObservableMap<K, V>, EventRegistrationTokenTable<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>>>(ref IObservableMap<K, V>.Vftbl._MapChanged_tokenTables, new ConditionalWeakTable<Windows.Foundation.Collections.IObservableMap<K, V>, EventRegistrationTokenTable<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>>(), null);
				return IObservableMap<K, V>.Vftbl._MapChanged_tokenTables;
			}

			// Token: 0x17000466 RID: 1126
			// (get) Token: 0x0600173A RID: 5946 RVA: 0x00041740 File Offset: 0x0003F940
			private static ConditionalWeakTable<Windows.Foundation.Collections.IObservableMap<K, V>, EventRegistrationTokenTable<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>> _MapChanged_TokenTables
			{
				get
				{
					return IObservableMap<K, V>.Vftbl._MapChanged_tokenTables ?? IObservableMap<K, V>.Vftbl.MakeMapChangedTable();
				}
			}

			// Token: 0x0600173B RID: 5947 RVA: 0x00041754 File Offset: 0x0003F954
			private unsafe static int Do_Abi_add_MapChanged_0(IntPtr thisPtr, IntPtr vhnd, EventRegistrationToken* __return_value__)
			{
				*__return_value__ = default(EventRegistrationToken);
				int result;
				try
				{
					Windows.Foundation.Collections.IObservableMap<K, V> observableMap = ComWrappersSupport.FindObject<Windows.Foundation.Collections.IObservableMap<K, V>>(thisPtr);
					Windows.Foundation.Collections.MapChangedEventHandler<K, V> mapChangedEventHandler = MapChangedEventHandler<K, V>.FromAbi(vhnd);
					*__return_value__ = IObservableMap<K, V>.Vftbl._MapChanged_TokenTables.GetOrCreateValue(observableMap).AddEventHandler(mapChangedEventHandler);
					observableMap.MapChanged += mapChangedEventHandler;
					result = 0;
				}
				catch (Exception ex)
				{
					result = ex.HResult;
				}
				return result;
			}

			// Token: 0x0600173C RID: 5948 RVA: 0x000417B4 File Offset: 0x0003F9B4
			private static int Do_Abi_remove_MapChanged_1(IntPtr thisPtr, EventRegistrationToken token)
			{
				int result;
				try
				{
					Windows.Foundation.Collections.IObservableMap<K, V> observableMap = ComWrappersSupport.FindObject<Windows.Foundation.Collections.IObservableMap<K, V>>(thisPtr);
					EventRegistrationTokenTable<Windows.Foundation.Collections.MapChangedEventHandler<K, V>> eventRegistrationTokenTable;
					Windows.Foundation.Collections.MapChangedEventHandler<K, V> mapChangedEventHandler;
					if (observableMap != null && IObservableMap<K, V>.Vftbl._MapChanged_TokenTables.TryGetValue(observableMap, ref eventRegistrationTokenTable) && eventRegistrationTokenTable.RemoveEventHandler(token, ref mapChangedEventHandler))
					{
						observableMap.MapChanged -= mapChangedEventHandler;
					}
					result = 0;
				}
				catch (Exception ex)
				{
					result = ex.HResult;
				}
				return result;
			}

			// Token: 0x040007DB RID: 2011
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040007DC RID: 2012
			private unsafe void* _add_MapChanged_0;

			// Token: 0x040007DD RID: 2013
			private unsafe void* _remove_MapChanged_1;

			// Token: 0x040007DE RID: 2014
			public static Guid PIID;

			// Token: 0x040007DF RID: 2015
			private static readonly IObservableMap<K, V>.Vftbl AbiToProjectionVftable;

			// Token: 0x040007E0 RID: 2016
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x040007E1 RID: 2017
			private static Delegate[] DelegateCache;

			// Token: 0x040007E2 RID: 2018
			private static volatile ConditionalWeakTable<Windows.Foundation.Collections.IObservableMap<K, V>, EventRegistrationTokenTable<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>> _MapChanged_tokenTables;
		}
	}
}
