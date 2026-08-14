using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Collections;
using WinRT;
using WinRT.Interop;

namespace ABI.System.Collections.Generic
{
	// Token: 0x02000158 RID: 344
	[DynamicInterfaceCastableImplementation]
	[Guid("3C2925FE-8519-45C1-AA79-197B6718C1C1")]
	internal interface IDictionary<K, V> : IDictionary<!0, !1>, ICollection<KeyValuePair<!0, !1>>, IEnumerable<KeyValuePair<!0, !1>>, IEnumerable
	{
		// Token: 0x06000AFD RID: 2813 RVA: 0x0001E9FE File Offset: 0x0001CBFE
		public static IObjectReference CreateMarshaler(IDictionary<K, V> obj)
		{
			if (obj != null)
			{
				return ComWrappersSupport.CreateCCWForObject<IDictionary<K, V>.Vftbl>(obj, IDictionary<K, V>.PIID);
			}
			return null;
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0001EA10 File Offset: 0x0001CC10
		public static ObjectReferenceValue CreateMarshaler2(IDictionary<K, V> obj)
		{
			return ComWrappersSupport.CreateCCWForObjectForMarshaling(obj, IDictionary<K, V>.PIID);
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0001EA1D File Offset: 0x0001CC1D
		public static IntPtr GetAbi(IObjectReference objRef)
		{
			if (objRef == null)
			{
				return IntPtr.Zero;
			}
			return objRef.ThisPtr;
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x0001EA2E File Offset: 0x0001CC2E
		public static IDictionary<K, V> FromAbi(IntPtr thisPtr)
		{
			if (!(thisPtr == IntPtr.Zero))
			{
				return (IDictionary<!0, !1>)new IInspectable(IDictionary<K, V>.ObjRefFromAbi(thisPtr));
			}
			return null;
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0001EA50 File Offset: 0x0001CC50
		public static IntPtr FromManaged(IDictionary<K, V> value)
		{
			if (value != null)
			{
				return IDictionary<K, V>.CreateMarshaler2(value).Detach();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x0001EA74 File Offset: 0x0001CC74
		public static void DisposeMarshaler(IObjectReference objRef)
		{
			if (objRef != null)
			{
				objRef.Dispose();
			}
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0001EA7F File Offset: 0x0001CC7F
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<IMap<K, V>>.DisposeAbi(abi);
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0001EA87 File Offset: 0x0001CC87
		public static string GetGuidSignature()
		{
			return GuidGenerator.GetSignature(typeof(IDictionary<K, V>));
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0001EA98 File Offset: 0x0001CC98
		public static ObjectReference<IDictionary<K, V>.Vftbl> ObjRefFromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			IDictionary<K, V>.Vftbl vftbl = new IDictionary<K, V>.Vftbl(thisPtr);
			return ObjectReference<IDictionary<K, V>.Vftbl>.FromAbi(thisPtr, vftbl);
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x0001EAC3 File Offset: 0x0001CCC3
		ICollection<K> IDictionary<!0, !1>.Keys
		{
			get
			{
				return IDictionaryMethods<K, V>.get_Keys((ObjectReference<IDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IDictionary<!0, !1>).TypeHandle));
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x0001EAE9 File Offset: 0x0001CCE9
		ICollection<V> IDictionary<!0, !1>.Values
		{
			get
			{
				return IDictionaryMethods<K, V>.get_Values((ObjectReference<IDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IDictionary<!0, !1>).TypeHandle));
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x0001EB0F File Offset: 0x0001CD0F
		int ICollection<KeyValuePair<!0, !1>>.Count
		{
			get
			{
				return IDictionaryMethods<K, V>.get_Count((ObjectReference<IDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IDictionary<!0, !1>).TypeHandle));
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x0001EB35 File Offset: 0x0001CD35
		bool ICollection<KeyValuePair<!0, !1>>.IsReadOnly
		{
			get
			{
				return IDictionaryMethods<K, V>.get_IsReadOnly((ObjectReference<IDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IDictionary<!0, !1>).TypeHandle));
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000B0A RID: 2826 RVA: 0x0001EB5B File Offset: 0x0001CD5B
		// (set) Token: 0x06000B0B RID: 2827 RVA: 0x0001EB8D File Offset: 0x0001CD8D
		V IDictionary<!0, !1>.Item
		{
			get
			{
				return IDictionaryMethods<K, V>.Indexer_Get((ObjectReference<IDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IDictionary<!0, !1>).TypeHandle), IDictionary<K, V>.GetLookupCache((IWinRTObject)this), key);
			}
			set
			{
				IDictionaryMethods<K, V>.Indexer_Set((ObjectReference<IDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IDictionary<!0, !1>).TypeHandle), key, value);
			}
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x0001EBB5 File Offset: 0x0001CDB5
		void Add(K key, V value)
		{
			IDictionaryMethods<K, V>.Add((ObjectReference<IDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IDictionary<!0, !1>).TypeHandle), key, value);
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0001EBDD File Offset: 0x0001CDDD
		bool ContainsKey(K key)
		{
			return IDictionaryMethods<K, V>.ContainsKey((ObjectReference<IDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IDictionary<!0, !1>).TypeHandle), key);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x0001EC04 File Offset: 0x0001CE04
		bool Remove(K key)
		{
			return IDictionaryMethods<K, V>.Remove((ObjectReference<IDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IDictionary<!0, !1>).TypeHandle), key);
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x0001EC2B File Offset: 0x0001CE2B
		internal static Dictionary<K, ValueTuple<IntPtr, V>> GetLookupCache(IWinRTObject _this)
		{
			RuntimeTypeHandle typeHandle = typeof(IDictionary<!0, !1>).TypeHandle;
			Func<object> helperDataFactory;
			if ((helperDataFactory = IDictionary<K, V>.<>c.<>9__26_0) == null)
			{
				helperDataFactory = (IDictionary<K, V>.<>c.<>9__26_0 = new Func<object>(IDictionary<K, V>.<>c.<>9.<GetLookupCache>b__26_0));
			}
			return (Dictionary<K, ValueTuple<IntPtr, V>>)_this.GetOrCreateTypeHelperData(typeHandle, helperDataFactory);
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0001EC66 File Offset: 0x0001CE66
		bool TryGetValue(K key, out V value)
		{
			return IDictionaryMethods<K, V>.TryGetValue((ObjectReference<IDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IDictionary<!0, !1>).TypeHandle), IDictionary<K, V>.GetLookupCache((IWinRTObject)this), key, ref value);
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x0001EC99 File Offset: 0x0001CE99
		void Add(KeyValuePair<K, V> item)
		{
			IDictionaryMethods<K, V>.Add((ObjectReference<IDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IDictionary<!0, !1>).TypeHandle), item);
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0001ECC0 File Offset: 0x0001CEC0
		bool Contains(KeyValuePair<K, V> item)
		{
			return IDictionaryMethods<K, V>.Contains((ObjectReference<IDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IDictionary<!0, !1>).TypeHandle), IDictionary<K, V>.GetLookupCache((IWinRTObject)this), item);
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x0001ECF4 File Offset: 0x0001CEF4
		void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
		{
			IObjectReference objectReference = (ObjectReference<IDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IDictionary<!0, !1>).TypeHandle);
			((IWinRTObject)this).IsInterfaceImplemented(typeof(IEnumerable<KeyValuePair<K, V>>).TypeHandle, true);
			ObjectReference<IDictionary<K, V>.Vftbl> objectReference2 = (ObjectReference<IDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IEnumerable<KeyValuePair<K, V>>).TypeHandle);
			IDictionaryMethods<K, V>.CopyTo(objectReference, objectReference2, array, arrayIndex);
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x0001ED64 File Offset: 0x0001CF64
		bool Remove(KeyValuePair<K, V> item)
		{
			return IDictionaryMethods<K, V>.Remove((ObjectReference<IDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IDictionary<!0, !1>).TypeHandle), item);
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0001ED8C File Offset: 0x0001CF8C
		IEnumerator<KeyValuePair<K, V>> GetEnumerator()
		{
			((IWinRTObject)this).IsInterfaceImplemented(typeof(IEnumerable<KeyValuePair<K, V>>).TypeHandle, true);
			return IEnumerableMethods<KeyValuePair<K, V>>.GetEnumerator((ObjectReference<IDictionary<K, V>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IEnumerable<KeyValuePair<K, V>>).TypeHandle));
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x0001EDD9 File Offset: 0x0001CFD9
		IEnumerator GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0001EDE1 File Offset: 0x0001CFE1
		static IDictionary()
		{
			IDictionary<K, V>.PIID = IDictionary<K, V>.Vftbl.PIID;
		}

		// Token: 0x040001ED RID: 493
		public static Guid PIID;

		// Token: 0x02000381 RID: 897
		public sealed class ToAbiHelper : IMap<K, V>, IIterable<IKeyValuePair<K, V>>
		{
			// Token: 0x06001930 RID: 6448 RVA: 0x000454DD File Offset: 0x000436DD
			public ToAbiHelper(IDictionary<K, V> dictionary)
			{
				this._dictionary = dictionary;
			}

			// Token: 0x06001931 RID: 6449 RVA: 0x000454EC File Offset: 0x000436EC
			IEnumerator<IKeyValuePair<K, V>> IIterable<IKeyValuePair<!0, !1>>.First()
			{
				return new KeyValuePair<K, V>.Enumerator(this._dictionary.GetEnumerator());
			}

			// Token: 0x06001932 RID: 6450 RVA: 0x00045504 File Offset: 0x00043704
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

			// Token: 0x170004C7 RID: 1223
			// (get) Token: 0x06001933 RID: 6451 RVA: 0x0004554A File Offset: 0x0004374A
			public uint Size
			{
				get
				{
					return (uint)this._dictionary.Count;
				}
			}

			// Token: 0x06001934 RID: 6452 RVA: 0x00045557 File Offset: 0x00043757
			public bool HasKey(K key)
			{
				return this._dictionary.ContainsKey(key);
			}

			// Token: 0x06001935 RID: 6453 RVA: 0x00045568 File Offset: 0x00043768
			IReadOnlyDictionary<K, V> IMap<!0, !1>.GetView()
			{
				IReadOnlyDictionary<K, V> readOnlyDictionary = this._dictionary as IReadOnlyDictionary<K, V>;
				if (readOnlyDictionary == null)
				{
					readOnlyDictionary = new ReadOnlyDictionary<K, V>(this._dictionary);
				}
				return readOnlyDictionary;
			}

			// Token: 0x06001936 RID: 6454 RVA: 0x00045591 File Offset: 0x00043791
			public bool Insert(K key, V value)
			{
				bool result = this._dictionary.ContainsKey(key);
				this._dictionary[key] = value;
				return result;
			}

			// Token: 0x06001937 RID: 6455 RVA: 0x000455AC File Offset: 0x000437AC
			public void _Remove(K key)
			{
				if (!this._dictionary.Remove(key))
				{
					KeyNotFoundException ex = new KeyNotFoundException(string.Format(WinRTRuntimeErrorStrings.Arg_KeyNotFoundWithKey, key.ToString()));
					ex.SetHResult(-2147483637);
					throw ex;
				}
			}

			// Token: 0x06001938 RID: 6456 RVA: 0x000455E4 File Offset: 0x000437E4
			public void Clear()
			{
				this._dictionary.Clear();
			}

			// Token: 0x040008D4 RID: 2260
			private readonly IDictionary<K, V> _dictionary;
		}

		// Token: 0x02000382 RID: 898
		[Guid("3C2925FE-8519-45C1-AA79-197B6718C1C1")]
		public struct Vftbl
		{
			// Token: 0x170004C8 RID: 1224
			// (get) Token: 0x06001939 RID: 6457 RVA: 0x000455F1 File Offset: 0x000437F1
			// (set) Token: 0x0600193A RID: 6458 RVA: 0x000455F9 File Offset: 0x000437F9
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

			// Token: 0x170004C9 RID: 1225
			// (get) Token: 0x0600193B RID: 6459 RVA: 0x00045602 File Offset: 0x00043802
			// (set) Token: 0x0600193C RID: 6460 RVA: 0x0004560A File Offset: 0x0004380A
			public method GetView_3
			{
				get
				{
					return this._getView_3;
				}
				set
				{
					this._getView_3 = value;
				}
			}

			// Token: 0x170004CA RID: 1226
			// (get) Token: 0x0600193D RID: 6461 RVA: 0x00045613 File Offset: 0x00043813
			// (set) Token: 0x0600193E RID: 6462 RVA: 0x0004561B File Offset: 0x0004381B
			public method Clear_6
			{
				get
				{
					return this._clear_6;
				}
				set
				{
					this._clear_6 = value;
				}
			}

			// Token: 0x0600193F RID: 6463 RVA: 0x00045624 File Offset: 0x00043824
			internal unsafe Vftbl(IntPtr thisPtr)
			{
				this = default(IDictionary<K, V>.Vftbl);
				VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
				IntPtr* ptr = (IntPtr*)((void*)vftblPtr.Vftbl);
				this.IInspectableVftbl = Marshal.PtrToStructure<IInspectable.Vftbl>(vftblPtr.Vftbl);
				this.Lookup_0 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IDictionary<K, V>.Vftbl.Lookup_0_Type);
				this.GetSize_1 = (void*)ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.HasKey_2 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IDictionary<K, V>.Vftbl.HasKey_2_Type);
				this.GetView_3 = (void*)ptr[(IntPtr)9 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.Insert_4 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)10 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IDictionary<K, V>.Vftbl.Insert_4_Type);
				this.Remove_5 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)11 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IDictionary<K, V>.Vftbl.Remove_5_Type);
				this.Clear_6 = (void*)ptr[(IntPtr)12 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
			}

			// Token: 0x06001940 RID: 6464 RVA: 0x00045718 File Offset: 0x00043918
			unsafe static Vftbl()
			{
				IDictionary<K, V>.Vftbl.PIID = GuidGenerator.CreateIID(typeof(IDictionary<K, V>));
				IDictionary<K, V>.Vftbl.Lookup_0_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					Marshaler<K>.AbiType,
					Marshaler<V>.AbiType.MakeByRefType(),
					typeof(int)
				});
				IDictionary<K, V>.Vftbl.HasKey_2_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					Marshaler<K>.AbiType,
					typeof(byte).MakeByRefType(),
					typeof(int)
				});
				IDictionary<K, V>.Vftbl.Insert_4_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					Marshaler<K>.AbiType,
					Marshaler<V>.AbiType,
					typeof(byte).MakeByRefType(),
					typeof(int)
				});
				IDictionary<K, V>.Vftbl.Remove_5_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					Marshaler<K>.AbiType,
					typeof(int)
				});
				IDictionary<K, V>.Vftbl.DelegateCache = new Delegate[3];
				IDictionary<K, V>.Vftbl._adapterTable = new ConditionalWeakTable<IDictionary<K, V>, IDictionary<K, V>.ToAbiHelper>();
				IDictionary<K, V>.Vftbl.AbiToProjectionVftable = new IDictionary<K, V>.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					Lookup_0 = Delegate.CreateDelegate(IDictionary<K, V>.Vftbl.Lookup_0_Type, typeof(IDictionary<K, V>.Vftbl).GetMethod("Do_Abi_Lookup_0", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<K>.AbiType,
						Marshaler<V>.AbiType
					})),
					_get_Size_1 = (void*)Marshal.GetFunctionPointerForDelegate(IDictionary<K, V>.Vftbl.DelegateCache[0] = new _get_PropertyAsUInt32(IDictionary<K, V>.Vftbl.Do_Abi_get_Size_1)),
					HasKey_2 = Delegate.CreateDelegate(IDictionary<K, V>.Vftbl.HasKey_2_Type, typeof(IDictionary<K, V>.Vftbl).GetMethod("Do_Abi_HasKey_2", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<K>.AbiType
					})),
					_getView_3 = (void*)Marshal.GetFunctionPointerForDelegate(IDictionary<K, V>.Vftbl.DelegateCache[1] = new IDictionary_Delegates.GetView_3(IDictionary<K, V>.Vftbl.Do_Abi_GetView_3)),
					Insert_4 = Delegate.CreateDelegate(IDictionary<K, V>.Vftbl.Insert_4_Type, typeof(IDictionary<K, V>.Vftbl).GetMethod("Do_Abi_Insert_4", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<K>.AbiType,
						Marshaler<V>.AbiType
					})),
					Remove_5 = Delegate.CreateDelegate(IDictionary<K, V>.Vftbl.Remove_5_Type, typeof(IDictionary<K, V>.Vftbl).GetMethod("Do_Abi_Remove_5", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<K>.AbiType
					})),
					_clear_6 = (void*)Marshal.GetFunctionPointerForDelegate(IDictionary<K, V>.Vftbl.DelegateCache[2] = new IDictionary_Delegates.Clear_6(IDictionary<K, V>.Vftbl.Do_Abi_Clear_6))
				};
				IntPtr* ptr = (IntPtr*)((void*)Marshal.AllocCoTaskMem(Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 7));
				Marshal.StructureToPtr<IInspectable.Vftbl>(IDictionary<K, V>.Vftbl.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
				ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IDictionary<K, V>.Vftbl.AbiToProjectionVftable.Lookup_0);
				ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IDictionary<K, V>.Vftbl.AbiToProjectionVftable._get_Size_1;
				ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IDictionary<K, V>.Vftbl.AbiToProjectionVftable.HasKey_2);
				ptr[(IntPtr)9 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IDictionary<K, V>.Vftbl.AbiToProjectionVftable.GetView_3;
				ptr[(IntPtr)10 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IDictionary<K, V>.Vftbl.AbiToProjectionVftable.Insert_4);
				ptr[(IntPtr)11 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IDictionary<K, V>.Vftbl.AbiToProjectionVftable.Remove_5);
				ptr[(IntPtr)12 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IDictionary<K, V>.Vftbl.AbiToProjectionVftable.Clear_6;
				IDictionary<K, V>.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06001941 RID: 6465 RVA: 0x00045AD4 File Offset: 0x00043CD4
			private static IMap<K, V> FindAdapter(IntPtr thisPtr)
			{
				IDictionary<K, V> dictionary = ComWrappersSupport.FindObject<IDictionary<K, V>>(thisPtr);
				ConditionalWeakTable<IDictionary<K, V>, IDictionary<K, V>.ToAbiHelper> adapterTable = IDictionary<K, V>.Vftbl._adapterTable;
				IDictionary<K, V> dictionary2 = dictionary;
				ConditionalWeakTable<IDictionary<K, V>, IDictionary<K, V>.ToAbiHelper>.CreateValueCallback createValueCallback;
				if ((createValueCallback = IDictionary<K, V>.Vftbl.<>c.<>9__28_0) == null)
				{
					createValueCallback = (IDictionary<K, V>.Vftbl.<>c.<>9__28_0 = new ConditionalWeakTable<IDictionary<K, V>, IDictionary<K, V>.ToAbiHelper>.CreateValueCallback(IDictionary<K, V>.Vftbl.<>c.<>9.<FindAdapter>b__28_0));
				}
				return adapterTable.GetValue(dictionary2, createValueCallback);
			}

			// Token: 0x06001942 RID: 6466 RVA: 0x00045B14 File Offset: 0x00043D14
			private unsafe static int Do_Abi_Lookup_0<KAbi, VAbi>(void* thisPtr, KAbi key, out VAbi __return_value__)
			{
				V v = default(V);
				__return_value__ = default(VAbi);
				try
				{
					v = IDictionary<K, V>.Vftbl.FindAdapter(new IntPtr(thisPtr)).Lookup(Marshaler<K>.FromAbi.Invoke(key));
					__return_value__ = (VAbi)((object)Marshaler<V>.FromManaged.Invoke(v));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001943 RID: 6467 RVA: 0x00045B8C File Offset: 0x00043D8C
			private unsafe static int Do_Abi_HasKey_2<KAbi>(void* thisPtr, KAbi key, out byte __return_value__)
			{
				__return_value__ = 0;
				try
				{
					__return_value__ = (IDictionary<K, V>.Vftbl.FindAdapter(new IntPtr(thisPtr)).HasKey(Marshaler<K>.FromAbi.Invoke(key)) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001944 RID: 6468 RVA: 0x00045BEC File Offset: 0x00043DEC
			private static int Do_Abi_GetView_3(IntPtr thisPtr, out IntPtr __return_value__)
			{
				__return_value__ = default(IntPtr);
				try
				{
					IReadOnlyDictionary<K, V> view = IDictionary<K, V>.Vftbl.FindAdapter(thisPtr).GetView();
					__return_value__ = MarshalInterface<IReadOnlyDictionary<K, V>>.FromManaged(view);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001945 RID: 6469 RVA: 0x00045C3C File Offset: 0x00043E3C
			private unsafe static int Do_Abi_Insert_4<KAbi, VAbi>(void* thisPtr, KAbi key, VAbi value, out byte __return_value__)
			{
				__return_value__ = 0;
				try
				{
					__return_value__ = (IDictionary<K, V>.Vftbl.FindAdapter(new IntPtr(thisPtr)).Insert(Marshaler<K>.FromAbi.Invoke(key), Marshaler<V>.FromAbi.Invoke(value)) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001946 RID: 6470 RVA: 0x00045CAC File Offset: 0x00043EAC
			private unsafe static int Do_Abi_Remove_5<KAbi>(void* thisPtr, KAbi key)
			{
				try
				{
					IDictionary<K, V>.Vftbl.FindAdapter(new IntPtr(thisPtr))._Remove(Marshaler<K>.FromAbi.Invoke(key));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001947 RID: 6471 RVA: 0x00045CFC File Offset: 0x00043EFC
			private static int Do_Abi_Clear_6(IntPtr thisPtr)
			{
				try
				{
					IDictionary<K, V>.Vftbl.FindAdapter(thisPtr).Clear();
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001948 RID: 6472 RVA: 0x00045D38 File Offset: 0x00043F38
			private static int Do_Abi_get_Size_1(IntPtr thisPtr, out uint __return_value__)
			{
				__return_value__ = 0U;
				try
				{
					uint size = IDictionary<K, V>.Vftbl.FindAdapter(thisPtr).Size;
					__return_value__ = size;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040008D5 RID: 2261
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040008D6 RID: 2262
			public Delegate Lookup_0;

			// Token: 0x040008D7 RID: 2263
			private unsafe void* _get_Size_1;

			// Token: 0x040008D8 RID: 2264
			public Delegate HasKey_2;

			// Token: 0x040008D9 RID: 2265
			private unsafe void* _getView_3;

			// Token: 0x040008DA RID: 2266
			public Delegate Insert_4;

			// Token: 0x040008DB RID: 2267
			public Delegate Remove_5;

			// Token: 0x040008DC RID: 2268
			private unsafe void* _clear_6;

			// Token: 0x040008DD RID: 2269
			public static Guid PIID;

			// Token: 0x040008DE RID: 2270
			private static readonly Type Lookup_0_Type;

			// Token: 0x040008DF RID: 2271
			private static readonly Type HasKey_2_Type;

			// Token: 0x040008E0 RID: 2272
			private static readonly Type Insert_4_Type;

			// Token: 0x040008E1 RID: 2273
			private static readonly Type Remove_5_Type;

			// Token: 0x040008E2 RID: 2274
			private static readonly IDictionary<K, V>.Vftbl AbiToProjectionVftable;

			// Token: 0x040008E3 RID: 2275
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x040008E4 RID: 2276
			private static readonly Delegate[] DelegateCache;

			// Token: 0x040008E5 RID: 2277
			private static ConditionalWeakTable<IDictionary<K, V>, IDictionary<K, V>.ToAbiHelper> _adapterTable;
		}
	}
}
