using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Collections;
using WinRT;

namespace ABI.System.Collections.Generic
{
	// Token: 0x02000169 RID: 361
	[Guid("02B51929-C1C4-4A7E-8940-0312B5C18500")]
	internal class KeyValuePair<K, V> : IKeyValuePair<K, V>
	{
		// Token: 0x06000BA6 RID: 2982 RVA: 0x000207A9 File Offset: 0x0001E9A9
		public static IObjectReference CreateMarshaler(KeyValuePair<K, V> obj)
		{
			return MarshalInterface<KeyValuePair<K, V>>.CreateMarshaler(obj);
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x000207B4 File Offset: 0x0001E9B4
		public static ObjectReferenceValue CreateMarshaler2(KeyValuePair<K, V> obj)
		{
			return MarshalInterface<KeyValuePair<K, V>>.CreateMarshaler2(obj, default(Guid));
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x000207D0 File Offset: 0x0001E9D0
		public static IntPtr GetAbi(IObjectReference objRef)
		{
			if (objRef == null)
			{
				return IntPtr.Zero;
			}
			return objRef.ThisPtr;
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x000207E4 File Offset: 0x0001E9E4
		public static object CreateRcw(IInspectable obj)
		{
			KeyValuePair<K, V> keyValuePair = new KeyValuePair<K, V>(obj.As<KeyValuePair<K, V>.Vftbl>());
			return new KeyValuePair<K, V>(keyValuePair.Key, keyValuePair.Value);
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x00020814 File Offset: 0x0001EA14
		public static KeyValuePair<K, V> FromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return default(KeyValuePair<K, V>);
			}
			KeyValuePair<K, V> keyValuePair = new KeyValuePair<K, V>(KeyValuePair<K, V>._FromAbi(thisPtr));
			return new KeyValuePair<K, V>(keyValuePair.Key, keyValuePair.Value);
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x00020858 File Offset: 0x0001EA58
		public static IntPtr FromManaged(KeyValuePair<K, V> obj)
		{
			return KeyValuePair<K, V>.CreateMarshaler2(obj).Detach();
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x00020874 File Offset: 0x0001EA74
		internal unsafe static void CopyManaged(KeyValuePair<K, V> o, IntPtr dest)
		{
			*(IntPtr*)dest.ToPointer() = KeyValuePair<K, V>.CreateMarshaler2(o).Detach();
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x00020897 File Offset: 0x0001EA97
		internal static MarshalInterfaceHelper<KeyValuePair<K, V>>.MarshalerArray CreateMarshalerArray(KeyValuePair<K, V>[] array)
		{
			Func<KeyValuePair<K, V>, ObjectReferenceValue> func;
			if ((func = KeyValuePair<K, V>.<>c.<>9__7_0) == null)
			{
				func = (KeyValuePair<K, V>.<>c.<>9__7_0 = new Func<KeyValuePair<K, V>, ObjectReferenceValue>(KeyValuePair<K, V>.<>c.<>9.<CreateMarshalerArray>b__7_0));
			}
			return MarshalInterfaceHelper<KeyValuePair<K, V>>.CreateMarshalerArray2(array, func);
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x000208BE File Offset: 0x0001EABE
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		internal static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<KeyValuePair<K, V>>.GetAbiArray(box);
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x000208C6 File Offset: 0x0001EAC6
		internal static KeyValuePair<K, V>[] FromAbiArray(object box)
		{
			Func<IntPtr, KeyValuePair<K, V>> func;
			if ((func = KeyValuePair<K, V>.<>c.<>9__9_0) == null)
			{
				func = (KeyValuePair<K, V>.<>c.<>9__9_0 = new Func<IntPtr, KeyValuePair<K, V>>(KeyValuePair<K, V>.<>c.<>9.<FromAbiArray>b__9_0));
			}
			return MarshalInterfaceHelper<KeyValuePair<K, V>>.FromAbiArray(box, func);
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x000208ED File Offset: 0x0001EAED
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(KeyValuePair<K, V>[] array)
		{
			Func<KeyValuePair<K, V>, IntPtr> func;
			if ((func = KeyValuePair<K, V>.<>c.<>9__10_0) == null)
			{
				func = (KeyValuePair<K, V>.<>c.<>9__10_0 = new Func<KeyValuePair<K, V>, IntPtr>(KeyValuePair<K, V>.<>c.<>9.<FromManagedArray>b__10_0));
			}
			return MarshalInterfaceHelper<KeyValuePair<K, V>>.FromManagedArray(array, func);
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x00020914 File Offset: 0x0001EB14
		internal static void CopyManagedArray(KeyValuePair<K, V>[] array, IntPtr data)
		{
			Action<KeyValuePair<K, V>, IntPtr> action;
			if ((action = KeyValuePair<K, V>.<>c.<>9__11_0) == null)
			{
				action = (KeyValuePair<K, V>.<>c.<>9__11_0 = new Action<KeyValuePair<K, V>, IntPtr>(KeyValuePair<K, V>.<>c.<>9.<CopyManagedArray>b__11_0));
			}
			MarshalInterfaceHelper<KeyValuePair<K, V>>.CopyManagedArray(array, data, action);
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0002093C File Offset: 0x0001EB3C
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInterfaceHelper<IKeyValuePair<K, V>>.DisposeMarshaler(value);
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00020944 File Offset: 0x0001EB44
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<IKeyValuePair<K, V>>.DisposeAbi(abi);
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0002094C File Offset: 0x0001EB4C
		public static string GetGuidSignature()
		{
			return GuidGenerator.GetSignature(typeof(KeyValuePair<K, V>));
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x00020960 File Offset: 0x0001EB60
		public static ObjectReference<KeyValuePair<K, V>.Vftbl> _FromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			KeyValuePair<K, V>.Vftbl vftbl = new KeyValuePair<K, V>.Vftbl(thisPtr);
			return ObjectReference<KeyValuePair<K, V>.Vftbl>.FromAbi(thisPtr, vftbl);
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0002098B File Offset: 0x0001EB8B
		public static implicit operator KeyValuePair<K, V>(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new KeyValuePair<K, V>(obj);
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00020998 File Offset: 0x0001EB98
		public static implicit operator KeyValuePair<K, V>(ObjectReference<KeyValuePair<K, V>.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new KeyValuePair<K, V>(obj);
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x000209A5 File Offset: 0x0001EBA5
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x000209AD File Offset: 0x0001EBAD
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x000209BA File Offset: 0x0001EBBA
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x000209C7 File Offset: 0x0001EBC7
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x000209D4 File Offset: 0x0001EBD4
		public KeyValuePair(IObjectReference obj) : this(obj.As<KeyValuePair<K, V>.Vftbl>())
		{
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x000209E2 File Offset: 0x0001EBE2
		public KeyValuePair(ObjectReference<KeyValuePair<K, V>.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x000209F4 File Offset: 0x0001EBF4
		public K Key
		{
			get
			{
				object[] array = new object[2];
				array[0] = this.ThisPtr;
				object[] array2 = array;
				K result;
				try
				{
					this._obj.Vftbl.get_Key_0.DynamicInvokeAbi(array2);
					result = Marshaler<K>.FromAbi.Invoke(array2[1]);
				}
				finally
				{
					Marshaler<K>.DisposeAbi.Invoke(array2[1]);
				}
				return result;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x00020A5C File Offset: 0x0001EC5C
		public V Value
		{
			get
			{
				object[] array = new object[2];
				array[0] = this.ThisPtr;
				object[] array2 = array;
				V result;
				try
				{
					this._obj.Vftbl.get_Value_1.DynamicInvokeAbi(array2);
					result = Marshaler<V>.FromAbi.Invoke(array2[1]);
				}
				finally
				{
					Marshaler<V>.DisposeAbi.Invoke(array2[1]);
				}
				return result;
			}
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00020AC4 File Offset: 0x0001ECC4
		// Note: this type is marked as 'beforefieldinit'.
		static KeyValuePair()
		{
			KeyValuePair<K, V>.PIID = KeyValuePair<K, V>.Vftbl.PIID;
		}

		// Token: 0x040001F5 RID: 501
		public static Guid PIID;

		// Token: 0x040001F6 RID: 502
		protected readonly ObjectReference<KeyValuePair<K, V>.Vftbl> _obj;

		// Token: 0x020003A0 RID: 928
		internal sealed class ToIKeyValuePair : IKeyValuePair<K, V>
		{
			// Token: 0x06001A00 RID: 6656 RVA: 0x0004814C File Offset: 0x0004634C
			public ToIKeyValuePair([In] ref KeyValuePair<K, V> pair)
			{
				this._pair = pair;
			}

			// Token: 0x170004E2 RID: 1250
			// (get) Token: 0x06001A01 RID: 6657 RVA: 0x00048160 File Offset: 0x00046360
			public K Key
			{
				get
				{
					return this._pair.Key;
				}
			}

			// Token: 0x170004E3 RID: 1251
			// (get) Token: 0x06001A02 RID: 6658 RVA: 0x0004816D File Offset: 0x0004636D
			public V Value
			{
				get
				{
					return this._pair.Value;
				}
			}

			// Token: 0x04000939 RID: 2361
			private readonly KeyValuePair<K, V> _pair;
		}

		// Token: 0x020003A1 RID: 929
		internal struct Enumerator : IEnumerator<IKeyValuePair<K, V>>, IEnumerator, IDisposable
		{
			// Token: 0x06001A03 RID: 6659 RVA: 0x0004817A File Offset: 0x0004637A
			internal Enumerator(IEnumerator<KeyValuePair<K, V>> enumerator)
			{
				this._enum = enumerator;
			}

			// Token: 0x06001A04 RID: 6660 RVA: 0x00048183 File Offset: 0x00046383
			public bool MoveNext()
			{
				return this._enum.MoveNext();
			}

			// Token: 0x170004E4 RID: 1252
			// (get) Token: 0x06001A05 RID: 6661 RVA: 0x00048190 File Offset: 0x00046390
			public IKeyValuePair<K, V> Current
			{
				get
				{
					KeyValuePair<K, V> keyValuePair = this._enum.Current;
					return new KeyValuePair<K, V>.ToIKeyValuePair(ref keyValuePair);
				}
			}

			// Token: 0x170004E5 RID: 1253
			// (get) Token: 0x06001A06 RID: 6662 RVA: 0x000481B0 File Offset: 0x000463B0
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06001A07 RID: 6663 RVA: 0x000481B8 File Offset: 0x000463B8
			void IEnumerator.Reset()
			{
				this._enum.Reset();
			}

			// Token: 0x06001A08 RID: 6664 RVA: 0x000481C5 File Offset: 0x000463C5
			public void Dispose()
			{
			}

			// Token: 0x0400093A RID: 2362
			private readonly IEnumerator<KeyValuePair<K, V>> _enum;
		}

		// Token: 0x020003A2 RID: 930
		[Guid("02B51929-C1C4-4A7E-8940-0312B5C18500")]
		public struct Vftbl
		{
			// Token: 0x06001A09 RID: 6665 RVA: 0x000481C8 File Offset: 0x000463C8
			internal unsafe Vftbl(IntPtr thisPtr)
			{
				VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
				IntPtr* ptr = (IntPtr*)((void*)vftblPtr.Vftbl);
				this.IInspectableVftbl = Marshal.PtrToStructure<IInspectable.Vftbl>(vftblPtr.Vftbl);
				this.get_Key_0 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], KeyValuePair<K, V>.Vftbl.get_Key_0_Type);
				this.get_Value_1 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], KeyValuePair<K, V>.Vftbl.get_Value_1_Type);
			}

			// Token: 0x06001A0A RID: 6666 RVA: 0x00048234 File Offset: 0x00046434
			unsafe static Vftbl()
			{
				KeyValuePair<K, V>.Vftbl.PIID = GuidGenerator.CreateIID(typeof(KeyValuePair<K, V>));
				KeyValuePair<K, V>.Vftbl.get_Key_0_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					Marshaler<K>.AbiType.MakeByRefType(),
					typeof(int)
				});
				KeyValuePair<K, V>.Vftbl.get_Value_1_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					Marshaler<V>.AbiType.MakeByRefType(),
					typeof(int)
				});
				KeyValuePair<K, V>.Vftbl._adapterTable = new ConditionalWeakTable<object, KeyValuePair<K, V>.ToIKeyValuePair>();
				KeyValuePair<K, V>.Vftbl.AbiToProjectionVftable = new KeyValuePair<K, V>.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					get_Key_0 = Delegate.CreateDelegate(KeyValuePair<K, V>.Vftbl.get_Key_0_Type, typeof(KeyValuePair<K, V>.Vftbl).GetMethod("Do_Abi_get_Key_0", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<K>.AbiType
					})),
					get_Value_1 = Delegate.CreateDelegate(KeyValuePair<K, V>.Vftbl.get_Value_1_Type, typeof(KeyValuePair<K, V>.Vftbl).GetMethod("Do_Abi_get_Value_1", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<V>.AbiType
					}))
				};
				IntPtr* ptr = (IntPtr*)((void*)Marshal.AllocCoTaskMem(Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 2));
				Marshal.StructureToPtr<IInspectable.Vftbl>(KeyValuePair<K, V>.Vftbl.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
				ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(KeyValuePair<K, V>.Vftbl.AbiToProjectionVftable.get_Key_0);
				ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(KeyValuePair<K, V>.Vftbl.AbiToProjectionVftable.get_Value_1);
				KeyValuePair<K, V>.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06001A0B RID: 6667 RVA: 0x000483CC File Offset: 0x000465CC
			private static KeyValuePair<K, V>.ToIKeyValuePair FindAdapter(IntPtr thisPtr)
			{
				KeyValuePair<K, V>.Vftbl.<>c__DisplayClass11_0 CS$<>8__locals1 = new KeyValuePair<K, V>.Vftbl.<>c__DisplayClass11_0();
				CS$<>8__locals1.__this = (KeyValuePair<K, V>)ComWrappersSupport.FindObject<object>(thisPtr);
				return KeyValuePair<K, V>.Vftbl._adapterTable.GetValue(CS$<>8__locals1.__this, new ConditionalWeakTable<object, KeyValuePair<K, V>.ToIKeyValuePair>.CreateValueCallback(CS$<>8__locals1.<FindAdapter>b__0));
			}

			// Token: 0x06001A0C RID: 6668 RVA: 0x00048414 File Offset: 0x00046614
			private unsafe static int Do_Abi_get_Key_0<KAbi>(void* thisPtr, out KAbi __return_value__)
			{
				K k = default(K);
				__return_value__ = default(KAbi);
				try
				{
					k = KeyValuePair<K, V>.Vftbl.FindAdapter(new IntPtr(thisPtr)).Key;
					__return_value__ = (KAbi)((object)Marshaler<K>.FromManaged.Invoke(k));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001A0D RID: 6669 RVA: 0x0004847C File Offset: 0x0004667C
			private unsafe static int Do_Abi_get_Value_1<VAbi>(void* thisPtr, out VAbi __return_value__)
			{
				V v = default(V);
				__return_value__ = default(VAbi);
				try
				{
					v = KeyValuePair<K, V>.Vftbl.FindAdapter(new IntPtr(thisPtr)).Value;
					__return_value__ = (VAbi)((object)Marshaler<V>.FromManaged.Invoke(v));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0400093B RID: 2363
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400093C RID: 2364
			public Delegate get_Key_0;

			// Token: 0x0400093D RID: 2365
			public Delegate get_Value_1;

			// Token: 0x0400093E RID: 2366
			public static Guid PIID;

			// Token: 0x0400093F RID: 2367
			private static readonly Type get_Key_0_Type;

			// Token: 0x04000940 RID: 2368
			private static readonly Type get_Value_1_Type;

			// Token: 0x04000941 RID: 2369
			private static readonly KeyValuePair<K, V>.Vftbl AbiToProjectionVftable;

			// Token: 0x04000942 RID: 2370
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x04000943 RID: 2371
			private static ConditionalWeakTable<object, KeyValuePair<K, V>.ToIKeyValuePair> _adapterTable;
		}
	}
}
